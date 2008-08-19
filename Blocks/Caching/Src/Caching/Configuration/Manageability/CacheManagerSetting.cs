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

using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData"/>
    /// <seealso cref="NamedConfigurationSetting"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public class CacheManagerSetting : CacheManagerBaseSetting
    {
        string cacheStorage;
        int expirationPollFrequencyInSeconds;
        int maximumElementsInCacheBeforeScavenging;
        int numberToRemoveWhenScavenging;

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerSetting"/> class with the name of the cache manager,
        /// the cache storage, the expiration poll frequency in seconds, the maximum elements in the cache before scavenging,
        /// and the number to remove when scavenging.
        /// </summary>
        /// <param name="name">The name of the cache manager.</param>
        /// <param name="cacheStorage">The cache storage.</param>
        /// <param name="expirationPollFrequencyInSeconds">The expiration poll frequency.</param>
        /// <param name="maximumElementsInCacheBeforeScavenging">The maximm number of elements in the cache before scavenging will occur.</param>
        /// <param name="numberToRemoveWhenScavenging">The number of items to remove when scavenging.</param>
        public CacheManagerSetting(string name,
                                   string cacheStorage,
                                   int expirationPollFrequencyInSeconds,
                                   int maximumElementsInCacheBeforeScavenging,
                                   int numberToRemoveWhenScavenging)
            : base(name)
        {
            this.cacheStorage = cacheStorage;
            this.expirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
            this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
        }

        /// <summary>
        /// Gets the name of the cache storage for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData.CacheStorage">CacheManagerData.CacheStorage</seealso>
        [ManagementProbe]
        public string CacheStorage
        {
            get { return cacheStorage; }
            set { cacheStorage = value; }
        }

        /// <summary>
        /// Gets the expiration poll frequency for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData.ExpirationPollFrequencyInSeconds">CacheManagerData.ExpirationPollFrequencyInSeconds</seealso>
        [ManagementProbe]
        public int ExpirationPollFrequencyInSeconds
        {
            get { return expirationPollFrequencyInSeconds; }
            set { expirationPollFrequencyInSeconds = value; }
        }

        /// <summary>
        /// Gets the maximum number of elements in cache before scavenging for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData.MaximumElementsInCacheBeforeScavenging">CacheManagerData.MaximumElementsInCacheBeforeScavenging</seealso>
        [ManagementProbe]
        public int MaximumElementsInCacheBeforeScavenging
        {
            get { return maximumElementsInCacheBeforeScavenging; }
            set { maximumElementsInCacheBeforeScavenging = value; }
        }

        /// <summary>
        /// Gets the number of elements to remove when scavenging for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData.NumberToRemoveWhenScavenging">CacheManagerData.NumberToRemoveWhenScavenging</seealso>
        [ManagementProbe]
        public int NumberToRemoveWhenScavenging
        {
            get { return numberToRemoveWhenScavenging; }
            set { numberToRemoveWhenScavenging = value; }
        }

        /// <summary>
        /// Returns the <see cref="CacheManagerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="CacheManagerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static CacheManagerSetting BindInstance(string ApplicationName,
                                                       string SectionName,
                                                       string Name)
        {
            return BindInstance<CacheManagerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="CacheManagerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<CacheManagerSetting> GetInstances()
        {
            return GetInstances<CacheManagerSetting>();
        }
    }
}