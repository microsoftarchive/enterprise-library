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

        /// <summary>
        /// Resizes the backing size, trying to preserve all the entries.
        /// </summary>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes.</param>
        /// <exception cref="InvalidOperationException">when the repository is not available.</exception>
        /// <exception cref="IOException">when the repository cannot be re-initialized.</exception>
        void ResizeBackingStore(int maxSizeInKilobytes);
    }
}
