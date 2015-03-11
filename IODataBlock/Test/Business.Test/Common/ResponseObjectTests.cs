using System;
using Business.Common.Responses;
using Business.Exceptions.Base;
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
                  "ResponseCode": null,
                  "Success": true,
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
                responseObject.AddException(exception);
            }

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            /*
                {
                  "ResponseData": "okay!",
                  "Success": false,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.ResponseMessageTests.CanHaveSystemExceptionTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\ResponseMessageTests.cs:line 37"
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
            catch (Exception exception)
            {
                responseObject.AddException(exception, true);
            }

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            /*
                {
                  "ResponseData": "okay!",
                  "Success": false,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Meta": {
                      "DateCreatedUtc": "2015-03-08T12:56:38.5596122Z",
                      "HostComputerName": "JKOSHLT1",
                      "HostUserName": "jkosh",
                      "HostUserDomain": "BROADVOX"
                    },
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.ResponseMessageTests.CanHaveSystemExceptionWithExceptionMetaTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\ResponseMessageTests.cs:line 76"
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
                    , typeName: "ResponseMessageTests"
                    , memberName: "CanHaveSystemExceptionWithExplicitTest"
                    , parentName: "Business.Test"
                    );
            }
            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "ResponseData": "Your Screwed! You tried to divide by 0?",
                  "Success": false,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Meta": {
                      "DateCreatedUtc": "2015-03-10T21:45:26.2735836Z",
                      "Title": "Divide by 0 Test",
                      "Description": "You cannot divide by 0! It is an unbreakable law.",
                      "ExceptionGroup": "Test Exceptions",
                      "HostComputerName": "JKOSHLT1",
                      "HostUserName": "jkosh",
                      "HostUserDomain": "BROADVOX",
                      "TypeName": "ResponseMessageTests",
                      "MemberName": "CanHaveSystemExceptionWithExplicitTest",
                      "ParentName": "Business.Test"
                    },
                    "Exceptions": [
                      {
                        "LogLevel": 3,
                        "HResult": -2147352558,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.Common.ResponseMessageTests.CanHaveSystemExceptionWithExplicitTest() in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\Common\\ResponseMessageTests.cs:line 124"
                      }
                    ]
                  }
                }
            */

            #endregion Json result
        }
    }
}