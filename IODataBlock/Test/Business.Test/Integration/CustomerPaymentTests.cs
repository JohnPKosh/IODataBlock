using Business.Common.Configuration;
using HubSpot.Models.Contacts;
using HubSpot.Models.Properties;
using HubSpot.Services;
using HubSpot.Services.ModeTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NsRest;

namespace Business.Test.Integration
{
    /// <summary>
    /// Summary description for ContactSyncToNSTests
    /// </summary>
    [TestClass]
    public class CustomerPaymentTests
    {
        #region Class Initialization

        public CustomerPaymentTests()
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

        #endregion Additional test attributes

        #endregion Class Initialization

        #region Other Tests

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
            //var nscolumns = new string[] { "entitystatus", "email", "balance", "stage", "custentity_cr_hs_profile_url" };
            var nscolumns = new string[] { "entitystatus", "email", "balance", "stage" };
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
            var partnerForms = new List<string>() { "1952d04a-7c08-484c-9920-5c78838f0b7f", "da714af6-e4d7-40b2-9f0c-fa3873b01dc4" };

            //var contacts = GetRecentContactViewModels(100, new UnixMsTimestamp(DateTime.Today.AddDays(-15)), propertyMode: PropertyModeType.value_only)

            var contacts = GetAllContactViewModels(100, propertyMode: PropertyModeType.value_only)
                //.Where(x => x.Properties.First(y => y.Key == "lifecyclestage").Value == "lead").ToList();
                //.Where(x => x.form_submissions.Any(y => y.GetValue("form-id").ToObject<string>() == "1952d04a-7c08-484c-9920-5c78838f0b7f"));
                //.Where(x => x.form_submissions.Any(y => y.GetValue("form-id").ToObject<string>() == "da714af6-e4d7-40b2-9f0c-fa3873b01dc4"));
                .Where(x => x.form_submissions != null && x.form_submissions.Any(y => partnerForms.Contains(y.GetValue("form-id").ToObject<string>())));

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

