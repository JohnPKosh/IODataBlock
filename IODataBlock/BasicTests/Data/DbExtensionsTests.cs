using System;
using DbExtensions;
using data = Data.DbClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.DbClient.Extensions;


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
                .WHERE("[LATA] = {0}","224")
                .WHERE("[NPA] = {0}","201");
            

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

        [TestMethod]
        public void TestMethod4()
        {
            var query = SQL
                .SELECT(@"[LATA]
      ,[LATA NAME]
      ,[STATUS]
      ,[EFF DATE]
      ,[NPA]
      ,[NXX]
      ,[BLOCK ID]
      ,[FILLER1]
      ,[COC TYPE]
      ,[SSC]
      ,[DIND]
      ,[TD-EO]
      ,[TD-AT]
      ,[PORTABLE]
      ,[AOCN]
      ,[FILLER2]
      ,[OCN]
      ,[LOC NAME]
      ,[LOC INDEX]
      ,[LOC STATE]
      ,[RC ABBRE]
      ,[RC TYPE]
      ,[LINE FR]
      ,[LINE TO]
      ,[SWITCH]
      ,[SHA INDICATOR]
      ,[FILLER3]
      ,[TEST LINE #]
      ,[TEST LINE RESPONSE]
      ,[TEST LINE EXP DATE]
      ,[1000 BLK POOL]
      ,[RC LATA]
      ,[FILLER4]
      ,[CREATION DATE]
      ,[FILLER5]
      ,[E STATUS DATE]
      ,[FILLER6]
      ,[LAST MODIFIED]
      ,[FILLER7]")
                .FROM("[LERG 6]")
                .WHERE("[LATA] = {0}", "224")
                .WHERE("[NPA] = {0}", "201");


            string sql;
            using (var db = data.Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                var command = query.ToCommand(db.Connection);
                sql = command.ToTraceString();
                //command.CommandText = data.Database.CreateSqlServer2008BatchSelect(command.CommandText, 2, 100, "NPA");
                var dt = db.QueryAsDataTable(command.SetPagingOptions(1, 100, "NPA"), "results");
                Assert.IsNotNull(dt);
            }
            Assert.IsNotNull(sql);

/*

WITH [temp_batch_table] AS
(
    SELECT ROW_NUMBER() OVER (ORDER BY NPA) AS [RowNumber],  *
FROM [LERG 6]
WHERE [LATA] = @p0 AND [NPA] = @p1
)
SELECT *
FROM [temp_batch_table]
WHERE [RowNumber] BETWEEN 100 AND 199;

*/
        }

        [TestMethod]
        public void TestMethod5()
        {
            var query = SQL
                .SELECT("*")
                .FROM("SMS.sms_numbers")
                .WHERE("cust_id = {0}", "8X8")
                .Append(";");


            string sql;
            using (var db = data.Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                var command = query.ToCommand(db.Connection);
                
                var provider = db.GetConnectionProviderName();
                sql = command.ToTraceString();
                command.CommandText = data.Database.CreateMySqlBatchSelect(command.CommandText, 1, 100, "tn");
                var dt = db.QueryAsDataTable(command, "results");
                Assert.IsNotNull(dt);
            }
            Assert.IsNotNull(sql);

/*

SELECT *
FROM 
(
    SELECT *
FROM SMS.sms_numbers
WHERE cust_id = @p0
) as a
ORDER BY tn
LIMIT 100 OFFSET 100;

*/
        }

        [TestMethod]
        public void TestMethod6()
        {
            var query = SQL
                .SELECT("*")
                .FROM("SMS.sms_numbers")
                .WHERE("cust_id = {0}", "8X8")
                .Append(";");


            string sql;
            using (var db = data.Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                var command = query.ToCommand(db.Connection);
                sql = command.ToTraceString();
                command.CommandText = data.Database.CreateOracleBatchSelect(command.CommandText, 1, 100, "tn");
                var dt = db.QueryAsDataTable(command, "results");
                Assert.IsNotNull(dt);

                /*

                SELECT *
                FROM(
                        SELECT ROW_NUMBER() OVER (ORDER BY tn) AS RowNumber,  *
                FROM SMS.sms_numbers
                WHERE cust_id = @p0        
                    ) a
                WHERE [RowNumber] BETWEEN 100 AND 200;

                */
            }
            Assert.IsNotNull(sql);
        }
    }
}
