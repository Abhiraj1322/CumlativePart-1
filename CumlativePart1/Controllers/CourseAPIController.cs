using CumlativePart1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CumlativePart1.Controllers
{
    // Define the route for the API controller
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;

        public CourseAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// get a list of courses from the database.
        /// </summary>
        /// <returns>A list of Course  with details of as course ID, teacher ID, course code, 
        /// course name, start date, and finish date.</returns>
        [HttpGet]
        [Route(template: "listCourse")]
        public List<Course> ListCourse()
        {
            
            List<Course> Courses = new List<Course>();

            // Open a connection to the databse
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection to the MySQL database
                Connection.Open();

                // Create a command to execute the SQL query
                MySqlCommand Command = Connection.CreateCommand();

                // Define the SQL query to rall records from the 'courses' table
                Command.CommandText = "Select * from courses";

               
                Command.Prepare();

                // Execute the query and read the results
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // Extract data from each column in the current row
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        // Create a new Course object with the extracted data
                        Course CurrentCourse = new Course()
                        {
                            courseId = CourseId,
                            teacherid = TeacherId,
                            coursecode = CourseCode,
                            coursename = CourseName,
                            startdate = StartDate,
                            finishdate = FinishDate
                        };

                        // Add the current course to the list
                        Courses.Add(CurrentCourse);
                    }
                }
            }

           
            return Courses;
        }
    }
}