        //22334
        [TestMethod]
        public void SearchNsCustomersById()
        {
            var nsleads = GetNsCustomerJsonById("22334");
            if (nsleads != null)
            {
                // get NS id
            }
            if (nsleads == "null")
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetCcById()
        {
            var nsleads = GetNsCcJsonById("11");
            if (nsleads != null)
            {
                // get NS id
            }
        }

        [TestMethod]
        public void SearchNsCustomersByEmail()
        {
            var nscolumns = new string[] { "entitystatus", "email", "balance", "stage", "ccnumber" };
            var email = "kpi78@ukr.net";
            var nsleads = SearchNsCustomersJsonByEmail(email, nscolumns);
            if (nsleads != null)
            {
                // get NS id
            }
        }

        [TestMethod]
        public void SearchNSCustomerPayments()
        {
            var nscolumns = new string[] { "ccholdername", "amount" };
            var email = "lwitter@teligencepartners.com";
            var nsleads = SearchNsCustomerPayments("Test CustomerName", nscolumns);
            if (nsleads != null)
            {
                // get NS id
                foreach (var o in nsleads)
                {
                }
            }
        }

        [TestMethod]
        public void testupdate()
        {
            try
            {
                dynamic o = new ExpandoObject();
                var ccs = new List<dynamic>();

                dynamic cc1 = new ExpandoObject();
                cc1.ccexpiredate = "01/2018";
                ccs.Add(cc1);

                dynamic cc2 = new ExpandoObject();
                cc2.ccexpiredate = "12/2021";

                ccs.Add(cc2);

                o.creditcards = ccs;

                var parameters = new Dictionary<string, object> { { "type", "customer" }, { "id", "22334" }, { "request_body", o } };

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

        #endregion Other Tests

        #region Credit Card CRUD Tests

        [TestMethod]
        public void InsertCreditCardTest()
        {
            try
            {
                dynamic o = new ExpandoObject();
                var ccs = new List<dynamic>();

                dynamic cc1 = new ExpandoObject();

                /* Add Credit Card Info */
                cc1.ccdefault = "T";
                cc1.ccexpiredate = "07/2027";
                cc1.ccname = "Test Rest4";
                cc1.ccnumber = "4242424242424242";
                cc1.customercode = "378";
                cc1.ccsecuritycode = "378";

                ccs.Add(cc1);
                o.creditcards = ccs;
                var parameters = new Dictionary<string, object> { { "id", "22334" }, { "method", "CREATE" }, { "request_body", o } };
                var restlet = PostRestletBase.Create(BaseUrl, GetCCScriptSetting(), GetLogin());
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void ReadAllCreditCardsTest()
        {
            try
            {
                var parameters = new Dictionary<string, object> { { "id", "22334" }, { "method", "READ" } };

                var restlet = PostRestletBase.Create(BaseUrl, GetCCScriptSetting(), GetLogin());
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void ReadSpecificCreditCardTest()
        {
            try
            {
                dynamic o = new ExpandoObject();
                var ccs = new List<dynamic>();

                dynamic cc1 = new ExpandoObject();
                cc1.internalid = 10;
                ccs.Add(cc1);

                dynamic cc2 = new ExpandoObject();
                cc2.internalid = 11;
                ccs.Add(cc2);

                o.creditcards = ccs;

                var parameters = new Dictionary<string, object> { { "id", "22334" }, { "method", "READ" }, { "request_body", o } };

                var restlet = PostRestletBase.Create(BaseUrl, GetCCScriptSetting(), GetLogin());
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void UpdateCreditCardTest()
        {
            try
            {
                dynamic o = new ExpandoObject();
                var ccs = new List<dynamic>();

                dynamic cc1 = new ExpandoObject();
                cc1.internalid = 1613;
                cc1.ccdefault = "T";
                cc1.ccexpiredate = "08/2020";
                cc1.ccname = "Test Rest Update";
                cc1.ccnumber = "4242424242424242";
                cc1.customercode = "378";
                cc1.ccsecuritycode = "378";
                ccs.Add(cc1);
                o.creditcards = ccs;

                var parameters = new Dictionary<string, object> { { "id", "22334" }, { "method", "UPDATE" }, { "request_body", o } };

                var restlet = PostRestletBase.Create(BaseUrl, GetCCScriptSetting(), GetLogin());
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void DeleteCreditCardTest()
        {
            try
            {
                dynamic o = new ExpandoObject();
                var ccs = new List<dynamic>();

                dynamic cc1 = new ExpandoObject();
                cc1.internalid = 2012;
                ccs.Add(cc1);

                //dynamic cc2 = new ExpandoObject();
                //cc2.internalid = 1112;
                //ccs.Add(cc2);

                o.creditcards = ccs;

                var parameters = new Dictionary<string, object> { { "id", "22334" }, { "method", "DELETE" }, { "request_body", o } };

                var restlet = PostRestletBase.Create(BaseUrl, GetCCScriptSetting(), GetLogin());
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void DeleteSpecificCreditCardTest()
        {
            try
            {
                var parameters = new Dictionary<string, object> { { "id", "22334" }, { "internalid", 1912 } };

                var restlet = DelRestletBase.Create(BaseUrl, GetCCScriptSetting(), GetLogin());
                var rv = restlet.DelAsync(parameters);
                var result = rv.Result;

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Credit Card CRUD Tests

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

        private string SearchNsCustomersJsonByEmail(string email, IEnumerable<string> columns = null)
        {
            var parameters = new Dictionary<string, object> { { "type", "customer" }, { "field", "email" }, { "op", "is" }, { "value1", email }, { "columns", columns ?? columns.ToArray() } };
            var restlet = GetRestletBase.Create(BaseUrl, GetSearchRecordScriptSetting(), GetLogin());
            var rv = restlet.ExecuteToJsonStringAsync(parameters);
            if (rv.Exception != null) throw rv.Exception;
            return rv.Result;
        }

        private string GetNsCustomerJsonById(string id)
        {
            var parameters = new Dictionary<string, object> { { "type", "customer" }, { "id", id } };

            var login = GetLogin();
            var script = GetScriptSetting();

            var restlet = GetRestletBase.Create(BaseUrl, GetScriptSetting(), GetLogin());
            var rv = restlet.ExecuteToJsonStringAsync(parameters);
            if (rv.Exception != null) throw rv.Exception;
            return rv.Result;
        }

        private string GetNsCcJsonById(string id)
        {
            var parameters = new Dictionary<string, object> { { "type", "customerpayment" }, { "id", id } };
            var restlet = GetRestletBase.Create(BaseUrl, GetScriptSetting(), GetLogin());
            var rv = restlet.ExecuteToJsonStringAsync(parameters);
            if (rv.Exception != null) throw rv.Exception;
            return rv.Result;
        }

        private IEnumerable<dynamic> SearchNsCustomerPayments(string id, IEnumerable<string> columns = null)
        {
            //var parameters = new Dictionary<string, object> { { "type", "lead" }, { "field", "email" }, { "op", "is" }, { "value1", email }, { "columns", columns ?? columns.ToArray() } };
            var parameters = new Dictionary<string, object> { { "type", "customerpayment" }, { "field", "ccholdername" }, { "op", "is" }, { "value1", id }, { "columns", columns ?? columns.ToArray() } };

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
                o.phone = c.Properties.First(x => x.Key == "phone").Value;
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

        #endregion Utility Methods

        #region NS Login helpers

        //public static String BaseUrl = @"https://rest.na1.netsuite.com/app/site/hosting/restlet.nl";
        public static String BaseUrl = @"https://rest.sandbox.netsuite.com/app/site/hosting/restlet.nl";

        public NetSuiteScriptSetting GetScriptSetting()
        {
            return NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud");
        }

        public NetSuiteScriptSetting GetCCScriptSetting()
        {
            return NetSuiteScriptSetting.Create("customscript_cr_cc_crud", "customdeploy_cr_cc_crud");
        }

        public NetSuiteScriptSetting GetSearchRecordScriptSetting()
        {
            return NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search");
        }

        public NetSuiteLogin GetLogin()
        {
            return NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, "3");
        }

        #endregion NS Login helpers
    }
}