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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_an_empty_in_memory_cache
{
    [TestClass]
    public class when_adding_items_over_scavenging_limit : Context
    {
        private string lastKeyAdded;
        private List<CacheEntryRemovedArguments> removedArgumentsList;

        protected override void Act()
        {
            removedArgumentsList = new List<CacheEntryRemovedArguments>();

            base.Act();

            var currentTime = new DateTimeOffset(2011, 1, 2, 2, 3, 0, TimeSpan.Zero);

            for (int i = 0; i <= ItemsBeforeScavenging; ++i)
            {
                var key = string.Format("key {0}", i + 1);
                var value = string.Format("value {0}", i + 1);

                DateTimeOffset timeItemAdded = currentTime;
                CachingTimeProvider.SetTimeProviderForTests(() => timeItemAdded);
                Cache.Add(key, value, new CacheItemPolicy
                {
                    AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                    RemovedCallback = (args) => this.removedArgumentsList.Add(args)
                });
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

        [TestMethod]
        public void then_removed_callback_was_invoked_for_each_removed_item()
        {
            Assert.AreEqual(ItemsBeforeScavenging - ItemsAfterScavenging + 1, removedArgumentsList.Count);
            foreach (var removedArguments in removedArgumentsList)
            {
                Assert.IsNotNull(removedArguments);
                Assert.AreEqual(Cache, removedArguments.Source);
                Assert.AreEqual(CacheEntryRemovedReason.Evicted, removedArguments.RemovedReason);
            }

            Assert.IsNotNull(removedArgumentsList.Single(x => x.CacheItem.Key == "key 1"));
            Assert.IsNotNull(removedArgumentsList.Single(x => x.CacheItem.Key == "key 2"));
        }
    }
}
