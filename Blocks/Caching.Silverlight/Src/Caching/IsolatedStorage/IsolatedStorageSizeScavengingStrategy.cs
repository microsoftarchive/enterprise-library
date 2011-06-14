//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Scavenging strategy for isolated storage, based on quota usage.
    /// </summary>
    public class IsolatedStorageSizeScavengingStrategy : IScavengingStrategy<IsolatedStorageCacheEntry>
    {
        private const int DefaultMaxItemsBeforeScavengingWhenNotWritable = 20;

        private readonly ICacheEntryStore store;
        private readonly IIsolatedStorageInfo isoStorage;
        private readonly float percentOfQuotaUsedBeforeScavenging;
        private readonly float percentOfQuotaUsedAfterScavenging;

        private int maxItemsBeforeScavengingWhenNotWritable;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageSizeScavengingStrategy"/> class.
        /// </summary>
        /// <param name="store">The cache entry store.</param>
        /// <param name="isoStorage">The isolated storage information provider.</param>
        /// <param name="percentOfQuotaUsedBeforeScavenging">The percentage of quota used before scavenging.</param>
        /// <param name="percentOfQuotaUsedAfterScavenging">The percentage of quota used after scavenging.</param>
        public IsolatedStorageSizeScavengingStrategy(ICacheEntryStore store, IIsolatedStorageInfo isoStorage, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging)
        {
            if (percentOfQuotaUsedBeforeScavenging <= 0 || percentOfQuotaUsedBeforeScavenging > 100)
                throw new ArgumentOutOfRangeException("percentOfQuotaUsedBeforeScavenging");

            if (percentOfQuotaUsedAfterScavenging <= 0 || percentOfQuotaUsedAfterScavenging > 100)
                throw new ArgumentOutOfRangeException("percentOfQuotaUsedAfterScavenging");

            if (percentOfQuotaUsedAfterScavenging > percentOfQuotaUsedBeforeScavenging)
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ExceptionPercentOfQuotaRangeComparison, percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging),
                    "percentOfQuotaUsedAfterScavenging");

            this.store = store;
            this.isoStorage = isoStorage;
            this.percentOfQuotaUsedBeforeScavenging = percentOfQuotaUsedBeforeScavenging / 100f;
            this.percentOfQuotaUsedAfterScavenging = percentOfQuotaUsedAfterScavenging / 100f;
        }

        /// <summary>
        /// Determines whether scavenging is needed for <paramref name="entries"/>.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <returns><see langword="true"/> if scavenging is needed, otherwise <see langword="false"/>.</returns>
        public bool ShouldScavenge(IDictionary<string, IsolatedStorageCacheEntry> entries)
        {
            if (entries == null) throw new ArgumentNullException("entries");

            if (this.store.IsWritable)
            {
                if (this.store.Quota > 0)
                {
                    if (this.store.UsedPhysicalSize > GetCurrentCacheMaxSize() * this.percentOfQuotaUsedBeforeScavenging)
                    {
                        return entries.Count > 0;
                    }
                }
            }
            else if (entries.Count > this.GetMaxItemsBeforeScavengingWhenNotWritable(entries.Count))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether additional scavenging is needed for <paramref name="entries"/>.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <returns><see langword="true"/> if additional scavenging is needed, otherwise <see langword="false"/>.</returns>
        public bool ShouldScavengeMore(IDictionary<string, IsolatedStorageCacheEntry> entries)
        {
            if (entries == null) throw new ArgumentNullException("entries");

            if (this.store.IsWritable)
            {
                if (this.store.Quota > 0)
                {
                    if (this.store.UsedPhysicalSize > GetCurrentCacheMaxSize() * this.percentOfQuotaUsedAfterScavenging)
                    {
                        return entries.Count > 0;
                    }
                }
            }
            else if (entries.Count > this.GetMaxItemsBeforeScavengingWhenNotWritable(entries.Count))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines the entries that should be scavenged from <paramref name="currentEntries"/>.
        /// </summary>
        /// <param name="currentEntries">The entries to scavenge.</param>
        /// <returns>A set of the entries that should be scavenged.</returns>
        public IEnumerable<IsolatedStorageCacheEntry> EntriesToScavenge(IEnumerable<IsolatedStorageCacheEntry> currentEntries)
        {
            return currentEntries.OrderBy(x => x.Priority).ThenBy(x => x.LastAccessTime);
        }

        private long GetCurrentCacheMaxSize()
        {
            return Math.Min(this.store.Quota, this.isoStorage.AvailableFreeSpace + this.store.UsedPhysicalSize);
        }

        /// <summary>
        /// Gets the maximum quantity of items allowed before scavenging when the cache is in-memory only.
        /// </summary>
        /// <param name="entriesCount">The entries count.</param>
        /// <returns>The maximum quantity of items.</returns>
        protected virtual int GetMaxItemsBeforeScavengingWhenNotWritable(int entriesCount)
        {
            if (this.maxItemsBeforeScavengingWhenNotWritable == 0)
            {
                this.maxItemsBeforeScavengingWhenNotWritable = Math.Max(entriesCount, DefaultMaxItemsBeforeScavengingWhenNotWritable);
            }

            return this.maxItemsBeforeScavengingWhenNotWritable;
        }
    }
}
