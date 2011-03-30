using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageSizeScavengingStrategy : IScavengingStrategy<IsolatedStorageCacheEntry>
    {
        private const int DefaultMaxItemsBeforeScavengingWhenNotWritable = 20;

        private readonly ICacheEntryStore store;
        private readonly IIsolatedStorageInfo isoStorage;
        private readonly float percentOfQuotaUsedBeforeScavenging;
        private readonly float percentOfQuotaUsedAfterScavenging;

        private int maxItemsBeforeScavengingWhenNotWritable;

        public IsolatedStorageSizeScavengingStrategy(ICacheEntryStore store, IIsolatedStorageInfo isoStorage, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging)
        {
            if (percentOfQuotaUsedBeforeScavenging <= 0 || percentOfQuotaUsedBeforeScavenging > 100)
                throw new ArgumentException("percentOfQuotaUsedBeforeScavenging");

            if (percentOfQuotaUsedAfterScavenging <= 0 || percentOfQuotaUsedAfterScavenging > 100)
                throw new ArgumentException("percentOfQuotaUsedAfterScavenging");

            if (percentOfQuotaUsedAfterScavenging > percentOfQuotaUsedBeforeScavenging)
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ExceptionPercentOfQuotaRangeComparison, percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging), 
                    "percentOfQuotaUsedAfterScavenging");

            this.store = store;
            this.isoStorage = isoStorage;
            this.percentOfQuotaUsedBeforeScavenging = percentOfQuotaUsedBeforeScavenging / 100f;
            this.percentOfQuotaUsedAfterScavenging = percentOfQuotaUsedAfterScavenging / 100f;
        }

        public bool ShouldScavenge(IDictionary<string, IsolatedStorageCacheEntry> entries)
        {
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

        public bool ShouldScavengeMore(IDictionary<string, IsolatedStorageCacheEntry> entries)
        {
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

        public IEnumerable<IsolatedStorageCacheEntry> EntriesToScavenge(IEnumerable<IsolatedStorageCacheEntry> currentEntries)
        {
            return currentEntries.OrderBy(x => x.Priority).ThenBy(x => x.LastAccessTime);
        }

        private long GetCurrentCacheMaxSize()
        {
            return Math.Min(this.store.Quota, this.isoStorage.AvailableFreeSpace + this.store.UsedPhysicalSize);
        }

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
