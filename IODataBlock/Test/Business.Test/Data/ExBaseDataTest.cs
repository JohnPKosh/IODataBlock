using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Data.DbClient;
//using ExBaseDataUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Database = ExBaseData.Database;

namespace ExBaseTests
{
    [TestClass]
    public class ExBaseDataTest
    {
        #region Npgsql Tests

        [TestMethod]
        public void ConnectNpgsqlTest()
        {
            //// Server=10.128.28.21;Port=5432;User Id=rater;Database=dbrate;
            //var constr = "Server=10.128.28.21;Port=5432;User Id=rater;Database=dbrate;";
            //using (Database db = Database.OpenConnectionString(constr, "Npgsql"))
            //{
            //    db.Connection.Open();
            //    if(db.Connection.State != ConnectionState.Open)Assert.Fail();
            //}

            var constr = "Server=10.128.28.92;Port=5432;User Id=qixlrn;Database=qixlrn;";
            using (Database db = Database.OpenConnectionString(constr, "Npgsql"))
            {
                db.Connection.Open();
                if (db.Connection.State != ConnectionState.Open) Assert.Fail();
            }
        }

        [TestMethod]
        public void SelectSchemaTablesNpgsqlTest()
        {
            var constr = "Server=10.128.28.92;Port=5432;User Id=qixlrn;Database=qixlrn;";
            using (Database db = Database.OpenConnectionString(constr, "Npgsql"))
            {
                var data = db.Query(@"SELECT * FROM information_schema.tables WHERE table_schema='public' AND table_type = 'BASE TABLE'", 120);
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void SelectSchemaTablesNpgsql_DbrateLabTest()
        {
            var constr = "Server=172.16.5.43;Port=5432;User Id=rater;Database=dbrate;";
            using (Database db = Database.OpenConnectionString(constr, "Npgsql"))
            {
                var data = db.Query(@"SELECT * FROM information_schema.tables WHERE table_schema='public' AND table_type = 'BASE TABLE'", 120);
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void SelectSchemaTablesNpgsql_RatesyscontrolLabTest()
        {
            var constr = "Server=172.16.5.43;Port=5432;User Id=rater;Database=ratesyscontrol;";
            using (Database db = Database.OpenConnectionString(constr, "Npgsql"))
            {
                try
                {
                    var data = db.Query(@"SELECT * FROM information_schema.tables WHERE table_schema='public' AND table_type = 'BASE TABLE'", 120);
                    if (!data.Any())
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [TestMethod]
        public void SelectSchemaTablesWithParamsNpgsqlTest()
        {
            var constr = "Server=10.128.28.92;Port=5432;User Id=qixlrn;Database=qixlrn;";
            using (Database db = Database.OpenConnectionString(constr, "Npgsql"))
            {
                var data = db.Query(@"SELECT * FROM information_schema.tables WHERE table_schema='public' AND table_type = 'BASE TABLE' AND (table_name LIKE @0 OR table_name LIKE @1)", 120, "tn2lrn200%", "tn2lrn216%");
                if (!data.Any())
                {
                    Assert.Fail();
                }
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

            var constr = "Server=172.16.5.110;Port=5432;User Id=qixlrn;Database=qixlrn;";
            using (Database db = Database.OpenConnectionString(constr, "Npgsql"))
            {
                var data = db.Query(sql, 120);
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        #endregion Npgsql Tests

        #region SQL Server Tests

        [TestMethod]
        public void QuerySqlServerSchemaTest()
        {
            var constr = Database.CreateSqlConnectionString("CLEHBSQL0301", "FoundationIntranet", "servermgr", "defr3sTu");
            using (Database db = Database.OpenConnectionString(constr, "System.Data.SqlClient"))
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

                var data = db.Query(sql, 120, "LCR%");
                if (!data.Any())
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void CopyToMySqlTest()
        {
            var constr = Database.CreateSqlConnectionString("CLEHBSQL0301", "CDRRECORD", "servermgr", "defr3sTu");
            using (Database db = Database.OpenConnectionString(constr, "System.Data.SqlClient"))
            {
                #region sql

                var sql = @"
SELECT TOP 74117 [did]
FROM [dbo].[didinventory]
where [did] > '4842026062'
";

                #endregion sql

                const string insert = @"INSERT INTO `sms_numbers`(`tn`,`cust_id`,`route_label`,`email_fwd`,`email_src`) VALUES(@0,@1,@2,@3,@4) ";

                using (Database mysqldb = Database.OpenConnectionString(@"Server=172.16.12.21;Database=SMS;Uid=bvoss;Pwd=T3@RdR0p$@r3F@llIn6;", "MySql.Data.MySqlClient"))
                {
                    foreach (var r in db.Query(sql, 1200))
                    {
                        mysqldb.Execute(insert, 60, r.did, "BRANDON", "BRANDON_RL", "", "");
                    }
                }
            }
        }

        #endregion SQL Server Tests

        #region MySql Tests

        [TestMethod]
        public void SugarCRM_GetSugarAccountsAll()
        {
            var data = GetSugarAccountsAll().ToList();
            Assert.IsTrue(data.Count > 0);
        }

        [TestMethod]
        public void SugarCRM_GetSugarAccountsWhere()
        {
            var data = GetSugarAccountsWhere("1=1").ToList();
            Assert.IsTrue(data.Count > 0);
        }

        [TestMethod]
        public void SugarCRM_GetSugarAccountsActive()
        {
            var data = GetSugarAccountsActive().ToList();
            Assert.IsTrue(data.Count > 0);
        }

        [TestMethod]
        public void SugarCRM_GetSugarAccountsActiveWhere()
        {
            var data = GetSugarAccountsActiveWhere("1=1").ToList();
            Assert.IsTrue(data.Count > 0);
        }

        public IEnumerable<dynamic> GetSugarAccountsAll()
        {
            using (var db = Database.OpenConnectionString(@"Server=10.64.1.206;Database=sugarcrm6;Uid=jkosh;Pwd=JWMYUKchMGE4cnbe;", "MySql.Data.MySqlClient"))
            {
                db.Connection.Open();

                #region sql

                var sql = @"

SELECT
    cast(a.`id` as char(36)) as `id`,
    `name`,
    `date_entered`,
    a.`date_modified`,
    cast(`modified_user_id` as char(36)) as `modified_user_id`,
    cast(`created_by` as char(36)) as `created_by`,
    `description`,
    a.`deleted`,
    cast(`assigned_user_id` as char(36)) as `assigned_user_id`,
    `account_type`,
    `industry`,
    `annual_revenue`,
    `phone_fax`,
    `billing_address_street`,
    `billing_address_city`,
    `billing_address_state`,
    `billing_address_postalcode`,
    `billing_address_country`,
    `rating`,
    `phone_office`,
    `phone_alternate`,
    `website`,
    `ownership`,
    `employees`,
    `ticker_symbol`,
    `shipping_address_street`,
    `shipping_address_city`,
    `shipping_address_state`,
    `shipping_address_postalcode`,
    `shipping_address_country`,
    cast(`parent_id` as char(36)) as `parent_id`,
    `sic_code`,
    cast(`campaign_id` as char(36)) as `campaign_id`,
    b.`accountid_c`,
    b.`account_status_c`,
    b.`cadebillaccountid_c`,
    cast(c.`pr_prospects_accountspr_prospects_ida` as char(36)) as `pr_prospects_id`
FROM `accounts` as a
JOIN `accounts_cstm` as b
ON a.`id` = b.`id_c`
LEFT JOIN (
        SELECT a.`pr_prospects_accountspr_prospects_ida`,
        b.`pr_prospects_accountsaccounts_idb`
        FROM `pr_prospects_accounts_c` as a
        JOIN(
            SELECT
                MAX(`id`) as `id`,
                `pr_prospects_accountsaccounts_idb`
            FROM `pr_prospects_accounts_c`
            WHERE `deleted` <> 1
            GROUP BY `pr_prospects_accountsaccounts_idb`
        ) as b
        ON a.`id` = b.`id`
) as c
ON a.`id` = c.`pr_prospects_accountsaccounts_idb`

";

                #endregion sql

                return db.Query(sql, 60);
            }
        }

        public IEnumerable<dynamic> GetSugarAccountsWhere(String WhereSql)
        {
            using (var db = Database.OpenConnectionString(@"Server=10.64.1.206;Database=sugarcrm6;Uid=jkosh;Pwd=JWMYUKchMGE4cnbe;", "MySql.Data.MySqlClient"))
            {
                db.Connection.Open();

                #region sql

                var sql = @"
SELECT
    cast(a.`id` as char(36)) as `id`,
    `name`,
    `date_entered`,
    a.`date_modified`,
    cast(`modified_user_id` as char(36)) as `modified_user_id`,
    cast(`created_by` as char(36)) as `created_by`,
    `description`,
    a.`deleted`,
    cast(`assigned_user_id` as char(36)) as `assigned_user_id`,
    `account_type`,
    `industry`,
    `annual_revenue`,
    `phone_fax`,
    `billing_address_street`,
    `billing_address_city`,
    `billing_address_state`,
    `billing_address_postalcode`,
    `billing_address_country`,
    `rating`,
    `phone_office`,
    `phone_alternate`,
    `website`,
    `ownership`,
    `employees`,
    `ticker_symbol`,
    `shipping_address_street`,
    `shipping_address_city`,
    `shipping_address_state`,
    `shipping_address_postalcode`,
    `shipping_address_country`,
    cast(`parent_id` as char(36)) as `parent_id`,
    `sic_code`,
    cast(`campaign_id` as char(36)) as `campaign_id`,
    b.`accountid_c`,
    b.`account_status_c`,
    b.`cadebillaccountid_c`,
    cast(c.`pr_prospects_accountspr_prospects_ida` as char(36)) as `pr_prospects_id`
FROM `accounts` as a
JOIN `accounts_cstm` as b
ON a.`id` = b.`id_c`
LEFT JOIN (
        SELECT a.`pr_prospects_accountspr_prospects_ida`,
        b.`pr_prospects_accountsaccounts_idb`
        FROM `pr_prospects_accounts_c` as a
        JOIN(
            SELECT
                MAX(`id`) as `id`,
                `pr_prospects_accountsaccounts_idb`
            FROM `pr_prospects_accounts_c`
            WHERE `deleted` <> 1
            GROUP BY `pr_prospects_accountsaccounts_idb`
        ) as b
        ON a.`id` = b.`id`
) as c
ON a.`id` = c.`pr_prospects_accountsaccounts_idb`
WHERE $(WhereSql)".Replace("$(WhereSql)", WhereSql);

                #endregion sql

                return db.Query(sql, 60);
            }
        }

        public IEnumerable<dynamic> GetSugarAccountsActive()
        {
            using (var db = Database.OpenConnectionString(@"Server=10.64.1.206;Database=sugarcrm6;Uid=jkosh;Pwd=JWMYUKchMGE4cnbe;", "MySql.Data.MySqlClient"))
            {
                db.Connection.Open();

                #region sql

                var sql = @"

SELECT
    cast(a.`id` as char(36)) as `id`,
    `name`,
    `date_entered`,
    a.`date_modified`,
    cast(`modified_user_id` as char(36)) as `modified_user_id`,
    cast(`created_by` as char(36)) as `created_by`,
    `description`,
    a.`deleted`,
    cast(`assigned_user_id` as char(36)) as `assigned_user_id`,
    `account_type`,
    `industry`,
    `annual_revenue`,
    `phone_fax`,
    `billing_address_street`,
    `billing_address_city`,
    `billing_address_state`,
    `billing_address_postalcode`,
    `billing_address_country`,
    `rating`,
    `phone_office`,
    `phone_alternate`,
    `website`,
    `ownership`,
    `employees`,
    `ticker_symbol`,
    `shipping_address_street`,
    `shipping_address_city`,
    `shipping_address_state`,
    `shipping_address_postalcode`,
    `shipping_address_country`,
    cast(`parent_id` as char(36)) as `parent_id`,
    `sic_code`,
    cast(`campaign_id` as char(36)) as `campaign_id`,
    b.`accountid_c`,
    b.`account_status_c`,
    b.`cadebillaccountid_c`,
    cast(c.`pr_prospects_accountspr_prospects_ida` as char(36)) as `pr_prospects_id`
FROM `accounts` as a
JOIN `accounts_cstm` as b
ON a.`id` = b.`id_c`
LEFT JOIN (
        SELECT a.`pr_prospects_accountspr_prospects_ida`,
        b.`pr_prospects_accountsaccounts_idb`
        FROM `pr_prospects_accounts_c` as a
        JOIN(
            SELECT
                MAX(`id`) as `id`,
                `pr_prospects_accountsaccounts_idb`
            FROM `pr_prospects_accounts_c`
            WHERE `deleted` <> 1
            GROUP BY `pr_prospects_accountsaccounts_idb`
        ) as b
        ON a.`id` = b.`id`
) as c
ON a.`id` = c.`pr_prospects_accountsaccounts_idb`
WHERE 1=1
AND a.`deleted` <> 1

";

                #endregion sql

                return db.Query(sql, 60);
            }
        }

        public IEnumerable<dynamic> GetSugarAccountsActiveWhere(String WhereSql)
        {
            using (var db = Database.OpenConnectionString(@"Server=10.64.1.206;Database=sugarcrm6;Uid=jkosh;Pwd=JWMYUKchMGE4cnbe;", "MySql.Data.MySqlClient"))
            {
                db.Connection.Open();

                #region sql

                var sql = @"

SELECT
    cast(a.`id` as char(36)) as `id`,
    `name`,
    `date_entered`,
    a.`date_modified`,
    cast(`modified_user_id` as char(36)) as `modified_user_id`,
    cast(`created_by` as char(36)) as `created_by`,
    `description`,
    a.`deleted`,
    cast(`assigned_user_id` as char(36)) as `assigned_user_id`,
    `account_type`,
    `industry`,
    `annual_revenue`,
    `phone_fax`,
    `billing_address_street`,
    `billing_address_city`,
    `billing_address_state`,
    `billing_address_postalcode`,
    `billing_address_country`,
    `rating`,
    `phone_office`,
    `phone_alternate`,
    `website`,
    `ownership`,
    `employees`,
    `ticker_symbol`,
    `shipping_address_street`,
    `shipping_address_city`,
    `shipping_address_state`,
    `shipping_address_postalcode`,
    `shipping_address_country`,
    cast(`parent_id` as char(36)) as `parent_id`,
    `sic_code`,
    cast(`campaign_id` as char(36)) as `campaign_id`,
    b.`accountid_c`,
    b.`account_status_c`,
    b.`cadebillaccountid_c`,
    cast(c.`pr_prospects_accountspr_prospects_ida` as char(36)) as `pr_prospects_id`
FROM `accounts` as a
JOIN `accounts_cstm` as b
ON a.`id` = b.`id_c`
LEFT JOIN (
        SELECT a.`pr_prospects_accountspr_prospects_ida`,
        b.`pr_prospects_accountsaccounts_idb`
        FROM `pr_prospects_accounts_c` as a
        JOIN(
            SELECT
                MAX(`id`) as `id`,
                `pr_prospects_accountsaccounts_idb`
            FROM `pr_prospects_accounts_c`
            WHERE `deleted` <> 1
            GROUP BY `pr_prospects_accountsaccounts_idb`
        ) as b
        ON a.`id` = b.`id`
) as c
ON a.`id` = c.`pr_prospects_accountsaccounts_idb`
WHERE a.`deleted` <> 1
AND $(WhereSql)".Replace("$(WhereSql)", WhereSql);

                #endregion sql

                return db.Query(sql, 60);
            }
        }

        [TestMethod]
        public void SugarCRM_GetSugarProspectsAll()
        {
            var data = GetSugarProspectsAll().ToList();
            Assert.IsTrue(data.Count > 0);
        }

        public IEnumerable<dynamic> GetSugarProspectsAll()
        {
            using (var db = Database.OpenConnectionString(@"Server=10.64.1.206;Database=sugarcrm6;Uid=jkosh;Pwd=JWMYUKchMGE4cnbe;", "MySql.Data.MySqlClient"))
            {
                db.Connection.Open();

                #region sql

                var sql = @"
SELECT
    cast(`id` as char(36)) as `id`,
    `name`,
    `date_entered`,
    `date_modified`,
    cast(`modified_user_id` as char(36)) as `modified_user_id`,
    cast(`created_by` as char(36)) as `created_by`,
    `description`,
    `deleted`,
    cast(`assigned_user_id` as char(36)) as `assigned_user_id`,
    `pr_prospects_type`,
    `industry`,
    `annual_revenue`,
    `phone_fax`,
    `billing_address_street`,
    `billing_address_city`,
    `billing_address_state`,
    `billing_address_postalcode`,
    `billing_address_country`,
    `rating`,
    `phone_office`,
    `phone_alternate`,
    `website`,
    `ownership`,
    `employees`,
    `ticker_symbol`,
    `shipping_address_street`,
    `shipping_address_city`,
    `shipping_address_state`,
    `shipping_address_postalcode`,
    `shipping_address_country`,
    `salesstage`,
    `sales_stage`,
    `status`
FROM
    `pr_prospects`;
";

                #endregion sql

                return db.Query(sql, 60);
            }
        }

        [TestMethod]
        public void CanGetSMSRouteLabelsAll()
        {
            var data = GetSMSRouteLabelsAll().ToList();
            Assert.IsTrue(data.Count > 0);
        }

        public IEnumerable<dynamic> GetSMSRouteLabelsAll()
        {
            using (var db = Database.OpenConnectionString(@"Server=172.16.12.21;Database=SMS;Uid=bvoss;Pwd=T3@RdR0p$@r3F@llIn6;", "MySql.Data.MySqlClient"))
            {
                db.Connection.Open();

                #region sql

                var sql = @"SELECT * FROM SMS.route_label;";

                #endregion sql

                return db.Query(sql, 60);
            }
        }

        #endregion MySql Tests

        #region SQLite

        [TestMethod]
        public void SqliteSimpleTest()
        {
            //var constr = DataExtensionBase.GetSqlConnectionString("CLEHBSQL0301", "FoundationIntranet", "servermgr", "defr3sTu");
            var constr = "Data Source=sqliteTest.sl3;";

            using (var db = Database.OpenConnectionString(constr, "System.Data.SQLite"))
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
            var constr = "Data Source=sqliteTest.sl3;";

            using (var db = Database.OpenConnectionString(constr, "System.Data.SQLite"))
            {
                //// Execute query
                //foreach (var a in db.Query("SELECT * FROM `ImportLog`  ORDER BY `rowid` ASC LIMIT 0, 50000;"))
                //{
                //    if (a.LogData != null)
                //    {
                //        // do somethin
                //    }
                //}

                db.Connection.Open();
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

    }
}