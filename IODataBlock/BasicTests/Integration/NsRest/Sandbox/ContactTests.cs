using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NsRest.Services;

namespace BasicTests.Integration.NsRest.Sandbox
{
    [TestClass]
    public class ContactTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            NsRestContactHelper helper = new NsRestContactHelper();
            //iar.kor@test.com
            var contact = helper.SearchJObjectsByEmail(@"iar.kor@test.com");
            if (contact != null)
            {
                var companyItem = contact["columns"]["company"];
                var NsCompanyName = companyItem.Value<string>("name");
            }
        }

        [TestMethod]
        public void TestById()
        {
            var baseService = BaseService.Create(true);
            var json = baseService.GetJsonById("200030", "contact");
            Assert.IsNotNull(json);
        }


    }
}
