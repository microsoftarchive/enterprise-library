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
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.KeyedHashAlgorithmProviderData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.KeyedHashAlgorithmProviderData"/>
    /// <seealso cref="NamedConfigurationSetting"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public class KeyedHashAlgorithmProviderSetting : HashProviderSetting
    {
        string algorithmType;
        string protectedKeyFilename;
        string protectedKeyProtectionScope;
        bool saltEnabled;

        /// <summary>
        /// Initialize a new instance of the <see cref="KeyedHashAlgorithmProviderSetting"/> class.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="name">The name of the provider.</param>
        /// <param name="algorithmType">The algorithm type.</param>
        /// <param name="protectedKeyFilename">The protected key file name.</param>
        /// <param name="protectedKeyProtectionScope">The protected key protection scope.</param>
        /// <param name="saltEnabled">true if salt is enabled; otherwise false.</param>
        public KeyedHashAlgorithmProviderSetting(KeyedHashAlgorithmProviderData sourceElement,
                                                 string name,
                                                 string algorithmType,
                                                 string protectedKeyFilename,
                                                 string protectedKeyProtectionScope,
                                                 bool saltEnabled)
            : base(sourceElement, name)
        {
            this.algorithmType = algorithmType;
            this.protectedKeyFilename = protectedKeyFilename;
            this.protectedKeyProtectionScope = protectedKeyProtectionScope;
            this.saltEnabled = saltEnabled;
        }

        /// <summary>
        /// Gets the name of the algorithm type for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashAlgorithmProviderData.AlgorithmType">
        /// Inherited HashAlgorithmProviderData.AlgorithmType</seealso>
        [ManagementConfiguration]
        public string AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }

        /// <summary>
        /// Gets the name of the protected key file for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.KeyedHashAlgorithmProviderData.ProtectedKeyFilename">
        /// KeyedHashAlgorithmProviderData.ProtectedKeyFilename</seealso>
        [ManagementConfiguration]
        public string ProtectedKeyFilename
        {
            get { return protectedKeyFilename; }
            set { protectedKeyFilename = value; }
        }

        /// <summary>
        /// Gets the name of the protected key scope value for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.KeyedHashAlgorithmProviderData.ProtectedKeyProtectionScope">
        /// KeyedHashAlgorithmProviderData.ProtectedKeyProtectionScope</seealso>
        [ManagementConfiguration]
        public string ProtectedKeyProtectionScope
        {
            get { return protectedKeyProtectionScope; }
            set { protectedKeyProtectionScope = value; }
        }

        /// <summary>
        /// Gets the value of the salt enabled property for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashAlgorithmProviderData.SaltEnabled">
        /// Inherited HashAlgorithmProviderData.SaltEnabled</seealso>
        [ManagementConfiguration]
        public bool SaltEnabled
        {
            get { return saltEnabled; }
            set { saltEnabled = value; }
        }

        /// <summary>
        /// Returns the <see cref="KeyedHashAlgorithmProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="KeyedHashAlgorithmProviderSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static KeyedHashAlgorithmProviderSetting BindInstance(string ApplicationName,
                                                                     string SectionName,
                                                                     string Name)
        {
            return BindInstance<KeyedHashAlgorithmProviderSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="KeyedHashAlgorithmProviderSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<KeyedHashAlgorithmProviderSetting> GetInstances()
        {
            return GetInstances<KeyedHashAlgorithmProviderSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="KeyedHashAlgorithmProviderSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return KeyedHashAlgorithmProviderDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
