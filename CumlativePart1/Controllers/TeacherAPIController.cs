using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using MySql.Data.MySqlClient;
using CumlativePart1.Models;
using System.Collections.Generic;

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

        // This method is used to connect to the database and return a list of teachers
        [HttpGet]
        [Route("ListTeachers")]
        public List<Teacher> ListTeachers(DateTime? StartDate = null, DateTime? EndDate = null)
        {
            // Create a list to hold teacher objects
            List<Teacher> teachers = new List<Teacher>();

            // Open a connection to the database
            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();

                // Create the SQL query
                string query = "SELECT * FROM teachers INNER JOIN courses ON teachers.teacherid = courses.teacherid";

                // Check if we need to filter by a date range
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    query += " WHERE hiredate BETWEEN @startDate AND @endDate";
                }

                // Create a command to run the SQL query
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                // Add the parameters if filtering by date
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    command.Parameters.AddWithValue("@startDate", StartDate.Value);
                    command.Parameters.AddWithValue("@endDate", EndDate.Value);
                }

                // Execute the query and get the result
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Read the data row by row
                    while (reader.Read())
                    {
                        // Map the data to a Teacher object
                        Teacher teacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            TeacherEmpNu = reader["employeenumber"].ToString(),
                            TeacherHireDate = Convert.ToDateTime(reader["hiredate"]),
                            TeacherSalary = reader["salary"].ToString(),
                            // We will store courses as a list of strings
                            CourseNames = new List<string> { reader["coursename"].ToString() }
                        };

                        // Add the teacher to the list
                        teachers.Add(teacher);
                    }
                }
            }

            // Return the list of teachers
            return teachers;
        }

        // This method finds a specific teacher by their ID
        [HttpGet]
        [Route("FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Create a teacher object to store the result
            Teacher selectedTeacher = new Teacher();

            // Open the database connection
            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();

                // SQL query to find teacher by ID
                string query = "SELECT teachers.*, courses.courseName FROM courses INNER JOIN teachers ON teachers.teacherId = courses.teacherId WHERE teachers.teacherId = @id";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                // Add the parameter for the teacher ID
                command.Parameters.AddWithValue("@id", id);

                // Execute the query
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Read the data from the query result
                    while (reader.Read())
                    {
                        // If the teacher is not already set, set the teacher information
                        if (selectedTeacher.TeacherId == 0)
                        {
                            selectedTeacher.TeacherId = Convert.ToInt32(reader["teacherid"]);
                            selectedTeacher.TeacherFName = reader["teacherfname"].ToString();
                            selectedTeacher.TeacherLName = reader["teacherlname"].ToString();
                            selectedTeacher.TeacherEmpNu = reader["employeenumber"].ToString();
                            selectedTeacher.TeacherHireDate = Convert.ToDateTime(reader["hiredate"]);
                            selectedTeacher.TeacherSalary = reader["salary"].ToString();
                            selectedTeacher.CourseNames = new List<string>(); // Initialize the list for course names
                        }

                        // Add the course name to the teacher's list
                        selectedTeacher.CourseNames.Add(reader["coursename"].ToString());
                    }
                }
            }

            // Return the found teacher
            return selectedTeacher;
        }

        // This method gets the list of courses a teacher is teaching by their ID
        [HttpGet]
        [Route("GetCoursesByTeacher/{id}")]
        public List<string> GetCoursesByTeacher(int id)
        {
            // Create a list to store course names
            List<string> courses = new List<string>();

            // Open the database connection
            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();

                // SQL query to get courses for a teacher by ID
                string query = "SELECT CourseName FROM courses WHERE TeacherId = @id";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                // Add the parameter for the teacher ID
                command.Parameters.AddWithValue("@id", id);

                // Execute the query
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Read the results and add each course to the list
                    while (reader.Read())
                    {
                        courses.Add(reader["CourseName"].ToString());
                    }
                }
            }

            // Return the list of courses
            return courses;
        }
    }
}
