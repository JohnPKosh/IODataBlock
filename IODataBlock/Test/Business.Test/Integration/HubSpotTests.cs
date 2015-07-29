using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Fasterflect;
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

    }
}
