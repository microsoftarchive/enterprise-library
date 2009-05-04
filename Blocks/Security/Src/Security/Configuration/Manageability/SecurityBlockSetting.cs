//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
    /// <summary>
    /// Represents the general configuration information for the Security Application Block.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.SecuritySettings"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public partial class SecurityBlockSetting : ConfigurationSectionSetting
    {
        string defaultAuthorizationProvider;
        string defaultSecurityCacheProvider;

        /// <summary>
        /// Initialize a new instance of the <see cref="SecurityBlockSetting"/> class with the configuration,
        /// default authorization provider, and default security cache provider.
        /// </summary>
        /// <param name="sourceElement">The configuration settings.</param>
        /// <param name="defaultAuthorizationProvider">The default authorization provider.</param>
        /// <param name="defaultSecurityCacheProvider">The default security cache provider.</param>
        public SecurityBlockSetting(SecuritySettings sourceElement,
                                    string defaultAuthorizationProvider,
                                    string defaultSecurityCacheProvider)
            : base(sourceElement)
        {
            this.defaultAuthorizationProvider = defaultAuthorizationProvider;
            this.defaultSecurityCacheProvider = defaultSecurityCacheProvider;
        }

        /// <summary>
        /// Gets the name of the default hash provider for the represented configuration section.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.SecuritySettings.DefaultAuthorizationProviderName">
        /// SecuritySettings.DefaultAuthorizationProviderName</seealso>
        [ManagementConfiguration]
        public string DefaultAuthorizationProvider
        {
            get { return defaultAuthorizationProvider; }
            set { defaultAuthorizationProvider = value; }
        }

        /// <summary>
        /// Gets the name of the default hash provider for the represented configuration section.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.SecuritySettings.DefaultSecurityCacheProviderName">
        /// SecuritySettings.DefaultSecurityCacheProviderName</seealso>
        [ManagementConfiguration]
        public string DefaultSecurityCacheProvider
        {
            get { return defaultSecurityCacheProvider; }
            set { defaultSecurityCacheProvider = value; }
        }

        /// <summary>
        /// Returns the <see cref="SecurityBlockSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <returns>The published <see cref="SecurityBlockSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static SecurityBlockSetting BindInstance(string ApplicationName,
                                                        string SectionName)
        {
            return BindInstance<SecurityBlockSetting>(ApplicationName, SectionName);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="SecurityBlockSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<SecurityBlockSetting> GetInstances()
        {
            return GetInstances<SecurityBlockSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="SecurityBlockSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return SecuritySettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
