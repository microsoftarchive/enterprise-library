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
using System.IO;

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

        /// <summary>
        /// Gets the maximum size in kilobytes as available when the storage was initialized.
        /// </summary>
        int ActualMaxSizeInKilobytes { get; }

        /// <summary>
        /// Resizes the backing size, trying to preserve all the entries.
        /// </summary>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes.</param>
        /// <exception cref="InvalidOperationException">when the repository is not available.</exception>
        /// <exception cref="IOException">when the repository cannot be re-initialized.</exception>
        void Resize(int maxSizeInKilobytes);
    }
}
