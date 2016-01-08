using System.Collections.Generic;
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
            CheckDynamicResponse(ro);

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
        public void SearchCustomerJObjectsByExternalId_Test()
        {
            var externalid = "d0973491-df5e-4bcd-b9b6-9340fc00a5f0";
            var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
            var response = baseService.SearchJObjectsAsync("customer", filters, scriptKey: "search").Result;
            CheckJObjectsResponse(response);
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



        #endregion

    }
}
