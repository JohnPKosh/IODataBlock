using Business.Common.Extensions;
using Business.Common.System;
using Flurl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Business.HttpClient.Navigation
{
    public class ApiUrl : IApiUrl, IObjectBase<ApiUrl>
    {
        public string Root { get; set; }
        public IEnumerable<string> PathSegments { get; set; }
        public IDictionary<string, object> QueryParams { get; set; }
        public IEnumerable<object> Args { get; set; }

        public ApiUrl()
        {
            PathSegments = new List<string>();
            QueryParams = new Dictionary<string, object>();
        }

        public ApiUrl(string root, IEnumerable<string> pathSegments = null, Dictionary<string, object> queryParams = null)
        {
            Root = root;
            PathSegments = pathSegments ?? new List<string>();
            QueryParams = queryParams ?? new Dictionary<string, object>();
        }

        internal Url GetUrl()
        {
            return Root.AppendPathSegments(PathSegments).SetQueryParams(QueryParams);
        }

        public string ToJson(bool indented = false)
        {
            return JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);
        }

        public void PopulateFromJson(string value)
        {
            this.PopulateObjectFromJson(value);
        }

        public void PopulateFromJson(string value, JsonSerializerSettings settings)
        {
            this.PopulateObjectFromJson(value, settings);
        }

        public static implicit operator string(ApiUrl value)
        {
            return value.GetUrl();
        }

        public static implicit operator Url(ApiUrl value)
        {
            return value.GetUrl();
        }

        public static implicit operator JObject(ApiUrl value)
        {
            return value.ToJObject();
        }

        public static implicit operator ApiUrl(JObject value)
        {
            return value.ToObject<ApiUrl>();
        }

        public string Format(params object[] args)
        {
            return string.Format(GetUrl(), args);
        }

        //private string parseUrl()
        //{
        //    if (Args == null || !DynamicQueryable.Any(Args)) return GetUrl();
        //    var args = Args.ToArray();

        //}
    }
}