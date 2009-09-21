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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Overall configuration settings for Caching
    /// </summary>    
    [ViewModel(ViewModels.TabularViewModel)]
    public class CacheManagerSettings : SerializableConfigurationSection, ITypeRegistrationsProvider
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
        [Reference(typeof(CacheManagerSettings), typeof(CacheManagerDataBase))]
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
        [ConfigurationCollection(typeof(CacheManagerDataBase))]
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
        [ConfigurationCollection(typeof(CacheStorageData))]
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
        [ConfigurationProperty(encryptionProvidersProperty, IsRequired = false)]
        [ConfigurationCollection(typeof(StorageEncryptionProviderData))]
        public NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData> EncryptionProviders
		{
            get { return (NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData>)base[encryptionProvidersProperty]; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var cacheManagerRegistrations = SetDefaultCacheManagerRegistration(CacheManagers.SelectMany(cmd => cmd.GetRegistrations(configurationSource)));
            cacheManagerRegistrations = SetDefaultCacheManagerRegistration(cacheManagerRegistrations);

            var encryptionProviderRegistrations = EncryptionProviders.SelectMany(epd => epd.GetRegistrations());
            var backingStoreRegistrations = BackingStores.SelectMany(bsd => bsd.GetRegistrations());

            return backingStoreRegistrations
                                .Concat(GetDefaultEventLoggerRegistrations(configurationSource))
                                .Concat(cacheManagerRegistrations)
                                .Concat(encryptionProviderRegistrations);
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrations(configurationSource);
        }

        private IEnumerable<TypeRegistration> SetDefaultCacheManagerRegistration(IEnumerable<TypeRegistration> cacheManagerRegistrations)
        {
            foreach (TypeRegistration registration in cacheManagerRegistrations)
            {
                if (registration.ServiceType == typeof(ICacheManager) && string.Equals(registration.Name, DefaultCacheManager))
                {
                    registration.IsDefault = true;
                    yield return registration;
                }
                else
                {
                    yield return registration;
                }
            }
        }

        private static IEnumerable<TypeRegistration> GetDefaultEventLoggerRegistrations(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);
            return new TypeRegistration[]
                       {
                           new TypeRegistration<DefaultCachingEventLogger>(
                               () =>
                               new DefaultCachingEventLogger(instrumentationSection.EventLoggingEnabled,
                                                             instrumentationSection.WmiEnabled))
                                                             {
                                                                 IsDefault = true
                                                             }
                       };
        }
    }
}
