using System;
using System.Collections.Generic;
using Business.Common.Requests;
using Business.Common.System;
using Business.Test.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common
{
    /// <summary>
    /// Summary description for CommandTests
    /// </summary>
    [TestClass]
    public class CommandTests
    {
        public CommandTests()
        {
            _commands = new List<ICommand> { new WriteToFileCommand() };
            _parser = new CommandParser(_commands);
        }

        #region Scaffolded Unit Test Stuff

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

        #endregion Scaffolded Unit Test Stuff

        private List<ICommand> _commands;

        private CommandParser _parser;

        [TestMethod]
        public void ReallyBasicSuccessfullCommandTest()
        {
            // create a fake request with the command name and some simple data.
            var ro = new RequestObject
            {
                CommandName = "WriteToFile",
                RequestData = "Sending Fake Monkey Data!",
                CorrelationId = Guid.NewGuid().ToString()
            };

            // Create the command instance.
            var command = _parser.Parse(ro);

            // Execute the command that does the work.
            var responseObject = command.Execute();

            // TODO: Shorten this to 1 or 2 steps!
            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "WriteToFile",
                    "RequestData": "Sending Fake Monkey Data!",
                    "CorrelationId": "c21277e3-467d-43e9-8839-d3a1419abb8c"
                  },
                  "CorrelationId": "c21277e3-467d-43e9-8839-d3a1419abb8c",
                  "ResponseData": "Did some fake work!",
                  "ResponseCode": "200",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }
    }
}