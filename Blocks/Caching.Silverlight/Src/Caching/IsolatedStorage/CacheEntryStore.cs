using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Manages the storage and retrieval of cache entries in isolated storage.
    /// </summary>
    public class CacheEntryStore : ICacheEntryStore, IDisposable
    {
        private const string AccesorPreffix = "Cache_";

        private readonly string name;
        private readonly long maxSizeInBytes;

        private readonly IIsolatedStorageCacheEntrySerializer serializer;
        private StorageAccessor storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntryStore"/> class.
        /// </summary>
        /// <param name="name">The name of the store.</param>
        /// <param name="maxSizeInBytes">The maximum size in bytes.</param>
        /// <param name="serializer">An entry serializer.</param>
        public CacheEntryStore(string name, long maxSizeInBytes, IIsolatedStorageCacheEntrySerializer serializer)
        {
            if (maxSizeInBytes < 0)
                throw new ArgumentOutOfRangeException("maxSizeInBytes");

            this.name = name;
            this.maxSizeInBytes = maxSizeInBytes;
            this.serializer = serializer;

            try
            {
                this.storage = new StorageAccessor(AccesorPreffix + name, maxSizeInBytes);
            }
            catch (IsolatedStorageException)
            {
                // use in memory cache only
                this.DisposeChildDependencies();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is writable.
        /// </summary>
        /// <remarks>
        /// An instance is not writable if another instance of the same application is already using the
        /// isolated storage with the same name.
        /// </remarks>
        public bool IsWritable
        {
            get { return this.storage != null && !this.storage.IsReadOnly; }
        }

        /// <summary>
        /// Stores a new entry.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        /// <remarks>
        /// The <see cref="IsolatedStorageCacheEntry.StorageId"/> on the added entry is updated to match the physical
        /// storage.
        /// </remarks>
        public void Add(IsolatedStorageCacheEntry entry)
        {
            if (this.IsWritable)
            {
                var serialized = this.serializer.Serialize(entry);
                entry.StorageId = this.storage.Save(serialized);
            }
        }

        /// <summary>
        /// Removes an entry from storage.
        /// </summary>
        /// <param name="entry">The entry to remove.</param>
        public void Remove(IsolatedStorageCacheEntry entry)
        {
            if (this.IsWritable)
            {
                EnsurePersisted(entry);

                this.storage.Remove(entry.StorageId);
                entry.StorageId = null;
            }
        }

        /// <summary>
        /// Updates the last access time for the entry in storage.
        /// </summary>
        /// <param name="entry">The entry to update.</param>
        public void UpdateLastUpdateTime(IsolatedStorageCacheEntry entry)
        {
            if (this.IsWritable)
            {
                EnsurePersisted(entry);

                var update = this.serializer.GetUpdateForLastUpdateTime(entry);
                this.storage.Overwrite(entry.StorageId, update.Bytes, update.Offset);
            }
        }

        /// <summary>
        /// Retrieves all the entries currently stored by the store.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IsolatedStorageCacheEntry> GetSerializedEntries()
        {
            if (this.storage != null)
            {
                var serializedEntries = this.storage.ReadAll();

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

        /// <summary>
        /// Deletes the store with the given name.
        /// </summary>
        /// <param name="name">The store name.</param>
        public static void DeleteStore(string name)
        {
            StorageAccessor.DeleteStorage(AccesorPreffix + name);
        }

        /// <summary>
        /// Gets the quota allowed for the store.
        /// </summary>
        public long Quota
        {
            get { return this.IsWritable ? this.storage.MaxSize : 0; }
        }

        /// <summary>
        /// Gets an estimate of the physical size used by the store.
        /// </summary>
        public long UsedPhysicalSize
        {
            get { return this.IsWritable ? this.storage.UsedPhysicalSize : 0; }
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeChildDependencies();
            }
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
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
