﻿using System;
using System.Dynamic;
using System.Text;
using System.Collections.Generic;
using Business.Common.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common.System.Commands
{
    /// <summary>
    /// Summary description for SystemCommandTests
    /// </summary>
    [TestClass]
    public class SystemCommandTests
    {
        public SystemCommandTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        #endregion

        [TestMethod]
        public void GetSetTest()
        {
            var setResult = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "Set", "hello");
            var getResult = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "Get", null).ResponseData;
            Assert.IsNotNull(getResult);
        }

        [TestMethod]
        public void SetSaveTest()
        {
            dynamic d = new ExpandoObject();
            d.Text = "Hello";

            var setResult = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "Set", d);
            var saveResult = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "Save", null);
            var loadResult = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "Load", null);
            dynamic getResult = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "Get", null).ResponseData;
            Assert.IsNotNull(getResult);
        }

    }
}
