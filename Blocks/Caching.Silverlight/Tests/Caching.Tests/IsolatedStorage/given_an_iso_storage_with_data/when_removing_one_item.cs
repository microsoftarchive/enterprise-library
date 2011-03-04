using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_removing_one_item_via_remove_method : Context
    {
        private object removed;
        private int previousCount;

        protected override void Act()
        {
            base.Act();
            previousCount = Cache.Count();

            removed = Cache.Remove("key");

            base.RefreshCache();
        }

        [TestMethod]
        public void then_count_of_items_is_decremented()
        {
            Assert.AreEqual(previousCount - 1, Cache.GetCount());
        }

        [TestMethod]
        public void then_item_cannot_be_retrieved_by_indexer()
        {
            Assert.IsNull(Cache["key"]);
        }

        [TestMethod]
        public void then_item_cannot_be_retrieved_by_get_method()
        {
            Assert.IsNull(Cache.Get("key"));
        }

        [TestMethod]
        public void then_querying_for_key_returns_false()
        {
            Assert.IsFalse(Cache.Contains("key"));
        }

        [TestMethod]
        public void then_item_is_returned_on_removal()
        {
            Assert.AreEqual("value", removed);
        }

        [TestMethod]
        public void then_adding_that_key_again_is_posible()
        {
            Assert.IsTrue(Cache.Add("key", "new value", ObjectCache.InfiniteAbsoluteExpiration));

            this.RefreshCache();

            Assert.AreEqual("new value", Cache["key"]);
        }

        [TestMethod]
        public void then_setting_key_sets_new_value()
        {
            Cache.Set("key", "new value", new CacheItemPolicy());
            Assert.AreEqual("new value", Cache["key"]);

            this.RefreshCache();

            Assert.AreEqual("new value", Cache["key"]);
        }

        [TestMethod]
        public void then_cache_item_for_key_cannot_be_retrieved()
        {
            var item = Cache.GetCacheItem("key");
            Assert.IsNull(item);
        }

        [TestMethod]
        public void then_removing_that_key_again_returns_null()
        {
            object newRemoval = Cache.Remove("key");

            Assert.IsNull(newRemoval);
        }

        [TestMethod]
        public void then_removing_that_key_again_does_not_decrement_count()
        {
            var count = Cache.GetCount();
            object newRemoval = Cache.Remove("key");

            Assert.AreEqual(count, Cache.GetCount());
        }
    }
}
