using System;
using System.Collections.Generic;
using Business.Common.Configuration;
using Business.Common.Extensions;
using HubSpot.Models;
using HubSpot.Services;
using HubSpot.Services.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Business.Test.Integration
{
    [TestClass]
    public class FormsServiceTests
    {

        public FormsServiceTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
        }

        private readonly string _hapiKey;


        [TestMethod]
        public void TestMethod1()
        {
            var service = new FormsService(_hapiKey);
            var ro = service.GetForms();
            if (ro.HasExceptions)
            {
                Assert.Fail();
            }
            else
            {
                var data = ro.ResponseData;
                var dto = data.ConvertJson<List<JObject>>();
                if(dto == null)Assert.Fail();

                dto.WriteJsonToFilePath(@"c:\junk\formlist.json");
            }
        }
    }
}
