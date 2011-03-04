using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class CacheEntryStore : ICacheEntryStore, IDisposable
    {
        private const int BlockSize = 256;

        private readonly string name;
        private readonly long maxSize;

        private IsolatedStorageCacheEntrySerializer serializer;
        private BlockStorage blockStorage;

        public CacheEntryStore(string name, long maxSize)
        {
            if (maxSize < 0)
                throw new ArgumentOutOfRangeException("maxSize");

            this.name = name;
            this.maxSize = maxSize;

            try
            {
                this.blockStorage = new BlockStorage(name, BlockSize, maxSize);
                this.serializer = new IsolatedStorageCacheEntrySerializer(this.blockStorage, Encoding.UTF8);
            }
            catch (IsolatedStorageException)
            {
                // use in memory cache only
                DisposeChildDependencies();
            }
            catch (InvalidDataException)
            {
                DisposeChildDependencies();

                BlockStorage.DeleteStorage(name);

                this.blockStorage = new BlockStorage(name, BlockSize, maxSize);
                this.serializer = new IsolatedStorageCacheEntrySerializer(this.blockStorage, Encoding.UTF8);
            }
        }

        public bool IsEnabled
        {
            get { return this.serializer != null; }
        }

        public void Add(IsolatedStorageCacheEntry entry)
        {
            if (this.IsEnabled)
            {
                this.serializer.Add(entry);
            }
        }

        public void Remove(IsolatedStorageCacheEntry entry)
        {
            if (this.IsEnabled)
            {
                this.serializer.Remove(entry);
            }
        }

        public void UpdateLastUpdateTime(IsolatedStorageCacheEntry entry)
        {
            if (this.IsEnabled)
            {
                this.serializer.UpdateLastUpdateTime(entry);
            }
        }

        public IEnumerable<IsolatedStorageCacheEntry> GetSerializedEntries()
        {
            if (this.IsEnabled)
            {
                try
                {
                    return this.serializer.GetSerializedEntries();
                }
                catch (InvalidDataException)
                {
                    DisposeChildDependencies();

                    BlockStorage.DeleteStorage(name);

                    this.blockStorage = new BlockStorage(name, BlockSize, maxSize);
                    this.serializer = new IsolatedStorageCacheEntrySerializer(this.blockStorage, Encoding.UTF8);

                    return this.serializer.GetSerializedEntries();
                }
            }

            return null;
        }

        public static void DeleteStore(string name)
        {
            BlockStorage.DeleteStorage(name);
        }

        private void DisposeChildDependencies()
        {
            using (this.serializer as IDisposable) { this.serializer = null; }
            using (this.blockStorage as IDisposable) { this.blockStorage = null; }
        }

        public long Quota
        {
            get { return this.IsEnabled ? this.blockStorage.MaxSize : 0; }
        }

        public long UsedSize
        {
            get { return this.IsEnabled ? this.blockStorage.UsedSize : 0; }
        }

        public long UsedPhysicalSize
        {
            get { return this.IsEnabled ? this.blockStorage.UsedPhysicalSize : 0; }
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
                DisposeChildDependencies();
            }
        }

        ~CacheEntryStore()
        {
            Dispose(false);
        }

        public IDictionary<int, int> Compact()
        {
            if (this.IsEnabled)
            {
                return this.blockStorage.Compact();
            }

            return null;
        }
    }
}
