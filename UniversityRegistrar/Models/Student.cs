﻿using System;
using MySql.Data.MySqlClient;
using UniversityRegistrar;
using System.Collections.Generic;

namespace UniversityRegistrar.Models
{
    public class Student
    {
        public string Name { get; set; }
        public DateTime DOE { get; set; }
        public int Id { get; set; }

        public Student(string name, DateTime doe, int id = 0)
        {
            Name = name;
            DOE = doe;
            Id = id;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student)otherStudent;
                bool idEquality = this.Id == newStudent.Id;
                bool nameEquality = this.Name == newStudent.Name;
                bool doeEquality = this.DOE == newStudent.DOE;

                return (idEquality && nameEquality && doeEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students (name, doe) VALUES (@Name, @DOE);";

            cmd.Parameters.AddWithValue("@Name", this.Name);
            cmd.Parameters.AddWithValue("@DOE", this.DOE);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students_courses(course_id, student_id) VALUES(@CourseId, @StudentId);";


            cmd.Parameters.AddWithValue("@CourseId", newCourse.Id);
            cmd.Parameters.AddWithValue("@StudentId", this.Id);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Course> GetCourses()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM students
                                JOIN students_courses ON (students.id = students_courses.student_id)
                                JOIN courses ON (students_courses.course_id = courses.id)
                                WHERE student_id = @StudentId;";

            cmd.Parameters.AddWithValue("@StudentId", this.Id);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Course> allCourses = new List<Course> { };

            while (rdr.Read())
            {
                int courseId = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseCRN = rdr.GetString(2);
                Course newCourse = new Course(courseName, courseCRN, courseId);
                allCourses.Add(newCourse);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCourses;
        }

        public List<Course> GetAllCourses()
        {
            return Course.GetAll();
        }

        public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                DateTime studentDOE = rdr.GetDateTime(2);

                Student newStudent = new Student(studentName, studentDOE, studentId);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public static Student Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students WHERE id = @studentId;";

            cmd.Parameters.AddWithValue("@studentId", id);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int studentId = 0;
            string studentName = "";
            DateTime DOE = DateTime.MinValue;

            while (rdr.Read())
            {
                studentId = rdr.GetInt32(0);
                studentName = rdr.GetString(1);
                DOE = rdr.GetDateTime(2);
            }

            Student foundStudent = new Student(studentName, DOE, studentId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundStudent;
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students WHERE id = @StudentId; DELETE FROM students_courses WHERE student_id = @StudentId;";

            cmd.Parameters.AddWithValue("@StudentId", this.Id);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public void Edit(string newName, DateTime newDOE)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE students SET name = @Name, doe = @DOE  WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@Name", newName);
            cmd.Parameters.AddWithValue("@DOE", newDOE);
            cmd.Parameters.AddWithValue("@searchId", Id);

            cmd.ExecuteNonQuery();
            this.Name = newName;
            this.DOE = newDOE;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}