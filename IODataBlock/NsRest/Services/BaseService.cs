using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.GenericResponses;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using NsRest.Search;

namespace NsRest.Services
{
    public class BaseService
    {
        #region Class Initialization

        public BaseService(string baseUrl, INetSuiteLogin login, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            BaseUrl = baseUrl;
            ScriptSettings = scriptSettings;
            Login = login;
        }

        public BaseService(string baseUrl, string nsAccount, string nsEmail, string nsPassword, string nsRole, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            BaseUrl = baseUrl;
            ScriptSettings = scriptSettings;
            Login = NetSuiteLogin.Create(nsAccount, nsEmail, nsPassword, nsRole);
        }

        #region Factory Methods

        public static BaseService Create(string baseUrl, INetSuiteLogin login, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            return new BaseService(baseUrl, login, scriptSettings);
        }

        public static BaseService Create(string baseUrl, string nsAccount, string nsEmail, string nsPassword, string nsRole, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            return new BaseService(baseUrl, nsAccount, nsEmail, nsPassword, nsRole, scriptSettings);
        } 

        #endregion

        #endregion Class Initialization

        #region Fields and Properties

        public string BaseUrl { get; set; }

        public IDictionary<string, INetSuiteScriptSetting> ScriptSettings { get; set; }

        public INetSuiteLogin Login { get; set; }

        #endregion Fields and Properties

        #region Raw API Implementation

        #region Create / Update / Delete


        #endregion

        #region Read Objects

        #region Get Methods

        public IResponseObject<string, string> GetJsonStringById(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "id", id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, string>> GetJsonStringByIdAsync(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetJsonStringById(id, typeName, scriptKey));

        public IResponseObject<string, JObject> GetJobjectById(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, JObject>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "id", id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                ro.ResponseData = JObject.Parse(rv.Result);
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, JObject>> GetJobjectByIdAsync(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetJobjectById(id, typeName, scriptKey));

        public IResponseObject<string, dynamic> GetDynamicById(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, dynamic>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "id", id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToDynamicAsync(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, dynamic>> GetDynamicByIdAsync(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetDynamicById(id, typeName, scriptKey));
        
        public IResponseObject<string, T> GetById<T>(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, T>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "id", id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToAsync<T>(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, T>> GetByIdAsync<T>(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetByIdAsync<T>(id, typeName, scriptKey));

        public IResponseObject<string, T> GetAndPopulateById<T>(T target, string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, T>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "id", id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteAndPopulateAsync<T>(target, parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, T>> GetAndPopulateByIdAsync<T>(T target, string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetAndPopulateById<T>(target, id, typeName, scriptKey));
        
        #endregion

        #region Search Methods

        public IResponseObject<string, string> SearchJsonString(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "searchFilters", filters }, { "savedSearch", savedSearch }, { "columns", columns } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJsonStringAsync(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, string>> SearchJsonStringAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search") => await Task.Run(() => SearchJsonString(typeName, filters, columns, savedSearch, scriptKey));

        public IResponseObject<string, IList<ExpandoObject>> SearchDynamicList(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search")
        {
            var ro = new ResponseObject<string, IList<ExpandoObject>>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "searchFilters", filters }, { "savedSearch", savedSearch }, { "columns", columns } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToDynamicListAsync(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, IList<ExpandoObject>>> SearchDynamicListAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search") => await Task.Run(() => SearchDynamicList(typeName, filters, columns, savedSearch, scriptKey));

        public IResponseObject<string, JArray> SearchJArray(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search")
        {
            var ro = new ResponseObject<string, JArray>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "searchFilters", filters }, { "savedSearch", savedSearch }, { "columns", columns } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJArrayAsync(parameters);
                ro.ResponseData = rv.Result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, JArray>> SearchJArrayAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search") => await Task.Run(() => SearchJArray(typeName, filters, columns, savedSearch, scriptKey));

        public IResponseObject<string, IEnumerable<JObject>> SearchJObjects(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search")
        {
            var ro = new ResponseObject<string, IEnumerable<JObject>>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "searchFilters", filters }, { "savedSearch", savedSearch }, { "columns", columns } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = restlet.ExecuteToJArrayAsync(parameters);
                var result = rv.Result;
                if (result.HasValues) ro.ResponseData = result.Children<JObject>();
                else ro.ResponseData = new List<JObject>();
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.ExceptionList.Add(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, IEnumerable<JObject>>> SearchJObjectsAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search") => await Task.Run(() => SearchJObjects(typeName, filters, columns, savedSearch, scriptKey));


        #endregion


        #endregion

        #region Private Utility Methods



        #endregion

        #endregion

    }
}
