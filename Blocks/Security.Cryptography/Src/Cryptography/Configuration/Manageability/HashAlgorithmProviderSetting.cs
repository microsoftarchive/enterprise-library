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
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashAlgorithmProviderData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashAlgorithmProviderData"/>
    /// <seealso cref="NamedConfigurationSetting"/>
    /// <seealso cref="ConfigurationSetting"/>
    [ManagementEntity]
    public partial class HashAlgorithmProviderSetting : HashProviderSetting
    {
        string algorithmType;
        bool saltEnabled;

        /// <summary>
        /// Initialize a new instance of the <see cref="HashAlgorithmProviderSetting"/> class with the source element, the name,
        /// the algorithm type and if salt is enabled.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="name">The name of the provider.</param>
        /// <param name="algorithmType">The algorithm type.</param>
        /// <param name="saltEnabled">true if salt is enabled; otherwise, false.</param>
        public HashAlgorithmProviderSetting(HashAlgorithmProviderData sourceElement,
                                            string name,
                                            string algorithmType,
                                            bool saltEnabled)
            : base(sourceElement, name)
        {
            this.algorithmType = algorithmType;
            this.saltEnabled = saltEnabled;
        }

        /// <summary>
        /// Gets the name of the algorithm type for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashAlgorithmProviderData.AlgorithmType">
        /// HashAlgorithmProviderData.AlgorithmType</seealso>
        [ManagementConfiguration]
        public string AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }

        /// <summary>
        /// Gets the value of the salt enabled property for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashAlgorithmProviderData.SaltEnabled">
        /// HashAlgorithmProviderData.SaltEnabled</seealso>
        [ManagementConfiguration]
        public bool SaltEnabled
        {
            get { return saltEnabled; }
            set { saltEnabled = value; }
        }

        /// <summary>
        /// Returns the <see cref="HashAlgorithmProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="HashAlgorithmProviderSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static HashAlgorithmProviderSetting BindInstance(string ApplicationName,
                                                                string SectionName,
                                                                string Name)
        {
            return BindInstance<HashAlgorithmProviderSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="HashAlgorithmProviderSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<HashAlgorithmProviderSetting> GetInstances()
        {
            return GetInstances<HashAlgorithmProviderSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="HashAlgorithmProviderSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return HashAlgorithmProviderDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}