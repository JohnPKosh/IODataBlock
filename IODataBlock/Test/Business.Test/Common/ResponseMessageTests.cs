using System;
using Business.Common.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common
{
    [TestClass]
    public class ResponseMessageTests
    {
        [TestMethod]
        public void CanGetJsonStringFromResponseMessageTest()
        {
            var messageResponse = new ResponseMessage { ResponseData = "okay!" };

            var responseString = messageResponse.ToJson(true);
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
            var messageResponse = new ResponseMessage { ResponseData = "okay!" };
            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;
            }
            catch (Exception exception)
            {
                messageResponse.AddException(exception);
            }

            var responseString = messageResponse.ToJson(true);
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
            var messageResponse = new ResponseMessage { ResponseData = "okay!" };
            try
            {
                // ReSharper disable once ConvertToConstant.Local
                var my0 = 0;
                // ReSharper disable once UnusedVariable
                var db0 = 1 / my0;
            }
            catch (Exception exception)
            {
                messageResponse.AddException(exception, true);
            }

            var responseString = messageResponse.ToJson(true);
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
    }
}