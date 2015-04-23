using System;
using System.IO;
using Newtonsoft.Json;

namespace Business.Common.Extensions
{
    public static class JsonObjectFilePathSerialization
    {
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
    }
}