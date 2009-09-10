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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// Implementation of <see cref="ICachingInstrumentationProvider"/> that generates
    /// performance counter updates and WMI events in response to instrumentation activities.
    /// </summary>
    [HasInstallableResourcesAttribute]
    [PerformanceCountersDefinition(CounterCategoryName, "CounterCategoryHelpResourceName")]
    [EventLogDefinition("Application", EventLogSourceName)]
    public class CachingInstrumentationProvider : InstrumentationListener, ICachingInstrumentationProvider
    {
        /// <summary>
        /// The name of the caching counters.
        /// </summary>
        public const string CounterCategoryName = "Enterprise Library Caching Counters";

        /// <summary>
        /// The name of the event log source.
        /// </summary>
        public const string EventLogSourceName = "Enterprise Library Caching";

        /// <summary>
        /// The total cache expires counter name.
        /// </summary>
        public const string TotalCacheExpiriesCounterName = "Total Cache Expiries";

        /// <summary>
        /// The total cache hits counter name.
        /// </summary>
        public const string TotalCacheHitsCounterName = "Total Cache Hits";

        /// <summary>
        /// The total cache misses counter name.
        /// </summary>
        public const string TotalCacheMissesCounterName = "Total Cache Misses";

        /// <summary>
        /// The total cache scavenged items counter name.
        /// </summary>
        public const string TotalCacheScavengedItemsCounterName = "Total Cache Scavenged Items";

        /// <summary>
        /// The total updated entries counter name.
        /// </summary>
        public const string TotalUpdatedEntriesItemsCounterName = "Total Updated Entries";
        static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

        EnterpriseLibraryPerformanceCounter cacheAccessAttemptsCounter;

        [PerformanceCounter("Cache Expiries/sec", "CacheExpiriesPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheExpiriesCounter;

        [PerformanceCounter("Cache Hit Ratio", "CacheHitRatioCounterHelpResource", PerformanceCounterType.RawFraction,
            BaseCounterName = "Total # of Cache Access Attempts", BaseCounterHelp = "CacheAccessAttemptsCounterHelpResource", BaseCounterType = PerformanceCounterType.RawBase)]
        EnterpriseLibraryPerformanceCounter cacheHitRatioCounter;

        [PerformanceCounter("Cache Hits/sec", "CacheHitsPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheHitsCounter;

        [PerformanceCounter("Cache Misses/sec", "CacheMissesPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheMissesCounter;

        [PerformanceCounter("Cache Scavenged Items/sec", "CacheScavengedItemsPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheScavengedItemsCounter;

        [PerformanceCounter("Total Cache Entries", "CacheTotalEntriesCounterHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter cacheTotalEntriesCounter;

        [PerformanceCounter("Updated Entries/sec", "CacheUpdatedEntriesPerSecHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheUpdatedEntriesCounter;

        string counterInstanceName;
        IEventLogEntryFormatter eventLogEntryFormatter;
        string instanceName;

        [PerformanceCounter(TotalCacheExpiriesCounterName, "TotalCacheExpiriesCounterHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter totalCacheExpiriesCounter;

        [PerformanceCounter(TotalCacheHitsCounterName, "TotalCacheHitsCounterHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalCacheHitsCounter;

        [PerformanceCounter(TotalCacheMissesCounterName, "TotalCacheMissesCounterHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalCacheMissesCounter;

        [PerformanceCounter(TotalCacheScavengedItemsCounterName, "TotalCacheScavengedItemsCounterHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter totalCacheScavengedItemsCounter;

        [PerformanceCounter(TotalUpdatedEntriesItemsCounterName, "TotalCacheUpdatedEntriesHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter totalUpdatedEntriesItemsCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="CacheManager"/> instance this instrumentation listener is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
        public CachingInstrumentationProvider(string instanceName,
                                              bool performanceCountersEnabled,
                                              bool eventLoggingEnabled,
                                              bool wmiEnabled,
                                              string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName)) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="CacheManager"/> instance this instrumentation listener is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
        /// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        public CachingInstrumentationProvider(string instanceName,
                                              bool performanceCountersEnabled,
                                              bool eventLoggingEnabled,
                                              bool wmiEnabled,
                                              IPerformanceCounterNameFormatter nameFormatter)
            : base(new [] { instanceName }, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
        {
            this.instanceName = instanceName;
            counterInstanceName = CreateInstanceName(instanceName);

            eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
        }

        /// <summary>
        /// Fires the CacheUpdated event - reported when items added or
        /// removed from the cache.
        /// </summary>
        /// <param name="updatedEntriesCount">The number of entries updated.</param>
        /// <param name="totalEntriesCount">The total number of entries in cache.</param>
        public void FireCacheUpdated(long updatedEntriesCount, long totalEntriesCount)
        {
            if (PerformanceCountersEnabled)
            {
                cacheTotalEntriesCounter.SetValueFor(counterInstanceName, totalEntriesCount);
                cacheUpdatedEntriesCounter.IncrementBy(updatedEntriesCount);
                totalUpdatedEntriesItemsCounter.IncrementBy(updatedEntriesCount);
            }
        }

        /// <summary>
        /// Fires the CacheAccessed event - reported when an item is retrieved from the
        /// cache, or if an item was requested but not found.
        /// </summary>
        /// <param name="key">The key which was used to access the cache.</param>
        /// <param name="hit"><code>true</code> if accessing the cache was successful</param>
        public void FireCacheAccessed(string key, bool hit)
        {
            if (PerformanceCountersEnabled)
            {
                cacheAccessAttemptsCounter.Increment();
                if (hit)
                {
                    cacheHitRatioCounter.Increment();
                    cacheHitsCounter.Increment();
                    totalCacheHitsCounter.Increment();
                }
                else
                {
                    cacheMissesCounter.Increment();
                    totalCacheMissesCounter.Increment();
                }
            }
        }

        /// <summary>
        /// Fires the CacheExpired event - reported when items are expired from the cache.
        /// </summary>
        /// <param name="itemsExpired">The number of items that are expired.</param>
        public void FireCacheExpired(long itemsExpired)
        {
            if (PerformanceCountersEnabled)
            {
                cacheExpiriesCounter.IncrementBy(itemsExpired);
                totalCacheExpiriesCounter.IncrementBy(itemsExpired);
            }
        }

        /// <summary>
        /// Fires the CacheScavenged event - reported when the cache is scavenged.
        /// </summary>
        /// <param name="itemsScavenged">The number of items scavenged from cache.</param>
        public void FireCacheScavenged(long itemsScavenged)
        {
            if (PerformanceCountersEnabled)
            {
                cacheScavengedItemsCounter.IncrementBy(itemsScavenged);
                totalCacheScavengedItemsCounter.IncrementBy(itemsScavenged);
            }
            if (WmiEnabled)
            {
                FireManagementInstrumentation(new CacheScavengedEvent(instanceName, itemsScavenged));
            }
        }

        /// <summary>
        /// Fires the CacheCallbackFailed event - reported when an exception occurs during
        /// a cache callback.
        /// </summary>
        /// <param name="key">The key that was used accessing the <see cref="CacheManager"/> when this failure occurred.</param>
        /// <param name="exception">The exception causing the failure.</param>
        public void FireCacheCallbackFailed(string key, Exception exception)
        {
            if (WmiEnabled)
            {
                FireManagementInstrumentation(new CacheCallbackFailureEvent(instanceName, key, exception.ToString()));
            }
            if (EventLoggingEnabled)
            {
                string errorMessage
                    = string.Format(
                        Resources.Culture,
                        Resources.ErrorCacheCallbackFailedMessage,
                        instanceName,
                        key);
                string entryText = eventLogEntryFormatter.GetEntryText(errorMessage, exception);

                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Fires the CacheFailed event - reported when an exception is thrown during a cache operation.
        /// </summary>
        /// <param name="errorMessage">The message that describes the failure.</param>
        /// <param name="exception">The message that represents the exception causing the failure.</param>
        public void FireCacheFailed(string errorMessage, Exception exception)
        {
            if (WmiEnabled)
            {
                FireManagementInstrumentation(new CacheFailureEvent(instanceName, errorMessage, exception.ToString()));
            }
            if (EventLoggingEnabled)
            {
                string message
                    = string.Format(
                        Resources.Culture,
                        Resources.ErrorCacheOperationFailedMessage,
                        instanceName);
                string entryText = eventLogEntryFormatter.GetEntryText(message, exception, errorMessage);

                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Creates the performance counters to instrument the caching events for the specified instance names.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
        {
            cacheHitsCounter = factory.CreateCounter(CounterCategoryName, "Cache Hits/sec", instanceNames);
            totalCacheHitsCounter = factory.CreateCounter(CounterCategoryName, TotalCacheHitsCounterName, instanceNames);

            cacheMissesCounter = factory.CreateCounter(CounterCategoryName, "Cache Misses/sec", instanceNames);
            totalCacheMissesCounter = factory.CreateCounter(CounterCategoryName, TotalCacheMissesCounterName, instanceNames);

            cacheHitRatioCounter = factory.CreateCounter(CounterCategoryName, "Cache Hit Ratio", instanceNames);
            cacheAccessAttemptsCounter = factory.CreateCounter(CounterCategoryName, "Total # of Cache Access Attempts", instanceNames);

            cacheExpiriesCounter = factory.CreateCounter(CounterCategoryName, "Cache Expiries/sec", instanceNames);
            totalCacheExpiriesCounter = factory.CreateCounter(CounterCategoryName, TotalCacheExpiriesCounterName, instanceNames);

            cacheScavengedItemsCounter = factory.CreateCounter(CounterCategoryName, "Cache Scavenged Items/sec", instanceNames);
            totalCacheScavengedItemsCounter = factory.CreateCounter(CounterCategoryName, TotalCacheScavengedItemsCounterName, instanceNames);

            cacheTotalEntriesCounter = factory.CreateCounter(CounterCategoryName, "Total Cache Entries", instanceNames);
            cacheUpdatedEntriesCounter = factory.CreateCounter(CounterCategoryName, "Updated Entries/sec", instanceNames);
            totalUpdatedEntriesItemsCounter = factory.CreateCounter(CounterCategoryName, TotalUpdatedEntriesItemsCounterName, instanceNames);
        }
    }
}
