using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public interface ICacheEntryStore
    {
        bool IsWritable { get; }

        long Quota { get; }
        long UsedPhysicalSize { get; }

        void Add(IsolatedStorageCacheEntry entry);
        void Remove(IsolatedStorageCacheEntry entry);
        void UpdateLastUpdateTime(IsolatedStorageCacheEntry entry);
        IEnumerable<IsolatedStorageCacheEntry> GetSerializedEntries();
    }
}
