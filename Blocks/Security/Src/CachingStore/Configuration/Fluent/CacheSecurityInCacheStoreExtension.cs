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
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureSecuritySettings"/> extension that allows to add <see cref="CachingStoreProvider"/> instances to the security configuration.
    /// </summary>
    /// <seealso cref="CachingStoreProvider"/>
    /// <seealso cref="CachingStoreProviderData"/>
    public static class CacheSecurityInCacheStoreExtension
    {
        /// <summary>
        /// Adds a new <see cref="CachingStoreProvider"/> instance to the security configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cachingStoreName">The name of the <see cref="CachingStoreProvider"/> instance that will be added.</param>
        /// <seealso cref="CachingStoreProvider"/>
        /// <seealso cref="CachingStoreProviderData"/>
        public static ICacheSecurityInCacheStore CacheSecurityInCacheStoreNamed(this IConfigureSecuritySettings context, string cachingStoreName)
        {
            if (string.IsNullOrEmpty(cachingStoreName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "cachingStoreName");

            return new CacheUsingCachingBlockBuilder(context, cachingStoreName);
        }

        private class CacheUsingCachingBlockBuilder : ConfigureSecuritySettingsExtension, ICacheSecurityInCacheStore, ICacheSecurityInCacheStoreOptions
        {
            CachingStoreProviderData cachingStoreData;

            public CacheUsingCachingBlockBuilder(IConfigureSecuritySettings context, string cachingStoreName)
                :base(context)
            {
                cachingStoreData = new CachingStoreProviderData
                {
                    Name = cachingStoreName
                };

                SecuritySettings.SecurityCacheProviders.Add(cachingStoreData);
            }
        
            public ICacheSecurityInCacheStoreOptions  AbsoluteExpiration(TimeSpan timeSpan)
            {
                cachingStoreData.AbsoluteExpiration = timeSpan.Minutes;

                return this;
            }

            public ICacheSecurityInCacheStoreOptions  SlidingExpiration(TimeSpan timeSpan)
            {
                cachingStoreData.SlidingExpiration = timeSpan.Minutes;

                return this;
            }

            public ICacheSecurityInCacheStoreOptions UseSharedCacheManager(string cacheManagerName)
            {
                if (string.IsNullOrEmpty(cacheManagerName)) 
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "cacheManagerName");

                cachingStoreData.CacheManager = cacheManagerName;

                return this;
            }

            public ICacheSecurityInCacheStoreOptions  WithOptions
            {
                get { return this ; }
            }

            public ICacheSecurityInCacheStore SetAsDefault()
            {
                SecuritySettings.DefaultSecurityCacheProviderName = cachingStoreData.Name;

                return this;
            }
        }
    }
}
