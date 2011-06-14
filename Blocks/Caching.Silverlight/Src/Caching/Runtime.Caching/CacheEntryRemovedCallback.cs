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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    /// <summary>
    /// Defines a reference to a method that is called after a cache entry is removed from the cache.
    /// </summary>
    /// <param name="arguments">The information about the cache entry that was removed from the cache.</param>
    public delegate void CacheEntryRemovedCallback(CacheEntryRemovedArguments arguments);
        
    /// <summary>
    /// Provides information about a cache entry that was removed from the cache.
    /// </summary>
    public class CacheEntryRemovedArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntryRemovedArguments"/> class.
        /// </summary>
        /// <param name="source">The <see cref="ObjectCache"/> instance from which cacheItem was removed.</param>
        /// <param name="reason">One of the enumeration values that indicate why <paramref name="cacheItem"/> was removed.</param>
        /// <param name="cacheItem">An instance of the cached entry that was removed.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cacheItem"/> is <see langword="null"/>.
        /// </exception>
        public CacheEntryRemovedArguments(ObjectCache source, CacheEntryRemovedReason reason, CacheItem cacheItem)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (cacheItem == null)
                throw new ArgumentNullException("cacheItem");

            this.Source = source;
            this.RemovedReason = reason;
            this.CacheItem = cacheItem;
        }

        /// <summary>Gets an instance of a cache entry that was removed from the cache.</summary>
        /// <returns>An instance of the <see cref="CacheItem"/> class that was removed from the cache.</returns>
        public CacheItem CacheItem { get; private set; }
        
        /// <summary>Gets a value that indicates why a cache entry was removed.</summary>
        /// <returns>One of the enumeration values that indicates why the entry was removed.</returns>
        public CacheEntryRemovedReason RemovedReason { get; private set; }
        
        /// <summary>Gets a reference to the source <see cref="ObjectCache"/> instance that originally contained the removed cache entry.</summary>
        /// <returns>A reference to the cache that originally contained the removed cache entry.</returns>
        public ObjectCache Source { get; private set; }
    }
}
