using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests.Integration.NsRest.Sandbox
{
    [TestClass]
    public class ContactTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            NsRestContactHelper helper = new NsRestContactHelper();
            var contact = helper.SearchJObjectsByEmail(@"jkosh@cloudroute.com");
            if (contact != null)
            {
                var companyItem = contact["columns"]["company"];
                var NsCompanyName = companyItem.Value<string>("name");
            }


        }
    }
}
