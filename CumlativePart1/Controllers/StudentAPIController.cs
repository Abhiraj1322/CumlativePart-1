using CumlativePart1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
namespace CumlativePart1.Controllers;



[Route("api/Student")]
        [ApiController]
        public class StudentAPIController : ControllerBase
        {
            private readonly SchooldbContext _context;
            public StudentAPIController(SchooldbContext context)
            {
                _context = context;
            }

         

            [HttpGet]
            [Route(template: "listStudents")]
            public List<Student> ListStudent()
            {
                List<Student> Students = new List<Student>();
                using (MySqlConnection Connection = _context.AccessDatabase())
                {
                    Connection.Open();
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "Select * from students";
                    Command.Prepare();
                    using (MySqlDataReader ResultSet = Command.ExecuteReader())
                    {
                        while (ResultSet.Read())
                        {
                            int id = Convert.ToInt32(ResultSet["studentid"]);
                            string FirstName = ResultSet["studentfname"].ToString();
                            string LastName = ResultSet["studentlname"].ToString();
                            string StudentNumber = ResultSet["studentnumber"].ToString();
                            DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);
                            Student CurrentStudent = new Student()
                            {
                                StudentId = id,
                                StudentFName = FirstName,
                                StudentLName = LastName,
                                EnrollDate = EnrolDate,
                                StudentNumber = StudentNumber,

                            };
                            Students.Add(CurrentStudent);
                        }
                    }

                }
                return Students;
            }


            [HttpGet]
            [Route(template: "FindStudent/{id}")]
            public Student FindStudent(int id)
            {
                Student SelectedStudents = new Student();

                using (MySqlConnection Connection = _context.AccessDatabase())
                {
                    Connection.Open();
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "Select * from students WHERE studentid = @id";
                    Command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader ResultSet = Command.ExecuteReader())
                    {
                        while (ResultSet.Read())
                        {
                            int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                            string FirstName = ResultSet["studentfname"].ToString();
                            string LastName = ResultSet["studentlname"].ToString();
                            string StudentNumber = ResultSet["studentnumber"].ToString();
                            DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                            SelectedStudents.StudentId = StudentId;
                            SelectedStudents.StudentFName = FirstName;
                            SelectedStudents.StudentLName = LastName;
                            SelectedStudents.EnrollDate = EnrolDate;
                            SelectedStudents.StudentNumber = StudentNumber;

                        }
                    }
                }
                return SelectedStudents;
            }

        }
