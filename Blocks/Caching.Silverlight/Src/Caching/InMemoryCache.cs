using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public class InMemoryCache : MemoryBackedCacheBase<CacheEntry>
    {
        public InMemoryCache(string name, int maxItemsBeforeScavenging, int itemsLeftAfterScavenging, 
            IManuallyScheduledWork scavengingScheduler,
            IRecurringScheduledWork expirationScheduler)
            : base(name, 
                new NumberOfItemsScavengingStrategy<CacheEntry>(maxItemsBeforeScavenging, itemsLeftAfterScavenging),
                scavengingScheduler, expirationScheduler)
        {
            GuardScavengingSettings(maxItemsBeforeScavenging, itemsLeftAfterScavenging);
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

        private static void GuardScavengingSettings(int maxItemsBeforeScavenging, int itemsLeftAfterScavenging)
        {
            if (maxItemsBeforeScavenging <= 0)
                throw new ArgumentException(Resources.MaxItemsBeforeScavengingMustBePositive, "maxItemsBeforeScavenging");
            if (itemsLeftAfterScavenging <= 0)
                throw new ArgumentException(Resources.ItemsLeftAfterScavengingMustBePositive, "itemsLeftAfterScavenging");
            if (itemsLeftAfterScavenging >= maxItemsBeforeScavenging)
                throw new ArgumentException(Resources.ItemsLeftMustBeLessThanMaxItemsBefore, "itemsLeftAfterScavenging");
        }
    }
}