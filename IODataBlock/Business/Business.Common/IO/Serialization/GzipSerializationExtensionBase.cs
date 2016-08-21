using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Business.Common.IO.Serialization
{
    public static class GZipSerializationExtensionBase
    {
        #region Serialization Extensions

        #region Base Stream GZip Serialization Extensions

        /// <summary>
        /// Serializes an object to the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="obj">The object to serialize and write.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="encodingType"></param>
        /// <param name="knownTypes"></param>
        public static void GZipSerialize<T>(this Stream stream, T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings != null || encodingType != null)
            {
                if (settings == null)
                {
                    settings = new XmlWriterSettings { Encoding = encodingType, OmitXmlDeclaration = false };
                }
                else if (encodingType != null)
                {
                    settings.Encoding = encodingType;
                }

                if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
                var gs = new GZipStream(stream, CompressionMode.Compress, true);
                var sw = new StreamWriter(gs, settings.Encoding);
                using (var xw = XmlWriter.Create(sw, settings))
                {
                    dcs.WriteObject(xw, obj);
                    xw.Flush();
                    xw.Close();
                    sw.Close();
                }
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            }
            else
            {
                if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
                var gs = new GZipStream(stream, CompressionMode.Compress, true);
                var sw = new StreamWriter(gs);
                using (var xw = XmlWriter.Create(sw))
                {
                    dcs.WriteObject(xw, obj);
                    xw.Flush();
                    xw.Close();
                    sw.Close();
                }
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            }
        }

        #endregion Base Stream GZip Serialization Extensions

        #region Base Stream GZip Deserialization Extensions

        /// <summary>
        /// Deserializes an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current FileStream.</param>
        /// <param name="settings"></param>
        /// <param name="knownTypes"></param>
        /// <returns>T object</returns>
        public static T GZipDeserialize<T>(this FileStream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            return ((Stream)stream).GZipDeserialize<T>(settings, knownTypes);
        }

        /// <summary>
        /// Deserializes an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="settings"></param>
        /// <param name="knownTypes"></param>
        /// <returns>T object</returns>
        public static T GZipDeserialize<T>(this Stream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            T returnvalue;
            var dcs = knownTypes != null ? new DataContractSerializer(typeof(T), knownTypes) : new DataContractSerializer(typeof(T));
            if (settings == null) settings = new XmlReaderSettings();
            settings.CloseInput = false;  // leave the underlying stream still open.

            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            Stream gz = new GZipStream(stream, CompressionMode.Decompress, true);
            var sr = new StreamReader(gz);
            using (var xr = XmlReader.Create(sr, settings))
            {
                returnvalue = (T)dcs.ReadObject(xr);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            return returnvalue;
            // StreamReader and XmlReader on deserialization is used to avoid Encoding issues!!!!
        }

        #endregion Base Stream GZip Deserialization Extensions

        #region T Object to Stream GZipSerialization Extensions

        public static Stream GZipSerializeToStream<T>(this T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            Stream ms = new MemoryStream();
            ms.GZipSerialize(obj, settings, encodingType, knownTypes);
            return ms;
        }

        #endregion T Object to Stream GZipSerialization Extensions

        #region T Object to MemoryStream GZipSerialization Extensions

        public static MemoryStream GZipSerializeToMemoryStream<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            Stream ms = new MemoryStream();
            ms.GZipSerialize(obj, settings, encodingType, knownTypes);
            return (MemoryStream)ms;
        }

        #endregion T Object to MemoryStream GZipSerialization Extensions

        #region T Object to Byte[] GZipSerialization Extensions

        public static byte[] GZipSerializeToBytes<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            var ms = new MemoryStream();
            ms.GZipSerialize(obj, settings, encodingType, knownTypes);
            return ms.ToArray();
        }

        #endregion T Object to Byte[] GZipSerialization Extensions

        #region Byte[] to T Object GZipDeserialization Extensions

        public static T GZipDeserializeBytes<T>(this byte[] data,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            using (var ms = new MemoryStream(data))
            {
                return ms.GZipDeserialize<T>(settings, knownTypes);
            }
        }

        #endregion Byte[] to T Object GZipDeserialization Extensions

        #region Base64String GZipSerialization Extensions

        public static string GZipSerializeToBase64String<T>(this T obj,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return Convert.ToBase64String(obj.GZipSerializeToBytes(settings, encodingType, knownTypes));
        }

        #endregion Base64String GZipSerialization Extensions

        #region Base64String GZipDeserialization Extensions

        public static T GZipDeserializeFromBase64String<T>(this string value,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return Convert.FromBase64String(value).GZipDeserializeBytes<T>(settings, encodingType, knownTypes);
        }

        #endregion Base64String GZipDeserialization Extensions

        #region File GZip Serialization Extensions

        public static FileInfo GZipSerialize<T>(this T obj, string filePath,
        int lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return obj.GZipSerialize(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
        }

        public static FileInfo GZipSerialize<T>(this T obj, FileInfo fileInfo,
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
                        fileInfo = obj.GZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.InMemory:
                        fileInfo = obj.GZipSerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.FileCopy:
                        fileInfo = obj.GZipSerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    default:
                        fileInfo = obj.GZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
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

        #region Private FileInfo GZip Serializer Methods

        private static FileInfo GZipSerializeNoRollback<T>(this T obj, FileInfo fileInfo,
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
                    fs.GZipSerialize(obj, settings, encodingType, knownTypes);
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        private static FileInfo GZipSerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (fileInfo.DirectoryName == null) return null;
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
                        fs.GZipSerialize(obj, settings, encodingType, knownTypes);
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

        private static FileInfo GZipSerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
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
                        fs.GZipSerialize(obj, settings, encodingType, enumerable);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        #endregion Private FileInfo GZip Serializer Methods

        #endregion File GZip Serialization Extensions

        #region File GZip Deserialization Extensions

        public static T GZipDeserialize<T>(this string filePath,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return GZipDeserialize<T>(new FileInfo(filePath), lockWaitMs, settings, encodingType, knownTypes);
        }

        public static T GZipDeserialize<T>(this FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = fileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
                {
                    return fs.GZipDeserialize<T>(settings, knownTypes);
                }
            }
        }

        public static T GZipDeserialize<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return fileInfo.GZipDeserialize<T>(lockWaitMs, settings, encodingType, knownTypes);
        }

        public static T GZipDeserialize<T>(this T obj, string filePath,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return new FileInfo(filePath).GZipDeserialize<T>(lockWaitMs, settings, encodingType, knownTypes);
        }

        #endregion File GZip Deserialization Extensions

        #endregion Serialization Extensions
    }
}