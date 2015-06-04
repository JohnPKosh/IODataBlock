using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Business.Common.Security.Aes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Security
{
    /// <summary>
    /// Summary description for AesCryptoTest
    /// </summary>
    [TestClass]
    public class AesCryptoTest
    {
        public AesCryptoTest()
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
        public void ReadWriteFile()
        {
            var input = new FileInfo(@"Junk\kirk.xml");
            var encrypted = new FileInfo(@"Junk\kirk.dat");
            var output = new FileInfo(@"Junk\kirk_output.xml");

            input.AesSimpleEncryptToFile(encrypted);
            encrypted.AesSimpleDecryptToFile(output);

            var inputstr = File.ReadAllText(input.FullName);
            var outputstr = File.ReadAllText(output.FullName);

            Assert.AreEqual(inputstr,outputstr);

            //AesEncryption.AesEncryptFileToFile();
        }
    }
}
