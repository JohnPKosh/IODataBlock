using Business.Common.Configuration;
using Data.DbClient.BulkCopy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BasicTests.Data
{
    [TestClass]
    public class SqlBulkCopyTests
    {
        public SqlBulkCopyTests()
        {
            _dnssearchConnectionString = _configMgr.GetConnectionString("dnssearch");
        }

        private readonly string _dnssearchConnectionString;
        private readonly ConfigMgr _configMgr = new ConfigMgr();

        [TestMethod]
        public void TestMethod1()
        {
            var con = @"Integrated Security=True; Initial Catalog=TestData; Data Source=.\EXP14;";
            try
            {
                SqlBulkCopyUtility.BulkInsert(DnInsertBuilder.GetSample1(), con, "DnActiveRecords");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var columns = new Dictionary<string, string>();
            columns.Add("DomainName", "DomainName");
            columns.Add("Status", "Status");
            columns.Add("CreatedDate", "CreatedDate");

            var con = @"Integrated Security=True; Initial Catalog=TestData; Data Source=.\EXP14;";
            try
            {
                SqlBulkCopyUtility.BulkInsert(DnInsertBuilder.GetSample1(), con, "DnActiveRecords", sqlBulkCopyColumnMappings: columns);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void TestBulkCopy()
        {
            //SqlServerBulkCopy
            var sourcecon = @"Integrated Security=True; Initial Catalog=DnsSearch; Data Source=.\EXP14;";
            var destcon = _dnssearchConnectionString;
            try
            {
                SqlBulkCopyUtility bc = new SqlBulkCopyUtility();
                bc.SqlServerBulkCopy(sourcecon, destcon, @"SELECT * FROM SystemDebugLog", @"SystemDebugLog", 1000, 300, true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void BulkCopyDnsSearchData()
        {
            //BulkInsertTable("SystemDebugLog");
            //BulkInsertTable("SystemErrorLog");
            //BulkInsertTable("SystemInfoLog");

            //BulkInsertTable("DnActiveRecords");
            //BulkInsertTable("DnInactiveRecords");
            //BulkInsertTable("DnSearchResults");
            //BulkInsertTable("DnSearchResultsHistory");

            //BulkInsertTable("AutoDiscoverAResults");
            //BulkInsertTable("GoogleMxResults");
            //BulkInsertTable("MsMxResults");
            //BulkInsertTable("MxResults");
            BulkInsertTable("SfbFederatedSrvRecords");
            BulkInsertTable("SfbSrvResults");
            BulkInsertTable("SipAResults");
            BulkInsertTable("SpfTxtResults");
        }

        private void BulkInsertTable(string tableName)
        {
            var sourcecon = @"Integrated Security=True; Initial Catalog=DnsSearch; Data Source=.\EXP14;";
            var destcon = _dnssearchConnectionString;
            try
            {
                SqlBulkCopyUtility bc = new SqlBulkCopyUtility();
                bc.SqlServerBulkCopy(sourcecon, destcon, $"SELECT * FROM {tableName}", tableName, 5000, 600, true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}