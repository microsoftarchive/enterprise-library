//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class CryptographerFixture
    {
        const string hashInstance = "hmac1";
        const string symmInstance = "dpapiSymmetric1";
        const string symmetricAlgorithm1 = "symmetricAlgorithm1";
        const string symmetricKeyFile = "ProtectedKey.file";
        const string keyedHashKeyFile = "KeyedHashKey.file";

        const string plainTextString = "secret";
        static readonly byte[] plainTextBytes = new byte[] { 0, 1, 2, 3 };

        [TestInitialize]
        public void CreateKeyFile()
        {
            CreateKeyFile(symmetricKeyFile);
            CreateKeyFile(keyedHashKeyFile);
        }

        [TestCleanup]
        public void DeleteKeyFile()
        {
            File.Delete(symmetricKeyFile);
            File.Delete(keyedHashKeyFile);
        }

        [TestMethod]
        public void CreateHashBytes()
        {
            byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes);
            Assert.IsFalse(CryptographyUtility.CompareBytes(plainTextBytes, hash));
        }

        [TestMethod]
        public void CreateHashString()
        {
            string hashString = Cryptographer.CreateHash(hashInstance, plainTextString);
            Assert.IsFalse(plainTextString == hashString);
        }

        [TestMethod]
        public void CreateAndCompareHashBytes()
        {
            byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes);
            bool result = Cryptographer.CompareHash(hashInstance, plainTextBytes, hash);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateAndCompareInvalidHashBytes()
        {
            byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes);

            byte[] badPlainText = new byte[] { 2, 1, 0 };
            bool result = Cryptographer.CompareHash(hashInstance, badPlainText, hash);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateAndCompareHashString()
        {
            string hashString = Cryptographer.CreateHash(hashInstance, plainTextString);

            bool result = Cryptographer.CompareHash(hashInstance, plainTextString, hashString);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EncryptAndDecryptBytes()
        {
            byte[] encrypted = Cryptographer.EncryptSymmetric(symmInstance, plainTextBytes);
            Assert.IsFalse(CryptographyUtility.CompareBytes(plainTextBytes, encrypted));

            byte[] decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainTextBytes, decrypted));
        }

        [TestMethod]
        public void EncryptAndDecryptString()
        {
            string encrypted = Cryptographer.EncryptSymmetric(symmInstance, plainTextString);
            Assert.IsFalse(plainTextString == encrypted);

            string decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted);
            Assert.IsTrue(plainTextString == decrypted);
        }

        [TestMethod]
        public void EncryptAndDecryptStringWithASymmetricAlgorithm()
        {
            string encrypted = Cryptographer.EncryptSymmetric(symmetricAlgorithm1, plainTextString);
            Assert.IsFalse(plainTextString == encrypted);

            string decrypted = Cryptographer.DecryptSymmetric(symmetricAlgorithm1, encrypted);
            Assert.IsTrue(plainTextString == decrypted);
        }

        [TestMethod]
        public void EncryptAndDecryptOneByte()
        {
            byte[] onebyte = new byte[1];
            CryptographyUtility.GetRandomBytes(onebyte);

            byte[] encrypted = Cryptographer.EncryptSymmetric(symmInstance, onebyte);
            Assert.IsFalse(CryptographyUtility.CompareBytes(onebyte, encrypted));

            byte[] decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted);

            Assert.IsTrue(CryptographyUtility.CompareBytes(onebyte, decrypted));
        }

        [TestMethod]
        public void EncryptAndDecryptOneMegabyte()
        {
            byte[] megabyte = new byte[1024 * 1024];
            CryptographyUtility.GetRandomBytes(megabyte);

            byte[] encrypted = Cryptographer.EncryptSymmetric(symmInstance, megabyte);
            Assert.IsFalse(CryptographyUtility.CompareBytes(megabyte, encrypted));

            byte[] decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted);

            Assert.IsTrue(CryptographyUtility.CompareBytes(megabyte, decrypted));
        }

        public static void CreateKeyFile(string fileName)
        {
            ProtectedKey key = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.CurrentUser);
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                KeyManager.Write(stream, key);
            }
        }
    }
}