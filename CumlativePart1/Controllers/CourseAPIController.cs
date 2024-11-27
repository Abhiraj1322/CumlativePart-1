using CumlativePart1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CumlativePart1.Controllers
{
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
        // get  a list of all courses from the database.
        /// </summary>
        /// <returns>
        /// a list contains , the course ID, teacher ID, course code, name, start date, and finish date.
        /// </returns>
        /// <remarks>
        /// This method  check the  "courses" table and returns a collection of course objects with relevant details.
        /// </remarks>
        /// <example>

        /// </example>



        [HttpGet]
        [Route(template: "listCourse")]
        public List<Course> ListCourse()
        {
            List<Course> Courses = new List<Course>();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Select * from courses";
                Command.Prepare();
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        Course CurrentCourse = new Course()
                        {
                            courseId = CourseId,
                            teacherid = TeacherId,
                           coursecode = CourseCode,
                            coursename = CourseName,
                            startdate = StartDate,
                            finishdate = FinishDate

                        };
                        Courses.Add(CurrentCourse);
                    }
                }

            }
            return Courses;
        }

    }
}