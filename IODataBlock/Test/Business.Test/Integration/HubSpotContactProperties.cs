using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Flurl;
using Flurl.Http;
using HubSpot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Business.Test.Integration
{
    [TestClass]
    public class HubSpotContactProperties
    {

        public HubSpotContactProperties()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
        }

        private readonly string _hapiKey;

        /* http://developers.hubspot.com/docs/methods/contacts/v2/get_contacts_properties */


        [TestMethod]
        public void GetAllPropertiesDynamicTest()
        {
            /* https://api.hubapi.com/contacts/v2/properties?hapikey=demo */

            var result = "https://api.hubapi.com/contacts/v2/properties"
            .SetQueryParam("hapikey", _hapiKey)
            .GetJsonAsync<List<ExpandoObject>>().Result;

            if (result == null) Assert.Fail();
            else
            {
                result.WriteJsonToFilePath(@"SampleResults\allPropertiesDynamic.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }

        [TestMethod]
        public void GetAllPropertiesTest()
        {
            /* https://api.hubapi.com/contacts/v2/properties?hapikey=demo */

            var result = "https://api.hubapi.com/contacts/v2/properties"
            .SetQueryParam("hapikey", _hapiKey)
            .GetJsonAsync<List<ContactPropertyDto>>().Result;

            if (result == null) Assert.Fail();
            else
            {
                result.WriteJsonToFilePath(@"SampleResults\allPropertiesDto.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }
    }
}
