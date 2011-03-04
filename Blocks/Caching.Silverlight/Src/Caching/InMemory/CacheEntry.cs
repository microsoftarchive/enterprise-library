using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    public class CacheEntry : CacheItem
    {
        public CacheEntry(string key, object value, CacheItemPolicy policy)
            : base(key, value, null)
        {
            Policy = policy is IExtendedCacheItemPolicy
                ? (IExtendedCacheItemPolicy) policy
                : WrapCacheItemPolicy(policy);

            UpdateLastAccessTime();
        }

        public CacheEntry(string key, object value, IExtendedCacheItemPolicy policy)
            : base(key, value, null)
        {
            Policy = policy;

            UpdateLastAccessTime();
        }

        public IExtendedCacheItemPolicy Policy { get; private set; }
        public DateTimeOffset LastAccessTime { get; protected set; }
        public CacheItemPriority Priority { get { return Policy.Priority; } }

        public void UpdateLastAccessTime()
        {
            LastAccessTime = CachingTimeProvider.Now;
        }

        protected IExtendedCacheItemPolicy WrapCacheItemPolicy(CacheItemPolicy policy)
        {
            return new DefaultExtendedCacheItemPolicy(policy);
        }
    }
}