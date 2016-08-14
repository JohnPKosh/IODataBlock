using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            ExportDataWithNvarcharMax();
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
    }
}
