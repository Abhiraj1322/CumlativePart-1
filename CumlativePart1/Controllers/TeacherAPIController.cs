using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using MySql.Data.MySqlClient;
using CumlativePart1.Models;

namespace CumlativePart1.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;
        public TeacherAPIController(SchooldbContext context)
        {
            _context = context;
        }
        


        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(DateTime? StartDate = null, DateTime? EndDate = null)
        {
            List<Teacher> Teachers = new List<Teacher>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();


                string query = "SELECT * FROM teachers INNER JOIN courses ON teachers.teacherid = courses.teacherid";


                bool hasConditions = false;
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    query += " WHERE hiredate BETWEEN @startDate AND @endDate";
                    Command.Parameters.AddWithValue("@startDate", StartDate.Value);
                    Command.Parameters.AddWithValue("@endDate", EndDate.Value);
                    hasConditions = true;
                }

                Command.CommandText = query;
                Command.Prepare();

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    Dictionary<int, Teacher> teacherDict = new Dictionary<int, Teacher>();

                    while (ResultSet.Read())
                    {
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string TeacherEmpNu = ResultSet["employeenumber"].ToString();
                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        string TeacherSalary = ResultSet["salary"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();

                        if (!teacherDict.ContainsKey(Id))
                        {
                            teacherDict[Id] = new Teacher()
                            {
                                TeacherId = Id,
                                TeacherFName = FirstName,
                                TeacherLName = LastName,
                                TeacherHireDate = TeacherHireDate,
                                TeacherSalary = TeacherSalary,
                                TeacherEmpNu = TeacherEmpNu,
                                CourseNames = new List<string>()
                            };
                        }
                        teacherDict[Id].CourseNames.Add(CourseName);
                    }

                    Teachers.AddRange(teacherDict.Values);
                }
            }

            return Teachers;
        }


       

        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {


            Teacher SelectedTeacher = new Teacher();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT teachers.*, courses.courseName FROM courses INNER JOIN teachers ON teachers.teacherId = courses.teacherId WHERE teachers.teacherId = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string TeacherEmpNu = ResultSet["employeenumber"].ToString();

                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        string TeacherSalary = ResultSet["salary"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        if (SelectedTeacher.TeacherId == 0)
                        {

                            SelectedTeacher.TeacherFName = FirstName;
                            SelectedTeacher.TeacherLName = LastName;
                            SelectedTeacher.TeacherSalary = TeacherSalary;
                            SelectedTeacher.TeacherHireDate = TeacherHireDate;
                            SelectedTeacher.TeacherEmpNu = TeacherEmpNu;
                            SelectedTeacher.CourseNames = new List<string>();
                        }
                        SelectedTeacher.CourseNames.Add(CourseName);
                    }
                }
            }
            return SelectedTeacher;
        }
        /// <summary>
        /// Retrieves a list of courses taught by a specific teacher, identified by their teacher ID.
        /// </summary>
        /// <param name="id">The ID of the teacher whose courses are to be retrieved.</param>
        /// <returns>
        /// A list of course names taught by the specified teacher.
        /// </returns>
        /// <remarks>
        /// This method connects to the database and retrieves the list of courses assigned to the teacher with the given ID.
        /// If no courses are found, an empty list will be returned.
        /// </remarks>
        /// <example>
        /// GET api/Teacher/GetCoursesByTeacher/1 -> ["Web Application Development"]
        /// </example>

        [HttpGet]
        [Route("GetCoursesByTeacher/{id}")]
        public List<string> GetCoursesByTeacher(int id)
        {
            List<string> courses = new List<string>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT CourseName FROM courses WHERE TeacherId = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        string courseName = ResultSet["CourseName"].ToString();
                        courses.Add(courseName);
                    }
                }
            }

            return courses;
        }

    }
}