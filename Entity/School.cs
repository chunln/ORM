using DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class School
    {
        [Id(CheckAutoId = false)]
        public string GUID { get; set; }
        public string SchoolName { get; set; }
        public string Address { get; set; }
    }
}
