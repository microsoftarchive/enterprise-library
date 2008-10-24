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

using System;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
    [TestClass]
    public class HashCryptographerFixture
    {
        ProtectedKey key;

        [TestInitialize]
        public void CreateKey()
        {
            key = KeyManager.GenerateKeyedHashKey(typeof(HMACSHA1), DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructingWithBadTypeThrows()
        {
            new HashCryptographer(typeof(object), (ProtectedKey)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingWithNullTypeThrows()
        {
            new HashCryptographer(null, (ProtectedKey)null);
        }

        [TestMethod]
        public void HashMD5()
        {
            byte[] plaintext = new byte[] { 0, 1, 2, 3 };
            HashCryptographer cryptographer = new HashCryptographer(typeof(MD5CryptoServiceProvider));
            byte[] hash1 = cryptographer.ComputeHash(plaintext);

            Assert.IsFalse(CryptographyUtility.CompareBytes(plaintext, hash1));

            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] hash2 = md5.ComputeHash(plaintext);

            Assert.IsTrue(CryptographyUtility.CompareBytes(hash1, hash2));
        }

        [TestMethod]
        public void HashHMACSHA1()
        {
            byte[] plaintext = new byte[] { 0, 1, 2, 3 };
            HashCryptographer cryptographer = new HashCryptographer(typeof(HMACSHA1), key);
            byte[] hash1 = cryptographer.ComputeHash(plaintext);

            Assert.IsFalse(CryptographyUtility.CompareBytes(plaintext, hash1));

            KeyedHashAlgorithm hmacsha1 = HMACSHA1.Create();
            hmacsha1.Key = key.DecryptedKey;
            byte[] hash2 = hmacsha1.ComputeHash(plaintext);

            Assert.IsTrue(CryptographyUtility.CompareBytes(hash1, hash2));
        }
    }
}
