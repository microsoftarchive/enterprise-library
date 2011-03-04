using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageCacheEntry : CacheEntry
    {
        public IsolatedStorageCacheEntry(string key, object value, CacheItemPolicy policy)
            : base(key, value, policy)
        {
        }

        public IsolatedStorageCacheEntry(string key, object value, IExtendedCacheItemPolicy policy)
            : base(key, value, policy)
        {
        }

        public int? StorageId { get; set; }

        public new DateTimeOffset LastAccessTime
        {
            get { return base.LastAccessTime; }
            set { base.LastAccessTime = value; }
        }
    }
}
