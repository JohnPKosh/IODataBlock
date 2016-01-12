using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Common.GenericResponses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NsRest;
using NsRest.Search;
using NsRest.Services;

namespace BasicTests.Integration.NsRest
{
    [TestClass]
    public class NsRestServiceTests
    {
        #region Class Initialization

        public NsRestServiceTests()
        {
            configMgr = new ConfigMgr();
            NsBaseUrl = configMgr.GetAppSetting("nsbaseurl");
            //NsBaseUrl = configMgr.GetAppSetting("nssandboxurl");
            NsAccount = configMgr.GetAppSetting("nsaccount");
            NsEmail = configMgr.GetAppSetting("nsemail");
            NsPassword = configMgr.GetAppSetting("nspassword");
            NsRole = configMgr.GetAppSetting("nsrole");

            scriptSettings = new Dictionary<string, INetSuiteScriptSetting>
            {
                {"crud", NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud")},
                {"cc_crud", NetSuiteScriptSetting.Create("customscript_cr_cc_crud", "customdeploy_cr_cc_crud")},
                {"search", NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search")}
            };
            baseService = BaseService.Create(NsBaseUrl, NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, NsRole), scriptSettings);

            TestCustomerGuid = "952bf0d0-fdf8-4011-9493-8df388313b95";
        }

        private ConfigMgr configMgr { get; set; }

        private string NsBaseUrl { get; set; }
        private string NsAccount { get; set; }
        private string NsEmail { get; set; }
        private string NsRole { get; set; }
        private string NsPassword { get; set; }
        private BaseService baseService { get; set; }
        private IDictionary<string, INetSuiteScriptSetting> scriptSettings { get; set; }

        private string TestCustomerGuid { get; set; }

        #endregion

        #region Test Methods

        #region Basic CRUD Tests

        #region Customer Tests

        #region Read

        [TestMethod]
        public void GetCustomerJsonStringById_Test()
        {
            var response = baseService.GetJsonStringById("29838", "customer", "crud");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void GetCustomerJsonStringByIdAsync_Test()
        {
            var response = baseService.GetJsonStringByIdAsync("22334", "customer", "crud").Result;
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void GetCustomerDynamicById_Test()
        {
            var response = baseService.GetDynamicById("22334", "customer", "crud");
            CheckDynamicResponse(response);
        }

        [TestMethod]
        public void GetCustomerDynamicByIdAsync_Test()
        {
            var response = baseService.GetDynamicByIdAsync("22334", "customer", "crud").Result;
            CheckDynamicResponse(response);
        }

        #endregion

        #region Search


        [TestMethod]
        public void SearchCustomersJsonString_Test()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "name" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJsonString("customercategory", null, columns, scriptKey: "search");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void SearchCustomerCategoriesJsonString_Test()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "name" };
            var filters = new[] { NsSearchFilter.NewStringFilter("isinactive", SearchStringFieldOperatorType.Is, false.GetNsValue()) };
            var response = baseService.SearchJsonString("customercategory", filters, columns, scriptKey: "search");
            CheckJsonStringResponse(response);
        }


        [TestMethod]
        public void SearchEmployeeSalesRepsJsonString_Test()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "firstname", "lastname", "salesrole" };
            var filters = new[] {
                NsSearchFilter.NewStringFilter("salesrep", SearchStringFieldOperatorType.Is, true.GetNsValue()),
                NsSearchFilter.NewStringFilter("isinactive", SearchStringFieldOperatorType.Is, false.GetNsValue()),
                NsSearchFilter.NewEnumMultiSelectFilter("salesrole", SearchEnumMultiSelectFieldOperatorType.AnyOf, "-2", null)
            };
            var response = baseService.SearchJsonString("employee", filters, columns, scriptKey: "search");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJsonStringByExternalId_Test()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJsonString("customer", filters, scriptKey: "search");
            CheckJsonStringResponse(response);

            var r = JArray.Parse(response.ResponseData);
            var id = r[0].Value<string>("id");
            response = baseService.GetJsonStringById(id, "customer", "crud");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJsonStringByExternalId_Test2()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchDynamicList("customer", filters, scriptKey: "search");
            dynamic ro = response.ResponseData.First();
            CheckDynamicListResponse(response);

            var response2 = baseService.GetJsonStringById(ro.id, "customer", "crud");
            CheckJsonStringResponse(response2);
        }

        [TestMethod]
        public void SearchCustomerJsonStringByExternalId_Test3()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchDynamicList("customer", filters, columns, scriptKey: "search");

            if (response.HasExceptions) Assert.Fail(response.ExceptionList.ToJson(true));
            if (response.ResponseData == null) Assert.Fail("No records found!");
            dynamic ro = response.ResponseData.First();
            CheckDynamicListResponse(response);

