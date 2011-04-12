using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    /// <summary>
    /// Represents an object cache and provides the base methods and properties for accessing the object cache.
    /// </summary>
    public abstract class ObjectCache : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Gets a value that indicates that a cache entry has no absolute expiration. 
        /// </summary>
        public static readonly DateTimeOffset InfiniteAbsoluteExpiration = DateTimeOffset.MaxValue;

        /// <summary>
        /// Indicates that a cache entry has no sliding expiration time.
        /// </summary>
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCache"/> class.
        /// </summary>
        protected ObjectCache()
        {
        }

        /// <summary>
        /// When overridden in a derived class, tries to insert a cache entry into the cache as a <see cref="CacheItem"/> 
        /// instance, and adds details about how the entry should be evicted.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>true if insertion succeeded, or false if there is an already an entry in the cache that has the same key as item.</returns>
        public virtual bool Add(CacheItem item, CacheItemPolicy policy)
        {
            return AddOrGetExisting(item, policy) == null;
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache without overwriting any existing cache entry. 
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if 
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>true if insertion succeeded, or false if there is an already an entry in the cache that has the same key as item.</returns>
        public virtual bool Add(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            return AddOrGetExisting(key, value, absoluteExpiration, regionName) == null;
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if 
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>true if insertion succeeded, or false if there is an already an entry in the cache that has the same key as item.</returns>
        public virtual bool Add(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            return AddOrGetExisting(key, value, policy, regionName) == null;
        }

        /// <summary>
        /// When overridden in a derived class, inserts the specified CacheItem object into the cache, specifying 
        /// information about how the entry will be evicted.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry; otherwise, <see langword="null"/>.</returns>
        public abstract CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, by using a key, an object for the 
        /// cache entry, an absolute expiration value, and an optional region to add the cache into.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if 
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry's value; otherwise, <see langword="null"/>.</returns>
        public abstract object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying a key and a value for 
        /// the cache entry, and information about how the entry will be evicted.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if 
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry's value; otherwise, <see langword="null"/>.</returns>
        public abstract object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, checks whether the cache entry already exists in the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>true if the cache contains a cache entry with the same key value as key; otherwise, false.</returns>
        public abstract bool Contains(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, creates a <see cref="CacheEntryChangeMonitor"/> object that can trigger 
        /// events in response to changes to specified cache entries. 
        /// </summary>
        /// <param name="keys">The unique identifiers for cache entries to monitor.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>A change monitor that monitors cache entries in the cache.</returns>
        public abstract CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, gets the specified cache entry from the cache as an object.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>The cache entry that is identified by key.</returns>
        public abstract object Get(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, gets the specified cache entry from the cache as a <see cref="CacheItem"/> instance.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>The cache item.</returns>
        public abstract CacheItem GetCacheItem(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, gets the total number of cache entries in the cache. 
        /// </summary>
        /// <param name="regionName">Optional. A named region in the cache for which the cache entry count should be computed, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>The number of cache entries in the cache. If regionName is not Nothing, the count indicates the 
        /// number of entries that are in the specified cache region. </returns>
        public abstract long GetCount(string regionName = null);

        /// <summary>
        /// When overridden in a derived class, creates an enumerator that can be used to iterate through a collection 
        /// of cache entries. 
        /// </summary>
        /// <returns>The enumerator object that provides access to the cache entries in the cache.</returns>
        protected abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        /// <summary>
        /// When overridden in a derived class, gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry or entries were 
        /// added, if regions are implemented. The default value for the optional parameter is <see langword="null"/>.</param>
        /// <returns>A dictionary of key/value pairs that represent cache entries.</returns>
        public abstract IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null);

        /// <summary>
        /// Gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry or entries were 
        /// added, if regions are implemented. The default value for the optional parameter is <see langword="null"/>.</param>
        /// <param name="keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <returns>A dictionary of key/value pairs that represent cache entries.</returns>
        public virtual IDictionary<string, object> GetValues(string regionName = null, params string[] keys)
        {
            return GetValues(keys, regionName);
        }

        /// <summary>
        /// When overridden in a derived class, removes the cache entry from the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>An object that represents the value of the removed cache entry that was specified by the key, 
        /// or <see langword="null"/> if the specified entry was not found.
        /// </returns>
        public abstract object Remove(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, inserts the cache entry into the cache as a CacheItem instance, 
        /// specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="item">The cache item to add.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides 
        /// more options for eviction than a simple absolute expiration.</param>
        public abstract void Set(CacheItem item, CacheItemPolicy policy);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying time-based expiration details.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if 
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        public abstract void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache. 
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if 
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        public abstract void Set(string key, object value, CacheItemPolicy policy, string regionName = null);

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
        }

        /// <summary>
        /// When overridden in a derived class, gets a description of the features that a cache implementation provides.
        /// </summary>
        public abstract DefaultCacheCapabilities DefaultCacheCapabilities { get; }

        /// <summary>
        /// Gets the name of the ObjectCache instance.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the default indexer for the ObjectCache class.
        /// </summary>
        public abstract object this[string key] { get; set; }
    }
}
