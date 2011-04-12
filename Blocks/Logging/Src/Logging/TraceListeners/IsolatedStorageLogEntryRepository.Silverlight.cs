using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Stores log entries in isolated storage, keeping the file size under a specified maximum.
    /// </summary>
    public class IsolatedStorageLogEntryRepository : ILogEntryRepository
    {
        private const string RepositoryDirectory = "__logging";
        private const float sizeScalingFactor = 0.9F;
        private const int sizeScalingRetries = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageLogEntryRepository"/> with a name and a
        /// maximum size.
        /// </summary>
        /// <remarks>
        /// If the instance cannot be initialized (e.g. the storage file cannot be opened) then the instance will
        /// not be available and the operations will throw an <see cref="InvalidOperationException"/>.
        /// </remarks>
        /// <param name="storageName">The name of the storage file.</param>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes.</param>
        public IsolatedStorageLogEntryRepository(string storageName, int maxSizeInKilobytes)
        {
            Guard.ArgumentNotNullOrEmpty(storageName, "storageName");

            var repositoryFileName = Path.Combine(RepositoryDirectory, storageName);

            var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream storageStream;

            if (isolatedStorageFile.FileExists(repositoryFileName))
            {
                try
                {
                    storageStream = isolatedStorageFile.OpenFile(repositoryFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IsolatedStorageException)
                {
                    storageStream = null;
                }
            }
            else
            {
                storageStream = InitializeRepositoryStream(repositoryFileName, maxSizeInKilobytes);
            }

            if (storageStream != null)
            {
                this.storage = new BoundedStreamStorage(storageStream);
                this.serializer = new LogEntrySerializer();
            }
        }

        private readonly BoundedStreamStorage storage;
        private readonly LogEntrySerializer serializer;

        /// <summary>
        /// Gets the maximum size in kilobytes as originally requested.
        /// </summary>
        public int MaxSizeInKilobytes
        {
            get
            {
                return this.storage.MaxSizeInBytes / 1024;
            }
        }

        /// <summary>
        /// Gets the maximum size in kilobytes as available when the storage was initialized.
        /// </summary>
        public int ActualMaxSizeInKilobytes
        {
            get
            {
                return this.storage.ActualMaxSizeInBytes / 1024;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the repository is available.
        /// </summary>
        /// <remarks>
        /// When not available, all the operations will throw an <see cref="InvalidOperationException"/>.
        /// </remarks>
        public bool IsAvailable
        {
            get
            {
                return this.storage != null;
            }
        }

        /// <summary>
        /// Initializes file in isolated storage to be used as a repository.
        /// </summary>
        /// <param name="repositoryFileName">The name of the repository file name.</param>
        /// <param name="maxSizeInKilobytes">The requested maximum size in kilobytes</param>
        /// <returns>The initialized stream.</returns>
        /// <remarks>
        /// Initialization will attempt to use the specified maximum size in kilobytes, but if not enough room is available
        /// it will use the maximum available size as a fallback.
        /// </remarks>
        /// <exception cref="ArgumentException">when the specified file already exists.</exception>
        public static IsolatedStorageFileStream InitializeRepositoryStream(string repositoryFileName, int maxSizeInKilobytes)
        {
            var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();

            if (isolatedStorageFile.FileExists(repositoryFileName))
            {
                throw new ArgumentException(Resources.ErrorRepositoryFileAlreadyExists, "repositoryFileName");
            }

            if (!isolatedStorageFile.DirectoryExists(RepositoryDirectory))
            {
                try
                {
                    isolatedStorageFile.CreateDirectory(RepositoryDirectory);
                }
                catch (IsolatedStorageException)
                {
                    if (!isolatedStorageFile.DirectoryExists(RepositoryDirectory))
                    {
                        throw;
                    }
                }
            }

            var storageStream = isolatedStorageFile.CreateFile(repositoryFileName);

            var maxSizeInBytes = maxSizeInKilobytes * 1024;

            var attempt = 1;

            while (true)
            {
                var effectiveMaxSizeInBytes =
                    isolatedStorageFile.AvailableFreeSpace >= maxSizeInBytes
                        ? maxSizeInBytes
                        : (int)(isolatedStorageFile.AvailableFreeSpace * sizeScalingFactor);

                try
                {
                    storageStream.SetLength(effectiveMaxSizeInBytes);
                }
                catch (IsolatedStorageException)
                {
                    if (attempt++ > sizeScalingRetries)
                    {
                        storageStream.Close();
                        throw;
                    }

                    continue;
                }

                BoundedStreamStorage.Initialize(storageStream, maxSizeInBytes, effectiveMaxSizeInBytes);

                return storageStream;
            }
        }

        /// <summary>
        /// Computes the isolated storage file name for a repository name.
        /// </summary>
        /// <param name="repositoryName">The repository name.</param>
        /// <returns>The isolated storage file name.</returns>
        public static string GetRepositoryFileName(string repositoryName)
        {
            return Path.Combine(RepositoryDirectory, repositoryName);
        }

        /// <summary>
        /// Adds an entry to the repository.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        /// <remarks>
        /// Adding an entry can result in older entries to be discarded.
        /// </remarks>
        /// <exception cref="InvalidOperationException">when the repository is not available.</exception>
        public void Add(LogEntry entry)
        {
            this.CheckAvailable();

            this.storage.Add(this.serializer.Serialize(entry));
        }

        /// <summary>
        /// Returns a copy of the entries currently stored in the repository.
        /// </summary>
        /// <returns>The entries.</returns>
        /// <exception cref="InvalidOperationException">when the repository is not available.</exception>
        public IEnumerable<LogEntry> RetrieveEntries()
        {
            this.CheckAvailable();

            return this.storage.RetrieveEntries().Select(bytes => this.serializer.Deserialize(bytes)).ToArray();
        }

        /// <summary>
        /// Flushes the underlying storage.
        /// </summary>
        public void Flush()
        {
            this.CheckAvailable();

            this.storage.Flush();
        }

        private void CheckAvailable()
        {
            if (!this.IsAvailable)
            {
                throw new InvalidOperationException(Resources.ErrorRepositoryIsNotAvailable);
            }
        }

        /// <summary>
        /// Dispose of the repository before garbage collection.
        /// </summary>
        ~IsolatedStorageLogEntryRepository()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose of the repository before garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the repository before garbage collection.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> if disposing; otherwise, <see langword="false"/>.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.storage != null)
                {
                    this.storage.Dispose();
                }
            }
        }
    }
}
