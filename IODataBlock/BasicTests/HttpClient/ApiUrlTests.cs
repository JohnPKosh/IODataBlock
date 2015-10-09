using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Business.Common.Extensions;
using Business.HttpClient.Navigation;
using Flurl;
using Flurl.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BasicTests.HttpClient
{
    /// <summary>
    /// Summary description for ApiUrlTests
    /// </summary>
    [TestClass]
    public class ApiUrlTests
    {
        public ApiUrlTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
            //https://api.hubapi.com/contacts/v1/contact/vid/41/profile?hapikey=4c8d196f-7105-4c06-a17c-9fb83e6dcd10
            var url = new ApiUrl
            {
                Root = @"https://api.hubapi.com",
                PathSegments = new[] {"contacts", "v1", "contact", "vid", "41", "profile"},
                QueryParams = new Dictionary<string, object>() {{"hapikey", "4c8d196f-7105-4c06-a17c-9fb83e6dcd10"}}
            };

            string _out = url;
            Assert.IsNotNull(_out);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //https://api.hubapi.com/contacts/v1/contact/vid/41/profile?hapikey=4c8d196f-7105-4c06-a17c-9fb83e6dcd10
            var url = new ApiUrl
            {
                Root = @"https://api.hubapi.com",
                //PathSegments = new[] { "contacts", "v1", "contact", "vid", "41", "profile" },
                QueryParams = new Dictionary<string, object>() { { "hapikey", "4c8d196f-7105-4c06-a17c-9fb83e6dcd10" } }
            };

            string _out = url;
            Assert.IsNotNull(_out);
        }

        [TestMethod]
        public void TestMethod3()
        {
            //https://api.hubapi.com/contacts/v1/contact/vid/41/profile?hapikey=4c8d196f-7105-4c06-a17c-9fb83e6dcd10
            var url = new ApiUrl
            {
                Root = @"https://api.hubapi.com",
                PathSegments = new[] { "contacts", "v1", "contact", "vid", "41", "profile" }
            };

            string _out = url;
            Assert.IsNotNull(_out);
        }

        [TestMethod]
        public void TestMethod4()
        {
            //https://api.hubapi.com/contacts/v1/contact/vid/41/profile?hapikey=4c8d196f-7105-4c06-a17c-9fb83e6dcd10
            Url url = new ApiUrl
            {
                Root = @"https://api.hubapi.com",
                PathSegments = new[] { "contacts", "v1", "contact", "vid", "41", "profile" },
                QueryParams = new Dictionary<string, object>() { { "hapikey", "4c8d196f-7105-4c06-a17c-9fb83e6dcd10" } }
            };

            string _out = url.GetStringAsync().Result;
            Assert.IsNotNull(_out);
        }

        [TestMethod]
        public void TestMethod5()
        {
            //https://api.hubapi.com/contacts/v1/contact/vid/41/profile?hapikey=4c8d196f-7105-4c06-a17c-9fb83e6dcd10
            var url = new ApiUrl
            {
                Root = @"https://api.hubapi.com",
                PathSegments = new[] { "contacts", "v1", "contact", "vid", "{0}", "profile" },
                QueryParams = new Dictionary<string, object>() { { "hapikey", "4c8d196f-7105-4c06-a17c-9fb83e6dcd10" } }
            };

            var fi = new FileInfo(@"url.json");

            url.WriteJsonToFile(fi);
            Url newUrl = fi.ReadJsonFile<ApiUrl>();

            var x = url.Format("41");

            string _out = x.GetStringAsync().Result;
            Assert.IsNotNull(_out);

            
        }
    }
}
