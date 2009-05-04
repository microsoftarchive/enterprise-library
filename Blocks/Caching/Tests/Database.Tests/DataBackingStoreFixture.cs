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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.Expirations;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
    [TestClass]
    public class DataBackingStoreFixture
    {
        DataBackingStore backingStore;
        const string instanceName = "CachingDatabase";

        [TestInitialize]
        public void TestInitialize()
        {
            string partitionName = "Partition1";

            backingStore = CreateBackingStore(instanceName, partitionName);

            Data.Database db = new SqlDatabase(@"server=(local)\SQLEXPRESS;database=Caching;Integrated Security=true");
            DbCommand wrapper = db.GetSqlStringCommand("delete from CacheData");
            db.ExecuteNonQuery(wrapper);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            backingStore.Flush();
        }

        [TestMethod]
        public void CanConstructDataCacheStorageDataCorrectly()
        {
            const string name = "name";
            const string instance = "instance";
            const string partition = "partition";

            DataCacheStorageData data = new DataCacheStorageData(name, instance, partition);

            Assert.AreEqual(name, data.Name);
            Assert.AreEqual(instance, data.DatabaseInstanceName);
            Assert.AreEqual(partition, data.PartitionName);
            Assert.AreEqual(typeof(DataBackingStore), data.Type);
        }

        [TestMethod]
        public void CanGetCountOfItems()
        {
            Assert.AreEqual(0, backingStore.Count);
        }

        [TestMethod]
        public void CanAddMinimalCacheItemToStoreWithItemNotPresent()
        {
            CacheItem cacheItem = new CacheItem("key", new SerializableClass(13), CacheItemPriority.Low, null);
            backingStore.Add(cacheItem);

            Assert.AreEqual(1, backingStore.Count);
        }

        [TestMethod]
        public void CanAddWithNullValue()
        {
            CacheItem cacheItem = new CacheItem("key", null, CacheItemPriority.Low, null);
            backingStore.Add(cacheItem);

            Assert.AreEqual(1, backingStore.Count);
        }

        [TestMethod]
        public void CanSerializeOneExpiration()
        {
            CacheItem cacheItem = new CacheItem("key", null, CacheItemPriority.Low, null, new AlwaysExpired());
            backingStore.Add(cacheItem);

            Assert.AreEqual(1, backingStore.Count);
        }

        [TestMethod]
        public void CanSerializeThreeExpirations()
        {
            CacheItem cacheItem = new CacheItem("key", null, CacheItemPriority.Low, null,
                                                new AlwaysExpired(), new SlidingTime(TimeSpan.FromSeconds(10)), new AbsoluteTime(TimeSpan.FromHours(1.0)));
            backingStore.Add(cacheItem);

            Assert.AreEqual(1, backingStore.Count);
        }

        [TestMethod]
        public void CanSerializeFullCacheItem()
        {
            CacheItem cacheItem = new CacheItem("key", new SerializableClass(13), CacheItemPriority.Low,
                                                new RefreshAction(),
                                                new AlwaysExpired(), new SlidingTime(TimeSpan.FromSeconds(10)), new AbsoluteTime(TimeSpan.FromHours(1.0)));
            backingStore.Add(cacheItem);

            Assert.AreEqual(1, backingStore.Count);
        }

        [TestMethod]
        public void CanFlush()
        {
            backingStore.Add(new CacheItem("key", "value", CacheItemPriority.Low, null));

            Assert.AreEqual(1, backingStore.Count);

            backingStore.Flush();
            Assert.AreEqual(0, backingStore.Count);
        }

        [TestMethod]
        public void CanLoadEmptyTable()
        {
            Hashtable emptyHashtable = backingStore.Load();
            Assert.AreEqual(0, emptyHashtable.Count);
        }

        [TestMethod]
        public void CanLoadOneItemFromTable()
        {
            RefreshAction refreshAction = new RefreshAction();
            CacheItem addedItem = new CacheItem("key", "value", CacheItemPriority.NotRemovable,
                                                refreshAction,
                                                new SlidingTime(TimeSpan.FromHours(1)), new AbsoluteTime(TimeSpan.FromHours(1.0)));
            backingStore.Add(addedItem);

            Hashtable retrievedData = backingStore.Load();
            Assert.AreEqual(1, retrievedData.Count);

            CacheItem item = (CacheItem)retrievedData["key"];
            Assert.AreEqual("key", item.Key);
            Assert.AreEqual("value", item.Value);
            Assert.AreEqual(CacheItemPriority.NotRemovable, item.ScavengingPriority);
            Assert.AreEqual(typeof(RefreshAction), item.RefreshAction.GetType());
            Assert.IsFalse(ReferenceEquals(refreshAction, item.RefreshAction));
            Assert.AreEqual(2, item.GetExpirations().Length);
            Assert.AreEqual(typeof(SlidingTime), item.GetExpirations()[0].GetType());
            Assert.AreEqual(typeof(AbsoluteTime), item.GetExpirations()[1].GetType());
        }

        [TestMethod]
        public void CanLoadOneItemWithNullExpirations()
        {
            RefreshAction refreshAction = new RefreshAction();
            CacheItem addedItem = new CacheItem("key", "value", CacheItemPriority.NotRemovable, refreshAction);
            backingStore.Add(addedItem);

            Hashtable retrievedData = backingStore.Load();
            Assert.AreEqual(1, retrievedData.Count);
        }

        [TestMethod]
        public void CanLoadOneItemWithNullRefreshAction()
        {
            CacheItem addedItem = new CacheItem("key", "value", CacheItemPriority.NotRemovable, null);
            backingStore.Add(addedItem);

            Hashtable retrievedData = backingStore.Load();
            Assert.AreEqual(1, retrievedData.Count);
        }

        [TestMethod]
        public void CanLoadOneItemWithNullValue()
        {
            CacheItem addedItem = new CacheItem("key", null, CacheItemPriority.Low, null);
            backingStore.Add(addedItem);

            Hashtable retrievedData = backingStore.Load();
            Assert.AreEqual(1, retrievedData.Count);
        }

        [TestMethod]
        public void CanLoadTwoItemsFromCache()
        {
            CacheItem firstItem = new CacheItem("key1", "value1", CacheItemPriority.Low, null);
            CacheItem secondItem = new CacheItem("key2", "value2", CacheItemPriority.Low, null);

            backingStore.Add(firstItem);
            backingStore.Add(secondItem);

            Hashtable retrievedData = backingStore.Load();
            Assert.AreEqual(2, retrievedData.Count);
            Assert.IsNotNull("key1", ((CacheItem)retrievedData["key1"]).Key);
            Assert.IsNotNull("key2", ((CacheItem)retrievedData["key2"]).Key);
        }

        [TestMethod]
        public void CanRemoveOneItemFromCache()
        {
            CacheItem itemToRemove = new CacheItem("key1", "value1", CacheItemPriority.Low, null);
            backingStore.Add(itemToRemove);

            backingStore.Remove("key1");

            Assert.AreEqual(0, backingStore.Count);
        }

        [TestMethod]
        public void CanRemoveOneItemInCacheAndLeaveOneInCache()
        {
            CacheItem itemToRemove = new CacheItem("key1", "value1", CacheItemPriority.Low, null);
            CacheItem itemToKeep = new CacheItem("key2", "value2", CacheItemPriority.Low, null);
            backingStore.Add(itemToRemove);
            backingStore.Add(itemToKeep);

            backingStore.Remove("key1");

            Assert.AreEqual(1, backingStore.Count);

            Hashtable retrievedData = backingStore.Load();
            Assert.IsNotNull(retrievedData["key2"]);
        }

        [TestMethod]
        public void CanUpdateLastAccessedTime()
        {
            CacheItem item = new CacheItem("key", "value", CacheItemPriority.Low, null);
            DateTime yesterday = DateTime.Now - TimeSpan.FromDays(1.0);
            item.SetLastAccessedTime(yesterday);

            backingStore.Add(item);

            DateTime now = DateTime.Now;
            backingStore.UpdateLastAccessedTime("key", now);

            Hashtable retrievedData = backingStore.Load();
            CacheItem retrievedItem = (CacheItem)retrievedData["key"];

            Assert.AreEqual(now.ToString(), retrievedItem.LastAccessedTime.ToString());
        }

        [TestMethod]
        public void ExpiredItemsWillStillBeLoadedFromStorage()
        {
            CacheItem item = new CacheItem("key", "value", CacheItemPriority.Low, null, new AlwaysExpired());
            backingStore.Add(item);

            Hashtable cacheItems = backingStore.Load();
            Assert.AreEqual(1, cacheItems.Count, "Should not load expired item from store");
            Assert.AreEqual(1, backingStore.Count);
        }

        [TestMethod]
        public void CleansUpAfterFailedAdd()
        {
            CacheItem canBeSerialized = new CacheItem("key", "value", CacheItemPriority.Normal, null);
            CacheItem cannotBeSerialized = new CacheItem("key", new NotSerializableClass(), CacheItemPriority.Normal, null);

            backingStore.Add(canBeSerialized);
            try
            {
                backingStore.Add(cannotBeSerialized);
                Assert.Fail("Adding non-serializable object to cache should have thrown exception");
            }
            catch (Exception)
            {
                Assert.AreEqual(0, backingStore.Count, "All traces of failed add should be removed, including original item");
            }
        }

        [TestMethod]
        public void AddingTwoKeysDifferingOnlyInCaseResultsInTwoCacheItems()
        {
            CacheItem lowerCase = new CacheItem("key", "value", CacheItemPriority.Normal, null);
            CacheItem upperCase = new CacheItem("KEY", "different value", CacheItemPriority.Normal, null);

            backingStore.Add(lowerCase);
            backingStore.Add(upperCase);

            Hashtable cacheItems = backingStore.Load();
            Assert.AreEqual(2, cacheItems.Count);

            CacheItem lowerCaseItem = cacheItems["key"] as CacheItem;
            CacheItem upperCaseItem = cacheItems["KEY"] as CacheItem;

            Assert.AreEqual("value", lowerCaseItem.Value);
            Assert.AreEqual("different value", upperCaseItem.Value);
        }

        DataBackingStore CreateBackingStore(string instanceName,
                                            string partitionName)
        {
            DataBackingStore backingStore = new DataBackingStore(new SqlDatabase(@"server=(local)\SQLEXPRESS;database=Caching;Integrated Security=true"), partitionName, null);

            return backingStore;
        }

        [Serializable]
        class SerializableClass
        {
            public int counter;

            public SerializableClass(int counter)
            {
                this.counter = counter;
            }
        }

        class NotSerializableClass {}

        [Serializable]
        class RefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason) {}
        }
    }
}
