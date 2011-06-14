//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    /// <summary>
    /// Manages storage of entries to the application's Isolated Storage.
    /// </summary>
    public class StorageAccessor : IDisposable
    {
        // could use actual cluster size used in Isolated Storage's disk to improve accuracy, but it is not trivial to get correctly.
        private const int ClusterSize = 1024;
        private const int MaxSizeTypeSize = sizeof(long);
        
        private readonly string name;
        private readonly Dictionary<string, int> metadata = new Dictionary<string, int>();

        private IsolatedStorageFile store;
        private IsolatedStorageFileStream writeLockFile;
        private long maxSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageAccessor"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="maxSize">The maximum size in bytes.</param>
        public StorageAccessor(string name, long maxSize)
        {
            GuardMaxSize(maxSize);

            this.name = GetDirectoryName(name);

            this.maxSize = maxSize;

            this.store = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
                if (!store.DirectoryExists(this.name))
                {
                    this.SizeIsCurrent = true;
                    this.store.CreateDirectory(this.name);
                }

                this.writeLockFile = this.store.OpenFile(Path.Combine(this.name, "__writeLock"), FileMode.OpenOrCreate);
                if (!this.ReadPersistedMaxSize())
                {
                    this.PersistMaxSize(this.maxSize);
                }
            }
            catch
            {
                // The storage might be completely full, where not even the directory might be created
                // or the file is locked by another process (probably due to another instance of the application is running)
                // Either way, ignore and use the store in read-only mode.
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageAccessor"/> class. This constructor opens the storage if it's available, or throws if it's not already created.
        /// </summary>
        /// <param name="name">The name.</param>
        protected StorageAccessor(string name)
        {
            this.name = GetDirectoryName(name);

            this.store = IsolatedStorageFile.GetUserStoreForApplication();

            bool exists = false;
            try
            {
                if (store.DirectoryExists(this.name))
                {
                    string pathToLock = Path.Combine(this.name, "__writeLock");
                    if (store.FileExists(pathToLock))
                    {
                        exists = true;
                        this.writeLockFile = this.store.OpenFile(Path.Combine(this.name, "__writeLock"), FileMode.Open);
                        this.ReadPersistedMaxSize();
                    }
                }
            }
            catch
            {
                // The storage may not exist could not exist or is locked by another process (probably due to another instance of the application is running)
                // Either way, the storage cannot be opened.
            }
            if (!exists)
            {
                this.ReleaseFileSystem();
                throw new InvalidOperationException(Resources.StorageDoesNotExist);
            }
        }

        /// <summary>
        /// Tries to open the storage that is currently on disk. If there is not a storage already created, <see langword="null"/> will be returned.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The storage that was previously created, or <see langword="null"/> if it does not exist on disk.</returns>
        public static StorageAccessor TryOpen(string name)
        {
            try
            {
                return new StorageAccessor(name);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return this.writeLockFile == null; }
        }

        /// <summary>
        /// Gets the maximum size.
        /// </summary>
        public long MaxSize
        {
            get { return this.maxSize; }
        }

        /// <summary>
        /// Gets the logical size used for storage.
        /// </summary>
        public long UsedLogicalSize
        {
            get
            {
                if (!this.SizeIsCurrent)
                    throw new InvalidOperationException(Resources.ExceptionSizeIsNotCurrent);

                return this.metadata.Values.Sum();
            }
        }

        /// <summary>
        /// Gets an estimate of the physical size used for storage.
        /// </summary>
        public long UsedPhysicalSize
        {
            get
            {
                if (!this.SizeIsCurrent)
                    throw new InvalidOperationException(Resources.ExceptionSizeIsNotCurrent);

                return
                    this.metadata.Values.Sum((Func<int, int>)this.GetPhysicalSize)
                    + ClusterSize; // lock file size
            }
        }

        private int GetPhysicalSize(int logicalSize)
        {
            var remainder = logicalSize % ClusterSize;
            return remainder > 0 ? logicalSize + ClusterSize - remainder : logicalSize;
        }

        /// <summary>
        /// Deletes storage for the supplied storage name.
        /// </summary>
        /// <param name="name">The storage name.</param>
        public static void DeleteStorage(string name)
        {
            name = GetDirectoryName(name);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.DirectoryExists(name))
                {
                    var lockFilePath = Path.Combine(name, "__writeLock");
                    bool hasLock = false;
                    if (!store.FileExists(lockFilePath))
                    {
                        hasLock = true;
                    }
                    else
                    {
                        try
                        {
                            using (store.CreateFile(Path.Combine(name, "__writeLock")))
                            {
                                hasLock = true;
                            }
                        }
                        catch { } // cannot aquire lock
                    }

                    if (hasLock)
                    {
                        foreach (string fileName in store.GetFileNames(name + Path.DirectorySeparatorChar + "*.*"))
                        {
                            store.DeleteFile(Path.Combine(name, fileName));
                        }

                        store.DeleteDirectory(name);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>An id for the saved content.</returns>
        public string Save(byte[] content)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException(Resources.ExceptionWriteNotSupportedInReadOnlyStorage);

            if (content == null)
                throw new ArgumentNullException("content");

            if (content.Length + this.UsedPhysicalSize > this.MaxSize)
                throw new AllocationException(Resources.ExceptionAvailableSpaceNotEnough);

            if (this.GetPhysicalSize(content.Length) > this.store.AvailableFreeSpace)
                throw new AllocationException(Resources.ExceptionAvailableSpaceNotEnough);

            string id = CreateId();
            string fileName = GetFileName(id, false);
            try
            {
                using (var file = this.store.OpenFile(fileName, FileMode.Create, FileAccess.Write))
                {
                    file.Write(content, 0, content.Length);
                    file.Flush();
                }

                this.metadata[id] = content.Length;
            }
            catch (IsolatedStorageException ex)
            {
                this.store.DeleteFile(fileName);
                throw new AllocationException(Resources.ExceptionAvailableSpaceNotEnough, ex);
            }

            return id;
        }

        /// <summary>
        /// Overwrites a portion of the content.
        /// </summary>
        /// <param name="id">The block id.</param>
        /// <param name="content">The content to overwrite.</param>
        /// <param name="offset">The offset within the current block data where the content will be updated.</param>
        /// <remarks>This method should be used to overwrite bytes when there is a guarantee that it will fit the previous content size.
        /// For performance reasons, this method does not check what was the previous content size. If the data being updated should result in
        /// an updated content length, then this method should not be used, as the data will be corrupt. In this case, the alternative
        /// is to invoke the <see cref="Remove"/> method and then invoke <see cref="Save"/> to re-add it.</remarks>
        public void Overwrite(string id, byte[] content, int offset)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException(Resources.ExceptionWriteNotSupportedInReadOnlyStorage);
            if (content == null)
                throw new ArgumentNullException("content");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

            string fileName = GetFileName(id, true);

            using (var file = store.OpenFile(fileName, FileMode.Open, FileAccess.Write))
            {
                if (file.Length < offset + content.Length)
                {
                    throw new InvalidOperationException(Resources.ExceptionOverwriteOverflow);
                }

                file.Seek(offset, SeekOrigin.Current);
                file.Write(content, 0, content.Length);
                file.Flush();
            }
        }

        /// <summary>
        /// Returns a dictionary with all the content saved in the storage.
        /// </summary>
        /// <returns>A dictionary with ids as keys and the saved content as values.</returns>
        public IDictionary<string, byte[]> ReadAll()
        {
            this.metadata.Clear();
            var contents = new Dictionary<string, byte[]>();

            foreach (var id in GetIds())
            {
                var content = this.Read(id);
                contents.Add(id, content);
                this.metadata[id] = content != null ? content.Length : 0;
            }

            this.SizeIsCurrent = true;

            return contents;
        }

        /// <summary>
        /// Removes the entry identified by <paramref name="id"/> from the storage.
        /// </summary>
        /// <param name="id">The entry id.</param>
        public void Remove(string id)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException(Resources.ExceptionWriteNotSupportedInReadOnlyStorage);

            string fileName = GetFileName(id, false);
            if (store.FileExists(fileName))
            {
                store.DeleteFile(fileName);
            }

            this.metadata.Remove(id);
        }

        /// <summary>
        /// Changes the Maximum size that can be used by the storage. This method changes the max size, but does not trim entries if
        /// the new max size is smaller.
        /// </summary>
        /// <param name="maxSize">The new maximum size.</param>
        /// <exception cref="InvalidOperationException">when the storage is in read-only mode.</exception>
        public void ChangeMaxSize(long maxSize)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException(Resources.ExceptionWriteNotSupportedInReadOnlyStorage);

            GuardMaxSize(maxSize);

            this.PersistMaxSize(maxSize);
            this.maxSize = maxSize;
        }

        private static string GetDirectoryName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            name = name.Trim();
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "name");

            Regex nameRegex = new Regex(@"[^\w\d_]");
            name = nameRegex.Replace(name, "_");
            name = "__" + name;
            return name;
        }

        private string GetFileName(string id, bool verifyExists)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "id");

            var fileName = Path.Combine(this.name, id + ".dat");

            if (verifyExists && !this.store.FileExists(fileName))
            {
                throw new ArgumentException(Resources.ExceptionIdDoesNotExistInFileSystem);
            }

            return fileName;
        }

        private string CreateId()
        {
            while (true)
            {
                string id = Guid.NewGuid().ToString();
                if (!this.metadata.ContainsKey(id))
                {
                    string fileName = GetFileName(id, false);

                    if (!this.store.FileExists(fileName))
                    {
                        return id;
                    }
                }
            }
        }

        private byte[] Read(string id)
        {
            string fileName = GetFileName(id, true);
            try
            {
                using (var file = store.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[file.Length];
                    file.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
            catch (IsolatedStorageException)
            {
                return null;
            }
        }

        private IEnumerable<string> GetIds()
        {
            return GetFileNames().Select(Path.GetFileNameWithoutExtension).ToList();
        }

        private IEnumerable<string> GetFileNames()
        {
            if (store.DirectoryExists(this.name))
            {
                return
                    store.GetFileNames(this.name + Path.DirectorySeparatorChar + "*.dat").Select(
                        x => Path.Combine(this.name, x));
            }

            return Enumerable.Empty<string>();
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
                this.ReleaseFileSystem();
            }
        }

        private void ReleaseFileSystem()
        {
            using (this.store) this.store = null;
            using (this.writeLockFile) this.writeLockFile = null;
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        ~StorageAccessor()
        {
            Dispose(false);
        }

        private bool SizeIsCurrent { get; set; }

        private void PersistMaxSize(long size)
        {
            this.writeLockFile.Position = 0;
            var bytes = BitConverter.GetBytes(size);
            this.writeLockFile.Write(bytes, 0, MaxSizeTypeSize);
        }

        private bool ReadPersistedMaxSize()
        {
            if (this.writeLockFile.Length >= MaxSizeTypeSize)
            {
                var bytes = new byte[MaxSizeTypeSize];
                if (this.writeLockFile.Read(bytes, 0, bytes.Length) == MaxSizeTypeSize)
                {
                    var size = BitConverter.ToInt64(bytes, 0);
                    if (size > 0)
                    {
                        this.maxSize = size;
                        return true;
                    }
                }
            }

            return false;
        }

        private static void GuardMaxSize(long maxSize)
        {
            const long MinimumSize = ClusterSize * 2;

            if (maxSize < MinimumSize)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StorageAccessor_GuardMaxSize, MinimumSize), "maxSize");
        }
    }
}
