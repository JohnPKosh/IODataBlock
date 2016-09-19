using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Business.Common.Extensions;
using Business.HttpClient.Navigation;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using WebTrakrData.Model.Dto;

namespace WebTrakrData.Services
{
    public class FullContactPersonService
    {
        #region Class Initialization

        public FullContactPersonService(string apiKey)
        {
            FullContactApiKey = apiKey;
        }

        private string FullContactApiKey { get; }

        #endregion

        #region Base Utility Methods

        private HttpResponseMessage GetResponse(Url targetUrl)
        {
            var call = targetUrl.WithHeader("X-FullContact-APIKey", FullContactApiKey).AllowAnyHttpStatus().GetAsync();
            return call.Result;
        }

        private static FullContactResponse ProcessGetResponse(int requestTypeId, JObject requestData, string userId, Url targetUrl, Func<Url, HttpResponseMessage> requestFuntion)
        {
            try
            {
                var result = requestFuntion(targetUrl);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    var o = JObject.Parse(json);
                    JToken requestId;
                    return new FullContactResponse()
                    {
                        Message = "OK",
                        Status = 200,
                        ResponseData = o,
                        RequestId = o.TryGetValue("requestId", out requestId) ? requestId.Value<string>() : null,
                        RequestTypeId = requestTypeId,
                        AspNetUserId = userId,
                        RequestData = requestData
                    };
                }
                else
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    var o = JObject.Parse(json);
                    JToken requestId;
                    JToken message;
                    return new FullContactResponse()
                    {
                        Message = o.TryGetValue("message", out message) ? message.Value<string>() : result.ReasonPhrase,
                        Status = (int)result.StatusCode,
                        ResponseData = o,
                        RequestId = o.TryGetValue("requestId", out requestId) ? requestId.Value<string>() : null,
                        RequestTypeId = requestTypeId,
                        AspNetUserId = userId,
                        RequestData = requestData
                    };
                }

            }
            catch (FlurlHttpTimeoutException)
            {
                return new FullContactResponse()
                {
                    Message = "Client Timed Out! Check connectivity and try again!",
                    Status = 500,
                    RequestTypeId = requestTypeId,
                    AspNetUserId = userId,
                    RequestData = requestData
                };
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.Response != null)
                    return new FullContactResponse()
                    {
                        Message = ex.Call.Response.ReasonPhrase,
                        Status = (int)ex.Call.Response.StatusCode,
                        RequestTypeId = requestTypeId,
                        AspNetUserId = userId,
                        RequestData = requestData
                    };
                else
                    return new FullContactResponse()
                    {
                        Message = ex.Message,
                        Status = 500,
                        RequestTypeId = requestTypeId,
                        AspNetUserId = userId,
                        RequestData = requestData
                    };
            }
            catch (Exception ex)
            {
                return new FullContactResponse()
                {
                    Message = ex.Message,
                    Status = 500,
                    RequestTypeId = requestTypeId,
                    AspNetUserId = userId,
                    RequestData = requestData
                };
            }
        } 

        #endregion

        #region By Email Methods

        public FullContactResponse GetPersonByEmail(string email, string userId)
        {
            const int requestTypeId = 1; // TODO: MUST Match this to DB Model!!!!
            Url targetUrl = GetPersonByEmailUrl(email);
            return ProcessGetResponse(requestTypeId, JObject.FromObject(new { input = email }), userId, targetUrl, GetResponse);
        }

        private static string GetPersonByEmailUrl(string email)
        {
            var url = new ApiUrl
            {
                Root = @"https://api.fullcontact.com",
                PathSegments = new[] { "v2", "person.json" },
                QueryParams = new Dictionary<string, object>() { { "email", email } }
            };
            return url;
        }

        #endregion

        #region By Phone Methods

        public FullContactResponse GetPersonByPhone(string phone, string userId)
        {
            const int requestTypeId = 2; // TODO: MUST Match this to DB Model!!!!
            Url targetUrl = GetPersonByPhoneUrl(phone);
            return ProcessGetResponse(requestTypeId, JObject.FromObject(new { input = phone }), userId, targetUrl, GetResponse);
        }

        private static string GetPersonByPhoneUrl(string phone)
        {
            var url = new ApiUrl
            {
                Root = @"https://api.fullcontact.com",
                PathSegments = new[] { "v2", "person.json" },
                QueryParams = new Dictionary<string, object>() { { "phone", NormalizePhoneNumber(phone) } }
            };
            return url;
        }

        private static string NormalizePhoneNumber(string phone)
        {
            phone = phone.Replace("(", string.Empty)
                    .Replace(")", string.Empty)
                    .Replace(" ", string.Empty)
                    .Replace("-", string.Empty)
                    .Trim().Right(10);
            return ($"+1{phone}");
        }

        #endregion

        #region By Twitter Methods

        public FullContactResponse GetPersonByTwitter(string twitter, string userId)
        {
            const int requestTypeId = 3; // TODO: MUST Match this to DB Model!!!!
            Url targetUrl = GetPersonByTwitterUrl(twitter);
            return ProcessGetResponse(requestTypeId, JObject.FromObject(new { input = twitter }), userId, targetUrl, GetResponse);
        }

        private static string GetPersonByTwitterUrl(string twitter)
        {
            var url = new ApiUrl
            {
                Root = @"https://api.fullcontact.com",
                PathSegments = new[] { "v2", "person.json" },
                QueryParams = new Dictionary<string, object>() { { "twitter", twitter } }
            };
            return url;
        }
        

        #endregion
    }
}
