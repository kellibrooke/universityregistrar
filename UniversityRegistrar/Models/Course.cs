using System;
using MySql.Data.MySqlClient;
using UniversityRegistrar;
using System.Collections.Generic;

namespace UniversityRegistrar.Models
{
    public class Course
    {
        public string Name { get; set; }
        public string CRN { get; set; }
        public int Id { get; set; }

        public Course(string name, string crn, int id = 0)
        {
            Name = name;
            CRN = crn;
            Id = id;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course)otherCourse;
                bool idEquality = this.Id == newCourse.Id;
                bool nameEquality = this.Name == newCourse.Name;
                bool crnEquality = this.CRN == newCourse.CRN;

                return (idEquality && nameEquality && crnEquality);
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
            cmd.CommandText = @"INSERT INTO courses (name, crn) VALUES (@Name, @CRN);";

            cmd.Parameters.AddWithValue("@Name", this.Name);
            cmd.Parameters.AddWithValue("@CRN", this.CRN);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students_courses(course_id, student_id) VALUES(@CourseId, @StudentId);";


            cmd.Parameters.AddWithValue("@StudentId", newStudent.Id);
            cmd.Parameters.AddWithValue("@CourseId", this.Id);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Student> GetStudents()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT students.* FROM courses
                                JOIN students_courses ON (student.id = students_courses.course_id)
                                JOIN courses ON (students_courses.courses_id = courses.id)
                                WHERE students.id = @StudentId;";

            cmd.Parameters.AddWithValue("@StudentId", this.Id);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Student> allStudents = new List<Student> { };

            while (rdr.Read())
            {
                int studentId = rdr.GetInt32(1);
                string studentName = rdr.GetString(2);
                DateTime enrollmentDate = rdr.GetDateTime(3);
                Student newStudent = new Student(studentName, enrollmentDate, studentId);
                allStudents.Add(newStudent);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allStudents;
        }

        public List<Student> GetAllStudents()
        {
            return Student.GetAll();
        }

        public static List<Course> GetAll()
        {
            List<Course> allCourses = new List<Course> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
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

        public static Course Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses WHERE id = @thisId;";

            cmd.Parameters.AddWithValue("@CourseId", id);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int courseId = 0;
            string courseName = 0;
            string CRN = "";

            while (rdr.Read())
            {
                courseId = rdr.GetInt32(0);
                courseName = rdr.GetString(1);
                CRN = rdr.GetString(2);
            }

            Course foundCourse = new Course(courseName, CRN, courseId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundCourse;
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM courses WHERE id = @CourseId; DELETE FROM students_courses WHERE course_id = @CourseId;";

            cmd.Parameters.AddWithValue("@CourseId", this.Id);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public void Edit(string name, string crn, int newStudentId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE courses SET name = @Name, crn = @CRN  WHERE id = @searchId; UPDATE students_courses SET student_id = @newStudentId WHERE course_id = @searchId";

            cmd.Parameters.AddWithValue("@Name", this.Name);
            cmd.Parameters.AddWithValue("@CRN", this.CRN);
            cmd.Parameters.AddWithValue("@searchId", this.Id);

            cmd.ExecuteNonQuery();
            this.Name = name;
            this.CRN = crn;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
