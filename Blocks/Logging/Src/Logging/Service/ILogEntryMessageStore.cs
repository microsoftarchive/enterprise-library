using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Manages the storage and retrieval of log entries.
    /// </summary>
    public interface ILogEntryMessageStore
    {
        /// <summary>
        /// Gets a value indicating whether this instance is writable.
        /// </summary>
        bool IsWritable { get; }

        /// <summary>
        /// Gets the quota allowed for the store.
        /// </summary>
        long Quota { get; }

        /// <summary>
        /// Gets an estimate of the physical size used by the store.
        /// </summary>
        long UsedPhysicalSize { get; }

        /// <summary>
        /// Stores a new entry.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        void Add(LogEntryMessage entry);

        ///// <summary>
        ///// Removes an entry from store.
        ///// </summary>
        ///// <param name="entry">The entry to remove.</param>
        //void Remove(LogEntryMessage entry);

        /// <summary>
        /// Removes several entries from store, from the first entry, until it finds <paramref name="entry"/>. If <paramref name="entry"/> is not found, no entry will be removed.
        /// </summary>
        /// <param name="entry">The last entry to remove.</param>
        void RemoveUntil(LogEntryMessage entry);

        /// <summary>
        /// Retrieves all the entries currently stored by the store.
        /// </summary>
        /// <returns></returns>
        LogEntryMessage[] GetEntries();
    }
}
