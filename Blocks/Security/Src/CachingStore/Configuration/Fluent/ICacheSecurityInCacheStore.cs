//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Security;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface that is used to configure <see cref="CachingStoreProvider"/> instances.
    /// </summary>
    /// <seealso cref="CachingStoreProvider"/>
    /// <seealso cref="CachingStoreProviderData"/>
    public interface ICacheSecurityInCacheStore : IConfigureSecuritySettings, IFluentInterface 
    {
        /// <summary>
        /// Specifies this <see cref="CachingStoreProvider"/> will be used as the default <see cref="ISecurityCacheProvider"/>.
        /// </summary>
        /// <seealso cref="CachingStoreProvider"/>
        /// <seealso cref="CachingStoreProviderData"/>
        ICacheSecurityInCacheStore SetAsDefault();

        /// <summary>
        /// Returns a fluent interface to further configure this <see cref="CachingStoreProvider"/> instance.
        /// </summary>
        /// <seealso cref="CachingStoreProvider"/>
        /// <seealso cref="CachingStoreProviderData"/>
        ICacheSecurityInCacheStoreOptions WithOptions { get; }
    }

    /// <summary>
    /// Fluent interface that is further configure <see cref="CachingStoreProvider"/> instances.
    /// </summary>
    /// <seealso cref="CachingStoreProvider"/>
    /// <seealso cref="CachingStoreProviderData"/>
    public interface ICacheSecurityInCacheStoreOptions : ICacheSecurityInCacheStore, IFluentInterface 
    {
        /// <summary>
        /// Specified the absolute expiration for security information added to the <see cref="ICacheManager"/>. 
        /// </summary>
        /// <param name="timeSpan">The absolute expiration for security information added to the <see cref="ICacheManager"/>.</param>
        /// <seealso cref="ICacheManager"/>
        /// <seealso cref="CachingStoreProvider"/>
        /// <seealso cref="CachingStoreProviderData"/>
        ICacheSecurityInCacheStoreOptions AbsoluteExpiration(TimeSpan timeSpan);

        /// <summary>
        /// Specified the sliding expiration for security information added to the <see cref="ICacheManager"/>. 
        /// </summary>
        /// <param name="timeSpan">The sliding expiration for security information added to the <see cref="ICacheManager"/>.</param>
        /// <seealso cref="ICacheManager"/>
        /// <seealso cref="CachingStoreProvider"/>
        /// <seealso cref="CachingStoreProviderData"/>
        ICacheSecurityInCacheStoreOptions SlidingExpiration(TimeSpan timeSpan);

        /// <summary>
        /// Specifies the <see cref="ICacheManager"/> instance that should be used to store security information.
        /// </summary>
        /// <param name="cacheManagerName">The name of the <see cref="ICacheManager"/> instance that should be used to store security information.</param>
        /// <seealso cref="ICacheManager"/>
        /// <seealso cref="CachingStoreProvider"/>
        /// <seealso cref="CachingStoreProviderData"/>
        ICacheSecurityInCacheStoreOptions UseSharedCacheManager(string cacheManagerName);
    }
}
