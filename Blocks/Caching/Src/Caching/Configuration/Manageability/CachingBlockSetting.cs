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
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    /// <summary>
    /// Represents the general configuration information for the Caching Application Block.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerSettings"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public class CachingBlockSetting : ConfigurationSectionSetting
    {
        string defaultCacheManager;

        /// <summary>
        /// Initialize a new instance of the <see cref="CachingBlockSetting"/> class with the cache manager configuration and the default cache manager.
        /// </summary>
        /// <param name="settings">The cache manager configuration.</param>
        /// <param name="defaultCacheManager">The default cache manager.</param>
        public CachingBlockSetting(CacheManagerSettings settings,
                                   string defaultCacheManager)
            : base(settings)
        {
            this.defaultCacheManager = defaultCacheManager;
        }

        /// <summary>
        /// Gets the name of the default cache manager for the represented configuration section.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerSettings.DefaultCacheManager">CacheManagerSettings.DefaultCacheManager</seealso>
        [ManagementConfiguration]
        public string DefaultCacheManager
        {
            get { return defaultCacheManager; }
            set { defaultCacheManager = value; }
        }

        /// <summary>
        /// Returns the <see cref="CachingBlockSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <returns>The published <see cref="CachingBlockSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static CachingBlockSetting BindInstance(string ApplicationName,
                                                       string SectionName)
        {
            return BindInstance<CachingBlockSetting>(ApplicationName, SectionName);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="CachingBlockSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<CachingBlockSetting> GetInstances()
        {
            return GetInstances<CachingBlockSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="CachingBlockSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return CacheManagerSettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
