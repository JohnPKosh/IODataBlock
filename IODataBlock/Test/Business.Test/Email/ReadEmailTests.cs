using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.EWS.Mail;
using Microsoft.Exchange.WebServices.Data;

namespace Business.Test.Email
{
    [TestClass]
    public class ReadEmailTests
    {
        public ReadEmailTests()
        {
            var configMgr = new ConfigMgr();
            Url = configMgr.GetAppSetting("ews:url");
            Email = configMgr.GetAppSetting("ews:email");
            Password = configMgr.GetAppSetting("ews:password");

            FromEmail = "jkosh@broadvox.com";
        }

        private string Url { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }

        private string FromEmail { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var mailer = new ExchangeEmailSearch(Url, Email, Password);
            try
            {
                var results = mailer.SearchInboxContainsSubstring("Update CROSS bundle on NS prod", 50);
                foreach (Item item in results)
                {
                    item.Load();
                    var subject = item.Subject;
                    var body = item.Body;
                    Assert.IsNotNull(body);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mailer = new ExchangeEmailSearch(Url, Email, Password);
            try
            {
                var results = mailer.SearchInboxContainsSubstring("Test TempId Email message", 50);
                foreach (Item item in results)
                {
                    var body = GetExtPropOrEmpty(item, "TempId"); // must do this first before reloading item data below.
                    item.Load();
                    var subject = item.Subject;
                    
                    Assert.IsNotNull(body);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [TestMethod]
        public void TestMethod3()
        {
            var mailer = new ExchangeEmailSearch(Url, Email, Password);
            try
            {
                var results = mailer.SearchInboxUnreadFrom(FromEmail, 50);
                foreach (Item item in results)
                {
                    var body = GetExtPropOrEmpty(item, "TempId"); // must do this first before reloading item data below.
                    item.Load();
                    var subject = item.Subject;

                    Assert.IsNotNull(body);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            var mailer = new ExchangeEmailSearch(Url, Email, Password);
            try
            {
                var results = mailer.SearchInboxFromAddressesToday(FromEmail, 2);
                foreach (var item in results)
                {
                    //var body = GetExtPropOrEmpty(item, "TempId"); // must do this first before reloading item data below.
                    item.Load();
                    //var subject = item.Subject;

                    //Assert.IsNotNull(body);
                    var id = item.Id;
                    var from = item.From;
                    var sender = item.Sender;
                    var received = item.DateTimeReceived;
                    Assert.IsNotNull(id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [TestMethod]
        public void TestMethod5()
        {
            var mailer = new ExchangeEmailSearch(Url, Email, Password);
            try
            {
                var fromAddresses = new HashSet<Tuple<string, string>>();
                var results = mailer.SearchInboxAllToday(15);
                foreach (var item in results)
                {
                    //var body = GetExtPropOrEmpty(item, "TempId"); // must do this first before reloading item data below.
                    //item.Load();
                    //var subject = item.Subject;

                    //Assert.IsNotNull(body);
                    //var id = item.Id;
                    //var from = item.From;
                    //var sender = item.Sender;
                    //var received = item.DateTimeReceived;
                    //Assert.IsNotNull(id);
                    fromAddresses.Add(new Tuple<string, string>(item.From.Address, item.Sender.Address));
                }
                Assert.IsTrue(fromAddresses.Count > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetExtPropOrEmpty(Item item, string propertyName)
        {
            var result = item.ExtendedProperties.FirstOrDefault(extendedProperty => extendedProperty.PropertyDefinition.Name == propertyName);
            return result != null ? result.Value.ToString() : string.Empty;
            //foreach (var extendedProperty in item.ExtendedProperties.Where(extendedProperty => extendedProperty.PropertyDefinition.Name == propertyName))
            //{
            //    result = extendedProperty.Value.ToString();
            //}
        }

    }
}
