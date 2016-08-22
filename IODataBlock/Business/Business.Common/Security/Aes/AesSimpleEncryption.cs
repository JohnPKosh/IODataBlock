using Business.Common.IO;
using Business.Common.IO.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Business.Common.Security.Aes
{
    public static class AesSimpleEncryption
    {
        #region Helper Methods

        /// <summary>
        /// Makes the Base64 Encryption Initalization Vector.
        /// </summary>
        /// <returns>String Base64 Encryption Initalization Vector</returns>
        public static string MakeAesBase64Iv()
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.GenerateIV();
            return Convert.ToBase64String(cryptoProvider.IV);
        }

        /// <summary>
        /// Makes the Base64 Encryption Key.
        /// </summary>
        /// <returns>String Base64 Encryption Key</returns>
        public static string MakeAesBase64Key()
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.GenerateKey();
            return Convert.ToBase64String(cryptoProvider.Key);
        }

        /// <summary>
        /// Makes the Byte[] Encryption Initalization Vector.
        /// </summary>
        /// <returns>Byte[] Encryption Initalization Vector</returns>
        public static byte[] MakeAesIv()
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.GenerateIV();
            return cryptoProvider.IV;
        }

        /// <summary>
        /// Makes the Byte[] Encryption Key.
        /// </summary>
        /// <returns>Byte[] Encryption Key</returns>
        public static byte[] MakeAesKey()
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.GenerateKey();
            return cryptoProvider.Key;
        }

        #endregion Helper Methods

        #region String AesEncryptBase64 and AesDecryptBase64 Extensions

        /// <summary>
        /// Encrypts a String as Base64 Encoded String.
        /// </summary>
        /// <param name="encryptstr">The String to Encrypt.</param>
        /// <returns>Encrypted Base64 Encoded String</returns>
        public static string AesSimpleEncryptBase64(this string encryptstr)
        {
            string returnstr;
            encryptstr = encryptstr ?? string.Empty;
            var key = MakeAesBase64Key();
            var iv = MakeAesBase64Iv();

            var byteArray = Encoding.Unicode.GetBytes(encryptstr);
            var cryptoTransform = new AesCryptoServiceProvider().CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv));
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(byteArray, 0, byteArray.Length);
                    cryptoStream.FlushFinalBlock();
                }
                var encstr = string.Concat((MakeAesBase64Iv() + @"q4XjXd1=" + key + @"jt0dX4qI" + iv + @"q3jy4ixh" + Convert.ToBase64String(memoryStream.ToArray())).Reverse());
                returnstr = global::System.Web.HttpUtility.UrlEncode(encstr);
            }
            return returnstr;
        }

        /// <summary>
        /// Decrypts a String from Base64 Encoded String.
        /// </summary>
        /// <param name="decryptstr">The String to Decrypt.</param>
        /// <returns>Decrypted String</returns>
        public static string AesSimpleDecryptBase64(this string decryptstr)
        {
            string returnstr;
            decryptstr = string.Concat(global::System.Web.HttpUtility.UrlDecode(decryptstr).Reverse());
            var entries = decryptstr.Split(new[] { @"q4XjXd1=", @"jt0dX4qI", @"q3jy4ixh" }, StringSplitOptions.None);

            var byteArray = Convert.FromBase64String(entries[3]);
            var cryptoProvider = new AesCryptoServiceProvider();
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

        #endregion String AesEncryptBase64 and AesDecryptBase64 Extensions

        #region Stream AesEncrypt Extensions

        /// <summary>
        /// Encrypts a Stream and returns the Encrypted Stream.
        /// </summary>
        /// <param name="inputStream">The input Stream.</param>
        /// <returns>Encrypted Stream</returns>
        public static Stream AesSimpleEncryptToStream(this Stream inputStream)
        {
            var r = new AesCryptoServiceProvider();

            var keyarr = MakeAesKey();  // 32
            var IVarr = MakeAesIv();  // 16

            var encryptstream = new MemoryStream();
            using (var cryptor = new CryptoStream(encryptstream, r.CreateEncryptor(keyarr, IVarr), CryptoStreamMode.Write))
            {
                inputStream.Seek(0, SeekOrigin.Begin);

                encryptstream.Write(MakeAesKey(), 0, 32);  // ignore on decrypt
                encryptstream.Write(keyarr, 0, 32);
                encryptstream.Write(IVarr, 0, 16);
                encryptstream.Write(MakeAesIv(), 0, 16);  // ignore on decrypt

                var buffersize = inputStream.Length > 4096 ? 4096 : (int)inputStream.Length;

                var bytes = new byte[buffersize];

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
        public static void AesSimpleEncryptStream(this Stream inputStream)
        {
            var r = new AesCryptoServiceProvider();

            var keyarr = MakeAesKey();  // 32
            var IVarr = MakeAesIv();  // 16

            using (var encryptstream = new MemoryStream())
            {
                using (var cryptor = new CryptoStream(encryptstream, r.CreateEncryptor(keyarr, IVarr), CryptoStreamMode.Write))
                {
                    inputStream.Seek(0, SeekOrigin.Begin);

                    encryptstream.Write(MakeAesKey(), 0, 32);  // ignore on decrypt
                    encryptstream.Write(keyarr, 0, 32);
                    encryptstream.Write(IVarr, 0, 16);
                    encryptstream.Write(MakeAesIv(), 0, 16);  // ignore on decrypt

                    var buffersize = inputStream.Length > 4096 ? 4096 : (int)inputStream.Length;
                    var bytes = new byte[buffersize];

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
        public static void AesSimpleEncryptToFile(this Stream inputStream, string outputFilePath)
        {
            inputStream.AesSimpleEncryptToFile(new FileInfo(outputFilePath));
        }

        /// <summary>
        /// Encrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        public static void AesSimpleEncryptToFile(this Stream inputStream, FileInfo outputFileInfo)
        {
            var r = new AesCryptoServiceProvider();
            var keyarr = MakeAesKey();  // 32
            var IVarr = MakeAesIv();  // 16

            using (var outputStream = outputFileInfo.OpenWrite())
            {
                using (var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(keyarr, IVarr), CryptoStreamMode.Write))
                {
                    inputStream.Seek(0, SeekOrigin.Begin);

                    outputStream.Write(MakeAesKey(), 0, 32);  // ignore on decrypt
                    outputStream.Write(keyarr, 0, 32);
                    outputStream.Write(IVarr, 0, 16);
                    outputStream.Write(MakeAesIv(), 0, 16);  // ignore on decrypt

                    using (new StreamReader(inputStream))
                    {
                        var buffersize = inputStream.Length > 4096 ? 4096 : (int)inputStream.Length;
                        var bytes = new byte[buffersize];

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

        #endregion Stream AesEncrypt Extensions

        #region Stream AesDecrypt Extensions

        /// <summary>
        /// Decrypts a Stream and returns the Decrypted Stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>Decrypted Stream</returns>
        public static Stream AesSimpleDecryptToStream(this Stream inputStream)
        {
            var r = new AesCryptoServiceProvider();

            var keyarr = new byte[32];  // 32
            var IVarr = new byte[16];  // 16

            inputStream.Seek(32, SeekOrigin.Begin);
            inputStream.Read(keyarr, 0, 32);
            inputStream.Read(IVarr, 0, 16);
            inputStream.Seek(16, SeekOrigin.Current);

            var decryptstream = new MemoryStream();
            using (var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(keyarr, IVarr), CryptoStreamMode.Read))
            {
                var buffersize = inputStream.Length - 96 > 4096 ? 4096 : (int)inputStream.Length - 96;
                var bytes = new byte[buffersize];

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
                return decryptstream;
            }
        }

        /// <summary>
        /// Decrypts an existing Stream in-place.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public static void AesSimpleDecryptStream(this Stream inputStream)
        {
            var r = new AesCryptoServiceProvider();
            var keyarr = new byte[32];  // 32
            var IVarr = new byte[16];  // 16

            inputStream.Seek(32, SeekOrigin.Begin);
            inputStream.Read(keyarr, 0, 32);
            inputStream.Read(IVarr, 0, 16);
            inputStream.Seek(16, SeekOrigin.Current);

            using (var decryptstream = new MemoryStream())
            {
                var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(keyarr, IVarr), CryptoStreamMode.Read);

                var buffersize = inputStream.Length - 96 > 4096 ? 4096 : (int)inputStream.Length - 96;
                var bytes = new byte[buffersize];

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
        public static void AesSimpleDecryptToFile(this Stream inputStream, string outputFilePath)
        {
            inputStream.AesSimpleDecryptToFile(new FileInfo(outputFilePath));
        }

        /// <summary>
        /// Decrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        public static void AesSimpleDecryptToFile(this Stream inputStream, FileInfo outputFileInfo)
        {
            var r = new AesCryptoServiceProvider();
            var keyarr = new byte[32];  // 32
            var IVarr = new byte[16];  // 16

            inputStream.Seek(32, SeekOrigin.Begin);
            inputStream.Read(keyarr, 0, 32);
            inputStream.Read(IVarr, 0, 16);
            inputStream.Seek(16, SeekOrigin.Current);

            using (var decryptstream = outputFileInfo.OpenWrite())
            {
                var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(keyarr, IVarr), CryptoStreamMode.Read);

                var buffersize = inputStream.Length - 96 > 4096 ? 4096 : (int)inputStream.Length - 96;
                var bytes = new byte[buffersize];

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

        #endregion Stream AesDecrypt Extensions

        #region AesEncrypt FileInfo Extensions

        /// <summary>
        /// Encrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesSimpleEncryptToFile(this FileInfo inputFileInfo, string outputFilePath, int lockWaitMs = 60000)
        {
            inputFileInfo.AesSimpleEncryptToFile(new FileInfo(outputFilePath), lockWaitMs);
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
        public static void AesSimpleEncryptToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, int lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = inputFileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
                {
                    fs.AesSimpleEncryptToFile(outputFileInfo);
                }
            }
        }

        #endregion AesEncrypt FileInfo Extensions

        #region AesDecrypt FileInfo Extensions

        /// <summary>
        /// Decrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesSimpleDecryptToFile(this FileInfo inputFileInfo, string outputFilePath, int lockWaitMs = 60000)
        {
            inputFileInfo.AesSimpleDecryptToFile(new FileInfo(outputFilePath), lockWaitMs);
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
        public static void AesSimpleDecryptToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, int lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = inputFileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
                {
                    fs.AesSimpleDecryptToFile(outputFileInfo);
                }
            }
        }

        #endregion AesDecrypt FileInfo Extensions

        #region Serialization Extensions

        #region Stream Serialization Extensions

        #region Stream AesEncrypt Serialization Extensions

        /// <summary>
        /// Serializes and Encrypts an object to the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="obj">The object to serialize and write.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="encodingType">Encoding Enum</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        public static void AesSimpleEncryptSerializeStream<T>(this Stream stream, T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            stream.Serialize(obj, settings, encodingType, knownTypes);
            stream.AesSimpleEncryptStream();
        }

        #endregion Stream AesEncrypt Serialization Extensions

        #region Stream AesDecrypt Serialization Extensions

        /// <summary>
        /// Deserializes and Decrypts an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        /// <returns>T object</returns>
        public static T AesSimpleDecryptDeserializeStream<T>(this Stream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            using (var ms = (MemoryStream)stream.AesSimpleDecryptToStream())
            {
                return ms.Deserialize<T>(settings, knownTypes);
            }
        }

        #endregion Stream AesDecrypt Serialization Extensions

        #region Stream AesEncryptGZip Serialization Extensions

        /// <summary>
        /// Serializes, compresses, and Encrypts an object to the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="obj">The object to serialize and write.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="encodingType">Encoding Enum</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        public static void AesSimpleEncryptGZipSerializeStream<T>(this Stream stream, T obj,
            XmlWriterSettings settings = null,
            Encoding encodingType = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            stream.GZipSerialize(obj, settings, encodingType, knownTypes);
            stream.AesSimpleEncryptStream();
        }

        #endregion Stream AesEncryptGZip Serialization Extensions

        #region Stream AesDecryptGZip Serialization Extensions

        /// <summary>
        /// Deserializes, uncompresses, and Decrypts an object from the current Stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="stream">The current Stream.</param>
        /// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        /// <param name="knownTypes">Known Types for Serialization.</param>
        /// <returns>T object</returns>
        public static T AesSimpleDecryptGZipDeserializeStream<T>(this Stream stream,
            XmlReaderSettings settings = null,
            IEnumerable<Type> knownTypes = null) where T : class
        {
            using (var ms = (MemoryStream)stream.AesSimpleDecryptToStream())
            {
                return ms.GZipDeserialize<T>(settings, knownTypes);
            }
        }

        #endregion Stream AesDecryptGZip Serialization Extensions

        #endregion Stream Serialization Extensions

        #region File Serialization Extensions

        #region File AesEncrypt Serialization Extensions

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
        public static FileInfo AesSimpleEncryptSerializeToFile<T>(this T obj, string filePath,
        int lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return obj.AesSimpleEncryptSerializeToFile(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
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
        public static FileInfo AesSimpleEncryptSerializeToFile<T>(this T obj, FileInfo fileInfo,
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
                        fileInfo = obj.AesSimpleEncryptSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.InMemory:
                        fileInfo = obj.AesSimpleEncryptSerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.FileCopy:
                        fileInfo = obj.AesSimpleEncryptSerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    default:
                        fileInfo = obj.AesSimpleEncryptSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
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

        #region Private FileInfo AesEncrypt Serializer Methods

        private static FileInfo AesSimpleEncryptSerializeNoRollback<T>(this T obj, FileInfo fileInfo,
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
                    List<Type> types = null;
                    if (knownTypes != null) types = knownTypes.ToList();
                    using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
                    {
                        ms.AesSimpleEncryptSerializeStream(obj, settings, encodingType, types);
                        ms.CopyTo(fs);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        private static FileInfo AesSimpleEncryptSerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
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
                                ms.AesSimpleEncryptSerializeStream(obj, settings, encodingType, types);
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

        private static FileInfo AesSimpleEncryptSerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
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
                        ms.AesSimpleEncryptSerializeStream(obj, settings, encodingType, types);
                        ms.CopyTo(fs);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        #endregion Private FileInfo AesEncrypt Serializer Methods

        #endregion File AesEncrypt Serialization Extensions

        #region File AesDecrypt Deserialization Extensions

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
        public static T AesSimpleDecryptDeserializeFile<T>(this FileInfo fileInfo,
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
                    return fs.AesSimpleDecryptDeserializeStream<T>(settings, knownTypes);
                }
            }
        }

        #endregion File AesDecrypt Deserialization Extensions

        #region File AesEncryptGZip Serialization Extensions

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
        public static FileInfo AesSimpleEncryptGZipSerializeToFile<T>(this T obj, string filePath,
        int lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : class
        {
            return obj.AesSimpleEncryptGZipSerializeToFile(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
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
        public static FileInfo AesSimpleEncryptGZipSerializeToFile<T>(this T obj, FileInfo fileInfo,
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
                        fileInfo = obj.AesSimpleEncryptGZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.InMemory:
                        fileInfo = obj.AesSimpleEncryptGZipSerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    case IoRollbackType.FileCopy:
                        fileInfo = obj.AesSimpleEncryptGZipSerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
                        break;

                    default:
                        fileInfo = obj.AesSimpleEncryptGZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
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

        #region Private FileInfo AesEncryptGZip Serializer Methods

        private static FileInfo AesSimpleEncryptGZipSerializeNoRollback<T>(this T obj, FileInfo fileInfo,
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
                    List<Type> types = null;
                    if (knownTypes != null) types = knownTypes.ToList();
                    using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
                    {
                        ms.AesSimpleEncryptGZipSerializeStream(obj, settings, encodingType, types);
                        ms.CopyTo(fs);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        private static FileInfo AesSimpleEncryptGZipSerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
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
                                ms.AesSimpleEncryptGZipSerializeStream(obj, settings, encodingType, types);
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

        private static FileInfo AesSimpleEncryptGZipSerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
        int lockWaitMs = 60000,
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
                        ms.AesSimpleEncryptGZipSerializeStream(obj, settings, encodingType, types);
                        ms.CopyTo(fs);
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        #endregion Private FileInfo AesEncryptGZip Serializer Methods

        #endregion File AesEncryptGZip Serialization Extensions

        #region File AesDecryptGZip Deserialization Extensions

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
        public static T AesSimpleDecryptGZipDeserializeFile<T>(this FileInfo fileInfo,
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
                    return fs.AesSimpleDecryptGZipDeserializeStream<T>(settings, knownTypes);
                }
            }
        }

        #endregion File AesDecryptGZip Deserialization Extensions

        #endregion File Serialization Extensions

        #endregion Serialization Extensions
    }
}