using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    /// <summary>
    /// This interface provides a standard way to implemented
    /// extended expirations above and beyond what <see cref="Runtime.Caching.CacheItemPolicy"/>
    /// does. That class is extensible, but it's also a dumb data
    /// class. Working through this interface lets us add some
    /// logic to the process.
    /// </summary>
    public interface IExtendedCacheItemPolicy
    {
        /// <summary>
        /// Based on the logic, settings, and last access time for
        /// this cache item, has it expired?
        /// </summary>
        /// <param name="lastAccessedTime">Last time the cache item was accessed.</param>
        /// <returns>True if expired, false if not.</returns>
        bool IsExpired(DateTimeOffset lastAccessedTime);

        /// <summary>
        /// The priority of this cache item.
        /// </summary>
        CacheItemPriority Priority { get; }
    }
}
