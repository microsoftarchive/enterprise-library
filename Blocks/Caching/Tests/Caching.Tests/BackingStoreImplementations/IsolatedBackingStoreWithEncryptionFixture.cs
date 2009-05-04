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

using Microsoft.Practices.EnterpriseLibrary.Caching.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestClass]
    public class IsolatedBackingStoreWithEncryptionFixture
    {
        static IsolatedStorageBackingStore backingStore;

        [TestInitialize]
        public void CreateIsolatdStorageArea()
        {
            backingStore = new IsolatedStorageBackingStore("EntLib");
            backingStore.Flush();
        }

        [TestCleanup]
        public void CleanupIsolatedStorage()
        {
            CleanOutIsolatedStorageArea();
        }

        static void CleanOutIsolatedStorageArea()
        {
            backingStore.Flush();
            backingStore.Dispose();
        }

        [TestMethod]
        public void NullEncryptor()
        {
            MockStorageEncryptionProvider.Encrypted = false;
            MockStorageEncryptionProvider.Decrypted = false;

            CacheManagerTest("InIsoStorePersistenceWithNullEncryption");

            // second instance should load up encrypted data into in memory store
            CacheManagerTest("InIsoStorePersistenceWithNullEncryption2");

            Assert.IsTrue(MockStorageEncryptionProvider.Encrypted);
            Assert.IsTrue(MockStorageEncryptionProvider.Decrypted);
        }

        [TestMethod]
        public void NoEncryptorDefined()
        {
            MockStorageEncryptionProvider.Encrypted = false;
            MockStorageEncryptionProvider.Decrypted = false;

            CacheManagerTest("InIsoStorePersistence");

            Assert.IsFalse(MockStorageEncryptionProvider.Encrypted);
            Assert.IsFalse(MockStorageEncryptionProvider.Decrypted);
        }

        void CacheManagerTest(string instanceName)
        {
            CacheManagerFactory factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            ICacheManager mgr = factory.Create(instanceName);

            string key = "key1";
            string val = "value123";

            mgr.Add(key, val);

            string result = (string)mgr.GetData(key);
            Assert.AreEqual(val, result, "result");
        }
    }
}
