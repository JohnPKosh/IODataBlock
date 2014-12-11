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

        }
    }
}


/*
{
  "ResponseData": "okay!",
  "ResponseCode": null,
  "Success": true,
  "ExceptionCount": 0,
  "ExceptionList": null
}
*/