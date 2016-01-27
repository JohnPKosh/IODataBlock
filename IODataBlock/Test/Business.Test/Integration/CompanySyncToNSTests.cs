using Business.Common.Configuration;
using HubSpot.Models.Companies;
using HubSpot.Models.Contacts;
using HubSpot.Services.Companies;
using HubSpot.Services.Contacts;
using HubSpot.Services.ModeTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Business.Test.Integration
{
    /// <summary>
    /// Summary description for CompanySyncToNSTests
    /// </summary>
    [TestClass]
    public class CompanySyncToNSTests
    {
        #region Test Initialization

        public CompanySyncToNSTests()
        {
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            NsAccount = configMgr.GetAppSetting("nsaccount");
            NsEmail = configMgr.GetAppSetting("nsemail");
            NsPassword = configMgr.GetAppSetting("nspassword");
        }

        private readonly string _hapiKey;
        private string NsAccount { get; set; }
        private string NsEmail { get; set; }
        private string NsPassword { get; set; }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Additional test attributes

        #endregion Test Initialization

        [TestMethod]
        public void FindCompaniesByDomainTest()
        {
            var contacts = GetAllContactViewModels().ToList();

            foreach (var c in contacts)
            {
                var email = c.identity_profiles.First(x => x.vid == c.vid).identities.First(y => y.type == "EMAIL").value;
            }
        }

        #region Utility Methods

        public IEnumerable<ContactViewModel> GetAllContactViewModels(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            var service = new ContactService(_hapiKey);
            return service.GetAllContactViewModels(count, vidOffset, properties, propertyMode, formSubmissionMode, showListMemberships).ToList();
        }

        public IEnumerable<CompanyViewModel> GetCompaniesByDomain(string domain)
        {
            var service = new CompanyService(_hapiKey);
            return service.GetByDomainViewModel("broadvox.com");
        }

        #endregion Utility Methods
    }
}