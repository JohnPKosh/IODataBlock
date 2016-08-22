using Business.Common.Generic;
using Business.Common.GenericRequests;
using Business.Test.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Business.Test.Common
{
    /// <summary>
    /// Summary description for CommandTests
    /// </summary>
    [TestClass]
    public class GenericCommandObjectTests
    {
        public GenericCommandObjectTests()
        {
            // Set up some commands to call!
            _commands = new List<ICommandObject<string, string>> { new WriteToFileGenericCommand(), new ReadFromFileGenericCommand(), new ThrowExceptionFromGenericCommand() };

            // create the parser that will do the work!
            _parser = new CommandObjectParser<string, string>(_commands);
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

        private List<ICommandObject<string, string>> _commands;

        private CommandObjectParser<string, string> _parser;

        [TestMethod]
        public void ReallyBasicSuccessfullCommandTest()
        {
            // create a fake request with the command name and some simple data.
            var ro = new RequestObject<string>
            {
                CommandName = "WriteToFileGeneric",
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
                    "CorrelationId": "781850a0-5f58-405b-8671-792d1c809ad7",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "781850a0-5f58-405b-8671-792d1c809ad7",
                  "ResponseData": "Did some fake work!",
                  "ResponseCode": "200",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void ReallyBasicSuccessfullCommandTest2()
        {
            // create a fake request with the command name and some simple data.
            var ro = new RequestObject<string>
            {
                CommandName = "WriteToFileGeneric",
                RequestData = "Sending Fake Monkey Data!",
                CorrelationId = Guid.NewGuid().ToString()
            };

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.Execute(ro);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "WriteToFile",
                    "RequestData": "Sending Fake Monkey Data!",
                    "CorrelationId": "e846d170-2ba1-4be3-96d2-7b5152f21fbd",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "e846d170-2ba1-4be3-96d2-7b5152f21fbd",
                  "ResponseData": "Did some fake work!",
                  "ResponseCode": "200",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void ReallyBasicSuccessfullReadFromFileTest()
        {
            // create a fake request with the command name and some simple data.
            var ro = new RequestObject<string>
            {
                CommandName = "ReadFromFileGeneric",
                RequestData = "Monkey?",
                CorrelationId = Guid.NewGuid().ToString()
            };

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.Execute(ro);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "ReadFromFile",
                    "RequestData": "Monkey?",
                    "CorrelationId": "07e38f1e-e25f-469a-94d5-2ada2d5ffde3",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "07e38f1e-e25f-469a-94d5-2ada2d5ffde3",
                  "ResponseData": "hello Monkey? from ReadFromFileCommand!",
                  "ResponseCode": "200",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void ReallyBasicSuccessfullReadFromFileTest2()
        {
            var correllationId = NewGuid();

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.Execute("ReadFromFileGeneric", "Monkey?", correllationId);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "ReadFromFile",
                    "RequestData": "Monkey?",
                    "CorrelationId": "4ca6003b-5f02-419e-bf6b-b8a37d28f4a9",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "4ca6003b-5f02-419e-bf6b-b8a37d28f4a9",
                  "ResponseCode": "500",
                  "HasExceptions": true,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.TestUtility.ReadFromFileCommand.<Create>b__1(IRequestObject o) in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\TestUtility\\ReadFromFileCommand.cs:line 48\r\n   at Business.Common.System.CommandObjectBase.Execute() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Business\\Business.Common\\System\\CommandObjectBase.cs:line 46"
                      }
                    ]
                  }
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void ReallyBasicExceptionTest()
        {
            var correllationId = NewGuid();

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.Execute("ThrowExceptionFromGeneric", "Bad Monkey?", correllationId);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "ReadFromFile",
                    "RequestData": "Monkey?",
                    "CorrelationId": "4ca6003b-5f02-419e-bf6b-b8a37d28f4a9",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "4ca6003b-5f02-419e-bf6b-b8a37d28f4a9",
                  "ResponseCode": "500",
                  "HasExceptions": true,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.TestUtility.ReadFromFileCommand.<Create>b__1(IRequestObject o) in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\TestUtility\\ReadFromFileCommand.cs:line 48\r\n   at Business.Common.System.CommandObjectBase.Execute() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Business\\Business.Common\\System\\CommandObjectBase.cs:line 46"
                      }
                    ]
                  }
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void NoCommandFoundExceptionTest()
        {
            var correllationId = NewGuid();

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.Execute("ThrowExceptionFromNonExistant", "Bad Monkey?", correllationId);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "ReadFromFile",
                    "RequestData": "Monkey?",
                    "CorrelationId": "4ca6003b-5f02-419e-bf6b-b8a37d28f4a9",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "4ca6003b-5f02-419e-bf6b-b8a37d28f4a9",
                  "ResponseCode": "500",
                  "HasExceptions": true,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.TestUtility.ReadFromFileCommand.<Create>b__1(IRequestObject o) in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\TestUtility\\ReadFromFileCommand.cs:line 48\r\n   at Business.Common.System.CommandObjectBase.Execute() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Business\\Business.Common\\System\\CommandObjectBase.cs:line 46"
                      }
                    ]
                  }
                }
            */

            #endregion Json result
        }

        private string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}