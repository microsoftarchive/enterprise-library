using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public class IsolatedStorageCache : MemoryBackedCacheBase<IsolatedStorageCacheEntry>
    {
        private readonly ICacheEntryStore store;

        public IsolatedStorageCache(string name, long maxSizeInKiloBytes, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging, TimeSpan expirationPollingInterval)
            : this(name, maxSizeInKiloBytes, percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging, expirationPollingInterval, new IsolatedStorageCacheEntrySerializer())
        {
        }

        public IsolatedStorageCache(string name, long maxSizeInKiloBytes, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging, TimeSpan expirationPollingInterval, IIsolatedStorageCacheEntrySerializer serializer)
            : this(name, new CacheEntryStore(name, maxSizeInKiloBytes*1024, serializer), percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging, expirationPollingInterval)
        {
        }

        protected IsolatedStorageCache(string name, ICacheEntryStore store, int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging, TimeSpan expirationPollingInterval)
            : base(
                name,
                new IsolatedStorageSizeScavengingStrategy(store, new IsolatedStorageInfo(), percentOfQuotaUsedBeforeScavenging, percentOfQuotaUsedAfterScavenging),
                new ScavengingScheduler(),
                new ExpirationScheduler(expirationPollingInterval))
        {
            this.store = store;

            foreach (var entry in this.store.GetSerializedEntries())
            {
                base.InnerAdd(entry);
            }

            base.ScheduleScavengingIfNeeded();
        }

        /// <summary>
        /// Gets a description of the features that the cache provides. See remarks section for more info on <see cref="DefaultCacheCapabilities.CacheEntryUpdateCallback"/> 
        /// and <see cref="DefaultCacheCapabilities.CacheEntryUpdateCallback"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="IsolatedStorageCache"/> implementation partially supports <see cref="DefaultCacheCapabilities.CacheEntryUpdateCallback"/> and 
        /// <see cref="DefaultCacheCapabilities.CacheEntryUpdateCallback"/>; the callbacks are invoked as long as the cache object instance that added
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

        protected override IsolatedStorageCacheEntry CreateCacheEntry(string key, object value, CacheItemPolicy policy)
        {
            return new IsolatedStorageCacheEntry(key, value, policy);
        }

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
