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
    /// Represents the configuration information from an unrecognized cache manager instance. 
    /// </summary>
    /// <seealso cref="NamedConfigurationSetting"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public class UnknownCacheManagerSetting : CacheManagerBaseSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownCacheManagerSetting"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the cache manager.
        /// </param>
        public UnknownCacheManagerSetting(string name)
            : base(name) {}

        /// <summary>
        /// Returns the <see cref="UnknownCacheManagerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="UnknownCacheManagerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static UnknownCacheManagerSetting BindInstance(string ApplicationName,
                                                              string SectionName,
                                                              string Name)
        {
            return BindInstance<UnknownCacheManagerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="UnknownCacheManagerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<UnknownCacheManagerSetting> GetInstances()
        {
            return GetInstances<UnknownCacheManagerSetting>();
        }
    }
}