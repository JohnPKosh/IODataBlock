using Business.Common.Configuration;
using HubSpot.Services.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Business.Common.Extensions;

namespace Business.Test.Integration
{
    /// <summary>
    /// Summary description for CompanyServiceTests
    /// </summary>
    [TestClass]
    public class CompanyServiceTests
    {
        #region Class Initialization

        public CompanyServiceTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
        }

        private readonly string _hapiKey;
        private TestContext testContextInstance; 


        #endregion



        [TestMethod]
        public void GetById()
        {
            var service = new CompanyService(_hapiKey);

            var contact = service.GetByIdViewModel(92251969);
            contact.WriteJsonToFilePath(@"c:\junk\companyById_92251969.json", new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
        }

        [TestMethod]
        public void GetByDomain()
        {
            var service = new CompanyService(_hapiKey);

            var contact = service.GetByDomainViewModel("broadvox.com");
            contact.WriteJsonToFilePath(@"c:\junk\companyByDomain_myforefronttech.json", new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
        }

        [TestMethod]
        public void GetAllContacts()
        {
            var service = new CompanyService(_hapiKey);

            var contact = service.GetAllContactsDynamic(112942323);
            contact.WriteJsonToFilePath(@"c:\junk\companyContacts_myforefronttech.json", new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
        }


        [TestMethod]
        public void GetAllContactIds()
        {
            var service = new CompanyService(_hapiKey);

            var contact = service.GetAllContactIdsDynamic(112942323);
            contact.WriteJsonToFilePath(@"c:\junk\companyContactIds_myforefronttech.json", new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
        }


        [TestMethod]
        public void GetAllContactViewModels()
        {
            var service = new CompanyService(_hapiKey);

            var contact = service.GetAllContactViewModels(69585958, 1);
            contact.WriteJsonToFilePath(@"c:\junk\companyContactViewModels_broadvox.json", new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            });
        }

        //112942323

    }
}
