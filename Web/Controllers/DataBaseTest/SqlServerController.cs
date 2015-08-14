using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using DapperEx;
using Entity;

namespace Web.Controllers.DataBaseTest
{
    public class SqlServerController : BaseController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public JsonResult GetList()
        {
            object result;
            //var result = new IList<Student>();
            string Name = Request["Name"];
            string Age = Request["Age"];
            string School = Request["School"];

            using (var db = CreateDbBase())
            {
                var d = SqlQuery<Student>.Builder(db);

                if (!string.IsNullOrWhiteSpace(Name))
                    d.AndWhere(m => m.Name, OperationMethod.Contains, Name);
                if (!string.IsNullOrWhiteSpace(Age))
                    d.AndWhere(m => m.Age, OperationMethod.Equal, Age);
                if (!string.IsNullOrWhiteSpace(School))
                    d.AndWhere(m => m.SchoolGuid, OperationMethod.In, string.Format("(SELECT GUID FROM School WHERE SchoolName LIKE '%{0}%')", School));

                result = db.Query<Student>(d);
            }

            return Json(result);
        }

        public JsonResult LoadSchool()
        {
            object result;
            using (var db = CreateDbBase())
            {
                result = db.Query<School>();

            }
            return Json(result);
        }


        public JsonResult SaveStudent()
        {
            object info;
            using (var db = CreateDbBase())
            {
                var result = db.Insert<Student>(new Student()
                {
                    GUID = Guid.NewGuid().ToString(),
                    Name = Request["Name"],
                    Gender = Request["Gender"],
                    Age = int.Parse(Request["Age"]),
                    Phone = Request["Phone"],
                    Address = Request["Address"],
                    SchoolGuid = Request["School"]
                });
                if (result)
                {
                    info = new { state = true, message = "保存成功！", icon = "success" };
                }
                else
                {
                    info = new { state = true, message = "保存失败！", icon = "filed" };
                }
            }
            return Json(info);
        }
    }
}
