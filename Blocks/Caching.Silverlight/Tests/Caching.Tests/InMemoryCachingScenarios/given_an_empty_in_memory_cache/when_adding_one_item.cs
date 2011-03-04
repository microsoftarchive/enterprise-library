using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_an_empty_in_memory_cache
{
    [TestClass]
    public class when_adding_one_item_via_add_method : Context
    {
        private bool added;

        protected override void Act()
        {
            base.Act();

            added = Cache.Add("key", "value", new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });
        }

        [TestMethod]
        public void then_item_was_successfully_added()
        {
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void then_count_of_items_is_now_one()
        {
            Assert.AreEqual(1, Cache.GetCount());
        }

        [TestMethod]
        public void then_item_can_be_retrieved_by_indexer()
        {
            Assert.AreEqual("value", Cache["key"]);
        }

        [TestMethod]
        public void then_item_can_be_retrieved_by_get_method()
        {
            Assert.AreEqual("value", Cache.Get("key"));
        }

        [TestMethod]
        public void then_item_can_be_retrieved_by_enumeration()
        {
            var items = new List<KeyValuePair<string, object>>(Cache);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("key", items[0].Key);
            Assert.AreEqual("value", items[0].Value);
        }

        [TestMethod]
        public void then_adding_that_key_again_fails()
        {
            Assert.IsFalse(Cache.Add("key", "next value", ObjectCache.InfiniteAbsoluteExpiration));
        }

        [TestMethod]
        public void then_add_or_get_existing_returns_existing()
        {
            Assert.AreEqual("value",
                Cache.AddOrGetExisting("key", "new value", ObjectCache.InfiniteAbsoluteExpiration));
        }

        [TestMethod]
        public void then_setting_key_overwrites_current_value()
        {
            Cache.Set("key", "new value", new CacheItemPolicy());
            Assert.AreEqual("new value", Cache["key"]);
        }

        [TestMethod]
        public void then_cache_item_for_key_can_be_retrieved()
        {
            var item = Cache.GetCacheItem("key");
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void then_cache_item_for_key_contains_correct_key_value_and_null_region()
        {
            var item = Cache.GetCacheItem("key");
            Assert.AreEqual("key", item.Key);
            Assert.AreEqual("value", item.Value);
            Assert.IsNull(item.RegionName);
        }
    }
}
