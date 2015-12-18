using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.ViewModels.Student
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string MobileContact { get; set; }
        public int CourseId { get; set; }
        public string CourseAbbreviation { get; set; }
    }
}
