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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCacheScavenging
{
    [TestClass]
    public class NumberOfItemsScavengingStrategyFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenZeroIsUsedForHighThreshold_ThenThrows()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(0, 90);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenZeroIsUsedForLowThreshold_ThenThrows()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(100, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenValueLargerThanHighIsUsedForLowThreshold_ThenThrows()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(50, 51);
        }

        [TestMethod]
        public void WhenCacheIsEmpty_ThenShouldNotScavenge()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(100, 90);
            var entries = new Dictionary<string, CacheEntry>();

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }
        
        [TestMethod]
        public void WhenCacheIsFilledAboveThreshold_ThenShouldSuggestScavenge()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(2, 1);
            var entries = new Dictionary<string, CacheEntry>{ 
                { "key1", new CacheEntry("key1", 4, new CacheItemPolicy()) },
                { "key2", new CacheEntry("key2", 4, new CacheItemPolicy()) },
                { "key3", new CacheEntry("key3", 4, new CacheItemPolicy()) },
            };

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenUsingSameThresholdAndCacheIsFilledAboveThreshold_ThenShouldSuggestScavengeAndScavengeMore()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(2, 2);
            var entries = new Dictionary<string, CacheEntry>{ 
                { "key1", new CacheEntry("key1", 4, new CacheItemPolicy()) },
                { "key2", new CacheEntry("key2", 4, new CacheItemPolicy()) },
                { "key3", new CacheEntry("key3", 4, new CacheItemPolicy()) },
            };

            Assert.IsTrue(strategy.ShouldScavenge(entries));
            Assert.IsTrue(strategy.ShouldScavengeMore(entries));
        }

        [TestMethod]
        public void WhenUsingSameThresholdAndCacheIsFilledExactlyAtThreshold_ThenShouldNotSuggestScavengeAndScavengeMore()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(2, 2);
            var entries = new Dictionary<string, CacheEntry>{ 
                { "key1", new CacheEntry("key1", 4, new CacheItemPolicy()) },
                { "key2", new CacheEntry("key2", 4, new CacheItemPolicy()) },
            };

            Assert.IsFalse(strategy.ShouldScavenge(entries));
            Assert.IsFalse(strategy.ShouldScavengeMore(entries));
        }

        [TestMethod]
        public void WhenCacheIsFilledBelowHighThreshold_ThenShouldNotScavenge()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(3, 1);
            var entries = new Dictionary<string, CacheEntry>{ 
                { "key1", new CacheEntry("key1", 4, new CacheItemPolicy()) },
                { "key2", new CacheEntry("key2", 4, new CacheItemPolicy()) },
            };

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenCacheIsFilledAboveLowThreshold_ThenShouldSuggestScavengeMore()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(3, 1);
            var entries = new Dictionary<string, CacheEntry>{ 
                { "key1", new CacheEntry("key1", 4, new CacheItemPolicy()) },
                { "key2", new CacheEntry("key2", 4, new CacheItemPolicy()) },
            };

            Assert.IsTrue(strategy.ShouldScavengeMore(entries));
        }

        [TestMethod]
        public void WhenCacheIsFilledBelowLowThreshold_ThenShouldNotScavengeAnyMore()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(3, 2);
            var entries = new Dictionary<string, CacheEntry>{ 
                { "key1", new CacheEntry("key1", 4, new CacheItemPolicy()) },
                { "key2", new CacheEntry("key2", 4, new CacheItemPolicy()) },
            };

            Assert.IsFalse(strategy.ShouldScavengeMore(entries));
        }

        [TestMethod]
        public void WhenQueriedForCandidates_ThenReturnsListOrderedByPriority()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(2, 1);
            var entries = new []
            { 
                new CacheEntry("second", 4, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable }),
                new CacheEntry("first", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default })
            };

            var actual = strategy.EntriesToScavenge(entries).ToList();

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("first", actual[0].Key);
            Assert.AreEqual("second", actual[1].Key);
        }

        [TestMethod]
        public void WhenQueriedForCandidates_ThenReturnsListOrderedByPriorityAndLastAccessTime()
        {
            var strategy = new NumberOfItemsScavengingStrategy<CacheEntry>(2, 1);
            var now = DateTimeOffset.Now;
            var entries = new CacheEntry[] 
            { 
                new MockCacheEntry("third", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default }) { LastAccessTime = now },
                new MockCacheEntry("first", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default }) { LastAccessTime = now.AddMinutes(-20) },
                new MockCacheEntry("fourth", 4, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable }),
                new MockCacheEntry("second", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default }) { LastAccessTime = now.AddMinutes(-10) },
            };

            var actual = strategy.EntriesToScavenge(entries).ToList();

            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual("first", actual[0].Key);
            Assert.AreEqual("second", actual[1].Key);
            Assert.AreEqual("third", actual[2].Key);
            Assert.AreEqual("fourth", actual[3].Key);
        }

        class MockCacheEntry : CacheEntry
        {
            public MockCacheEntry(string key, object value, CacheItemPolicy policy)
                : base(key, value, policy)
            {
            }

            public new DateTimeOffset LastAccessTime
            {
                get { return base.LastAccessTime; }
                set { base.LastAccessTime = value; }
            }
        }
    }
}
