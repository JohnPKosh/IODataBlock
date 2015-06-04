using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Business.Common.IO;
using Business.Common.IO.Serialization;

namespace Business.Common.Security
{
    /// <summary>
    /// Base Encryption and Decryption Utility Methods.
    /// </summary>
    public static class TripleDesEncryption
    {
        #region Helper Methods

        /// <summary>
        /// Makes the Base64 Encryption Initalization Vector.
        /// </summary>
        /// <returns>String Base64 Encryption Initalization Vector</returns>
        public static String MakeBase64Iv()
        {
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.GenerateIV();
            return Convert.ToBase64String(cryptoProvider.IV);
        }

        /// <summary>
        /// Makes the Base64 Encryption Key.
        /// </summary>
        /// <returns>String Base64 Encryption Key</returns>
        public static String MakeBase64Key()
        {
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.GenerateKey();
            return Convert.ToBase64String(cryptoProvider.Key);
        }

        /// <summary>
        /// Makes the Byte[] Encryption Initalization Vector.
        /// </summary>
        /// <returns>Byte[] Encryption Initalization Vector</returns>
        public static Byte[] MakeIv()
        {
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.GenerateIV();
            return cryptoProvider.IV;
        }

        /// <summary>
        /// Makes the Byte[] Encryption Key.
        /// </summary>
        /// <returns>Byte[] Encryption Key</returns>
        public static Byte[] MakeKey()
        {
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.GenerateKey();
            return cryptoProvider.Key;
        }

        #endregion Helper Methods

        #region String xEncryptBase64 and xDecryptBase64 Extensions

