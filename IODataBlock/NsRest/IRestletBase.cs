using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace NsRest
{
    public interface IRestletBase
    {
        String BaseUrl { set; get; }

        INetSuiteScriptSetting ScriptSetting { get; set; }

        HttpMethodType HttpMethodType { set; get; }

        INetSuiteLogin Login { set; get; }

        Task<string> ExecuteToJsonStringAsync(IDictionary<string,object> input);

        Task<dynamic> ExecuteToDynamicAsync(IDictionary<string, object> input);

        Task<IList<ExpandoObject>> ExecuteToDynamicListAsync(IDictionary<string, object> input);

        Task<T> ExecuteToAsync<T>(IDictionary<string, object> input);

        Task<T> ExecuteAndPopulateAsync<T>(T target, IDictionary<string, object> input);
    }
}