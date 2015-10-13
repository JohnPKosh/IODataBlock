using System;
using DbExtensions;
using data = Data.DbClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BasicTests.Data
{
    [TestClass]
    public class DbExtensionsTests
    {

        private const string SqlServer = @".\EXP14";
        private const string SqlServerDatabase = @"LERG";

        private static string SqlServerConnectionString
        {
            get
            {
                return data.Database.CreateSqlConnectionString(SqlServer, SqlServerDatabase);
            }
        }


        [TestMethod]
        public void TestMethod1()
        {
            var query = new SqlBuilder(@"
            SELECT ProductID, ProductName
            FROM Products")
           .WHERE("CategoryID = {0}", 1);

            string sql;
            using (var db = data.Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                var command = query.ToCommand(db.Connection);
                sql = command.ToTraceString();
            }
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var query = new SqlBuilder()
                .SELECT("*")
                .FROM("[LERG 6]")
           .WHERE("[LATA] = {0}", "224");

            string sql;
            using (var db = data.Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                var command = query.ToCommand(db.Connection);
                sql = command.ToTraceString();
                
            }
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var query = SQL
                .SELECT("*")
                .FROM("[LERG 6]")
                .WHERE("[LATA] = {0}","224");
            

            string sql;
            using (var db = data.Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                var command = query.ToCommand(db.Connection);
                sql = command.ToTraceString();
                command.CommandText = data.Database.CreateSqlServer2008BatchSelect(command.CommandText, 2, 100, "NPA");
                var dt = db.QueryAsDataTable(command,"results");
                Assert.IsNotNull(dt);
            }
            Assert.IsNotNull(sql);
        }
    }
}