            var response2 = baseService.GetJsonStringById(ro.id, "customer", "crud");
            CheckJsonStringResponse(response2);
        }

        [TestMethod]
        public void SearchCustomerJArrayByExternalId_Test()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync("customer", filters, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJArrayByExternalId_Test2()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync("customer", filters, columns, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJObjectsByExternalId_Test()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("customer", filters, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJObjectsByExternalId_Test2()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("customer", filters, columns, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        #endregion

        #region Create

        [TestMethod]
        public void PostCustomer_Test()
        {
            dynamic o = new ExpandoObject();
            o.phone = "2163734600";
            o.mobilephone = "2165132288";
            o.recordtype = "customer";
            o.email = "jkosh@broadvox.com";
            o.companyname = "MYCO";
            o.url = "http://www.nfl.com";
            o.subsidiary = "2";
            o.category = "1"; /* TODO: Add customer and partner category items to sandbox from production */
            o.isperson = "F";
            o.partner = "352";
            o.entitystatus = "13";
            o.externalid = TestCustomerGuid;

            var response = baseService.Create("customer", o, "crud");
            CheckJsonStringResponse(response);
        }


        [TestMethod]
        public void PostLead_Test()
        {
            dynamic o = new ExpandoObject();
            o.phone = "2163734600";
            o.mobilephone = "2165132288";
            o.recordtype = "customer";
            o.email = "jkosh@MYCO.com";
            o.companyname = "MYCO";
            o.url = "http://www.MYCO.com";
            o.subsidiary = "2";
            //o.category = "1"; /* TODO: Add customer and partner category items to sandbox from production */
            o.isperson = "F";
            //o.partner = "352";
            o.entitystatus = "6";
            o.externalid = TestCustomerGuid;

            var response = baseService.Create("lead", o, "crud");
            CheckJsonStringResponse(response);
        }


        [TestMethod]
        public void PostCustomerJson_Test()
        {
            dynamic o = new ExpandoObject();
            o.phone = "2163734699";
            o.mobilephone = "2165132289";
            o.recordtype = "customer";
            o.email = "jkosh@mytestorg.com";
            o.companyname = "mytestorg";
            o.url = "http://www.mytestorg.com";
            o.subsidiary = "2";
            o.category = "1"; /* TODO: Add customer and partner category items to sandbox from production */
            o.isperson = "F";
            o.partner = "352";
            o.entitystatus = "13";
            //o.externalid = TestCustomerGuid;
            //string json = JObject.FromObject(o).ToString();
            string json = JsonExpandoStringSerialization.ToJsonString(o);

            var response = baseService.CreateByJsonString("customer", json, "crud");
            CheckJsonStringResponse(response);
        }

        #endregion

        #region Update

        [TestMethod]
        public void PutCustomer_Test()
        {
            dynamic o = new ExpandoObject();
            o.comments = "blah blah blah";
            var response = baseService.Update("28481", "customer", o, "crud");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void PutCustomerByExternalId_Test()
        {
            //var id = GetIdByExternalId(TestCustomerGuid);
            var id = "28481";
            if (!string.IsNullOrWhiteSpace(id))
            {
                dynamic o = new ExpandoObject();
                o.comments = "null";
                //o.phone = "2163734624";
                var response = baseService.UpdateByDynamic(id, "customer", o, "crud");
                CheckJsonStringResponse(response);
            }
            else
            {
                Assert.Fail();
            }
        }

        #endregion

        #region Delete

        //[TestMethod]
        //public void DelCustomer_Test()
        //{
        //    var response = baseService.DeleteByIdAsync("29337", "customer", "crud").Result;
        //    CheckBooleanResponse(response);
        //}

        [TestMethod]
        public void DelCustomerByExternalId_Test()
        {
            //var id = GetIdByExternalId(TestCustomerGuid);
            var id = "28481";
            var response = baseService.DeleteByIdAsync(id, "customer", "crud").Result;
            CheckBooleanResponse(response);
        }

        #endregion

        #endregion


        #region Partner Tests

        #region Read

        [TestMethod]
        public void GetPartnerJsonStringById_Test()
        {
            var response = baseService.GetJsonStringById("27661", "partner", "crud");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void GetPartnerJsonStringByIdAsync_Test()
        {
            var response = baseService.GetJsonStringByIdAsync("22334", "partner", "crud").Result;
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void GetPartnerDynamicById_Test()
        {
            var response = baseService.GetDynamicById("22334", "partner", "crud");
            CheckDynamicResponse(response);
        }

        [TestMethod]
        public void GetPartnerDynamicByIdAsync_Test()
        {
            var response = baseService.GetDynamicByIdAsync("22334", "partner", "crud").Result;
            CheckDynamicResponse(response);
        }

        #endregion

        #region Search

        [TestMethod]
        public void SearchPartnerJsonStringByExternalId_Test()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJsonString("partner", filters, scriptKey: "search");
            CheckJsonStringResponse(response);

            var r = JArray.Parse(response.ResponseData);
            var id = r[0].Value<string>("id");
            response = baseService.GetJsonStringById(id, "partner", "crud");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void SearchPartnerJsonStringByExternalId_Test2()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchDynamicList("partner", filters, scriptKey: "search");
            dynamic ro = response.ResponseData.First();
            CheckDynamicListResponse(response);

            var response2 = baseService.GetJsonStringById(ro.id, "partner", "crud");
            CheckJsonStringResponse(response2);
        }

        [TestMethod]
        public void SearchPartnerJsonStringByExternalId_Test3()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchDynamicList("partner", filters, columns, scriptKey: "search");

            if (response.HasExceptions) Assert.Fail(response.ExceptionList.ToJson(true));
            if (response.ResponseData == null) Assert.Fail("No records found!");
            dynamic ro = response.ResponseData.First();
            CheckDynamicListResponse(response);

            var response2 = baseService.GetJsonStringById(ro.id, "partner", "crud");
            CheckJsonStringResponse(response2);
        }

        [TestMethod]
        public void SearchPartnerArrayByExternalId_Test()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync("partner", filters, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchPartnerJArrayByExternalId_Test2()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync("partner", filters, columns, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchPartnerJObjectsByExternalId_Test()
        {
            var externalid = TestCustomerGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("partner", filters, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        [TestMethod]
        public void SearchPartnerJObjectsByExternalId_Test2()
        {
            var externalid = TestCustomerGuid;
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("partner", filters, columns, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        #endregion

        #region Create

        [TestMethod]
        public void PostPartner_Test()
        {
            dynamic o = new ExpandoObject();
            o.phone = "2163734600";
            o.mobilephone = "2165132288";
            o.recordtype = "partner";
            o.email = "jkosh@broadvox.com";
            o.companyname = "MYPARTNER";
            o.url = "http://www.nfl.com";
            o.subsidiary = "2";
            //o.category = "1"; /* TODO: Add customer and partner category items to sandbox from production */
            o.isperson = "F";
            //o.partner = "352";
            o.entitystatus = "13";
            o.externalid = TestCustomerGuid;

            var response = baseService.Create("customer", o, "crud");
            CheckJsonStringResponse(response);
        }


        [TestMethod]
        public void PostPartnerJson_Test()
        {
            dynamic o = new ExpandoObject();
            o.phone = "2163734600";
            o.mobilephone = "2165132288";
            o.recordtype = "partner";
            o.email = "jkosh@broadvox.com";
            o.companyname = "TEST";
            o.url = "http://www.nfl.com";
            o.subsidiary = "2";
            o.category = "1"; /* TODO: Add customer and partner category items to sandbox from production */
            o.isperson = "F";
            o.partner = "27661";
            o.entitystatus = "13";
            //o.externalid = TestCustomerGuid;
            //string json = JObject.FromObject(o).ToString();
            string json = JsonExpandoStringSerialization.ToJsonString(o);

            var response = baseService.CreateByJsonString("customer", json, "crud");
            CheckJsonStringResponse(response);
        }

        #endregion

        #region Update

        [TestMethod]
        public void PutPartnerByExternalId_Test()
        {
            //var id = GetIdByExternalId(TestCustomerGuid);
            var id = "352";
            if (!string.IsNullOrWhiteSpace(id))
            {
                dynamic o = new ExpandoObject();
                o.comments = "null";
                //o.phone = "2163734626";
                var response = baseService.Update(id, "partner", o, "crud");
                CheckJsonStringResponse(response);
            }
            else
            {
                Assert.Fail();
            }
        }

        #endregion

        #region Delete

        [TestMethod]
        public void DelPartnerByExternalId_Test()
        {
            var id = "29737";
            //var id = GetIdByExternalId(TestCustomerGuid);
            var response = baseService.DeleteByIdAsync(id, "partner", "crud").Result;
            CheckBooleanResponse(response);
        }

        #endregion

        #endregion

        #endregion



        //[TestMethod]
        //public void PostCC_Test()
        //{
        //    var externalid = TestCustomerGuid;
        //    var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
        //    //var response = baseService.SearchJsonString("customer", filters, scriptKey: "search");
        //    var response = baseService.CreateByDynamic()
        //    CheckJsonStringResponse(response);

        //    var r = JArray.Parse(response.ResponseData);
        //    var id = r[0].Value<string>("id");


        //    response = baseService.GetJsonStringById(id, "customer", "crud");
        //    CheckJsonStringResponse(response);
        //}

        #endregion


        #region Private Utility Methods

        private void CheckJsonStringResponse(IResponseObject<string, string> response)
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

        private string GetIdByExternalId(string externalid)
        {
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("customer", filters, scriptKey: "search").Result;
            var responseData = response.ResponseData.ToList();
            if (!responseData.Any()) return null;
            if (responseData.Count()>1) throw new Exception("Duplicated Records Found!");
            return responseData[0].Value<string>("id");
        }

        #endregion

    }
}