        /// <summary>
        /// Encrypts a String as Base64 Encoded String.
        /// </summary>
        /// <param name="encryptstr">The String to Encrypt.</param>
        /// <returns>Encrypted Base64 Encoded String</returns>
        public static String xEncryptBase64(this String encryptstr)
        {
            String returnstr;
            encryptstr = encryptstr ?? String.Empty;
            var key = MakeBase64Key();
            var iv = MakeBase64Iv();

            var byteArray = Encoding.Unicode.GetBytes(encryptstr);
            var cryptoTransform = new TripleDESCryptoServiceProvider().CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv));
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(byteArray, 0, byteArray.Length);
                    cryptoStream.FlushFinalBlock();
                }
                var encstr = String.Concat((MakeBase64Iv() + @"q4XjXd1=" + key + @"jt0dX4qI" + iv + @"q3jy4ixh" + Convert.ToBase64String(memoryStream.ToArray())).Reverse());
                returnstr = global::System.Web.HttpUtility.UrlEncode(encstr);
            }
            return returnstr;
        }

        /// <summary>
        /// Decrypts a String from Base64 Encoded String.
        /// </summary>
        /// <param name="decryptstr">The String to Decrypt.</param>
        /// <returns>Decrypted String</returns>
        public static String xDecryptBase64(this String decryptstr)
        {
            String returnstr;
            decryptstr = String.Concat(global::System.Web.HttpUtility.UrlDecode(decryptstr).Reverse());
            var entries = decryptstr.Split(new[] { @"q4XjXd1=", @"jt0dX4qI", @"q3jy4ixh" }, StringSplitOptions.None);

            var byteArray = Convert.FromBase64String(entries[3]);
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            var cryptoTransform = cryptoProvider.CreateDecryptor(Convert.FromBase64String(entries[1]), Convert.FromBase64String(entries[2]));
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(byteArray, 0, byteArray.Length);
                    cryptoStream.FlushFinalBlock();
                }
                returnstr = Encoding.Unicode.GetString(memoryStream.ToArray());
            }
            return returnstr;
        }

        #endregion String xEncryptBase64 and xDecryptBase64 Extensions

        #region Stream xEncrypt Extensions

        /// <summary>
        /// Encrypts a Stream and returns the Encrypted Stream.
        /// </summary>
        /// <param name="inputStream">The input Stream.</param>
        /// <returns>Encrypted Stream</returns>
        public static Stream xEncryptStreamToStream(this Stream inputStream)
        {
            var r = new TripleDESCryptoServiceProvider();

            var keyarr = MakeKey();  // 24
            var IVarr = MakeIv();  // 8

            var encryptstream = new MemoryStream();
            using (var cryptor = new CryptoStream(encryptstream, r.CreateEncryptor(keyarr, IVarr), CryptoStreamMode.Write))
            {
                inputStream.Seek(0, SeekOrigin.Begin);

                encryptstream.Write(MakeKey(), 0, 24);  // ignore on decrypt
                encryptstream.Write(keyarr, 0, 24);
                encryptstream.Write(IVarr, 0, 8);
                encryptstream.Write(MakeIv(), 0, 8);  // ignore on decrypt

                var buffersize = inputStream.Length > 4096 ? 4096 : (Int32)inputStream.Length;

                var bytes = new Byte[buffersize];

                while (true) // Loops Rule!!!!!!!!
                {
                    var bytecount = inputStream.Read(bytes, 0, bytes.Length);
                    if (bytecount > 0)
                    {
                        cryptor.Write(bytes, 0, bytecount);
                        cryptor.Flush();
                    }
                    else
                    {
                        break;
                    }
                }

                cryptor.FlushFinalBlock();
                encryptstream.Seek(0, SeekOrigin.Begin);
                return encryptstream;
            }
        }

        /// <summary>
        /// Encrypts an existing Stream in-place.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public static void xEncryptStream(this Stream inputStream)
        {
            var r = new TripleDESCryptoServiceProvider();

            var keyarr = MakeKey();  // 24
            var IVarr = MakeIv();  // 8

            using (var encryptstream = new MemoryStream())
            {
                using (var cryptor = new CryptoStream(encryptstream, r.CreateEncryptor(keyarr, IVarr), CryptoStreamMode.Write))
                {
                    inputStream.Seek(0, SeekOrigin.Begin);

                    encryptstream.Write(MakeKey(), 0, 24);  // ignore on decrypt
                    encryptstream.Write(keyarr, 0, 24);
                    encryptstream.Write(IVarr, 0, 8);
                    encryptstream.Write(MakeIv(), 0, 8);  // ignore on decrypt

                    var buffersize = inputStream.Length > 4096 ? 4096 : (Int32)inputStream.Length;
                    var bytes = new Byte[buffersize];

                    while (true) // Loops Rule!!!!!!!!
                    {
                        var bytecount = inputStream.Read(bytes, 0, bytes.Length);
                        if (bytecount > 0)
                        {
                            cryptor.Write(bytes, 0, bytecount);
                            cryptor.Flush();
                        }
                        else
                        {
                            break;
                        }
                    }

                    cryptor.FlushFinalBlock();
                    encryptstream.Seek(0, SeekOrigin.Begin);
                    inputStream.SetLength(0);
                    encryptstream.CopyTo(inputStream);
                }
                inputStream.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Encrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFilePath">The output file path.</param>
        public static void xEncryptStreamToFile(this Stream inputStream, String outputFilePath)
        {
            inputStream.xEncryptStreamToFile(new FileInfo(outputFilePath));
        }

        /// <summary>
        /// Encrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        public static void xEncryptStreamToFile(this Stream inputStream, FileInfo outputFileInfo)
        {
            var r = new TripleDESCryptoServiceProvider();
            var keyarr = MakeKey();  // 24
            var IVarr = MakeIv();  // 8

            using (var outputStream = outputFileInfo.OpenWrite())
            {
                using (var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(keyarr, IVarr), CryptoStreamMode.Write))
                {
                    inputStream.Seek(0, SeekOrigin.Begin);

                    outputStream.Write(MakeKey(), 0, 24);  // ignore on decrypt
                    outputStream.Write(keyarr, 0, 24);
                    outputStream.Write(IVarr, 0, 8);
                    outputStream.Write(MakeIv(), 0, 8);  // ignore on decrypt

                    using (new StreamReader(inputStream))
                    {
                        var buffersize = inputStream.Length > 4096 ? 4096 : (Int32)inputStream.Length;
                        var bytes = new Byte[buffersize];

                        while (true) // Loops Rule!!!!!!!!
                        {
                            var bytecount = inputStream.Read(bytes, 0, bytes.Length);
                            if (bytecount > 0)
                            {
                                cryptor.Write(bytes, 0, bytecount);
                                cryptor.Flush();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    cryptor.FlushFinalBlock();
                }
            }
        }

        #endregion Stream xEncrypt Extensions

        #region Stream xDecrypt Extensions

        /// <summary>
        /// Decrypts a Stream and returns the Decrypted Stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>Decrypted Stream</returns>
        public static Stream xDecryptStreamToStream(this Stream inputStream)
        {
            var r = new TripleDESCryptoServiceProvider();

            var keyarr = new Byte[24];  // 24
            var IVarr = new Byte[8];  // 8

            inputStream.Seek(24, SeekOrigin.Begin);
            inputStream.Read(keyarr, 0, 24);
            inputStream.Read(IVarr, 0, 8);
            inputStream.Seek(8, SeekOrigin.Current);

            var decryptstream = new MemoryStream();
            using (var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(keyarr, IVarr), CryptoStreamMode.Read))
            {
                var buffersize = inputStream.Length - 64 > 4096 ? 4096 : (Int32)inputStream.Length - 64;
                var bytes = new Byte[buffersize];

                while (true) // Loops Rule!!!!!!!!
                {
                    Int32 bytecount = cryptor.Read(bytes, 0, bytes.Length);
                    if (bytecount > 0)
                    {
                        decryptstream.Write(bytes, 0, bytecount);
                        decryptstream.Flush();
                    }
                    else
                    {
                        break;
                    }
                }
                decryptstream.Seek(0, SeekOrigin.Begin);
                return decryptstream;
            }
        }

        /// <summary>
        /// Decrypts an existing Stream in-place.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public static void xDecryptStream(this Stream inputStream)
        {
            var r = new TripleDESCryptoServiceProvider();
            var keyarr = new Byte[24];  // 24
            var IVarr = new Byte[8];  // 8

            inputStream.Seek(24, SeekOrigin.Begin);
            inputStream.Read(keyarr, 0, 24);
            inputStream.Read(IVarr, 0, 8);
            inputStream.Seek(8, SeekOrigin.Current);

            using (var decryptstream = new MemoryStream())
            {
                var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(keyarr, IVarr), CryptoStreamMode.Read);

                var buffersize = inputStream.Length - 64 > 4096 ? 4096 : (Int32)inputStream.Length - 64;
                var bytes = new Byte[buffersize];

                while (true) // Loops Rule!!!!!!!!
                {
                    var bytecount = cryptor.Read(bytes, 0, bytes.Length);
                    if (bytecount > 0)
                    {
                        decryptstream.Write(bytes, 0, bytecount);
                        decryptstream.Flush();
                    }
                    else
                    {
                        break;
                    }
                }
                decryptstream.Seek(0, SeekOrigin.Begin);
                inputStream.SetLength(0);
                decryptstream.CopyTo(inputStream);
                inputStream.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Decrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFilePath">The output file path.</param>
        public static void xDecryptStreamToFile(this Stream inputStream, String outputFilePath)
        {
            inputStream.xDecryptStreamToFile(new FileInfo(outputFilePath));
        }

        /// <summary>
        /// Decrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        public static void xDecryptStreamToFile(this Stream inputStream, FileInfo outputFileInfo)
        {
            var r = new TripleDESCryptoServiceProvider();
            var keyarr = new Byte[24];  // 24
            var IVarr = new Byte[8];  // 8

            inputStream.Seek(24, SeekOrigin.Begin);
            inputStream.Read(keyarr, 0, 24);
            inputStream.Read(IVarr, 0, 8);
            inputStream.Seek(8, SeekOrigin.Current);

            using (var decryptstream = outputFileInfo.OpenWrite())
            {
                var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(keyarr, IVarr), CryptoStreamMode.Read);

                var buffersize = inputStream.Length - 64 > 4096 ? 4096 : (Int32)inputStream.Length - 64;
                var bytes = new Byte[buffersize];

                while (true) // Loops Rule!!!!!!!!
                {
                    var bytecount = cryptor.Read(bytes, 0, bytes.Length);
                    if (bytecount > 0)
                    {
                        decryptstream.Write(bytes, 0, bytecount);
                        decryptstream.Flush();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        #endregion Stream xDecrypt Extensions

        #region xEncrypt FileInfo Extensions

        /// <summary>
        /// Encrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void xEncryptFileToFile(this FileInfo inputFileInfo, String outputFilePath, Int32 lockWaitMs = 60000)
        {
            inputFileInfo.xEncryptFileToFile(new FileInfo(outputFilePath), lockWaitMs);
        }

        /// <summary>
        /// Encrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFileInfo">The output file.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">
        /// @Can not open locked file! The file is locked by another process.
        /// </exception>
        public static void xEncryptFileToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, Int32 lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = inputFileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
                {
                    fs.xEncryptStreamToFile(outputFileInfo);
                }
            }
        }

        #endregion xEncrypt FileInfo Extensions

        #region xDecrypt FileInfo Extensions

        /// <summary>
        /// Decrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void xDecryptFileToFile(this FileInfo inputFileInfo, String outputFilePath, Int32 lockWaitMs = 60000)
        {
            inputFileInfo.xDecryptFileToFile(new FileInfo(outputFilePath), lockWaitMs);
        }

        /// <summary>
        /// Decrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFileInfo">The output file.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">
        /// @Can not open locked file! The file is locked by another process.
        /// </exception>
        public static void xDecryptFileToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, Int32 lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = inputFileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
                {
                    fs.xDecryptStreamToFile(outputFileInfo);
                }
            }
        }

        #endregion xDecrypt FileInfo Extensions

        #region Serialization Extensions

        #region Stream Serialization Extensions

        #region Stream xEncrypt Serialization Extensions

        /// <summary>
        /// Serializes and Encrypts an object to the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="obj">The object to serialize and write.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="encodingType">Encoding Enum</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        public static void xEncryptSerializeStream<T>(this Stream stream, T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            stream.Serialize(obj, settings, encodingType, knownTypes);
            stream.xEncryptStream();
        }

        #endregion Stream xEncrypt Serialization Extensions

        #region Stream xDecrypt Serialization Extensions

        /// <summary>
        /// Deserializes and Decrypts an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        /// <returns>T object</returns>
        public static T xDecryptDeserializeStream<T>(this Stream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            using (var ms = (MemoryStream)stream.xDecryptStreamToStream())
            {
                return ms.Deserialize<T>(settings, knownTypes);
            }
        }

        #endregion Stream xDecrypt Serialization Extensions

        #region Stream xEncryptGZip Serialization Extensions

        /// <summary>
        /// Serializes, compresses, and Encrypts an object to the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="obj">The object to serialize and write.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="encodingType">Encoding Enum</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        public static void xEncryptGZipSerializeStream<T>(this Stream stream, T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            stream.GZipSerialize(obj, settings, encodingType, knownTypes);
            stream.xEncryptStream();
        }

        #endregion Stream xEncryptGZip Serialization Extensions

        #region Stream xDecryptGZip Serialization Extensions

        /// <summary>
        /// Deserializes, uncompresses, and Decrypts an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        /// <returns>T object</returns>
        public static T xDecryptGZipDeserializeStream<T>(this Stream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            using (var ms = (MemoryStream)stream.xDecryptStreamToStream())
            {
                return ms.GZipDeserialize<T>(settings, knownTypes);
            }
        }

        #endregion Stream xDecryptGZip Serialization Extensions

        #endregion Stream Serialization Extensions

        #region File Serialization Extensions

        #region File xEncrypt Serialization Extensions

        /// <summary>
        /// Serializes and Encrypts an object to a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="rollbackType">Type of the rollback.</param>
        /// <param name="settings">The XmlWriterSettings.</param>
        /// <param name="encodingType">Type of the encoding.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        public static FileInfo xEncryptSerializeToFile<T>(this T obj, String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return obj.xEncryptSerializeToFile(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
        }

        /// <summary>
        /// Serializes and Encrypts an object to a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="rollbackType">Type of the rollback.</param>
        /// <param name="settings">The XmlWriterSettings.</param>
        /// <param name="encodingType">Type of the encoding.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static FileInfo xEncryptSerializeToFile<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
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
                        fileInfo = obj.xEncryptSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.InMemory:
                        fileInfo = obj.xEncryptSerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.FileCopy:
                        fileInfo = obj.xEncryptSerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    default:
                        fileInfo = obj.xEncryptSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
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

        #region Private FileInfo xEncrypt Serializer Methods

        private static FileInfo xEncryptSerializeNoRollback<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && fileInfo.Directory.Exists)
            {
                using (var fileAccess = new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (!fileAccess.IsAccessible) throw new Exception(@"Could not create lock file!");
                    using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
                    {
                        List<Type> types = null;
                        if (knownTypes != null) types = knownTypes.ToList();
                        using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
                        {
                            ms.xEncryptSerializeStream(obj, settings, encodingType, types);
                            ms.CopyTo(fs);
                        }
                    }
                }
                fileInfo.Refresh();
                return fileInfo;
            }
            throw new DirectoryNotFoundException();
        }

        private static FileInfo xEncryptSerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && fileInfo.Directory.Exists)
            {
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
                            List<Type> types = null;
                            if (knownTypes != null) types = knownTypes.ToList();
                            using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
                            {
                                ms.xEncryptSerializeStream(obj, settings, encodingType, types);
                                ms.CopyTo(fs);
                            }
                        }
                    }
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
            else
            {
                throw new DirectoryNotFoundException();
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        private static FileInfo xEncryptSerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            List<Type> types = null;
            if (knownTypes != null) types = knownTypes.ToList();
            using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
            {
                using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
                    {
                        ms.xEncryptSerializeStream(obj, settings, encodingType, types);
                        ms.CopyTo(fs);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        #endregion Private FileInfo xEncrypt Serializer Methods

        #endregion File xEncrypt Serialization Extensions

        #region File xDecrypt Deserialization Extensions

        /// <summary>
        /// Deserializes and Decrypts an object from a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="settings">The XmlReaderSettings.</param>
        /// <param name="encodingType">Type of the encoding.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">
        /// @Can not open locked file! The file is locked by another process.
        /// </exception>
        public static T xDecryptDeserializeFile<T>(this FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
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
                    return fs.xDecryptDeserializeStream<T>(settings, knownTypes);
                }
            }
        }

        #endregion File xDecrypt Deserialization Extensions

        #region File xEncryptGZip Serialization Extensions

        /// <summary>
        /// Serializes, compresses, and Encrypts an object to a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="rollbackType">Type of the rollback.</param>
        /// <param name="settings">The XmlWriterSettings.</param>
        /// <param name="encodingType">Type of the encoding.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        public static FileInfo xEncryptGZipSerializeToFile<T>(this T obj, String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return obj.xEncryptGZipSerializeToFile(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
        }

        /// <summary>
        /// Serializes, compresses, and Encrypts an object to a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="rollbackType">Type of the rollback.</param>
        /// <param name="settings">The XmlWriterSettings.</param>
        /// <param name="encodingType">Type of the encoding.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static FileInfo xEncryptGZipSerializeToFile<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
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
                        fileInfo = obj.xEncryptGZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.InMemory:
                        fileInfo = obj.xEncryptGZipSerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.FileCopy:
                        fileInfo = obj.xEncryptGZipSerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    default:
                        fileInfo = obj.xEncryptGZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
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

        #region Private FileInfo xEncryptGZip Serializer Methods

        private static FileInfo xEncryptGZipSerializeNoRollback<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
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
                    List<Type> types = null;
                    if (knownTypes != null) types = knownTypes.ToList();
                    using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
                    {
                        ms.xEncryptGZipSerializeStream(obj, settings, encodingType, types);
                        ms.CopyTo(fs);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        private static FileInfo xEncryptGZipSerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && fileInfo.Directory.Exists)
            {
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
                            List<Type> types = null;
                            if (knownTypes != null) types = knownTypes.ToList();
                            using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
                            {
                                ms.xEncryptGZipSerializeStream(obj, settings, encodingType, types);
                                ms.CopyTo(fs);
                            }
                        }
                    }
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
            else
            {
                throw new DirectoryNotFoundException();
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        private static FileInfo xEncryptGZipSerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            fileInfo.Refresh();
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            List<Type> types = null;
            if (knownTypes != null) types = knownTypes.ToList();
            using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
            {
                using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
                    {
                        ms.xEncryptGZipSerializeStream(obj, settings, encodingType, types);
                        ms.CopyTo(fs);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        #endregion Private FileInfo xEncryptGZip Serializer Methods

        #endregion File xEncryptGZip Serialization Extensions

        #region File xDecryptGZip Deserialization Extensions

        /// <summary>
        /// Deserializes, uncompresses, and Decrypts an object from a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="settings">The XmlReaderSettings.</param>
        /// <param name="encodingType">Type of the encoding.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">
        /// @Can not open locked file! The file is locked by another process.
        /// </exception>
        public static T xDecryptGZipDeserializeFile<T>(this FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
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
                    return fs.xDecryptGZipDeserializeStream<T>(settings, knownTypes);
                }
            }
        }

        #endregion File xDecryptGZip Deserialization Extensions

        #endregion File Serialization Extensions

        #endregion Serialization Extensions
    }
}