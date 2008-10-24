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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Represents a view to navigate the <see cref="CacheManagerSettings"/> configuration data.
    /// </summary>
    public class CachingConfigurationView
    {
        private IConfigurationSource configurationSource;

        /// <summary>
        /// Initialize a new instance of the <see cref="CachingConfigurationView"/> with a <see cref="Common.Configuration.IConfigurationSource"/>.
        /// </summary>
        /// <param name="configurationSource">
        /// An <see cref="IConfigurationSource"/> object.
        /// </param>
        public CachingConfigurationView(IConfigurationSource configurationSource)
        {
            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// Gets the <see cref="CacheStorageData"/> from configuration for the named <see cref="CacheManager"/>
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManager"/>.
        /// </param>
        /// <returns>
        /// A <see cref="CacheStorageData"/> object.
        /// </returns>
        public CacheStorageData GetCacheStorageData(string name)
        {
            CacheManagerSettings settings = this.CacheManagerSettings;
            if (!settings.BackingStores.Contains(name))
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionBackingStoreNotDefined, name));
            }
            return settings.BackingStores.Get(name);
        }

        /// <summary>
        /// Gets the name of the default <see cref="CacheManagerData"/>.
        /// </summary>
        /// <returns>
        /// The name of the default <see cref="CacheManagerData"/>.
        /// </returns>
        public string DefaultCacheManager
        {
            get
            {
                CacheManagerSettings configSettings = this.CacheManagerSettings;
                if (string.IsNullOrEmpty(configSettings.DefaultCacheManager))
                {
                    throw new ConfigurationErrorsException(Resources.NoDefaultCacheManager);
                }

                return configSettings.DefaultCacheManager;
            }
        }

        /// <summary>
        /// Gets the <see cref="CacheManagerSettings"/> configuration data.
        /// </summary>
        /// <returns>
        /// The <see cref="CacheManagerSettings"/> configuration data.
        /// </returns>
        public CacheManagerSettings CacheManagerSettings
        {
            get
            {
                CacheManagerSettings settings
                    = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);
                if (settings == null)
                {
                    throw new ConfigurationErrorsException(Resources.MissingSection);
                }

                return settings;
            }
        }

        /// <summary>
        /// Gets the <see cref="CacheManagerDataBase"/> from configuration for the named <see cref="CacheManager"/>
        /// </summary>
        /// <param name="cacheManagerName">
        /// The name of the <see cref="CacheManager"/>.
        /// </param>
        /// <returns>
        /// A <see cref="CacheManagerDataBase"/> object.
        /// </returns>
        public CacheManagerDataBase GetCacheManagerData(string cacheManagerName)
        {
            CacheManagerSettings configSettings = this.CacheManagerSettings;
            CacheManagerDataBase data = configSettings.CacheManagers.Get(cacheManagerName);
            if (data == null)
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.UnableToFindCacheManagerInstance, cacheManagerName));
            }
            return data;
        }

        /// <summary>
        /// Gets the <see cref="StorageEncryptionProviderData"/> from configuration for the named <see cref="CacheManager"/>
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManager"/>.
        /// </param>
        /// <returns>
        /// A <see cref="StorageEncryptionProviderData"/> object.
        /// </returns>
        public StorageEncryptionProviderData GetStorageEncryptionProviderData(string name)
        {
            CacheManagerSettings settings = this.CacheManagerSettings;
            if (!settings.EncryptionProviders.Contains(name))
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionEncryptionProviderNotDefined, name));
            }
            return settings.EncryptionProviders.Get(name);
        }
    }
}
