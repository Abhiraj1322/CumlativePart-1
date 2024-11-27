
using Microsoft.AspNetCore.Mvc;
using CumlativePart1.Models;

namespace CumlativePart1.Controllers
{

    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudent();
            return View(Students);
        }

        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

    }

}
