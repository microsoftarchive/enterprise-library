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

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// A cache entry for the <see cref="IsolatedStorageCache"/>.
    /// </summary>
    public class IsolatedStorageCacheEntry : CacheEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCacheEntry"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="policy">The policy.</param>
        public IsolatedStorageCacheEntry(string key, object value, CacheItemPolicy policy)
            : base(key, value, policy)
        {
        }

        /// <summary>
        /// Gets or sets the storage id for the entry.
        /// </summary>
        public string StorageId { get; set; }

        /// <summary>
        /// Gets or sets the last access time for the entry.
        /// </summary>
        public new DateTimeOffset LastAccessTime
        {
            get { return base.LastAccessTime; }
            set { base.LastAccessTime = value; }
        }
    }
}
