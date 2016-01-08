using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Common.GenericResponses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        }

        private ConfigMgr configMgr { get; set; }

        private string NsBaseUrl { get; set; }
        private string NsAccount { get; set; }
        private string NsEmail { get; set; }
        private string NsRole { get; set; }
        private string NsPassword { get; set; }
        private BaseService baseService { get; set; }
        private IDictionary<string, INetSuiteScriptSetting> scriptSettings { get; set; }

        #endregion

        #region Test Methods

        #region Get Tests

        [TestMethod]
        public void GetCustomerJsonStringById_Test()
        {
            var response = baseService.GetJsonStringById("22334", "customer", "crud");
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

        #region Search Tests

        [TestMethod]
        public void SearchCustomerJsonStringByExternalId_Test()
        {
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
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
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
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
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
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
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync("customer", filters, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJArrayByExternalId_Test2()
        {
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJArrayAsync("customer", filters, columns, scriptKey: "search").Result;
            CheckJArrayResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJObjectsByExternalId_Test()
        {
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("customer", filters, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        [TestMethod]
        public void SearchCustomerJObjectsByExternalId_Test2()
        {
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
            var columns = new[] { "externalid" };
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("customer", filters, columns, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
        }

        #endregion

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
            o.externalid = Guid.NewGuid().ToString();
            var response = baseService.CreateByDynamic("customer", o, "crud");
            CheckJsonStringResponse(response);
        }

        //[TestMethod]
        //public void PostCC_Test()
        //{
        //    var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
        //    var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
        //    //var response = baseService.SearchJsonString("customer", filters, scriptKey: "search");
        //    var response = baseService.CreateByDynamic()
        //    CheckJsonStringResponse(response);

        //    var r = JArray.Parse(response.ResponseData);
        //    var id = r[0].Value<string>("id");


        //    response = baseService.GetJsonStringById(id, "customer", "crud");
        //    CheckJsonStringResponse(response);
        //}


        [TestMethod]
        public void PutCustomer_Test()
        {
            dynamic o = new ExpandoObject();
            o.comments = "Mad Scientist";
            var response = baseService.UpdateByDynamic("28637", "customer", o, "crud");
            CheckJsonStringResponse(response);
        }

        [TestMethod]
        public void DelCustomer_Test()
        {
            var response = baseService.DeleteByIdAsync("28637", "customer", "crud").Result;
            CheckBooleanResponse(response);
        }


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

        #endregion

    }
}
