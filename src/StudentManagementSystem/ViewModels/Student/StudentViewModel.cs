using StudentManagementSystem.Models;
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

        public static List<StudentViewModel> JoinStudentsAndCourses(List<Models.Student> students, List<Course> courses)
        {
            List<StudentViewModel> studentVMs = students.AsEnumerable().Join(
                courses,
                (student) => student.CourseId,
                (course) => course.CourseId,
                (student, course) =>
                {
                    StudentViewModel studentVM = new StudentViewModel()
                    {
                        StudentId = student.StudentId,
                        FullName = student.FullName,
                        DateOfBirth = student.DateOfBirth,
                        Email = student.Email,
                        MobileContact = student.MobileContact,
                        CourseId = student.CourseId,
                        CourseAbbreviation = course.CourseAbbreviation,
                    };
                    return studentVM;
                }
            ).ToList();

            return studentVMs;
        }
    }
}
