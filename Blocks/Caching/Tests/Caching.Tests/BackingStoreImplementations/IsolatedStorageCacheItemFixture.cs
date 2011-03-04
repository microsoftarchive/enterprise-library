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
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.Expirations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestClass]
    public class IsolatedStorageCacheItemFixture
    {
        static IsolatedStorageFile storage;
        const string DirectoryName = "1234567890123";
        const string DirectoryRoot = DirectoryName + @"\";

        [TestInitialize]
        public void CreateIsolatedStorage()
        {
            storage = IsolatedStorageFile.GetUserStoreForDomain();
            storage.CreateDirectory(DirectoryName);
        }

        [TestCleanup]
        public void FlushIsolatedStorage()
        {
            string[] directories = storage.GetDirectoryNames(DirectoryRoot + "*");
            foreach (string directoryName in directories)
            {
                string directoryRoot = DirectoryRoot + directoryName;
                string[] files = storage.GetFileNames(directoryRoot + @"\*");
                foreach (string fileName in files)
                {
                    string fileToDelete = directoryRoot + @"\" + fileName;
                    storage.DeleteFile(fileToDelete);
                }
                storage.DeleteDirectory(directoryRoot);
            }

            storage.DeleteDirectory(DirectoryName);
        }

        [TestMethod]
        public void StoreMinimalCacheItemIntoIsolatedStorage()
        {
            CacheItem itemToStore = new CacheItem("key", "value", CacheItemPriority.NotRemovable, null);

            string itemRoot = DirectoryRoot + itemToStore.Key;
            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, null);
            item.Store(itemToStore);

            Assert.AreEqual(1, storage.GetDirectoryNames(DirectoryRoot + @"\*").Length);
        }

        [TestMethod]
        public void ReadMinimalCacheItemFromIsolatedStorage()
        {
            ReadMinimalCacheItemFromIsolatedStorageEncrypted(false);
        }

        void ReadMinimalCacheItemFromIsolatedStorageEncrypted(bool encrypted)
        {
            DateTime historicalTimestamp = DateTime.Now - TimeSpan.FromHours(1.0);

            CacheItem itemToStore = new CacheItem("key1", "value1", CacheItemPriority.Normal, null);
            itemToStore.SetLastAccessedTime(historicalTimestamp);

            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, encrypted);

            Assert.AreEqual("key1", readItem.Key);
            Assert.AreEqual("value1", readItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, readItem.ScavengingPriority);
            Assert.IsNull(readItem.RefreshAction);
            ICacheItemExpiration[] expirations = readItem.GetExpirations();
            Assert.AreEqual(0, expirations.Length);
            Assert.AreEqual(historicalTimestamp, readItem.LastAccessedTime);
        }

        [TestMethod]
        public void ReadCacheItemWithNullValue()
        {
            CacheItem itemToStore = new CacheItem("key", null, CacheItemPriority.Normal, null);
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            Assert.IsNull(readItem.Value);
            Assert.AreEqual(itemToStore.Key, readItem.Key);
        }

        [TestMethod]
        public void ReadCacheItemWithRefreshAction()
        {
            CacheItem itemToStore = new CacheItem("key", null, CacheItemPriority.Normal, new MockRefreshAction());
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            Assert.IsNotNull(readItem.RefreshAction, "MockRefreshAction should have been stored into IsoStore and retrieved.");
        }

        [TestMethod]
        public void ReadCacheItemWithOneExpirationAction()
        {
            CacheItem itemToStore = new CacheItem("monkey", "baboon", CacheItemPriority.NotRemovable, null, new NeverExpired());
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            ICacheItemExpiration[] expirations = readItem.GetExpirations();
            Assert.AreEqual(1, expirations.Length);
            Assert.AreEqual(typeof(NeverExpired), expirations[0].GetType());
        }

        [TestMethod]
        public void ReadCacheItemWithThreeExpirationActions()
        {
            CacheItem itemToStore = new CacheItem("monkey", "baboon", CacheItemPriority.NotRemovable, null,
                                                  new NeverExpired(), new AlwaysExpired(), new AbsoluteTime(TimeSpan.FromSeconds(1.0)));
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            ICacheItemExpiration[] expirations = readItem.GetExpirations();
            Assert.AreEqual(3, expirations.Length);
            Assert.AreEqual(typeof(NeverExpired), expirations[0].GetType());
            Assert.AreEqual(typeof(AlwaysExpired), expirations[1].GetType());
            Assert.AreEqual(typeof(AbsoluteTime), expirations[2].GetType());
        }

        [TestMethod]
        public void UpdateLastAccessedTime()
        {
            CacheItem historicalItem = new CacheItem("foo", "bar", CacheItemPriority.NotRemovable, null);
            historicalItem.SetLastAccessedTime(DateTime.Now - TimeSpan.FromHours(1.0));

            string itemRoot = DirectoryRoot + historicalItem.Key;
            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, null);
            item.Store(historicalItem);

            DateTime now = DateTime.Now;
            historicalItem.SetLastAccessedTime(DateTime.Now);
            item.UpdateLastAccessedTime(now);

            CacheItem restoredItem = item.Load();

            Assert.AreEqual(now, restoredItem.LastAccessedTime);
        }

        [TestMethod]
        public void StoreAndEncryptItem()
        {
            IStorageEncryptionProvider encryptionProvider = new MockStorageEncryptionProvider();

            CacheItem itemToStore = new CacheItem("key", "value", CacheItemPriority.NotRemovable, null);

            string itemRoot = DirectoryRoot + itemToStore.Key;
            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, encryptionProvider);
            item.Store(itemToStore);

            Assert.AreEqual(1, storage.GetDirectoryNames(DirectoryRoot + @"\*").Length);
        }

        [TestMethod]
        public void ReadMinimalCacheItemFromIsolatedStorageEncrypted()
        {
            ReadMinimalCacheItemFromIsolatedStorageEncrypted(true);
        }

        CacheItem DoCacheItemRoundTripToStorage(CacheItem itemToStore,
                                                bool encrypted)
        {
            IStorageEncryptionProvider encryptionProvider = null;

            if (encrypted)
            {
                encryptionProvider = new MockStorageEncryptionProvider();
            }

            string itemRoot = DirectoryRoot + itemToStore.Key;

            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, encryptionProvider);
            item.Store(itemToStore);

            IsolatedStorageCacheItem itemToRead = new IsolatedStorageCacheItem(storage, itemRoot, encryptionProvider);
            return itemToRead.Load();
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
