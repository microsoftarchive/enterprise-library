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

using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.DatabaseAndCryptography.Tests
{
    [TestClass]
    public class DataBackingStoreWithEncryptionFixture
    {
        static DataBackingStore unencryptedBackingStore;
        static Data.Database db;
        const string CacheManagerName = "EncryptedCacheInDatabase";

        [TestInitialize]
        public void SetUp()
        {
            // to force mstest to copy the assembly
            var ignored = typeof(Caching.Cryptography.SymmetricStorageEncryptionProvider).GetConstructors();

            DatabaseProviderFactory dbFactory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
            db = dbFactory.CreateDefault();
            unencryptedBackingStore = new DataBackingStore(db, "encryptionTests", null);
            unencryptedBackingStore.Flush();

            ProtectedKey key = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.CurrentUser);
            using (FileStream stream = new FileStream("ProtectedKey.file", FileMode.Create))
            {
                KeyManager.Write(stream, key);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            unencryptedBackingStore.Flush();
            File.Delete("ProtectedKey.file");
        }

        [TestMethod]
        public void DataCanBeRetrievedWithNoEncryptionProvider()
        {
            unencryptedBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));

            Hashtable dataInCache = unencryptedBackingStore.Load();

            CacheItem retrievedItem = (CacheItem)dataInCache["key"];
            Assert.AreEqual("key", retrievedItem.Key);
            Assert.AreEqual("value", retrievedItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, retrievedItem.ScavengingPriority);
            Assert.AreEqual(typeof(MockRefreshAction), retrievedItem.RefreshAction.GetType());
            Assert.AreEqual(typeof(AlwaysExpired), retrievedItem.GetExpirations()[0].GetType());
        }

        [TestMethod]
        public void AttemptingToReadEncryptedDataWithoutDecryptingThrowsException()
        {
            IStorageEncryptionProvider encryptionProvider = null;
            encryptionProvider = EnterpriseLibraryContainer.Current.GetInstance<IStorageEncryptionProvider>("Fred");

            DataBackingStore encryptingBackingStore = new DataBackingStore(db, "encryptionTests", encryptionProvider);

            encryptingBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));
            try
            {
                Hashtable dataInCache = unencryptedBackingStore.Load();
                Assert.Fail("Expected either SerializationException or NullReferenceException");
            }
            catch(SerializationException)
            {
                // We expect this one...
            }
            catch(NullReferenceException)
            {
                // or this one (despite the docs, BinaryFormatter can throw NullReferenceException on garbage data)
            }
        }

        [TestMethod]
        public void DecryptedDataCanBeReadBackFromDatabase()
        {
            IStorageEncryptionProvider encryptionProvider = null;
            encryptionProvider = EnterpriseLibraryContainer.Current.GetInstance<IStorageEncryptionProvider>("Fred");

            DataBackingStore encryptingBackingStore = new DataBackingStore(db, "encryptionTests", encryptionProvider);

            encryptingBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));
            Hashtable dataInCache = encryptingBackingStore.Load();

            CacheItem retrievedItem = (CacheItem)dataInCache["key"];
            Assert.AreEqual("key", retrievedItem.Key);
            Assert.AreEqual("value", retrievedItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, retrievedItem.ScavengingPriority);
            Assert.AreEqual(typeof(MockRefreshAction), retrievedItem.RefreshAction.GetType());
            Assert.AreEqual(typeof(AlwaysExpired), retrievedItem.GetExpirations()[0].GetType());
        }

        [Serializable]
        class MockRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason) { }
        }
    }
}
