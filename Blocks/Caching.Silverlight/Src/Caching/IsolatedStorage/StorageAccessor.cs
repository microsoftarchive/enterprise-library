using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class StorageAccessor : IDisposable
    {
        // could use actual cluster size used in Isolated Storage's disk to improve accuracy, but it is not trivial to get correctly.
        private const int ClusterSize = 1024;

        private readonly string name;
        private readonly long maxSize;
        private readonly Dictionary<string, int> metadata = new Dictionary<string, int>();

        private IsolatedStorageFile store;
        private IsolatedStorageFileStream writeLockFile;

        public StorageAccessor(string name, long maxSize)
        {
            this.name = GetDirectoryName(name);

            this.maxSize = maxSize;

            this.store = IsolatedStorageFile.GetUserStoreForApplication();
            if (!store.DirectoryExists(this.name))
            {
                this.store.CreateDirectory(this.name);
                this.SizeIsCurrent = true;
            }

            try
            {
                this.writeLockFile = this.store.CreateFile(Path.Combine(this.name, "__writeLock"));
            }
            catch
            {
                // ignore and use the store in read-only mode
            }
        }

        public long MaxSize
        {
            get { return this.maxSize; }
        }

        public long UsedLogicalSize
        {
            get
            {
                if (!this.SizeIsCurrent)
                    throw new InvalidOperationException(Resources.ExceptionSizeIsNotCurrent);

                return this.metadata.Values.Sum();
            }
        }

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

        public static void DeleteStorage(string name)
        {
            name = GetDirectoryName(name);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.DirectoryExists(name))
                {
                    foreach (string fileName in store.GetFileNames(name + Path.DirectorySeparatorChar + "*.*"))
                    {
                        store.DeleteFile(Path.Combine(name, fileName));
                    }

                    store.DeleteDirectory(name);
                }
            }
        }

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

        private static string GetDirectoryName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            name = name.Trim();
            if (name == string.Empty)
                throw new ArgumentException("name");

            Regex nameRegex = new Regex(@"[^\w\d_]");
            name = nameRegex.Replace(name, "_");
            name = "Cache_" + name;
            return name;
        }

        private string GetFileName(string id, bool verifyExists)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (id == string.Empty)
                throw new ArgumentException("id");

            var fileName = Path.Combine(this.name, id + ".cache");

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
            var ids = new List<string>();
            foreach (var fileName in GetFileNames())
            {
                ids.Add(Path.GetFileNameWithoutExtension(fileName));
            }

            return ids;
        }

        private IEnumerable<string> GetFileNames()
        {
            return store.GetFileNames(this.name + Path.DirectorySeparatorChar + "*.cache").Select(x => Path.Combine(this.name, x));
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
                using (this.store) this.store = null;
                using (this.writeLockFile) this.writeLockFile = null;
            }
        }

        ~StorageAccessor()
        {
            Dispose(false);
        }

        private bool SizeIsCurrent { get; set; }

        public bool IsReadOnly
        {
            get { return this.writeLockFile == null; }
        }
    }
}
