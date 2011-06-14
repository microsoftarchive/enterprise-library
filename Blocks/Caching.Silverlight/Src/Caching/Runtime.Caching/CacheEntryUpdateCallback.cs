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
    ///  Defines a reference to a method that is invoked when a cache entry is about to be removed from the cache.
    /// </summary>
    /// <param name="arguments">The information about the entry that is about to be removed from the cache.</param>
    /// <remarks>
    /// Information about a cache entry to be removed is contained in a <see cref="CacheEntryUpdateArguments"/> object.
    /// A <see cref="CacheEntryUpdateArguments"/> object is passed to the <see cref="CacheEntryUpdateCallback"/> delegate.
    /// The method that implements the <see cref="CacheEntryUpdateCallback"/> delegate can pass an updated cache entry value back to the cache implementation.
    /// The updated cache entry replaces the cache item that is about to be removed.
    /// </remarks>
    public delegate void CacheEntryUpdateCallback(CacheEntryUpdateArguments arguments);

    /// <summary>
    /// Provides information about a cache entry that will be removed from the cache.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The arguments in the <see cref="CacheEntryUpdateArguments"/> class contain details about an entry that the cache implementation is about to remove.
    /// The arguments include a key to the cache entry, a reference to the <see cref="ObjectCache"/> instance that the entry will be removed from, a reason 
    /// for the removal, and the region name in the cache that contains the entry. The constructor of the <see cref="CacheEntryUpdateArguments"/> class uses 
    /// these arguments to create a new instance of the class.
    /// A <see cref="CacheEntryUpdateArguments"/> object is passed to a <see cref="CacheEntryUpdateCallback"/> handler, which notifies the cache about 
    /// the entry to remove.
    /// </para>
    /// <para>
    /// Notes to Implementers.
    /// A callback handler must notify the cache implementation whether to insert a replacement entry into the cache in place of the cache entry that is 
    /// about to be removed. If you want to exchange cache entries, you must assign a value other than null to the 
    /// <see cref="CacheEntryUpdateArguments.UpdatedCacheItem"/>  property. Cache implementations will interpret a null value for the 
    /// <see cref="CacheEntryUpdateArguments.UpdatedCacheItem"/> property as a notice that the current cache entry should be removed but not replaced.
    /// </para>
    /// </remarks>
    public class CacheEntryUpdateArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntryUpdateArguments"/> class.
        /// </summary>
        /// <param name="source">The <see cref="ObjectCache"/> instance from which cacheItem was removed.</param>
        /// <param name="reason">One of the enumeration values that indicate why the item was removed.</param>
        /// <param name="key">The key of the cache entry that will be removed.</param>
        /// <param name="regionName">The name of the region in the cache to remove the cache entry from. This
        ///    parameter is optional. If cache regions are not defined, <paramref name="regionName"/> must be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        public CacheEntryUpdateArguments(ObjectCache source, CacheEntryRemovedReason reason, string key, string regionName)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (key == null)
                throw new ArgumentNullException("key");

            this.Source = source;
            this.RemovedReason = reason;
            this.Key = key;
            this.RegionName = regionName;
        }

        /// <summary>Gets the unique identifier for a cache entry that is about to be removed.</summary>
        /// <returns>The unique identifier for the cache entry.</returns>
        public string Key { get; private set; }

        /// <summary>Gets a value that indicates why a cache entry was removed.</summary>
        /// <returns>One of the enumeration values that indicates why the entry was removed.</returns>
        public CacheEntryRemovedReason RemovedReason { get; private set; }

        /// <summary>Gets a reference to the source <see cref="ObjectCache"/> instance that originally contained the removed cache entry.</summary>
        /// <returns>A reference to the cache that originally contained the removed cache entry.</returns>
        public ObjectCache Source { get; private set; }

        /// <summary>Gets the name of a region in the cache that contains a cache entry.</summary>
        /// <returns>The name of a region in the cache. If regions are not used, this value is <see langword="null"/>.</returns>
        public string RegionName { get; private set; }

        /// <summary>Gets or sets the value of <see cref="CacheItem"/> entry that is used to update the cache object.</summary>
        /// <returns>The cache entry to update in the cache object. The default is <see langword="null"/>.</returns>
        public CacheItem UpdatedCacheItem { get; set; }

        /// <summary>Gets or sets the cache eviction or expiration policy of the <see cref="CacheItem"/> entry that is updated.</summary>
        /// <returns>The cache eviction or expiration policy of the cache item that was updated. The default is <see langword="null"/>.</returns>
        public CacheItemPolicy UpdatedCacheItemPolicy { get; set; }
    }
}
