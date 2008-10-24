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
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestClass]
    public class IsolatedStorageBackingStoreFixture
    {
        static IsolatedStorageBackingStore backingStore;
        static IsolatedStorageBackingStore differentStore;

        [TestInitialize]
        public void CreateIsolatdStorageArea()
        {
            backingStore = new IsolatedStorageBackingStore("foo", null);
            differentStore = new IsolatedStorageBackingStore("diffFoo", null);
        }

        [TestCleanup]
        public void CleanOutIsolatedStorageArea()
        {
            backingStore.Flush();
            backingStore.Dispose();

            differentStore.Flush();
            differentStore.Dispose();
        }

        [TestMethod]
        public void IsolatedStorageAreaEmptyWhenCreated()
        {
            Assert.AreEqual(0, backingStore.Count, "Size should be empty when created");
        }

        [TestMethod]
        public void SizeNotZeroWhenOneItemInserted()
        {
            CacheItem itemToStore = new CacheItem("key", "value", CacheItemPriority.NotRemovable, null);

            backingStore.Add(itemToStore);
            Assert.AreEqual(1, backingStore.Count, "Inserted item should be in Isolated Storage");
        }

        [TestMethod]
        public void RemoveItem()
        {
            CacheItem itemToStore = new CacheItem("key", "value", CacheItemPriority.NotRemovable, null);

            backingStore.Add(itemToStore);
            backingStore.Remove(itemToStore.Key);

            Assert.AreEqual(0, backingStore.Count, "Inserted item should have been removed");
        }

        [TestMethod]
        public void RemoveNonExistentItem()
        {
            backingStore.Remove("fred");
            Assert.AreEqual(0, backingStore.Count, "Nothing to be removed means size is still zero.");
        }

        [TestMethod]
        public void ReplaceItem()
        {
            CacheItem oldItem = new CacheItem("key", "value1", CacheItemPriority.NotRemovable, null);
            CacheItem newItem = new CacheItem("key", "value2", CacheItemPriority.Low, null);

            backingStore.Add(oldItem);
            backingStore.Add(newItem);

            Hashtable items = backingStore.Load();

            Assert.AreEqual(1, backingStore.Count, "New item should have replaced old item when New Item added.");
            Assert.AreEqual("value2", ((CacheItem)items["key"]).Value);
        }

        [TestMethod]
        public void WillLoadNoItemsFromEmptyIsolatedStorage()
        {
            Hashtable emptyHashTable = backingStore.Load();
            Assert.AreEqual(0, emptyHashTable.Count);
        }

        [TestMethod]
        public void WillLoadOneItem()
        {
            CacheItem item = new CacheItem("foo", "bar", CacheItemPriority.NotRemovable, null);
            backingStore.Add(item);

            IsolatedStorageBackingStore loadingStore = new IsolatedStorageBackingStore("foo", null);
            Hashtable items = loadingStore.Load();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("bar", ((CacheItem)items["foo"]).Value);
            Assert.AreEqual(item.Key, ((CacheItem)items["foo"]).Key);
        }

        [TestMethod]
        public void WillNotFailForUpdatingAnInvalidEntry()
        {
            const string invalidKey = "invalidKey";
            CacheItem item = new CacheItem("foo", "bar", CacheItemPriority.None, null);
            IsolatedStorageBackingStore store = new IsolatedStorageBackingStore("test");
            store.Add(item);
            store.UpdateLastAccessedTime(invalidKey, DateTime.Now);
        }

        [TestMethod]
        public void WillLoadMultipleItems()
        {
            CacheItem firstItem = new CacheItem("foo1", "bar1", CacheItemPriority.Low, null);
            CacheItem secondItem = new CacheItem("foo2", "bar2", CacheItemPriority.Low, null);
            CacheItem thirdItem = new CacheItem("foo3", "bar3", CacheItemPriority.Low, null);

            backingStore.Add(firstItem);
            backingStore.Add(secondItem);
            backingStore.Add(thirdItem);

            IsolatedStorageBackingStore loadingStore = new IsolatedStorageBackingStore("foo");
            Hashtable items = loadingStore.Load();
            Assert.AreEqual(3, items.Count);
            Assert.AreEqual("foo2", ((CacheItem)items["foo2"]).Key);
            Assert.AreEqual("foo1", ((CacheItem)items["foo1"]).Key);
            Assert.AreEqual("foo3", ((CacheItem)items["foo3"]).Key);
        }

        [TestMethod]
        public void CanDealWithTwoSeparateIsolatedStorageSegments()
        {
            CacheItem firstStoreItem = new CacheItem("first", "FIRST", CacheItemPriority.Low, null);
            CacheItem secondStoreItem = new CacheItem("second", "SECOND", CacheItemPriority.Low, null);

            backingStore.Add(firstStoreItem);
            Assert.AreEqual(1, backingStore.Count, "Added one item to this store");
            Assert.AreEqual(0, differentStore.Count, "Have not added anything to this store yet");

            differentStore.Add(secondStoreItem);
            Assert.AreEqual(1, backingStore.Count, "Still have only one item in this store");
            Assert.AreEqual(1, differentStore.Count, "Now added item to this store");

            differentStore.Flush();
            Assert.AreEqual(1, backingStore.Count, "Have not flushed this store yet");
            Assert.AreEqual(0, differentStore.Count, "Flushed this store already");

            backingStore.Flush();
            Assert.AreEqual(0, backingStore.Count, "Now we flushed this store as well");
        }

        [TestMethod]
        public void UpdateLastAccessedTime()
        {
            CacheItem item = new CacheItem("first", "data", CacheItemPriority.Normal, null);
            backingStore.Add(item);

            DateTime future = DateTime.Now + TimeSpan.FromHours(1.0);
            backingStore.UpdateLastAccessedTime(item.Key, future);

            Hashtable items = backingStore.Load();
            Assert.AreEqual(future, ((CacheItem)items["first"]).LastAccessedTime);
        }

        [TestMethod]
        public void WillDeletePartiallyAddedItemOnException()
        {
            CacheItem unsavableItem = new CacheItem("key", new NonSerializableClass(), CacheItemPriority.Normal, null);
            try
            {
                backingStore.Add(unsavableItem);
                Assert.Fail("Saving non-serializable class should throw exception");
            }
            catch (Exception) {}

            Hashtable shouldBeEmpty = backingStore.Load();
            Assert.AreEqual(0, shouldBeEmpty.Count, "Partially added item should be removed from cache on add failure");
        }

        [TestMethod]
        public void PartialFailureWhileAddingThrowsException()
        {
            CacheItem unsavableItem = new CacheItem("key", new NonSerializableClass(), CacheItemPriority.Normal, null);
            try
            {
                backingStore.Add(unsavableItem);
                Assert.Fail("Any exception thrown while adding should be leaked to outside callers");
            }
            catch (Exception)
            {
                // Any exception thrown will make this test pass.
            }
        }

        [TestMethod]
        public void ExpiredItemCanStillBeLoadedIntoCache()
        {
            CacheItem expiredItem = new CacheItem("key", "asdf", CacheItemPriority.Normal, null, new AlwaysExpired());
            backingStore.Add(expiredItem);

            Hashtable cacheData = backingStore.Load();
            Assert.AreEqual(1, cacheData.Count);
        }

        class NonSerializableClass {}
    }
}
