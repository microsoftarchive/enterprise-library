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

        CachingInstrumentationListener enabledListener;

        [TestInitialize]
        public void SetUp()
        {
            formatter = new NoPrefixNameFormatter();
            formattedInstanceName = formatter.CreateName(instanceName);

            enabledListener = new CachingInstrumentationListener(instanceName, true, true, true, formatter);

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

            totalCacheExpiriesCounter = CreatePerformanceCounter(CachingInstrumentationListener.TotalCacheExpiriesCounterName, formattedInstanceName);
            totalCacheExpiriesCounter.RawValue = 0;
            totalCacheHitsCounter = CreatePerformanceCounter(CachingInstrumentationListener.TotalCacheHitsCounterName, formattedInstanceName);
            totalCacheHitsCounter.RawValue = 0;
            totalCacheMissesCounter = CreatePerformanceCounter(CachingInstrumentationListener.TotalCacheMissesCounterName, formattedInstanceName);
            totalCacheMissesCounter.RawValue = 0;
            totalCacheScavengedItemsCounter = CreatePerformanceCounter(CachingInstrumentationListener.TotalCacheScavengedItemsCounterName, formattedInstanceName);
            totalCacheScavengedItemsCounter.RawValue = 0;
            totalUpdatedEntriesItemsCounter = CreatePerformanceCounter(CachingInstrumentationListener.TotalUpdatedEntriesItemsCounterName, formattedInstanceName);
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
            return new PerformanceCounter(CachingInstrumentationListener.CounterCategoryName, name, formattedInstanceName, false);
        }

        [TestMethod]
        public void TotalCacheExpiriesCounterIncremented()
        {
            enabledListener.CacheExpired(this, new CacheExpiredEventArgs(30L));

            Assert.AreEqual(30L, totalCacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void TotalCacheHitsCounterIncremented()
        {
            enabledListener.CacheAccessed(this, new CacheAccessedEventArgs("key", true));

            Assert.AreEqual(1L, totalCacheHitsCounter.RawValue);
        }

        [TestMethod]
        public void TotalCacheMissesCounterIncremented()
        {
            enabledListener.CacheAccessed(this, new CacheAccessedEventArgs("key", false));

            Assert.AreEqual(1L, totalCacheMissesCounter.RawValue);
        }

        [TestMethod]
        public void TotalCacheScavengedItemsCounterIncremented()
        {
            enabledListener.CacheScavenged(this, new CacheScavengedEventArgs(30L));

            Assert.AreEqual(30L, totalCacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void TotalUpdatedEntriesItemsCounterIncremented()
        {
            enabledListener.CacheUpdated(this, new CacheUpdatedEventArgs(5L, 10L));

            Assert.AreEqual(5L, totalUpdatedEntriesItemsCounter.RawValue);
        }

        [TestMethod]
        public void CacheAccessWithPerformanceCountersDisabledDoesnNotUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);

            CacheAccessedEventArgs args = new CacheAccessedEventArgs("key", true);

            listener.CacheAccessed(null, args);

            Assert.AreEqual(0L, cacheHitsCounter.RawValue);
            Assert.AreEqual(0L, cacheMissesCounter.RawValue);
            Assert.AreEqual(0f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(0L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void CacheHitWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            CacheAccessedEventArgs args = new CacheAccessedEventArgs("key", true);

            listener.CacheAccessed(null, args);

            Assert.AreEqual(1L, cacheHitsCounter.RawValue);
            Assert.AreEqual(0L, cacheMissesCounter.RawValue);
            Assert.AreEqual(100f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(1L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void CacheMissWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            CacheAccessedEventArgs args = new CacheAccessedEventArgs("key", false);

            listener.CacheAccessed(null, args);

            Assert.AreEqual(0L, cacheHitsCounter.RawValue);
            Assert.AreEqual(1L, cacheMissesCounter.RawValue);
            Assert.AreEqual(0f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(1L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheAccessWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            CacheAccessedEventArgs hitArgs = new CacheAccessedEventArgs("key", true);
            CacheAccessedEventArgs missArgs = new CacheAccessedEventArgs("key", false);

            for (int i = 0; i < 10; i++)
            {
                listener.CacheAccessed(null, hitArgs);
                listener.CacheAccessed(null, missArgs);
                listener.CacheAccessed(null, hitArgs);
            }

            Assert.AreEqual(20L, cacheHitsCounter.RawValue);
            Assert.AreEqual(10L, cacheMissesCounter.RawValue);
            Assert.AreEqual(66.666f, cacheHitRatioCounter.NextValue(), 0.01f);
            Assert.AreEqual(30L, cacheAccessAttemptsCounter.RawValue);
        }

        [TestMethod]
        public void CacheExpirationWithPerformanceCountersDisabledDoesnNotUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);

            CacheExpiredEventArgs args = new CacheExpiredEventArgs(10);

            listener.CacheExpired(null, args);

            Assert.AreEqual(0L, cacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void CacheExpirationWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            CacheExpiredEventArgs args = new CacheExpiredEventArgs(10);

            listener.CacheExpired(null, args);

            Assert.AreEqual(10L, cacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheExpirationsWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            for (int i = 1; i <= 10; i++)
            {
                CacheExpiredEventArgs args = new CacheExpiredEventArgs(10);

                listener.CacheExpired(null, args);
            }

            Assert.AreEqual(100L, cacheExpiriesCounter.RawValue);
        }

        [TestMethod]
        public void CacheScavengingWithPerformanceCountersDisabledDoesnNotUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);

            CacheScavengedEventArgs args = new CacheScavengedEventArgs(10);

            listener.CacheScavenged(null, args);

            Assert.AreEqual(0L, cacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void CacheScavengingWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            CacheScavengedEventArgs args = new CacheScavengedEventArgs(10);

            listener.CacheScavenged(null, args);

            Assert.AreEqual(10L, cacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheScavengingsWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            for (int i = 1; i <= 10; i++)
            {
                CacheScavengedEventArgs args = new CacheScavengedEventArgs(10);

                listener.CacheScavenged(null, args);
            }

            Assert.AreEqual(100L, cacheScavengedItemsCounter.RawValue);
        }

        [TestMethod]
        public void CacheUpdateWithPerformanceCountersDisabledDoesNotUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);

            CacheUpdatedEventArgs args = new CacheUpdatedEventArgs(10, 20);

            listener.CacheUpdated(null, args);

            Assert.AreEqual(0L, cacheTotalEntriesCounter.RawValue);
            Assert.AreEqual(0L, cacheUpdatedEntriesCounter.RawValue);
        }

        [TestMethod]
        public void CacheUpdateWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            CacheUpdatedEventArgs args = new CacheUpdatedEventArgs(10, 20);

            listener.CacheUpdated(null, args);

            Assert.AreEqual(20L, cacheTotalEntriesCounter.RawValue);
            Assert.AreEqual(10L, cacheUpdatedEntriesCounter.RawValue);
        }

        [TestMethod]
        public void MultipleCacheUpdatesWithPerformanceCountersEnabledDoesUpdateCounters()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, true, false, false, formatter);

            for (int i = 1; i <= 10; i++)
            {
                CacheUpdatedEventArgs args = new CacheUpdatedEventArgs(i, 10 + i);

                listener.CacheUpdated(null, args);
            }

            Assert.AreEqual(20L, cacheTotalEntriesCounter.RawValue);
            Assert.AreEqual(55L, cacheUpdatedEntriesCounter.RawValue); // 55 = 10 * (10+1) / 2
        }
    }
}
