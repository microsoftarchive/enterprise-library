using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    /// <summary>
    /// A <see cref="CacheItem"/> for in-memory caches.
    /// </summary>
    public class CacheEntry : CacheItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntry"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="policy">The policy.</param>
        public CacheEntry(string key, object value, CacheItemPolicy policy)
            : base(key, value, null)
        {
            Policy = policy;

            UpdateLastAccessTime();
        }

        /// <summary>
        /// Gets the policy for the entry.
        /// </summary>
        public CacheItemPolicy Policy { get; private set; }

        /// <summary>
        /// Gets or sets the last access time for the entry.
        /// </summary>
        public DateTimeOffset LastAccessTime { get; protected set; }

        /// <summary>
        /// Gets the priority for the entry.
        /// </summary>
        public CacheItemPriority Priority { get { return Policy.Priority; } }

        /// <summary>
        /// Updates the last access time for the entry to the current time.
        /// </summary>
        public void UpdateLastAccessTime()
        {
            LastAccessTime = CachingTimeProvider.Now;
        }
    }
}