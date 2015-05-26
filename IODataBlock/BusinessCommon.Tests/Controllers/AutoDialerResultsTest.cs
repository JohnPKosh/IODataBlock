using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using BusinessCommon.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessCommon;
using BusinessCommon.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Flurl;
using Flurl.Http;
using System.Net.Http;

namespace BusinessCommon.Tests.Controllers
{
    [TestClass]
    public class AutoDialerResultsTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            AutoDialerResultsController controller = new AutoDialerResultsController();

            // Act
            dynamic result = controller.Get().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            AutoDialerResultsController controller = new AutoDialerResultsController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            AutoDialerResultsController controller = new AutoDialerResultsController();

            // Act
            var jobj = JObject.Parse(@"{""vendorTrunk"":""TEST_TRUNK""}");
            //controller.Post(jobj);

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            AutoDialerResultsController controller = new AutoDialerResultsController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            AutoDialerResultsController controller = new AutoDialerResultsController();

            // Act
            controller.Delete(5);

            // Assert
        }

        [TestMethod]
        public void GetWithFlurl()
        {
            // http://localhost:48982/
            // http://jkoshlt1.broadvox.local:48982/

            var result = "http://jkoshlt1.broadvox.local/BusinessCommon/api"
                .AppendPathSegment("AutoDialerResults")
                .GetJsonAsync().Result;

            if (result == null) Assert.Fail();
            else
            {
                var dir = result.DataDirectory;
                Assert.IsNotNull(dir);
            }
        }


        [TestMethod]
        public void PostWithFlurl()
        {
            // http://172.16.3.92
            // api/AutoDialerResults

            var jobj = JObject.Parse(@"{""vendorTrunk"":""TEST_TRUNK""}");

            var result = "http://jkoshlt1.broadvox.local/BusinessCommon/api"
                .AppendPathSegment("AutoDialerResults")
                .AppendPathSegment("2165132288")
                .PostJsonAsync(new { vendorTrunk = "TEST_TRUNK" }).Result;

            if (!result.IsSuccessStatusCode) Assert.Fail();
        }

        [TestMethod]
        public void PostAutoDialWithFlurl()
        {
//            var json = @"{
//""transactionId"":""3adfjoi9ilq344iodfd-0234rjsdf"",
//""cps"":""20"",
//""numbers"":[
//{""number"":""2163734622"",""callback-url"":""http://172.16.3.92/BusinessCommon/api/2163734622""},
//{""number"":""4407819740"",""callback-url"":""http://172.16.3.92/BusinessCommon/api/4407819740""}
//]
//}
//";

            var url = @"http://172.16.5.144:8080/llama/spank";

            var result =
                url.WithBasicAuth("naughtydog", "dGV0sBH89NLmlDoiCGgo").PostJsonAsync(new
                {
                    transactionId = "3adfjoi9ilq344iodfd-0234rjsdf"
                    , cps = "20" 
                    , numbers = new[]
                    {
                        new{number="2163734622", callback = "http://172.16.3.92/BusinessCommon/api/2163734622"}
                        , new{number="4407819740", callback = "http://172.16.3.92/BusinessCommon/api/4407819740"}
                    }
                }).Result;

            if (!result.IsSuccessStatusCode) Assert.Fail();
        }

        [TestMethod]
        public void PostAutoDialModelWithFlurl()
        {
            var url = @"http://172.16.5.144:8080/llama/spank";
            var result = url.WithBasicAuth("naughtydog", "dGV0sBH89NLmlDoiCGgo").PostJsonAsync(GetFakeAutoDialRequest()).Result;

            if (!result.IsSuccessStatusCode) Assert.Fail();
        }

        private AutoDialRequestModel GetFakeAutoDialRequest()
        {
            return new AutoDialRequestModel()
            {
                transactionId = "3adfjoi9ilq344iodfd-0234rjsdf",
                cps = "20",
                numbers = new List<AutoDialRequestModel.Number>()
                {
                    new AutoDialRequestModel.Number(){number="2163734622", callback = "http://172.16.3.92/BusinessCommon/api/2163734622"},
                    new AutoDialRequestModel.Number(){number="4407819740", callback = "http://172.16.3.92/BusinessCommon/api/4407819740"}
                }
            };
        }

        //public void RequestDialer()
        //{
        //    var client = new RestClient();
        //    client.BaseUrl = new Uri("http://172.16.3.175/llama/spank");
        //    client.Authenticator = new HttpBasicAuthenticator("naughtydog", "dGV0sBH89NLmlDoiCGgo");

        //    var request = new RestRequest();
        //    request.Method = Method.POST;

        //    //request.Resource = "api/Values";

        //    IRestResponse response = client.Execute(request);


        //}



    }
}
