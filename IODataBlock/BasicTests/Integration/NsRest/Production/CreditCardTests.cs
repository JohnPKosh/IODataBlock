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
            //NsBaseUrl = configMgr.GetAppSetting("nsbaseurl");
            NsBaseUrl = configMgr.GetAppSetting("nssandboxurl");
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

            TestGuid = "76dddbbc-b42b-4a08-bc30-2383a9170e60";
            TestId = "6";
            TestCreditCardId = "2512";
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

            var JArrayResponse = response.TransformResponseData(new Func<string, JArray>(JArray.Parse));
            CheckJArrayResponse(JArrayResponse);
            
        }

        [TestMethod]
        public void ReadAllByExternalId_Test()
        {
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.ReadByJsonAsync(TestGuid, null, "cc_crud").Result;
            baseService.UseExternalId = false; /* Use NS ID */
            CheckJsonResponse(response);

            var JArrayResponse = response.TransformResponseData(new Func<string, JArray>(JArray.Parse));
            CheckJArrayResponse(JArrayResponse);

        }

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

        [TestMethod]
        public void UpdateByExternalId_Test()
        {
            dynamic o = new ExpandoObject();
            var ccs = new List<dynamic>();

            dynamic cc1 = new ExpandoObject();
            cc1.internalid = TestCreditCardId;
            cc1.ccdefault = "F";
            cc1.ccexpiredate = "7/2017";
            cc1.ccname = "Vasiliy Popovich";
            cc1.ccnumber = "4242424242424242";
            cc1.customercode = "377";
            cc1.ccsecuritycode = "378";
            ccs.Add(cc1);
            o.creditcards = ccs;

            string json = JsonExpandoStringSerialization.ToJsonString(o);
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.UpdateByJsonAsync(TestGuid, json, "cc_crud").Result;
            baseService.UseExternalId = false; /* Use NS ID */
            CheckJsonResponse(response);
        }

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

        [TestMethod]
        public void DeleteByExternalId_Test()
        {
            dynamic o = new ExpandoObject();
            var ccs = new List<dynamic>();

            dynamic cc1 = new ExpandoObject();
            cc1.internalid = TestCreditCardId;
            ccs.Add(cc1);
            o.creditcards = ccs;

            string json = JsonExpandoStringSerialization.ToJsonString(o);
            baseService.UseExternalId = true; /* Use External CROSS ID */
            var response = baseService.DeleteByJsonAsync(TestId, json, "cc_crud").Result;
            baseService.UseExternalId = false; /* Use NS ID */
            CheckJsonResponse(response);
        }

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

        #endregion

    }
}
