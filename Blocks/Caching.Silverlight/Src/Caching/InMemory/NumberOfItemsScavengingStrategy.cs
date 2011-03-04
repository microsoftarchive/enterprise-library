using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

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
            return currentEntries
                .Where(entry => entry.Priority != CacheItemPriority.NotRemovable)
                .OrderBy(entry => entry.LastAccessTime);
        }

        public void OnFinishingScavenging(IDictionary<string, TCacheEntry> entries)
        {
        }
    }
}
