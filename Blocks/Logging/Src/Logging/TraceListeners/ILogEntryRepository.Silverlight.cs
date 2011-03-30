using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Stores log entries.
    /// </summary>
    public interface ILogEntryRepository : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the repository is available.
        /// </summary>
        /// <remarks>
        /// When not available, all the operations will throw an <see cref="InvalidOperationException"/>.
        /// </remarks>
        bool IsAvailable { get; }

        /// <summary>
        /// Adds an entry to the repository.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        /// <remarks>
        /// Adding an entry can result in older entries to be discarded.
        /// </remarks>
        /// <exception cref="InvalidOperationException">when the repository is not available.</exception>
        void Add(LogEntry entry);

        /// <summary>
        /// Returns a copy of the entries currently stored in the repository.
        /// </summary>
        /// <returns>The entries.</returns>
        /// <exception cref="InvalidOperationException">when the repository is not available.</exception>
        IEnumerable<LogEntry> RetrieveEntries();

        /// <summary>
        /// Flushes the underlying storage.
        /// </summary>
        void Flush();
    }
}
