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

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfItemsScavengingStrategy{TCacheEntry}"/> class.
        /// </summary>
        /// <param name="maxItemsBeforeScavenging">Maximum number of items in cache before an add causes scavenging to take place.</param>
        /// <param name="itemsLeftAfterScavenging">Number of items left in the cache after scavenging has taken place.</param>
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

        /// <summary>
        /// Determines whether scavenging is needed for <paramref name="entries"/>.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <returns><see langword="true"/> if scavenging is needed, otherwise <see langword="false"/>.</returns>
        public bool ShouldScavenge(IDictionary<string, TCacheEntry> entries)
        {
            return entries.Count > maxItemsBeforeScavenging;
        }

        /// <summary>
        /// Determines whether additional scavenging is needed for <paramref name="entries"/>.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <returns><see langword="true"/> if additional scavenging is needed, otherwise <see langword="false"/>.</returns>
        public bool ShouldScavengeMore(IDictionary<string, TCacheEntry> entries)
        {
            return entries.Count > itemsLeftAfterScavenging;
        }

        /// <summary>
        /// Determines the entries that should be scavenged from <paramref name="currentEntries"/>.
        /// </summary>
        /// <param name="currentEntries">The entries to scavenge.</param>
        /// <returns>A set of the entries that should be scavenged.</returns>
        public IEnumerable<TCacheEntry> EntriesToScavenge(IEnumerable<TCacheEntry> currentEntries)
        {
            return currentEntries.OrderBy(x => x.Priority).ThenBy(x => x.LastAccessTime);
        }
    }
}
