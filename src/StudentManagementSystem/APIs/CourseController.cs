using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using StudentManagementSystem.Models;
using Newtonsoft.Json;

namespace StudentManagementSystem.APIs
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private IOptions<MyOptions> _options;

        public CourseController(IOptions<MyOptions> options)
        {
            _options = options;
        }

        // GET: api/Course
        [HttpGet]
        public string Get()
        {
            IEnumerable<Course> courses = new CourseContext(_options.Value.ConnectionString).GetCourses();
            return JsonConvert.SerializeObject(courses);
        }
    }
}
