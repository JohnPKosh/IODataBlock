using Business.Test.TestUtility;
using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Business.Test.Utilities
{
    /// <summary>
    /// Summary description for CsvHelperTests
    /// </summary>
    [TestClass]
    public class CsvHelperTests
    {
        // http://joshclose.github.io/CsvHelper/

        public CsvHelperTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private CsvReader _csvReader;
        private CsvWriter _csvWriter;

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

        [TestMethod]
        public void TestMethod1()
        {
            List<FakePet> pets;
            using (_csvWriter = new CsvWriter(new StreamWriter(@"C:\junk\pets.csv")))
            {
                //_csvWriter.Configuration.Quote = '"';
                //_csvWriter.Configuration.QuoteAllFields = true;
                pets = new List<FakePet>() { FakePet.CreateBela(), FakePet.CreateNala() };
                _csvWriter.WriteHeader<FakePet>();
                _csvWriter.WriteRecords(pets);
            }

            using (_csvReader = new CsvReader(new StreamReader(@"C:\junk\pets.csv")))
            {
                var newpets = _csvReader.GetRecords<FakePet>();
                Assert.IsNotNull(newpets);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            using (_csvReader = new CsvReader(new StreamReader(@"C:\junk\pets2.csv")))
            {
                var newpets = _csvReader.GetRecords<FakePet>();
                Assert.IsNotNull(newpets);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            using (_csvReader = new CsvReader(new StreamReader(@"C:\junk\pets2.csv")))
            {
                while (_csvReader.Read())
                {
                    var fcnt = _csvReader.CurrentRecord.Length;
                    var r = _csvReader.CurrentRecord;
                    Assert.IsNotNull(r);

                    for (int i = 0; i < fcnt; i++)
                    {
                        var stringField = _csvReader.GetField<string>(i);
                        Assert.IsNotNull(stringField);
                    }

                    //int intField;
                    //if (!_csvReader.TryGetField(0, out intField))
                    //{
                    //    // Do something when it can't convert.
                    //}
                }

                //Assert.IsNotNull(newpets);
            }
        }
    }
}