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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Overall configuration settings for Caching
    /// </summary>    
    public class CacheManagerSettings : SerializableConfigurationSection
    {
		/// <summary>
		/// Configuration key for cache manager settings.
		/// </summary>
		public const string SectionName = "cachingConfiguration";

        private const string defaultCacheManagerProperty = "defaultCacheManager";
		private const string cacheManagersProperty = "cacheManagers";
		private const string backingStoresProperty = "backingStores";
		private const string encryptionProvidersProperty = "encryptionProviders";    

        /// <summary>
        /// Defines the default manager instance to use when no other manager is specified
        /// </summary>
		[ConfigurationProperty(defaultCacheManagerProperty, IsRequired= true)]
        public string DefaultCacheManager
        {
			get { return (string)base[defaultCacheManagerProperty]; }
			set { base[defaultCacheManagerProperty] = value; }
        }

        /// <summary>
		/// Gets the collection of defined <see cref="CacheManager"/> objects.
        /// </summary>
		/// <value>
		/// The collection of defined <see cref="CacheManager"/> objects.
		/// </value>
        [ConfigurationProperty(cacheManagersProperty, IsRequired= true)]
		public NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData> CacheManagers
		{
			get { return (NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData>)base[cacheManagersProperty]; }
		}

		/// <summary>
		/// Gets the collection of defined <see cref="IBackingStore"/> objects.
		/// </summary>
		/// <value>
		/// The collection of defined <see cref="IBackingStore"/> objects.
		/// </value>
		[ConfigurationProperty(backingStoresProperty, IsRequired= false)]
		public NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData> BackingStores
		{
            get { return (NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData>)base[backingStoresProperty]; }
		}

		/// <summary>
		/// Gets the collection of defined <see cref="IStorageEncryptionProvider"/> objects.
		/// </summary>
		/// <value>
		/// The collection of defined <see cref="IStorageEncryptionProvider"/> objects.
		/// </value>
		[ConfigurationProperty(encryptionProvidersProperty, IsRequired= false)]
        public NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData> EncryptionProviders
		{
            get { return (NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData>)base[encryptionProvidersProperty]; }
		}
    }
}
