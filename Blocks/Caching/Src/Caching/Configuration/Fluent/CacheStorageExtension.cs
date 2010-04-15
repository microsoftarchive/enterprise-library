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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Base class for fluent interface builders that extend the <see cref="IBackingStoreEncryptItemsUsing"/> interface.
    /// </summary>
    public abstract class CacheStorageExtension : ICachingConfigurationCacheStorageExtension
    {
        ICachingConfigurationCacheStorageExtension contextExtension;

        /// <summary>
        /// Creates an instance of <see cref="CacheStorageExtension"/> passing the current <see cref="IBackingStore"/>'s fluent interface builder.
        /// </summary>
        /// <param name="context">The current <see cref="IBackingStore"/>'s fluent interface builder.<br/>
        /// This interface must implement <see cref="ICachingConfigurationCacheStorageExtension"/>.</param>
        protected CacheStorageExtension(IBackingStoreEncryptItemsUsing context)
        {
            contextExtension = context as ICachingConfigurationCacheStorageExtension;

            if (contextExtension == null) throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Resources.ExceptionParameterMustImplement, typeof(ICachingConfigurationCacheStorageExtension).FullName),
                "context");
        }

        /// <summary>
        /// Adds a <see cref="StorageEncryptionProviderData"/> to the <see cref="CacheManagerSettings"/> as well as adds a reference to the <see cref="IBackingStore"/> instance currently being configured.
        /// </summary>
        /// <param name="storageEncyption">The <see cref="StorageEncryptionProviderData"/> that should be added to configuration.</param>
        protected void AddEncryptionProviderToCachingConfigurationAndBackingStore(StorageEncryptionProviderData storageEncyption)
        {
            contextExtension.CachingSettings.EncryptionProviders.Add(storageEncyption);
            contextExtension.CacheStorage.StorageEncryption = storageEncyption.Name;
        }


        /// <summary>
        /// Returns the <see cref="CacheStorageData"/> instance that corresponds to the <see cref="IBackingStore"/> being configured.
        /// </summary>
        protected CacheStorageData CacheStorage
        {
            get
            {
                return contextExtension.CacheStorage;
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
            get { return contextExtension.CacheStorage; }
        }

        CacheManagerSettings ICachingConfigurationCacheStorageExtension.CachingSettings
        {
            get { return contextExtension.CachingSettings; }
        }
    }


    /// <summary>
    /// Allows access to the underlying configuration classes that are used for the <see cref="IBackingStore"/> instance being configured.
    /// </summary>
    public interface ICachingConfigurationCacheStorageExtension : IFluentInterface
    {
        /// <summary>
        /// Returns the <see cref="CacheStorageData"/> instance that corresponds to the <see cref="IBackingStore"/> being configured.
        /// </summary>
        CacheStorageData CacheStorage { get; }


        /// <summary>
        /// Returns the <see cref="CacheManagerSettings"/> instance that is currently being build up.
        /// </summary>
        CacheManagerSettings CachingSettings { get; }
    }
}
