using Business.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Configuration
{
    /// <summary>
    /// Summary description for ConfigMgrTests
    /// </summary>
    [TestClass]
    public class ConfigMgrTests
    {
        public ConfigMgrTests()
        {
            appSettingKey = "testKey";
            appSettingValue = "Hello World";

            appSettingTempKey = "tempKey";
            appSettingTempValue = "Only passing through.";

            connName = "TESTCONN";
            connString = "User ID=TESTER; Password=*******; Initial Catalog=TESTDB; Data Source=TESTSERVER; Connection Timeout=600;";
            connProvider = "System.Data.SqlClient";
        }

        private string appSettingKey;
        private string appSettingValue;

        private string appSettingTempKey;
        private string appSettingTempValue;

        private string connString;
        private string connProvider;
        private string connName;

        #region Test Context Stuff

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

        #endregion Test Context Stuff

        #region AppSettings

        [TestMethod]
        public void SetAppSettingTest()
        {
            var cm = new ConfigMgr();
            cm.SetAppSetting(appSettingKey, appSettingValue);
        }

        [TestMethod]
        public void GetAppSettingTest()
        {
            var cm = new ConfigMgr();
            var value = cm.GetAppSetting(appSettingKey);
            Assert.AreEqual(appSettingValue, value);
        }

        [TestMethod]
        public void ProtectAppSettingTest()
        {
            var cm = new ConfigMgr();
            cm.ProtectAppSettings();
            var value = cm.GetAppSetting(appSettingKey);
            Assert.AreEqual(appSettingValue, value);
        }

        [TestMethod]
        public void UnProtectAppSettingTest()
        {
            var cm = new ConfigMgr();
            cm.UnProtectAppSettings();
            var value = cm.GetAppSetting(appSettingKey);
            Assert.AreEqual(appSettingValue, value);
        }

        [TestMethod]
        public void SetTempAppSettingTest()
        {
            var cm = new ConfigMgr();
            cm.SetAppSetting(appSettingTempKey, appSettingTempValue);
        }

        [TestMethod]
        public void GetTempAppSettingTest()
        {
            var cm = new ConfigMgr();
            var value = cm.GetAppSetting(appSettingTempKey);
            Assert.AreEqual(appSettingTempValue, value);
        }

        [TestMethod]
        public void RemoveTempAppSettingTest()
        {
            var cm = new ConfigMgr();
            cm.RemoveAppSetting(appSettingTempKey);
            var value = cm.GetAppSetting(appSettingTempKey);
            Assert.IsNull(value);
        }

        #endregion AppSettings

        [TestMethod]
        public void SetConnStringTest()
        {
            var cm = new ConfigMgr();
            cm.SetConnectionString(connName, connString, connProvider);
        }

        [TestMethod]
        public void GetConnStringTest()
        {
            var cm = new ConfigMgr();
            var value = cm.GetConnectionString(connName);
            Assert.AreEqual(connString, value);
        }

        [TestMethod]
        public void RemoveConnStringTest()
        {
            var cm = new ConfigMgr();
            cm.RemoveConnectionString(connName);
            var value = cm.GetConnectionString(connName);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void ProtectConnStringTest()
        {
            var cm = new ConfigMgr();
            cm.ProtectConnectionStrings();
            var value = cm.GetConnectionString(connName);
            Assert.AreEqual(connString, value);
        }

        [TestMethod]
        public void UnProtectConnStringTest()
        {
            var cm = new ConfigMgr();
            cm.UnProtectConnectionStrings();
            var value = cm.GetConnectionString(connName);
            Assert.AreEqual(connString, value);
        }


        [TestMethod]
        public void CreateCspDefaultFile()
        {
            // TODO: Generate CSP defaults for your production environment with different values.
            const string Ksaltp = "11c30c1a34c06143730e9218bfd1903eb01361036f5bbbe546ca8251ec81a1f0";
            const string Ksaltstr = "vDhRGMa4hwg=";
            const string Isaltp = "d77ceafc71cfa337c9f69466de2909f19438c73a3a595860a9fdf074edf10659";
            const string Isaltstr = "9lhLqFSsI+I=";

            var cm = new ConfigMgr();
            var created = cm.TryCreateCsp(Ksaltp, Ksaltstr, Isaltp, Isaltstr, false);
            Assert.IsTrue(created);
        }

        [TestMethod]
        public void CreateCspDefaults()
        {
            const string Ksaltstr = "vDhRGMa4hwg=";
            const string Ksaltp = "11c30c1a34c06143730e9218bfd1903eb01361036f5bbbe546ca8251ec81a1f0";
            const string Isaltstr = "9lhLqFSsI+I=";
            const string Isaltp = "d77ceafc71cfa337c9f69466de2909f19438c73a3a595860a9fdf074edf10659";

            var cm = new ConfigMgr();
            cm.SetDefaults(false);

            var sK = cm.GetAppSetting("cspsK");
            Assert.AreEqual(Ksaltstr, sK);

            var pK = cm.GetAppSetting("csppK");
            Assert.AreEqual(Ksaltp, pK);

            var sI = cm.GetAppSetting("cspsI");
            Assert.AreEqual(Isaltstr, sI);

            var pI = cm.GetAppSetting("csppI");
            Assert.AreEqual(Isaltp, pI);
        }

    }
}