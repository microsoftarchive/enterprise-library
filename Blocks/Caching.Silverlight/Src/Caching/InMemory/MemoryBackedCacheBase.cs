using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.InMemory
{
    public abstract class MemoryBackedCacheBase<TCacheEntry> : ObjectCache, IDisposable
        where TCacheEntry : CacheEntry
    {
        private readonly Dictionary<string, TCacheEntry> entries = new Dictionary<string, TCacheEntry>();
        private readonly string name;
        private readonly object padlock = new object();
        private readonly IManuallyScheduledWork scavengingScheduler;
        private IRecurringScheduledWork expirationScheduler;
        private IScavengingStrategy<TCacheEntry> scavengingStrategy;

        protected MemoryBackedCacheBase(string name,
            IScavengingStrategy<TCacheEntry> scavengingStrategy,
            IManuallyScheduledWork scavengingScheduler,
            IRecurringScheduledWork expirationScheduler)
        {
            if (scavengingScheduler == null) throw new ArgumentNullException("scavengingScheduler");
            if (expirationScheduler == null) throw new ArgumentNullException("expirationScheduler");
            if(scavengingStrategy == null) throw new ArgumentNullException("scavengingStrategy");

            this.name = name;
            this.scavengingStrategy = scavengingStrategy;

            this.scavengingScheduler = scavengingScheduler;
            scavengingScheduler.SetAction(DoScavenging);

            this.expirationScheduler = expirationScheduler;
            expirationScheduler.SetAction(DoExpirations);
            expirationScheduler.Start();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (expirationScheduler != null)
                {
                    expirationScheduler.Stop();
                    expirationScheduler.Dispose();
                    expirationScheduler = null;
                }
            }
        }

        ~MemoryBackedCacheBase()
        {
            Dispose(false);
        }

        public override string Name
        {
            get { return name; }
        }

        public override object this[string key]
        {
            get { return Get(key); }
            set { Set(key, value, InfiniteAbsoluteExpiration); }
        }

        public override CacheItem AddOrGetExisting(CacheItem value,
            CacheItemPolicy policy)
        {
            GuardNoRegion(value.RegionName);

            lock (padlock)
            {
                object cachedValue = DoAddOrGetExisting(value.Key, value.Value, policy);
                return new CacheItem(value.Key, cachedValue, null);
            }
        }

        public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration,
            string regionName = null)
        {
            GuardNoRegion(regionName);
            lock (padlock)
            {
                return DoAddOrGetExisting(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
            }
        }

        public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy,
            string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoAddOrGetExisting(key, value, policy);
            }
        }

        public override bool Contains(string key, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoContains(key);
            }
        }

        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys,
            string regionName = null)
        {
            GuardNoRegion(regionName);
            throw new NotSupportedException();
        }

        public override object Get(string key, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoGet(key);
            }
        }

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

        public override long GetCount(string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoGetCount();
            }
        }

        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            lock (padlock)
            {
                return DoGetEnumerator();
            }
        }

        public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoGetValues(keys);
            }
        }

        public override object Remove(string key, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                return DoRemove(key);
            }
        }

        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            GuardNoRegion(item.RegionName);

            lock (padlock)
            {
                DoSet(item.Key, item.Value, policy);
            }
        }

        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                DoSet(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
            }
        }

        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            GuardNoRegion(regionName);

            lock (padlock)
            {
                DoSet(key, value, policy);
            }
        }

        protected static void GuardNoRegion(string regionName)
        {
            if (!string.IsNullOrEmpty(regionName))
            {
                throw new NotSupportedException(Resources.RegionsNotSupported);
            }
        }

        #region Subclass hooks to add / remove / retrieve items from the store.

        protected void InnerAdd(TCacheEntry entry)
        {
            this.entries.Add(entry.Key, entry);
        }

        protected virtual void OnItemRemoved(TCacheEntry entry)
        {
        }

        #endregion

        protected object DoAddOrGetExisting(string key, object value, CacheItemPolicy policy)
        {
            return DoGet(key) ?? AddNew(key, value, policy);
        }

        protected object DoGet(string key)
        {
            TCacheEntry entry;
            if (entries.TryGetValue(key, out entry))
            {
                this.DoUpdateLastAccessTime(entry);
                return entry.Value;
            }
            return null;
        }

        protected virtual void DoUpdateLastAccessTime(TCacheEntry entry)
        {
            entry.UpdateLastAccessTime();
        }

        protected bool DoContains(string key)
        {
            return entries.ContainsKey(key);
        }

        protected int DoGetCount()
        {
            return entries.Count;
        }

        protected IEnumerator<KeyValuePair<string, object>> DoGetEnumerator()
        {
            List<KeyValuePair<string, object>> snapshot = entries.Select(pair =>
            {
                this.DoUpdateLastAccessTime(pair.Value);
                return new KeyValuePair<string, object>(pair.Key, pair.Value.Value);
            }).ToList();
            return snapshot.GetEnumerator();
        }

        protected IDictionary<string, object> DoGetValues(IEnumerable<string> keys)
        {
            return keys.Where(k => entries.ContainsKey(k))
                .ToDictionary(k => k, k => entries[k].Value);

        }

        protected object DoRemove(string key)
        {
            TCacheEntry cachedItem;
            if (entries.TryGetValue(key, out cachedItem))
            {
                entries.Remove(key);
                OnItemRemoved(cachedItem);
                return cachedItem.Value;
            }
            return null;
        }

        protected virtual void DoSet(string key, object value, CacheItemPolicy policy)
        {
            var entry = CreateCacheEntry(key, value, policy);
            entries[key] = entry;
            ScheduleScavengingIfNeeded();
        }

        protected abstract TCacheEntry CreateCacheEntry(string key, object value, CacheItemPolicy policy);

        private object AddNew(string key, object value, CacheItemPolicy policy)
        {
            DoSet(key, value, policy);
            return null;
        }

        protected void ScheduleScavengingIfNeeded()
        {
            if (scavengingStrategy.ShouldScavenge(entries))
            {
                scavengingScheduler.ScheduleWork();
            }
        }

        protected void DoExpirations()
        {
            lock (padlock)
            {
                List<string> removedKeys = entries.Where(e => e.Value.Policy.IsExpired(e.Value.LastAccessTime))
                    .Select(pair => pair.Key).ToList();
                foreach (string key in removedKeys)
                {
                    DoRemove(key);
                }
            }
        }

        protected void DoScavenging()
        {
            lock (padlock)
            {
                var scavengingCandidates = scavengingStrategy.EntriesToScavenge(this.entries.Values).ToList();

                foreach (var itemToScavenge in scavengingCandidates)
                {
                    if(!scavengingStrategy.ShouldScavengeMore(this.entries))
                        break;
                    DoRemove(itemToScavenge.Key);
                }

                scavengingStrategy.OnFinishingScavenging(this.entries);
            }
        }
    }
}