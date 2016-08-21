using Newtonsoft.Json;
using System;
using System.IO;

namespace Business.Common.Extensions
{
    public static class JsonObjectFileInfoSerialization
    {
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
    }
}