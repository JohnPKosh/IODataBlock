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

namespace BasicTests.Integration.NsRest.Production
{
    [TestClass]
    public class CreditCardTests
    {
        #region Class Initialization

        public CreditCardTests()
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
                //{"crud", NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud")},
                {"cc_crud", NetSuiteScriptSetting.Create("customscript_cr_cc_crud", "customdeploy_cr_cc_crud")},
                {"search", NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search")}
            };
            baseService = CreditCardService.Create(NsBaseUrl, NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, NsRole), scriptSettings);

            TestGuid = "952bf0d0-fdf8-4011-9493-8df388313b95";
            TestId = "6";
            TestCreditCardId = "1115";
            TypeName = "customer";
        }

        private ConfigMgr configMgr { get; set; }

        private string NsBaseUrl { get; set; }
        private string NsAccount { get; set; }
        private string NsEmail { get; set; }
        private string NsRole { get; set; }
        private string NsPassword { get; set; }
        private CreditCardService baseService { get; set; }
        private IDictionary<string, INetSuiteScriptSetting> scriptSettings { get; set; }

        private string TestGuid { get; set; }
        private string TestId { get; set; }
        private string TestCreditCardId { get; set; }
        private string TypeName { get; set; }

        #endregion

        #region Test Methods

        //#region Read

        //[TestMethod]
        //public void GetJsonById_Test()
        //{
        //    var response = baseService.GetJsonById(TestId, TypeName, "crud");
        //    CheckJsonResponse(response);
        //}

        //[TestMethod]
        //public void GetJsonByIdAsync_Test()
        //{
        //    var response = baseService.GetJsonByIdAsync(TestId, TypeName, "crud").Result;
        //    CheckJsonResponse(response);
        //}

        //[TestMethod]
        //public void GetDynamicById_Test()
        //{
        //    var response = baseService.GetDynamicById(TestId, TypeName, "crud");
        //    CheckDynamicResponse(response);
        //}

        //[TestMethod]
        //public void GetDynamicByIdAsync_Test()
        //{
        //    var response = baseService.GetDynamicByIdAsync(TestId, TypeName, "crud").Result;
        //    CheckDynamicResponse(response);
        //}

        //#endregion

        //#region Search

        //[TestMethod]
        //public void SearchJsonWithoutFilters_Test()
        //{
        //    var externalid = TestGuid;
        //    var columns = new[] { "companyname", "custentity_cr_ext_cross_id" };
        //    //var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJson(TypeName, null, columns, scriptKey: "search");
        //    CheckJsonResponse(response);
        //}

        //[TestMethod]
        //public void SearchJson_Test()
        //{
        //    var externalid = TestGuid;
        //    var columns = new[] { "companyname", "custentity_cr_ext_cross_id" };
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJson(TypeName, null, columns, scriptKey: "search");
        //    CheckJsonResponse(response);
        //}

        //[TestMethod]
        //public void SearchJsonByExternalId_Test()
        //{
        //    var externalid = TestGuid;
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJson(TypeName, filters, scriptKey: "search");
        //    CheckJsonResponse(response);

        //    var r = JArray.Parse(response.ResponseData);
        //    var id = r[0].Value<string>("id");
        //    response = baseService.GetJsonById(id, TypeName, "crud");
        //    CheckJsonResponse(response);
        //}

        //[TestMethod]
        //public void SearchDynamicListByExternalId_Test()
        //{
        //    var externalid = TestGuid;
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchDynamicList(TypeName, filters, scriptKey: "search");
        //    dynamic ro = response.ResponseData.First();
        //    CheckDynamicListResponse(response);

        //    var response2 = baseService.GetJsonById(ro.id, TypeName, "crud");
        //    CheckJsonResponse(response2);
        //}

        //[TestMethod]
        //public void SearchJsonByExternalId_Test3()
        //{
        //    var externalid = TestGuid;
        //    var columns = new[] { "custentity_cr_ext_cross_id" };
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchDynamicList(TypeName, filters, columns, scriptKey: "search");

        //    if (response.HasExceptions) Assert.Fail(response.ExceptionList.ToJson(true));
        //    if (response.ResponseData == null) Assert.Fail("No records found!");
        //    dynamic ro = response.ResponseData.First();
        //    CheckDynamicListResponse(response);

        //    var response2 = baseService.GetJsonById(ro.id, TypeName, "crud");
        //    CheckJsonResponse(response2);
        //}

        //[TestMethod]
        //public void SearchJArrayByExternalId_Test()
        //{
        //    var externalid = TestGuid;
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJArrayAsync(TypeName, filters, scriptKey: "search").Result;
        //    CheckJArrayResponse(response);
        //}

        //[TestMethod]
        //public void SearchJArrayByExternalId_Test2()
        //{
        //    var externalid = TestGuid;
        //    var columns = new[] { "custentity_cr_ext_cross_id" };
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJArrayAsync(TypeName, filters, columns, scriptKey: "search").Result;
        //    CheckJArrayResponse(response);
        //}

        //[TestMethod]
        //public void SearchJObjectsByExternalId_Test()
        //{
        //    var externalid = TestGuid;
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJObjectsAsync(TypeName, filters, scriptKey: "search").Result;
        //    CheckJObjectsResponse(response);
        //}

        //[TestMethod]
        //public void SearchJObjectsByExternalId_Test2()
        //{
        //    var externalid = TestGuid;
        //    var columns = new[] { "custentity_cr_ext_cross_id" };
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJObjectsAsync(TypeName, filters, columns, scriptKey: "search").Result;
        //    CheckJObjectsResponse(response);
        //}

        //#endregion

        #region Create

        [TestMethod]
        public void Create_Test()
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

            var response = baseService.Create(TestId, o, "cc_crud");
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void CreateByJsonAsync_Test()
        {
            dynamic o = new ExpandoObject();
            var ccs = new List<dynamic>();

            dynamic cc1 = new ExpandoObject();
            /* Add Credit Card Info */
            cc1.ccdefault = "T";
            cc1.ccexpiredate = "07/2027";
            cc1.ccname = "Test Rest JSON";
            cc1.ccnumber = "4242424242424242";
            cc1.customercode = "378";
            cc1.ccsecuritycode = "378";

            ccs.Add(cc1);
            o.creditcards = ccs;

            string json = JsonExpandoStringSerialization.ToJsonString(o);

            var response = baseService.CreateByJsonAsync(TestId, json, "cc_crud").Result;
            CheckJsonResponse(response);
        }

        #endregion

        #region Read

        [TestMethod]
        public void ReadByJsonAsync_Test()
        {
            dynamic o = new ExpandoObject();
            var ccs = new List<dynamic>();

            dynamic cc1 = new ExpandoObject();
            cc1.internalid = TestCreditCardId;
            ccs.Add(cc1);
            o.creditcards = ccs;

            string json = JsonExpandoStringSerialization.ToJsonString(o);
            var response = baseService.ReadByJsonAsync(TestId, json, "cc_crud").Result;
            CheckJsonResponse(response);
        }

        [TestMethod]
        public void ReadAllByJsonAsync_Test()
        {
            var response = baseService.ReadByJsonAsync(TestId, null, "cc_crud").Result;
            CheckJsonResponse(response);
        }

        //[TestMethod]
        //public void ReadByExternalId_Test()
        //{
        //    var id = GetIdByExternalId(TestGuid);

        //    dynamic o = new ExpandoObject();
        //    var ccs = new List<dynamic>();

        //    dynamic cc1 = new ExpandoObject();
        //    cc1.internalid = TestCreditCardId;
        //    ccs.Add(cc1);
        //    o.creditcards = ccs;

        //    var response = baseService.Read(id, o, "cc_crud");
        //    CheckJsonResponse(response);
        //}

        #endregion

        #region Update

        [TestMethod]
        public void UpdateByJsonAsync_Test()
        {
            dynamic o = new ExpandoObject();
            var ccs = new List<dynamic>();

            dynamic cc1 = new ExpandoObject();
            cc1.internalid = TestCreditCardId;
            cc1.ccdefault = "T";
            cc1.ccexpiredate = "08/2020";
            cc1.ccname = "TestAsync Update";
            cc1.ccnumber = "4242424242424242";
            cc1.customercode = "377";
            cc1.ccsecuritycode = "378";
            ccs.Add(cc1);
            o.creditcards = ccs;

            string json = JsonExpandoStringSerialization.ToJsonString(o);
            var response = baseService.UpdateByJsonAsync(TestId, json, "cc_crud").Result;
            CheckJsonResponse(response);
        }

        //[TestMethod]
        //public void UpdateByExternalId_Test()
        //{
        //    var id = GetIdByExternalId(TestGuid);
        //    if (!string.IsNullOrWhiteSpace(id))
        //    {
        //        dynamic o = new ExpandoObject();
        //        var ccs = new List<dynamic>();

        //        dynamic cc1 = new ExpandoObject();
        //        cc1.internalid = 815;
        //        cc1.ccdefault = "T";
        //        cc1.ccexpiredate = "08/2027";
        //        cc1.ccname = "Test Rest Update";
        //        cc1.ccnumber = "4242424242424242";
        //        cc1.customercode = "377";
        //        cc1.ccsecuritycode = "378";
        //        ccs.Add(cc1);
        //        o.creditcards = ccs;

        //        var response = baseService.Update(id, o, "crud");
        //        CheckJsonResponse(response);
        //    }
        //    else
        //    {
        //        Assert.Fail();
        //    }
        //}

        #endregion

        #region Delete

        [TestMethod]
        public void DeleteByJsonAsync_Test()
        {
            dynamic o = new ExpandoObject();
            var ccs = new List<dynamic>();

            dynamic cc1 = new ExpandoObject();
            cc1.internalid = TestCreditCardId;
            ccs.Add(cc1);
            o.creditcards = ccs;

            string json = JsonExpandoStringSerialization.ToJsonString(o);
            var response = baseService.DeleteByJsonAsync(TestId, json, "cc_crud").Result;

            CheckJsonResponse(response);
        }

        //[TestMethod]
        //public void DeleteByExternalId_Test()
        //{
        //    var id = GetIdByExternalId(TestGuid);

        //    dynamic o = new ExpandoObject();
        //    var ccs = new List<dynamic>();

        //    dynamic cc1 = new ExpandoObject();
        //    cc1.internalid = TestCreditCardId;
        //    ccs.Add(cc1);
        //    o.creditcards = ccs;

        //    var response = baseService.Delete(id, o, "cc_crud");
        //    CheckJsonResponse(response);
        //}

        #endregion

        #endregion

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

        //private string GetIdByExternalId(string externalid)
        //{
        //    var filters = new[] { NsSearchFilter.NewStringFilter("custentity_cr_ext_cross_id", SearchStringFieldOperatorType.Is, externalid) };
        //    var response = baseService.SearchJObjectsAsync(TypeName, filters, scriptKey: "search").Result;
        //    var responseData = response.ResponseData.ToList();
        //    if (!responseData.Any()) return null;
        //    if (responseData.Count() > 1) throw new Exception("Duplicated Records Found!");
        //    return responseData[0].Value<string>("id");
        //}

        #endregion

    }
}
