using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NsRest
{
    public class PostRestletBase : IRestletBase
    {
        #region Class Initialization

        public PostRestletBase(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            BaseUrl = baseUrl;
            ScriptSetting = scriptSetting;
            Login = login;
            HttpMethodType = HttpMethodType.Get;
        }

        public static PostRestletBase Create(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            return new PostRestletBase(baseUrl, scriptSetting, login);
        }

        #endregion Class Initialization

        #region Fields and Properties

        public string BaseUrl { get; set; }

        public INetSuiteScriptSetting ScriptSetting { get; set; }

        public HttpMethodType HttpMethodType { get; set; }

        public INetSuiteLogin Login { get; set; }

        #endregion Fields and Properties

        #region Public Methods

        public async Task<string> ExecuteToJsonStringAsync(IDictionary<string, object> input)
        {
            var result = await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .PostJsonAsync(input).ReceiveString();
            return result;
        }

        public async Task<dynamic> ExecuteToDynamicAsync(IDictionary<string, object> input)
        {
            var result = await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .PostJsonAsync(input).ReceiveJson();
            return result;
        }

        public async Task<IList<ExpandoObject>> ExecuteToDynamicListAsync(IDictionary<string, object> input)
        {
            var result = await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .PostJsonAsync(input).ReceiveJsonList();
            return result as IList<ExpandoObject>;
        }

        /* TODO: fix up methods below for no results */

        public async Task<IEnumerable<JObject>> ExecuteToJObjectsAsync(IDictionary<string, object> input) => await Task.Run(() => ExecuteToJArrayAsync(input).Result.Children<JObject>());

        public async Task<JArray> ExecuteToJArrayAsync(IDictionary<string, object> input) => await Task.Run(
            () =>
            {
                var result = ExecuteToJsonStringAsync(input).Result;
                if(string.IsNullOrWhiteSpace(result) || result == "null") return new JArray();
                return JArray.Parse(result);
            });



        public async Task<T> ExecuteToAsync<T>(IDictionary<string, object> input)
        {
            var result = await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .PostJsonAsync(input).ReceiveJson<T>();
            return result;
        }

        public async Task<T> ExecuteAndPopulateAsync<T>(T target, IDictionary<string, object> input)
        {
            var result = await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .PostJsonAsync(input).ReceiveString();
            return JsonConvert.DeserializeObject<T>(result, new ExpandoObjectConverter(), new StringEnumConverter());
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetAuthorizationHeaders(INetSuiteLogin login)
        {
            return String.Format(@"NLAuth nlauth_account={0},nlauth_email={1},nlauth_signature={2},nlauth_role={3}", login.Account, login.Email, login.Password, login.Role);
        }

        private Url BuildUrl(IEnumerable<KeyValuePair<string, object>> input)
        {
            var rv = new Url(BaseUrl);
            rv.SetQueryParam("script", ScriptSetting.ScriptName);
            rv.SetQueryParam("deploy", ScriptSetting.DeplomentName);
            return rv;
        }

        #endregion Private Methods
    }
}