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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="ICachingConfiguration"/> extensions to support configuring custom <see cref="ICacheManager"/> instances.
    /// </summary>
    /// <seealso cref="CustomCacheManagerData"/>
    /// <seealso cref="ICacheManager"/>
    public static class ForCustomCacheManagerNamedExtension
    {
        /// <summary>
        /// Adds a custom cache mananger of type <typeparamref name="TCustomCacheManager"/> to the caching configuration.
        /// </summary>
        /// <typeparam name="TCustomCacheManager">The concrete type of the custom cache manager.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheManagerName">The name of the cache manager that should be added to configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomCacheManagerData"/>. </returns>
        /// <seealso cref="CustomCacheManagerData"/>
        public static ICachingConfigurationCustomCacheManager ForCustomCacheManagerNamed<TCustomCacheManager>(this ICachingConfiguration context, string cacheManagerName)
            where TCustomCacheManager : ICacheManager
        {

            return ForCustomCacheManagerNamed(context, cacheManagerName, typeof(TCustomCacheManager), new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom cache mananger of type <paramref name="customCacheManagerType"/> to the caching configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheManagerName">The name of the cache manager that should be added to configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomCacheManagerData"/>. </returns>
        /// <param name="customCacheManagerType">The concrete type of the custom cache manager. This type must implement <see cref="ICacheManager"/>.</param>
        /// <seealso cref="CustomCacheManagerData"/>
        public static ICachingConfigurationCustomCacheManager ForCustomCacheManagerNamed(this ICachingConfiguration context, string cacheManagerName, Type customCacheManagerType)
        {
            return ForCustomCacheManagerNamed(context, cacheManagerName, customCacheManagerType, new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom cache mananger of type <typeparamref name="TCustomCacheManager"/> to the caching configuration.<br/>
        /// Specifying additional conifguration attributes.
        /// </summary>
        /// <typeparam name="TCustomCacheManager">The concrete type of the custom cache manager.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheManagerName">The name of the cache manager that should be added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomCacheManager"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomCacheManagerData"/>. </returns>
        /// <seealso cref="CustomCacheManagerData"/>
        public static ICachingConfigurationCustomCacheManager ForCustomCacheManagerNamed<TCustomCacheManager>(this ICachingConfiguration context, string cacheManagerName, NameValueCollection attributes)
            where TCustomCacheManager : ICacheManager
        {
            return ForCustomCacheManagerNamed(context, cacheManagerName, typeof(TCustomCacheManager), attributes);
        }

        /// <summary>
        /// Adds a custom cache mananger of type <paramref name="customCacheManagerType"/> to the caching configuration.<br/>
        /// Specifying additional conifguration attributes.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheManagerName">The name of the cache manager that should be added to configuration.</param>
        /// <param name="customCacheManagerType">The concrete type of the custom cache manager. This type must implement <see cref="ICacheManager"/>.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customCacheManagerType"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomCacheManagerData"/>. </returns>
        /// <seealso cref="CustomCacheManagerData"/>
        public static ICachingConfigurationCustomCacheManager ForCustomCacheManagerNamed(this ICachingConfiguration context, string cacheManagerName, Type customCacheManagerType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(cacheManagerName)) throw new ArgumentException(Resources.EmptyParameterName);
            if (customCacheManagerType == null) throw new ArgumentNullException("customCacheManagerType");
            if (attributes == null) throw new ArgumentNullException("attributes");

            return new ForCustomCacheManagerNamedBuilder(context, cacheManagerName, customCacheManagerType, attributes);
        }

        private class ForCustomCacheManagerNamedBuilder : CacheManagerSettingsExtension, ICachingConfigurationCustomCacheManager
        {
            CustomCacheManagerData cacheManagerData;
            public ForCustomCacheManagerNamedBuilder(ICachingConfiguration context, string cacheManagerName, Type customCacheManagerType, NameValueCollection attributes)
                : base(context)
            {
                cacheManagerData = new CustomCacheManagerData
                {
                    Name = cacheManagerName,
                    Type = customCacheManagerType
                };

                cacheManagerData.Attributes.Add(attributes);

                base.CachingSettings.CacheManagers.Add(cacheManagerData);
            }

            public ICachingConfigurationCustomCacheManager UseAsDefaultCache()
            {
                base.CachingSettings.DefaultCacheManager = cacheManagerData.Name;

                return this;
            }

        }
    }
}
