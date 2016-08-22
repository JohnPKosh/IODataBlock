using Business.Common.GenericResponses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NsRest.Services
{
    public class CreditCardService
    {
        #region Class Initialization

        public CreditCardService(string baseUrl, INetSuiteLogin login, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            BaseUrl = baseUrl;
            ScriptSettings = scriptSettings;
            Login = login;
        }

        public CreditCardService(string baseUrl, string nsAccount, string nsEmail, string nsPassword, string nsRole, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            BaseUrl = baseUrl;
            ScriptSettings = scriptSettings;
            Login = NetSuiteLogin.Create(nsAccount, nsEmail, nsPassword, nsRole);
        }

        #region Factory Methods

        public static CreditCardService Create(string baseUrl, INetSuiteLogin login, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            return new CreditCardService(baseUrl, login, scriptSettings);
        }

        public static CreditCardService Create(string baseUrl, string nsAccount, string nsEmail, string nsPassword, string nsRole, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            return new CreditCardService(baseUrl, nsAccount, nsEmail, nsPassword, nsRole, scriptSettings);
        }

        #endregion Factory Methods

        #endregion Class Initialization

        #region Fields and Properties

        public string BaseUrl { get; set; }

        public IDictionary<string, INetSuiteScriptSetting> ScriptSettings { get; set; }

        public INetSuiteLogin Login { get; set; }

        public bool UseExternalId { get; set; }

        private string IdColumnName => UseExternalId ? "externalid" : "id";

        #endregion Fields and Properties

        #region Create / Read / Update / Delete

        public IResponseObject<string, string> Create(string id, object requestBody, string scriptKey = "cc_crud")
        {
            return ExecuteMethod(id, requestBody, "CREATE", scriptKey);
        }

        public async Task<IResponseObject<string, string>> CreateAsync(string id, object requestBody, string scriptKey = "cc_crud") => await Task.Run(() => Create(id, requestBody, scriptKey));

        public IResponseObject<string, string> Read(string id, object requestBody = null, string scriptKey = "cc_crud")
        {
            return ExecuteMethod(id, requestBody, "READ", scriptKey);
        }

        public async Task<IResponseObject<string, string>> ReadAsync(string id, object requestBody = null, string scriptKey = "cc_crud") => await Task.Run(() => Read(id, requestBody, scriptKey));

        public IResponseObject<string, string> Update(string id, object requestBody, string scriptKey = "cc_crud")
        {
            return ExecuteMethod(id, requestBody, "UPDATE", scriptKey);
        }

        public async Task<IResponseObject<string, string>> UpdateAsync(string id, object requestBody, string scriptKey = "cc_crud") => await Task.Run(() => Update(id, requestBody, scriptKey));

        public IResponseObject<string, string> Delete(string id, object requestBody, string scriptKey = "cc_crud")
        {
            return ExecuteMethod(id, requestBody, "DELETE", scriptKey);
        }

        public async Task<IResponseObject<string, string>> DeleteAsync(string id, object requestBody, string scriptKey = "cc_crud") => await Task.Run(() => Delete(id, requestBody, scriptKey));

        #region JSON Overloads

        public IResponseObject<string, string> CreateByJson(string id, string requestBody, string scriptKey = "crud")
        {
            return Create(id, JObject.Parse(requestBody), scriptKey);
        }

        public async Task<IResponseObject<string, string>> CreateByJsonAsync(string id, string requestBody, string scriptKey = "crud") => await CreateAsync(id, JObject.Parse(requestBody), scriptKey);

        public IResponseObject<string, string> ReadByJson(string id, string requestBody = null, string scriptKey = "crud")
        {
            return Read(id, string.IsNullOrWhiteSpace(requestBody) ? null : JObject.Parse(requestBody), scriptKey);
        }

        public async Task<IResponseObject<string, string>> ReadByJsonAsync(string id, string requestBody = null, string scriptKey = "crud") => await ReadAsync(id, string.IsNullOrWhiteSpace(requestBody) ? null : JObject.Parse(requestBody), scriptKey);

        public IResponseObject<string, string> UpdateByJson(string id, string typeName, string requestBody, string scriptKey = "crud")
        {
            return Update(id, JObject.Parse(requestBody), scriptKey);
        }

        public async Task<IResponseObject<string, string>> UpdateByJsonAsync(string id, string requestBody, string scriptKey = "crud") => await UpdateAsync(id, JObject.Parse(requestBody), scriptKey);

        public IResponseObject<string, string> DeleteByJson(string id, string typeName, string requestBody, string scriptKey = "crud")
        {
            return Delete(id, JObject.Parse(requestBody), scriptKey);
        }

        public async Task<IResponseObject<string, string>> DeleteByJsonAsync(string id, string requestBody, string scriptKey = "crud") => await DeleteAsync(id, JObject.Parse(requestBody), scriptKey);

        #endregion JSON Overloads

        #endregion Create / Read / Update / Delete

        #region Utility Methods

        public IResponseObject<string, string> ExecuteMethod(string id, object requestBody = null, string method = "READ", string scriptKey = "cc_crud")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "method", method } };
                if (requestBody != null) parameters.Add("request_body", requestBody);
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, IList<ExpandoObject>> ExecuteMethodToDynamicList(string id, object requestBody = null, string method = "READ", string scriptKey = "cc_crud")
        {
            var ro = new ResponseObject<string, IList<ExpandoObject>>();
            try
            {
                var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "method", method } };
                if (requestBody != null) parameters.Add("request_body", requestBody);
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToDynamicListAsync(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, JArray> ExecuteMethodToJArray(string id, object requestBody = null, string method = "READ", string scriptKey = "cc_crud")
        {
            var ro = new ResponseObject<string, JArray>();
            try
            {
                var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "method", method } };
                if (requestBody != null) parameters.Add("request_body", requestBody);
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJArrayAsync(parameters);
                var result = rv.Result;
                ro.ResponseData = result.HasValues ? result : new JArray();
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, IList<JObject>> ExecuteMethodToJObjectList(string id, object requestBody = null, string method = "READ", string scriptKey = "cc_crud")
        {
            var ro = new ResponseObject<string, IList<JObject>>();
            try
            {
                var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "method", method } };
                if (requestBody != null) parameters.Add("request_body", requestBody);
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJArrayAsync(parameters);
                var result = rv.Result;
                ro.ResponseData = result.HasValues ? result.Children<JObject>().ToList() : new List<JObject>();
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public IResponseObject<string, IList<T>> ExecuteMethodTo<T>(string id, object requestBody = null, string method = "READ", string scriptKey = "cc_crud")
        {
            var ro = new ResponseObject<string, IList<T>>();
            try
            {
                var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "method", method } };
                if (requestBody != null) parameters.Add("request_body", requestBody);
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJArrayAsync(parameters);
                var result = rv.Result;
                ro.ResponseData = result.HasValues ? result.Children<JObject>().Select(x => x.ToObject<T>()).ToList() : new List<T>();
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        #endregion Utility Methods
    }
}