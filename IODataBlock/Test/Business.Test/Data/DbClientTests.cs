using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Excel;
using Business.Test.TestUtility;
using Data.DbClient.Extensions;
using DbExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Database = Data.DbClient.Database;

namespace Business.Test.Data
{
    [TestClass]
    public class DbClientTests
    {
        public DbClientTests()
        {
            _npgsqlConnectionString = _configMgr.GetConnectionString("qixlrn");
            _mySqlConnectionString = _configMgr.GetConnectionString("mysql_local");
            _oracleConnectionString = _configMgr.GetConnectionString("oracle");
        }

        private readonly ConfigMgr _configMgr = new ConfigMgr();

        //private const string SqlServer = @"(localdb)\ProjectsV12";

        //private const string SqlServerDatabase = @"IODataBlock.Database";

        private const string SqlServer = @".\EXP14";

        private const string SqlServerDatabase = @"LERG";

        private static string SqlServerConnectionString
        {
            get
            {
                return Database.CreateSqlConnectionString(SqlServer, SqlServerDatabase);
            }
        }

        private readonly string _mySqlConnectionString;

        private readonly string _npgsqlConnectionString;

        private const string SqliteFile = "sqliteTest.sl3";

        private static string SqliteConnectionString
        {
            get
            {
                return Database.CreateSqlLiteConnectionString(SqliteFile, "foo", 60, false, false, 100, 2000, 1024, false, false);
            }
        }

        private readonly string _oracleConnectionString;

        #region Npgsql

        [TestMethod]
        public void ConnectNpgsqlTest()
        {
            //var name = typeof (Npgsql.NpgsqlFactory).AssemblyQualifiedName;

            using (var db = Database.OpenConnectionString(_npgsqlConnectionString, "Npgsql"))
            {
                db.Connection.Open();
                if (db.Connection.State != ConnectionState.Open) Assert.Fail();
            }
        }

        [TestMethod]
        public void QueryTn2LrnTableNpgsqlTest()
        {
            var sql = @"
                SELECT tn
                    ,lrn
                    ,spid
                    ,cr_date
                FROM tn2lrn216
                limit 100;";

            using (var db = Database.OpenConnectionString(_npgsqlConnectionString, "Npgsql"))
            {
                var data = db.QueryToJObjects(sql, 120);
                if (!data.Any())
                {
                    Assert.Fail();
                }
                var json = data.ToJsonString();
                Assert.IsNotNull(json);
            }
        }

        #endregion Npgsql

        #region MySql Tests

