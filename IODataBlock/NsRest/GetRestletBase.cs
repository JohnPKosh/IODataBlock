using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NsRest
{
    public class GetRestletBase : IRestletBase
    {
        #region Class Initialization

        public GetRestletBase(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            BaseUrl = baseUrl;
            ScriptSetting = scriptSetting;
            Login = login;
            HttpMethodType = HttpMethodType.Get;
        }

        public static GetRestletBase Create(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            return new GetRestletBase(baseUrl, scriptSetting, login);
        }

        #endregion Class Initialization

        #region Fields and Properties

        public String BaseUrl { get; set; }

        public INetSuiteScriptSetting ScriptSetting { get; set; }

        public HttpMethodType HttpMethodType { get; set; }

        public INetSuiteLogin Login { get; set; }

        #endregion Fields and Properties

        #region Public Methods

        public async Task<string> ExecuteToJsonStringAsync(IDictionary<string, object> input)
        {
            using (var httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, BuildUrl(input));
                requestMessage.Headers.TryAddWithoutValidation("Authorization", GetAuthorizationHeaders(Login));
                var response = await httpClient.SendAsync(requestMessage);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<dynamic> ExecuteToDynamicAsync(IDictionary<string, object> input)
        {
            using (var httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, BuildUrl(input));
                requestMessage.Headers.TryAddWithoutValidation("Authorization", GetAuthorizationHeaders(Login));
                var response = await httpClient.SendAsync(requestMessage);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter(), new StringEnumConverter());
            }
        }

        public async Task<IList<ExpandoObject>> ExecuteToDynamicListAsync(IDictionary<string, object> input)
        {
            using (var httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, BuildUrl(input));
                requestMessage.Headers.TryAddWithoutValidation("Authorization", GetAuthorizationHeaders(Login));
                var response = await httpClient.SendAsync(requestMessage);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<ExpandoObject>>(result, new ExpandoObjectConverter(), new StringEnumConverter());
            }
        }

        public async Task<T> ExecuteToAsync<T>(IDictionary<string, object> input)
        {
            using (var httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, BuildUrl(input));
                requestMessage.Headers.TryAddWithoutValidation("Authorization", GetAuthorizationHeaders(Login));
                var response = await httpClient.SendAsync(requestMessage);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result, new ExpandoObjectConverter(), new StringEnumConverter());
            }
        }

        public async Task<T> ExecuteAndPopulateAsync<T>(T target, IDictionary<string, object> input)
        {
            using (var httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, BuildUrl(input));
                requestMessage.Headers.TryAddWithoutValidation("Authorization", GetAuthorizationHeaders(Login));
                var response = await httpClient.SendAsync(requestMessage);
                var result = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new ExpandoObjectConverter());
                settings.Converters.Add(new StringEnumConverter());
                JsonConvert.PopulateObject(result, target, settings);
                return target;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetAuthorizationHeaders(INetSuiteLogin login)
        {
            return String.Format(@"NLAuth nlauth_account={0},nlauth_email={1},nlauth_signature={2},nlauth_role={3}", login.Account, login.Email, login.Password, login.Role) +
                   Environment.NewLine + "Content-Type: application/json";
        }

        private Url BuildUrl(IEnumerable<KeyValuePair<string, object>> input)
        {
            var rv = new Url(BaseUrl);
            rv.SetQueryParam("script", ScriptSetting.ScriptName);
            rv.SetQueryParam("deploy", ScriptSetting.DeplomentName);
            foreach (var o in input)
            {
                rv.SetQueryParam(o.Key, o.Value);
            }
            return rv;
        }

        #endregion Private Methods
    }
}