using System;
using Business.Common.Exceptions;
using Business.Common.Requests;
using Business.Common.Responses;
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
            var requestObject = new RequestObject { CorrelationId = "111111", RequestData = "This is my request :-)" };
            // create a response object to hold the data.
            var responseObject = new ResponseObject
            {
                // add in the request data to the response since this will not change even on error.
                RequestData = requestObject.RequestData,
                CorrelationId = requestObject.CorrelationId
            };

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
                    , ExceptionLogLevelType.Debug
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
            var requestObject = new RequestObject { CorrelationId = "111111", RequestData = "This is my request :-)" };
            // create a response object to hold the data.
            var responseObject = new ResponseObject
            {
                // add in the request data to the response since this will not change even on error.
                RequestData = requestObject.RequestData,
                CorrelationId = requestObject.CorrelationId
            };

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
                    , ExceptionLogLevelType.Debug
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
                    "Exceptions": [
                      {
                        "Title": "Divide by 0 Test",
                        "Description": "You cannot divide by 0! It is an unbreakable law.",
                        "ExceptionGroup": "Test Exceptions",
                        "LogLevel": 0,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.RequestObjectTests.HowToWorkTogetherSimpleFailureTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\RequestObjectTests.cs:line 74"
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
                    , ExceptionLogLevelType.Debug
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
                    , ExceptionLogLevelType.Debug
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
                    "Exceptions": [
                      {
                        "Title": "Divide by 0 Test",
                        "Description": "You cannot divide by 0! It is an unbreakable law.",
                        "ExceptionGroup": "Test Exceptions",
                        "LogLevel": 0,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.RequestObjectTests.HowToWorkTogetherReallySimpleFailureTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\RequestObjectTests.cs:line 171"
                      }
                    ]
                  }
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void CallAReallySimpleSuccessfullTest()
        {
            // fake request data here
            var requestData = "Hello World!";
            // fake resonse data here
            var responseData = "Some ResponseData goes HERE!";

            // simulate a good response
            var responseObject = ExecuteSuccessfullResponse(requestData, responseData, "Success Response Code", "1000000");

            // review the results
            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": "Hello World!",
                  "CorrelationId": "1000000",
                  "ResponseData": "Some ResponseData goes HERE!",
                  "ResponseCode": "Success Response Code",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void CallAReallySimpleFailedTest()
        {
            // fake request data here
            var requestData = "Hello World!";

            // simulate a failed response
            var responseObject = ExecuteFailedResponse(requestData, "1000000");

            // review the results
            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": "Hello World!",
                  "CorrelationId": "1000000",
                  "ResponseCode": "Failed Response Code",
                  "HasExceptions": true,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Exceptions": [
                      {
                        "Title": "Divide by 0 Test",
                        "Description": "You cannot divide by 0! It is an unbreakable law.",
                        "ExceptionGroup": "Test Exceptions",
                        "LogLevel": 0,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.RequestObjectTests.ExecuteFailedResponse(Object requestData, String correlationId) in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\RequestObjectTests.cs:line 333"
                      }
                    ]
                  }
                }
            */

            #endregion Json result
        }

        #region private methods

        private IResponseObject ExecuteSuccessfullResponse(object requestData, object responseData, object responseCode = null, string correlationId = null)
        {
            //var rv = requestData.ToUncompletedResponse(null, correlationId);
            try
            {
                return requestData.ToSuccessfullResponse(responseData, responseCode, correlationId);
            }
            catch (Exception ex)
            {
                return requestData.ToFailedResponse(ExceptionObjectListBase.Create(ex, ex.Message));
            }
        }

        private IResponseObject ExecuteFailedResponse(object requestData, string correlationId = null)
        {
            //var rv = requestData.ToUncompletedResponse(null, correlationId);
            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;

                // if I would have gotten here the below would have been set
                return requestData.ToSuccessfullResponse(null, null, correlationId);
            }
            catch (Exception ex)
            {
                return requestData.ToFailedResponse(ExceptionObjectListBase.Create(
                    ex
                    , "Divide by 0 Test"
                    , "You cannot divide by 0! It is an unbreakable law."
                    , "Test Exceptions"
                    , ExceptionLogLevelType.Debug
                    )
                    , "Failed Response Code"
                    , correlationId
                    );
            }
        }

        #endregion private methods
    }
}