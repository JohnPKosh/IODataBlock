using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NsRest
{
    public class DelRestletBase
    {
        public DelRestletBase(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            BaseUrl = baseUrl;
            ScriptSetting = scriptSetting;
            Login = login;
            HttpMethodType = HttpMethodType.Get;
        }

        public static DelRestletBase Create(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            return new DelRestletBase(baseUrl, scriptSetting, login);
        }

        public string BaseUrl { get; set; }
        public INetSuiteScriptSetting ScriptSetting { get; set; }
        public HttpMethodType HttpMethodType { get; set; }
        public INetSuiteLogin Login { get; set; }

        private static string GetAuthorizationHeaders(INetSuiteLogin login)
        {
            return String.Format(@"NLAuth nlauth_account={0},nlauth_email={1},nlauth_signature={2},nlauth_role={3}", login.Account, login.Email, login.Password, login.Role);
        }

        //private Url BuildUrl(IEnumerable<KeyValuePair<string, object>> input)
        //{
        //    var rv = new Url(BaseUrl);
        //    rv.SetQueryParam("script", ScriptSetting.ScriptName);
        //    rv.SetQueryParam("deploy", ScriptSetting.DeplomentName);
        //    return rv;
        //}

        private Url BuildUrl(string type, string id)
        {
            var rv = new Url(BaseUrl);
            rv.SetQueryParam("script", ScriptSetting.ScriptName);
            rv.SetQueryParam("deploy", ScriptSetting.DeplomentName);
            rv.SetQueryParam("type", type);
            rv.SetQueryParam("id", id);
            return rv;
        }

        private Url BuildUrl(IEnumerable<KeyValuePair<string, object>> input = null)
        {
            var rv = new Url(BaseUrl);
            rv.SetQueryParam("script", ScriptSetting.ScriptName);
            rv.SetQueryParam("deploy", ScriptSetting.DeplomentName);
            if (input == null) return rv;
            foreach (var o in input)
            {
                rv.SetQueryParam(o.Key, o.Value);
            }
            return rv;
        }

        public async Task<HttpResponseMessage> DelAsync(IDictionary<string, object> input)
        {
            return await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .SendAsync(HttpMethod.Delete, (HttpContent)new CapturedJsonContent(JsonConvert.SerializeObject(input)), new CancellationToken?(), HttpCompletionOption.ResponseContentRead);
        }

        public bool Delete(IDictionary<string, object> input)
        {
            var rv = DelAsync(input);
            var result = rv.Result;
            return result.IsSuccessStatusCode;
        }

        public async Task<HttpResponseMessage> DelAsync(string type, string id)
        {
            return await BuildUrl(type, id)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .SendAsync(HttpMethod.Delete, (HttpContent)new CapturedJsonContent(JsonConvert.SerializeObject(new { type = type, id = id })), new CancellationToken?(), HttpCompletionOption.ResponseContentRead);
        }

        public bool Delete(string type, string id)
        {
            var rv = DelAsync(type, id);
            var result = rv.Result;
            return result.IsSuccessStatusCode;
        }

        //public async Task<string> ExecuteToJsonStringAsync(IDictionary<string, object> input)
        //{
        //    var result = await BuildUrl(input)
        //        .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
        //        .PutJsonAsync(input).ReceiveString();
        //    return result;
        //}

        //public async Task<dynamic> ExecuteToDynamicAsync(IDictionary<string, object> input)
        //{
        //    var result = await BuildUrl(input)
        //        .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
        //        .PutJsonAsync(input).ReceiveJson();
        //    return result;
        //}

        //public async Task<IList<ExpandoObject>> ExecuteToDynamicListAsync(IDictionary<string, object> input)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<T> ExecuteToAsync<T>(IDictionary<string, object> input)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<T> ExecuteAndPopulateAsync<T>(T target, IDictionary<string, object> input)
        //{
        //    throw new NotImplementedException();
        //}
    }
}