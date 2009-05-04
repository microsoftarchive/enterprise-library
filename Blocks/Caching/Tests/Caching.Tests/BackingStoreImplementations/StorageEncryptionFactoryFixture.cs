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

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestClass]
    public class StorageEncryptionFactoryFixture
    {
        [TestMethod]
        public void GetNullEncryptorInMemory()
        {
            NullEncryptorTests("InMemoryPersistenceWithNullEncryption");
        }

        [TestMethod]
        public void GetNullEncryptedIsolatedStore()
        {
            NullEncryptorTests("InIsoStorePersistenceWithNullEncryption");
        }

        void NullEncryptorTests(string instanceName)
        {
            MockStorageEncryptionProvider.Encrypted = false;
            MockStorageEncryptionProvider.Decrypted = false;

            CacheManagerSettings settings = (CacheManagerSettings)TestConfigurationSource.GenerateConfiguration().GetSection(CacheManagerSettings.SectionName);
            CacheStorageData cacheStorageData = settings.BackingStores.Get(((CacheManagerData)settings.CacheManagers.Get(instanceName)).CacheStorage);
            StorageEncryptionProviderData encryptionProviderData = settings.EncryptionProviders.Get(cacheStorageData.StorageEncryption);

            IStorageEncryptionProvider provider =
                EnterpriseLibraryFactory.BuildUp<IStorageEncryptionProvider>(encryptionProviderData.Name, TestConfigurationSource.GenerateConfiguration());

            Assert.IsNotNull(provider);

            byte[] input = new byte[] { 0, 1, 2, 3, 4, 5 };
            byte[] encrypted = provider.Encrypt(input);

            Assert.IsTrue(MockStorageEncryptionProvider.Encrypted, "static encrypted");

            Assert.IsTrue(CompareBytes(input, encrypted), "no encryption performed");

            byte[] decrypted = provider.Decrypt(encrypted);
            Assert.IsTrue(MockStorageEncryptionProvider.Decrypted, "static decrypted");

            Assert.IsTrue(CompareBytes(encrypted, decrypted), "no decryption performed");
            Assert.IsTrue(CompareBytes(input, decrypted), "no decryption performed2");
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
