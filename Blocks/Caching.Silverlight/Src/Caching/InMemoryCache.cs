using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Represents the type that implements an in-memory cache.
    /// </summary>
    /// <remarks>
    /// The <see cref="InMemoryCache"/> class is a concrete implementation of the abstract <see cref="ObjectCache"/> class.
    /// <para>
    /// Note: The <see cref="InMemoryCache"/> class is somewhat similar to the System.Runtime.Caching.MemoryCache class available in the .NET Framework in the Desktop.
    /// The <see cref="InMemoryCache"/> class has many properties and methods for accessing the cache that will be familiar to you if you have used the Desktop's MemoryCache class.
    /// The MemoryCache class does not allow null as a value in the cache. Any attempt to add or change a cache entry with a value of null will fail.
    /// </para>
    /// <para> The <see cref="InMemoryCache"/> type does not implement cache regions. Therefore, when you call <see cref="InMemoryCache"/> methods that 
    /// implement base methods that contain a parameter for regions, do not pass a value for the parameter. The methods that use the region parameter 
    /// all supply a default <see langword="null"/> value. For example, the <see cref="AddOrGetExisting"/> method overload has a regionName parameter whose
    /// default value is <see langword="null"/>.</para>
    /// </remarks>
    public class InMemoryCache : MemoryBackedCacheBase<CacheEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCache"/> class.
        /// </summary>
        /// <param name="name">The name to use to look up configuration information.</param>
        /// <param name="maxItemsBeforeScavenging">Maximum number of items in cache before an add causes scavenging to take place.</param>
        /// <param name="itemsLeftAfterScavenging">Number of items left in the cache after scavenging has taken place.</param>
        /// <param name="expirationPollingInterval">Frequency of expiration polling cycle.</param>
        public InMemoryCache(string name, int maxItemsBeforeScavenging, int itemsLeftAfterScavenging, TimeSpan expirationPollingInterval)
            : this(name, maxItemsBeforeScavenging, itemsLeftAfterScavenging, new ScavengingScheduler(), new ExpirationScheduler(expirationPollingInterval))
        {
        }

        public InMemoryCache(string name, int maxItemsBeforeScavenging, int itemsLeftAfterScavenging, 
            IManuallyScheduledWork scavengingScheduler,
            IRecurringScheduledWork expirationScheduler)
            : base(name, 
                new NumberOfItemsScavengingStrategy<CacheEntry>(maxItemsBeforeScavenging, itemsLeftAfterScavenging),
                scavengingScheduler, expirationScheduler)
        {
        }

        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return DefaultCacheCapabilities.InMemoryProvider |
                    DefaultCacheCapabilities.CacheEntryUpdateCallback |
                        DefaultCacheCapabilities.CacheEntryRemovedCallback |
                            DefaultCacheCapabilities.SlidingExpirations |
                                DefaultCacheCapabilities.AbsoluteExpirations;
            }
        }

        protected override CacheEntry CreateCacheEntry(string key, object value, CacheItemPolicy policy)
        {
            return new CacheEntry(key, value, policy);
        }
    }
}