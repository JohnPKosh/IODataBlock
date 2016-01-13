using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Common.GenericResponses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NsRest;
using NsRest.Search;
using NsRest.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace BasicTests.Integration.NsRest.Production
{
    [TestClass]
    public class CustomerTests
    {
        #region Class Initialization

        public CustomerTests()
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
                //{"cc_crud", NetSuiteScriptSetting.Create("customscript_cr_cc_crud", "customdeploy_cr_cc_crud")},
                {"search", NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search")}
            };
            baseService = BaseService.Create(NsBaseUrl, NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, NsRole), scriptSettings);

            TestGuid = "00000000-0000-0000-0000-000000000000";
            TestId = "28686";
            TypeName = "customer";
        }

        private ConfigMgr configMgr { get; set; }

        private string NsBaseUrl { get; set; }
        private string NsAccount { get; set; }
        private string NsEmail { get; set; }
        private string NsRole { get; set; }
        private string NsPassword { get; set; }
        private BaseService baseService { get; set; }
        private IDictionary<string, INetSuiteScriptSetting> scriptSettings { get; set; }

        private string TestGuid { get; set; }
        private string TestId { get; set; }
        private string TypeName { get; set; }

        #endregion Class Initialization

        #region Test Methods

        #region Read

        [TestMethod]
        public void GetJsonById_Test()
        {
            var response = baseService.GetJsonById(TestId, TypeName, "crud");
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void GetJsonByIdAsync_Test()
        {
            var response = baseService.GetJsonByIdAsync(TestId, TypeName, "crud").Result;
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void GetDynamicById_Test()
        {
            var response = baseService.GetDynamicById(TestId, TypeName, "crud");
            CheckDynamicResponse(response);
        }

        [TestMethod]
        public void GetDynamicByIdAsync_Test()
        {
            var response = baseService.GetDynamicByIdAsync(TestId, TypeName, "crud").Result;
            CheckDynamicResponse(response);
        }

        [TestMethod]
        public void GetJsonByExternalId_Test()
        {
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.GetJsonById(TestGuid, TypeName, "crud");
            baseService.UseExternalId = false; /* Use NS ID */
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void GetJsonByExternalIdAsync_Test()
        {
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.GetJsonByIdAsync(TestGuid, TypeName, "crud").Result;
            baseService.UseExternalId = false; /* Use NS ID */
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void GetDynamicByExternalId_Test()
        {
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.GetDynamicById(TestGuid, TypeName, "crud");
            baseService.UseExternalId = false; /* Use NS ID */
            CheckDynamicResponse(response);
        }

        [TestMethod]
        public void GetDynamicByExternalIdAsync_Test()
        {
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.GetDynamicByIdAsync(TestGuid, TypeName, "crud").Result;
            baseService.UseExternalId = false; /* Use NS ID */
            CheckDynamicResponse(response);
        }

        #endregion Read

        #region Search

        [TestMethod]
        public void SearchJsonWithoutFilters_Test()
        {
            var externalid = TestGuid;
            var columns = new[] { "companyname", "custentity_cr_ext_cross_id" };
            //var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJson(TypeName, null, columns, scriptKey: "search");
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void SearchJson_Test()
        {
            var externalid = TestGuid;
            var columns = new[] { "companyname", "custentity_cr_ext_cross_id" };
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJson(TypeName, null, columns, scriptKey: "search");
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void SearchJsonByExternalId_Test()
        {
            var externalid = TestGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJson(TypeName, filters, scriptKey: "search");
            CheckJsonResponse(response);

            var r = JArray.Parse(response.ResponseData);
            var id = r[0].Value<string>("id");
            response = baseService.GetJsonById(id, TypeName, "crud");
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void SearchDynamicListByExternalId_Test()
        {
            var externalid = TestGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchDynamicList(TypeName, filters, scriptKey: "search");
            dynamic ro = response.ResponseData.First();
            CheckDynamicListResponse(response);

            var response2 = baseService.GetJsonById(ro.id, TypeName, "crud");
            CheckJsonResponse(response2);
        }

        [TestMethod]
        public void SearchJsonByExternalId_Test3()
        {
            var externalid = TestGuid;
            var columns = new[] { "custentity_cr_ext_cross_id" };
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchDynamicList(TypeName, filters, columns, scriptKey: "search");

            if (response.HasExceptions) Assert.Fail(response.ExceptionList.ToJson(true));
            if (response.ResponseData == null) Assert.Fail("No records found!");
            dynamic ro = response.ResponseData.First();
            CheckDynamicListResponse(response);

            var response2 = baseService.GetJsonById(ro.id, TypeName, "crud");
            CheckJsonResponse(response2);
        }

        [TestMethod]
        public void SearchJArrayByExternalId_Test()
        {
            var externalid = TestGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync(TypeName, filters, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchJArrayByExternalId_Test2()
        {
            var externalid = TestGuid;
            var columns = new[] { "custentity_cr_ext_cross_id" };
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync(TypeName, filters, columns, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchJObjectsByExternalId_Test()
        {
            var externalid = TestGuid;
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync(TypeName, filters, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        [TestMethod]
        public void SearchJObjectsByExternalId_Test2()
        {
            var externalid = TestGuid;
            var columns = new[] { "custentity_cr_ext_cross_id" };
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync(TypeName, filters, columns, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        #endregion Search

        #region Create

        [TestMethod]
        public void Post_Test()
        {
            dynamic o = new ExpandoObject();
            o.phone = "2163734600";
            o.mobilephone = "2165132288";
            o.recordtype = TypeName;
            o.email = "jkosh@myco.com";
            o.companyname = "MYCO";
            o.url = "http://www.myco.com";
            o.subsidiary = "2";
            o.category = "6"; /* TODO: Add customer and partner category items to sandbox from production */
            o.isperson = "F";
            //o.partner = "352";
            o.entitystatus = "13";
            //o.externalid = TestGuid;

            var response = baseService.Create(TypeName, o, "crud");
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void PostJson_Test()
        {
            dynamic o = new ExpandoObject();
            o.phone = "2163734699";
            o.mobilephone = "2165132289";
            o.recordtype = TypeName;
            o.email = "jkosh@mytestorg.com";
            o.companyname = "mytestorg";
            o.url = "http://www.mytestorg.com";
            o.subsidiary = "2";
            o.category = "6"; /* TODO: Add customer and partner category items to sandbox from production */
            o.isperson = "F";
            //o.partner = "352";
            o.entitystatus = "13";
            o.custentity_cr_ext_cross_id = TestGuid;
            //o.externalid = TestGuid;
            //string json = JObject.FromObject(o).ToString();
            string json = JsonExpandoStringSerialization.ToJsonString(o);

            var response = baseService.CreateByJson(TypeName, json, "crud");
            CheckJsonResponse(response);
        }

        #endregion Create

        #region Update

        [TestMethod]
        public void Put_Test()
        {
            dynamic o = new ExpandoObject();
            o.comments = "null";
            var response = baseService.Update(TestId, TypeName, o, "crud");
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void PutByExternalId_Test()
        {
            dynamic o = new ExpandoObject();
            o.comments = "Test Customer";
            //o.custentity_cr_ext_cross_id = "null";
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.Update(TestGuid, TypeName, o, "crud");
            baseService.UseExternalId = false; /* Use NS ID */
            CheckJsonResponse(response);
        }

        #endregion Update

        #region Delete

        [TestMethod]
        public void Del_Test()
        {
            var response = baseService.DeleteByIdAsync(TestId, TypeName, "crud").Result;
            CheckBooleanResponse(response);
        }

        [TestMethod]
        public void DelByExternalId_Test()
        {
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.DeleteByIdAsync(TestGuid, TypeName, "crud").Result;
            baseService.UseExternalId = false; /* Use NS ID */

            CheckBooleanResponse(response);
        }

        #endregion Delete

        #endregion Test Methods

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

        private string GetIdByExternalId(string externalid)
        {
            var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync(TypeName, filters, scriptKey: "search").Result;
            var responseData = response.ResponseData.ToList();
            if (!responseData.Any()) return null;
            if (responseData.Count() > 1) throw new Exception("Duplicated Records Found!");
            return responseData[0].Value<string>("id");
        }

        #endregion Private Utility Methods
    }
}