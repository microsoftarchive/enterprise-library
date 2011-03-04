using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageSizeScavengingStrategy
        : IScavengingStrategy<IsolatedStorageCacheEntry>
    {
        private const int ShouldCompactThreshold = 4 * 1024;

        private const float DefaultMaxPercentualUsedSizeBeforeScavenging = 0.8f;
        private const float DefaultPercentualUsedSizeLeftAfterScavenging = 0.6f;

        private readonly ICacheEntryStore store;
        private readonly IIsolatedStorageInfo isoStorage;
        private readonly float maxPercentualUsedSizeBeforeScavenging;
        private readonly float percentualUsedSizeLeftAfterScavenging;

        public IsolatedStorageSizeScavengingStrategy(ICacheEntryStore store, IIsolatedStorageInfo isoStorage, float maxPercentualUsedSizeBeforeScavenging, float percentualUsedSizeLeftAfterScavenging)
        {
            if (maxPercentualUsedSizeBeforeScavenging < 0 || maxPercentualUsedSizeBeforeScavenging > 1)
                throw new ArgumentException("maxPercentualUsedSizeBeforeScavenging");

            if (percentualUsedSizeLeftAfterScavenging < 0 || percentualUsedSizeLeftAfterScavenging > 1)
                throw new ArgumentException("percentualUsedSizeLeftAfterScavenging");

            if (maxPercentualUsedSizeBeforeScavenging == 0)
                maxPercentualUsedSizeBeforeScavenging = DefaultMaxPercentualUsedSizeBeforeScavenging;

            if (percentualUsedSizeLeftAfterScavenging == 0)
                percentualUsedSizeLeftAfterScavenging = DefaultPercentualUsedSizeLeftAfterScavenging;

            if (percentualUsedSizeLeftAfterScavenging > maxPercentualUsedSizeBeforeScavenging)
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "Upper threshold ({0:F2}) cannot be lower than lower threshold ({1:F2}).", maxPercentualUsedSizeBeforeScavenging, percentualUsedSizeLeftAfterScavenging), 
                    "percentualUsedSizeLeftAfterScavenging");

            this.store = store;
            this.isoStorage = isoStorage;
            this.maxPercentualUsedSizeBeforeScavenging = maxPercentualUsedSizeBeforeScavenging;
            this.percentualUsedSizeLeftAfterScavenging = percentualUsedSizeLeftAfterScavenging;
        }

        public bool ShouldScavenge(IDictionary<string, IsolatedStorageCacheEntry> entries)
        {
            if (this.store.IsEnabled && this.store.Quota > 0)
            {
                if (this.ShouldCompact())
                {
                    return true;
                }

                if (this.store.UsedSize > GetCurrentCacheMaxSize() * this.maxPercentualUsedSizeBeforeScavenging)
                {
                    return entries.Count > 0;
                }
            }

            return false;
        }

        public bool ShouldScavengeMore(IDictionary<string, IsolatedStorageCacheEntry> entries)
        {
            if (this.store.IsEnabled && this.store.Quota > 0)
            {
                if (this.store.UsedSize > GetCurrentCacheMaxSize() * this.percentualUsedSizeLeftAfterScavenging)
                {
                    return entries.Count > 0;
                }
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

        public void OnFinishingScavenging(IDictionary<string, IsolatedStorageCacheEntry> entries)
        {
            if (this.store.IsEnabled)
            {
                if (this.ShouldCompact())
                {
                    var mappings = this.store.Compact();

                    if (mappings != null)
                    {
                        foreach (var mapping in mappings)
                        {
                            var entry = entries.Values.FirstOrDefault(x => x.StorageId == mapping.Key);
                            if (entry == null)
                            {
                                throw new InvalidDataException("Serialized cache does not correspond to the version that is in memory.");
                            }

                            entry.StorageId = mapping.Value;
                        }
                    }
                }
            }
        }

        private bool ShouldCompact()
        {
            var usedPhysicalSize = this.store.UsedPhysicalSize;
            if (usedPhysicalSize > (this.isoStorage.AvailableFreeSpace + usedPhysicalSize) * this.maxPercentualUsedSizeBeforeScavenging)
            {
                return this.store.UsedSize + ShouldCompactThreshold < this.store.UsedPhysicalSize;
            }

            return false;
        }
    }
}
