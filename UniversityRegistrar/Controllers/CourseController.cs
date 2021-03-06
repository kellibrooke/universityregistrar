﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using UniversityRegistrar;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversityRegistrar.Controllers
{
    public class CourseController : Controller
    {
        [HttpGet("/courses/new")]
        public IActionResult CourseForm()
        {
            return View();
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

        [HttpPost("/courses/{id}/delete")]
        public IActionResult Delete(int id)
        {
            Course newCourse = Course.Find(id);
            newCourse.Delete();
            return RedirectToAction("ViewAllCourses");
        }

        [HttpGet("/courses/{id}/update")]
        public IActionResult UpdateForm(int id)
        {
            Course newCourse = Course.Find(id);
            return View(newCourse);
        }

        [HttpPost("/courses/{id}/update")]
        public IActionResult Update(string newName, string newCRN, int id)
        {
            Course newCourse = Course.Find(id);
            newCourse.Edit(newName, newCRN);
            return RedirectToAction("ViewAllCourses");
        }
    }
}

