using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Business.Common.IO.Serialization
{
    public static class SerializationExtensionBase
    {
        public static string TrimXmlBom(this string value, bool trimWhitespace = true)
        {
            if (trimWhitespace) value = value.Trim();
            var index = value.IndexOf('<');
            return index > 0 ? value.Substring(index, value.Length - index) : value;
        }

        #region Serialization Extensions

        #region Base Stream Serialization Extensions

        /* Stream Extensions should not use using statements for StreamWriter/StreamReader/GZipStream/etc. because when disposing they close the underlying stream. */

        /// <summary>
        /// Serializes an object to the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="obj">The object to serialize and write.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="encodingType"></param>
        /// <param name="knownTypes"></param>
        public static void Serialize<T>(this FileStream stream, T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            ((Stream)stream).Serialize(obj, settings, encodingType, knownTypes);
        }

        /// <summary>
        /// Serializes an object to the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="obj">The object to serialize and write.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="encodingType"></param>
        /// <param name="knownTypes"></param>
        public static void Serialize<T>(this Stream stream, T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings != null || encodingType != null)
            {
                if (settings == null)
                {
                    settings = new XmlWriterSettings {Encoding = encodingType, OmitXmlDeclaration = false};
                }
                else if (encodingType != null)
                {
                    settings.Encoding = encodingType;
                }
                if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
                var sw = new StreamWriter(stream, settings.Encoding);
                using (var xw = XmlWriter.Create(sw, settings))
                {
                    dcs.WriteObject(xw, obj);
                    xw.Flush();
                    xw.Close();
                }
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            }
            else
            {
                if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
                dcs.WriteObject(stream, obj);
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            }
        }

        #endregion Base Stream Serialization Extensions

        #region Base Stream Deserialization Extensions

        /// <summary>
        /// Deserializes an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current FileStream.</param>
        /// <param name="settings"></param>
        /// <param name="knownTypes"></param>
        /// <returns>T object</returns>
        public static T Deserialize<T>(this FileStream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            return ((Stream)stream).Deserialize<T>(settings, knownTypes);
        }

        /// <summary>
        /// Deserializes an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="settings">XmlReaderSettings to be used in deserialization.</param>
        /// <param name="knownTypes"></param>
        /// <returns>T object</returns>
        public static T Deserialize<T>(this Stream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            T returnvalue;
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings == null) settings = new XmlReaderSettings();
            settings.CloseInput = false;  // leave the underlying stream still open.

            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            var sr = new StreamReader(stream);
            using (var xr = XmlReader.Create(sr, settings))
            {
                returnvalue = (T)dcs.ReadObject(xr);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            return returnvalue;
            // StreamReader and XmlReader on deserialization is used to avoid Encoding issues!!!!
        }

        /// <summary>
        /// Deserializes an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="settings">XmlReaderSettings to be used in deserialization.</param>
        /// <param name="knownTypes"></param>
        /// <returns>T object</returns>
        public static T Deserialize<T>(this MemoryStream stream,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return ((Stream)stream).Deserialize<T>(settings, knownTypes);
        }

        #endregion Base Stream Deserialization Extensions

        #region T Object to Stream Serialization Extensions

        public static Stream SerializeToStream<T>(this T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            Stream ms = new MemoryStream();
            ms.Serialize(obj, settings, encodingType, knownTypes);
            return ms;
        }

        #endregion T Object to Stream Serialization Extensions

        #region T Object to MemoryStream Serialization Extensions

        public static MemoryStream SerializeToMemoryStream<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            Stream ms = new MemoryStream();
            ms.Serialize(obj, settings, encodingType, knownTypes);
            return ms as MemoryStream;
        }

        #endregion T Object to MemoryStream Serialization Extensions

        #region T Object to Byte[] Serialization Extensions

        public static byte[] SerializeToBytes<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            var ms = new MemoryStream();
            ms.Serialize(obj, settings, encodingType, knownTypes);
            return ms.ToArray();
        }

        #endregion T Object to Byte[] Serialization Extensions

        #region Byte[] to T Object Deserialization Extensions

        public static T DeserializeBytes<T>(this byte[] data,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            using (var ms = new MemoryStream(data))
            {
                return ms.Deserialize<T>(settings, knownTypes);
            }
        }

        #endregion Byte[] to T Object Deserialization Extensions

        #region String Serialization Extensions

        public static string SerializeToString<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings != null || encodingType != null)
            {
                if (settings == null)
                {
                    settings = new XmlWriterSettings {Encoding = encodingType, OmitXmlDeclaration = false};
                }
                else if (encodingType != null)
                {
                    settings.Encoding = encodingType;
                }
                using (var stream = new MemoryStream())
                {
                    var sw = new StreamWriter(stream, settings.Encoding);
                    using (var xw = XmlWriter.Create(sw, settings))
                    {
                        dcs.WriteObject(xw, obj);
                        xw.Flush();
                        xw.Close();
                    }
                    //stream.Flush();
                    var value = settings.Encoding.GetString(stream.ToArray());
                    return value.TrimXmlBom();  // FYI return string will include byte order marks use Trim() to remove
                }
            }
            using (var stream = new MemoryStream())
            {
                dcs.WriteObject(stream, obj);
                stream.Flush();
                var value = Encoding.Default.GetString(stream.ToArray());
                return value.TrimXmlBom();
            }
        }

        #endregion String Serialization Extensions

        #region String Deserialization Extensions

        public static T DeserializeString<T>(this string value,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            T returnvalue;
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings == null) settings = new XmlReaderSettings();
            settings.CloseInput = false;  // leave the underlying stream still open.
            using (var sr = new StringReader(value.TrimXmlBom())) // byte order marks and extra chars will be removed otherwise possible errors
            {
                using (var xr = XmlReader.Create(sr, settings))
                {
                    returnvalue = (T)dcs.ReadObject(xr);
                }
            }
            return returnvalue;
        }

        #endregion String Deserialization Extensions

        #region Base64String Serialization Extensions

        public static string SerializeToBase64String<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return Convert.ToBase64String(obj.SerializeToBytes(settings, encodingType, knownTypes));
        }

        #endregion Base64String Serialization Extensions

        #region Base64String Deserialization Extensions

        public static T DeserializeBase64String<T>(this string value,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return Convert.FromBase64String(value).DeserializeBytes<T>(settings, encodingType, knownTypes);
        }

        #endregion Base64String Deserialization Extensions

        #region XElement Serialization Extensions

        public static XElement SerializeToXElement<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings != null || encodingType != null)
            {
                if (settings == null)
                {
                    settings = new XmlWriterSettings {Encoding = encodingType, OmitXmlDeclaration = false};
                }
                else if (encodingType != null)
                {
                    settings.Encoding = encodingType;
                }
                using (var stream = new MemoryStream())
                {
                    var sw = new StreamWriter(stream, settings.Encoding);
                    using (var xw = XmlWriter.Create(sw, settings))
                    {
                        dcs.WriteObject(xw, obj);
                        xw.Flush();
                        xw.Close();
                    }
                    stream.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var xr = XmlReader.Create(stream))
                    {
                        return XElement.Load(xr);
                    }
                }
            }
            using (var stream = new MemoryStream())
            {
                dcs.WriteObject(stream, obj);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                using (var xr = XmlReader.Create(stream))
                {
                    return XElement.Load(xr);
                }
            }
        }

        #endregion XElement Serialization Extensions

        #region XElement Deserialization Extensions

        public static T Deserialize<T>(this XElement value,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            T returnvalue;
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings == null) settings = new XmlReaderSettings();
            settings.CloseInput = false;  // leave the underlying stream still open.
            using (var ms = new MemoryStream())
            {
                using (var sw = XmlWriter.Create(ms))
                {
                    value.WriteTo(sw);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    using (var xr = XmlReader.Create(ms, settings))
                    {
                        returnvalue = (T)dcs.ReadObject(xr);
                    }
                }
            }
            return returnvalue;
        }

        #endregion XElement Deserialization Extensions

        #region File Serialization Extensions

        public static FileInfo Serialize<T>(this T obj, string filePath,
            int lockWaitMs = 60000,
            IoRollbackType rollbackType = IoRollbackType.None,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            return obj.Serialize(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
        }

        public static FileInfo Serialize<T>(this T obj, FileInfo fileInfo,
            int lockWaitMs = 60000,
            IoRollbackType rollbackType = IoRollbackType.None,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && fileInfo.Directory.Exists)
            {
                switch (rollbackType)
                {
                    case IoRollbackType.None:
                        fileInfo = obj.SerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.InMemory:
                        fileInfo = obj.SerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.FileCopy:
                        fileInfo = obj.SerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    default:
                        fileInfo = obj.SerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;
                }
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        #region Private FileInfo Serializer Methods

        private static FileInfo SerializeNoRollback<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Could not create lock file!");
                using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
                {
                    fs.Serialize(obj, settings, encodingType, knownTypes);
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        private static FileInfo SerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
// ReSharper disable once AssignNullToNotNullAttribute
            var tempfileinfo = new FileInfo(Path.Combine(fileInfo.DirectoryName, Path.GetRandomFileName()));
            try
            {
                using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (fileInfo.Exists)
                    {
                        fileInfo.CopyTo(tempfileinfo.FullName, true, lockWaitMs);
                        tempfileinfo.Refresh();
                    }
                    using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
                    {
                        fs.Serialize(obj, settings, encodingType, knownTypes);
                    }
                }
                fileInfo.Refresh();
                return fileInfo;
            }
            catch (Exception)
            {
                fileInfo.Refresh();
                if (fileInfo.Exists) fileInfo.Delete();
                if (tempfileinfo.Exists) tempfileinfo.MoveTo(fileInfo.FullName);
                throw;
            }
            finally
            {
                if (tempfileinfo.Exists) tempfileinfo.Delete();
            }
        }

        private static FileInfo SerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            var enumerable = knownTypes as Type[];
            using (obj.SerializeToMemoryStream(settings, encodingType, enumerable))
            {
                using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
                    {
                        fs.Serialize(obj, settings, encodingType, enumerable);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        #endregion Private FileInfo Serializer Methods

        #endregion File Serialization Extensions

        #region File Deserialization Extensions

        public static T Deserialize<T>(this string filePath,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return Deserialize<T>(new FileInfo(filePath), lockWaitMs, settings, knownTypes);
        }

        public static T Deserialize<T>(this FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = fileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
                {
                    return fs.Deserialize<T>(settings, knownTypes);
                }
            }
        }

        public static T Deserialize<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return fileInfo.Deserialize<T>(lockWaitMs, settings, knownTypes);
        }

        public static T Deserialize<T>(this T obj, string filePath,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return new FileInfo(filePath).Deserialize<T>(lockWaitMs, settings, knownTypes);
        }

        #endregion File Deserialization Extensions

        #endregion Serialization Extensions
    }
}