//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

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
        private readonly int maxElementsInBuffer;

        private readonly List<LogEntryItem> entries = new List<LogEntryItem>();
        private readonly object lockObject = new object();
        private StorageAccessor storage;
        private int maxSizeInKilobytes;

        private readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LogEntryMessage));

        /// <summary>
        /// Creates a new instance of <see cref="LogEntryMessageStore"/>
        /// </summary>
        /// <param name="name">The name of the store.</param>
        /// <param name="maxElementsInBuffer">The maximum amount of elements that will be buffered in memory, for example when there are connectivity issues that prevent the listener from submitting the log entries.</param>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes to be used when storing entries into the isolated storage as a backup strategy.</param>
        public LogEntryMessageStore(string name, int maxElementsInBuffer, int maxSizeInKilobytes)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name == string.Empty) throw new ArgumentException("name");
            if (maxElementsInBuffer < 1) throw new ArgumentException("maxElementsInBuffer");
            GuardMaxSizeInKilobytes(maxSizeInKilobytes);

            this.name = name;
            this.maxElementsInBuffer = maxElementsInBuffer;
            this.maxSizeInKilobytes = maxSizeInKilobytes;

            this.InitializeBackingStore();
            this.ReadSerializedEntries();
        }

        private bool IsPersistingEntries { get; set; }

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
            get { return this.IsPersistingEntries ? this.ConvertBytesToKilobytes(this.storage.MaxSize) : 0; }
        }

        /// <summary>
        /// Gets an estimate of the physical size used by the store.
        /// </summary>
        public long UsedPhysicalSize
        {
            get { return this.IsWritable ? this.ConvertBytesToKilobytes(this.storage.UsedPhysicalSize) : 0; }
        }

        /// <summary>
        /// Stores a new entry.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        public void Add(LogEntryMessage entry)
        {
            string id = null;
            if (this.IsWritable && this.IsPersistingEntries)
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
                if (this.entries.Count > this.maxElementsInBuffer)
                {
                    this.RemoveRange(1);
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
                while (i < this.entries.Count)
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
        /// Resizes the backing size, trying to preserve all the entries.
        /// </summary>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes.</param>
        /// <exception cref="InvalidOperationException">when the repository is not available.</exception>
        /// <exception cref="IOException">when the repository cannot be re-initialized.</exception>
        public void ResizeBackingStore(int maxSizeInKilobytes)
        {
            GuardMaxSizeInKilobytes(maxSizeInKilobytes);
            lock (lockObject)
            {
                if (this.maxSizeInKilobytes != maxSizeInKilobytes)
                {
                    this.maxSizeInKilobytes = maxSizeInKilobytes;

                    if (maxSizeInKilobytes <= 0)
                    {
                        if (this.storage == null || !this.storage.IsReadOnly)
                        {
                            this.DisposeStorage();
                            try
                            {
                                StorageAccessor.DeleteStorage(name);
                            }
                            catch
                            {
                            } // best effort to remove.
                        }
                    }
                    else if (this.storage == null)
                    {
                        this.InitializeBackingStore();
                    }
                    else if (this.IsWritable)
                    {
                        this.storage.ChangeMaxSize(maxSizeInKilobytes * 1024);
                        this.TrimBackingStore();
                    }
                }
            }
        }

        /// <summary>
        /// Checks that the specified max size in kilobytes is valid.
        /// </summary>
        /// <param name="maxSizeInKilobytes">The value to check.</param>
        protected internal static void GuardMaxSizeInKilobytes(int maxSizeInKilobytes)
        {
            const int MinimumSize = 5;

            if (maxSizeInKilobytes < MinimumSize && maxSizeInKilobytes != 0)
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Resources.LogEntryMessageStoreMaxSizeMustBeEitherZeroOrLargerThanZero, MinimumSize),
                    "maxSizeInKilobytes");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the method is being called from the <see cref="Dispose()"/> method. <see langword="false"/> if it is being called from within the object finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeStorage();
            }
        }

        /// <summary>
        /// Releases resources for a <see cref="LogEntryMessageStore"/> before garbage collection.
        /// </summary>
        ~LogEntryMessageStore()
        {
            this.Dispose(false);
        }

        private void DisposeStorage()
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

        private void InitializeBackingStore()
        {
            string storageName = AccesorPreffix + this.name;
            this.IsPersistingEntries = false;
            if (this.maxSizeInKilobytes > 0)
            {
                try
                {
                    this.storage = new StorageAccessor(storageName, this.maxSizeInKilobytes * 1024);
                    this.IsPersistingEntries = !this.storage.IsReadOnly;
                }
                catch (IsolatedStorageException)
                {
                    // use in memory buffer only
                    this.DisposeStorage();
                }
            }
            else
            {
                try
                {
                    this.storage = StorageAccessor.TryOpen(storageName);
                }
                catch (IsolatedStorageException)
                {
                    // Workaround, because TryOpen should not surface any exception, but if the isolated storage is disabled, it still throws.
                }
            }
        }

        private void TrimBackingStore()
        {
            int i = 0;
            while (this.storage.UsedPhysicalSize > this.storage.MaxSize && i < this.entries.Count)
            {
                var current = this.entries[i];
                if (current.StorageId != null)
                {
                    try
                    {
                        this.storage.Remove(current.StorageId);
                        current.StorageId = null;
                    }
                    catch
                    {
                        // if cannot remove it, keep it as assigned in the memory buffer.
                    }
                }

                i++;
            }
        }


        private long ConvertBytesToKilobytes(long bytes)
        {
            long kb = bytes / 1024;
            if (bytes % 1024 > 0)
            {
                kb++;
            }

            return kb;
        }
    }
}