        [TestMethod]
        public void QueryMySqlTest()
        {
            using (var db = Database.OpenConnectionString(_mySqlConnectionString, "MySql.Data.MySqlClient"))
            {
                #region sql

                var sql = @"SELECT * FROM test.notes;";

                #endregion sql

                var data = db.Query(sql, 120);
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        #endregion MySql Tests

        #region SQL Server Tests

        [TestMethod]
        public void TestStaticDatabaseQuery()
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
,NULL as testnull
FROM [INFORMATION_SCHEMA].[COLUMNS]
WHERE [TABLE_NAME] LIKE @0
ORDER BY [ORDINAL_POSITION]
";

            #endregion sql

            var data = Database.Query(SqlServerConnectionString, "System.Data.SqlClient", sql, 120, "LERG%");
            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void temptest()
        {
            Assert.IsNotNull(TestDbClient());
        }
        private string TestDbClient()
        {
            //Data Source=.\EXP14;Initial Catalog=LERG;User ID=servermgr;Password=defr3sTu

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
,NULL as testnull
FROM [INFORMATION_SCHEMA].[COLUMNS]
WHERE [TABLE_NAME] LIKE @0
ORDER BY [ORDINAL_POSITION]
";

            #endregion sql

            try
            {
                var data = Database.Query(@"Data Source=.\EXP14;Initial Catalog=LERG;User ID=servermgr;Password=defr3sTu", "System.Data.SqlClient", sql, 120, "LERG%");
                if (!data.Any())
                {
                    return data.Count().ToString();
                }
                return "broke";
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [TestMethod]
        public void TestStaticDatabaseQuery2()
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
,NULL as testnull
FROM [INFORMATION_SCHEMA].[COLUMNS]
WHERE [TABLE_NAME] LIKE @0
ORDER BY [ORDINAL_POSITION]
";

            #endregion sql

            var myparams = new List<object>() {"LERG%"};
            var data = Database.Query(SqlServerConnectionString, "System.Data.SqlClient", sql, 120, myparams.ToArray());
            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestQueryQueryTransformToTest1()
        {
            #region sql

            var sql = @"SELECT [OCN_#] as [OCN], [OCN_NAME], [CATEGORY] FROM [LERG].[dbo].[LERG 1]";

            #endregion sql

            var data = Database.QueryTransformEach(SqlServerConnectionString, "System.Data.SqlClient", sql, Lerg1DtoLoad, 120, "LERG%");
            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        private static Lerg1Dto Lerg1DtoLoad(JObject jObject)
        {
            return new Lerg1Dto()
            {
                Ocn = jObject.Value<string>("OCN"),
                OcnName = jObject.Value<string>("OCN_NAME"),
                Category = jObject.Value<string>("CATEGORY")
            };
        }

        [TestMethod]
        public void TestQueryQueryTransformToTest2()
        {
            #region sql

            var sql = @"SELECT [OCN_#] as [OCN], [OCN_NAME], [CATEGORY] FROM [LERG].[dbo].[LERG 1]";

            #endregion sql

            var data = Database.QueryTransformEach(SqlServerConnectionString, "System.Data.SqlClient", sql, o => new Lerg1Dto()
            {
                Ocn = o.Value<string>("OCN"),
                OcnName = o.Value<string>("OCN_NAME"),
                Category = o.Value<string>("CATEGORY")
            }, 120, "LERG%");
            if (!data.Any())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestStaticDatabaseFillDataTableSchema()
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
,NULL as testnull
FROM [INFORMATION_SCHEMA].[COLUMNS]
WHERE [TABLE_NAME] LIKE @0
ORDER BY [ORDINAL_POSITION]
";

            #endregion sql

            var data = Database.FillSchemaDataTable(SqlServerConnectionString, "System.Data.SqlClient", sql, "data", 120, "LERG%");
            if (data == null || data.Columns == null || data.Columns.Count == 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void QuerySqlServerSchemaTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
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
,NULL as testnull
FROM [INFORMATION_SCHEMA].[COLUMNS]
WHERE [TABLE_NAME] LIKE @0
ORDER BY [ORDINAL_POSITION]
";

                #endregion sql

                //var data = db.Query(sql, 120, "Data%");
                var data = db.Query(sql, 120, "LERG%");
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void QuerySqlServerJObjectSchemaTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
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

                //var data = db.QueryToJObjects(sql, 120, "Data%").ToList();
                var data = db.QueryToJObjects(sql, 120, "LERG%").ToList();
                if (!data.Any())
                {
                    Assert.Fail();
                }

                foreach (var d in data)
                {
                    var str = d.ToString(Formatting.Indented);
                    if (String.IsNullOrWhiteSpace(str)) Assert.Fail("no json????");
                }

                var jarr = new JArray(data);
                var jarrstr = jarr.ToString();
                if (String.IsNullOrWhiteSpace(jarrstr)) Assert.Fail("no json????");
            }
        }

        [TestMethod]
        public void QuerySqlServerJObjectSchemaTest2()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
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

                //var jarr = new JArray(db.QueryToJObjects(sql, 120, "Data%"));
                var jarr = new JArray(db.QueryToJObjects(sql, 120, "LERG%"));
                var jarrstr = jarr.ToString();
                if (String.IsNullOrWhiteSpace(jarrstr)) Assert.Fail("no json????");
            }
        }

        [TestMethod]
        public void QuerySqlServerToJsonFile()
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

            using (var fs = File.Open(@"c:\junk\query.json", FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                fs.JsonDbQuery(sql, SqlServerConnectionString, null, 60, converters: null, parameters: "Data%");
            }
        }

        [TestMethod]
        public void DeserializeFromStream()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
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

                db.QueryToJObjects(sql, 120, "Data%").WriteJsonToFilePath(@"c:\junk\data.json");

                using (var fs = File.Open(@"c:\junk\data.json", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var jarr = fs.JsonDeserialize<JArray>();
                    var jarrstr = jarr.ToString();
                    if (String.IsNullOrWhiteSpace(jarrstr)) Assert.Fail("no json????");
                }
            }
        }

        [TestMethod]
        public void DeserializeFromBsonStream()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
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

                //db.QueryToJObjects(sql, 120, "Data%").WriteJsonToFilePath(@"c:\junk\data.bson");
                File.WriteAllBytes(@"c:\junk\data.bson", (db.QueryToBson(sql, 120, "%") as MemoryStream).ToArray());

                using (var fs = File.Open(@"c:\junk\data.bson", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var jarr = fs.BsonDeserialize<JObject>();
                    var jarrstr = jarr.ToString();
                    if (String.IsNullOrWhiteSpace(jarrstr)) Assert.Fail("no json????");
                }
            }
        }

        [TestMethod]
        public void DeserializeFromJsonStream()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
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

                //db.QueryToJObjects(sql, 120, "Data%").WriteJsonToFilePath(@"c:\junk\data.bson");
                File.WriteAllBytes(@"c:\junk\data2.json", (db.QueryToJsonStream(sql, 120, "%") as MemoryStream).ToArray());

                using (var fs = File.Open(@"c:\junk\data2.json", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var jarr = fs.JsonDeserialize<JArray>();
                    var jarrstr = jarr.ToString();
                    if (String.IsNullOrWhiteSpace(jarrstr)) Assert.Fail("no json????");
                }
            }
        }

