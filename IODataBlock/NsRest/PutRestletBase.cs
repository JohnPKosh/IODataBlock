﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace NsRest
{
    public class PutRestletBase:IRestletBase
    {
        public PutRestletBase(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            BaseUrl = baseUrl;
            ScriptSetting = scriptSetting;
            Login = login;
            HttpMethodType = HttpMethodType.Get;
        }

        public static PutRestletBase Create(String baseUrl, INetSuiteScriptSetting scriptSetting, INetSuiteLogin login)
        {
            return new PutRestletBase(baseUrl, scriptSetting, login);
        }

        public string BaseUrl { get; set; }
        public INetSuiteScriptSetting ScriptSetting { get; set; }
        public HttpMethodType HttpMethodType { get; set; }
        public INetSuiteLogin Login { get; set; }

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

        public async Task<string> ExecuteToJsonStringAsync(IDictionary<string, object> input)
        {
            var result = await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .PutJsonAsync(input).ReceiveString();
            return result;
        }

        public async Task<dynamic> ExecuteToDynamicAsync(IDictionary<string, object> input)
        {
            var result = await BuildUrl(input)
                .ConfigureHttpClient(http => http.DefaultRequestHeaders.TryAddWithoutValidation(@"Authorization", GetAuthorizationHeaders(Login)))
                .PutJsonAsync(input).ReceiveJson();
            return result;
        }

        public async Task<IList<ExpandoObject>> ExecuteToDynamicListAsync(IDictionary<string, object> input)
        {
            throw new NotImplementedException();
        }

        public async Task<T> ExecuteToAsync<T>(IDictionary<string, object> input)
        {
            throw new NotImplementedException();
        }

        public async Task<T> ExecuteAndPopulateAsync<T>(T target, IDictionary<string, object> input)
        {
            throw new NotImplementedException();
        }
    }
}