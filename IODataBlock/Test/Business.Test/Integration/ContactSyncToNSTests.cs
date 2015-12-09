using System;
using System.Text;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Security.Aes;
using Business.Common.System;
using Fasterflect;
using HubSpot.Models.Contacts;
using HubSpot.Models.Properties;
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

        //[TestMethod]
        //public void DeleteNsContactWhereHsStageIsOther()
        //{
        //    var hsprops = new List<string> { "lastname", "firstname", "email", "hs_lead_status", "lifecyclestage" };
        //    var nscolumns = new string[] { "entitystatus", "email", "balance", "stage" };
        //    var contacts = GetAllContacts(100, null, null, PropertyModeType.value_and_history, FormSubmissionModeType.All, true).ConvertToIEnumerableDynamic();

        //    foreach (var contact in contacts.Where(x => x.lifecyclestage == "other"))
        //    {
        //        var lifecyclestage = contact.lifecyclestage;
        //        Assert.IsNotNull(lifecyclestage);

        //        //var email = (string)contact.Properties.TryGetValue("email");
        //        var email = contact.identity_email;
        //        if (string.IsNullOrWhiteSpace(email)) continue;
        //        var nsleads = SearchNsLeadsByEmail(email, nscolumns);
        //        if (nsleads == null) continue;
        //        foreach (dynamic nslead in nsleads)
        //        {
        //            Assert.IsNotNull(nslead);

        //            var status = nslead.columns.entitystatus.name;
        //            if(status == "LEAD-Qualified") continue;
        //            Assert.IsTrue(nslead.columns.balance < .01 || nslead.columns.balance == null);
        //            Assert.IsTrue(nslead.columns.stage.internalid == "LEAD");
        //            var deleted = DeleteNs(nslead.recordtype, nslead.id);
        //            Assert.IsTrue(deleted);
        //        }
        //    }
        //}


        [TestMethod]
        public void DeleteNsContactWhereHsStageIsOther()
        {
            var hsprops = new List<string> { "lastname", "firstname", "email", "hs_lead_status", "lifecyclestage" };
            //var nscolumns = new string[] { "entitystatus", "email", "balance", "stage", "custentity_cr_hs_profile_url" };
            var nscolumns = new string[] { "entitystatus", "email", "balance", "stage" };
            var partnerForms = new List<string>() { "1952d04a-7c08-484c-9920-5c78838f0b7f", "da714af6-e4d7-40b2-9f0c-fa3873b01dc4" };
            var contacts = GetAllContactViewModels(100, null, null, PropertyModeType.value_and_history, FormSubmissionModeType.All, true).Where(x => x.Properties.First(y => y.Key == "lifecyclestage").Value != "lead");

            foreach (var contact in contacts.Where(x => x.form_submissions == null || !x.form_submissions.Any(y => partnerForms.Contains(y.GetValue("form-id").ToObject<string>()))))
            {
                //var email = (string)contact.Properties.TryGetValue("email");
                var email = (string)contact.Properties.First(x => x.Key == "email").Value;
                if (string.IsNullOrWhiteSpace(email)) continue;
                var nsleads = SearchNsLeadsByEmail(email, nscolumns);
                if (nsleads == null) continue;
                foreach (dynamic nslead in nsleads)
                {
                    Assert.IsNotNull(nslead);

                    var status = nslead.columns.entitystatus.name;
                    if (status == "LEAD-Qualified") continue;
                    Assert.IsTrue(nslead.columns.balance < .01 || nslead.columns.balance == null);
                    Assert.IsTrue(nslead.columns.stage.internalid == "LEAD");
                    var deleted = DeleteNs(nslead.recordtype, nslead.id);
                    Assert.IsTrue(deleted);
                }
            }
        }

        //"category_field_partner"
        //"category_field_end_user"

        [TestMethod]
        public void InsertNsContactWhereHsIsLead()
        {
            var nscolumns = new string[] { "entitystatus", "email", "balance", "stage", "custentity_cr_hs_profile_url" };
            var partnerForms = new List<string>() { "1952d04a-7c08-484c-9920-5c78838f0b7f", "da714af6-e4d7-40b2-9f0c-fa3873b01dc4" };

            //var contacts = GetRecentContactViewModels(100, new UnixMsTimestamp(DateTime.Today.AddDays(-15)), propertyMode: PropertyModeType.value_only)

            var contacts = GetAllContactViewModels(100, propertyMode: PropertyModeType.value_only)
                .Where(x => x.Properties.First(y => y.Key == "lifecyclestage").Value == "lead").ToList();
            //.Where(x => x.form_submissions != null && x.form_submissions.Any(y => partnerForms.Contains(y.GetValue("form-id").ToObject<string>())));

            foreach (var c in contacts)
            {
                var email = (string)c.Properties.First(x => x.Key == "email").Value;
                //var email = c.identity_profiles.First(x => x.vid == c.vid).identities.First(y => y.type == "EMAIL").value;
                if (string.IsNullOrWhiteSpace(email)) continue;
                var nsleads = SearchNsLeadsByEmail(email, nscolumns);
                if (nsleads == null)
                {
                    InsertNsLead(c);
                }
                else
                {
                    var nsId = c.Properties.FirstOrDefault(x => x.Key == "netsuite_internal_id")?.Value;
                    foreach (var o in nsleads.Where(o => o.id != nsId))
                    {
                        UpdateHubspotContactId(c, o.id);
                    }

                    var leadDictionary = nsleads.Select(x => x as IDictionary<string, object>); 
                    foreach (var o in nsleads.Where(o => !(o as IDictionary<string, object>).ContainsKey("columns") || !(o.columns as IDictionary<string, object>).ContainsKey("custentity_cr_hs_profile_url") || o.columns.custentity_cr_hs_profile_url != c.profile_url))
                    {
                        UpdateNsLeadProfileUrlByEmail(c, o.id as string);
                    }
                }
            }
        }

        [TestMethod]
        public void InsertNsContactWhereHsFormSubmitted()
        {
            var nscolumns = new string[] { "entitystatus", "email", "balance", "stage", "custentity_cr_hs_profile_url" };
            var partnerForms = new List<string>() { "1952d04a-7c08-484c-9920-5c78838f0b7f" , "da714af6-e4d7-40b2-9f0c-fa3873b01dc4" };

            //var contacts = GetRecentContactViewModels(100, new UnixMsTimestamp(DateTime.Today.AddDays(-15)), propertyMode: PropertyModeType.value_only)

            var contacts = GetAllContactViewModels(100, propertyMode: PropertyModeType.value_only)
                //.Where(x => x.Properties.First(y => y.Key == "lifecyclestage").Value == "lead").ToList();
                //.Where(x => x.form_submissions.Any(y => y.GetValue("form-id").ToObject<string>() == "1952d04a-7c08-484c-9920-5c78838f0b7f"));
                //.Where(x => x.form_submissions.Any(y => y.GetValue("form-id").ToObject<string>() == "da714af6-e4d7-40b2-9f0c-fa3873b01dc4"));
                .Where(x => x.form_submissions != null && x.form_submissions.Any(y => partnerForms.Contains(y.GetValue("form-id").ToObject<string>())));

            foreach (var c in contacts)
            {
                var email = (string)c.Properties.First(x=>x.Key == "email").Value;
                //var email = c.identity_profiles.First(x => x.vid == c.vid).identities.First(y => y.type == "EMAIL").value;
                if (string.IsNullOrWhiteSpace(email)) continue;
                var nsleads = SearchNsLeadsByEmail(email, nscolumns);
                if (nsleads == null)
                {
                    InsertNsLead(c);
                }
                else
                {
                    var nsId = c.Properties.FirstOrDefault(x => x.Key == "netsuite_internal_id")?.Value;
                    foreach (var o in nsleads.Where(o => o.id != nsId))
                    {
                        UpdateHubspotContactId(c, o.id);
                    }

                    var leadDictionary = nsleads.Select(x => x as IDictionary<string, object>);
                    foreach (var o in nsleads.Where(o => !(o as IDictionary<string, object>).ContainsKey("columns") || !(o.columns as IDictionary<string, object>).ContainsKey("custentity_cr_hs_profile_url") || o.columns.custentity_cr_hs_profile_url != c.profile_url))
                    {
                        UpdateNsLeadProfileUrlByEmail(c, o.id as string);
                    }
                }
            }
        }

        [TestMethod]
        public void SearchNSLeadTest()
        {
            var nscolumns = new string[] { "entitystatus", "email", "balance", "stage" };
            var email = "lwitter@teligencepartners.com";
            var nsleads = SearchNsLeadsByEmail(email, nscolumns);
            if (nsleads != null)
            {
                // get NS id 
                foreach (var o in nsleads)
                {

                }
            }

        }

        #region Utility Methods

        private List<ContactViewModel> GetAllContacts(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var service = new ContactService(_hapiKey);
            return service.GetAllContactViewModels(count, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships).ToList();
        }

        public IEnumerable<ContactViewModel> GetAllContactViewModels(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var service = new ContactService(_hapiKey);
            return service.GetAllContactViewModels(count, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships).ToList();
        }

        private List<ContactViewModel> GetRecentContactViewModels(int? count = null, long? timeOffset = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var service = new ContactService(_hapiKey);
            return service.GetRecentContactViewModels(count, timeOffset, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships).ToList();
        }

        private IEnumerable<dynamic> SearchNsLeadsByEmail(string email, IEnumerable<string> columns = null)
        {
            var parameters = new Dictionary<string, object> { { "type", "lead" }, { "field", "email" }, { "op", "is" }, { "value1", email }, { "columns", columns ?? columns.ToArray() } };
            var restlet = GetRestletBase.Create(BaseUrl, GetSearchRecordScriptSetting(), GetLogin());
            var rv = restlet.ExecuteToDynamicListAsync(parameters);
            if (rv.Exception != null) throw rv.Exception;
            return rv.Result;
        }

        private IEnumerable<dynamic> SearchNsCustomersByEmail(string email, IEnumerable<string> columns = null)
        {
            var parameters = new Dictionary<string, object> { { "type", "customer" }, { "field", "email" }, { "op", "is" }, { "value1", email }, { "columns", columns ?? columns.ToArray() } };
            var restlet = GetRestletBase.Create(BaseUrl, GetSearchRecordScriptSetting(), GetLogin());
            var rv = restlet.ExecuteToDynamicListAsync(parameters);
            if (rv.Exception != null) throw rv.Exception;
            return rv.Result;
        }

        private bool DeleteNs(string type, string id)
        {
            var restlet = DelRestletBase.Create(BaseUrl, GetScriptSetting(), GetLogin());
            return restlet.Delete(type, id);
        }

        private void InsertNsLead(ContactViewModel c)
        {
            try
            {
                var props = c.Properties;
                //var updateModel = (ContactUpdateModel) c;
                dynamic o = new ExpandoObject();
                o.phone = c.Properties.First(x=>x.Key == "phone").Value;
                o.lastname = c.Properties.First(x => x.Key == "lastname").Value;
                o.mobilephone = c.Properties.First(x => x.Key == "mobilephone").Value;
                o.title = c.Properties.First(x => x.Key == "jobtitle").Value;
                o.recordtype = "lead";
                o.firstname = c.Properties.First(x => x.Key == "firstname").Value;
                o.custentity_cr_hs_profile_url = c.profile_url;
                //o.email = updateModel.Properties.FirstOrDefault(x => x.Key == "email");
                o.email = c.identity_profiles.First(x => x.vid == c.vid).identities.First(y => y.type == "EMAIL").value;

                o.subsidiary = "2";
                o.category = "1";

                var parameters = new Dictionary<string, object> { { "type", "lead" }, { "request_body", o } };

                var restlet = PostRestletBase.Create(BaseUrl, GetScriptSetting(), GetLogin());
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UpdateNsLeadProfileUrlByEmail(ContactViewModel c, string id)
        {
            try
            {
                dynamic o = new ExpandoObject();
                o.custentity_cr_hs_profile_url = c.profile_url;

                var parameters = new Dictionary<string, object> { { "type", "lead" }, { "id", id }, { "request_body", o } };

                var restlet = PutRestletBase.Create(BaseUrl, GetScriptSetting(), GetLogin());
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UpdateHubspotContactId(ContactViewModel c, string id)
        {
            var service = new ContactService(_hapiKey);

            c.Properties.Clear();
            c.Properties.Add(new PropertyValue("netsuite_internal_id", id));

            var contactstring = service.GetContactUpdateString(c);

            var ro = service.UpdateContact(contactstring, c.vid);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
            }
        }

        #endregion

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
