using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Data.DbClient;
using Data.DbClient.Fluent;
using Data.DbClient.Fluent.Select;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests.Data
{
    [TestClass]
    public class DbQueryBuilderTests
    {
        private const string SqlServer = @".\EXP14";
        private const string SqlServerDatabase = @"LERG";

        private static string SqlServerConnectionString
        {
            get
            {
                return Database.CreateSqlConnectionString(SqlServer, SqlServerDatabase);
            }
        }

        #region sql

        private const string Sql = @"
SELECT [TABLE_CATALOG]
    ,[TABLE_SCHEMA]
    ,[TABLE_NAME]
    ,[COLUMN_NAME]
    ,[ORDINAL_POSITION]
    ,[COLUMN_DEFAULT]
    ,[IS_NULLABLE]
    ,[DATA_TYPE]
    ,[CHARACTER_MAXIMUM_LENGTH]
    ,[CHARACTER_OCTET_LENGTH]
    ,[NUMERIC_PRECISION]
    ,[NUMERIC_PRECISION_RADIX]
    ,[NUMERIC_SCALE]
    ,[DATETIME_PRECISION]
    ,[CHARACTER_SET_CATALOG]
    ,[CHARACTER_SET_SCHEMA]
    ,[CHARACTER_SET_NAME]
    ,[COLLATION_CATALOG]
    ,[COLLATION_SCHEMA]
    ,[COLLATION_NAME]
    ,[DOMAIN_CATALOG]
    ,[DOMAIN_SCHEMA]
    ,[DOMAIN_NAME]
FROM [INFORMATION_SCHEMA].[COLUMNS]
WHERE [TABLE_NAME] LIKE @0
ORDER BY [ORDINAL_POSITION]
";

        #endregion sql

        [TestMethod]
        public void SimpleDbQueryBuilderTest1()
        {
            var qb = new DbQueryBuilder();

            // build a query
            DbQuery query = qb.FromConnection(new SqlConnection(SqlServerConnectionString))
                .WithCommand("SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS]");

            // execute a query
            var data = query.ExecuteQuery();
            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SimpleDbQueryBuilderTest2()
        {
            var qb = new DbQueryBuilder();

            // build a query
            DbQuery query = qb.FromConnection(new SqlConnection(SqlServerConnectionString))
                .WithCommand(Sql)
                .SetParameters("Data%")
                .TimeoutAfter(120);

            // execute a query
            var data = query.ExecuteQuery();
            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SimpleDbQueryBuilderTest3()
        {
            var qb = new DbQueryBuilder();

            // execute a query
            var data = qb.FromConnection(new SqlConnection(SqlServerConnectionString))
                .WithCommand(Sql)
                .SetParameters("Data%")
                .TimeoutAfter(120)
                .Query();

            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SimpleDbQueryBuilderTest4()
        {
            // execute a query
            var data = DbQueryBuilder.CreateFrom(new SqlConnection(SqlServerConnectionString))
                .WithCommand(Sql)
                .SetParameters("Data%")
                .TimeoutAfter(120)
                .Query();

            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SimpleDbQueryBuilderTest5()
        {
            var dbquery = new DbQuery(new SqlConnection(SqlServerConnectionString), Sql, 120, "Data%");

            // execute a query
            var data = DbQueryBuilder.CreateFrom(dbquery).Query();

            if (!data.Any())
            {
                Assert.Fail();
            }
        }

    }
}