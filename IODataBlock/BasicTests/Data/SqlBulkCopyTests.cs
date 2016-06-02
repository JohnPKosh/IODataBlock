using System;
using System.Collections.Generic;
using Data.DbClient.BulkCopy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests.Data
{
    [TestClass]
    public class SqlBulkCopyTests
    {
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
                SqlBulkCopyUtility.BulkInsert(DnInsertBuilder.GetSample1(), con, "DnActiveRecords",sqlBulkCopyColumnMappings: columns);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
