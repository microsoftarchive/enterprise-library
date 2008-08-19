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
using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class CacheManagerFixture : ICacheScavenger
    {
        static CacheManagerFactory factory;
        static CacheManager cacheMgr;

        static string removedKey;
        static object expiredValue;
        static CacheItemRemovedReason removalReason;

        [TestInitialize]
        public void CreateCache()
        {
            removedKey = null;
            expiredValue = null;
            removalReason = CacheItemRemovedReason.Unknown;

            factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            cacheMgr = (CacheManager)factory.Create("SmallInMemoryPersistence");
        }

        [TestCleanup]
        public void ShutdownCache()
        {
            cacheMgr.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void BadStorageProviderNameThrowsException()
        {
            factory.Create("BadStorageProvider");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void BadBackingStoreNameThrowsException()
        {
            factory.Create("BadBackingStore");
        }

        [TestMethod]
        public void NoElementsInCacheAtStartupWithNoBackingStore()
        {
            Assert.AreEqual(0, cacheMgr.Count, "Cache with no backing store should always be empty at creation");
        }

        [TestMethod]
        public void OneElementAfterAdding()
        {
            cacheMgr.Add("key", "This is the cache");
            Assert.AreEqual(1, cacheMgr.Count, "Should be one element in cache after adding the item");
        }

        [TestMethod]
        public void CountZeroAfterRemoveFromCache()
        {
            cacheMgr.Add("key", "cache");
            cacheMgr.Remove("key");

            Assert.AreEqual(0, cacheMgr.Count, "Removing should decrement count back to 0");
        }

        [TestMethod]
        public void CountCorrectAfterSomeAddsAndRemoves()
        {
            cacheMgr.Add("key", "adsf");
            cacheMgr.Add("key2", "asdfsd");
            cacheMgr.Remove("key");
            Assert.AreEqual(1, cacheMgr.Count);
            cacheMgr.Remove("key2");
            Assert.AreEqual(0, cacheMgr.Count);
        }

        [TestMethod]
        public void CanRetrieveItemFromCache()
        {
            cacheMgr.Add("key", "value");
            object retrievedValue = cacheMgr.GetData("key");
            Assert.AreEqual("value", retrievedValue);
        }

        [TestMethod]
        public void RetrieveDoesNotRemoveItemFromCache()
        {
            cacheMgr.Add("key", "value");
            cacheMgr.GetData("key");
            Assert.AreEqual(1, cacheMgr.Count);
        }

        [TestMethod]
        public void FlushRemovesAllElements()
        {
            cacheMgr.Add("key1", "value1");
            cacheMgr.Add("key2", "value2");
            cacheMgr.Flush();
            Assert.AreEqual(0, cacheMgr.Count, "Flushing should remove all elements from cache");
        }

        [TestMethod]
        public void CanFlushEmptyCache()
        {
            cacheMgr.Flush();
            Assert.AreEqual(0, cacheMgr.Count, "Flushing empty cache should work");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PassingNullKeyToAddThrowsException()
        {
            cacheMgr.Add(null, "asdf");
        }

        [TestMethod]
        public void CanRetrieveNullValueFromCache()
        {
            cacheMgr.Add("null", null);
            object retrievedNull = cacheMgr.GetData("null");
            Assert.IsNull(retrievedNull, "Null in should be null when taken out");
        }

        [TestMethod]
        public void RemovingNonExistentItemIsOk()
        {
            cacheMgr.Remove("fred");
            Assert.IsTrue(true, "Should be OK to remove non-existent item from cache");
        }

        [TestMethod]
        public void CanRemoveNonExistentItemAfterItWasAlreadyInCache()
        {
            cacheMgr.Add("linus", "foo");
            cacheMgr.Remove("linus");
            cacheMgr.Remove("linus");

            Assert.AreEqual(0, cacheMgr.Count);
        }

        [TestMethod]
        public void GettingDataForNonExistentItemReturnsNull()
        {
            Assert.IsNull(cacheMgr.GetData("nonExistentKey"));
        }

        [TestMethod]
        public void CanAddSameKeyTwice()
        {
            cacheMgr.Add("key", "value1");
            cacheMgr.Add("key", "value2");

            Assert.AreEqual(1, cacheMgr.Count, "Duplicate keys should result in only one cache item with second value");
            Assert.AreEqual("value2", cacheMgr.GetData("key"));
        }

        [TestMethod]
        public void CanAddItemWithExpirationIntoCache()
        {
            cacheMgr.Add("key", "value", CacheItemPriority.Normal, null, new NeverExpired());
            Assert.AreEqual("value", cacheMgr.GetData("key"));
        }

        [TestMethod]
        public void GettingDataThatIsAlreadyExpiredReturnsNull()
        {
            cacheMgr.Add("key", "value", CacheItemPriority.Normal, null, new AlwaysExpired());
            Assert.IsNull(cacheMgr.GetData("key"), "Expired item should have been removed before returning it");
        }

        [TestMethod]
        public void GettingDataThatIsAlreadyExpiredCausesCallback()
        {
            cacheMgr.Add("expiredKey", "expiredValue", CacheItemPriority.NotRemovable, new MockCallback(), new AlwaysExpired());
            object returnedData = cacheMgr.GetData("expiredKey");
            Assert.IsNull(returnedData);
            Thread.Sleep(100);

            Assert.AreEqual("expiredKey", removedKey);
            Assert.AreEqual("expiredValue", expiredValue);
            Assert.AreEqual(CacheItemRemovedReason.Expired, removalReason);
        }

        [TestMethod]
        public void RemovingItemFromCacheCausesRemovedCallback()
        {
            cacheMgr.Add("removedKey", "removedValue", CacheItemPriority.NotRemovable, new MockCallback(), new NeverExpired());
            cacheMgr.Remove("removedKey");
            Thread.Sleep(100);

            Assert.AreEqual("removedKey", removedKey);
            Assert.AreEqual("removedValue", expiredValue);
            Assert.AreEqual(CacheItemRemovedReason.Removed, removalReason);
        }

        public void StartScavenging() {}

        class MockCallback : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason)
            {
                CacheManagerFixture.removedKey = removedKey;
                CacheManagerFixture.expiredValue = expiredValue;
                CacheManagerFixture.removalReason = removalReason;
            }
        }
    }
}