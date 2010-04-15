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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
    [TestClass]
    public class CachingPerformanceCountersFixture
    {
        const string instanceName = "test";
        string formattedInstanceName;

        NoPrefixNameFormatter formatter;
        PerformanceCounter cacheHitsCounter;
        PerformanceCounter cacheMissesCounter;
        PerformanceCounter cacheHitRatioCounter;
        PerformanceCounter cacheAccessAttemptsCounter;
        PerformanceCounter cacheExpiriesCounter;
        PerformanceCounter cacheScavengedItemsCounter;
        PerformanceCounter cacheTotalEntriesCounter;
        PerformanceCounter cacheUpdatedEntriesCounter;

        PerformanceCounter totalCacheExpiriesCounter;
        PerformanceCounter totalCacheHitsCounter;
        PerformanceCounter totalCacheMissesCounter;
        PerformanceCounter totalCacheScavengedItemsCounter;
        PerformanceCounter totalUpdatedEntriesItemsCounter;

        ICachingInstrumentationProvider enabledProvider;

        [TestInitialize]
        public void SetUp()
        {
            formatter = new NoPrefixNameFormatter();
            formattedInstanceName = formatter.CreateName(instanceName);

            enabledProvider = new CachingInstrumentationProvider(instanceName, true, true, formatter);

            cacheHitsCounter = CreatePerformanceCounter("Cache Hits/sec", formattedInstanceName);
            cacheHitsCounter.RawValue = 0;
            cacheMissesCounter = CreatePerformanceCounter("Cache Misses/sec", formattedInstanceName);
            cacheMissesCounter.RawValue = 0;
            cacheHitRatioCounter = CreatePerformanceCounter("Cache Hit Ratio", formattedInstanceName);
            cacheHitRatioCounter.RawValue = 0;
            cacheAccessAttemptsCounter = CreatePerformanceCounter("Total # of Cache Access Attempts", formattedInstanceName);
            cacheAccessAttemptsCounter.RawValue = 0;

            cacheExpiriesCounter = CreatePerformanceCounter("Cache Expiries/sec", formattedInstanceName);
            cacheExpiriesCounter.RawValue = 0;

            cacheScavengedItemsCounter = CreatePerformanceCounter("Cache Scavenged Items/sec", formattedInstanceName);
            cacheScavengedItemsCounter.RawValue = 0;

            cacheTotalEntriesCounter = CreatePerformanceCounter("Total Cache Entries", formattedInstanceName);
            cacheTotalEntriesCounter.RawValue = 0;
            cacheUpdatedEntriesCounter = CreatePerformanceCounter("Updated Entries/sec", formattedInstanceName);
            cacheUpdatedEntriesCounter.RawValue = 0;

            totalCacheExpiriesCounter = CreatePerformanceCounter(CachingInstrumentationProvider.TotalCacheExpiriesCounterName, formattedInstanceName);
            totalCacheExpiriesCounter.RawValue = 0;
            totalCacheHitsCounter = CreatePerformanceCounter(CachingInstrumentationProvider.TotalCacheHitsCounterName, formattedInstanceName);
            totalCacheHitsCounter.RawValue = 0;
            totalCacheMissesCounter = CreatePerformanceCounter(CachingInstrumentationProvider.TotalCacheMissesCounterName, formattedInstanceName);
            totalCacheMissesCounter.RawValue = 0;
            totalCacheScavengedItemsCounter = CreatePerformanceCounter(CachingInstrumentationProvider.TotalCacheScavengedItemsCounterName, formattedInstanceName);
            totalCacheScavengedItemsCounter.RawValue = 0;
            totalUpdatedEntriesItemsCounter = CreatePerformanceCounter(CachingInstrumentationProvider.TotalUpdatedEntriesItemsCounterName, formattedInstanceName);
            totalUpdatedEntriesItemsCounter.RawValue = 0;
        }

        [TestCleanup]
        public void TearDown()
        {
            cacheHitsCounter.Dispose();
            cacheMissesCounter.Dispose();
            cacheHitRatioCounter.Dispose();
            cacheAccessAttemptsCounter.Dispose();

            cacheExpiriesCounter.Dispose();

            cacheScavengedItemsCounter.Dispose();

            cacheTotalEntriesCounter.Dispose();
            cacheUpdatedEntriesCounter.Dispose();

            totalCacheExpiriesCounter.Dispose();
            totalCacheHitsCounter.Dispose();
            totalCacheMissesCounter.Dispose();
            totalCacheScavengedItemsCounter.Dispose();
            totalUpdatedEntriesItemsCounter.Dispose();
        }

        static PerformanceCounter CreatePerformanceCounter(string name,
                                                           string formattedInstanceName)
        {
            return new PerformanceCounter(CachingInstrumentationProvider.CounterCategoryName, name, formattedInstanceName, false);
        }

        [TestMethod]
        public void TotalCacheExpiriesCounterIncremented()
        {
            enabledProvider.FireCacheExpired(30L);

            Assert.AreEqual(30L, totalCacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void TotalCacheHitsCounterIncremented()
        {
            enabledProvider.FireCacheAccessed("key", true);

            Assert.AreEqual(1L, totalCacheHitsCounter.RawValue);
        }

        [TestMethod]
        public void TotalCacheMissesCounterIncremented()
        {
            enabledProvider.FireCacheAccessed("key", false);

            Assert.AreEqual(1L, totalCacheMissesCounter.RawValue);
        }

        [TestMethod]
        public void TotalCacheScavengedItemsCounterIncremented()
        {
            enabledProvider.FireCacheScavenged(30L);

            Assert.AreEqual(30L, totalCacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void TotalUpdatedEntriesItemsCounterIncremented()
        {
            enabledProvider.FireCacheUpdated(5L, 10L);

            Assert.AreEqual(5L, totalUpdatedEntriesItemsCounter.RawValue);
        }

        [TestMethod]
        public void CacheAccessWithPerformanceCountersDisabledDoesnNotUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, false, formatter);

            provider.FireCacheAccessed("key", true);

            Assert.AreEqual(0L, cacheHitsCounter.RawValue);
            Assert.AreEqual(0L, cacheMissesCounter.RawValue);
            Assert.AreEqual(0f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(0L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void CacheHitWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            provider.FireCacheAccessed("key", true);

            Assert.AreEqual(1L, cacheHitsCounter.RawValue);
            Assert.AreEqual(0L, cacheMissesCounter.RawValue);
            Assert.AreEqual(100f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(1L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void CacheMissWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            provider.FireCacheAccessed("key", false);

            Assert.AreEqual(0L, cacheHitsCounter.RawValue);
            Assert.AreEqual(1L, cacheMissesCounter.RawValue);
            Assert.AreEqual(0f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(1L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheAccessWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            for (int i = 0; i < 10; i++)
            {
                provider.FireCacheAccessed("key", true);
                provider.FireCacheAccessed("key", false);
                provider.FireCacheAccessed("key", true);
            }

            Assert.AreEqual(20L, cacheHitsCounter.RawValue);
            Assert.AreEqual(10L, cacheMissesCounter.RawValue);
            Assert.AreEqual(66.666f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(30L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void CacheExpirationWithPerformanceCountersDisabledDoesnNotUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, false, formatter);

            provider.FireCacheExpired(10);

            Assert.AreEqual(0L, cacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void CacheExpirationWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            provider.FireCacheExpired(10);

            Assert.AreEqual(10L, cacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheExpirationsWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            for (int i = 1; i <= 10; i++)
            {
                provider.FireCacheExpired(10);
            }

            Assert.AreEqual(100L, cacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void CacheScavengingWithPerformanceCountersDisabledDoesnNotUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, false, formatter);

            provider.FireCacheScavenged(10);

            Assert.AreEqual(0L, cacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void CacheScavengingWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            provider.FireCacheScavenged(10);

            Assert.AreEqual(10L, cacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheScavengingsWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            for (int i = 1; i <= 10; i++)
            {
                provider.FireCacheScavenged(10);
            }

            Assert.AreEqual(100L, cacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void CacheUpdateWithPerformanceCountersDisabledDoesNotUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, false, false, formatter);

            provider.FireCacheUpdated(10, 20);

            Assert.AreEqual(0L, cacheTotalEntriesCounter.RawValue);
            Assert.AreEqual(0L, cacheUpdatedEntriesCounter.RawValue);
        }

        [TestMethod]
        public void CacheUpdateWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            provider.FireCacheUpdated(10, 20);

            Assert.AreEqual(20L, cacheTotalEntriesCounter.RawValue);
            Assert.AreEqual(10L, cacheUpdatedEntriesCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheUpdatesWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            ICachingInstrumentationProvider provider
                = new CachingInstrumentationProvider(instanceName, true, false, formatter);

            for (int i = 1; i <= 10; i++)
            {
                provider.FireCacheUpdated(i, 10 + i);
            }

            Assert.AreEqual(20L, cacheTotalEntriesCounter.RawValue);
            Assert.AreEqual(55L, cacheUpdatedEntriesCounter.RawValue); // 55 = 10 * (10+1) / 2
        }
    }
}
