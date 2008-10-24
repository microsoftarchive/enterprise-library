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

using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.SymmetricStorageEncryptionProviderData"/> 
    /// instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.SymmetricStorageEncryptionProviderData"/> 
    /// <seealso cref="StorageEncryptionProviderSetting"/>
    [ManagementEntity]
    public partial class SymmetricStorageEncryptionProviderSetting : StorageEncryptionProviderSetting
    {
        String symmetricInstance;

        /// <summary>
        /// Initialize a new instance of the <see cref="SymmetricStorageEncryptionProviderSetting"/> class with a name and 
        /// the symmetric instance to use.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="symmetricInstance">The symmetric instance to use.</param>
        public SymmetricStorageEncryptionProviderSetting(String name,
                                                         String symmetricInstance)
            : base(name)
        {
            this.symmetricInstance = symmetricInstance;
        }

        /// <summary>
        /// Gets the name of symmetric encryption provider for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.SymmetricStorageEncryptionProviderData.SymmetricInstance">
        /// SymmetricStorageEncryptionProviderData.SymmetricInstance</seealso>
        [ManagementProbe]
        public String SymmetricInstance
        {
            get { return symmetricInstance; }
            set { symmetricInstance = value; }
        }

        /// <summary>
        /// Returns the <see cref="SymmetricStorageEncryptionProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="SymmetricStorageEncryptionProviderSetting"/> instance specified by the values for the key properties, or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static SymmetricStorageEncryptionProviderSetting BindInstance(string ApplicationName,
                                                                             string SectionName,
                                                                             string Name)
        {
            return BindInstance<SymmetricStorageEncryptionProviderSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="SymmetricStorageEncryptionProviderSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<SymmetricStorageEncryptionProviderSetting> GetInstances()
        {
            return GetInstances<SymmetricStorageEncryptionProviderSetting>();
        }
    }
}
