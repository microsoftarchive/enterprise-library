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
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    /// <summary>
    /// Represents a set of eviction and expiration details for a specific cache entry.
    /// </summary>
    public class CacheItemPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItemPolicy"/> class.
        /// </summary>
        public CacheItemPolicy()
        {
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            SlidingExpiration = ObjectCache.NoSlidingExpiration;
            Priority = CacheItemPriority.Default;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a cache entry should be evicted after a specified duration.
        /// </summary>
        /// <returns>The period of time that must pass before a cache entry is evicted. The default
        ///     value is System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration, meaning
        ///     that the entry does not expire.</returns>
        public DateTimeOffset AbsoluteExpiration { get; set; }

        /// <summary>
        /// Gets or sets a priority setting that is used to determine whether to evict a cache entry.
        /// </summary>
        /// <returns>One of the enumeration values that indicates the priority for eviction. The
        ///     default priority value is System.Runtime.Caching.CacheItemPriority.Default,
        ///     which means no priority.</returns>
        public CacheItemPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether a cache entry should be evicted if it has not been accessed in a given span of time.
        /// </summary>
        /// <returns>A span of time within which a cache entry must be accessed before the cache entry is 
        /// evicted from the cache. The default is System.Runtime.Caching.ObjectCache.NoSlidingExpiration,
        /// meaning that the item should not be expired based on a time span.
        /// </returns>
        public TimeSpan SlidingExpiration { get; set; }

        /// <summary>
        /// Gets or sets a reference to a <see cref="CacheEntryRemovedCallback"/> delegate that is called after an entry is removed from the cache.
        /// </summary>
        /// <returns>A reference to a delegate that is called by a cache implementation.</returns>
        [IgnoreDataMember]
        public CacheEntryRemovedCallback RemovedCallback { get; set; }

        /// <summary>
        /// Gets or sets a reference to a <see cref="CacheEntryUpdateCallback"/>
        /// delegate that is called before a cache entry is removed from the cache.
        /// </summary>
        /// <returns>A reference to a delegate that is called by a cache implementation.</returns>
        [IgnoreDataMember]
        public CacheEntryUpdateCallback UpdateCallback { get; set; }
    }
}