        [TestMethod]
        public void QueryToExcel()
        {
            var con = Database.CreateSqlConnectionString("CLEHBDB02", "BDVOXData");

            var sql = @"
SELECT [SLE_CHNL_ID]
      ,[SLE_CHNL_CD]
      ,[SLE_CHNL_NAME]
      ,[SLE_CHNL_ORIENT]
      ,[SLE_CHNL_ASSGN_DATE]
      ,[SLE_CHNL_TRMN_DATE]
      ,[SLE_CHNL_CONTACT]
      ,[SLE_CHNL_TITLE]
      ,[SLE_CHNL_EMAIL]
      ,[sle_chnl_enable_email]
      ,[SLE_CHNL_PHONE_NUM]
      ,[SLE_COMP_NAME]
      ,[SLE_CHNL_FAX_NUM]
      ,[SLE_CHNL_SOC_SEC]
      ,[SLE_CHNL_FED_ID]
      ,[SLE_CHNL_ADRES1]
      ,[SLE_CHNL_ADRES2]
      ,[SLE_CHNL_CITY]
      ,[STATE_CODE_ID]
      ,[SLE_CHNL_ZIP]
  FROM [BDVOXData].[dbo].[SALE_CHANNEL]
";
            MsExcelExtensionBase.CreateExcelFromQuery(new FileInfo(@"C:\junk\SalesChannel.xlsx"), sql, con);


        }

        #endregion SQL Server Tests

        #region SQLite

        [TestMethod]
        public void SqliteSimpleTest()
        {
            using (var db = Database.OpenConnectionString(SqliteConnectionString, "System.Data.SQLite"))
            {
                // Execute query
                foreach (var a in db.Query(@"SELECT * FROM [ImportLog]"))
                {
                    if (a.LogData == null)
                    {
                        Assert.Fail();
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

        #region Oracle

        [TestMethod]
        public void ConnectOracleTest()
        {
            //var name = typeof (Npgsql.NpgsqlFactory).AssemblyQualifiedName;

            using (var db = Database.OpenConnectionString(_oracleConnectionString, "Oracle.ManagedDataAccess.Client"))
            {
                db.Connection.Open();
                if (db.Connection.State != ConnectionState.Open) Assert.Fail();
            }
        }

        #endregion Oracle

        #region DbExtensions.SqlBuilder Tests

        [TestMethod]
        public void SqlBuilderCanCreateSimpleQueryTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
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

        #endregion DbExtensions.SqlBuilder Tests
    }
}