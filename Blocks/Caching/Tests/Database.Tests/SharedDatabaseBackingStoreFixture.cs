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
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
    [TestClass]
    public class SharedDatabaseBackingStoreFixture
    {
        DataBackingStore firstCache;
        DataBackingStore secondCache;

        [TestInitialize]
        public void TestInitialize()
        {
            firstCache = new DataBackingStore(new SqlDatabase(@"server=(local)\SQLEXPRESS;database=Caching;Integrated Security=true"), "Partition1", null);
            secondCache = new DataBackingStore(new SqlDatabase(@"server=(local)\SQLEXPRESS;database=Caching;Integrated Security=true"), "Partition2", null);

            firstCache.Flush();
            secondCache.Flush();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            firstCache.Flush();
            secondCache.Flush();
        }

        [TestMethod]
        public void AddInsertsToCorrectDatabasePartition()
        {
            firstCache.Add(new CacheItem("key", "value", CacheItemPriority.Low, null));
            Assert.AreEqual(1, firstCache.Count);
            Assert.AreEqual(0, secondCache.Count);

            secondCache.Add(new CacheItem("key2", "value2", CacheItemPriority.High, null));
            Assert.AreEqual(1, firstCache.Count);
            Assert.AreEqual(1, secondCache.Count);
        }

        [TestMethod]
        public void LoadRetrievesFromCorrectDatabasePartition()
        {
            firstCache.Add(new CacheItem("key1", "value1", CacheItemPriority.Low, null));

            var firstDatabaseContents = firstCache.Load();
            Assert.AreEqual(1, firstDatabaseContents.Count);

            var secondDatabaseContents = secondCache.Load();
            Assert.AreEqual(0, secondDatabaseContents.Count);
        }

        [TestMethod]
        public void FlushOnClearsProperPartition()
        {
            secondCache.Add(new CacheItem("k1", "v1", CacheItemPriority.Low, null));

            firstCache.Flush();
            Assert.AreEqual(1, secondCache.Count);

            secondCache.Flush();
            Assert.AreEqual(0, secondCache.Count);
        }

        [TestMethod]
        public void RemoveOnRemovesFromCorrectPartition()
        {
            firstCache.Add(new CacheItem("key", "value", CacheItemPriority.Low, null));

            secondCache.Remove("key");
            Assert.AreEqual(1, firstCache.Count);

            firstCache.Remove("key");
            Assert.AreEqual(0, firstCache.Count);
        }

        [TestMethod]
        public void UpdateLastAccessTimeOnlyAffectsItemInCorrectPartition()
        {
            CacheItem item = new CacheItem("key", "value", CacheItemPriority.Low, null);
            DateTime originalTimeStamp = item.LastAccessedTime;

            firstCache.Add(item);

            secondCache.UpdateLastAccessedTime("key", DateTime.Now + TimeSpan.FromHours(1.0));

            var firstCacheContents = firstCache.Load();
            CacheItem retrievedItem = firstCacheContents["key"] as CacheItem;
            Assert.AreEqual(originalTimeStamp.ToString(), retrievedItem.LastAccessedTime.ToString());

            DateTime newTimestamp = DateTime.Now + TimeSpan.FromMinutes(2.0);
            firstCache.UpdateLastAccessedTime("key", newTimestamp);

            var afterChangeContents = firstCache.Load();
            CacheItem changedItem = afterChangeContents["key"] as CacheItem;
            Assert.AreEqual(newTimestamp.ToString(), changedItem.LastAccessedTime.ToString());
        }
    }
}
