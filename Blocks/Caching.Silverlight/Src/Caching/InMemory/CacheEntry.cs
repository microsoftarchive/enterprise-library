using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    public class CacheEntry : CacheItem
    {
        public CacheEntry(string key, object value, CacheItemPolicy policy)
            : base(key, value, null)
        {
            Policy = policy;

            UpdateLastAccessTime();
        }

        public CacheItemPolicy Policy { get; private set; }
        public DateTimeOffset LastAccessTime { get; protected set; }
        public CacheItemPriority Priority { get { return Policy.Priority; } }

        public void UpdateLastAccessTime()
        {
            LastAccessTime = CachingTimeProvider.Now;
        }
    }
}