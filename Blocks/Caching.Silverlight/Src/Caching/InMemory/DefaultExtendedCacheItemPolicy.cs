using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    /// <summary>
    /// An implmentation if <see cref="IExtendedCacheItemPolicy"/>
    /// that wraps a <see cref="CacheItemPolicy"/> object.
    /// </summary>
    public class DefaultExtendedCacheItemPolicy : IExtendedCacheItemPolicy
    {
        public DefaultExtendedCacheItemPolicy()
        {
        }

        public DefaultExtendedCacheItemPolicy(CacheItemPolicy policy)
        {
            this.Policy = policy;
        }

        public bool IsExpired(DateTimeOffset lastAccessedTime)
        {
            var now = CachingTimeProvider.Now;

            bool isExpired = Policy.AbsoluteExpiration != ObjectCache.InfiniteAbsoluteExpiration &&
                now > Policy.AbsoluteExpiration;

            isExpired = isExpired ||
                (Policy.SlidingExpiration != ObjectCache.NoSlidingExpiration &&
                    now > lastAccessedTime + Policy.SlidingExpiration);
        
            return isExpired;
        }

        public CacheItemPriority Priority
        {
            get { return Policy.Priority; }
        }

        public CacheItemPolicy Policy
        {
            get;
            set;
        }
    }
}
