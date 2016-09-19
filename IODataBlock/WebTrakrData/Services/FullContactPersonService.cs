using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Business.Common.Extensions;
using Business.HttpClient.Navigation;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;

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
        
        #region By Email Methods

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

        public bool TryGetPersonByEmailJObject(string email, out JObject o)
        {
            try
            {
                var result = GetPersonByEmailResponse(email);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    o = JObject.Parse(json);
                }
                else
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    o = JObject.Parse(json);
                }
                return result.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                o = new JObject { { "email", email }, { "exception", ex.InnerException.Message } };
                return false;
            }
        }

        public HttpResponseMessage GetPersonByEmailResponse(string email)
        {
            Url targetUrl = GetPersonByEmailUrl(email);
            var call = targetUrl.WithHeader("X-FullContact-APIKey", FullContactApiKey).AllowAnyHttpStatus().GetAsync();
            return call.Result;
        }

        #endregion
        
        #region By Phone Methods

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

        public bool TryGetPersonByPhoneJObject(string phone, out JObject o)
        {
            try
            {
                var result = GetPersonByPhoneResponse(phone);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    o = JObject.Parse(json);
                }
                else
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    o = JObject.Parse(json);
                }
                return result.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                o = new JObject { { "Phone", phone }, { "exception", ex.InnerException.Message } };
                return false;
            }
        }

        public HttpResponseMessage GetPersonByPhoneResponse(string phone)
        {
            Url targetUrl = GetPersonByPhoneUrl(phone);
            var call = targetUrl.WithHeader("X-FullContact-APIKey", FullContactApiKey).AllowAnyHttpStatus().GetAsync();
            return call.Result;
        }

        #endregion

        #region By Twitter Methods

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

        public bool TryGetPersonByTwitterJObject(string twitter, out JObject o)
        {
            try
            {
                var result = GetPersonByTwitterResponse(twitter);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    o = JObject.Parse(json);
                }
                else
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    o = JObject.Parse(json);
                }
                return result.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                o = new JObject { { "Twitter", twitter }, { "exception", ex.InnerException.Message } };
                return false;
            }
        }
        
        public HttpResponseMessage GetPersonByTwitterResponse(string email)
        {
            Url targetUrl = GetPersonByTwitterUrl(email);
            var call = targetUrl.WithHeader("X-FullContact-APIKey", FullContactApiKey).AllowAnyHttpStatus().GetAsync();
            return call.Result;
        }

        #endregion
    }
}
