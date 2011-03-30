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

        private bool isDisposed;
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
                return DoRemove(key, CacheEntryRemovedReason.Removed);
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

        protected virtual bool OnItemRemoving(TCacheEntry entry, CacheEntryRemovedReason reason)
        {
            switch (reason)
            {
                case CacheEntryRemovedReason.Expired:
                case CacheEntryRemovedReason.ChangeMonitorChanged:
                    return !TryUpdateItem(entry, reason);

                case CacheEntryRemovedReason.Removed:
                case CacheEntryRemovedReason.CacheSpecificEviction:
                case CacheEntryRemovedReason.Evicted:
                default:
                    break;
            }

            return true;
        }

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
                    }

                    return true;
                }
                catch { }  // best effor to update
            }

            return false;
        }

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

        protected object DoAddOrGetExisting(string key, object value, CacheItemPolicy policy)
        {
            return DoGet(key) ?? AddNew(key, value, policy);
        }

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
           var snapshot = entries
                .Where(pair => !pair.Value.Policy.IsExpired(pair.Value.LastAccessTime))
                .Select(pair => new KeyValuePair<string, object>(pair.Key, pair.Value.Value))
                .ToList();
            return snapshot.GetEnumerator();
        }

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

        protected virtual void DoSet(string key, object value, CacheItemPolicy policy)
        {
            policy.Validate();
            var entry = CreateCacheEntry(key, value, policy);
            DoRemove(key, CacheEntryRemovedReason.Removed);
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

                    if(!scavengingStrategy.ShouldScavengeMore(this.entries))
                        break;

                    DoRemove(itemToScavenge.Key, CacheEntryRemovedReason.Evicted);
                }
            }
        }
    }
}