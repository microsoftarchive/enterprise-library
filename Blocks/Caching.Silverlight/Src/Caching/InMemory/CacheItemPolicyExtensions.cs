using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    /// <summary>
    /// Extension methods for <see cref="CacheItemPolicy"/>.
    /// </summary>
    public static class CacheItemPolicyExtensions
    {
        /// <summary>
        /// Determines whether an entry is expired.
        /// </summary>
        /// <param name="policy">The policy for the entry.</param>
        /// <param name="lastAccessedTime">The last accessed time for the entry.</param>
        /// <returns>
        ///   <c>true</c> if the entry is expired; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Validates the specified policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <exception cref="ArgumentException">when <paramref name="policy"/> is invalid.</exception>
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
