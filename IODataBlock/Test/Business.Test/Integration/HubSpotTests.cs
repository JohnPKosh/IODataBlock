﻿using System;
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
using HubSpot.Services;
using Business.Common.Extensions;
using Business.Common.System;
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
                .AppendPathSegment("37")
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


        [TestMethod]
        public void GetAllContacts()
        {
            // https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=your+key+here

            var result = "https://api.hubapi.com/contacts/v1/lists/all/contacts/all"
                .SetQueryParam("hapikey", _hapiKey)
                .GetJsonAsync<ContactListDto>().Result;

            if (result == null) Assert.Fail();
            else
            {
                //var vid = result.vid;
                //Assert.IsNotNull(vid);
                result.WriteJsonToFilePath(@"SampleResults\allContacts.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }

        [TestMethod]
        public void GetAllContactsDynamic()
        {
            // https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=your+key+here

            var result = "https://api.hubapi.com/contacts/v1/lists/all/contacts/all"
                .SetQueryParam("hapikey", _hapiKey)
                .GetJsonAsync<ExpandoObject>().Result;

            if (result == null) Assert.Fail();
            else
            {
                //var vid = result.vid;
                //Assert.IsNotNull(vid);
                result.WriteJsonToFilePath(@"SampleResults\allContactsdynamic.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }


        [TestMethod]
        public void GetContactDynamic()
        {
            // https://api.hubapi.com/contacts/v1/contact/vid/315/profile?hapikey=your+key+here

            var result = "https://api.hubapi.com/contacts/v1/contact/vid"
                .AppendPathSegment("315")
                .AppendPathSegment("profile")
                .SetQueryParam("hapikey", _hapiKey)
                .GetJsonAsync<ContactDto>().Result;

            if (result == null) Assert.Fail();
            else
            {
                var vid = result.vid;
                Assert.IsNotNull(vid);
                result.WriteJsonToFilePath(@"SampleResults\contactByIdDynamic.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }

        [TestMethod]
        public void GetCompanyWithFlurl()
        {
            // https://api.hubapi.com/companies/v2/companies/recent/modified?hapikey=your+key+here

            var result = "https://api.hubapi.com/companies/v2/companies/recent/modified"
                .SetQueryParam("hapikey", _hapiKey)
                .GetJsonAsync<ExpandoObject>().Result;

            if (result == null) Assert.Fail();
            else
            {
                //var vid = result.vid;
                //Assert.IsNotNull(vid);
                result.WriteJsonToFilePath(@"SampleResults\recentCompanies.json", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate });
            }
        }



        // mkanell@ajc.com
        // mfratto@nwc.co

        [TestMethod]
        public void GetContactByEmailFromService()
        {
            var service = new ContactService(_hapiKey);
            var props = new List<string> {"lastname", "firstname", "hs_email_optout_636817"};


            var ro = service.GetContactByEmail(@"ssalerno@ami-partners.com", props);
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = ClassExtensions.CreateFromJson<ContactDto>(data);

                //var optout = data.properties.hs_email_optout_636817;
                //DateTime? created = new UnixMsTimestamp(data.properties.createdate);
                //var value = optout.value;
            }
        }




    }
}
