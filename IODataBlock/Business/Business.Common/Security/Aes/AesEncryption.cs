using Business.Common.Extensions;
using Business.Common.IO;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Business.Common.Security.Aes
{
    public static class AesEncryption
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

        ///// <summary>
        ///// Encrypts a String as Base64 Encoded String.
        ///// </summary>
        ///// <param name="encryptstr">The String to Encrypt.</param>
        ///// <returns>Encrypted Base64 Encoded String</returns>
        //public static String AesEncryptBase64(this String encryptstr)
        //{
        //    String returnstr;
        //    encryptstr = encryptstr ?? String.Empty;
        //    var key = MakeAesBase64Key();
        //    var iv = MakeAesBase64Iv();

        //    var byteArray = Encoding.Unicode.GetBytes(encryptstr);
        //    var cryptoTransform = new AesCryptoServiceProvider().CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv));
        //    //r.Padding = PaddingMode.None;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
        //        {
        //            cryptoStream.Write(byteArray, 0, byteArray.Length);
        //            cryptoStream.FlushFinalBlock();
        //        }
        //        var encstr = String.Concat((MakeAesBase64Iv() + @"q4XjXd1=" + key + @"jt0dX4qI" + iv + @"q3jy4ixh" + Convert.ToBase64String(memoryStream.ToArray())).Reverse());
        //        returnstr = global::System.Web.HttpUtility.UrlEncode(encstr);
        //    }
        //    return returnstr;
        //}

        ///// <summary>
        ///// Decrypts a String from Base64 Encoded String.
        ///// </summary>
        ///// <param name="decryptstr">The String to Decrypt.</param>
        ///// <returns>Decrypted String</returns>
        //public static String AesDecryptBase64(this String decryptstr)
        //{
        //    String returnstr;
        //    decryptstr = String.Concat(global::System.Web.HttpUtility.UrlDecode(decryptstr).Reverse());
        //    var entries = decryptstr.Split(new[] { @"q4XjXd1=", @"jt0dX4qI", @"q3jy4ixh" }, StringSplitOptions.None);

        //    var byteArray = Convert.FromBase64String(entries[3]);
        //    var cryptoProvider = new AesCryptoServiceProvider();
        //    cryptoProvider.Padding = PaddingMode.None;
        //    var cryptoTransform = cryptoProvider.CreateDecryptor(Convert.FromBase64String(entries[1]), Convert.FromBase64String(entries[2]));
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
        //        {
        //            cryptoStream.Write(byteArray, 0, byteArray.Length);
        //            cryptoStream.FlushFinalBlock();
        //        }
        //        returnstr = Encoding.Unicode.GetString(memoryStream.ToArray());
        //    }
        //    return returnstr;
        //}

        #endregion String AesEncryptBase64 and AesDecryptBase64 Extensions

        #region Stream AesEncrypt Extensions

        ///// <summary>
        ///// Encrypts a Stream and returns the Encrypted Stream.
        ///// </summary>
        ///// <param name="inputStream">The input Stream.</param>
        ///// <param name="key">key byte[]</param>
        ///// <param name="iv">iv byte[]</param>
        ///// <returns>Encrypted Stream</returns>
        //public static Stream AesEncryptStreamToStream(this Stream inputStream, byte[] key, byte[] iv)
        //{
        //    var outputStream = new MemoryStream();
        //    var r = new AesCryptoServiceProvider();
        //    var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(key, iv), CryptoStreamMode.Write);
        //    inputStream.BufferCopyStream(cryptor);
        //    outputStream.Seek(0, SeekOrigin.Begin);
        //    return outputStream;
        //}

        /// <summary>
        /// Encrypts an existing Stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        public static void AesEncryptStream(this Stream inputStream, Stream outputStream, byte[] key, byte[] iv)
        {
            var r = new AesCryptoServiceProvider();
            using (var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                inputStream.BufferCopyStream(cryptor);
            }
        }

        /// <summary>
        /// Encrypts an existing Stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        public static void AesGzipEncryptStream(this Stream inputStream, Stream outputStream, byte[] key, byte[] iv)
        {
            var r = new AesCryptoServiceProvider();
            using (var ms = inputStream.GZipToStream())
            {
                using (var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    ms.BufferCopyStream(cryptor);
                }
            }
        }

        ///// <summary>
        ///// Encrypts an existing Stream.
        ///// </summary>
        ///// <param name="inputStream">The input stream.</param>
        ///// <param name="outputStream">The output stream.</param>
        ///// <param name="key">key byte[]</param>
        ///// <param name="iv">iv byte[]</param>
        //public static void AesEncryptFileStream(this Stream inputStream, FileStream outputStream, byte[] key, byte[] iv)
        //{
        //    var r = new AesCryptoServiceProvider();
        //    using (var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        //    {
        //        inputStream.BufferCopyStream(cryptor);
        //    }
        //}

        /// <summary>
        /// Encrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesEncryptToFile(this Stream inputStream, string outputFilePath, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputStream.AesEncryptToFile(new FileInfo(outputFilePath), key, iv);
        }

        /// <summary>
        /// Encrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void AesEncryptToFile(this Stream inputStream, FileInfo outputFileInfo, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            var r = new AesCryptoServiceProvider();
            outputFileInfo.Refresh();
            if (outputFileInfo.Directory == null || !outputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var writeFileAccess = new WriteFileAccess(outputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (writeFileAccess.FileExists()) outputFileInfo.Delete();
                using (var outputStream = writeFileAccess.OpenWriteFileStream(bufferSize))
                {
                    using (var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        inputStream.BufferCopyStream(cryptor);
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesGzipEncryptToFile(this Stream inputStream, string outputFilePath, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputStream.AesGzipEncryptToFile(new FileInfo(outputFilePath), key, iv);
        }

        /// <summary>
        /// Encrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void AesGzipEncryptToFile(this Stream inputStream, FileInfo outputFileInfo, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            var r = new AesCryptoServiceProvider();
            outputFileInfo.Refresh();
            if (outputFileInfo.Directory == null || !outputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var writeFileAccess = new WriteFileAccess(outputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (writeFileAccess.FileExists()) outputFileInfo.Delete();
                using (var outputStream = writeFileAccess.OpenWriteFileStream(bufferSize))
                {
                    using (var ms = inputStream.GZipToStream())
                    {
                        using (var cryptor = new CryptoStream(outputStream, r.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                        {
                            ms.BufferCopyStream(cryptor);
                        }
                    }
                }
            }
        }

        #endregion Stream AesEncrypt Extensions

        #region AesEncrypt FileInfo Extensions

        /// <summary>
        /// Encrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesEncryptToFile(this FileInfo inputFileInfo, string outputFilePath, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputFileInfo.AesEncryptToFile(new FileInfo(outputFilePath), key, iv, bufferSize, lockWaitMs);
        }

        /// <summary>
        /// Encrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFileInfo">The output file.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">@Can not open locked file! The file is locked by another process.</exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">@Can not open locked file! The file is locked by another process.</exception>
        public static void AesEncryptToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = fileAccess.OpenReadFileStream(bufferSize))
                {
                    fs.AesEncryptToFile(outputFileInfo, key, iv, bufferSize, lockWaitMs);
                }
            }
        }

        /// <summary>
        /// Encrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesGzipEncryptToFile(this FileInfo inputFileInfo, string outputFilePath, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputFileInfo.AesGzipEncryptToFile(new FileInfo(outputFilePath), key, iv, bufferSize, lockWaitMs);
        }

        /// <summary>
        /// Encrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFileInfo">The output file.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">@Can not open locked file! The file is locked by another process.</exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">@Can not open locked file! The file is locked by another process.</exception>
        public static void AesGzipEncryptToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = fileAccess.OpenReadFileStream(bufferSize))
                {
                    fs.AesGzipEncryptToFile(outputFileInfo, key, iv, bufferSize, lockWaitMs);
                }
            }
        }

        #endregion AesEncrypt FileInfo Extensions

        #region Stream AesDecrypt Extensions

        ///// <summary>
        ///// Decrypts a Stream and returns the Decrypted Stream.
        ///// </summary>
        ///// <param name="inputStream">The input stream.</param>
        ///// <param name="key">key byte[]</param>
        ///// <param name="iv">iv byte[]</param>
        ///// <returns>Decrypted Stream</returns>
        //public static Stream AesDecryptStreamToStream(this Stream inputStream, byte[] key, byte[] iv)
        //{
        //    var r = new AesCryptoServiceProvider();
        //    var decryptstream = new MemoryStream();
        //    using (var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(key, iv), CryptoStreamMode.Read))
        //    {
        //        cryptor.BufferCopyStream(decryptstream);
        //    }
        //    decryptstream.Seek(0, SeekOrigin.Begin);
        //    return decryptstream;
        //}

        /// <summary>
        /// Encrypts an existing Stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        public static void AesDecryptStream(this Stream inputStream, Stream outputStream, byte[] key, byte[] iv)
        {
            var r = new AesCryptoServiceProvider();
            using (var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                cryptor.BufferCopyStream(outputStream);
            }
        }

        ///// <summary>
        ///// Encrypts an existing Stream.
        ///// </summary>
        ///// <param name="inputStream">The input stream.</param>
        ///// <param name="outputStream">The output stream.</param>
        ///// <param name="key">key byte[]</param>
        ///// <param name="iv">iv byte[]</param>
        //public static void AesGzipDecryptStream(this Stream inputStream, Stream outputStream, byte[] key, byte[] iv)
        //{
        //    var r = new AesCryptoServiceProvider();
        //    using (var ms = new MemoryStream())
        //    {
        //        inputStream.AesDecryptStream(ms, key, iv);
        //        ms.GUnZipToStream().BufferCopyStream(outputStream);
        //    }
        //}

        /// <summary>
        /// Encrypts an existing Stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        public static void AesGzipDecryptStream(this Stream inputStream, Stream outputStream, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                inputStream.AesDecryptStream(ms, key, iv);
                ms.Seek(0, SeekOrigin.Begin);
                using (var gzstream = new GZipStream(ms, CompressionMode.Decompress, true))
                {
                    gzstream.BufferCopyStream(outputStream);
                }
            }
        }

        /// <summary>
        /// Decrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        public static void AesDecryptStreamToFile(this Stream inputStream, string outputFilePath, byte[] key, byte[] iv)
        {
            inputStream.AesDecryptStreamToFile(new FileInfo(outputFilePath), key, iv);
        }

        /// <summary>
        /// Decrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        public static void AesDecryptStreamToFile(this Stream inputStream, FileInfo outputFileInfo, byte[] key, byte[] iv)
        {
            var r = new AesCryptoServiceProvider();
            using (var decryptstream = new FileStream(outputFileInfo.FullName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var cryptor = new CryptoStream(inputStream, r.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    cryptor.BufferCopyStream(decryptstream);
                }
            }
        }

        /// <summary>
        /// Decrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesGzipDecryptToFile(this Stream inputStream, string outputFilePath, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputStream.AesGzipDecryptToFile(new FileInfo(outputFilePath), key, iv, bufferSize, lockWaitMs);
        }

        /// <summary>
        /// Decrypts a Stream and writes to a file.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputFileInfo">The output file information.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesGzipDecryptToFile(this Stream inputStream, FileInfo outputFileInfo, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            //var r = new AesCryptoServiceProvider();
            outputFileInfo.Refresh();
            if (outputFileInfo.Directory == null || !outputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var writeFileAccess = new WriteFileAccess(outputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (writeFileAccess.FileExists()) outputFileInfo.Delete();
                using (var outputStream = writeFileAccess.OpenWriteFileStream(bufferSize))
                {
                    using (var ms = new MemoryStream())
                    {
                        inputStream.AesDecryptStream(ms, key, iv);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var gzstream = new GZipStream(ms, CompressionMode.Decompress, true))
                        {
                            gzstream.BufferCopyStream(outputStream);
                        }
                    }
                }
            }
        }

        #endregion Stream AesDecrypt Extensions

        #region AesDecrypt FileInfo Extensions

        /// <summary>
        /// Decrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesDecryptToFile(this FileInfo inputFileInfo, string outputFilePath, byte[] key, byte[] iv, int lockWaitMs = 60000)
        {
            inputFileInfo.AesDecryptToFile(new FileInfo(outputFilePath), key, iv, lockWaitMs);
        }

        /// <summary>
        /// Decrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFileInfo">The output file.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">
        /// @Can not open locked file! The file is locked by another process.
        /// </exception>
        public static void AesDecryptToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, byte[] key, byte[] iv, int lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = inputFileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
                {
                    fs.AesDecryptStreamToFile(outputFileInfo, key, iv);
                }
            }
        }

        /// <summary>
        /// Decrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        public static void AesGzipDecryptFileToFile(this FileInfo inputFileInfo, string outputFilePath, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputFileInfo.AesGzipDecryptToFile(new FileInfo(outputFilePath), key, iv, bufferSize, lockWaitMs);
        }

        /// <summary>
        /// Decrypts an existing file and writes to the specified output file.
        /// </summary>
        /// <param name="inputFileInfo">The input file.</param>
        /// <param name="outputFileInfo">The output file.</param>
        /// <param name="key">key byte[]</param>
        /// <param name="iv">iv byte[]</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">@Can not open locked file! The file is locked by another process.</exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="Exception">@Can not open locked file! The file is locked by another process.</exception>
        public static void AesGzipDecryptToFile(this FileInfo inputFileInfo, FileInfo outputFileInfo, byte[] key, byte[] iv, int bufferSize = 4096, int lockWaitMs = 60000)
        {
            inputFileInfo.Refresh();
            if (inputFileInfo.Directory == null || !inputFileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            using (var fileAccess = new ReadFileAccess(inputFileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = fileAccess.OpenReadFileStream(bufferSize))
                {
                    fs.AesGzipDecryptToFile(outputFileInfo, key, iv, bufferSize, lockWaitMs);
                }
            }
        }

        #endregion AesDecrypt FileInfo Extensions

        //#region Serialization Extensions

        //#region Stream Serialization Extensions

        //#region Stream AesEncrypt Serialization Extensions

        ///// <summary>
        ///// Serializes and Encrypts an object to the current Stream.
        ///// </summary>
        ///// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        ///// <param name="stream">The current Stream.</param>
        ///// <param name="obj">The object to serialize and write.</param>
        ///// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        ///// <param name="encodingType">Encoding Enum</param>
        ///// <param name="knownTypes">Known Types for Serialization.</param>
        //public static void AesEncryptSerializeStream<T>(this Stream stream, T obj,
        //    XmlWriterSettings settings = null,
        //    Encoding encodingType = null,
        //    IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    stream.Serialize(obj, settings, encodingType, knownTypes);
        //    stream.AesEncryptStream();
        //}

        //#endregion Stream AesEncrypt Serialization Extensions

        //#region Stream AesDecrypt Serialization Extensions

        ///// <summary>
        ///// Deserializes and Decrypts an object from the current Stream.
        ///// </summary>
        ///// <typeparam name="T">Type of object to deserialize.</typeparam>
        ///// <param name="stream">The current Stream.</param>
        ///// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        ///// <param name="knownTypes">Known Types for Serialization.</param>
        ///// <returns>T object</returns>
        //public static T AesDecryptDeserializeStream<T>(this Stream stream,
        //    XmlReaderSettings settings = null,
        //    IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    using (var ms = (MemoryStream)stream.AesDecryptStreamToStream())
        //    {
        //        return ms.Deserialize<T>(settings, knownTypes);
        //    }
        //}

        //#endregion Stream AesDecrypt Serialization Extensions

        //#region Stream AesEncryptGZip Serialization Extensions

        ///// <summary>
        ///// Serializes, compresses, and Encrypts an object to the current Stream.
        ///// </summary>
        ///// <typeparam name="T">Type of object to serialize and write to this Stream.</typeparam>
        ///// <param name="stream">The current Stream.</param>
        ///// <param name="obj">The object to serialize and write.</param>
        ///// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        ///// <param name="encodingType">Encoding Enum</param>
        ///// <param name="knownTypes">Known Types for Serialization.</param>
        //public static void AesEncryptGZipSerializeStream<T>(this Stream stream, T obj,
        //    XmlWriterSettings settings = null,
        //    Encoding encodingType = null,
        //    IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    stream.GZipSerialize(obj, settings, encodingType, knownTypes);
        //    stream.AesEncryptStream();
        //}

        //#endregion Stream AesEncryptGZip Serialization Extensions

        //#region Stream AesDecryptGZip Serialization Extensions

        ///// <summary>
        ///// Deserializes, uncompresses, and Decrypts an object from the current Stream.
        ///// </summary>
        ///// <typeparam name="T">Type of object to deserialize.</typeparam>
        ///// <param name="stream">The current Stream.</param>
        ///// <param name="settings">XmlWriterSettings to be used in serialization.</param>
        ///// <param name="knownTypes">Known Types for Serialization.</param>
        ///// <returns>T object</returns>
        //public static T AesDecryptGZipDeserializeStream<T>(this Stream stream,
        //    XmlReaderSettings settings = null,
        //    IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    using (var ms = (MemoryStream)stream.AesDecryptStreamToStream())
        //    {
        //        return ms.GZipDeserialize<T>(settings, knownTypes);
        //    }
        //}

        //#endregion Stream AesDecryptGZip Serialization Extensions

        //#endregion Stream Serialization Extensions

        //#region File Serialization Extensions

        //#region File AesEncrypt Serialization Extensions

        ///// <summary>
        ///// Serializes and Encrypts an object to a file.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj">The object.</param>
        ///// <param name="filePath">The file path.</param>
        ///// <param name="lockWaitMs">The lock wait ms.</param>
        ///// <param name="rollbackType">Type of the rollback.</param>
        ///// <param name="settings">The XmlWriterSettings.</param>
        ///// <param name="encodingType">Type of the encoding.</param>
        ///// <param name="knownTypes">The known types.</param>
        ///// <returns></returns>
        //public static FileInfo AesEncryptSerializeToFile<T>(this T obj, String filePath,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.None,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    return obj.AesEncryptSerializeToFile(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
        //}

        ///// <summary>
        ///// Serializes and Encrypts an object to a file.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj">The object.</param>
        ///// <param name="fileInfo">The file information.</param>
        ///// <param name="lockWaitMs">The lock wait ms.</param>
        ///// <param name="rollbackType">Type of the rollback.</param>
        ///// <param name="settings">The XmlWriterSettings.</param>
        ///// <param name="encodingType">Type of the encoding.</param>
        ///// <param name="knownTypes">The known types.</param>
        ///// <returns></returns>
        ///// <exception cref="DirectoryNotFoundException"></exception>
        //public static FileInfo AesEncryptSerializeToFile<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.None,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory != null && fileInfo.Directory.Exists)
        //    {
        //        switch (rollbackType)
        //        {
        //            case IoRollbackType.None:
        //                fileInfo = obj.AesEncryptSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;

        //            case IoRollbackType.InMemory:
        //                fileInfo = obj.AesEncryptSerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;

        //            case IoRollbackType.FileCopy:
        //                fileInfo = obj.AesEncryptSerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;

        //            default:
        //                fileInfo = obj.AesEncryptSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        throw new DirectoryNotFoundException();
        //    }
        //    fileInfo.Refresh();
        //    return fileInfo;
        //}

        //#region Private FileInfo AesEncrypt Serializer Methods

        //private static FileInfo AesEncryptSerializeNoRollback<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory != null && fileInfo.Directory.Exists)
        //    {
        //        using (var fileAccess = new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //        {
        //            if (!fileAccess.IsAccessible) throw new Exception(@"Could not create lock file!");
        //            using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
        //            {
        //                List<Type> types = null;
        //                if (knownTypes != null) types = knownTypes.ToList();
        //                using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
        //                {
        //                    ms.AesEncryptSerializeStream(obj, settings, encodingType, types);
        //                    ms.CopyTo(fs);
        //                }
        //            }
        //        }
        //        fileInfo.Refresh();
        //        return fileInfo;
        //    }
        //    throw new DirectoryNotFoundException();
        //}

        //private static FileInfo AesEncryptSerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory != null && fileInfo.Directory.Exists)
        //    {
        //        var tempfileinfo = new FileInfo(Path.Combine(fileInfo.DirectoryName, Path.GetRandomFileName()));
        //        try
        //        {
        //            using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //            {
        //                if (fileInfo.Exists)
        //                {
        //                    fileInfo.CopyTo(tempfileinfo.FullName, true, lockWaitMs);
        //                    tempfileinfo.Refresh();
        //                }
        //                using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
        //                {
        //                    List<Type> types = null;
        //                    if (knownTypes != null) types = knownTypes.ToList();
        //                    using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
        //                    {
        //                        ms.AesEncryptSerializeStream(obj, settings, encodingType, types);
        //                        ms.CopyTo(fs);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            fileInfo.Refresh();
        //            if (fileInfo.Exists) fileInfo.Delete();
        //            if (tempfileinfo.Exists) tempfileinfo.MoveTo(fileInfo.FullName);
        //            throw;
        //        }
        //        finally
        //        {
        //            if (tempfileinfo.Exists) tempfileinfo.Delete();
        //        }
        //    }
        //    else
        //    {
        //        throw new DirectoryNotFoundException();
        //    }
        //    fileInfo.Refresh();
        //    return fileInfo;
        //}

        //private static FileInfo AesEncryptSerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
        //    List<Type> types = null;
        //    if (knownTypes != null) types = knownTypes.ToList();
        //    using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
        //    {
        //        using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //        {
        //            using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
        //            {
        //                ms.AesEncryptSerializeStream(obj, settings, encodingType, types);
        //                ms.CopyTo(fs);
        //            }
        //        }
        //    }
        //    fileInfo.Refresh();
        //    return fileInfo;
        //}

        //#endregion Private FileInfo AesEncrypt Serializer Methods

        //#endregion File AesEncrypt Serialization Extensions

        //#region File AesDecrypt Deserialization Extensions

        ///// <summary>
        ///// Deserializes and Decrypts an object from a file.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="fileInfo">The file information.</param>
        ///// <param name="lockWaitMs">The lock wait ms.</param>
        ///// <param name="settings">The XmlReaderSettings.</param>
        ///// <param name="encodingType">Type of the encoding.</param>
        ///// <param name="knownTypes">The known types.</param>
        ///// <returns></returns>
        ///// <exception cref="DirectoryNotFoundException"></exception>
        ///// <exception cref="Exception">
        ///// @Can not open locked file! The file is locked by another process.
        ///// </exception>
        //public static T AesDecryptDeserializeFile<T>(this FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlReaderSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
        //    using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //    {
        //        if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
        //        using (var fs = fileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
        //        {
        //            return fs.AesDecryptDeserializeStream<T>(settings, knownTypes);
        //        }
        //    }
        //}

        //#endregion File AesDecrypt Deserialization Extensions

        //#region File AesEncryptGZip Serialization Extensions

        ///// <summary>
        ///// Serializes, compresses, and Encrypts an object to a file.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj">The object.</param>
        ///// <param name="filePath">The file path.</param>
        ///// <param name="lockWaitMs">The lock wait ms.</param>
        ///// <param name="rollbackType">Type of the rollback.</param>
        ///// <param name="settings">The XmlWriterSettings.</param>
        ///// <param name="encodingType">Type of the encoding.</param>
        ///// <param name="knownTypes">The known types.</param>
        ///// <returns></returns>
        //public static FileInfo AesEncryptGZipSerializeToFile<T>(this T obj, String filePath,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.None,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    return obj.AesEncryptGZipSerializeToFile(new FileInfo(filePath), lockWaitMs, rollbackType, settings, encodingType, knownTypes);
        //}

        ///// <summary>
        ///// Serializes, compresses, and Encrypts an object to a file.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj">The object.</param>
        ///// <param name="fileInfo">The file information.</param>
        ///// <param name="lockWaitMs">The lock wait ms.</param>
        ///// <param name="rollbackType">Type of the rollback.</param>
        ///// <param name="settings">The XmlWriterSettings.</param>
        ///// <param name="encodingType">Type of the encoding.</param>
        ///// <param name="knownTypes">The known types.</param>
        ///// <returns></returns>
        ///// <exception cref="DirectoryNotFoundException"></exception>
        //public static FileInfo AesEncryptGZipSerializeToFile<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.None,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory != null && fileInfo.Directory.Exists)
        //    {
        //        switch (rollbackType)
        //        {
        //            case IoRollbackType.None:
        //                fileInfo = obj.AesEncryptGZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;

        //            case IoRollbackType.InMemory:
        //                fileInfo = obj.AesEncryptGZipSerializeRollbackInMemory(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;

        //            case IoRollbackType.FileCopy:
        //                fileInfo = obj.AesEncryptGZipSerializeRollbackFromCopy(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;

        //            default:
        //                fileInfo = obj.AesEncryptGZipSerializeNoRollback(fileInfo, lockWaitMs, settings, encodingType, knownTypes);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        throw new DirectoryNotFoundException();
        //    }
        //    fileInfo.Refresh();
        //    return fileInfo;
        //}

        //#region Private FileInfo AesEncryptGZip Serializer Methods

        //private static FileInfo AesEncryptGZipSerializeNoRollback<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
        //    using (var fileAccess = new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //    {
        //        if (!fileAccess.IsAccessible) throw new Exception(@"Could not create lock file!");
        //        using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
        //        {
        //            List<Type> types = null;
        //            if (knownTypes != null) types = knownTypes.ToList();
        //            using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
        //            {
        //                ms.AesEncryptGZipSerializeStream(obj, settings, encodingType, types);
        //                ms.CopyTo(fs);
        //            }
        //        }
        //    }
        //    fileInfo.Refresh();
        //    return fileInfo;
        //}

        //private static FileInfo AesEncryptGZipSerializeRollbackFromCopy<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory != null && fileInfo.Directory.Exists)
        //    {
        //        var tempfileinfo = new FileInfo(Path.Combine(fileInfo.DirectoryName, Path.GetRandomFileName()));
        //        try
        //        {
        //            using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //            {
        //                if (fileInfo.Exists)
        //                {
        //                    fileInfo.CopyTo(tempfileinfo.FullName, true, lockWaitMs);
        //                    tempfileinfo.Refresh();
        //                }
        //                using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
        //                {
        //                    List<Type> types = null;
        //                    if (knownTypes != null) types = knownTypes.ToList();
        //                    using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
        //                    {
        //                        ms.AesEncryptGZipSerializeStream(obj, settings, encodingType, types);
        //                        ms.CopyTo(fs);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            fileInfo.Refresh();
        //            if (fileInfo.Exists) fileInfo.Delete();
        //            if (tempfileinfo.Exists) tempfileinfo.MoveTo(fileInfo.FullName);
        //            throw;
        //        }
        //        finally
        //        {
        //            if (tempfileinfo.Exists) tempfileinfo.Delete();
        //        }
        //    }
        //    else
        //    {
        //        throw new DirectoryNotFoundException();
        //    }
        //    fileInfo.Refresh();
        //    return fileInfo;
        //}

        //private static FileInfo AesEncryptGZipSerializeRollbackInMemory<T>(this T obj, FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlWriterSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
        //    List<Type> types = null;
        //    if (knownTypes != null) types = knownTypes.ToList();
        //    using (var ms = obj.SerializeToMemoryStream(settings, encodingType, types))
        //    {
        //        using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //        {
        //            using (var fs = fileInfo.OpenFileStream(FileMode.Create, FileAccess.Write, FileShare.None, lockWaitMs, true))
        //            {
        //                ms.AesEncryptGZipSerializeStream(obj, settings, encodingType, types);
        //                ms.CopyTo(fs);
        //            }
        //        }
        //    }
        //    fileInfo.Refresh();
        //    return fileInfo;
        //}

        //#endregion Private FileInfo AesEncryptGZip Serializer Methods

        //#endregion File AesEncryptGZip Serialization Extensions

        //#region File AesDecryptGZip Deserialization Extensions

        ///// <summary>
        ///// Deserializes, uncompresses, and Decrypts an object from a file.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="fileInfo">The file information.</param>
        ///// <param name="lockWaitMs">The lock wait ms.</param>
        ///// <param name="settings">The XmlReaderSettings.</param>
        ///// <param name="encodingType">Type of the encoding.</param>
        ///// <param name="knownTypes">The known types.</param>
        ///// <returns></returns>
        ///// <exception cref="DirectoryNotFoundException"></exception>
        ///// <exception cref="Exception">
        ///// @Can not open locked file! The file is locked by another process.
        ///// </exception>
        //public static T AesDecryptGZipDeserializeFile<T>(this FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //XmlReaderSettings settings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : class
        //{
        //    fileInfo.Refresh();
        //    if (fileInfo.Directory == null || !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
        //    using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
        //    {
        //        if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
        //        using (var fs = fileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, lockWaitMs, true))
        //        {
        //            return fs.AesDecryptGZipDeserializeStream<T>(settings, knownTypes);
        //        }
        //    }
        //}

        //#endregion File AesDecryptGZip Deserialization Extensions

        //#endregion File Serialization Extensions

        //#endregion Serialization Extensions
    }
}