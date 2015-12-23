using MySql.Data.MySqlClient;
using StudentManagementSystem.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class StudentContext
    {
        private string _connectionString;

        public StudentContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        #region Create
        public bool AddStudent(Student student)
        {
            int numOfRecordsAffected = 0;

            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = "INSERT INTO student "
                    + "(FullName, DateOfBirth, Email, MobileContact, CourseId) "
                    + "VALUES "
                    + "(@fullName, @dob, @email, @mobileContact, @courseId);";
                cmd.Parameters.Add("@fullName", MySqlDbType.VarChar, 200).Value = student.FullName;
                cmd.Parameters.Add("@dob", MySqlDbType.DateTime).Value = student.DateOfBirth.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@email", MySqlDbType.VarChar, 200).Value = student.Email;
                cmd.Parameters.Add("@mobileContact", MySqlDbType.VarChar, 20).Value = student.MobileContact;
                cmd.Parameters.Add("@courseId", MySqlDbType.Int32).Value = student.CourseId;

                try
                {
                    cn.Open();
                    numOfRecordsAffected = cmd.ExecuteNonQuery();

                }
                catch (MySqlException e)
                {
                    cn.Close(); //only needed if you are throwing an error,
                                //as once thrown it will not go to finally block
                    if (e.Message.Contains("Student_EmailUniqueConstraint"))
                    {
                        string message = string.Format("{0} is already used.", student.Email);
                        throw new Exception(message);
                    }
                }
                finally
                {
                    cn.Close();
                }
            }
            return numOfRecordsAffected == 1;
        }
        #endregion

        #region Retrieve
        public Student GetStudent(int id)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            using (MySqlDataAdapter da = new MySqlDataAdapter())
            {
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM student " +
                    "WHERE StudentId = @id;";
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                da.SelectCommand = cmd;
                try
                {
                    cn.Open();
                    da.Fill(ds, "student");
                }
                catch (MySqlException e)
                {
                    //do something with the error
                }
                finally
                {
                    cn.Close();
                }
            }
            //Get the first row of the DataTable
            DataRow dr = ds.Tables["student"].AsEnumerable().Single((row) => row.Field<int>("StudentId") == id);
            Student student = new Student()
            {
                StudentId = dr.Field<int>("StudentId"),
                FullName = dr.Field<string>("FullName"),
                DateOfBirth = dr.Field<DateTime>("DateOfBirth"),
                Email = dr.Field<string>("Email"),
                MobileContact = dr.Field<string>("MobileContact"),
                CourseId = dr.Field<int>("CourseId")
            };

            return student;
        }
        public List<Student> GetStudents()
        {
            DataSet ds = new DataSet();

            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            using (MySqlDataAdapter da = new MySqlDataAdapter())
            {
                cmd.Connection = cn;
                cmd.CommandText = "SELECT StudentId, FullName, DateOfBirth, Email, MobileContact, CourseId FROM student;";
                da.SelectCommand = cmd;
                try
                {
                    cn.Open();
                    da.Fill(ds, "student");
                }
                catch (MySqlException e)
                {
                    //do something with the error
                }
                finally
                {
                    cn.Close();
                }
            }

            //Requires System.Data.DataSetExtensions reference for the AsEnumerable method
            List<Student> students = ds.Tables["student"].AsEnumerable().Select(row => new Student()
            {
                StudentId = row.Field<int>("StudentId"),
                FullName = row.Field<string>("FullName"),
                DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                Email = row.Field<string>("Email"),
                MobileContact = row.Field<string>("MobileContact"),
                CourseId = row.Field<int>("CourseId"),
            }).ToList();

            return students;
        }

        public List<Student> GetStudents(string search)
        {
            DataSet ds = new DataSet();

            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            using (MySqlDataAdapter da = new MySqlDataAdapter())
            {
                cmd.Connection = cn;
                cmd.CommandText = "SELECT StudentId, FullName, DateOfBirth, Email, MobileContact, CourseId FROM student "
                    + "WHERE FullName LIKE @search OR "
                    + "Email LIKE @search OR "
                    + "MobileContact LIKE @search;";
                cmd.Parameters.Add("@search", MySqlDbType.VarChar, 200).Value = string.Format("%{0}%", search);
                da.SelectCommand = cmd;
                try
                {
                    cn.Open();
                    da.Fill(ds, "student");
                }
                catch (MySqlException e)
                {
                    //do something with the error
                }
                finally
                {
                    cn.Close();
                }
            }

            //Requires System.Data.DataSetExtensions reference for the AsEnumerable method
            List<Student> students = ds.Tables["student"].AsEnumerable().Select(row => new Student()
            {
                StudentId = row.Field<int>("StudentId"),
                FullName = row.Field<string>("FullName"),
                DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                Email = row.Field<string>("Email"),
                MobileContact = row.Field<string>("MobileContact"),
                CourseId = row.Field<int>("CourseId"),
            }).ToList();

            return students;
        }
        #endregion

        #region Update
        public bool UpdateStudent(Student student)
        {
            int numOfRowsAffected = 0;
            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = "UPDATE student SET " +
                    "FullName = @fullName, " +
                    "DateOfBirth = @dob, " +
                    "Email = @email, " +
                    "MobileContact = @mobileContact, " +
                    "CourseId = @courseId " +
                    "WHERE " +
                    "StudentId = @studentId;";
                cmd.Parameters.Add("@fullName", MySqlDbType.VarChar, 200).Value = student.FullName;
                cmd.Parameters.Add("@dob", MySqlDbType.DateTime).Value = student.DateOfBirth.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@email", MySqlDbType.VarChar, 200).Value = student.Email;
                cmd.Parameters.Add("@mobileContact", MySqlDbType.VarChar, 20).Value = student.MobileContact;
                cmd.Parameters.Add("@courseId", MySqlDbType.Int32).Value = student.CourseId;
                cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = student.StudentId;
                try
                {
                    cn.Open();
                    numOfRowsAffected = cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    cn.Close();
                    if (e.Message.Contains("Student_EmailUniqueConstraint"))
                    {
                        string message = string.Format("{0} is already used.", student.Email);
                        throw new Exception(message);
                    }
                }
                finally
                {
                    cn.Close();
                }
            }
            return numOfRowsAffected == 1;
        }
        #endregion

        #region Delete
        public bool DeleteStudent(int studentId)
        {
            int numOfRowsAffected = 0;
            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = "DELETE FROM student " +
                    "WHERE StudentId = @studentId;";
                cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = studentId;
                cn.Open();
                numOfRowsAffected = cmd.ExecuteNonQuery();
            }
            return numOfRowsAffected == 1;
        }
        #endregion
    }
}
