using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Common.Responses;

namespace Business.Test
{
    [TestClass]
    public class ResponseMessageTests
    {
        [TestMethod]
        public void CanGetJsonStringFromResponseMessageTest()
        {
            var messageResponse = new ResponseMessage();
            messageResponse.ResponseData = "okay!";

            var responseString = messageResponse.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            /*
                {
                  "ResponseData": "okay!",
                  "ResponseCode": null,
                  "Success": true,
                  "ExceptionCount": 0,
                  "ExceptionList": null
                }
            */

        }

        [TestMethod]
        public void CanHaveSystemExceptionTest()
        {
            var messageResponse = new ResponseMessage();
            messageResponse.ResponseData = "okay!";
            try
            {
                var my0 = 0;
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
                  "ResponseCode": null,
                  "Success": false,
                  "ExceptionCount": 1,
                  "ExceptionList": {
                    "Meta": null,
                    "Exceptions": [
                      {
                        "Data": null,
                        "HelpLink": null,
                        "HResult": -2147352558,
                        "InnerExceptionDetail": null,
                        "Message": "Attempted to divide by zero.",
                        "Source": "Business.Test",
                        "StackTrace": "   at Business.Test.ResponseMessageTests.CanHaveSystemExceptionTest() 
                            in c:\\Users\\jkosh\\Documents\\GitHub\\IODataBlock\\IODataBlock\\Test\\Business.Test\\ResponseMessageTests.cs:line 29"
                      }
                    ]
                  }
                }
            */
        }
    }
}


