
using Microsoft.AspNetCore.Mvc;
using CumlativePart1.Controllers;
using CumlativePart1.Models;


namespace CumlativePart1.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult List(DateTime? StartDate, DateTime? EndDate)
        {

            List<Teacher> Teachers = _api.ListTeachers(StartDate, EndDate);
            return View(Teachers);
        }

        [HttpGet]
        public IActionResult Show(int id)
        {

            if (id <= 0)
            {
                ViewBag.ErrorMessage = "Invalid Teacher ID. Please provide a valid ID.";
                return View("Error");
            }


            var selectedTeacher = _api.FindTeacher(id);


            if (selectedTeacher == null)
            {
                ViewBag.ErrorMessage = "The specified teacher does not exist. Please check the Teacher ID.";
                return View("Error");
            }


            var teacherCourses = _api.GetCoursesByTeacher(id);


            if (teacherCourses == null || teacherCourses.Count == 0)
            {
                ViewBag.ErrorMessage = $"No courses found for the teacher with ID {id}.";
                return View("Error");
            }

            var viewModel = new TeacherCoursesViewModel
            {
                Teacher = selectedTeacher,
                Courses = teacherCourses
            };


            return View(viewModel);
        }


    }
}