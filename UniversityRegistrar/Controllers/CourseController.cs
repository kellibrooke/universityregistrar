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
        [HttpGet("/courses/new")]
        public IActionResult CourseForm()
        {
            return View(Student.GetAll());
        }

        [HttpPost("/courses/new")]
        public IActionResult CreateCourse(string name, string crn, int id)
        {
            Student newStudent = Student.Find(id);
            Course newCourse = new Course(name, crn);
            newCourse.Save();
            newCourse.AddStudent(newStudent);
            return RedirectToAction("ViewAllCourses");
        }

        [HttpGet("/courses")]
        public IActionResult ViewAllCourses()
        {
            return View(Course.GetAll());
        }

        [HttpGet("/courses/{id}/details")]
        public IActionResult CourseDetails(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Course newCourse = Course.Find(id);
            List<Student> studentList = newCourse.GetStudents();
            model.Add("course", newCourse);
            model.Add("students", studentList);
            return View(model);
        }
    }
}
