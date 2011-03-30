using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class CacheEntryStore : ICacheEntryStore, IDisposable
    {
        private readonly string name;
        private readonly long maxSizeInBytes;

        private IIsolatedStorageCacheEntrySerializer serializer;
        private StorageAccessor storage;

        public CacheEntryStore(string name, long maxSizeInBytes, IIsolatedStorageCacheEntrySerializer serializer)
        {
            if (maxSizeInBytes < 0)
                throw new ArgumentOutOfRangeException("maxSizeInBytes");

            this.name = name;
            this.maxSizeInBytes = maxSizeInBytes;
            this.serializer = serializer;

            try
            {
                this.storage = new StorageAccessor(name, maxSizeInBytes);
            }
            catch (IsolatedStorageException)
            {
                // use in memory cache only
                this.DisposeChildDependencies();
            }
        }

        public bool IsWritable
        {
            get { return this.storage != null && !this.storage.IsReadOnly; }
        }

        public void Add(IsolatedStorageCacheEntry entry)
        {
            if (this.IsWritable)
            {
                var serialized = this.serializer.Serialize(entry);
                entry.StorageId = this.storage.Save(serialized);
            }
        }

        public void Remove(IsolatedStorageCacheEntry entry)
        {
            if (this.IsWritable)
            {
                EnsurePersisted(entry);

                this.storage.Remove(entry.StorageId);
                entry.StorageId = null;
            }
        }

        public void UpdateLastUpdateTime(IsolatedStorageCacheEntry entry)
        {
            if (this.IsWritable)
            {
                EnsurePersisted(entry);

                var update = this.serializer.GetUpdateForLastUpdateTime(entry);
                this.storage.Overwrite(entry.StorageId, update.Bytes, update.Offset);
            }
        }

        public IEnumerable<IsolatedStorageCacheEntry> GetSerializedEntries()
        {
            if (this.storage != null)
            {
                IDictionary<string, byte[]> serializedEntries = null;

                try
                {
                    serializedEntries = this.storage.ReadAll();
                }
                catch (InvalidDataException)
                {
                    this.DisposeChildDependencies();

                    if (this.IsWritable)
                    {
                        try
                        {
                            StorageAccessor.DeleteStorage(name);

                            this.storage = new StorageAccessor(name, maxSizeInBytes);
                            serializedEntries = this.storage.ReadAll();
                        }
                        catch
                        {
                            // best effort to remove storage
                            this.DisposeChildDependencies();
                        }
                    }
                }

                if (serializedEntries != null)
                {
                    var entries = new List<IsolatedStorageCacheEntry>();

                    foreach (var serializedEntry in serializedEntries)
                    {
                        try
                        {
                            if (serializedEntry.Value != null)
                            {
                                var entry = this.serializer.Deserialize(serializedEntry.Value);
                                entry.StorageId = serializedEntry.Key;
                                entries.Add(entry);
                            }
                            else
                            {
                                this.TryRemove(serializedEntry.Key);
                            }
                        }
                        catch
                        {
                            // if cannot deserialize entry for some reason, try to remove it, and skip it
                            // TODO log about this deserialization exception
                            this.TryRemove(serializedEntry.Key);
                        }
                    }

                    return entries;
                }
            }

            return Enumerable.Empty<IsolatedStorageCacheEntry>();
        }

        public static void DeleteStore(string name)
        {
            StorageAccessor.DeleteStorage(name);
        }

        public long Quota
        {
            get { return this.IsWritable ? this.storage.MaxSize : 0; }
        }

        public long UsedPhysicalSize
        {
            get { return this.IsWritable ? this.storage.UsedPhysicalSize : 0; }
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

        private static void EnsurePersisted(IsolatedStorageCacheEntry entry)
        {
            if (entry.StorageId == null)
                throw new ArgumentException("Entry is not persisted.");
        }

        private void DisposeChildDependencies()
        {
            using (this.storage as IDisposable) { this.storage = null; }
        }

        private void TryRemove(string id)
        {
            if (this.IsWritable)
            {
                try
                {
                    this.storage.Remove(id);
                }
                catch { } // best effort to remove
            }
        }
    }
}
