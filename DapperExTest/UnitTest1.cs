using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using System.Collections.Generic;
using DapperEx;
using System.Configuration;
using System.Data.Common;

namespace DapperExTest
{
    [TestClass]
    public class UnitTest1
    {
        public string connectionName = "strSqlCe";
        public DbBase CreateDbBase()
        {
            return new DbBase(connectionName);
        }
        [TestMethod]
        public void Connection()
        {
            var providerName = "System.Data.SqlClient";
            var connStr = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionName].ProviderName))
                providerName = ConfigurationManager.ConnectionStrings[connectionName].ProviderName;

            var dbProvider = DbProviderFactories.GetFactory(providerName);
            var conn = dbProvider.CreateConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            Console.WriteLine("状态：" + conn.State.ToString());
        }
        [TestMethod]
        public void Insert()//插入一条数据
        {
            var model = new Account()
            {
                Id = "1",
                Name = "张三1",
                Password = "123456",
                Email = "123@qq.com",
                CreateTime = DateTime.Now,
                Age = 15
            };
            using (var db = CreateDbBase())
            {
                var result = db.Insert<Account>(model);
                if (result)
                    Console.WriteLine("添加成功");
                else
                    Console.WriteLine("添加失败");
            }
        }
        [TestMethod]
        public void Insert2()//插入一条数据
        {
            for (int i = 2; i < 10; i++)
            {
                var model = new Account()
                {
                    Id = i.ToString(),
                    Name = "张三" + i.ToString(),
                    Password = "123456",
                    Email = "123@qq.com",
                    CreateTime = DateTime.Now,
                    Age = 15
                };
                using (var db = CreateDbBase())
                {
                    var result = db.Insert<Account>(model);
                }
            }
        }
        [TestMethod]
        public void Insert3()//插入一条数据
        {
            using (var db = CreateDbBase())
            {
                var tran = db.DbTransaction;
                for (int i = 2; i < 10; i++)
                {
                    var model = new Account()
                    {
                        Id = i.ToString(),
                        Name = "张三" + i.ToString(),
                        Password = "123456",
                        Email = "123@qq.com",
                        CreateTime = DateTime.Now,
                        Age = 15
                    };

                    var result = db.Insert<Account>(model,tran);

                }
                tran.Commit();
            }
        }



        [TestMethod]
        public void InsertBatch()//插入多条数据
        {
            var list = new List<Account>();
            for (int i = 10; i < 21; i++)
            {
                var model = new Account()
                {
                    Id = i.ToString(),
                    Name = "张三" + i.ToString(),
                    Password = "123456",
                    Email = "123@qq.com",
                    CreateTime = DateTime.Now,
                    Age = 15
                };
                list.Add(model);
            }
            using (var db = CreateDbBase())
            {
                var result = db.InsertBatch<Account>(list);
                if (result)
                    Console.WriteLine("添加成功");
                else
                    Console.WriteLine("添加失败");
            }
        }
        [TestMethod]
        public void QueryNoWhere()//无条件的查询,相当于GetAll
        {
            using (var db = CreateDbBase())
            {
                var result = db.Query<Account>();
                Console.WriteLine("查询出数据条数:" + result.Count);
            }
        }
        [TestMethod]
        public void Query()//条件查询
        {
            using (var db = CreateDbBase())
            {
                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.Age, OperationMethod.Less, 20)
                     .LeftInclude()//此表示左括号，所以后面必须有右括号与之对应
                     .AndWhere(m => m.CreateTime, OperationMethod.Greater, DateTime.Now.AddDays(-5))
                     .AndWhere(m => m.CreateTime, OperationMethod.LessOrEqual, DateTime.Now.AddDays(5))
                     .OrWhere(m => m.Name, OperationMethod.Contains, "张")
                     .RightInclude()//右括号
                     .Top(10)//前10条
                     .AndWhere(m => m.Age, OperationMethod.In, new List<int>() { 15 })
                     .OrderBy(m => m.Age, true);
                //WHERE Age < @para_1 AND ( CreateTime > @para_2 OR Name LIKE @para_3 ) AND Age IN @para_4 ORDER BY Age DESC
                var result = db.Query<Account>(d);
                Console.WriteLine("查询出数据条数:" + result.Count);
            }
        }
        [TestMethod]
        public void Page()//分布查询
        {
            using (var db = CreateDbBase())
            {

                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.Age, OperationMethod.Less, 20)
                     .LeftInclude()
                     .AndWhere(m => m.CreateTime, OperationMethod.Greater, DateTime.Now.AddDays(-5))
                     .AndWhere(m => m.Name, OperationMethod.Contains, "张")
                     .RightInclude()
                     .AndWhere(m => m.Age, OperationMethod.In, new List<int>() { 15 })
                     ;
                long dc = 0;
                var result = db.Page<Account>(1, 20, out dc, d);
                Console.WriteLine("查询出数据条数:" + result.Count);
            }
        }

        [TestMethod]
        public void Delete()//删除测试
        {
            using (var db = CreateDbBase())
            {

                var d = SqlQuery<Account>.Builder(db)
                    .AndWhere(m => m.Id, OperationMethod.Equal, "1");
                var result = db.Delete<Account>(d);
            }
        }
        [TestMethod]
        public void Delete2() //删除测试
        {
            using (var db = CreateDbBase())
            {

                var d = SqlQuery<Account>.Builder(db)
                    .AndWhere(m => m.Id, OperationMethod.In, new List<string>() { "2", "3" });
                var result = db.Delete<Account>(d);
            }
        }
        [TestMethod]
        public void Single()//获取一条数据
        {
            using (var db = CreateDbBase())
            {
                var model = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db)
                    .AndWhere(m => m.Id, OperationMethod.Equal, "4"));
            }
        }
        [TestMethod]
        public void Update() //修改数据
        {
            using (var db = CreateDbBase())
            {
                var model = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db)
                   .AndWhere(m => m.Id, OperationMethod.Equal, "4"));
                model.Name = "李四";
                var result = db.Update<Account>(model);
            }
        }
        [TestMethod]
        public void UpdateBatch()//批量修改
        {
            using (var db = CreateDbBase())
            {
                var model = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db)
                   .AndWhere(m => m.Id, OperationMethod.Equal,"4"));
                model.Name = "李四";
                var result = db.Update<Account>(model, SqlQuery<Account>.Builder(db)
                   .AndWhere(m => m.Id, OperationMethod.In,new List<string>(){ "5","6"}));
            }
        }
        [TestMethod]
        public void Count()//数量
        {
            using (var db = CreateDbBase())
            {
                var result = db.Count<Account>(SqlQuery<Account>.Builder(db)
                   .AndWhere(m => m.Id, OperationMethod.In, new List<string>() { "5", "6" }));
                Console.WriteLine(result);
            }
        }
    }
}
