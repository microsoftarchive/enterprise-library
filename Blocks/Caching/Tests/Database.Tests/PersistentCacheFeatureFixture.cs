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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
    [TestClass]
    public class PersistentCacheFeatureFixture
    {
        static object callbackLock = new object();
        static int initializationCount;
        static bool wasCalledBack = false;

        CacheManager cache;

        [TestInitialize]
        public void TestInitialize()
        {
            CacheManagerFactory factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            cache = (CacheManager)factory.Create("InDatabasePersistence");
            cache.Flush();
            wasCalledBack = false;
            initializationCount = 0;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            cache.Flush();
            cache.Dispose();
        }

        [TestMethod]
        public void ItemsGetRemovedFromCacheAfterExpiration()
        {
            Assert.IsNull(cache.GetData("key"));

            DataBackingStore backingStore = CreateDataBackingStore();
            Assert.AreEqual(0, backingStore.Load().Count);

            AbsoluteTime threeSecondExpiration = new AbsoluteTime(DateTime.Now + TimeSpan.FromSeconds(3.0));
            lock (callbackLock)
            {
                cache.Add("key", "value", CacheItemPriority.NotRemovable,
                          new RefreshAction(),
                          threeSecondExpiration);

                Assert.IsNotNull(cache.GetData("key"));

                Hashtable oneEntryHashTable = backingStore.Load();
                Assert.AreEqual(1, oneEntryHashTable.Count);

                Monitor.Wait(callbackLock, 15000);

                Assert.IsTrue(wasCalledBack);
            }

            object removedItem = cache.GetData("key");
            Assert.IsNull(removedItem);

            Hashtable emptyHashTable = backingStore.Load();
            Assert.AreEqual(0, emptyHashTable.Count);
        }

        DataBackingStore CreateDataBackingStore()
        {
            DataBackingStore backingStore = new DataBackingStore(new SqlDatabase(@"server=(local)\SQLEXPRESS;database=Caching;Integrated Security=true"), "Partition1", null);
            return backingStore;
        }

        [TestMethod]
        public void AddMultipleItems()
        {
            cache.Add("AddM1", "12345");
            cache.Add("AddM2", "23456");
            cache.Add("AddM3", "34567");
            object o1 = cache.GetData("AddM1");
            object o2 = cache.GetData("AddM2");
            object o3 = cache.GetData("AddM3");

            Assert.AreEqual("12345", o1);
            Assert.AreEqual("23456", o2);
            Assert.AreEqual("34567", o3);

            DataBackingStore backingStore = CreateDataBackingStore();
            Hashtable inDatabaseItems = backingStore.Load();
            Assert.AreEqual("12345", ((CacheItem)inDatabaseItems["AddM1"]).Value);
            Assert.AreEqual("23456", ((CacheItem)inDatabaseItems["AddM2"]).Value);
            Assert.AreEqual("34567", ((CacheItem)inDatabaseItems["AddM3"]).Value);
        }

        [TestMethod]
        public void AddSameKey()
        {
            cache.Add("Add1", "12345");
            cache.Add("Add1", "23456");
            object o1 = cache.GetData("Add1");

            Assert.AreEqual("23456", o1);

            DataBackingStore backingStore = CreateDataBackingStore();
            Hashtable inDatabaseItems = backingStore.Load();
            Assert.AreEqual("23456", ((CacheItem)inDatabaseItems["Add1"]).Value);
        }

        [TestMethod]
        public void CacheRemove()
        {
            cache.Add("Remove1", "98761");
            cache.Remove("Remove1");

            Assert.IsNull(cache.GetData("Remove1"));

            DataBackingStore backingStore = CreateDataBackingStore();

            Hashtable inDatabaseItems = backingStore.Load();
            Assert.AreEqual(0, inDatabaseItems.Count);
        }

        [TestMethod]
        public void RetrieveItemThroughIndexer()
        {
            cache.Add("key", "value");
            object value = cache["key"];

            Assert.AreEqual("value", value);
        }

        [TestMethod]
        public void ItemsInitializeTheirExpirationsProperlyWhenLoaded()
        {
            cache.Add("key", "value", CacheItemPriority.Normal, null, new MockExpiration());
            initializationCount = 0;    // reset the initializations.

            CacheManager differentCacheManager = (CacheManager)new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration()).Create("SecondInDatabasePersistence");
            differentCacheManager.Dispose();

            Assert.AreEqual(1, initializationCount, "Act of creating new cache manager should have caused items to be loaded, initializing expirations");
        }

        [TestMethod]
        public void CanAddTwoCacheItemsWithKeysDifferentByCaseOnly()
        {
            cache.Add("KEY", "value 1");
            cache.Add("key", "value 2");

            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual("value 1", cache["KEY"] as string);
            Assert.AreEqual("value 2", cache["key"] as string);
        }

        [Serializable]
        class MockExpiration : ICacheItemExpiration
        {
            public bool HasExpired()
            {
                return false;
            }

            public void Initialize(CacheItem owningCacheItem)
            {
                initializationCount++;
            }

            public void Notify() {}
        }

        [Serializable]
        class RefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key,
                                object expiredValue,
                                CacheItemRemovedReason removalReason)
            {
                lock (callbackLock)
                {
                    wasCalledBack = true;
                    Monitor.Pulse(callbackLock);
                }
            }
        }
    }
}
