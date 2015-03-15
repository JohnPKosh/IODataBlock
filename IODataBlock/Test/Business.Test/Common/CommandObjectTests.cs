using System;
using System.Collections.Generic;
using System.Reflection;
using Business.Common.Exceptions;
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
            // Set up some commands to call!
            _commands = new List<ICommandObject> { new WriteToFileCommand(), new ReadFromFileCommand() };
            
            // create the parser that will do the work!
            _parser = new CommandObjectParser(_commands);
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

        private List<ICommandObject> _commands;

        private CommandObjectParser _parser;

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

        [TestMethod]
        public void ReallyBasicSuccessfullCommandTest2()
        {
            // create a fake request with the command name and some simple data.
            var ro = new RequestObject
            {
                CommandName = "WriteToFile",
                RequestData = "Sending Fake Monkey Data!",
                CorrelationId = Guid.NewGuid().ToString()
            };

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.ParseAndExecute(ro);

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

        [TestMethod]
        public void ReallyBasicSuccessfullReadFromFileTest()
        {
            // create a fake request with the command name and some simple data.
            var ro = new RequestObject
            {
                CommandName = "ReadFromFile",
                RequestData = "Monkey?",
                CorrelationId = Guid.NewGuid().ToString()
            };

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.ParseAndExecute(ro);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "ReadFromFile",
                    "RequestData": "Monkey?",
                    "CorrelationId": "6c43d805-c4f2-4b26-b147-b9fa81a4ffd5"
                  },
                  "CorrelationId": "6c43d805-c4f2-4b26-b147-b9fa81a4ffd5",
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
            var meta = ExceptionMetaBase.CreateExceptionMeta("ReadFromFile Test"
                , "Executing ReallyBasicSuccessfullReadFromFileTest2"
                , Assembly.GetExecutingAssembly().FullName
                , Environment.MachineName
                , Environment.UserName
                , Environment.UserDomainName
                , Assembly.GetExecutingAssembly().FullName
                ,null
                ,null
                //, Assembly.GetCallingAssembly().FullName
                //, Assembly.GetEntryAssembly().FullName
                , "CommandTests"
                , "ReallyBasicSuccessfullReadFromFileTest2"
                , null
                , "Some GUID goes here"
                , "Me"
                , "client IP goes here"
                , correllationId);

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.ParseAndExecute("ReadFromFile", "Monkey?", correllationId, meta);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "ReadFromFile",
                    "RequestData": "Monkey?",
                    "CorrelationId": "a213bfc7-a45a-418f-b000-c3359601ef74"
                  },
                  "CorrelationId": "a213bfc7-a45a-418f-b000-c3359601ef74",
                  "ResponseCode": "500",
                  "HasExceptions": true,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Meta": {
                      "DateCreatedUtc": "2015-03-15T00:15:45.2982339Z",
                      "Title": "ReadFromFile Test",
                      "Description": "Executing ReallyBasicSuccessfullReadFromFileTest2",
                      "ExceptionGroup": "Business.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                      "HostComputerName": "JKOSHLT1",
                      "HostUserName": "jkosh",
                      "HostUserDomain": "BROADVOX",
                      "ExecutingAssemblyFullName": "Business.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                      "TypeName": "CommandTests",
                      "MemberName": "ReallyBasicSuccessfullReadFromFileTest2",
                      "AppId": "Some GUID goes here",
                      "ClientName": "Me",
                      "ClientIp": "client IP goes here",
                      "CorrelationId": "a213bfc7-a45a-418f-b000-c3359601ef74"
                    },
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.TestUtility.ReadFromFileCommand.<Create>b__1(IRequestObject o) in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\TestUtility\\ReadFromFileCommand.cs:line 48\r\n   at Business.Common.System.CommandObjectBase.Execute(IExceptionMeta exceptionMeta) in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Business\\Business.Common\\System\\CommandObjectBase.cs:line 46"
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