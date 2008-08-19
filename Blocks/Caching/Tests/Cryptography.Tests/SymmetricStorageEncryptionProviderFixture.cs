//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Tests
{
    [TestClass]
    public class SymmetricStorageEncryptionProviderFixture
    {
        [TestMethod]
        public void SymmeticStorageEncryptionProviderDataConstructsCorrectly()
        {
            const string symm1 = "symm1";
            const string dpapi1 = "dpapi1";

            SymmetricStorageEncryptionProviderData data = new SymmetricStorageEncryptionProviderData(symm1, dpapi1);

            Assert.AreEqual(symm1, data.Name);
            Assert.AreEqual(dpapi1, data.SymmetricInstance);
            Assert.AreEqual(typeof(SymmetricStorageEncryptionProvider), data.Type);
        }

        [TestMethod]
        public void GetProvider()
        {
            SymmetricStorageEncryptionProvider provider = new SymmetricStorageEncryptionProvider(new DpapiSymmetricCryptoProvider(DataProtectionScope.CurrentUser, null));

            byte[] plainText = new byte[] { 0, 1, 2, 3, 4 };
            byte[] encrypted = provider.Encrypt(plainText);
            Assert.IsFalse(CompareBytes(plainText, encrypted));

            byte[] decrypted = provider.Decrypt(encrypted);

            Assert.IsTrue(CompareBytes(plainText, decrypted));
        }

        [TestMethod]
        public void GetCacheManagerWithDpapiEncryption()
        {
            CacheManagerFactory factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            ICacheManager mgr = factory.Create("InMemoryPersistenceWithSymmetricEncryption");
            CacheManagerTest(mgr);
        }

        [TestMethod]
        public void GetCacheManagerWithNullEncryption()
        {
            CacheManagerFactory factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            ICacheManager mgr = factory.Create("InMemoryPersistenceWithNullEncryption");
            CacheManagerTest(mgr);
        }

        void CacheManagerTest(ICacheManager mgr)
        {
            string key = "key1";
            string val = "value123";

            Assert.IsNull(mgr.GetData(key));

            mgr.Add(key, val);

            string result = (string)mgr.GetData(key);
            Assert.AreEqual(val, result, "result");
        }

        /// <summary>
        /// Test if two byte arrays are equal.
        /// </summary>
        /// <returns>True if they are the same.</returns>
        public static bool CompareBytes(byte[] byte1,
                                        byte[] byte2)
        {
            if (byte1 == null || byte2 == null)
            {
                return false;
            }
            if (byte1.Length != byte2.Length)
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < byte1.Length; i++)
            {
                if (byte1[i] != byte2[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}