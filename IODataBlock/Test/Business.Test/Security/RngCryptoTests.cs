using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Common.Security;
using System.Security.Cryptography;

namespace Business.Test.Security
{
    [TestClass]
    public class RngCryptoTests
    {
        [TestMethod]
        public void MakeTripleDesKeyAndIv()
        {
            var keyBytes = RngCrypto.GenerateTripleDesKey(); // 24
            var ivBytes = RngCrypto.GenerateTripleDesIv(); // 8

            Assert.IsNotNull(keyBytes);
            Assert.IsNotNull(ivBytes);
        }

        [TestMethod]
        public void MakeAesKeyAndIv()
        {
            var keyBytes = RngCrypto.GenerateAesKey(); // 32
            var ivBytes = RngCrypto.GenerateAesIv(); // 16

            Assert.IsNotNull(keyBytes);
            Assert.IsNotNull(ivBytes);
        }

        [TestMethod]
        public void MakeTripleDesKeyAndIvGenerateFalse()
        {
            var keyBytes = RngCrypto.GenerateTripleDesKey(false); // 24
            var ivBytes = RngCrypto.GenerateTripleDesIv(false); // 8

            Assert.IsNotNull(keyBytes);
            Assert.IsNotNull(ivBytes);
        }

        [TestMethod]
        public void MakeAesKeyAndIvGenerateFalse()
        {
            var keyBytes = RngCrypto.GenerateAesKey(false); // 32
            var ivBytes = RngCrypto.GenerateAesIv(false); // 16

            Assert.IsNotNull(keyBytes);
            Assert.IsNotNull(ivBytes);
        }

        [TestMethod]
        public void GetDerivedKeyBytes()
        {
            var salt = RngCrypto.GenerateSalt();

            var keyBytes1 = RngCrypto.GetDerivedByteArray("hello world", salt, 128);
            var keyBytes2 = RngCrypto.GetDerivedByteArray("hello world", salt, 128);

            Assert.IsNotNull(keyBytes1);
            Assert.IsNotNull(keyBytes2);
            Assert.IsTrue(Convert.ToBase64String(keyBytes1) == Convert.ToBase64String(keyBytes2));
        }

        [TestMethod]
        public void GetAesBytes()
        {
            var salt = RngCrypto.GenerateSalt(16);
            var pwd = Environment.UserDomainName;

            var keyBytes1 = RngCrypto.GetAesKeyBytes(pwd, salt);
            var keyBytes2 = RngCrypto.GetAesKeyBytes(pwd, salt);

            Assert.IsNotNull(keyBytes1);
            Assert.IsNotNull(keyBytes2);
            Assert.IsTrue(Convert.ToBase64String(keyBytes1) == Convert.ToBase64String(keyBytes2));

            var ivBytes1 = RngCrypto.GetAesIvBytes(pwd, salt);
            var ivBytes2 = RngCrypto.GetAesIvBytes(pwd, salt);

            Assert.IsNotNull(ivBytes1);
            Assert.IsNotNull(ivBytes2);
            Assert.IsTrue(Convert.ToBase64String(ivBytes1) == Convert.ToBase64String(ivBytes2));
        }

        [TestMethod]
        public void AesLegalKeySizes()
        {
            var aes = new AesManaged();
            var ks = aes.LegalKeySizes;
            foreach (var k in ks)
            {
                var min = k.MinSize;
                var max = k.MaxSize;
            }
        }

        [TestMethod]
        public void TripleDESLegalKeySizes()
        {
            var tsp = new TripleDESCryptoServiceProvider();
            var ks = tsp.LegalKeySizes;
            foreach (var k in ks)
            {
                var min = k.MinSize;
                var max = k.MaxSize;
            }
        }

        [TestMethod]
        public void AesKeySizes()
        {
            var aes = new AesManaged();
            var ks = aes.KeySize;
            var vs = aes.IV.Length;
            Assert.IsNotNull(ks); // 256
            Assert.IsNotNull(vs); // 16
        }

        [TestMethod]
        public void TripleDESKeySizes()
        {
            var csp = new TripleDESCryptoServiceProvider();
            var ks = csp.KeySize;
            var vs = csp.IV.Length;
            Assert.IsNotNull(ks); // 192
            Assert.IsNotNull(vs); // 8
        }

        [TestMethod]
        public void GenerateBase64Salt()
        {
            var str = RngCrypto.GenerateBase64Salt();
            Assert.IsNotNull(str);
        }

        
    }
}
