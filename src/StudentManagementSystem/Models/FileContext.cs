using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class FileContext
    {
        private string _connectionString;

        public FileContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Create
        public bool AddFile(File file)
        {
            int numOfRowsAffected = 0;

            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = "INSERT INTO file (FileName, ContentType, `Blob`) "
                    + "VALUES "
                    + "(@fileName, @contentType, @blob);";
                cmd.Parameters.Add("@fileName", MySqlDbType.VarChar, 255).Value = file.FileName;
                cmd.Parameters.Add("@contentType", MySqlDbType.VarChar, 255).Value = file.ContentType;
                cmd.Parameters.Add("@blob", MySqlDbType.MediumBlob).Value = file.Blob;

                try
                {
                    cn.Open();
                    numOfRowsAffected = cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {

                }
                finally
                {
                    cn.Close();
                }
            }

            return numOfRowsAffected == 1;
        }
        #endregion

        #region Retrieve
        public File GetFile(int fileId)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            using (MySqlDataAdapter da = new MySqlDataAdapter())
            {
                cmd.Connection = cn;
                cmd.CommandText = "SELECT FileId, FileName, ContentType, `Blob` FROM file "
                    + "WHERE FileId = @fileId;";
                cmd.Parameters.Add("@fileId", MySqlDbType.Int32).Value = fileId;
                da.SelectCommand = cmd;

                try
                {
                    cn.Open();
                    da.Fill(ds, "file");
                }
                catch (MySqlException e)
                {

                }
                finally
                {
                    cn.Close();
                }
            }

            DataRow dr = ds.Tables["file"].AsEnumerable().Single((row) => row.Field<int>("FileId") == fileId);
            File file = new File()
            {
                FileId = dr.Field<int>("FileId"),
                FileName = dr.Field<string>("FileName"),
                ContentType = dr.Field<string>("ContentType"),
                Blob = dr.Field<byte[]>("Blob"),
            };

            return file;
        }
        #endregion
    }
}
