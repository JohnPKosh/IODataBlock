using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Business.Common.Configuration;
using Business.Common.Extensions;
using Business.Common.IO;
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

        [TestMethod]
        public void ReadWriteFile2()
        {
            var input = new FileInfo(@"Junk\kirk.xml");
            var encrypted = new FileInfo(@"Junk\kirk.dat");
            var output = new FileInfo(@"Junk\kirk_output2.xml");

            var cfg = new ConfigMgr();
            cfg.SetDefaults(false);
            var key = cfg.GetAesKBytes();
            var iv = cfg.GetAesIvBytes();

            input.AesEncryptToFile(encrypted, key, iv);
            encrypted.AesDecryptToFile(output, key, iv);

            var inputstr = File.ReadAllText(input.FullName);
            var outputstr = File.ReadAllText(output.FullName);

            Assert.AreEqual(inputstr, outputstr);

            //AesEncryption.AesEncryptFileToFile();
        }

        [TestMethod]
        public void ReadWriteFile3()
        {
            var input = new FileInfo(@"Junk\kirk.xml");
            var encrypted = new FileInfo(@"Junk\kirk.dat");
            var output = new FileInfo(@"Junk\kirk_output.xml");

            var cfg = new ConfigMgr();
            cfg.SetDefaults(false);
            var key = cfg.GetAesKBytes();
            var iv = cfg.GetAesIvBytes();

            var inputstr = File.ReadAllText(input.FullName);

            using (var inputms = new MemoryStream(Encoding.Default.GetBytes(inputstr)))
            {
                using (var fs = encrypted.Create())
                {
                    inputms.AesEncryptStream(fs, key, iv);
                }
            }
            //input.AesEncryptFileToFile(encrypted, key, iv);
            encrypted.AesDecryptToFile(output, key, iv);

            var outputstr = File.ReadAllText(output.FullName);
            Assert.AreEqual(inputstr, outputstr);
        }

        [TestMethod]
        public void ReadWriteFile4()
        {
            var input = new FileInfo(@"Junk\kirk.xml");
            var encrypted = new FileInfo(@"Junk\kirk.dat");
            var output = new FileInfo(@"Junk\kirk_output.xml");

            var cfg = new ConfigMgr();
            cfg.SetDefaults(false);
            var key = cfg.GetAesKBytes();
            var iv = cfg.GetAesIvBytes();

            var inputstr = File.ReadAllText(input.FullName);

            using (var inputms = new MemoryStream(Encoding.Default.GetBytes(inputstr)))
            {
                using (var fs = encrypted.Create())
                {
                    inputms.AesEncryptStream(fs, key, iv);
                }
            }
            //input.AesEncryptFileToFile(encrypted, key, iv);

            var encryptedstr = File.ReadAllText(encrypted.FullName);
            using (var inputms = encrypted.OpenRead())
            {
                using (var fs = output.Create())
                {
                    inputms.AesDecryptStream(fs, key, iv);
                }
            }

            //encrypted.AesDecryptFileToFile(output, key, iv);

            var outputstr = File.ReadAllText(output.FullName);
            Assert.AreEqual(inputstr, outputstr);
        }

        [TestMethod]
        public void ReadWriteFile5()
        {
            var input = new FileInfo(@"Junk\kirk.xml");
            var encrypted = new FileInfo(@"Junk\kirk.dat");
            var output = new FileInfo(@"Junk\kirk_output.xml");

            var cfg = new ConfigMgr();
            cfg.SetDefaults(false);
            var key = cfg.GetAesKBytes();
            var iv = cfg.GetAesIvBytes();

            var inputstr = File.ReadAllText(input.FullName);

            using (var inputms = new MemoryStream(Encoding.Default.GetBytes(inputstr)))
            {
                using (var fs = encrypted.Create())
                {
                    inputms.AesGzipEncryptStream(fs, key, iv);
                }
            }
            //input.AesEncryptFileToFile(encrypted, key, iv);

            var encryptedBytes = File.ReadAllBytes(encrypted.FullName);
            using (var inputms = new MemoryStream(encryptedBytes))
            {
                using (var fs = output.Create())
                {
                    inputms.AesGzipDecryptStream(fs, key, iv);
                }
            }

            //encrypted.AesDecryptFileToFile(output, key, iv);

            var outputstr = File.ReadAllText(output.FullName);
            Assert.AreEqual(inputstr, outputstr);
        }


        [TestMethod]
        public void ReadWriteFile6()
        {
            var input = new FileInfo(@"Junk\kirk.xml");
            var encrypted = new FileInfo(@"Junk\kirk.dat");
            var output = new FileInfo(@"Junk\kirk_output.xml");

            var cfg = new ConfigMgr();
            cfg.SetDefaults(false);
            var key = cfg.GetAesKBytes();
            var iv = cfg.GetAesIvBytes();

            input.AesGzipEncryptToFile(encrypted, key, iv);
            encrypted.AesGzipDecryptToFile(output, key, iv);

            var inputstr = File.ReadAllText(input.FullName);
            var outputstr = File.ReadAllText(output.FullName);
            Assert.AreEqual(inputstr, outputstr);
        }

        [TestMethod]
        public void DecryptAppConfigTest()
        {

            var cfg = new ConfigMgr();
            cfg.UnProtectAppSettings();
        }
    }
}
