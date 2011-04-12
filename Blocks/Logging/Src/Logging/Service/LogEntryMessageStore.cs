using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Represents a log entry message temporary store to buffer messages before sending them to the server. It can use the Isolated Storage
    /// for persisting entries after a crash or when there is no network connectivity with the server.
    /// </summary>
    public class LogEntryMessageStore : ILogEntryMessageStore, IDisposable
    {
        private class LogEntryItem
        {
            private readonly LogEntryMessage entry;

            public LogEntryItem(LogEntryMessage entry, string storageId)
            {
                if (entry == null) throw new ArgumentNullException("entry");

                this.entry = entry;
                this.StorageId = storageId;
            }

            public LogEntryMessage Entry
            {
                get { return this.entry; }
            }

            public string StorageId { get; set; }
        }

        private const string AccesorPreffix = "Log_";
        private readonly string name;
        private readonly int memoryBufferItemsLimit;
        private readonly int maxSizeInKilobytes;

        private readonly List<LogEntryItem> entries = new List<LogEntryItem>();
        private readonly object lockObject = new object();
        private StorageAccessor storage;

        private readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LogEntryMessage));

        /// <summary>
        /// Creates a new instance of <see cref="LogEntryMessageStore"/>
        /// </summary>
        /// <param name="name">The name of the store.</param>
        /// <param name="memoryBufferItemsLimit">The maximum amount of items that will be buffered in memory, for example when there are connectivity issues that prevent the listener from submiting the log entries.</param>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes to be used when storing entries into the isolated storage as a backup strategy.</param>
        public LogEntryMessageStore(string name, int memoryBufferItemsLimit, int maxSizeInKilobytes)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name == string.Empty) throw new ArgumentException("name");
            if (memoryBufferItemsLimit < 1) throw new ArgumentException("memoryBufferItemsLimit");
            if (maxSizeInKilobytes < 0) throw new ArgumentException("maxSizeInBytes");

            this.name = name;
            this.memoryBufferItemsLimit = memoryBufferItemsLimit;
            this.maxSizeInKilobytes = maxSizeInKilobytes;

            if (this.maxSizeInKilobytes > 0)
            {
                try
                {
                    this.storage = new StorageAccessor(AccesorPreffix + this.name, this.maxSizeInKilobytes * 1024);
                    this.ReadSerializedEntries();
                }
                catch (IsolatedStorageException)
                {
                    // use in memory buffer only
                    this.DisposeChildDependencies();
                }
            }
            else
            {
                try
                {
                    DeleteStore(AccesorPreffix + this.name);
                }
                catch { } // best effort to clean-up previous program instances that did use the isolated storage.
            }
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
        /// Gets a value indicating whether this instance is writable.
        /// </summary>
        /// <remarks>
        /// An instance is not writable if another instance of the same application is already using the
        /// isolated storage with the same name, or if the <see cref="Quota"/> is zero.
        /// </remarks>
        public bool IsWritable
        {
            get { return this.storage != null && !this.storage.IsReadOnly; }
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
        /// Stores a new entry.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        public void Add(LogEntryMessage entry)
        {
            string id = null;
            if (this.IsWritable)
            {
                byte[] serialized;
                using (var stream = new MemoryStream())
                {
                    this.serializer.WriteObject(stream, entry);
                    serialized = stream.ToArray();
                }

                if (serialized.Length < this.storage.MaxSize)
                {
                    lock (lockObject)
                    {
                        int i = 0;
                        while (id == null)
                        {
                            try
                            {
                                id = this.storage.Save(serialized);
                                break;
                            }
                            catch (AllocationException)
                            {
                                if (i >= this.entries.Count)
                                {
                                    break;
                                }

                                while (i < this.entries.Count)
                                {
                                    var current = this.entries[i];
                                    i++;
                                    if (current.StorageId != null)
                                    {
                                        try
                                        {
                                            this.storage.Remove(current.StorageId);
                                            current.StorageId = null;
                                            break;
                                        }
                                        catch
                                        {
                                            // if cannot remove it, keep it as assigned in the memory buffer.
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // best effort to add
                                break;
                            }
                        }
                    }
                }
            }
            lock (lockObject)
            {
                this.entries.Add(new LogEntryItem(entry, id));
                if (this.entries.Count > this.memoryBufferItemsLimit)
                {
                    this.entries.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Removes several entries from store, from the first entry, until it finds <paramref name="entry"/>. If <paramref name="entry"/> is not found, no entry will be removed.
        /// </summary>
        /// <param name="entry">The last entry to remove.</param>
        public void RemoveUntil(LogEntryMessage entry)
        {
            lock (lockObject)
            {
                int i = 0;
                while(i < this.entries.Count)
                {
                    if (this.entries[i].Entry == entry)
                    {
                        this.RemoveRange(i + 1);
                        break;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// Retrieves all the entries currently stored by the store.
        /// </summary>
        /// <returns></returns>
        public LogEntryMessage[] GetEntries()
        {
            lock (lockObject)
            {
                return this.entries.Select(x => x.Entry).ToArray();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Disposing(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Disposing(bool disposing)
        {
            this.DisposeChildDependencies();
        }

        ~LogEntryMessageStore()
        {
            Disposing(false);
        }

        private void DisposeChildDependencies()
        {
            using (this.storage as IDisposable) { this.storage = null; }
        }

        private void ReadSerializedEntries()
        {
            if (this.storage != null)
            {
                var list = new List<LogEntryItem>();

                var serializedEntries = this.storage.ReadAll();

                if (serializedEntries != null)
                {
                    foreach (var serializedEntry in serializedEntries)
                    {
                        try
                        {
                            if (serializedEntry.Value != null)
                            {
                                using (var stream = new MemoryStream(serializedEntry.Value))
                                {
                                    var item = new LogEntryItem((LogEntryMessage)this.serializer.ReadObject(stream), serializedEntry.Key);
                                    list.Add(item);
                                }
                            }
                            else
                            {
                                this.TryRemove(serializedEntry.Key);
                            }
                        }
                        catch
                        {
                            // if cannot deserialize entry for some reason, try to remove it, and skip it
                            this.TryRemove(serializedEntry.Key);
                        }
                    }
                }

                foreach (var item in list.OrderBy(x => x.Entry.TimeStamp))
                {
                    this.entries.Add(item);
                }
            }
        }

        private void RemoveRange(int count)
        {
            foreach (var id in this.entries.Take(count).Select(x => x.StorageId).Where(i => i != null))
            {
                this.TryRemove(id);
            }

            this.entries.RemoveRange(0, count);
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