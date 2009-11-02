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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.Linq.Expressions;
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration
{
	/// <summary>
	/// Configuration data for Symmetric Storage Encryption
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "SymmetricStorageEncryptionProviderDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "SymmetricStorageEncryptionProviderDataDisplayName")]
    [AddSateliteProviderCommand(CryptographySettings.SectionName)]
    [System.ComponentModel.Browsable(true)]
	public class SymmetricStorageEncryptionProviderData : StorageEncryptionProviderData
	{
		private const string symmetricInstanceProperty = "symmetricInstance";

		/// <summary>
		/// Initializs an instance of a <see cref="SymmetricStorageEncryptionProviderData"/> class.
		/// </summary>
		public SymmetricStorageEncryptionProviderData() : base(typeof(SymmetricStorageEncryptionProvider))
		{
		}

		/// <summary>
		/// Create provider with a specified name and symmetric instance.
		/// </summary>
		/// <param name="name">The configured name of the provider</param>
		/// <param name="symmetricInstance">The full name of a <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</param>
		public SymmetricStorageEncryptionProviderData(string name, string symmetricInstance)
			: base(name, typeof (SymmetricStorageEncryptionProvider))
		{
			this.SymmetricInstance = symmetricInstance;
		}

		/// <summary>
		/// Name of symmetric instance
		/// </summary>       
        [ConfigurationProperty(symmetricInstanceProperty, IsRequired = true)]
        [Reference(typeof(NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData>), typeof(SymmetricAlgorithmProviderData))]
        [ResourceDescription(typeof(DesignResources), "SymmetricStorageEncryptionProviderDataSymmetricInstanceDescription")]
        [ResourceDisplayName(typeof(DesignResources), "SymmetricStorageEncryptionProviderDataSymmetricInstanceDisplayName")]
		public string SymmetricInstance
		{
			get { return (string) base[symmetricInstanceProperty]; }
			set { base[symmetricInstanceProperty] = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Expression<Func<IStorageEncryptionProvider>> GetCreationExpression()
        {
            return () => new SymmetricStorageEncryptionProvider(Container.Resolved<ISymmetricCryptoProvider>(SymmetricInstance));
        }
	}
}
