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
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
    /// <summary>
    /// This class is used when no backing store is needed to support the caching storage policy.
    /// Its job is to provide an implementation of a backing store that does nothing, merely enabling
    /// the cache to provide a strictly in-memory cache.
    /// </summary>
#if !SILVERLIGHT
    [Common.Configuration.ConfigurationElementType(typeof(Configuration.CacheStorageData))]
#endif
    public class NullBackingStore : IBackingStore
    {
        /// <summary>
        /// Always returns 0
        /// </summary>
        public int Count
        {
            get { return 0; }
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="newCacheItem">Not used</param>
        public void Add(CacheItem newCacheItem)
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="key">Not used</param>
        public void Remove(string key)
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="key">Not used</param>
        /// <param name="timestamp">Not used</param>
        public void UpdateLastAccessedTime(string key, DateTime timestamp)
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void Flush()
        {
        }

        /// <summary>
        /// Always returns an empty hash table.
        /// </summary>
        /// <returns>Empty hash table</returns>
        public IDictionary Load()
        {
            return new Dictionary<object, object>();
        }

        /// <summary>
        /// Empty dispose implementation
        /// </summary>
        public void Dispose()
        {

        }
    }
}
