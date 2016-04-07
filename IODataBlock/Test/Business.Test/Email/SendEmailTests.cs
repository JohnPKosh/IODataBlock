using System;
using System.Collections.Generic;
using System.Net.Mail;
using Business.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.EWS.Mail;
using Microsoft.Exchange.WebServices.Data;


namespace Business.Test.Email
{
    [TestClass]
    public class SendEmailTests
    {

        public SendEmailTests()
        {
            var configMgr = new ConfigMgr();
            Url = configMgr.GetAppSetting("ews:url");
            Email = configMgr.GetAppSetting("ews:email");
            Password = configMgr.GetAppSetting("ews:password");
        }

        private string Url { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion




        [TestMethod]
        public void TestMethod1()
        {
            var mailer = new ExchangeEmailSender(Url, Email, Password);
            try
            {
                mailer.Send("test", "test", new[] { "jkosh@cloudroute.com" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mailer = new ExchangeEmailSender(Url, Email, Password);
            try
            {
                MessageBody msg = new MessageBody(BodyType.Text, "test");
                var toAddresses = new List<EmailAddress>();
                var emailto = new EmailAddress {Address = "jkosh@cloudroute.com"};
                toAddresses.Add(emailto);
                mailer.Send("test", msg, toAddresses);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            var mailer = new ExchangeEmailSender(Url, Email, Password);
            try
            {
                MessageBody msg = new MessageBody(BodyType.Text, "Test TempId Email message");
                var toAddresses = new List<EmailAddress>();
                var emailto = new EmailAddress { Address = "jkosh@cloudroute.com" };
                toAddresses.Add(emailto);
                mailer.Send("Test TempId Email message", msg, toAddresses);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [TestMethod]
        public void TestMethod4()
        {
            var mailer = new ExchangeEmailSender(Url, Email, Password);
            try
            {
                MessageBody msg = new MessageBody(BodyType.Text, "Test TempId Email message");
                var toAddresses = new List<EmailAddress>();
                var emailto = new EmailAddress { Address = "jkosh@cloudroute.com" };
                toAddresses.Add(emailto);
                mailer.SendAtSpecificTime("Test TempId Email message", msg, toAddresses, DateTime.Now.AddMinutes(5));
            }
            catch (Exception)
            {
                throw;
            }
        }

        //EmailAddress
    }
}
