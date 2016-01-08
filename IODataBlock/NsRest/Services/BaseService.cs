using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.GenericResponses;
using Flurl;
using Flurl.Http;
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

        #region Read Contacts

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

        #endregion

        #region Search Methods

        public IResponseObject<string, string> SearchJsonString(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "crud")
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

        public async Task<IResponseObject<string, string>> SearchJsonStringAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "crud") => await Task.Run(() => SearchJsonString(typeName, filters, columns, savedSearch, scriptKey));

        public IResponseObject<string, IList<ExpandoObject>> SearchDynamicList(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "crud")
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

        public async Task<IResponseObject<string, IList<ExpandoObject>>> SearchDynamicListAsync(string typeName, IEnumerable<NsSearchFilter> filters, IEnumerable<string> columns = null, string savedSearch = null, string scriptKey = "crud") => await Task.Run(() => SearchDynamicList(typeName, filters, columns, savedSearch, scriptKey));

        #endregion


        #endregion

        #region Private Utility Methods



        #endregion

        #endregion

    }
}
