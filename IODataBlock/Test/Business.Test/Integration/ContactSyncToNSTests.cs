using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Configuration;
using Fasterflect;
using HubSpot.Models.Contacts;
using HubSpot.Services;
using HubSpot.Services.ModeTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSuite.RESTlet.Integration;

namespace Business.Test.Integration
{
    /// <summary>
    /// Summary description for ContactSyncToNSTests
    /// </summary>
    [TestClass]
    public class ContactSyncToNSTests
    {
        public ContactSyncToNSTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            NsAccount = configMgr.GetAppSetting("nsaccount");
            NsEmail = configMgr.GetAppSetting("nsemail");
            NsPassword = configMgr.GetAppSetting("nspassword");
        }

        private TestContext testContextInstance;

        private readonly string _hapiKey;
        private string NsAccount { get; set; }
        private string NsEmail { get; set; }
        private string NsPassword { get; set; }

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
            var hsprops = new List<string> { "lastname", "firstname", "email", "hs_lead_status", "lifecyclestage" };
            var nscolumns = new string[] { "entitystatus", "email", "balance", "stage" };
            var contacts = GetAllContacts(100, null, hsprops, PropertyModeType.value_and_history, FormSubmissionModeType.All, true).ConvertToIEnumerableDynamic();

            foreach (var contact in contacts.Where(x => x.lifecyclestage == "subscriber"))
            {
                var lifecyclestage = contact.lifecyclestage;
                Assert.IsNotNull(lifecyclestage);

                //var email = (string)contact.Properties.TryGetValue("email");
                var email = contact.identity_email;
                if (string.IsNullOrWhiteSpace(email)) continue;
                var nsleads = SearchLeadsByEmail(email, nscolumns);
                if (nsleads == null) continue;
                foreach (dynamic nslead in nsleads)
                {
                    Assert.IsNotNull(nslead);

                    var status = nslead.columns.entitystatus.name;
                    if(status == "LEAD-Qualified") continue;
                    Assert.IsTrue(nslead.columns.balance < .01 || nslead.columns.balance == null);
                    Assert.IsTrue(nslead.columns.stage.internalid == "LEAD");
                    var deleted = Delete(nslead.recordtype, nslead.id);
                    Assert.IsTrue(deleted);


                }
            }

            //foreach (var contact in contacts.Where(x=> x.Properties.First(y => y.Key == "lifecyclestage").Value == "lifecyclestage"))
            //{
            //    var lifecyclestage = contact.Properties.First(x=> x.Key == "lifecyclestage").Value;
            //    Assert.IsNotNull(lifecyclestage);

            //    //var email = (string)contact.Properties.TryGetValue("email");
            //    var email = (string)contact.identity_profiles.GetFieldValue();
            //    if (string.IsNullOrWhiteSpace(email))continue;
            //    var nsleads = SearchLeadsByEmail(email, nscolumns).ToList();
            //    foreach (dynamic nslead in nsleads)
            //    {
            //        Assert.IsNotNull(nslead);

            //        var status = nslead.columns.entitystatus.name;
            //        Assert.IsTrue(nslead.columns.balance < .01 || nslead.columns.balance == null);
            //        Assert.IsTrue(nslead.columns.stage.internalid == "LEAD");
            //        var deleted = Delete(nslead.recordtype, nslead.id);
            //        Assert.IsTrue(deleted);


            //    }
            //}
        }

        private List<ContactViewModel> GetAllContacts(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var service = new ContactService(_hapiKey);
            return service.GetAllContactViewModels(100, null, null, PropertyModeType.value_and_history, FormSubmissionModeType.All, true).ToList();
        } 

        private IEnumerable<dynamic> SearchLeadsByEmail(string email, IEnumerable<string> columns = null)
        {
            var parameters = new Dictionary<string, object> { { "type", "lead" }, { "field", "email" }, { "op", "is" }, { "value1", email }, { "columns", columns?? columns.ToArray() } };
            var restlet = GetRestletBase.Create(BaseUrl, GetSearchRecordScriptSetting(), GetLogin());
            var rv = restlet.ExecuteToDynamicListAsync(parameters);
            if (rv.Exception != null) throw rv.Exception;
            return rv.Result;
        }

        private bool Delete(string type, string id)
        {
            var restlet = DelRestletBase.Create(BaseUrl, GetScriptSetting(), GetLogin());
            return restlet.Delete(type, id);
        }

        #region NS Login helpers

        public static String BaseUrl = @"https://rest.na1.netsuite.com/app/site/hosting/restlet.nl";
        //public static String BaseUrl = @"https://rest.sandbox.netsuite.com/app/site/hosting/restlet.nl";

        public NetSuiteScriptSetting GetScriptSetting()
        {
            return NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud");
        }

        public NetSuiteScriptSetting GetSearchRecordScriptSetting()
        {
            return NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search");
        }

        public NetSuiteLogin GetLogin()
        {
            return NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, "3");
        } 

        #endregion
    }
}
