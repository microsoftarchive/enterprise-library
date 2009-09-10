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
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Base class for fluent interface builders that extend the <see cref="ICachingConfigurationCacheManager"/> interface.
    /// </summary>
    public abstract class CacheManagerExtension : ICachingConfigurationCacheStorageExtension
    {
        ICachingConfigurationCacheManagerExtension contextExtension;
        CacheStorageData currentBackingStore =  null;

        /// <summary>
        /// Creates an instance of <see cref="CacheManagerExtension"/> passing the current <see cref="CacheManager"/>'s fluent interface builder.
        /// </summary>
        /// <param name="context">The current <see cref="CacheManager"/>'s fluent interface builder.<br/>
        /// This interface must implement <see cref="ICachingConfigurationCacheManagerExtension"/>.</param>
        protected CacheManagerExtension(ICachingConfigurationCacheManager context)
        {
            contextExtension = context as ICachingConfigurationCacheManagerExtension;

            if (contextExtension == null) throw new ArgumentException(
                string.Format(Resources.Culture, Resources.ExceptionParameterMustImplement, typeof(ICachingConfigurationCacheManagerExtension).FullName),
                "context");
        }

        /// <summary>
        /// Adds a <see cref="CacheStorageData"/> to the <see cref="CacheManagerSettings"/> as well as adds a reference to the <see cref="CacheManager"/> instance currently being configured.
        /// </summary>
        /// <param name="backingStore">The <see cref="CacheStorageData"/> that should be added to configuration.</param>
        protected void AddBackingStoreToCachingConfigurationAndCurrentCacheManager(CacheStorageData backingStore)
        {
            currentBackingStore = backingStore;
            contextExtension.CachingSettings.BackingStores.Add(backingStore);
            contextExtension.CacheManager.CacheStorage = backingStore.Name;
        }

        /// <summary>
        /// Returns the <see cref="CacheManagerData"/> instance that corresponds to the <see cref="CacheManager"/> being configured.
        /// </summary>
        protected CacheManagerData CacheManager
        {
            get
            {
                return contextExtension.CacheManager;
            }
        }

        /// <summary>
        /// Returns the <see cref="CacheManagerSettings"/> instance that is currently being build up.
        /// </summary>
        protected CacheManagerSettings CachingSettings
        {
            get
            {
                return contextExtension.CachingSettings;
            }
        }

        CacheStorageData ICachingConfigurationCacheStorageExtension.CacheStorage
        {
            get { return currentBackingStore; }
        }

        CacheManagerSettings ICachingConfigurationCacheStorageExtension.CachingSettings
        {
            get { return contextExtension.CachingSettings; }
        }

    }


    /// <summary>
    /// Allows access to the underlying configuration classes that are used for the <see cref="CacheManager"/> instance being configured.
    /// </summary>
    public interface ICachingConfigurationCacheManagerExtension : IFluentInterface
    {
        /// <summary>
        /// Returns the <see cref="CacheManagerData"/> instance that corresponds to the <see cref="CacheManager"/> being configured.
        /// </summary>
        CacheManagerData CacheManager { get; }


        /// <summary>
        /// Returns the <see cref="CacheManagerSettings"/> instance that is currently being build up.
        /// </summary>
        CacheManagerSettings CachingSettings { get; }
    }
}
