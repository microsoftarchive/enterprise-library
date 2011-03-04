using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_empty_iso_storage_cache
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_initialized : Context
    {
        [TestMethod]
        public void then_cache_name_is_set_correctly()
        {
            Assert.AreEqual(CacheName, Cache.Name);
        }

        [TestMethod]
        public void then_cache_count_is_zero()
        {
            Assert.AreEqual(0, Cache.GetCount());
        }
        
        [TestMethod]
        public void then_getting_an_item_returns_null()
        {
            Assert.IsNull(Cache["some key"]);
        }

        [TestMethod]
        public void then_enumerating_returns_no_items()
        {
            Assert.AreEqual(0, Cache.Count());
        }

        [TestMethod]
        public void then_expected_capabilities_are_correctly_returned()
        {
            var capabilities =
                DefaultCacheCapabilities.InMemoryProvider |
                    DefaultCacheCapabilities.CacheEntryUpdateCallback |
                        DefaultCacheCapabilities.CacheEntryRemovedCallback |
                            DefaultCacheCapabilities.SlidingExpirations |
                                DefaultCacheCapabilities.AbsoluteExpirations;

            Assert.AreEqual(capabilities, Cache.DefaultCacheCapabilities);
        }

        [TestMethod]
        public void then_item_can_be_added_by_indexer()
        {
            Cache["new item"] = 37;

            Assert.AreEqual(37, Cache.Get("new item"));
        }

        [TestMethod]
        public void then_removing_item_does_nothing()
        {
            object removed = Cache.Remove("not-existing");

            Assert.AreEqual(0, Cache.Count());
            Assert.IsNull(removed);
        }
    }
}
