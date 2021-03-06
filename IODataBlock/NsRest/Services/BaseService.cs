﻿using Business.Common.Configuration;
using Business.Common.GenericResponses;
using Newtonsoft.Json.Linq;
using NsRest.Search;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace NsRest.Services
{
    public class BaseService
    {
        #region Class Initialization

        public BaseService(bool useSandbox = false)
        {
            configMgr = new ConfigMgr();
            BaseUrl = configMgr.GetAppSetting(useSandbox ? "nssandboxurl" : "nsbaseurl");

            ScriptSettings = new Dictionary<string, INetSuiteScriptSetting>
            {
                {"crud", NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud")},
                {"cc_crud", NetSuiteScriptSetting.Create("customscript_cr_cc_crud", "customdeploy_cr_cc_crud")},
                {"search", NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search")}
            };

            var NsAccount = configMgr.GetAppSetting("nsaccount");
            var NsEmail = configMgr.GetAppSetting("nsemail");
            var NsPassword = configMgr.GetAppSetting("nspassword");
            var NsRole = configMgr.GetAppSetting("nsrole");
            Login = NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, NsRole);
        }

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

        public static BaseService Create(bool useSandbox = false)
        {
            return new BaseService(useSandbox);
        }

        public static BaseService Create(string baseUrl, INetSuiteLogin login, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            return new BaseService(baseUrl, login, scriptSettings);
        }

        public static BaseService Create(string baseUrl, string nsAccount, string nsEmail, string nsPassword, string nsRole, IDictionary<string, INetSuiteScriptSetting> scriptSettings = null)
        {
            return new BaseService(baseUrl, nsAccount, nsEmail, nsPassword, nsRole, scriptSettings);
        }

        #endregion Factory Methods

        #endregion Class Initialization

        #region Fields and Properties

        public string BaseUrl { get; set; }

        public IDictionary<string, INetSuiteScriptSetting> ScriptSettings { get; set; }

        public INetSuiteLogin Login { get; set; }

        public bool UseExternalId { get; set; }

        private string IdColumnName => UseExternalId ? "externalid" : "id";

        public IDictionary<string, INetSuiteScriptSetting> scriptSettings { get; set; }

        private ConfigMgr configMgr { get; set; }

        #endregion Fields and Properties

        #region Create / Update / Delete

        public IResponseObject<string, string> Create(string typeName, object requestBody, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "request_body", requestBody } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToJsonStringAsync(parameters));
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

        public async Task<IResponseObject<string, string>> CreateAsync(string typeName, object requestBody, string scriptKey = "crud") => await Task.Run(() => Create(typeName, requestBody, scriptKey));

        public IResponseObject<string, string> Update(string id, string typeName, object requestBody, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "type", typeName }, { "request_body", requestBody } };
                var restlet = PutRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToJsonStringAsync(parameters));
                var result = rv.Result;
                ro.ResponseData = result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, string>> UpdateAsync(string id, string typeName, object requestBody, string scriptKey = "crud") => await Task.Run(() => Update(id, typeName, requestBody, scriptKey));

        public IResponseObject<string, bool> DeleteById(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, bool>();
            try
            {
                var restlet = DelRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var input = new Dictionary<string, object> { { IdColumnName, id }, { "type", typeName } };
                var rv = Task.Run(() => restlet.DelAsync(input));
                var result = rv.Result;
                if (result.IsSuccessStatusCode) ro.ResponseData = true;
                else ro.AddException(new Exception(result.ReasonPhrase));
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, bool>> DeleteByIdAsync(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => DeleteById(id, typeName, scriptKey));

        #region JSON Overloads

        public IResponseObject<string, string> CreateByJson(string typeName, string requestBody, string scriptKey = "crud")
        {
            return Create(typeName, JObject.Parse(requestBody), scriptKey);
        }

        public async Task<IResponseObject<string, string>> CreateByJsonAsync(string typeName, string requestBody, string scriptKey = "crud") => await Task.Run(() => CreateByJson(typeName, requestBody, scriptKey));

        public IResponseObject<string, string> UpdateByJson(string id, string typeName, string requestBody, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "type", typeName }, { "request_body", JObject.Parse(requestBody) } };
                var restlet = PutRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToJsonStringAsync(parameters));
                var result = rv.Result;
                ro.ResponseData = result;
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, string>> UpdateByJsonAsync(string id, string typeName, string requestBody, string scriptKey = "crud") => await Task.Run(() => UpdateByJson(id, typeName, requestBody, scriptKey));

        #endregion JSON Overloads

        #endregion Create / Update / Delete

        #region Get Methods

        public IResponseObject<string, string> GetJsonById(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { IdColumnName, id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToJsonStringAsync(parameters));
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

        public async Task<IResponseObject<string, string>> GetJsonByIdAsync(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetJsonById(id, typeName, scriptKey));

        public IResponseObject<string, JObject> GetJobjectById(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, JObject>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { IdColumnName, id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToJsonStringAsync(parameters));
                ro.ResponseData = JObject.Parse(rv.Result);
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, JObject>> GetJobjectByIdAsync(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetJobjectById(id, typeName, scriptKey));

        public IResponseObject<string, dynamic> GetDynamicById(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, dynamic>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { IdColumnName, id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToDynamicAsync(parameters));
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

        public async Task<IResponseObject<string, dynamic>> GetDynamicByIdAsync(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetDynamicById(id, typeName, scriptKey));

        public IResponseObject<string, T> GetById<T>(string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, T>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { IdColumnName, id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToAsync<T>(parameters));
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

        public async Task<IResponseObject<string, T>> GetByIdAsync<T>(string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetByIdAsync<T>(id, typeName, scriptKey));

        public IResponseObject<string, T> GetAndPopulateById<T>(T target, string id, string typeName, string scriptKey = "crud")
        {
            var ro = new ResponseObject<string, T>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { IdColumnName, id } };
                var restlet = GetRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteAndPopulateAsync<T>(target, parameters));
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

        public async Task<IResponseObject<string, T>> GetAndPopulateByIdAsync<T>(T target, string id, string typeName, string scriptKey = "crud") => await Task.Run(() => GetAndPopulateById<T>(target, id, typeName, scriptKey));

        #endregion Get Methods

        #region Search Methods

        public IResponseObject<string, string> SearchJson(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search")
        {
            var ro = new ResponseObject<string, string>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "searchFilters", filters }, { "savedSearch", savedSearch }, { "columns", columns } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToJsonStringAsync(parameters));
                ro.ResponseData = rv.Result;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, string>> SearchJsonAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search") => await Task.Run(() => SearchJson(typeName, filters, columns, savedSearch, scriptKey));

        public IResponseObject<string, IList<ExpandoObject>> SearchDynamicList(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search")
        {
            var ro = new ResponseObject<string, IList<ExpandoObject>>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "searchFilters", filters }, { "savedSearch", savedSearch }, { "columns", columns } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToDynamicListAsync(parameters));
                //ro.ResponseData = rv.Result;
                var result = rv.Result;
                ro.ResponseData = result ?? new List<ExpandoObject>();
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
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
                var rv = Task.Run(() => restlet.ExecuteToJArrayAsync(parameters));
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

        public async Task<IResponseObject<string, JArray>> SearchJArrayAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search") => await Task.Run(() => SearchJArray(typeName, filters, columns, savedSearch, scriptKey));

        public IResponseObject<string, IEnumerable<JObject>> SearchJObjects(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search")
        {
            var ro = new ResponseObject<string, IEnumerable<JObject>>();
            try
            {
                var parameters = new Dictionary<string, object> { { "type", typeName }, { "searchFilters", filters }, { "savedSearch", savedSearch }, { "columns", columns } };
                var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
                var rv = Task.Run(() => restlet.ExecuteToJArrayAsync(parameters));
                var result = rv.Result;
                if (result.HasValues) ro.ResponseData = result.Children<JObject>();
                else ro.ResponseData = new List<JObject>();
                if (rv.Exception != null) throw rv.Exception;
                return ro;
            }
            catch (Exception ex)
            {
                ro.AddException(ex);
                return ro;
            }
        }

        public async Task<IResponseObject<string, IEnumerable<JObject>>> SearchJObjectsAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "search") => await Task.Run(() => SearchJObjects(typeName, filters, columns, savedSearch, scriptKey));

        #endregion Search Methods

        #region Unused

        //public IResponseObject<string, string> CreateByDynamic(string typeName, dynamic requestBody, string scriptKey = "crud")
        //{
        //    var ro = new ResponseObject<string, string>();
        //    try
        //    {
        //        var parameters = new Dictionary<string, object> { { "type", typeName }, { "request_body", requestBody } };
        //        var restlet = PostRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
        //        var rv = restlet.ExecuteToJsonStringAsync(parameters);
        //        ro.ResponseData = rv.Result;
        //        if (rv.Exception != null) throw rv.Exception;
        //        return ro;
        //    }
        //    catch (Exception ex)
        //    {
        //        ro.AddException(ex);
        //        return ro;
        //    }
        //}

        //public IResponseObject<string, string> UpdateByDynamic(string id, string typeName, dynamic requestBody, string scriptKey = "crud")
        //{
        //    var ro = new ResponseObject<string, string>();
        //    try
        //    {
        //        var parameters = new Dictionary<string, object> { { IdColumnName, id }, { "type", typeName }, { "request_body", requestBody } };
        //        var restlet = PutRestletBase.Create(BaseUrl, ScriptSettings[scriptKey], Login);
        //        var rv = restlet.ExecuteToJsonStringAsync(parameters);
        //        ro.ResponseData = rv.Result;
        //        if (rv.Exception != null) throw rv.Exception;
        //        return ro;
        //    }
        //    catch (Exception ex)
        //    {
        //        ro.AddException(ex);
        //        return ro;
        //    }
        //}

        //public async Task<IResponseObject<string, string>> UpdateByDynamicAsync(string id, string typeName, dynamic requestBody, string scriptKey = "crud") => await Task.Run(() => UpdateByDynamic(id, typeName, requestBody, scriptKey));

        //public IResponseObject<string, string> FindIdByExternalId(string externalId, string typeName, string scriptKey = "search")
        //{
        //    var ro = new ResponseObject<string, string>();
        //    try
        //    {
        //        var filters = new[] { NsSearchFilter.NewStringFilter("externalid", SearchStringFieldOperatorType.Is, externalId) };
        //        var response = SearchDynamicList("customer", filters, scriptKey: scriptKey);
        //        if(response.HasExceptions)response
        //        dynamic rv = response.ResponseData.First();

        //        var response2 = baseService.GetJsonStringById(rv.id, "customer", "crud");
        //    }
        //    catch (Exception ex)
        //    {
        //        ro.AddException(ex);
        //        return ro;
        //    }
        //}

        #endregion Unused
    }
}