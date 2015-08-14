using DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Student
    {
        [Id(CheckAutoId = false)]
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string SchoolGuid { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
