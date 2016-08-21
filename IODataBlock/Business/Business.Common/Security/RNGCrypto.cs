using System;
using System.Security.Cryptography;

namespace Business.Common.Security
{
    public static class RngCrypto
    {
        public static string GenerateBase64Salt(int saltLength = 8)
        {
            return Convert.ToBase64String(GenerateSalt(saltLength));
        }

        public static byte[] GenerateSalt(int saltLength = 8)
        {
            var rv = new byte[saltLength];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(rv);
            }
            return rv;
        }

        public static Rfc2898DeriveBytes GetDerivedBytes(string pwd, int saltLength = 8, int iterations = 1000) // Note* - Will generate random derived bytes each time called.
        {
            return new Rfc2898DeriveBytes(pwd, GenerateSalt(saltLength), iterations);
        }

        public static Rfc2898DeriveBytes GetDerivedBytes(string pwd, byte[] salt, int iterations = 1000)
        {
            return new Rfc2898DeriveBytes(pwd, salt, iterations);
        }

        public static Rfc2898DeriveBytes GetDerivedBytes(string pwd, string salt, int iterations = 1000)
        {
            return new Rfc2898DeriveBytes(pwd, Convert.FromBase64String(salt), iterations);
        }

        public static byte[] GetDerivedByteArray(string pwd, int saltLength = 8, int length = 16, int iterations = 1000) // Note* - Will generate random derived bytes each time called.
        {
            var derivedBytes = GetDerivedBytes(pwd, GenerateSalt(saltLength), iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetDerivedByteArray(string pwd, byte[] salt, int length = 16, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetDerivedByteArray(string pwd, string salt, int length = 16, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, Convert.FromBase64String(salt), iterations);
            return derivedBytes.GetBytes(length);
        }
        
        #region TripleDesCryptoServiceProvider Methods

        public static byte[] GetTripleDesKeyBytes(string pwd, byte[] salt, int length = 24, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetTripleDesKeyBytes(string pwd, string salt, int length = 24, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetTripleDesIvBytes(string pwd, byte[] salt, int length = 8, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetTripleDesIvBytes(string pwd, string salt, int length = 8, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        #region TripleDesCryptoServiceProvider Basic Generate Methods

        public static byte[] GenerateTripleDesKey(bool useGenerateMethod = true) // 24 bytes default length.
        {
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            if (useGenerateMethod)
            {
                cryptoProvider.GenerateKey();
            }
            return cryptoProvider.Key;
        }

        public static byte[] GenerateTripleDesIv(bool useGenerateMethod = true) // 8 bytes default length.
        {
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            if (useGenerateMethod)
            {
                cryptoProvider.GenerateIV();
            }
            return cryptoProvider.IV;
        }

        #endregion TripleDesCryptoServiceProvider Basic Generate Methods

        #endregion TripleDesCryptoServiceProvider Methods

        #region AesCryptoServiceProvider Methods

        public static byte[] GetAesKeyBytes(string pwd, byte[] salt, int length = 32, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetAesKeyBytes(string pwd, string salt, int length = 32, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetAesIvBytes(string pwd, byte[] salt, int length = 16, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        public static byte[] GetAesIvBytes(string pwd, string salt, int length = 16, int iterations = 1000)
        {
            var derivedBytes = GetDerivedBytes(pwd, salt, iterations);
            return derivedBytes.GetBytes(length);
        }

        #region AesCryptoServiceProvider Basic Generate Methods

        public static byte[] GenerateAesKey(bool useGenerateMethod = true) //32 bytes default length.
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            if (useGenerateMethod)
            {
                cryptoProvider.GenerateKey();
            }
            return cryptoProvider.Key;
        }

        public static byte[] GenerateAesIv(bool useGenerateMethod = true) //16 bytes default length.
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            if (useGenerateMethod)
            {
                cryptoProvider.GenerateIV();
            }
            return cryptoProvider.IV;
        }

        #endregion AesCryptoServiceProvider Basic Generate Methods

        #endregion AesCryptoServiceProvider Methods
    }
}