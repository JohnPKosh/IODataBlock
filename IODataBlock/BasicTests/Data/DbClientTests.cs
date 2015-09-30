﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Data.DbClient;
using Data.DbClient.Extensions;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BasicTests.Data
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
AND [TABLE_NAME] LIKE @1
ORDER BY [ORDINAL_POSITION]
";

            #endregion sql

            var myparams = new List<object>() {"LERG%","%7%"};
            var data = Database.Query(SqlServerConnectionString, "System.Data.SqlClient", sql, myparams);
            if (!data.Any())
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void TestStaticDatabaseQuery3()
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

            var con = new SqlConnection(SqlServerConnectionString);
            var data = Database.Query(con, sql, 120, "LERG%");
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
AND [TABLE_NAME] LIKE @1
ORDER BY [ORDINAL_POSITION]
";

                #endregion sql

                //var data = db.Query(sql, 120, "Data%");
                var data = db.Query(sql, 120, "LERG%", "%7%");
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }


        [TestMethod]
        public void QuerySqlServerSchemaTest2()
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
AND [TABLE_NAME] LIKE @1
ORDER BY [ORDINAL_POSITION]
";

                #endregion sql

                var myparams = new List<object>() { "LERG%", "%7%" };
                var data = db.Query(sql, myparams);
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

        #endregion SQL Server Tests

    }
}