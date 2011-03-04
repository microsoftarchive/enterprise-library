using System;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageCache : MemoryBackedCacheBase<IsolatedStorageCacheEntry>
    {
        private const long DefaultMaxSize = 6 * 1048576;    // 6MB

        private readonly ICacheEntryStore store;

        public IsolatedStorageCache(string name)
            : this(name, DefaultMaxSize)
        {
        }

        public IsolatedStorageCache(string name, long maxSize)
            : this(name, maxSize, 0, 0, TimeSpan.FromMinutes(1))
        {
        }

        public IsolatedStorageCache(string name, long maxSize, float maxPercentualUsedSizeBeforeScavenging, float percentualUsedSizeLeftAfterScavenging, TimeSpan expirationPollingInterval)
            : this(name, new CacheEntryStore(name, maxSize), 0, 0, expirationPollingInterval)
        {
        }

        protected IsolatedStorageCache(string name, ICacheEntryStore store, float maxPercentualUsedSizeBeforeScavenging, float percentualUsedSizeLeftAfterScavenging, TimeSpan expirationPollingInterval)
            : base(
                name,
                new IsolatedStorageSizeScavengingStrategy(store, new IsolatedStorageInfo(), maxPercentualUsedSizeBeforeScavenging, percentualUsedSizeLeftAfterScavenging),
                new ScavengingScheduler(),
                new ExpirationScheduler(expirationPollingInterval))
        {
            this.store = store;

            if (this.store.IsEnabled)
            {
                foreach (var entry in this.store.GetSerializedEntries())
                {
                    base.InnerAdd(entry);
                }
            }
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
            DoRemove(key);

            bool add = true;
            var entry = CreateCacheEntry(key, value, policy);
            if (this.store.IsEnabled)
            {
                try
                {
                    this.store.Add(entry);
                }
                catch (IOException)
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

        protected override void OnItemRemoved(IsolatedStorageCacheEntry entry)
        {
            base.OnItemRemoved(entry);

            if (this.store.IsEnabled)
            {
                this.store.Remove(entry);
            }
        }

        protected override void DoUpdateLastAccessTime(IsolatedStorageCacheEntry entry)
        {
            base.DoUpdateLastAccessTime(entry);
            if (this.store.IsEnabled)
            {
                this.store.UpdateLastUpdateTime(entry);
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
