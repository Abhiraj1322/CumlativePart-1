using CumlativePart1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CumlativePart1.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;

        /// <summary>
        /// Constructor that initializes the StudentAPIController with a database context.
        /// </summary>
        /// <param name="context">The database context used for accessing the database.</param>
        public StudentAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all students from the database.
        /// </summary>
        /// <returns>A list of Student objects containing details such as student ID, first name, last name, student number, and enrollment date.</returns>
        [HttpGet]
        [Route(template: "listStudents")]
        public List<Student> ListStudent()
        {
            // Initialize a list to store the students retrieved from the database
            List<Student> Students = new List<Student>();

            // Open a connection to the database using the context's AccessDatabase method
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection to the MySQL database
                Connection.Open();

                // Create a command to execute the SQL query
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to retrieve all students from the 'students' table
                Command.CommandText = "Select * from students";

                // Prepare the command to prevent SQL injection
                Command.Prepare();

                // Execute the query and read the results
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // Extract data from each column for the current student
                        int id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        // Create a new Student object with the extracted data
                        Student CurrentStudent = new Student()
                        {
                            StudentId = id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            EnrollDate = EnrolDate,
                            StudentNumber = StudentNumber
                        };

                        // Add the student to the list
                        Students.Add(CurrentStudent);
                    }
                }
            }

            // Return the list of students
            return Students;
        }

        /// <summary>
        /// Finds and retrieves a student by their ID from the database.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <returns>A Student object with the details of the student matching the provided ID.</returns>
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            // Create a Student object to store the selected student's data
            Student SelectedStudents = new Student();

            // Open a connection to the database using the context's AccessDatabase method
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection to the MySQL database
                Connection.Open();

                // Create a command to execute the SQL query
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to retrieve a student by ID from the 'students' table
                Command.CommandText = "Select * from students WHERE studentid = @id";

                // Add the student ID parameter to the query
                Command.Parameters.AddWithValue("@id", id);

                // Execute the query and read the results
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // get data from each column for the student with the specified ID
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        // Assign the  data to the SelectedStudents object
                        SelectedStudents.StudentId = StudentId;
                        SelectedStudents.StudentFName = FirstName;
                        SelectedStudents.StudentLName = LastName;
                        SelectedStudents.EnrollDate = EnrolDate;
                        SelectedStudents.StudentNumber = StudentNumber;
                    }
                }
            }

            // Return the selected student
            return SelectedStudents;
        }
    }
}
