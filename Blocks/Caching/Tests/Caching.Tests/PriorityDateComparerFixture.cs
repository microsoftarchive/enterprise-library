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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class PriorityDateComparerFixture
    {
        static Hashtable inMemoryCache;

        [TestInitialize]
        public void AddElementsToHashTable()
        {
            inMemoryCache = new Hashtable();
        }

        [TestMethod]
        public void WillSortByDateWithSamePriority()
        {
            CacheItem firstItem = new CacheItem("key", "value", CacheItemPriority.Normal, null);
            CacheItem secondItem = new CacheItem("key2", "value2", CacheItemPriority.Normal, null);
            CacheItem thirdItem = new CacheItem("key3", "value3", CacheItemPriority.Normal, null);

            AddCacheItem("key", firstItem);
            AddCacheItem("key2", secondItem);
            AddCacheItem("key3", thirdItem);

            firstItem.SetLastAccessedTime(DateTime.Now + TimeSpan.FromSeconds(200.0));
            secondItem.SetLastAccessedTime(DateTime.Now + TimeSpan.FromSeconds(100.0));
            thirdItem.SetLastAccessedTime(DateTime.Now + TimeSpan.FromSeconds(150));

            SortedList itemsToBeScavenged = new SortedList(inMemoryCache, new PriorityDateComparer(inMemoryCache));

            Assert.AreEqual("key2", GetCacheKey(itemsToBeScavenged, 0));
            Assert.AreEqual("key3", GetCacheKey(itemsToBeScavenged, 1));
            Assert.AreEqual("key", GetCacheKey(itemsToBeScavenged, 2));
        }

        [TestMethod]
        public void WillSortByPriorityWithSameDate()
        {
            CacheItem firstItem = new CacheItem("key", "value", CacheItemPriority.High, null);
            CacheItem secondItem = new CacheItem("key2", "value2", CacheItemPriority.NotRemovable, null);
            CacheItem thirdItem = new CacheItem("key3", "value3", CacheItemPriority.Low, null);
            CacheItem fourthItem = new CacheItem("key4", "value4", CacheItemPriority.Normal, null);

            DateTime currentTime = DateTime.Now;
            firstItem.SetLastAccessedTime(currentTime);
            secondItem.SetLastAccessedTime(currentTime);
            thirdItem.SetLastAccessedTime(currentTime);
            fourthItem.SetLastAccessedTime(currentTime);

            AddCacheItem("key", firstItem);
            AddCacheItem("key2", secondItem);
            AddCacheItem("key3", thirdItem);
            AddCacheItem("key4", fourthItem);

            SortedList itemsToBeScavenged = new SortedList(inMemoryCache, new PriorityDateComparer(inMemoryCache));

            Assert.AreEqual("key3", GetCacheKey(itemsToBeScavenged, 0));
            Assert.AreEqual("key4", GetCacheKey(itemsToBeScavenged, 1));
            Assert.AreEqual("key", GetCacheKey(itemsToBeScavenged, 2));
            Assert.AreEqual("key2", GetCacheKey(itemsToBeScavenged, 3));
        }

        [TestMethod]
        public void BigSortingTest()
        {
            CacheItem firstItem = new CacheItem("key", "value", CacheItemPriority.High, null);
            CacheItem secondItem = new CacheItem("key2", "value2", CacheItemPriority.NotRemovable, null);
            CacheItem thirdItem = new CacheItem("key3", "value3", CacheItemPriority.Low, null);
            CacheItem fourthItem = new CacheItem("key4", "value4", CacheItemPriority.Normal, null);
            CacheItem fifthItem = new CacheItem("key5", "value", CacheItemPriority.High, null);
            CacheItem sixthItem = new CacheItem("key6", "value2", CacheItemPriority.NotRemovable, null);
            CacheItem seventhItem = new CacheItem("key7", "value3", CacheItemPriority.Low, null);
            CacheItem eighthItem = new CacheItem("key8", "value4", CacheItemPriority.Normal, null);

            DateTime currentTime = DateTime.Now;
            DateTime oneHourInFuture = DateTime.Now + TimeSpan.FromHours(1.0);
            firstItem.SetLastAccessedTime(oneHourInFuture);
            secondItem.SetLastAccessedTime(oneHourInFuture);
            thirdItem.SetLastAccessedTime(oneHourInFuture);
            fourthItem.SetLastAccessedTime(oneHourInFuture);
            fifthItem.SetLastAccessedTime(currentTime);
            sixthItem.SetLastAccessedTime(currentTime);
            seventhItem.SetLastAccessedTime(currentTime);
            eighthItem.SetLastAccessedTime(currentTime);

            AddCacheItem("key", firstItem);
            AddCacheItem("key2", secondItem);
            AddCacheItem("key3", thirdItem);
            AddCacheItem("key4", fourthItem);
            AddCacheItem("key5", fifthItem);
            AddCacheItem("key6", sixthItem);
            AddCacheItem("key7", seventhItem);
            AddCacheItem("key8", eighthItem);

            SortedList itemsToBeScavenged = new SortedList(inMemoryCache, new PriorityDateComparer(inMemoryCache));

            Assert.AreEqual("key7", GetCacheKey(itemsToBeScavenged, 0));
            Assert.AreEqual("key3", GetCacheKey(itemsToBeScavenged, 1));
            Assert.AreEqual("key8", GetCacheKey(itemsToBeScavenged, 2));
            Assert.AreEqual("key4", GetCacheKey(itemsToBeScavenged, 3));
            Assert.AreEqual("key5", GetCacheKey(itemsToBeScavenged, 4));
            Assert.AreEqual("key", GetCacheKey(itemsToBeScavenged, 5));
            Assert.AreEqual("key6", GetCacheKey(itemsToBeScavenged, 6));
            Assert.AreEqual("key2", GetCacheKey(itemsToBeScavenged, 7));
        }

        void AddCacheItem(string key,
                          CacheItem cacheItem)
        {
            inMemoryCache[key] = cacheItem;
        }

        string GetCacheKey(SortedList cacheItems,
                           int cacheIndex)
        {
            CacheItem cacheItem = (CacheItem)cacheItems.GetByIndex(cacheIndex);
            return cacheItem.Key;
        }
    }
}
