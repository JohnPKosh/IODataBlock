using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Business.Common.Extensions
{
    public static partial class ClassExtensions
    {
        #region Json.net Deserialization

        #region Json.net Populate

        public static void PopulateObjectFromJson(this object target, string value)
        {
            JsonConvert.PopulateObject(value, target, null);
        }

        public static void PopulateObjectFromJson(this object target, string value, JsonSerializerSettings settings)
        {
            JsonConvert.PopulateObject(value, target, settings);
        }

        #endregion Json.net Populate

        #region Json.net JObject Deserialization

        public static JObject ConvertJson(this string value)
        {
            return JsonConvert.DeserializeObject(value, null, (JsonSerializerSettings)null) as JObject;
        }

        public static JObject ConvertJson(this string value, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(value, null, settings) as JObject;
        }

        public static JObject ConvertJson(this string value, params JsonConverter[] converters)
        {
            var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
            return JsonConvert.DeserializeObject(value, null, settings) as JObject;
        }

        #endregion Json.net JObject Deserialization

        #region Json.net object Deserialization

        public static object ConvertJson(this string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, (JsonSerializerSettings)null);
        }

        public static object ConvertJson(this string value, Type type, params JsonConverter[] converters)
        {
            var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
            return JsonConvert.DeserializeObject(value, type, settings);
        }

        public static object ConvertJson(this string value, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(value, type, settings);
        }

        #endregion Json.net object Deserialization

        #region Json.net ExpandoObject Deserialization

        public static ExpandoObject ConvertJsonExpando(this string value)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(value, new ExpandoObjectConverter(), new StringEnumConverter());
        }

        public static ExpandoObject ConvertJsonExpando(this string value, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            return JsonConvert.DeserializeObject<ExpandoObject>(value, converterList.ToArray());
        }

        public static ExpandoObject ConvertJsonExpando(this string value, JsonSerializerSettings settings)
        {
            if (settings.Converters.All(x => x.GetType() != typeof(ExpandoObjectConverter))) settings.Converters.Add(new ExpandoObjectConverter());
            if (settings.Converters.All(x => x.GetType() != typeof(StringEnumConverter))) settings.Converters.Add(new StringEnumConverter());
            return value.ConvertJson<ExpandoObject>(settings);
        }

        #endregion Json.net ExpandoObject Deserialization

        #region Json.net <T> Deserialization

        public static T ConvertJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings)null);
        }

        public static T ConvertJson<T>(this string value, params JsonConverter[] converters)
        {
            return (T)JsonConvert.DeserializeObject(value, typeof(T), converters);
        }

        public static T ConvertJson<T>(this string value, JsonSerializerSettings settings)
        {
            return (T)JsonConvert.DeserializeObject(value, typeof(T), settings);
        }

        #endregion Json.net <T> Deserialization

        #region Json.net AnonymousType<T> Deserialization

        public static T ConvertJsonToAnonymousType<T>(this string value, T anonymousTypeObject)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T ConvertJsonToAnonymousType<T>(this string value, T anonymousTypeObject, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        #endregion Json.net AnonymousType<T> Deserialization

        #region Json.net File Deserialization

        public static T ReadJsonFile<T>(this FileInfo file)
        {
            using (var sr = file.OpenText())
            {
                return sr.ReadToEnd().ConvertJson<T>();
            }
        }

        public static T ReadJsonFile<T>(this FileInfo file, JsonSerializerSettings settings)
        {
            using (var sr = file.OpenText())
            {
                using (var jr = new JsonTextReader(sr))
                {
                    return JsonSerializer.CreateDefault(settings).Deserialize<T>(jr);
                }
            }
        }

        public static T ReadJsonFile<T>(this FileInfo file, params JsonConverter[] converters)
        {
            var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
            using (var sr = file.OpenText())
            {
                using (var jr = new JsonTextReader(sr))
                {
                    return JsonSerializer.CreateDefault(settings).Deserialize<T>(jr);
                }
            }
        }

        /* TODO add test methods for all 3 below */

        public static ExpandoObject ReadJsonFile(this FileInfo file)
        {
            using (var sr = file.OpenText())
            {
                return sr.ReadToEnd().ConvertJsonExpando();
            }
        }

        public static ExpandoObject ReadJsonFile(this FileInfo file, JsonSerializerSettings settings)
        {
            using (var sr = file.OpenText())
            {
                return sr.ReadToEnd().ConvertJsonExpando(settings);
            }
        }

        public static ExpandoObject ReadJsonFile(this FileInfo file, params JsonConverter[] converters)
        {
            var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
            using (var sr = file.OpenText())
            {
                return sr.ReadToEnd().ConvertJsonExpando(settings);
            }
        }

        #endregion Json.net File Deserialization

        #endregion Json.net Deserialization

        #region Json.net Utilities

        #region Json.net JToken

        public static JToken ToJToken(this object value)
        {
            return JToken.FromObject(value);
        }

        #endregion Json.net JToken

        #region Json.net JObject

        public static JObject ToJObject(this object value)
        {
            return (JObject)JToken.FromObject(value);
        }

        public static T ToOrOther<T>(this JObject obj, string propertyName, T other)
        {
            try
            {
                JToken temp;
                //return obj.TryGetValue(propertyName, out temp) ? temp.Value<T>(): other;
                return obj.TryGetValue(propertyName, out temp) ? temp.ToObject<T>() : other;
            }
            catch
            {
                return other;
            }
        }

        public static T ToOrOther<T>(this JToken obj, T other)
        {
            try
            {
                return obj.ToObject<T>();
            }
            catch
            {
                return other;
            }
        }

        public static XmlDocument ToJObjectXml(this object value)
        {
            //return (JObject)JToken.FromObject(value);
            return JsonConvert.DeserializeXmlNode(JToken.FromObject(value).ToString(),"Root");
        }

        #endregion Json.net JObject

        public static T ConvertExpandoTo<T>(this ExpandoObject source)
        {
            return JsonConvert.DeserializeObject<T>(source.ToJsonString(new ExpandoObjectConverter()));
        }

        #endregion Json.net Utilities

        #region BSON Serialization

        public static byte[] ToBsonByteArray(this object value)
        {
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, value);
                return ms.ToArray();
            }
        }

        public static byte[] ToBsonByteArray(this object value, params JsonConverter[] converters)
        {
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                foreach (var c in converters)
                {
                    serializer.Converters.Add(c);
                }
                serializer.Serialize(writer, value);
                return ms.ToArray();
            }
        }

        #endregion BSON Serialization

        #region BSON Deserialization

        public static JObject ConvertBson(this Stream value)
        {
            using (var reader = new BsonReader(value))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JObject>(reader);
            }
            //return JsonConvert.DeserializeObject(value, null, (JsonSerializerSettings)null) as JObject;
        }

        public static ExpandoObject ConvertBsonExpando(this Stream value)
        {
            using (var reader = new BsonReader(value))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JObject>(reader).ToExpando();
            }
        }

        /* TODO add test methods for all 3 below */

        public static ExpandoObject ReadBsonFile(this FileInfo file)
        {
            using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return fs.ConvertBsonExpando();
            }
        }

        //public static ExpandoObject ReadJsonFile(this FileInfo file, JsonSerializerSettings settings)
        //{
        //    using (var sr = file.OpenText())
        //    {
        //        return sr.ReadToEnd().ConvertJsonExpando(settings);
        //    }
        //}

        //public static ExpandoObject ReadJsonFile(this FileInfo file, params JsonConverter[] converters)
        //{
        //    var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
        //    using (var sr = file.OpenText())
        //    {
        //        return sr.ReadToEnd().ConvertJsonExpando(settings);
        //    }
        //}

        #endregion BSON Deserialization

        #region From IObjectBase

        public static T CreateFromJson<T>(string value)
        {
            return value.ConvertJson<T>();
        }

        public static T CreateFromJson<T>(string value, params JsonConverter[] converters)
        {
            return value.ConvertJson<T>(converters);
        }

        public static T CreateFromJson<T>(string value, JsonSerializerSettings settings)
        {
            return value.ConvertJson<T>(settings);
        }

        public static T CreateFromExpando<T>(dynamic value)
        {
            return (value as ExpandoObject).ConvertExpandoTo<T>();
        }

        public static T CreateFromExpando<T>(ExpandoObject value)
        {
            return value.ConvertExpandoTo<T>();
        }

        /*
        
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

        */

        #endregion
    }
}