using Dapper;
using DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperExTest
{
    public class Account
    {
        [Id]
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual int Age { get; set; }
        [Column(true)]
        public virtual int Flag { get; set; }
        [Ignore]
        public virtual string AgeStr
        {
            get
            {
                return "年龄：" + Age;
            }
        }
    }
    public class Acc
    {
        public virtual string Ids { get; set; }
        public virtual string Test { get; set; }
    }
}
