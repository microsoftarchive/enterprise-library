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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// Represents the general configuration information for the Cryptography Application Block.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.CryptographySettings"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public class CryptographyBlockSetting : ConfigurationSectionSetting
    {
        string defaultHashProvider;
        string defaultSymmetricCryptoProvider;

        /// <summary>
        /// Initialize a new instance of the <see cref="CryptographyBlockSetting"/> class with the source element,
        /// the default hash provider and the default symmetric cryptography povider.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="defaultHashProvider">The default hash provider.</param>
        /// <param name="defaultSymmetricCryptoProvider">The default symmetric cryptography provier.</param>
        public CryptographyBlockSetting(CryptographySettings sourceElement,
                                        string defaultHashProvider,
                                        string defaultSymmetricCryptoProvider)
            : base(sourceElement)
        {
            this.defaultHashProvider = defaultHashProvider;
            this.defaultSymmetricCryptoProvider = defaultSymmetricCryptoProvider;
        }

        /// <summary>
        /// Gets the name of the default hash provider for the represented configuration section.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.CryptographySettings.DefaultHashProviderName">
        /// CryptographySettings.DefaultHashProviderName</seealso>
        [ManagementConfiguration]
        public string DefaultHashProvider
        {
            get { return defaultHashProvider; }
            set { defaultHashProvider = value; }
        }

        /// <summary>
        /// Gets the name of the default symmetric crypto provider for the represented configuration section.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.CryptographySettings.DefaultSymmetricCryptoProviderName">
        /// CryptographySettings.DefaultSymmetricCryptoProviderName</seealso>
        [ManagementConfiguration]
        public string DefaultSymmetricCryptoProvider
        {
            get { return defaultSymmetricCryptoProvider; }
            set { defaultSymmetricCryptoProvider = value; }
        }

        /// <summary>
        /// Returns the <see cref="CryptographyBlockSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <returns>The published <see cref="CryptographyBlockSetting"/> instance specified by the values for the key properties, or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static CryptographyBlockSetting BindInstance(string ApplicationName,
                                                            string SectionName)
        {
            return BindInstance<CryptographyBlockSetting>(ApplicationName, SectionName);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="CryptographyBlockSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<CryptographyBlockSetting> GetInstances()
        {
            return GetInstances<CryptographyBlockSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="CryptographyBlockSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return CryptographySettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}