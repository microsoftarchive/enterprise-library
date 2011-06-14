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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    /// <summary>
    /// Base class for caches that keep items in memory.
    /// </summary>
    /// <typeparam name="TCacheEntry">The type of the cache entry specific for the concrete implementations.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class MemoryBackedCacheBase<TCacheEntry> : ObjectCache, IDisposable
        where TCacheEntry : CacheEntry
    {
        private readonly Dictionary<string, TCacheEntry> entries = new Dictionary<string, TCacheEntry>();
        private readonly string name;
        private readonly object padlock = new object();
        private readonly IManuallyScheduledWork scavengingScheduler;
        private IRecurringWorkScheduler expirationScheduler;
        private IScavengingStrategy<TCacheEntry> scavengingStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBackedCacheBase{TCacheEntry}"/> class.
        /// </summary>
        /// <param name="name">The name of the cache.</param>
        /// <param name="scavengingStrategy">The scavenging strategy.</param>
        /// <param name="scavengingScheduler">The scavenging scheduler.</param>
        /// <param name="expirationScheduler">The expiration scheduler.</param>
        protected MemoryBackedCacheBase(string name,
            IScavengingStrategy<TCacheEntry> scavengingStrategy,
            IManuallyScheduledWork scavengingScheduler,
            IRecurringWorkScheduler expirationScheduler)
        {
            if (scavengingScheduler == null) throw new ArgumentNullException("scavengingScheduler");
            if (expirationScheduler == null) throw new ArgumentNullException("expirationScheduler");
            if (scavengingStrategy == null) throw new ArgumentNullException("scavengingStrategy");

            this.name = name;
            this.scavengingStrategy = scavengingStrategy;

            this.scavengingScheduler = scavengingScheduler;
            scavengingScheduler.SetAction(DoScavenging);

            this.expirationScheduler = expirationScheduler;
            expirationScheduler.SetAction(DoExpirations);
            expirationScheduler.Start();
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed;

        /// <summary>
        /// Releases resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.isDisposed = true;

                if (this.expirationScheduler != null)
                {
                    using (this.expirationScheduler as IDisposable)
                    {
                        this.expirationScheduler.Stop();
                        this.expirationScheduler = null;
                    }
                }

                using (this.scavengingStrategy as IDisposable)
                {
                    this.scavengingStrategy = null;
                }
            }
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        ~MemoryBackedCacheBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the name of the cache instance.
        /// </summary>
        public override string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the default indexer for the ObjectCache class.
        /// </summary>
        public override object this[string key]
        {
            get { return Get(key); }
            set { Set(key, value, InfiniteAbsoluteExpiration); }
        }

        /// <summary>
        /// Inserts the specified CacheItem object into the cache, specifying
        /// information about how the entry will be evicted.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>
        /// If a cache entry with the same key exists, the specified cache entry; otherwise, <see langword="null"/>.
        /// </returns>
        public override CacheItem AddOrGetExisting(CacheItem value,
            CacheItemPolicy policy)
        {
            if (value == null) throw new ArgumentNullException("value");

            GuardNoRegion(value.RegionName);

            lock (padlock)
            {
                object cachedValue = DoAddOrGetExisting(value.Key, value.Value, policy);
                return new CacheItem(value.Key, cachedValue, null);
            }
        }

        /// <summary>
        /// Inserts a cache entry into the cache, by using a key, an object for the
        /// cache entry, an absolute expiration value, and an optional region to add the cache into.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>
        /// If a cache entry with the same key exists, the specified cache entry's value; otherwise, <see langword="null"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration,
            string regionName = null)
        {
            GuardNoRegion(regionName);
            lock (padlock)
            {
                return DoAddOrGetExisting(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
            }
        }

        /// <summary>
        /// Inserts a cache entry into the cache, specifying a key and a value for
        /// the cache entry, and information about how the entry will be evicted.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>
        /// If a cache entry with the same key exists, the specified cache entry's value; otherwise, <see langword="null"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy,
            string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoAddOrGetExisting(key, value, policy);
            }
        }

        /// <summary>
        /// Checks whether the cache entry already exists in the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>
        /// true if the cache contains a cache entry with the same key value as key; otherwise, false.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override bool Contains(string key, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoContains(key);
            }
        }

        /// <summary>
        /// Gets the specified cache entry from the cache as an object.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>
        /// The cache entry that is identified by key.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override object Get(string key, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoGet(key);
            }
        }

        /// <summary>
        /// Gets the specified cache entry from the cache as a <see cref="CacheItem"/> instance.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>
        /// The cache item.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override CacheItem GetCacheItem(string key, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                var item = DoGet(key);
                if (item != null)
                {
                    return new CacheItem(key, item, null);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the total number of cache entries in the cache.
        /// </summary>
        /// <param name="regionName">Optional. A named region in the cache for which the cache entry count should be computed, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>
        /// The number of cache entries in the cache. If regionName is not Nothing, the count indicates the
        /// number of entries that are in the specified cache region.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override long GetCount(string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoGetCount();
            }
        }

        /// <summary>
        /// Creates an enumerator that can be used to iterate through a collection
        /// of cache entries.
        /// </summary>
        /// <returns>
        /// The enumerator object that provides access to the cache entries in the cache.
        /// </returns>
        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            lock (padlock)
            {
                return DoGetEnumerator();
            }
        }

        /// <summary>
        /// Gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry or entries were
        /// added, if regions are implemented. The default value for the optional parameter is <see langword="null"/>.</param>
        /// <returns>
        /// A dictionary of key/value pairs that represent cache entries.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoGetValues(keys);
            }
        }

        /// <summary>
        /// Removes the cache entry from the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        /// <returns>
        /// An object that represents the value of the removed cache entry that was specified by the key,
        /// or <see langword="null"/> if the specified entry was not found.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override object Remove(string key, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoRemove(key, CacheEntryRemovedReason.Removed);
            }
        }

        /// <summary>
        /// Inserts the cache entry into the cache as a CacheItem instance,
        /// specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="item">The cache item to add.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides
        /// more options for eviction than a simple absolute expiration.</param>
        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            if (item == null) throw new ArgumentNullException("item");
            GuardNoRegion(item.RegionName);

            lock (padlock)
            {
                DoSet(item.Key, item.Value, policy);
            }
        }

        /// <summary>
        /// Inserts a cache entry into the cache, specifying time-based expiration details.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                DoSet(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
            }
        }

        /// <summary>
        /// Inserts a cache entry into the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if
        /// regions are implemented. Defaults to <see langword="null"/>.</param>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                DoSet(key, value, policy);
            }
        }

        /// <summary>
        /// Ensures that no region has been specified.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        protected static void GuardNoRegion(string regionName)
        {
            if (!string.IsNullOrEmpty(regionName))
            {
                throw new NotSupportedException(Resources.RegionsNotSupported);
            }
        }

        #region Subclass hooks to add / remove / retrieve items from the store.

        /// <summary>
        /// Adds an entry to the dictionary in the cache object.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        protected void InnerAdd(TCacheEntry entry)
        {
            this.entries.Add(entry.Key, entry);
        }

        /// <summary>
        /// Invoked when an entry is being removed from the cache.
        /// </summary>
        /// <param name="entry">The entry that is being removed.</param>
        /// <param name="reason">The reason for the removal.</param>
        protected virtual bool OnItemRemoving(TCacheEntry entry, CacheEntryRemovedReason reason)
        {
            switch (reason)
            {
                case CacheEntryRemovedReason.Expired:
                //case CacheEntryRemovedReason.ChangeMonitorChanged:
                    return !TryUpdateItem(entry, reason);

                case CacheEntryRemovedReason.Removed:
                //case CacheEntryRemovedReason.CacheSpecificEviction:
                case CacheEntryRemovedReason.Evicted:
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// Updates an entry in the cache.
        /// </summary>
        /// <param name="entry">The entry to update.</param>
        /// <param name="reason">The reason for the update.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Best effor to update. Cannot notify because this happens typically in a background thread.")]
        private bool TryUpdateItem(TCacheEntry entry, CacheEntryRemovedReason reason)
        {
            var callback = entry.Policy != null ? entry.Policy.UpdateCallback : null;
            if (callback != null)
            {
                try
                {
                    var args = new CacheEntryUpdateArguments(this, reason, entry.Key, entry.RegionName);
                    callback(args);

                    if (args.UpdatedCacheItem != null && args.UpdatedCacheItem.Key == entry.Key && args.UpdatedCacheItem.RegionName == entry.RegionName)
                    {
                        var policy = args.UpdatedCacheItemPolicy ?? new CacheItemPolicy { AbsoluteExpiration = InfiniteAbsoluteExpiration };
                        DoSet(entry.Key, args.UpdatedCacheItem.Value, policy);
                        return true;
                    }
                }
                catch { }  // best effor to update
            }

            return false;
        }

        /// <summary>
        /// Invoked when an entry was removed from the cache.
        /// </summary>
        /// <param name="entry">The entry that was removed.</param>
        /// <param name="reason">The reason for the removal.</param>
        protected virtual void OnItemRemoved(TCacheEntry entry, CacheEntryRemovedReason reason)
        {
            this.entries.Remove(entry.Key);

            var callback = entry.Policy != null ? entry.Policy.RemovedCallback : null;
            if (callback != null)
            {
                callback(new CacheEntryRemovedArguments(this, reason, new CacheItem(entry.Key, entry.Value, entry.RegionName)));
            }
        }

        #endregion

        /// <summary>
        /// Actual implementation for adding or getting a cache item.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>
        /// If a cache entry with the same key exists, the specified cache entry's value; otherwise, <see langword="null"/>.
        /// </returns>
        protected object DoAddOrGetExisting(string key, object value, CacheItemPolicy policy)
        {
            return DoGet(key) ?? AddNew(key, value, policy);
        }

        /// <summary>
        /// Actual implementation for getting a cache item.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <returns>
        /// The cache entry that is identified by key.
        /// </returns>
        protected object DoGet(string key)
        {
            TCacheEntry entry;
            if (entries.TryGetValue(key, out entry))
            {
                if (entry.Policy.IsExpired(entry.LastAccessTime))
                {
                    this.DoRemove(entry.Key, CacheEntryRemovedReason.Expired);
                }
                else
                {
                    this.DoUpdateLastAccessTime(entry);
                    return entry.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the last access time on an entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected virtual void DoUpdateLastAccessTime(TCacheEntry entry)
        {
            entry.UpdateLastAccessTime();
        }

        /// <summary>
        /// Checks whether the entry identified by a key is stored in memory.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <returns>
        /// true if the cache contains a cache entry with the same key value as key; otherwise, false.
        /// </returns>
        protected bool DoContains(string key)
        {
            return entries.ContainsKey(key);
        }

        /// <summary>
        /// Gets the total number of cache entries in the cache.
        /// </summary>
        /// <returns>
        /// The number of cache entries in the cache. If regionName is not Nothing, the count indicates the
        /// number of entries that are in the specified cache region.
        /// </returns>
        protected int DoGetCount()
        {
            return entries.Count;
        }

        /// <summary>
        /// Creates an enumerator that can be used to iterate through a collection
        /// of cache entries.
        /// </summary>
        /// <returns>
        /// The enumerator object that provides access to the cache entries in the cache.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Similar API as System.Runtime.Caching in the .NET Framework.")]
        protected IEnumerator<KeyValuePair<string, object>> DoGetEnumerator()
        {
            var snapshot = entries
                 .Where(pair => !pair.Value.Policy.IsExpired(pair.Value.LastAccessTime))
                 .Select(pair => new KeyValuePair<string, object>(pair.Key, pair.Value.Value))
                 .ToList();
            return snapshot.GetEnumerator();
        }

        /// <summary>
        /// Gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <returns>
        /// A dictionary of key/value pairs that represent cache entries.
        /// </returns>
        protected IDictionary<string, object> DoGetValues(IEnumerable<string> keys)
        {
            var snapshot = new Dictionary<string, object>();
            foreach (var key in keys.Distinct())
            {
                var item = DoGet(key);
                if (item != null)
                {
                    snapshot.Add(key, item);
                }
            }

            return snapshot;
        }

        /// <summary>
        /// Removes a cache entry from the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to remove.</param>
        /// <param name="reason">The reason for the removal.</param>
        protected object DoRemove(string key, CacheEntryRemovedReason reason)
        {
            TCacheEntry entry;
            if (entries.TryGetValue(key, out entry))
            {
                if (OnItemRemoving(entry, reason))
                {
                    OnItemRemoved(entry, reason);
                    return entry.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Inserts a cache entry into the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        protected virtual void DoSet(string key, object value, CacheItemPolicy policy)
        {
            policy.Validate();
            var entry = CreateCacheEntry(key, value, policy);
            DoRemove(key, CacheEntryRemovedReason.Removed);
            entries[key] = entry;
            ScheduleScavengingIfNeeded();
        }

        /// <summary>
        /// Creates a cache entry.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object for the cache entry.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>A new cache entry of type <typeparamref name="TCacheEntry"/>.</returns>
        protected abstract TCacheEntry CreateCacheEntry(string key, object value, CacheItemPolicy policy);

        /// <summary>
        /// Adds the new entry to the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        private object AddNew(string key, object value, CacheItemPolicy policy)
        {
            DoSet(key, value, policy);
            return null;
        }

        /// <summary>
        /// Schedules a scavenging operation if needed.
        /// </summary>
        protected void ScheduleScavengingIfNeeded()
        {
            if (this.scavengingStrategy.ShouldScavenge(entries))
            {
                this.ScheduleScavenging();
            }
        }

        /// <summary>
        /// Schedules a scavenging operation.
        /// </summary>
        protected void ScheduleScavenging()
        {
            this.scavengingScheduler.ScheduleWork();
        }

        /// <summary>
        /// Performs an expiration sweep.
        /// </summary>
        protected void DoExpirations()
        {
            if (this.isDisposed)
                return;

            lock (padlock)
            {
                if (this.isDisposed)
                    return;

                var removedKeys = entries.Where(e => e.Value.Policy.IsExpired(e.Value.LastAccessTime)).Select(pair => pair.Key).ToList();
                foreach (string key in removedKeys)
                {
                    if (this.isDisposed)
                        return;

                    DoRemove(key, CacheEntryRemovedReason.Expired);
                }
            }
        }

        /// <summary>
        /// Performs a scavenging sweep.
        /// </summary>
        protected void DoScavenging()
        {
            if (this.isDisposed)
                return;

            lock (padlock)
            {
                if (this.isDisposed)
                    return;

                var scavengingCandidates = scavengingStrategy.EntriesToScavenge(this.entries.Values).ToList();

                foreach (var itemToScavenge in scavengingCandidates)
                {
                    if (this.isDisposed)
                        return;

                    if (!scavengingStrategy.ShouldScavengeMore(this.entries))
                        break;

                    DoRemove(itemToScavenge.Key, CacheEntryRemovedReason.Evicted);
                }
            }
        }
    }
}
