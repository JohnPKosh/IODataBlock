using System.Dynamic;
using Business.Common.Extensions;
using Newtonsoft.Json;

namespace Business.Common.System
{
    public class ObjectBase<T> : IObjectBase
    {
        public static T CreateFromJson(string value)
        {
            return value.ConvertJson<T>();
        }

        public static T CreateFromJson(string value, params JsonConverter[] converters)
        {
            return value.ConvertJson<T>(converters);
        }

        public static T CreateFromJson(string value, JsonSerializerSettings settings)
        {
            return value.ConvertJson<T>(settings);
        }

        public static T CreateFromExpando(dynamic value)
        {
            return (value as ExpandoObject).ConvertExpandoTo<T>();
        }

        public static T CreateFromExpando(ExpandoObject value)
        {
            return value.ConvertExpandoTo<T>();
        }

        public string ToJson(bool indented = false)
        {
            return JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);
        }

        public void PopulateFromJson(string value)
        {
            this.PopulateObjectFromJson(value);
        }

        public void PopulateFromJson(string value, object target, JsonSerializerSettings settings)
        {
            this.PopulateObjectFromJson(value, settings);
        }
    }
}