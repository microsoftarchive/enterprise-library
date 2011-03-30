using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    /// <summary>
    /// An implementation of <see cref="IScavengingStrategy{TCacheEntry}"/>
    /// that bases decisions on the number of items in the cache.
    /// </summary>
    /// <typeparam name="TCacheEntry"></typeparam>
    public class NumberOfItemsScavengingStrategy<TCacheEntry> :
        IScavengingStrategy<TCacheEntry>
        where TCacheEntry : CacheEntry
    {
        private readonly int maxItemsBeforeScavenging;
        private readonly int itemsLeftAfterScavenging;

        public NumberOfItemsScavengingStrategy(int maxItemsBeforeScavenging, int itemsLeftAfterScavenging)
        {
            if (maxItemsBeforeScavenging <= 0)
                throw new ArgumentException(Resources.MaxItemsBeforeScavengingMustBePositive, "maxItemsBeforeScavenging");
            if (itemsLeftAfterScavenging <= 0)
                throw new ArgumentException(Resources.ItemsLeftAfterScavengingMustBePositive, "itemsLeftAfterScavenging");
            if (itemsLeftAfterScavenging >= maxItemsBeforeScavenging)
                throw new ArgumentException(Resources.ItemsLeftMustBeLessThanMaxItemsBefore, "itemsLeftAfterScavenging");

            this.maxItemsBeforeScavenging = maxItemsBeforeScavenging;
            this.itemsLeftAfterScavenging = itemsLeftAfterScavenging;
        }

        public bool ShouldScavenge(IDictionary<string, TCacheEntry> entries)
        {
            return entries.Count > maxItemsBeforeScavenging;
        }

        public bool ShouldScavengeMore(IDictionary<string, TCacheEntry> entries)
        {
            return entries.Count > itemsLeftAfterScavenging;
        }

        public IEnumerable<TCacheEntry> EntriesToScavenge(IEnumerable<TCacheEntry> currentEntries)
        {
            return currentEntries.OrderBy(x => x.Priority).ThenBy(x => x.LastAccessTime);
        }
    }
}
