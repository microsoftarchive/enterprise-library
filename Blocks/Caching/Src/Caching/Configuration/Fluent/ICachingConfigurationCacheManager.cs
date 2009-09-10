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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{ 
    /// <summary>
    /// Fluent interface used to configure a <see cref="CacheManager"/> instance.
    /// </summary>
    /// <seealso cref="CacheManager"/>
    /// <seealso cref="CacheManagerData"/>
    public interface ICachingConfigurationCacheManager : IFluentInterface
    {

        /// <summary>
        /// Specifies the current <see cref="CacheManager"/> as the default cache manager instance.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure this <see cref="CacheManager"/>.</returns>
        /// <seealso cref="CacheManager"/>
        /// <seealso cref="CacheManagerData"/>
        ICachingConfigurationCacheManager UseAsDefaultCache();

        /// <summary>
        /// Returns a fluent interface to further configure this <see cref="CacheManager"/> instance.
        /// </summary>
        ICachingConfigurationCacheManagerOptions WithOptions { get; }

        /// <summary>
        /// Specifies cache items should be stored using a previously configured <see cref="IBackingStore"/> of name <paramref name="backingStoreName"/>.
        /// </summary>
        /// <param name="backingStoreName">The name of the backing store that should be used to store cache items.</param>
        /// <returns>Fluent interface that can be used to further configure caching configuration.</returns>
        /// <seealso cref="CacheManager"/>
        /// <seealso cref="CacheManagerData"/>
        ICachingConfiguration StoreInSharedBackingStore(string backingStoreName);

        /// <summary>
        /// Specifies cache items should not be persisted, but kept in memory using a <see cref="NullBackingStore"/>.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure caching configuration.</returns>
        /// <seealso cref="NullBackingStore"/>
        /// <seealso cref="CacheManager"/>
        /// <seealso cref="CacheManagerData"/>
        ICachingConfiguration StoreInMemory();

    }
}
