using System;
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
    }
}
