using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversityRegistrar.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet("/new-student")]
        public IActionResult StudentForm()
        {
            return View(Course.GetAll());
        }

        [HttpPost("/new-student")]
        public IActionResult CreateStudent(string studentName, DateTime dateEnrolled, int course)
        {
            Student newStudent = new Student(studentName, dateEnrolled);
            Course newCourse = Course.Find(course);
            newStudent.Save();
            newStudent.AddCourse(newCourse);
            return RedirectToAction("ViewAllStudents");
        }

        [HttpGet("/view-all-students")]
        public IActionResult ViewAllStudents()
        {
            return View(Student.GetAll());
        }

    }
}
