using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Data.MsAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests.Data
{
    [TestClass]
    public class MsAccessTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //ExportAndZipData();
            //ExportDataWithNvarcharMax();
            ExportLinkedInCompaniesWithNvarcharMax();
        }

        private static void ExportData()
        {
            using (var dbf = new MsAccessFactory(new FileInfo(@"C:\Users\jkosh\Documents\TargetListExport.accdb")))
            {
                try
                {

                    var tables = new List<string> {"TargetListExport"};
                    //tables.Add("LinkedInCompanyScrapeImport3");


                    var isfirst = true;
                    foreach (var table in tables)
                    {
                        dbf.QuerySqlServerToAccess(

                        @".\EXP14",
                        "TestData",
                        $"SELECT TOP 1000 * FROM {table}",
                        CommandType.Text,
                        table,
                        "servermgr",
                        "defr3sTu",
                        isfirst,
                        true,
                        true,
                        false,
                        null,
                        null,
                        600000,
                        1200
                        );
                        isfirst = false;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private static void ExportAndZipData()
        {
            using (var dbf = new MsAccessFactory(new FileInfo(@"C:\Users\jkosh\Documents\TargetListExport.accdb")))
            {
                try
                {

                    var tables = new List<string> { "TargetListExport" };
                    //tables.Add("LinkedInCompanyScrapeImport3");


                    var isfirst = true;
                    foreach (var table in tables)
                    {
                        dbf.QuerySqlServerToAccess(

                        @".\EXP14",
                        "TestData",
                        $"SELECT TOP 1000 * FROM {table}",
                        CommandType.Text,
                        table,
                        "servermgr",
                        "defr3sTu",
                        isfirst,
                        true,
                        true,
                        true,
                        null,
                        null,
                        600000,
                        1200
                        );
                        isfirst = false;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        private static void ExportDataWithNvarcharMax()
        {
            using (var dbf = new MsAccessFactory(new FileInfo(@"C:\Users\jkosh\Documents\LinkedInCompanyScrapeImport3.accdb")))
            {
                try
                {

                    var tables = new List<string> { "LinkedInCompanyScrapeImport3" };
                    //tables.Add("LinkedInCompanyScrapeImport3");


                    var isfirst = true;
                    foreach (var table in tables)
                    {
                        dbf.QuerySqlServerToAccess(

                        @".\EXP14",
                        "DnsSearch",
                        $"SELECT TOP 1000 * FROM {table}",
                        CommandType.Text,
                        table,
                        "servermgr",
                        "defr3sTu",
                        isfirst,
                        true,
                        true,
                        false,
                        null,
                        null,
                        600000,
                        1200
                        );
                        isfirst = false;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        private static void ExportLinkedInCompaniesWithNvarcharMax()
        {
            using (var dbf = new MsAccessFactory(new FileInfo(@"C:\Users\jkosh\Documents\LinkedInCompany.accdb")))
            {
                try
                {

                    var tables = new List<string> { "LinkedInCompany" };
                    //tables.Add("LinkedInCompanyScrapeImport3");


                    var isfirst = true;
                    foreach (var table in tables)
                    {
                        dbf.QuerySqlServerToAccess(

                        @".\EXP14",
                        "DnsSearch",
                        $"SELECT * FROM {table}",
                        CommandType.Text,
                        table,
                        "servermgr",
                        "defr3sTu",
                        isfirst,
                        true,
                        true,
                        false,
                        null,
                        null,
                        600000,
                        1200
                        );
                        isfirst = false;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void TestQuery()
        {
            var accesscon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\jkosh\Documents\LinkedInCompany.accdb"";Persist Security Info=False;";
            using (var accessdb = AccessDatabase.OpenConnectionString(accesscon, "System.Data.OleDb"))
            {
                var data = accessdb.Query(@"SELECT TOP 100 * FROM LinkedInCompany WHERE CreatedDate < @0", 60, DateTime.Now).ToList();
                Assert.IsTrue(data.Any());
            }
        }
    }
}
