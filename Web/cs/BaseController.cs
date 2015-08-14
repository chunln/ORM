using DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web
{
    public class BaseController : Controller
    {

        public string connectionName = "connString";
        public DbBase CreateDbBase()
        {
            return new DbBase(connectionName);
        }
    }
}