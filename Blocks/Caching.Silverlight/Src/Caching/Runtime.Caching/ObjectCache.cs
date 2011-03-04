using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    public abstract class ObjectCache : IEnumerable<KeyValuePair<string, object>>
    {
        // Fields
        public static readonly DateTimeOffset InfiniteAbsoluteExpiration = DateTimeOffset.MaxValue;
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

        // Methods
        
        public virtual bool Add(CacheItem item, CacheItemPolicy policy)
        {
            return AddOrGetExisting(item, policy) == null;
        }

        public virtual bool Add(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            return AddOrGetExisting(key, value, absoluteExpiration, regionName) == null;
        }

        public virtual bool Add(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            return AddOrGetExisting(key, value, policy, regionName) == null;
        }

        public abstract CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy);

        public abstract object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration,  string regionName = null);

        public abstract object AddOrGetExisting(string key, object value, CacheItemPolicy policy,  string regionName = null);

        public abstract bool Contains(string key,  string regionName = null);

        public abstract CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys,  string regionName = null);

        public abstract object Get(string key,  string regionName = null);

        public abstract CacheItem GetCacheItem(string key,  string regionName = null);

        public abstract long GetCount( string regionName = null);

        protected abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        public abstract IDictionary<string, object> GetValues(IEnumerable<string> keys,  string regionName = null);

        public virtual IDictionary<string, object> GetValues(string regionName = null, params string[] keys)
        {
            return GetValues(keys, regionName);
        }

        public abstract object Remove(string key, string regionName = null);

        public abstract void Set(CacheItem item, CacheItemPolicy policy);

        public abstract void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

        public abstract void Set(string key, object value, CacheItemPolicy policy, string regionName = null);

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
        }

        // Properties
        public abstract DefaultCacheCapabilities DefaultCacheCapabilities { get; }

        public abstract object this[string key] { get; set; }
        public abstract string Name { get; }
    }
}
