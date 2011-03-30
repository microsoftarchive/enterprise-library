using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    public static class CacheItemPolicyExtensions
    {
        public static bool IsExpired(this CacheItemPolicy policy, DateTimeOffset lastAccessedTime)
        {
            var now = CachingTimeProvider.Now;

            bool isExpired = policy.AbsoluteExpiration != ObjectCache.InfiniteAbsoluteExpiration &&
                now > policy.AbsoluteExpiration;

            isExpired = isExpired ||
                (policy.SlidingExpiration != ObjectCache.NoSlidingExpiration &&
                    now > lastAccessedTime + policy.SlidingExpiration);

            return isExpired;
        }

        public static void Validate(this CacheItemPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException("policy");
            }

            if ((policy.AbsoluteExpiration != ObjectCache.InfiniteAbsoluteExpiration) && (policy.SlidingExpiration != ObjectCache.NoSlidingExpiration))
            {
                throw new ArgumentException(Resources.ExceptionInvalidExpirationCombination, "policy");
            }

            if ((policy.SlidingExpiration < ObjectCache.NoSlidingExpiration) || (new TimeSpan(365, 0, 0, 0) < policy.SlidingExpiration))
            {
                throw new ArgumentOutOfRangeException("policy.SlidingExpiration");
            }
            
            if ((policy.RemovedCallback != null) && (policy.UpdateCallback != null))
            {
                throw new ArgumentException(Resources.ExceptionInvalidCallbackCombination, "policy");
            }
        }
    }
}
