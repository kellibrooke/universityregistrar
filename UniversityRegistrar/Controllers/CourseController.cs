using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversityRegistrar.Controllers
{
    public class CourseController : Controller
    {
        [HttpGet("/new-course")]
        public IActionResult CourseForm()
        {
            return View(Course.GetAll());
        }

        [HttpPost("/new-course")]
        public IActionResult CreateCourse(string name, string crn, int id)
        {
            Student newStudent = Student.Find(id);
            Course newCourse = new Course(name, crn);
            newCourse.Save();
            newCourse.AddStudent(newStudent);
            return RedirectToAction("ViewAllCourses");
        }

        [HttpGet("/view-all-courses")]
        public IActionResult ViewAllCourses()
        {
            return View(Course.GetAll());
        }
    }
}
