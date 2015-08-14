using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.Common;
using System.Data;
using Dapper;

namespace DapperExTest
{
    [TestClass]
    public class UnitTest2
    {
        public string connectionName = "strSqlCe";
        public IDbConnection CreateConnection()
        {
            var providerName = "System.Data.SqlClient";
            var connStr = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionName].ProviderName))
                providerName = ConfigurationManager.ConnectionStrings[connectionName].ProviderName;

            var dbProvider = DbProviderFactories.GetFactory(providerName);
            var conn = dbProvider.CreateConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            return conn;
        }

        [TestMethod]
        public void TestMethod1()
        {
            using (var db = CreateConnection())
            {
                var data = db.Query("select * from Account");
            }
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
            using (var db = CreateConnection())
            {
                var result = db.Execute("INSERT INTO Account(Id,Name,Password,Email,CreateTime,Age) VALUES(@Id,@Name,@Password,@Email,@CreateTime,@Age)", model);
                if (result==1)
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
                using (var db = CreateConnection())
                {
                  var result= db.Execute("INSERT INTO Account(Id,Name,Password,Email,CreateTime,Age) VALUES(@Id,@Name,@Password,@Email,@CreateTime,@Age)", model);
                }
            }
        }
    }
}
