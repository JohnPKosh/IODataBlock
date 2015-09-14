using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Fasterflect;
using Flurl;
using Flurl.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using HubSpot.Models;
using Business.Common.Extensions;
using Version = HubSpot.Models.Version;

namespace Business.Test.Integration
{
    [TestClass]
    public class HubSpotTests
    {

        public HubSpotTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
        }

        private readonly string _hapiKey;


        [TestMethod]
        public void ReadWriteContact()
        {
            var contact = File.ReadAllText(@"SampleResults\contact.json").ConvertJson<ContactDto>();

            Assert.IsNotNull(contact.properties);
            Assert.IsNotNull(contact);

            contact.WriteJsonToFilePath(@"SampleResults\contactClone.json");
        }

        [TestMethod]
        public void ReadWriteContactById()
        {
            var contact = File.ReadAllText(@"SampleResults\contactById.json").ConvertJson<ContactDto>();

            Assert.IsNotNull(contact.properties);
            Assert.IsNotNull(contact);

            contact.WriteJsonToFilePath(@"SampleResults\contactByIdClone.json");
        }

        [TestMethod]
        public void ReadWriteContactList()
        {
            var contact = File.ReadAllText(@"SampleResults\contactListAll.json").ConvertJson<ContactListDto>();
            
            Assert.IsNotNull(contact);

            contact.WriteJsonToFilePath(@"SampleResults\contactListAllClone.json");
        }



        [TestMethod]
        public void ReadWriteContactListQuery()
        {
            var contact = File.ReadAllText(@"SampleResults\contactListQuery.json").ConvertJson<ContactListDto>();

            Assert.IsNotNull(contact);

            contact.WriteJsonToFilePath(@"SampleResults\contactListQueryClone.json");
        }


        [TestMethod]
        public void GetWithFlurl()
        {
            // https://api.hubapi.com/contacts/v1/contact/vid/3/profile?hapikey=your+key+here

            var result = "https://api.hubapi.com/contacts/v1/contact/vid"
                .AppendPathSegment("6")
                .AppendPathSegment("profile")
                .SetQueryParam("hapikey", _hapiKey)
                .GetJsonAsync<ContactDto>().Result;

            if (result == null) Assert.Fail();
            else
            {
                var vid = result.vid;
                Assert.IsNotNull(vid);
                result.WriteJsonToFilePath(@"SampleResults\contactByIdClone.json", new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate});
            }
        }

    }
}
