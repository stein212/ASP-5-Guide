using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels.Student;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private IOptions<MyOptions> _options;

        public StudentController(IOptions<MyOptions> options)
        {
            _options = options;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<StudentViewModel> studentVMs = new List<StudentViewModel>();

            try
            {
                List<Student> students = new StudentContext(_options.Value.ConnectionString).GetStudents();
                List<Course> courses = new CourseContext(_options.Value.ConnectionString).GetCourses();
                studentVMs = JoinStudentsAndCourses(students, courses);
            }
            catch (Exception e)
            {
                ViewData["divMessage"] = "Unable to retrieve data. Please contact the administrator if the problem persists";
            }

            return View(studentVMs);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            ViewData["students"] = new List<Student>();

            try
            {
                List<Student> students = new StudentContext(_options.Value.ConnectionString).GetStudents(search);
                ViewData["students"] = students;
                ViewData["divMessage"] = string.Format("\"{0}\" returned {1} results", search, students.Count);
            }
            catch (Exception e)
            {
                ViewData["divMessage"] = "Unable to retrieve data. Please contact the administrator if the problem persists";
            }

            return View();
        }

        private List<StudentViewModel> JoinStudentsAndCourses(List<Student> students, List<Course> courses)
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

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["courses"] = new CourseContext(_options.Value.ConnectionString).GetCourses();

            return View();
        }

        [HttpPost]
        public IActionResult Add(Student student)
        {
            ViewData["courses"] = new CourseContext(_options.Value.ConnectionString).GetCourses();

            string messageToUser = "";

            StudentContext context = new StudentContext(_options.Value.ConnectionString);
            bool success = false;
            try {
                success = context.addStudent(student);
            }
            catch (Exception e)
            {
                messageToUser = e.Message;
            }
            if (success)
                ViewData["divMessage"] = "Successfully added " + student.FullName;
            else
                ViewData["divMessage"] = "Failed to add " + student.FullName + ". Reason: " + messageToUser;
            return View();
        }
    }
}
