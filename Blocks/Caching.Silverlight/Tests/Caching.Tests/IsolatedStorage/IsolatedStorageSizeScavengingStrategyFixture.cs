using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage
{
    [TestClass]
    public class IsolatedStorageSizeScavengingStrategyFixture
    {
        private const int HighScavengeThreshold = 80;
        private const int LowScavengeThreshold = 60;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenZeroIsUsedForHighThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, 0, LowScavengeThreshold);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenValueLargerThan1IsUsedForHighThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, 101, LowScavengeThreshold);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenZeroIsUsedForLowThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenValueLargerThanHighIsUsedForLowThreshold_ThenThrows()
        {
            var store = Mock.Of<ICacheEntryStore>();
            var isoStore = Mock.Of<IIsolatedStorageInfo>();

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, 50, 60);
        }

        [TestMethod]
        public void WhenStoreIsEmpty_ThenShouldNotScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedPhysicalSize == 0 && x.IsWritable == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>();

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }
        
        [TestMethod]
        public void WhenStoreIsFilledAboveThreshold_ThenShouldSuggestScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedPhysicalSize == 9000 && x.IsWritable == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>{ { "key", new IsolatedStorageCacheEntry("key", 4, new CacheItemPolicy()) }};

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsFilledBelowHighThreshold_ThenShouldNotScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedPhysicalSize == 7000 && x.IsWritable == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, new CacheItemPolicy()) } };

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsFilledAboveLowThreshold_ThenShouldSuggestScavengeMore()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedPhysicalSize == 7000 && x.IsWritable == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, new CacheItemPolicy()) } };

            Assert.IsTrue(strategy.ShouldScavengeMore(entries));
        }

        [TestMethod]
        public void WhenStoreIsFilledBelowLowThreshold_ThenShouldNotScavengeAnyMore()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.Quota == 10000 && x.UsedPhysicalSize == 5000 && x.IsWritable == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == int.MaxValue);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, new CacheItemPolicy()) } };

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
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedPhysicalSize == cacheUsedSize && x.Quota == int.MaxValue && x.IsWritable == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, new CacheItemPolicy()) } };

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenIsolatedStorageFreeSpaceIsFilledBelowThreshold_ThenShouldNotScavenge()
        {
            var isoMaxSize = 10000;
            var cacheUsedSize = 7000;
            var store = Mock.Of<ICacheEntryStore>(x => x.UsedPhysicalSize == cacheUsedSize && x.Quota == int.MaxValue && x.IsWritable == true);
            var isoStore = Mock.Of<IIsolatedStorageInfo>(x => x.AvailableFreeSpace == isoMaxSize - cacheUsedSize);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, isoStore, HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry> { { "key", new IsolatedStorageCacheEntry("key", 4, new CacheItemPolicy()) } };

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsReadOnlyAndThereAreLessThan20Items_ThenShouldNotScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.IsWritable == false);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, Mock.Of<IIsolatedStorageInfo>(), HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>();
            Assert.IsFalse(strategy.ShouldScavenge(entries));

            for (int i = 0; i < 20; i++)
            {
                entries.Add(i.ToString(), new IsolatedStorageCacheEntry(i.ToString(), i, new CacheItemPolicy()));
            }

            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsReadOnlyAndThereAreMoreThan20Items_ThenShouldScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.IsWritable == false);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, Mock.Of<IIsolatedStorageInfo>(), HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>();
            Assert.IsFalse(strategy.ShouldScavenge(entries));
            for (int i = 0; i < 21; i++)
            {
                entries.Add(i.ToString(), new IsolatedStorageCacheEntry(i.ToString(), i, new CacheItemPolicy()));
            }

            Assert.IsTrue(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsReadOnlyAndThereAreLessThanTheAmountOfItemsThatWereInTheFirstCallAbove20_ThenShouldNotScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.IsWritable == false);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, Mock.Of<IIsolatedStorageInfo>(), HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>();

            for (int i = 0; i < 25; i++)
            {
                entries.Add(i.ToString(), new IsolatedStorageCacheEntry(i.ToString(), i, new CacheItemPolicy()));
            }

            Assert.IsFalse(strategy.ShouldScavenge(entries));
            Assert.IsFalse(strategy.ShouldScavenge(entries));
            entries.Remove("0");
            Assert.IsFalse(strategy.ShouldScavenge(entries));
            entries.Add("new", new IsolatedStorageCacheEntry("new", 25, new CacheItemPolicy()));
            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsReadOnlyAndThereAreMoreThanTheAmountOfItemsThatWereInTheFirstCallAbove20_ThenShouldScavenge()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.IsWritable == false);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, Mock.Of<IIsolatedStorageInfo>(), HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>();
            for (int i = 0; i < 25; i++)
            {
                entries.Add(i.ToString(), new IsolatedStorageCacheEntry(i.ToString(), i, new CacheItemPolicy()));
            }

            Assert.IsFalse(strategy.ShouldScavenge(entries));
            entries.Add("new", new IsolatedStorageCacheEntry("new", 26, new CacheItemPolicy()));
            Assert.IsTrue(strategy.ShouldScavenge(entries));
            entries.Remove("0");
            Assert.IsFalse(strategy.ShouldScavenge(entries));
        }

        [TestMethod]
        public void WhenStoreIsReadOnlyAndThereAreMoreThanTheAmountOfItemsThatWereInTheFirstCallAbove20_ThenShouldScavengeMore()
        {
            var store = Mock.Of<ICacheEntryStore>(x => x.IsWritable == false);

            var strategy = new IsolatedStorageSizeScavengingStrategy(store, Mock.Of<IIsolatedStorageInfo>(), HighScavengeThreshold, LowScavengeThreshold);
            var entries = new Dictionary<string, IsolatedStorageCacheEntry>();
            for (int i = 0; i < 25; i++)
            {
                entries.Add(i.ToString(), new IsolatedStorageCacheEntry(i.ToString(), i, new CacheItemPolicy()));
            }

            Assert.IsFalse(strategy.ShouldScavenge(entries));
            entries.Add("new", new IsolatedStorageCacheEntry("new", 26, new CacheItemPolicy()));
            Assert.IsTrue(strategy.ShouldScavenge(entries));
            Assert.IsTrue(strategy.ShouldScavengeMore(entries));
            entries.Remove("0");
            Assert.IsFalse(strategy.ShouldScavengeMore(entries));
        }
    }
}
