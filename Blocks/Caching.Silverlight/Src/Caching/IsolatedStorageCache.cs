using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Cache that persists entries in isolated storage and loads the entries on initialization.
    /// </summary>
    /// <remarks>
    /// If a second instance of an application using this cache is started, the cache on the second instance will
    /// only load the items in isolated storage but will not save them.
    /// </remarks>
    public class IsolatedStorageCache : MemoryBackedCacheBase<IsolatedStorageCacheEntry>
    {
        private readonly ICacheEntryStore store;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCache"/> class.
        /// </summary>
        /// <param name="name">The name for the cache.</param>
        /// <param name="maxSizeInKiloBytes">The maximum size in kilobytes.</param>
        /// <param name="percentOfQuotaUsedBeforeScavenging">The percentage of quota used before scavenging.</param>
        /// <param name="percentOfQuotaUsedAfterScavenging">The percentage of quota used after scavenging.</param>
        /// <param name="expirationPollingInterval">The expiration polling interval.</param>
        public IsolatedStorageCache(string name, long maxSizeInKiloBytes, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging, TimeSpan expirationPollingInterval)
            : this(name, maxSizeInKiloBytes, percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging, expirationPollingInterval, new IsolatedStorageCacheEntrySerializer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCache"/> class.
        /// </summary>
        /// <param name="name">The name for the cache.</param>
        /// <param name="maxSizeInKiloBytes">The maximum size in kilobytes.</param>
        /// <param name="percentOfQuotaUsedBeforeScavenging">The percentage of quota used before scavenging.</param>
        /// <param name="percentOfQuotaUsedAfterScavenging">The percentage of quota used after scavenging.</param>
        /// <param name="expirationPollingInterval">The expiration polling interval.</param>
        /// <param name="serializer">An entry serializer.</param>
        public IsolatedStorageCache(string name, long maxSizeInKiloBytes, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging, TimeSpan expirationPollingInterval, IIsolatedStorageCacheEntrySerializer serializer)
            : this(name, new CacheEntryStore(name, maxSizeInKiloBytes * 1024, serializer), percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging, expirationPollingInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCache"/> class.
        /// </summary>
        /// <param name="name">The name for the cache.</param>
        /// <param name="store">The isolated storage entries store.</param>
        /// <param name="percentOfQuotaUsedBeforeScavenging">The percentage of quota used before scavenging.</param>
        /// <param name="percentOfQuotaUsedAfterScavenging">The percentage of quota used after scavenging.</param>
        /// <param name="expirationPollingInterval">The expiration polling interval.</param>
        protected IsolatedStorageCache(string name, ICacheEntryStore store, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging, TimeSpan expirationPollingInterval)
            : base(
                name,
                new IsolatedStorageSizeScavengingStrategy(store, new IsolatedStorageInfo(), percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging),
                new ScavengingScheduler(),
                new RecurringWorkScheduler(expirationPollingInterval))
        {
            this.store = store;

            foreach (var entry in this.store.GetSerializedEntries())
            {
                base.InnerAdd(entry);
            }

            base.ScheduleScavengingIfNeeded();
        }

        /// <summary>
        /// Gets a description of the features that the cache provides. See remarks section for more info on <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching.DefaultCacheCapabilities.CacheEntryUpdateCallback"/> 
        /// and <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching.DefaultCacheCapabilities.CacheEntryRemovedCallback"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="IsolatedStorageCache"/> implementation partially supports <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching.DefaultCacheCapabilities.CacheEntryUpdateCallback"/> and 
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching.DefaultCacheCapabilities.CacheEntryRemovedCallback"/>; the callbacks are invoked as long as the cache object instance that added
        /// the cache entry with the callbacks is the same one that is running when the cache entry expires. If the cache instance is different (i.e. the 
        /// application was closed and reopened, so the cache entries are rehydrated from disk), then the callbacks will not be invoked, as they are
        /// not (de)serialized.
        /// </remarks>
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

        /// <summary>
        /// The amount of isolated storage space being used to store the cache entries.
        /// </summary>
        public long UsedPhysicalSize
        {
            get { return this.store.UsedPhysicalSize; }
        }

        /// <summary>
        /// Gets a value indicating if this instance of the cache is using the isolated storage to persist the cache entries. 
        /// </summary>
        /// <remarks>There might be occassions where the cache is working just in memory, so any changes to the cache will not be reflected 
        /// in future instances of the same cache.</remarks>
        public bool IsPersisting
        {
            get { return this.store.IsWritable; }
        }

        /// <summary>
        /// Deletes the data persisted by the cache with the specified name.
        /// </summary>
        /// <param name="name">The name of the cache that is to be removed.</param>
        public static void DeleteCache(string name)
        {
            CacheEntryStore.DeleteStore(name);
        }

        /// <summary>
        /// Creates a cache entry.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object for the cache entry.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>A new cache entry of type <see cref="IsolatedStorageCacheEntry"/>.</returns>
        protected override IsolatedStorageCacheEntry CreateCacheEntry(string key, object value, CacheItemPolicy policy)
        {
            return new IsolatedStorageCacheEntry(key, value, policy);
        }

        /// <summary>
        /// Inserts a cache entry into the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        protected override void DoSet(string key, object value, CacheItemPolicy policy)
        {
            DoRemove(key, CacheEntryRemovedReason.Removed);

            bool add = true;
            var entry = CreateCacheEntry(key, value, policy);
            if (this.store.IsWritable)
            {
                try
                {
                    this.store.Add(entry);
                }
                catch
                {
                    // Iso Storage is full
                    add = false;
                }
            }

            if (add)
            {
                base.InnerAdd(entry);
            }

            ScheduleScavengingIfNeeded();
        }

        /// <summary>
        /// Invoked when an entry was removed from the cache.
        /// </summary>
        /// <param name="entry">The entry that was removed.</param>
        /// <param name="reason">The reason for the removal.</param>
        protected override void OnItemRemoved(IsolatedStorageCacheEntry entry, CacheEntryRemovedReason reason)
        {
            base.OnItemRemoved(entry, reason);

            if (this.store.IsWritable)
            {
                try
                {
                    this.store.Remove(entry);
                }
                catch { } // best effort to remove
            }
        }

        /// <summary>
        /// Updates the last access time on an entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected override void DoUpdateLastAccessTime(IsolatedStorageCacheEntry entry)
        {
            base.DoUpdateLastAccessTime(entry);
            if (this.store.IsWritable)
            {
                try
                {
                    this.store.UpdateLastUpdateTime(entry);
                }
                catch { }  // best effort to update
            }
        }

        /// <summary>
        /// Releases the store.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (this.store as IDisposable) { }
            }

            base.Dispose(disposing);
        }
    }
}
