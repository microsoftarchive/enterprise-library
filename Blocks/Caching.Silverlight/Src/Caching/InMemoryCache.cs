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
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Represents the type that implements an in-memory cache.
    /// </summary>
    /// <remarks>
    /// The <see cref="InMemoryCache"/> class is a concrete implementation of the abstract <see cref="ObjectCache"/> class.
    /// <para>
    /// Note: The <see cref="InMemoryCache"/> class is somewhat similar to the System.Runtime.Caching.MemoryCache class available in the .NET Framework in the Desktop.
    /// The <see cref="InMemoryCache"/> class has many properties and methods for accessing the cache that will be familiar to you if you have used the Desktop's MemoryCache class.
    /// The MemoryCache class does not allow null as a value in the cache. Any attempt to add or change a cache entry with a value of null will fail.
    /// </para>
    /// <para> The <see cref="InMemoryCache"/> type does not implement cache regions. Therefore, when you call <see cref="InMemoryCache"/> methods that 
    /// implement base methods that contain a parameter for regions, do not pass a value for the parameter. The methods that use the region parameter 
    /// all supply a default <see langword="null"/> value. For example, the <see cref="MemoryBackedCacheBase{CacheEntry}.AddOrGetExisting(string, object, CacheItemPolicy, string)"/> 
    /// method overload has a regionName parameter whose default value is <see langword="null"/>.</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class InMemoryCache : MemoryBackedCacheBase<CacheEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCache"/> class.
        /// </summary>
        /// <param name="name">The name to use to look up configuration information.</param>
        /// <param name="maxItemsBeforeScavenging">Maximum number of items in cache before an add causes scavenging to take place.</param>
        /// <param name="itemsLeftAfterScavenging">Number of items left in the cache after scavenging has taken place.</param>
        /// <param name="expirationPollingInterval">Frequency of expiration polling cycle.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Explicitly checked that exceptions cannot be thrown by current implementations and/or objects are disposed correctly.")]
        public InMemoryCache(string name, int maxItemsBeforeScavenging, int itemsLeftAfterScavenging, TimeSpan expirationPollingInterval)
            : this(name, maxItemsBeforeScavenging, itemsLeftAfterScavenging, new ScavengingScheduler(), new RecurringWorkScheduler(expirationPollingInterval))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCache"/> class.
        /// </summary>
        /// <param name="name">The name to use to look up configuration information.</param>
        /// <param name="maxItemsBeforeScavenging">Maximum number of items in cache before an add causes scavenging to take place.</param>
        /// <param name="itemsLeftAfterScavenging">Number of items left in the cache after scavenging has taken place.</param>
        /// <param name="scavengingScheduler">The scavenging scheduler.</param>
        /// <param name="expirationScheduler">The expiration scheduler.</param>
        public InMemoryCache(string name, int maxItemsBeforeScavenging, int itemsLeftAfterScavenging,
            IManuallyScheduledWork scavengingScheduler,
            IRecurringWorkScheduler expirationScheduler)
            : base(name, 
                new NumberOfItemsScavengingStrategy<CacheEntry>(maxItemsBeforeScavenging, itemsLeftAfterScavenging),
                scavengingScheduler, expirationScheduler)
        {
        }

        /// <summary>
        /// Gets a description of the features that a cache implementation provides.
        /// </summary>
        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return DefaultCacheCapabilities.InMemoryProvider |
                    DefaultCacheCapabilities.CacheEntryUpdateCallback |
                        DefaultCacheCapabilities.CacheEntryRemovedCallback |
                            DefaultCacheCapabilities.SlidingExpirations |
                                DefaultCacheCapabilities.AbsoluteExpirations;
            }
        }

        /// <summary>
        /// Creates a cache entry.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object for the cache entry.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>A new cache entry of type <see cref="CacheEntry"/>.</returns>
        protected override CacheEntry CreateCacheEntry(string key, object value, CacheItemPolicy policy)
        {
            return new CacheEntry(key, value, policy);
        }
    }
}
