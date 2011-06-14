//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Manages the storage and retrieval of cache entries.
    /// </summary>
    public interface ICacheEntryStore
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
        void Add(IsolatedStorageCacheEntry entry);

        /// <summary>
        /// Removes an entry from storage.
        /// </summary>
        /// <param name="entry">The entry to remove.</param>
        void Remove(IsolatedStorageCacheEntry entry);

        /// <summary>
        /// Updates the last access time for the entry in storage.
        /// </summary>
        /// <param name="entry">The entry to update.</param>
        void UpdateLastUpdateTime(IsolatedStorageCacheEntry entry);

        /// <summary>
        /// Retrieves all the entries currently stored by the store.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is time-consuming as has side-effects.")]
        IEnumerable<IsolatedStorageCacheEntry> GetSerializedEntries();
    }
}
