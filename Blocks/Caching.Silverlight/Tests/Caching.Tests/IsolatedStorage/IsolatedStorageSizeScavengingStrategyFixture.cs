using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage
{
    [TestClass]
    public class IsolatedStorageSizeScavengingStrategyFixture
    {
        private const float HighScavengeThreshold = 0.8f;
        private const float LowScavengeThreshold = 0.6f;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNegativeValueIsUsedForHighThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, -1, LowScavengeThreshold);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenValueLargerThan1IsUsedForHighThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, 1.01f, LowScavengeThreshold);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNegativeValueIsUsedForLowThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenValueLargerThanHighIsUsedForLowThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, 0.5f, 0.6f);
        }

        [TestMethod]
        public void WhenStoreIsEmpty_ThenShouldNotScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedSize == 0 && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>();

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }
        
        [TestMethod]
        public void WhenStoreIsFilledAboveThreshold_ThenShouldSuggestScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedSize == 9000 && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>{ { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) }};

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenThresholdNotSpecified_ThenUsesDefault()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedSize == 9000 && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, 0, 0);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsFilledBelowHighThreshold_ThenShouldNotScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedSize == 7000 && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsFilledAboveLowThreshold_ThenShouldSuggestScavengeMore()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedSize == 7000 && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsTrue(strategy.ShouldScavengeMore(entries));
        }

        [TestMethod]
        public void WhenStoreIsFilledBelowLowThreshold_ThenShouldNotScavengeAnyMore()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedSize == 5000 && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsFalse(strategy.ShouldScavengeMore(entries));
        }

        [TestMethod]
        public void WhenQueriedForCandidates_ThenReturnsListOrderedByPriority()
        {
            var strategy = new IsolatedStorageSizeScavengingStrategy(Mock.Of<ICacheEntryStore>(), Mock.Of<IIsolatedStorageInfo>(), HighScavengeThreshold, LowScavengeThreshold);
            var entries = new[] 
            { 
                new IsolatedStorageCacheEntry("second", 4, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable }),
                new IsolatedStorageCacheEntry("first", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default })
            };

            var actual = strategy.EntriesToScavenge(entries).ToList();

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("first", actual[0].Key);
            Assert.AreEqual("second", actual[1].Key);
        }

        [TestMethod]
        public void WhenQueriedForCandidates_ThenReturnsListOrderedByPriorityAndLastAccessTime()
        {
            var strategy = new IsolatedStorageSizeScavengingStrategy(Mock.Of<ICacheEntryStore>(), Mock.Of<IIsolatedStorageInfo>(), HighScavengeThreshold, LowScavengeThreshold);
            var now = DateTimeOffset.Now;
            var entries = new[] 
            { 
                new IsolatedStorageCacheEntry("third", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default }) { LastAccessTime = now },
                new IsolatedStorageCacheEntry("first", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default }) { LastAccessTime = now.AddMinutes(-20) },
                new IsolatedStorageCacheEntry("fourth", 4, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable }),
                new IsolatedStorageCacheEntry("second", 4, new CacheItemPolicy { Priority = CacheItemPriority.Default }) { LastAccessTime = now.AddMinutes(-10) },
            };

            var actual = strategy.EntriesToScavenge(entries).ToList();

            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual("first", actual[0].Key);
            Assert.AreEqual("second", actual[1].Key);
            Assert.AreEqual("third", actual[2].Key);
            Assert.AreEqual("fourth", actual[3].Key);
        }

        [TestMethod]
        public void WhenIsolatedStorageFreeSpaceIsFilledAboveThreshold_ThenShouldSuggestScavenge()
        {
            var isoMaxSize = 10000;
            var cacheUsedSize = 9000;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedSize == cacheUsedSize && x.UsedPhysicalSize == cacheUsedSize && x.Quota == int.MaxValue && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenIsolatedStorageFreeSpaceIsFilledBelowThreshold_ThenShouldNotScavenge()
        {
            var isoMaxSize = 10000;
            var cacheUsedSize = 7000;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedSize == cacheUsedSize && x.UsedPhysicalSize == cacheUsedSize && x.Quota == int.MaxValue && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenFinishingScavenging_ThenInvokesCompactOnStore()
        {
            var isoMaxSize = 10000;
            var cacheUsedPhysicalSize = 9000;
            var cacheUsedLogicalSize = 1000;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedSize == cacheUsedLogicalSize && x.UsedPhysicalSize == cacheUsedPhysicalSize && x.Quota == int.MaxValue && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedPhysicalSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            strategy.OnFinishingScavenging(entries);

            Mock.Get(store).Verify(x => x.Compact(), Times.Once());
        }

        [TestMethod]
        public void WhenFinishingScavengingWithFreeSpaceAboveThreshold_ThenDoesNotCompactStore()
        {
            var isoMaxSize = 10000;
            var cacheUsedPhysicalSize = 5000;
            var cacheUsedLogicalSize = 1000;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedSize == cacheUsedLogicalSize && x.UsedPhysicalSize == cacheUsedPhysicalSize && x.Quota == int.MaxValue && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedPhysicalSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            strategy.OnFinishingScavenging(entries);

            Mock.Get(store).Verify(x => x.Compact(), Times.Never());
        }

        [TestMethod]
        public void WhenFinishingScavengingAndCompactReturnsMappings_ThenUpdatesStorageIdOnEntries()
        {
            var isoMaxSize = 10000;
            var cacheUsedPhysicalSize = 9000;
            var cacheUsedLogicalSize = 1000;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedSize == cacheUsedLogicalSize && x.UsedPhysicalSize == cacheUsedPhysicalSize && x.Quota == int.MaxValue && x.IsEnabled == true);
            Mock.Get(store).Setup(x => x.Compact()).Returns(new Dictionary<int,int> { { 5, 1 } });
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedPhysicalSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) { StorageId = 5 } } };

            strategy.OnFinishingScavenging(entries);

            Assert.AreEqual(1, entries.Values.First().StorageId);
        }

        [TestMethod]
        public void WhenShouldCompact_ThenSuggestScavenge()
        {
            var isoMaxSize = 100000;
            var cacheUsedPhysicalSize = 80100;
            var cacheUsedLogicalSize = 10000;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedSize == cacheUsedLogicalSize && x.UsedPhysicalSize == cacheUsedPhysicalSize && x.Quota == int.MaxValue && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedPhysicalSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenUsedSizeCloseToPhysicalSize_ThenDoesNotSuggestScavenge()
        {
            var isoMaxSize = 100000;
            var cacheUsedPhysicalSize = 80100;
            var cacheUsedLogicalSize = 79900;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedSize == cacheUsedLogicalSize && x.UsedPhysicalSize == cacheUsedPhysicalSize && x.Quota == int.MaxValue && x.IsEnabled == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedPhysicalSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, Mock.Of<IExtendedCacheItemPolicy>()) } };

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }
    }
}
