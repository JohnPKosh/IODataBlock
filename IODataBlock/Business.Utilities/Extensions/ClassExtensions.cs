using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Business.Utilities.Extensions
{
    public static class ClassExtensions
    {
        #region Json.net Serialization

        #region Json.net object Serialization

        public static string ToJsonString(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static string ToJsonString(this object value, bool indented)
        {
            return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None);
        }

        public static string ToJsonString(this object value, params JsonConverter[] converters)
        {
            var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this object value, bool indented, params JsonConverter[] converters)
        {
            var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this object value, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, type, settings);
        }

        public static string ToJsonString(this object value, bool indented, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this object value, Type type, bool indented, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, type, indented ? Formatting.Indented : Formatting.None, settings);
        }

        #endregion Json.net object Serialization

        #region Json.net ExpandoObject Serialization

        public static string ToJsonString(this ExpandoObject value)
        {
            return JsonConvert.SerializeObject(value, new ExpandoObjectConverter(), new StringEnumConverter());
        }

        public static string ToJsonString(this ExpandoObject value, bool indented)
        {
            return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, new ExpandoObjectConverter(), new StringEnumConverter());
        }

        public static string ToJsonString(this ExpandoObject value, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this ExpandoObject value, bool indented, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this ExpandoObject value, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this ExpandoObject value, Type type, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, type, settings);
        }

        public static string ToJsonString(this ExpandoObject value, bool indented, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this ExpandoObject value, Type type, bool indented, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, type, indented ? Formatting.Indented : Formatting.None, settings);
        }

        #endregion Json.net ExpandoObject Serialization

        #region Json.net File Serialization

        #region WriteJsonToFilePath

        public static void WriteJsonToFilePath(this object value, String filePath)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString());
            }
        }

        public static void WriteJsonToFilePath(this object value, String filePath, bool indented)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString(indented));
            }
        }

        public static void WriteJsonToFilePath(this object value, String filePath, params JsonConverter[] converters)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString(converters));
            }
        }

        public static void WriteJsonToFilePath(this object value, String filePath, bool indented, params JsonConverter[] converters)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString(indented, converters));
            }
        }

        public static void WriteJsonToFilePath(this object value, String filePath, JsonSerializerSettings settings)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString(settings));
            }
        }

        public static void WriteJsonToFilePath(this object value, String filePath, Type type, JsonSerializerSettings settings)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString(type, settings));
            }
        }

        public static void WriteJsonToFilePath(this object value, String filePath, bool indented, JsonSerializerSettings settings)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString(indented, settings));
            }
        }

        public static void WriteJsonToFilePath(this object value, String filePath, Type type, bool indented, JsonSerializerSettings settings)
        {
            using (var sw = new FileInfo(filePath).CreateText())
            {
                sw.Write(value.ToJsonString(type, indented, settings));
            }
        }

        #endregion WriteJsonToFilePath

        #region WriteJsonToFile

        public static void WriteJsonToFile(this object value, FileInfo file)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString());
            }
        }

        public static void WriteJsonToFile(this object value, FileInfo file, bool indented)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString(indented));
            }
        }

        public static void WriteJsonToFile(this object value, FileInfo file, params JsonConverter[] converters)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString(converters));
            }
        }

        public static void WriteJsonToFile(this object value, FileInfo file, bool indented, params JsonConverter[] converters)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString(indented, converters));
            }
        }

        public static void WriteJsonToFile(this object value, FileInfo file, JsonSerializerSettings settings)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString(settings));
            }
        }

        public static void WriteJsonToFile(this object value, FileInfo file, Type type, JsonSerializerSettings settings)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString(type, settings));
            }
        }

        public static void WriteJsonToFile(this object value, FileInfo file, bool indented, JsonSerializerSettings settings)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString(indented, settings));
            }
        }

        public static void WriteJsonToFile(this object value, FileInfo file, Type type, bool indented, JsonSerializerSettings settings)
        {
            using (var sw = file.CreateText())
            {
                sw.Write(value.ToJsonString(type, indented, settings));
            }
        }

        #endregion WriteJsonToFile

        #endregion Json.net File Serialization

        #endregion Json.net Serialization

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
            var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
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
            var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
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
            var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
            using (var sr = file.OpenText())
            {
                using (var jr = new JsonTextReader(sr))
                {
                    return JsonSerializer.CreateDefault(settings).Deserialize<T>(jr);
                }
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

        #endregion Json.net JObject

        public static T ConvertExpandoTo<T>(this ExpandoObject source)
        {
            return JsonConvert.DeserializeObject<T>(source.ToJsonString(new ExpandoObjectConverter()));
        }

        #endregion Json.net Utilities


        #region BSON Serialization

        public static Byte[] ToBsonByteArray(this object value)
        {
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer,value);
                return ms.ToArray();
            }
        }

        #endregion

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

        #endregion
    }
}