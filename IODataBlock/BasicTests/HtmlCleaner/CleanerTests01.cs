using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.EWS.HtmlUtil;
using System.IO;

namespace BasicTests.HtmlCleaner
{
    [TestClass]
    public class CleanerTests01
    {
        [TestMethod]
        public void TestMethod1()
        {
            var email = File.ReadAllText(@"C:\junk\email.html");
            var body = MailCleaner.CleanBody(email);
            Assert.IsNotNull(body);

        }

        [TestMethod]
        public void TestMethod2()
        {
            var email = File.ReadAllText(@"C:\junk\email2.html");
            var body = MailCleaner.CleanBody(email);
            Assert.IsNotNull(body);
        }


        [TestMethod]
        public void LoadWebPageTest()
        {
            var body = MailCleaner.LoadWebPage(@"http://www.cloudroute.com");
            Assert.IsNotNull(body);
        }
    }
}
