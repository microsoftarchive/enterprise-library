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

using System.Collections;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class ScavengerFixture : ICacheOperations
    {
        static string scavengedKeys = "";
        static Hashtable inMemoryCache;
        CachingInstrumentationProvider instrumentationProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            scavengedKeys = "";
            inMemoryCache = new Hashtable();
            instrumentationProvider = new CachingInstrumentationProvider();
        }

        [TestMethod]
        public void WillRemoveSingleItemFromCache()
        {
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(0);
            ScavengerTask scavenger = new ScavengerTask(1, scavengingPolicy, this, instrumentationProvider);
            CacheItem itemToRemove = new CacheItem("key", "value", CacheItemPriority.Low, null);
            itemToRemove.MakeEligibleForScavenging();
            AddCacheItem("key", itemToRemove);

            scavenger.DoScavenging();

            Assert.AreEqual("key", scavengedKeys);
        }

        [TestMethod]
        public void WillNotRemoveItemIfNotEligibleForScavenging()
        {
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(1);
            ScavengerTask scavenger = new ScavengerTask(0, scavengingPolicy, this, instrumentationProvider);
            CacheItem itemToRemove = new CacheItem("key", "value", CacheItemPriority.Low, null);
            itemToRemove.MakeNotEligibleForScavenging();
            AddCacheItem("key", itemToRemove);

            scavenger.DoScavenging();

            Assert.AreEqual("", scavengedKeys);
        }

        [TestMethod]
        public void WillRemoveMultipleEligibleForScavenging()
        {
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(2);
            ScavengerTask scavenger = new ScavengerTask(3, scavengingPolicy, this, instrumentationProvider);
            CacheItem itemToRemove = new CacheItem("key1", "value", CacheItemPriority.High, null);
            CacheItem itemToRemain = new CacheItem("key2", "value", CacheItemPriority.Low, null);
            CacheItem itemToRemoveAlso = new CacheItem("key3", "value", CacheItemPriority.Normal, null);

            itemToRemove.MakeEligibleForScavenging();
            itemToRemain.MakeEligibleForScavenging();
            itemToRemoveAlso.MakeEligibleForScavenging();

            AddCacheItem("key1", itemToRemove);
            AddCacheItem("key2", itemToRemain);
            AddCacheItem("key3", itemToRemoveAlso);

            scavenger.DoScavenging();

            Assert.AreEqual("key2key3key1", scavengedKeys);
        }

        [TestMethod]
        public void WillStopRemovingAtLimitForScavenging()
        {
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(2);
            ScavengerTask scavenger = new ScavengerTask(2, scavengingPolicy, this, instrumentationProvider);
            CacheItem itemToRemove = new CacheItem("key1", "value", CacheItemPriority.High, null);
            CacheItem itemToRemain = new CacheItem("key2", "value", CacheItemPriority.Low, null);
            CacheItem itemToRemoveAlso = new CacheItem("key3", "value", CacheItemPriority.Normal, null);

            itemToRemove.MakeEligibleForScavenging();
            itemToRemain.MakeEligibleForScavenging();
            itemToRemoveAlso.MakeEligibleForScavenging();

            AddCacheItem("key1", itemToRemove);
            AddCacheItem("key2", itemToRemain);
            AddCacheItem("key3", itemToRemoveAlso);

            scavenger.DoScavenging();

            Assert.AreEqual("key2key3", scavengedKeys);
        }

        [TestMethod]
        public void WillNotDieIfNotEnoughItemsToScavenge()
        {
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(2);
            ScavengerTask scavenger = new ScavengerTask(4, scavengingPolicy, this, instrumentationProvider);
            CacheItem itemToRemove = new CacheItem("key1", "value", CacheItemPriority.High, null);
            CacheItem itemToRemain = new CacheItem("key2", "value", CacheItemPriority.Low, null);
            CacheItem itemToRemoveAlso = new CacheItem("key3", "value", CacheItemPriority.Normal, null);

            itemToRemove.MakeEligibleForScavenging();
            itemToRemain.MakeEligibleForScavenging();
            itemToRemoveAlso.MakeEligibleForScavenging();

            AddCacheItem("key1", itemToRemove);
            AddCacheItem("key2", itemToRemain);
            AddCacheItem("key3", itemToRemoveAlso);

            scavenger.DoScavenging();

            Assert.AreEqual("key2key3key1", scavengedKeys);
        }

        [TestMethod]
        public void WillScavenge()
        {
            CacheItem item1 = new CacheItem("key1", "value1", CacheItemPriority.NotRemovable, null);
            CacheItem item2 = new CacheItem("key2", "value2", CacheItemPriority.High, null);
            CacheItem item3 = new CacheItem("key3", "value3", CacheItemPriority.Normal, null);
            CacheItem item4 = new CacheItem("key4", "value4", CacheItemPriority.Low, null);

            AddCacheItem("key1", item1);
            AddCacheItem("key2", item2);
            AddCacheItem("key3", item3);
            AddCacheItem("key4", item4);

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(1);
            ScavengerTask scavenger = new ScavengerTask(2, scavengingPolicy, this, instrumentationProvider);
            scavenger.DoScavenging();

            Assert.AreEqual("key4key3", scavengedKeys);
        }

        [TestMethod]
        public void CanScavengeInBackground()
        {
            CacheItem item1 = new CacheItem("key1", "value1", CacheItemPriority.Low, null);
            CacheItem item2 = new CacheItem("key2", "value2", CacheItemPriority.Normal, null);
            CacheItem item3 = new CacheItem("key3", "value3", CacheItemPriority.High, null);

            AddCacheItem("key1", item1);
            AddCacheItem("key2", item2);
            AddCacheItem("key3", item3);

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(2);
            ScavengerTask scavenger = new ScavengerTask(1, scavengingPolicy, this, instrumentationProvider);
            BackgroundScheduler scheduler = new BackgroundScheduler(null, scavenger, instrumentationProvider);
            scheduler.Start();

            Thread.Sleep(500);
            scheduler.StartScavenging();
            Thread.Sleep(250);

            scheduler.Stop();
            Thread.Sleep(250);

            Assert.AreEqual("key1", scavengedKeys);
        }

        [TestMethod]
        public void WillScavengeNoItemsIfNumberOfItemsToScavengeIsZero()
        {
            CacheItem item1 = new CacheItem("key1", "value1", CacheItemPriority.Low, null);
            CacheItem item2 = new CacheItem("key2", "value2", CacheItemPriority.Normal, null);
            CacheItem item3 = new CacheItem("key3", "value3", CacheItemPriority.High, null);

            AddCacheItem("key1", item1);
            AddCacheItem("key2", item2);
            AddCacheItem("key3", item3);

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(2);
            ScavengerTask scavenger = new ScavengerTask(0, scavengingPolicy, this, instrumentationProvider);
            scavenger.DoScavenging();

            Assert.AreEqual("", scavengedKeys);
        }

        void AddCacheItem(string key,
                          CacheItem cacheItem)
        {
            inMemoryCache[key] = cacheItem;
        }

        public Hashtable CurrentCacheState
        {
            get { return inMemoryCache; }
        }

        public void RemoveItemFromCache(string keyToRemove,
                                        CacheItemRemovedReason removalReason)
        {
            scavengedKeys += keyToRemove;
        }
    }
}