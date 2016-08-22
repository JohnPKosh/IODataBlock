using Data.DbClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Data
{
    /// <summary>
    /// Summary description for SqlBulkTests
    /// </summary>
    [TestClass]
    public class SqlBulkTests
    {
        private const string SqlServer = @".\EXP14";

        private const string SqlServerDatabase = @"RazorsightImportData";

        private static string SqlServerConnectionString
        {
            get
            {
                return Database.CreateSqlConnectionString(SqlServer, SqlServerDatabase);
            }
        }

        // TODO: implement CsvHelper library for all

        [TestMethod]
        public void BAN_VENDOR_LISTING_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[BAN_VENDOR_LISTING_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "BAN_VENDOR_LISTING_Import",
                    300,
                    @"C:\junk\Razorsight\samples\BAN_VENDOR_LISTING.csv",
                    @"C:\junk\Razorsight\samples\BAN_VENDOR_LISTING.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void BAN_VENDOR_LISTING_ImportTabSeperatedTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[BAN_VENDOR_LISTING_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "BAN_VENDOR_LISTING_Import",
                    300,
                    @"C:\junk\Razorsight\samples\BAN_VENDOR_LISTING.txt",
                    @"C:\junk\Razorsight\samples\BAN_VENDOR_LISTING_Tab.xml",
                    1000,
                    true,
                    "\t",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void INVOICE_MAIN_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[INVOICE_MAIN_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "INVOICE_MAIN_Import",
                    300,
                    @"C:\junk\Razorsight\samples\INVOICE_MAIN.csv",
                    @"C:\junk\Razorsight\samples\INVOICE_MAIN.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void CIRCUIT_DETAIL_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[CIRCUIT_DETAIL_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "CIRCUIT_DETAIL_Import",
                    300,
                    @"C:\junk\Razorsight\samples\CIRCUIT_DETAIL.csv",
                    @"C:\junk\Razorsight\samples\CIRCUIT_DETAIL.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void CIRCUIT_LOC_ATTR_DETAIL_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[CIRCUIT_LOC_ATTR_DETAIL_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "CIRCUIT_LOC_ATTR_DETAIL_Import",
                    300,
                    @"C:\junk\Razorsight\samples\CIRCUIT_LOC_ATTR_DETAIL.csv",
                    @"C:\junk\Razorsight\samples\CIRCUIT_LOC_ATTR_DETAIL.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void CIRCUIT_CHARGE_DETAIL_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[CIRCUIT_CHARGE_DETAIL_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "CIRCUIT_CHARGE_DETAIL_Import",
                    300,
                    @"C:\junk\Razorsight\samples\CIRCUIT_CHARGE_DETAIL.csv",
                    @"C:\junk\Razorsight\samples\CIRCUIT_CHARGE_DETAIL.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void TAX_SURCHARGE_LPC_DETAIL_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[TAX_SURCHARGE_LPC_DETAIL_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "TAX_SURCHARGE_LPC_DETAIL_Import",
                    300,
                    @"C:\junk\Razorsight\samples\TAX_SURCHARGE_LPC_DETAIL.csv",
                    @"C:\junk\Razorsight\samples\TAX_SURCHARGE_LPC_DETAIL.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void OCC_DETAIL_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[OCC_DETAIL_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "OCC_DETAIL_Import",
                    300,
                    @"C:\junk\Razorsight\samples\OCC_DETAIL.csv",
                    @"C:\junk\Razorsight\samples\OCC_DETAIL.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void USAGE_DETAIL_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[USAGE_DETAIL_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "USAGE_DETAIL_Import",
                    300,
                    @"C:\junk\Razorsight\samples\USAGE_DETAIL.csv",
                    @"C:\junk\Razorsight\samples\USAGE_DETAIL.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void CREDIT_ADJUSTMENT_DETAIL_ImportTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"DELETE FROM [RazorsightImportData].[dbo].[CREDIT_ADJUSTMENT_DETAIL_Import]");
                var rv = db.ImportSeperatedTxtToSql(
                    "CREDIT_ADJUSTMENT_DETAIL_Import",
                    300,
                    @"C:\junk\Razorsight\samples\CREDIT_ADJUSTMENT_DETAIL.csv",
                    @"C:\junk\Razorsight\samples\CREDIT_ADJUSTMENT_DETAIL.xml",
                    1000,
                    true,
                    ",",
                    @"""",
                    "",
                    true
                    );
                Assert.IsTrue(rv);
            }
        }

        [TestMethod]
        public void QueryToSqlServerBulkTest()
        {
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"TRUNCATE TABLE [dbo].[USAGE_DETAIL_Import_copy]");
                db.QueryToSqlServerBulk(
                    "SELECT *  FROM [RazorsightImportData].[dbo].[USAGE_DETAIL_Import]",
                    SqlServerConnectionString,
                    "USAGE_DETAIL_Import_copy",
                    300,
                    1000,
                    300,
                    true
                    );
            }
        }

        //TODO: Add real connectionstring to dbrate readdata
        private const string NpgsqlConnectionString = "********";

        [TestMethod]
        public void PostgreSqlQueryToSqlServerBulkTest()
        {
            // flush import table
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"TRUNCATE TABLE [dbo].[ratetable_cnt]");
            }

            // copy data from PostgreSql
            using (var db = Database.OpenConnectionString(NpgsqlConnectionString, "Npgsql"))
            {
                db.QueryToSqlServerBulk(
                    @"SELECT productid, count(*) as cnt
FROM (
	SELECT productid, zonedestinationcode
	FROM public.ratetable
	WHERE to_char(current_timestamp,'YYYY-MM-DD') BETWEEN dateeff AND dateexp
	GROUP BY productid, zonedestinationcode
) as a
GROUP BY productid
ORDER BY cnt, productid",
                    SqlServerConnectionString,
                    "ratetable_cnt",
                    300,
                    1000,
                    300,
                    true
                    );
            }
        }

        [TestMethod]
        public void PostgreSqlQueryToSqlServerBulkTest2()
        {
            // flush import table
            using (var db = Database.OpenConnectionString(SqlServerConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(@"TRUNCATE TABLE [dbo].[trunkgroups]");
            }

            // copy data from PostgreSql
            using (var db = Database.OpenConnectionString(NpgsqlConnectionString, "Npgsql"))
            {
                db.QueryToSqlServerBulk(
                    @"SELECT trunkgroupid, productid, productpriority, sourcerating, penaltyrate,
       rateperiod, initialperiod, initialrate, initialratetype, objid,
       recid, ratio, forcelrn, created_by, created_at, updated_by, updated_at,
       lcrrtype
  FROM trunkgroups;",
                    SqlServerConnectionString,
                    "trunkgroups",
                    300,
                    1000,
                    300,
                    true
                    );
            }
        }
    }
}

// 99,3124220928116,AT&T Ameritech CHG (C),,ATT Retail,YOY,,,,N,,3124220928,CYC,N,,,,C-0004893,PO BOX 8100 | Aurora, IL 60507-8100,2015-05-14 21:34:18.000,InActive,,,,,,maxelrod,2012-04-20 11:21:32.000,9999