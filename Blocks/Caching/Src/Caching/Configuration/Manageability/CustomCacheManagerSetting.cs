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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CustomCacheManagerData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CustomCacheManagerData"/>
    /// <seealso cref="CacheStorageSetting"/>
    [ManagementEntity]
    public class CustomCacheManagerSetting : CacheManagerBaseSetting
    {
        string[] attributes;
        string providerType;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomCacheManagerSetting"/> class with the name, the provider type
        /// and the attributes for the cache manager.
        /// </summary>
        /// <param name="name">The name of the cache manager.</param>
        /// <param name="providerType">The provider type for the cache manager.</param>
        /// <param name="attributes">The attributes for the cache manager.</param>
        public CustomCacheManagerSetting(string name,
                                         string providerType,
                                         string[] attributes)
            : base(name)
        {
            this.providerType = providerType;
            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the collection of attributes for the custom exception handler represented as a 
        /// <see cref="string"/> array of key/value pairs for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CustomCacheStorageData.Attributes">CustomCacheStorageData.Attributes</seealso>
        [ManagementProbe]
        public string[] Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// Gets the name of the type for the custom cache storage for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NameTypeConfigurationElement.Type">Inherited NameTypeConfigurationElement.Type</seealso>
        [ManagementProbe]
        public string ProviderType
        {
            get { return providerType; }
            set { providerType = value; }
        }

        /// <summary>
        /// Returns the <see cref="CustomCacheManagerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="CustomCacheManagerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static CustomCacheManagerSetting BindInstance(string ApplicationName,
                                                             string SectionName,
                                                             string Name)
        {
            return BindInstance<CustomCacheManagerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="CustomCacheManagerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<CustomCacheManagerSetting> GetInstances()
        {
            return GetInstances<CustomCacheManagerSetting>();
        }
    }
}