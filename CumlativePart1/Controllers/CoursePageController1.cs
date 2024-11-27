using CumlativePart1.Controllers;
using CumlativePart1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CumlativePart1.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourse();
            return View(Courses);
        }
       

    }
}