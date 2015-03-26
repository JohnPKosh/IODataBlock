using System.IO;
using System.Linq;
using Data.DbClient;
using DbExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Data
{
    [TestClass]
    public class DbClientTests
    {
        private const string SqlServer = @"(localdb)\ProjectsV12";
        private const string SqlServerDatabase = @"IODataBlock.Database";

        private string SqlServerConnectionString
        {
            get
            {
                return Database.CreateSqlConnectionString(SqlServer, SqlServerDatabase);
            }
        }

        private const string SqliteFile = "sqliteTest.sl3";

        private string SqliteConnectionString
        {
            get
            {
                return Database.CreateSqlLiteConnectionString(SqliteFile, "foo", 60, false, false, 100, 2000, 1024, false, false);
            }
        }

        #region SQL Server Tests

        [TestMethod]
        public void QuerySqlServerSchemaTest()
        {
            using (Database db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                #region sql

                var sql = @"
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

                var data = db.Query(sql, 120, "Data%");
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        #endregion SQL Server Tests

        #region SQLite

        [TestMethod]
        public void SqliteSimpleTest()
        {
            using (var db = Database.OpenConnectionString(SqliteConnectionString, "System.Data.SQLite"))
            {
                // Execute query
                foreach (var a in db.Query("SELECT * FROM `ImportLog`  ORDER BY `rowid` ASC LIMIT 0, 50000;"))
                {
                    if (a.LogData != null)
                    {
                        // do somethin
                    }
                }
            }
        }

        [TestMethod]
        public void SqliteTransactionTest()
        {
            if (File.Exists(SqliteFile)) File.Delete(SqliteFile);
            using (var db = Database.OpenConnectionString(SqliteConnectionString, "System.Data.SQLite"))
            {
                db.Connection.Open();

                db.Execute(@"CREATE TABLE `ImportLog`(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, LogDate DATETIME NOT NULL, LogData TEXT, RowCount INT)");

                using (var transaction = db.Connection.BeginTransaction())
                {
                    // 100,000 inserts
                    for (var i = 0; i < 10000; i++)
                    {
                        var CommandText = "INSERT INTO `ImportLog` (LogDate, LogData, RowCount) VALUES ('2014-06-29 00:00:00', 'test', @0);";
                        db.Execute(CommandText, 60, i);
                    }
                    transaction.Commit();
                }
            }
        }

        #endregion SQLite

        #region DbExtensions.SqlBuilder Tests

        [TestMethod]
        public void SqlBuilderCanCreateSimpleQueryTest()
        {
            using (Database db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                #region sql

//                var sql = @"
//SELECT [TABLE_CATALOG]
//    ,[TABLE_SCHEMA]
//    ,[TABLE_NAME]
//    ,[COLUMN_NAME]
//    ,[ORDINAL_POSITION]
//    ,[COLUMN_DEFAULT]
//    ,[IS_NULLABLE]
//    ,[DATA_TYPE]
//    ,[CHARACTER_MAXIMUM_LENGTH]
//    ,[CHARACTER_OCTET_LENGTH]
//    ,[NUMERIC_PRECISION]
//    ,[NUMERIC_PRECISION_RADIX]
//    ,[NUMERIC_SCALE]
//    ,[DATETIME_PRECISION]
//    ,[CHARACTER_SET_CATALOG]
//    ,[CHARACTER_SET_SCHEMA]
//    ,[CHARACTER_SET_NAME]
//    ,[COLLATION_CATALOG]
//    ,[COLLATION_SCHEMA]
//    ,[COLLATION_NAME]
//    ,[DOMAIN_CATALOG]
//    ,[DOMAIN_SCHEMA]
//    ,[DOMAIN_NAME]
//FROM [INFORMATION_SCHEMA].[COLUMNS]
//WHERE [TABLE_NAME] LIKE @0
//ORDER BY [ORDINAL_POSITION]
//";

                #endregion sql

                var query = SQL.SELECT("*")
                    .FROM("[INFORMATION_SCHEMA].[COLUMNS]")
                    .WHERE("[TABLE_NAME] LIKE @0");
                var sql = query.ToString();

                var data = db.Query(sql, 120, "Data%");
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        #endregion
    }
}