using System;
using Business.Common.Exceptions;
using Business.Common.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common
{
    [TestClass]
    public class ResponseObjectTests
    {
        [TestMethod]
        public void CanGetJsonStringFromResponseTest()
        {
            var responseObject = new ResponseObject { ResponseData = "okay!" };

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            /*
                {
                  "ResponseData": "okay!",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */
        }

        [TestMethod]
        public void CanHaveSystemExceptionTest()
        {
            var responseObject = new ResponseObject { ResponseData = "okay!" };
            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;
            }
            catch (Exception exception)
            {
                responseObject.ResponseData = "An error has occured!";
                //responseObject.ResponseCode = "bad mojo";
                responseObject.ResponseCode = new ResponseCode(500, "bad mojo");
                responseObject.AddException(exception, "You can't divide by 0!", "The description is pretty self explanatory!", "Too dumb to explain group", ExceptionLogLevelType.Critical);
            }

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            /*
                {
                  "ResponseData": "An error has occured!",
                  "ResponseCode": "bad mojo",
                  "HasExceptions": true,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Exceptions": [
                      {
                        "Title": "You can't divide by 0!",
                        "Description": "The description is pretty self explanatory!",
                        "ExceptionGroup": "Too dumb to explain group",
                        "LogLevel": 4,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.ResponseObjectTests.CanHaveSystemExceptionTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\ResponseObjectTests.cs:line 37"
                      }
                    ]
                  }
                }
            */
        }

        [TestMethod]
        public void CanHaveSystemExceptionWithExceptionMetaTest()
        {
            var responseObject = new ResponseObject { ResponseData = "okay!" };
            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;
            }
            catch (Exception ex)
            {
                responseObject.AddException(
                    ex
                    , "Divide by 0 Test"
                    , "You cannot divide by 0! It is an unbreakable law."
                    , "Test Exceptions"
                    , ExceptionLogLevelType.Debug
                    );
            }

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            /*
                {
                  "ResponseData": "okay!",
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
                        "StackTrace": "   at Business.Test.Common.ResponseObjectTests.CanHaveSystemExceptionWithExceptionMetaTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\ResponseObjectTests.cs:line 82"
                      }
                    ]
                  }
                }
            */
        }

        [TestMethod]
        public void CanHaveSystemExceptionWithExplicitTest()
        {
            var responseObject = new ResponseObject { ResponseData = "okay!" };
            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;
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
                        "StackTrace": "   at Business.Test.Common.ResponseObjectTests.CanHaveSystemExceptionWithExplicitTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\ResponseObjectTests.cs:line 130"
                        }
                    ]
                    }
                }
            */

            #endregion Json result
        }
    }
}