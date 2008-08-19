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
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.SymmetricAlgorithmProviderData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.SymmetricAlgorithmProviderData"/>
    /// <seealso cref="NamedConfigurationSetting"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public partial class SymmetricAlgorithmProviderSetting : SymmetricCryptoProviderSetting
    {
        string algorithmType;
        string protectedKeyFilename;
        string protectedKeyProtectionScope;

        /// <summary>
        /// Initialize a new instance of the <see cref="SymmetricCryptoProviderSetting"/> class with the source element, the name, 
        /// algorithm type, protected key file name, and the protected key scope.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="name">The name of the provider.</param>
        /// <param name="algorithmType">The algorithm type.</param>
        /// <param name="protectedKeyFilename">The proteced key file name.</param>
        /// <param name="protectedKeyProtectionScope">The protected key protection scope.</param>
        public SymmetricAlgorithmProviderSetting(ConfigurationElement sourceElement,
                                                 string name,
                                                 string algorithmType,
                                                 string protectedKeyFilename,
                                                 string protectedKeyProtectionScope)
            : base(sourceElement, name)
        {
            this.algorithmType = algorithmType;
            this.protectedKeyFilename = protectedKeyFilename;
            this.protectedKeyProtectionScope = protectedKeyProtectionScope;
        }

        /// <summary>
        /// Gets the name of the algorithm type for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.SymmetricAlgorithmProviderData.AlgorithmType">
        /// SymmetricAlgorithmProviderData.AlgorithmType</seealso>
        [ManagementConfiguration]
        public string AlgorithmType
        {
            get { return algorithmType; }
            set
            {
                ParseHelper.ParseType(value, true);
                algorithmType = value;
            }
        }

        /// <summary>
        /// Gets the name of the protected key file for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.SymmetricAlgorithmProviderData.ProtectedKeyFilename">
        /// SymmetricAlgorithmProviderData.ProtectedKeyFilename</seealso>
        [ManagementConfiguration]
        public string ProtectedKeyFilename
        {
            get { return protectedKeyFilename; }
            set { protectedKeyFilename = value; }
        }

        /// <summary>
        /// Gets the name of the protected key scope value for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.SymmetricAlgorithmProviderData.ProtectedKeyProtectionScope">
        /// SymmetricAlgorithmProviderData.ProtectedKeyProtectionScope</seealso>
        [ManagementConfiguration]
        public string ProtectedKeyProtectionScope
        {
            get { return protectedKeyProtectionScope; }
            set
            {
                ParseHelper.ParseEnum<DataProtectionScope>(value, true);
                protectedKeyProtectionScope = value;
            }
        }

        /// <summary>
        /// Returns the <see cref="SymmetricAlgorithmProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="SymmetricAlgorithmProviderSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static SymmetricAlgorithmProviderSetting BindInstance(string ApplicationName,
                                                                     string SectionName,
                                                                     string Name)
        {
            return BindInstance<SymmetricAlgorithmProviderSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="SymmetricAlgorithmProviderSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<SymmetricAlgorithmProviderSetting> GetInstances()
        {
            return GetInstances<SymmetricAlgorithmProviderSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="SymmetricAlgorithmProviderSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return SymmetricAlgorithmProviderDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}