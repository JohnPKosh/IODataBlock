using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Common.IO;
using Business.Common.System.States;
using Flurl;
using Flurl.Http;
using HubSpot.Models.Contacts;
using HubSpot.Models.Properties;
using HubSpot.Services.Contacts;
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
            var jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"ContactPropertyList.json");
            _propertyManager = new ContactPropertyManager(new ContactPropertyService(_hapiKey), new JsonFileLoader(new FileInfo(jsonFilePath)));
        }

        private readonly string _hapiKey;
        private readonly IPropertyManager _propertyManager;

        [TestMethod]
        public void PropertyManagerTest()
        {
            var props =_propertyManager.Properties;
            var lastUpdated = _propertyManager.LastUpdated;
            Assert.IsNotNull(props);
            Assert.IsNotNull(lastUpdated.Value);
        }




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
            .GetJsonAsync<List<PropertyTypeModel>>().Result.OrderBy(x=>x.name);

            if (result == null) Assert.Fail();
            else
            {
                result.WriteJsonToFilePath(@"SampleResults\allPropertiesDto.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }

        [TestMethod]
        public void GetAllPropertiesTest2()
        {
            /* https://api.hubapi.com/contacts/v2/properties?hapikey=demo */

            var result = "https://api.hubapi.com/contacts/v2/properties"
            .SetQueryParam("hapikey", _hapiKey)
            .GetJsonAsync<List<PropertyTypeModel>>().Result;

            if (result == null) Assert.Fail();
            else
            {
                Dictionary<string, string> d = new Dictionary<string, string>();
                foreach (var r in result.OrderBy(x=> x.name))
                {
                    d.Add(r.name, r.type);
                }
                d.WriteJsonToFilePath(@"c:\junk\contactpropnames.json");
            }
        }

        [TestMethod]
        public void GetAllPropertyTypes()
        {
            /* https://api.hubapi.com/contacts/v2/properties?hapikey=demo */

            var result = "https://api.hubapi.com/contacts/v2/properties"
            .SetQueryParam("hapikey", _hapiKey)
            .GetJsonAsync<List<PropertyTypeModel>>().Result;

            if (result == null) Assert.Fail();
            else
            {
                HashSet<string> d = new HashSet<string>();
                foreach (var r in result.OrderBy(x => x.name))
                {
                    d.Add(r.type);
                }
                d.WriteJsonToFilePath(@"c:\junk\contactproptypes.json");
            }
        }
    }
}
