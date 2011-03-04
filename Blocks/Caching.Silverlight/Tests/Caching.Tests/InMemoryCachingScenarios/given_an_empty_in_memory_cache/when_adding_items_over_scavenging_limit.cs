using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_an_empty_in_memory_cache
{
    [TestClass]
    public class when_adding_items_over_scavenging_limit : Context
    {
        private string lastKeyAdded;

        protected override void Act()
        {
            base.Act();

            var currentTime = new DateTimeOffset(2011, 1, 2, 2, 3, 0, TimeSpan.Zero);

            for (int i = 0; i <= ItemsBeforeScavenging; ++i)
            {
                var key = string.Format("key {0}", i + 1);
                var value = string.Format("value {0}", i + 1);

                DateTimeOffset timeItemAdded = currentTime;
                CachingTimeProvider.SetTimeProviderForTests(() => timeItemAdded);
                Cache.Add(key, value, ObjectCache.InfiniteAbsoluteExpiration);
                currentTime += TimeSpan.FromMinutes(1);
                lastKeyAdded = key;
            }
        }

        [TestMethod]
        public void then_one_scavenge_is_scheduled()
        {
            Assert.AreEqual(1, NumberOfScavengesScheduled);
        }

        [TestMethod]
        public void then_cache_scavenges_down_to_after_scavenging_limit()
        {
            Assert.AreEqual(ItemsAfterScavenging, Cache.GetCount());
        }

        [TestMethod]
        public void then_cache_contains_newest_item_only()
        {
            Assert.IsTrue(Cache.Contains(lastKeyAdded));
        }
    }
}
