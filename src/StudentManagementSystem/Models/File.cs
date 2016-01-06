using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class File
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Blob { get; set; }
    }
}
