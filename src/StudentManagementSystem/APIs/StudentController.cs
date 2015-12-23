using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using StudentManagementSystem.Models;
using Microsoft.Extensions.OptionsModel;
using Newtonsoft.Json;

namespace StudentManagementSystem.APIs
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private IOptions<MyOptions> _options;

        public StudentController(IOptions<MyOptions> options)
        {
            _options = options;
        }

        // GET api/Student/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            Student student = new StudentContext(_options.Value.ConnectionString).GetStudent(id);
            return JsonConvert.SerializeObject(student);
        }

        // GET: api/Student
        [HttpGet]
        public string Get()
        {
            IEnumerable<Student> students = new StudentContext(_options.Value.ConnectionString).GetStudents();
            return JsonConvert.SerializeObject(students);
        }

        // POST api/Student
        [HttpPost]
        public object Post([FromBody] Student student)
        {
            bool success = false;

            string messageToUser = "";
            try
            {
                success = new StudentContext(_options.Value.ConnectionString).AddStudent(student);
            }
            catch (Exception e)
            {
                messageToUser = e.Message;
            }
            return new { Success = success, Message = messageToUser };
        }

        // PUT api/Student
        [HttpPut]
        public object Put([FromBody] Student student)
        {
            bool success = false;

            string messageToUser = "";
            try
            {
                success = new StudentContext(_options.Value.ConnectionString).UpdateStudent(student);
            }
            catch (Exception e)
            {
                messageToUser = e.Message;
            }
            return new { Success = success, Message = messageToUser };
        }

        // DELETE api/Student
        [HttpDelete]
        public object Delete([FromBody] int[] ids)
        {
            StudentContext sc = new StudentContext(_options.Value.ConnectionString);
            int deleteCount = 0;
            foreach (int id in ids)
            {
                if (sc.DeleteStudent(id))
                {
                    deleteCount++;
                    continue;
                }
                else
                    break;
            }

            return new { deleted = deleteCount, notDeleted = ids.Length - deleteCount };
        }
    }
}
