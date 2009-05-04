//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.DpapiSymmetricCryptoProviderData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.DpapiSymmetricCryptoProviderData"/>
    /// <seealso cref="NamedConfigurationSetting"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public partial class DpapiSymmetricCryptoProviderSetting : SymmetricCryptoProviderSetting
    {
        string scope;

        /// <summary>
        /// Initialize a new instance of the <see cref="DpapiSymmetricCryptoProviderSetting"/> class with a source element, the provider name 
        /// and the scope.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="name">The name of the provider.</param>
        /// <param name="scope">The scope to use (machine / user).</param>
        public DpapiSymmetricCryptoProviderSetting(DpapiSymmetricCryptoProviderData sourceElement,
                                                   string name,
                                                   string scope)
            : base(sourceElement, name)
        {
            this.scope = scope;
        }

        /// <summary>
        /// Gets the name of the scope value for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.DpapiSymmetricCryptoProviderData.Scope">
        /// DpapiSymmetricCryptoProviderData.Scope</seealso>
        [ManagementConfiguration]
        public string Scope
        {
            get { return scope; }
            set
            {
                ParseHelper.ParseEnum<DataProtectionScope>(value, true);
                scope = value;
            }
        }

        /// <summary>
        /// Returns the <see cref="DpapiSymmetricCryptoProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="DpapiSymmetricCryptoProviderSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static DpapiSymmetricCryptoProviderSetting BindInstance(string ApplicationName,
                                                                       string SectionName,
                                                                       string Name)
        {
            return BindInstance<DpapiSymmetricCryptoProviderSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="DpapiSymmetricCryptoProviderSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<DpapiSymmetricCryptoProviderSetting> GetInstances()
        {
            return GetInstances<DpapiSymmetricCryptoProviderSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="DpapiSymmetricCryptoProviderSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return DpapiSymmetricCryptoProviderDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
