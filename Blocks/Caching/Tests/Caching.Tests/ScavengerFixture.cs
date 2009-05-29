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
        private int inMemoryCacheRequests;
        private EventWaitHandle inMemoryCacheRequestSemaphore;
        ICachingInstrumentationProvider instrumentationProvider;


        [TestInitialize]
        public void TestInitialize()
        {
            scavengedKeys = "";
            inMemoryCache = new Hashtable();
            inMemoryCacheRequests = 0;
            inMemoryCacheRequestSemaphore = null;
            instrumentationProvider = new NullCachingInstrumentationProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (inMemoryCacheRequestSemaphore != null)
            {
                inMemoryCacheRequestSemaphore.Close();
            }
        }

        [TestMethod]
        public void WillRemoveSingleItemFromCache()
        {
            ScavengerTask scavenger = new ScavengerTask(1, 0, this, instrumentationProvider);
            CacheItem itemToRemove = new CacheItem("key", "value", CacheItemPriority.Low, null);
            itemToRemove.MakeEligibleForScavenging();
            AddCacheItem("key", itemToRemove);

            scavenger.DoScavenging();

            Assert.AreEqual("key", scavengedKeys);
        }

        [TestMethod]
        public void WillNotRemoveItemIfNotEligibleForScavenging()
        {
            ScavengerTask scavenger = new ScavengerTask(0, 1, this, instrumentationProvider);
            CacheItem itemToRemove = new CacheItem("key", "value", CacheItemPriority.Low, null);
            itemToRemove.MakeNotEligibleForScavenging();
            AddCacheItem("key", itemToRemove);

            scavenger.DoScavenging();

            Assert.AreEqual("", scavengedKeys);
        }

        [TestMethod]
        public void WillRemoveMultipleEligibleForScavenging()
        {
            ScavengerTask scavenger = new ScavengerTask(3, 2, this, instrumentationProvider);
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
            ScavengerTask scavenger = new ScavengerTask(2, 2, this, instrumentationProvider);
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
            ScavengerTask scavenger = new ScavengerTask(4, 2, this, instrumentationProvider);
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

            ScavengerTask scavenger = new ScavengerTask(2, 1, this, instrumentationProvider);
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

            ScavengerTask scavenger = new ScavengerTask(1, 2, this, instrumentationProvider);
            BackgroundScheduler scheduler = new BackgroundScheduler(null, scavenger, instrumentationProvider);

            scheduler.StartScavenging();
            Thread.Sleep(250);

            Assert.AreEqual("key1", scavengedKeys);
        }

        /// <summary>
        /// This test depends on timing
        /// </summary>
        [TestMethod]
        public void WillNotScheduleNewScavengeTaskIfOneIsAlreadyScheduled()
        {
            inMemoryCacheRequestSemaphore = new EventWaitHandle(false, EventResetMode.ManualReset);

            CacheItem item1 = new CacheItem("key1", "value1", CacheItemPriority.Low, null);
            CacheItem item2 = new CacheItem("key2", "value2", CacheItemPriority.Normal, null);
            CacheItem item3 = new CacheItem("key3", "value3", CacheItemPriority.Normal, null);
            CacheItem item4 = new CacheItem("key4", "value4", CacheItemPriority.Normal, null);
            CacheItem item5 = new CacheItem("key5", "value5", CacheItemPriority.High, null);
            CacheItem item6 = new CacheItem("key6", "value6", CacheItemPriority.High, null);
            CacheItem item7 = new CacheItem("key7", "value7", CacheItemPriority.High, null);

            AddCacheItem("key1", item1);
            AddCacheItem("key2", item2);
            AddCacheItem("key3", item3);

            ScavengerTask scavenger = new ScavengerTask(2, 3, this, instrumentationProvider);
            BackgroundScheduler scheduler = new BackgroundScheduler(null, scavenger, instrumentationProvider);

            AddCacheItem("key4", item4);
            // this new scavenge request will be scheduled, it's the first one
            scheduler.StartScavenging();
            // the scavenge request scheduled above would be processed here and will be blocked by the event
            Thread.Sleep(500);
            AddCacheItem("key5", item5);
            // this new scavenge request will be scheduled, because the previously scheduled one will have started
            scheduler.StartScavenging();
            Thread.Sleep(250);
            AddCacheItem("key6", item6);
            // this new scavenge request will be ignored
            scheduler.StartScavenging();
            Thread.Sleep(250);
            AddCacheItem("key7", item7);
            // this new scavenge request will be scheduled, because the previously is "full" (it handles 2 elements only)
            scheduler.StartScavenging();
            Thread.Sleep(250);
            bool value = inMemoryCacheRequestSemaphore.Set();
            Thread.Sleep(250);

            Assert.AreEqual(2, inMemoryCacheRequests);
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

            ScavengerTask scavenger = new ScavengerTask(0, 2, this, instrumentationProvider);
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
            get
            {
                if (inMemoryCacheRequestSemaphore != null)
                {
                    inMemoryCacheRequestSemaphore.WaitOne();
                }
                inMemoryCacheRequests++;
                return inMemoryCache;
            }
        }

        public void RemoveItemFromCache(string keyToRemove,
                                        CacheItemRemovedReason removalReason)
        {
            scavengedKeys += keyToRemove;
        }

        public int Count
        {
            get { return inMemoryCache.Count; }
        }
    }
}
