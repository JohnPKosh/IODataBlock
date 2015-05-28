using System.Dynamic;
using System.Runtime.CompilerServices;
using Business.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Business.Common.System
{
    public class ObjectBase<T> : IObjectBase<T>
    {
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

        public static T CreateFromJson(string value)
        {
            return ClassExtensions.CreateFromJson<T>(value);
        }

        #region Conversion Operators

        //static public implicit operator ObjectBase<T>(JObject value)
        //{
        //    return value.ToObject<ObjectBase<T>>();
        //}

        static public implicit operator JObject(ObjectBase<T> value)
        {
            return value.ToJObject();
        }

        #endregion Conversion Operators
    }
}