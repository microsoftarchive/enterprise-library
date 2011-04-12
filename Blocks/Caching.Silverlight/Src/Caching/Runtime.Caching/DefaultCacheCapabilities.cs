using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    /// <summary>
    /// Represents a set of features that a cache implementation provides.
    /// </summary>
    [Flags]
    public enum DefaultCacheCapabilities
    {
        /// <summary>
        /// A cache implementation does not provide any of the features that are described in the DefaultCacheCapabilities enumeration.
        /// </summary>
        None = 0,

        /// <summary>
        /// A cache implementation runs at least partially in memory.
        /// </summary>
        InMemoryProvider = 1,

        /// <summary>
        /// A cache implementation runs out-of-process.
        /// </summary>
        OutOfProcessProvider = 2,

        /// <summary>
        /// A cache implementation supports the ability to create change monitors that monitor entries.
        /// </summary>
        CacheEntryChangeMonitors = 4,

        /// <summary>
        /// A cache implementation supports the ability to automatically remove cache entries at a specific date and time.
        /// </summary>
        AbsoluteExpirations = 8,

        /// <summary>
        /// A cache implementation supports the ability to automatically remove cache entries that have not been accessed in a specified time span.
        /// </summary>
        SlidingExpirations = 0x10,

        /// <summary>
        /// A cache implementation can raise a notification that an entry is about to be removed from the cache. 
        /// This setting also indicates that a cache implementation supports the ability to automatically replace the entry 
        /// that is being removed with a new cache entry.
        /// </summary>
        CacheEntryUpdateCallback = 0x20,

        /// <summary>
        /// A cache implementation can raise a notification that an entry has been removed from the cache.
        /// </summary>
        CacheEntryRemovedCallback = 0x40,

        /// <summary>
        /// A cache implementation supports the ability to partition its storage into cache regions, and supports the 
        /// ability to insert cache entries into those regions and to retrieve cache entries from those regions.
        /// </summary>
        CacheRegions = 0x80,
    }
}
