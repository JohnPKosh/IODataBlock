using System;
using System.IO;
using Business.Common.Extensions;
using Business.Common.IO;
//using Business.Utilities.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.IO
{
    [TestClass]
    public class DirectoryEntryTests
    {
        [TestMethod]
        public void TryToReadNonExistantDirectory()
        {
            var app_data = IOUtility.AppDataFolderPath;
            var di = new DirectoryInfo(@"c:\junk2");
            var de = new DirectoryEntry(di);

            try
            {
                var dijson = di.ToJsonString(true);
                Assert.IsNotNull(dijson);

                var json = de.ToJsonString(true);
                Assert.IsNotNull(json);

                //var createTime = de.CreationTime;
                //var ro = de.ReadOnly;
            }
            catch (Exception)
            {
                var exceptions = de.EntryReadErrors.Exceptions;
                throw;
            }
        }

        [TestMethod]
        public void TryToReadExistantDirectory()
        {
            var di = new DirectoryInfo(@"c:\junk");
            var de = new DirectoryEntry(di);

            try
            {
                var dijson = di.ToJsonString(true);
                var json = de.ToJsonString(true);
                var createTime = de.CreationTime;
            }
            catch (Exception)
            {
                var exceptions = de.EntryReadErrors.Exceptions;
                throw;
            }
        }
    }
}