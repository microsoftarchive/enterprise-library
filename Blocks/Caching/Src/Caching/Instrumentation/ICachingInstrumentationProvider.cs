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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// This interface defines the instrumentation events that can be raised from a
    /// <see cref="CacheManager"/>.
    /// </summary>
    public interface ICachingInstrumentationProvider
    {
        /// <summary>
        /// Fires the CacheUpdated event - reported when items added or
        /// removed from the cache.
        /// </summary>
        /// <param name="updatedEntriesCount">The number of entries updated.</param>
        /// <param name="totalEntriesCount">The total number of entries in cache.</param>
        void FireCacheUpdated(long updatedEntriesCount, long totalEntriesCount);

        /// <summary>
        /// Fires the CacheAccessed event - reported when an item is retrieved from the
        /// cache, or if an item was requested but not found.
        /// </summary>
        /// <param name="key">The key which was used to access the cache.</param>
        /// <param name="hit"><code>true</code> if accessing the cache was successful</param>
        void FireCacheAccessed(string key, bool hit);

        /// <summary>
        /// Fires the CacheExpired event - reported when items are expired from the cache.
        /// </summary>
        /// <param name="itemsExpired">The number of items that are expired.</param>
        void FireCacheExpired(long itemsExpired);

        /// <summary>
        /// Fires the CacheScavenged event - reported when the cache is scavenged.
        /// </summary>
        /// <param name="itemsScavenged">The number of items scavenged from cache.</param>
        void FireCacheScavenged(long itemsScavenged);

        /// <summary>
        /// Fires the CacheCallbackFailed event - reported when an exception occurs during
        /// a cache callback.
        /// </summary>
        /// <param name="key">The key that was used accessing the <see cref="CacheManager"/> when this failure occurred.</param>
        /// <param name="exception">The exception causing the failure.</param>
        void FireCacheCallbackFailed(string key, Exception exception);

        /// <summary>
        /// Fires the CacheFailed event - reported when an exception is thrown during a cache operation.
        /// </summary>
        /// <param name="errorMessage">The message that describes the failure.</param>
        /// <param name="exception">The message that represents the exception causing the failure.</param>
        void FireCacheFailed(string errorMessage, Exception exception);
    }
}
