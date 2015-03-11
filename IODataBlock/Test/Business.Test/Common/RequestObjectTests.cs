using System;
using Business.Common.Exceptions;
using Business.Common.Responses;
using Business.Common.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common
{
    [TestClass]
    public class RequestObjectTests
    {

        [TestMethod]
        public void HowToWorkTogetherSimpleSuccessfullTest()
        {
            // create a request here
            var requestObject = new RequestObject() { CorrelationId = "111111", RequestData = "This is my request :-)" };
            // create a response object to hold the data.
            var responseObject = new ResponseObject();
            // add in the request data to the response since this will not change even on error.
            responseObject.RequestData = requestObject.RequestData;
            responseObject.CorrelationId = requestObject.CorrelationId;

            try
            {
                // There is not much room for error here so I should succeed!
                responseObject.ResponseData = "okay!";
            }
            catch (Exception exception)
            {
                responseObject.ResponseData = "Your Screwed! You tried to divide by 0?";
                responseObject.ExceptionList = ExceptionObjectListBase.Create(
                    exception
                    , "Divide by 0 Test"
                    , "You cannot divide by 0! It is an unbreakable law."
                    , "Test Exceptions"
                    , typeName: "RequestObjectTests"
                    , memberName: "HowToWorkTogetherSimpleSuccessfullTest"
                    , parentName: "Business.Test"
                    );
            }
            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": "This is my request :-)",
                  "CorrelationId": "111111",
                  "ResponseData": "okay!",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void HowToWorkTogetherSimpleFailureTest()
        {
            // create a request here
            var requestObject = new RequestObject() {CorrelationId = "111111", RequestData = "This is my request :-)"};
            // create a response object to hold the data.
            var responseObject = new ResponseObject();
            // add in the request data to the response since this will not change even on error.
            responseObject.RequestData = requestObject.RequestData;
            responseObject.CorrelationId = requestObject.CorrelationId;

            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;
                // if I would have gotten here the below would have been set
                responseObject.ResponseData = "okay!";
            }
            catch (Exception exception)
            {
                responseObject.ResponseData = "Your Screwed! You tried to divide by 0?";
                responseObject.ExceptionList = ExceptionObjectListBase.Create(
                    exception
                    , "Divide by 0 Test"
                    , "You cannot divide by 0! It is an unbreakable law."
                    , "Test Exceptions"
                    , typeName: "RequestObjectTests"
                    , memberName: "HowToWorkTogetherSimpleFailureTest"
                    , parentName: "Business.Test"
                    );
            }
            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                    "RequestData": "This is my request :-)",
                    "CorrelationId": "111111",
                    "ResponseData": "Your Screwed! You tried to divide by 0?",
                    "HasExceptions": true,
                    "ExceptionCount": 1,
                    "ExceptionList": {
                    "Meta": {
                        "DateCreatedUtc": "2015-03-11T17:23:35.6365387Z",
                        "Title": "Divide by 0 Test",
                        "Description": "You cannot divide by 0! It is an unbreakable law.",
                        "ExceptionGroup": "Test Exceptions",
                        "HostComputerName": "JKOSHLT1",
                        "HostUserName": "jkosh",
                        "HostUserDomain": "BROADVOX",
                        "TypeName": "RequestObjectTests",
                        "MemberName": "HowToWorkTogetherSimpleFailureTest",
                        "ParentName": "Business.Test"
                    },
                    "Exceptions": [
                        {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.RequestObjectTests.HowToWorkTogetherSimpleFailureTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\RequestObjectTests.cs:line 76"
                        }
                    ]
                    }
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void HowToWorkTogetherReallySimpleSuccessfullTest()
        {
            var responseObject = "This is my request :-)".ToUncompletedResponse("Not Started!", "9999999");
            try
            {
                // There is not much room for error here so I should succeed!
                responseObject.ResponseData = "Since there were no errors I am okay!";
                responseObject.ResponseCode = "Completed!";
            }
            catch (Exception exception)
            {
                responseObject.ResponseData = "Your Screwed! You tried to divide by 0?";
                responseObject.ResponseCode = "Failed!";
                responseObject.ExceptionList = ExceptionObjectListBase.Create(
                    exception
                    , "Divide by 0 Test"
                    , "You cannot divide by 0! It is an unbreakable law."
                    , "Test Exceptions"
                    , typeName: "RequestObjectTests"
                    , memberName: "HowToWorkTogetherReallySimpleSuccessfullTest"
                    , parentName: "Business.Test"
                    );
            }
            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": "This is my request :-)",
                  "CorrelationId": "9999999",
                  "ResponseData": "Since there were no errors I am okay!",
                  "ResponseCode": "Completed!",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }
        
        [TestMethod]
        public void HowToWorkTogetherReallySimpleFailureTest()
        {
            var responseObject = "This is my request :-)".ToUncompletedResponse("Not Started!", "9999999");
            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;
                // if I would have gotten here the below would have been set
                responseObject.ResponseData = "Since there were no errors I am okay!";
                responseObject.ResponseCode = "Completed!";
            }
            catch (Exception exception)
            {
                responseObject.ResponseData = "Your Screwed! You tried to divide by 0?";
                responseObject.ResponseCode = "Failed!";
                responseObject.ExceptionList = ExceptionObjectListBase.Create(
                    exception
                    , "Divide by 0 Test"
                    , "You cannot divide by 0! It is an unbreakable law."
                    , "Test Exceptions"
                    , typeName: "RequestObjectTests"
                    , memberName: "HowToWorkTogetherReallySimpleFailureTest"
                    , parentName: "Business.Test"
                    );
            }
            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": "This is my request :-)",
                  "CorrelationId": "9999999",
                  "ResponseData": "Your Screwed! You tried to divide by 0?",
                  "ResponseCode": "Failed!",
                  "HasExceptions": true,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Meta": {
                      "DateCreatedUtc": "2015-03-11T17:39:31.3223489Z",
                      "Title": "Divide by 0 Test",
                      "Description": "You cannot divide by 0! It is an unbreakable law.",
                      "ExceptionGroup": "Test Exceptions",
                      "HostComputerName": "JKOSHLT1",
                      "HostUserName": "jkosh",
                      "HostUserDomain": "BROADVOX",
                      "TypeName": "RequestObjectTests",
                      "MemberName": "HowToWorkTogetherReallySimpleFailureTest",
                      "ParentName": "Business.Test"
                    },
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.RequestObjectTests.HowToWorkTogetherReallySimpleFailureTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\RequestObjectTests.cs:line 187"
                      }
                    ]
                  }
                }
            */

            #endregion Json result
        }
    }
}
