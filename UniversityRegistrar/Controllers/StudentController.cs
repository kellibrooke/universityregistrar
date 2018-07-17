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
        [HttpGet("/students/new")]
        public IActionResult StudentForm()
        {
            return View(Course.GetAll());
        }

        [HttpPost("/students/new")]
        public IActionResult CreateStudent(string studentName, DateTime dateEnrolled, int course)
        {
            Student newStudent = new Student(studentName, dateEnrolled);
            Course newCourse = Course.Find(course);
            newStudent.Save();
            newStudent.AddCourse(newCourse);
            return RedirectToAction("ViewAllStudents");
        }

        [HttpGet("/students")]
        public IActionResult ViewAllStudents()
        {
            return View(Student.GetAll());
        }

        [HttpGet("/students/{id}/details")]
        public IActionResult StudentDetails(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Student newStudent = Student.Find(id);
            List<Course> courseList = newStudent.GetCourses();
            model.Add("student", newStudent);
            model.Add("courses", courseList);
            return View(model);
        }
    }
}
