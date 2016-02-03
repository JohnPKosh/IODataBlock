using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Common.GenericResponses;
using HubSpot.Models.Companies;
using HubSpot.Models.Contacts;
using HubSpot.Services.Companies;
using HubSpot.Services.Contacts;
using HubSpot.Services.ModeTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NsRest;
using NsRest.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using HubSpot.Models.Properties;

namespace Business.Test.Integration
{
    /// <summary>
    /// Summary description for CompanySyncToNSTests
    /// </summary>
    [TestClass]
    public class CompanySyncToNSTests
    {
        #region Class Initialization

        public CompanySyncToNSTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");

            NsBaseUrl = configMgr.GetAppSetting("nsbaseurl");
            //NsBaseUrl = configMgr.GetAppSetting("nssandboxurl");
            NsAccount = configMgr.GetAppSetting("nsaccount");
            NsEmail = configMgr.GetAppSetting("nsemail");
            NsPassword = configMgr.GetAppSetting("nspassword");
            NsRole = configMgr.GetAppSetting("nsrole");

            scriptSettings = new Dictionary<string, INetSuiteScriptSetting>
            {
                {"crud", NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud")},
                //{"cc_crud", NetSuiteScriptSetting.Create("customscript_cr_cc_crud", "customdeploy_cr_cc_crud")},
                {"search", NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search")}
            };
            baseService = BaseService.Create(NsBaseUrl, NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, NsRole), scriptSettings);

            AllContactsCacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, configMgr.GetAppSetting("hubspot:contactsfilename")));
            //SaveAllContactsToDisk();
            if (!AllContactsCacheFile.Exists) SaveAllContactsToDisk();
            AllContacts = ReadAllContactsFromDisk().ToList();

            AllCompaniesCacheFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, configMgr.GetAppSetting("hubspot:companiesfilename")));
            //SaveAllCompaniesToDisk();
            if (!AllCompaniesCacheFile.Exists) SaveAllCompaniesToDisk();
            AllCompanies = ReadAllCompaniesFromDisk().ToList();
        }

        private readonly string _hapiKey;

        private string NsBaseUrl { get; set; }
        private string NsAccount { get; set; }
        private string NsEmail { get; set; }
        private string NsRole { get; set; }
        private string NsPassword { get; set; }
        private BaseService baseService { get; set; }
        private IDictionary<string, INetSuiteScriptSetting> scriptSettings { get; set; }

        private FileInfo AllContactsCacheFile { get; set; }
        private List<ContactViewModel> AllContacts { get; set; }

        private FileInfo AllCompaniesCacheFile { get; set; }
        private List<CompanyViewModel> AllCompanies { get; set; }

        #endregion Class Initialization

        [TestMethod]
        public void FindCompaniesByDomainTest()
        {
            foreach (var c in AllContacts)
            {
                var email = c.identity_profiles.First(x => x.vid == c.vid).identities.First(y => y.type == "EMAIL").value;
            }

            foreach (var c in AllCompanies)
            {
                var email = c.Properties.First().Value;
            }

            //var contacts = GetAllContactViewModels().ToList();

            //foreach (var c in contacts)
            //{
            //    var email = c.identity_profiles.First(x => x.vid == c.vid).identities.First(y => y.type == "EMAIL").value;
            //}
        }

        [TestMethod]
        public void EnumerateCompaniesTest()
        {
            foreach (var c in AllCompanies.Where(x => x.Properties.Any(y => y.Key == "website" && y.Value != null)))
            {
                var propNames = new List<string>();
                foreach (var managedProperty in c.ManagedProperties)
                {
                    propNames.Add(managedProperty.name);
                }
                var website = c.Properties.First(x => x.Key == "website").Value;
            }

            var companyIds = new List<string>();
            foreach (var c in AllContacts.Where(x => x.Properties.Any(y => y.Key == "associatedcompanyid" && y.Value != null)))
            {
                var associatedcompanyid = c.Properties.First(x => x.Key == "associatedcompanyid").Value;
                companyIds.Add(associatedcompanyid);
            }
            Assert.IsNotNull(companyIds);
            //var contacts = GetAllContactViewModels().ToList();

            //foreach (var c in contacts)
            //{
            //    var email = c.identity_profiles.First(x => x.vid == c.vid).identities.First(y => y.type == "EMAIL").value;
            //}
        }

        [TestMethod]
        public void CheckCompanyDomainsTest()
        {
            var response = baseService.SearchJArrayAsync("contact", null, null, "customsearch_cr_initial_target_contact", scriptKey: "search").Result;
            var nsitems = response.ResponseData;

            var missing = new List<dynamic>();
            var existing = new List<dynamic>();

            var nsdomains = nsitems.Children()["columns"]["url"].Values<string>().Select(GetDomainName).Distinct().ToList();
            Assert.IsNotNull(nsdomains);

            var hsdomains = AllCompanies.Where(x=> (x.Properties.FirstOrDefault(y => y.Key == "domain") != null && 
            x.Properties.FirstOrDefault(y => y.Key == "domain").Value != null)).Select(z => z.Properties.First(y => y.Key == "domain").Value).Distinct().ToList();
            Assert.IsNotNull(hsdomains);

            var hswebsitedomains = AllCompanies.Where(x => (x.Properties.FirstOrDefault(y => y.Key == "website") != null && 
            x.Properties.FirstOrDefault(y => y.Key == "website").Value != null)).Select(z => z.Properties.First(y => y.Key == "website").Value).Select(GetDomainName).Distinct().ToList();
            Assert.IsNotNull(hswebsitedomains);
        }

        [TestMethod]
        public void CheckMissingCompaniesTest()
        {
            var response = baseService.SearchJArrayAsync("contact", null, null, "customsearch_cr_initial_target_contact", scriptKey: "search").Result;
            var nsitems = response.ResponseData;

            var nsdomains = nsitems.Children()["columns"]["url"].Values<string>().Select(GetDomainName).Distinct().ToList();

            var hsdomains = AllCompanies.Where(x => (x.Properties.FirstOrDefault(y => y.Key == "domain") != null &&
            x.Properties.FirstOrDefault(y => y.Key == "domain").Value != null)).Select(z => z.Properties.First(y => y.Key == "domain").Value).Distinct().ToList();

            var hswebsitedomains = AllCompanies.Where(x => (x.Properties.FirstOrDefault(y => y.Key == "website") != null &&
            x.Properties.FirstOrDefault(y => y.Key == "website").Value != null)).Select(z => z.Properties.First(y => y.Key == "website").Value).Select(GetDomainName).Distinct().ToList();

            var missing = new List<dynamic>();
            var existing = new List<dynamic>();

            missing.AddRange(nsdomains.Where(x=> hsdomains.All(y => y != x)));
            foreach (var item in nsitems.Children())
            {
                var id = item["id"].Value<int>();
                var result = AddHsContact(item);

                var listResult = AddHsContactToList(139, result);
                Assert.IsNotNull(listResult);
            }
        }

        #region Utility Methods

        public IEnumerable<ContactViewModel> GetAllContactViewModels(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var service = new ContactService(_hapiKey);
            return service.GetAllContactViewModels(count, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships).ToList();
        }

        public IEnumerable<CompanyViewModel> GetCompaniesByDomain(string domain)
        {
            var service = new CompanyService(_hapiKey);
            return service.GetByDomainViewModel("broadvox.com");
        }

        private void SaveAllContactsToDisk()
        {
            GetAllContactViewModels().WriteJsonToFile(AllContactsCacheFile);
        }

        private IEnumerable<ContactViewModel> ReadAllContactsFromDisk()
        {
            return AllContactsCacheFile.ReadJsonFile<IEnumerable<ContactViewModel>>();
        }

        public IEnumerable<CompanyViewModel> GetAllCompanyViewModels(int? count = null, long? offset = null)
        {
            var service = new CompanyService(_hapiKey);
            return service.GetAllCompanyViewModels(count, offset).ToList();
            //return service.GetRecentCompanyViewModels(count, vidOffset).ToList();
        }

        private void SaveAllCompaniesToDisk()
        {
            GetAllCompanyViewModels(100, 0).WriteJsonToFile(AllCompaniesCacheFile);
        }

        private IEnumerable<CompanyViewModel> ReadAllCompaniesFromDisk()
        {
            return AllCompaniesCacheFile.ReadJsonFile<IEnumerable<CompanyViewModel>>();
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
                o.isperson = "F";
                o.companyname = c.Properties.First(x => x.Key == "company").Value;

                o.subsidiary = "2";
                o.category = "1";

                //var parameters = new Dictionary<string, object> { { "type", "lead" }, { "request_body", o } };

                //var restlet = PostRestletBase.Create(NsBaseUrl, GetScriptSetting(), GetLogin());
                //var rv = restlet.ExecuteToJsonStringAsync(parameters);
                var response = baseService.Create("lead", o, "crud");
                CheckJsonResponse(response);
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

                var response = baseService.Update("lead", o, "crud");
                CheckJsonResponse(response);

                //var restlet = PutRestletBase.Create(BaseUrl, GetScriptSetting(), GetLogin());
                //var rv = restlet.ExecuteToJsonStringAsync(parameters);
                //var result = rv.Result;

                //Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ContactViewModel AddHsContact(JToken nsitem)
        {
            var service = new ContactService(_hapiKey);
            //var contactstring = File.ReadAllText(@"Junk\ContactUpdate.json");

            var contact = service.GetContactByEmailViewModel(nsitem["columns"]["email"].Value<string>(), null, PropertyModeType.value_and_history, FormSubmissionModeType.All, true);
            if (contact == null)
            {
                ContactUpdateModel model = new ContactUpdateModel();
                model.Properties = new HashSet<PropertyUpdateValue>();

                if (nsitem["columns"]["firstname"] != null) model.Properties.Add(new PropertyUpdateValue("firstname", nsitem["columns"]["firstname"].Value<string>()));
                if (nsitem["columns"]["lastname"] != null) model.Properties.Add(new PropertyUpdateValue("lastname", nsitem["columns"]["lastname"].Value<string>()));
                if (nsitem["columns"]["email"] != null) model.Properties.Add(new PropertyUpdateValue("email", nsitem["columns"]["email"].Value<string>()));
                if (nsitem["columns"]["altname"] != null) model.Properties.Add(new PropertyUpdateValue("company", nsitem["columns"]["altname"].Value<string>()));
                if (nsitem["columns"]["url"] != null) model.Properties.Add(new PropertyUpdateValue("website", nsitem["columns"]["url"].Value<string>()));
                if (nsitem["columns"]["phone"] != null) model.Properties.Add(new PropertyUpdateValue("phone", nsitem["columns"]["phone"].Value<string>()));

                var contactstring = model.ToJson();
                var ro = service.Create(contactstring);
                if (ro.HasExceptions)
                {
                    Assert.Fail();
                }
                else
                {
                    var data = ro.ResponseData;
                    contact = service.GetContactByEmailViewModel(nsitem["columns"]["email"].Value<string>(), null, PropertyModeType.value_and_history, FormSubmissionModeType.All, true);
                }
            }
            return contact;
        }

        private string AddHsContactToList(int id, ContactViewModel contact)
        {
            var service = new ContactService(_hapiKey);

            var contacts = service.GetContactsInListViewModels(id.ToString(), 100, null, null, PropertyModeType.value_only, FormSubmissionModeType.None, false).ToList();
            if (contacts.Any(x => x.vid == contact.vid))
            {
                return "exists";
            }

            dynamic o = new ExpandoObject();
            o.vids = new[] { contact.vid };

            var ro = service.AddToList(id, o);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                return data;
            }
            return null;
        }

        #endregion Utility Methods

        #region NS Login helpers

        //public static String BaseUrl = @"https://rest.na1.netsuite.com/app/site/hosting/restlet.nl";
        ////public static String BaseUrl = @"https://rest.sandbox.netsuite.com/app/site/hosting/restlet.nl";

        //public NetSuiteScriptSetting GetScriptSetting()
        //{
        //    return NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud");
        //}

        //public NetSuiteScriptSetting GetSearchRecordScriptSetting()
        //{
        //    return NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search");
        //}

        //public NetSuiteLogin GetLogin()
        //{
        //    return NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, "3");
        //}

        #endregion NS Login helpers

        #region Private Utility Methods

        private void CheckJsonResponse(IResponseObject<string, string> response)
        {
            /* "null" string returned by RESTlet if Record not found error occurs, otherwise if no Exceptions exist object value is returned. */
            if (response.ResponseData == "null")
            {
                Assert.Fail();
            }
            if (response.HasExceptions) Assert.Fail(); /* Exceptions exist in response */
        }

        private void CheckDynamicListResponse(IResponseObject<string, IList<ExpandoObject>> response)
        {
            /* "null" string returned by RESTlet if Record not found error occurs, otherwise if no Exceptions exist object value is returned. */
            if (response.ResponseData == null)
            {
                Assert.Fail();
            }
            if (response.HasExceptions) Assert.Fail(); /* Exceptions exist in response */
        }

        private void CheckDynamicResponse(IResponseObject<string, dynamic> response)
        {
            /* "null" string returned by RESTlet if Record not found error occurs, otherwise if no Exceptions exist object value is returned. */
            if (response.ResponseData == null)
            {
                Assert.Fail();
            }
            if (response.HasExceptions) Assert.Fail(); /* Exceptions exist in response */
        }

        private void CheckJArrayResponse(IResponseObject<string, JArray> response)
        {
            /* "null" string returned by RESTlet if Record not found error occurs, otherwise if no Exceptions exist object value is returned. */
            if (response.ResponseData == null)
            {
                Assert.Fail();
            }
            if (response.HasExceptions) Assert.Fail(); /* Exceptions exist in response */
        }

        private void CheckJObjectsResponse(IResponseObject<string, IEnumerable<JObject>> response)
        {
            /* "null" string returned by RESTlet if Record not found error occurs, otherwise if no Exceptions exist object value is returned. */
            if (response.ResponseData == null)
            {
                Assert.Fail();
            }
            if (response.HasExceptions) Assert.Fail(); /* Exceptions exist in response */
        }

        private void CheckBooleanResponse(IResponseObject<string, bool> response)
        {
            /* "null" string returned by RESTlet if Record not found error occurs, otherwise if no Exceptions exist object value is returned. */
            if (!response.ResponseData)
            {
                Assert.Fail();
            }
            if (response.HasExceptions) Assert.Fail(); /* Exceptions exist in response */
        }

        private static string GetDomainName(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                var pos = value.IndexOf("://", StringComparison.Ordinal);
                if (pos == -1 || pos > 5) value = "http://" + value;
                //return new Uri(value).Host;
                var uri = new Uri(value);

                var domainParts = uri.Host.Split('.');
                if (domainParts.Length == 3)
                {
                    var topLevel = domainParts[domainParts.Length - 1];
                    var hostBody = domainParts[domainParts.Length - 2];
                    var hostHeader = domainParts[domainParts.Length - 3];
                    var domain = string.Format("{0}.{1}", hostBody, topLevel);
                    return domain;
                }
                return uri.Host;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Private Utility Methods
    }
}