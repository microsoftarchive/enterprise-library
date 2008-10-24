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
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration
{
	/// <summary>
	/// Configuration data for Symmetric Storage Encryption
	/// </summary>
	[Assembler(typeof (SymmetricStorageEncryptionProviderAssembler))]
    [ContainerPolicyCreator(typeof(SymmetricStorageEncryptionProviderPolicyCreator))]
	public class SymmetricStorageEncryptionProviderData : StorageEncryptionProviderData
	{
		private const string symmetricInstanceProperty = "symmetricInstance";

		/// <summary>
		/// Initializs an instance of a <see cref="SymmetricStorageEncryptionProviderData"/> class.
		/// </summary>
		public SymmetricStorageEncryptionProviderData()
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
		public string SymmetricInstance
		{
			get { return (string) base[symmetricInstanceProperty]; }
			set { base[symmetricInstanceProperty] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="SymmetricStorageEncryptionProvider"/> described by a <see cref="SymmetricStorageEncryptionProviderData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="SymmetricStorageEncryptionProviderData"/> type and it is used by the <see cref="StorageEncryptionProviderCustomFactory"/> 
	/// to build the specific <see cref="IStorageEncryptionProvider"/> object represented by the configuration object.
	/// </remarks>
	public class SymmetricStorageEncryptionProviderAssembler :
		IAssembler<IStorageEncryptionProvider, StorageEncryptionProviderData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds an <see cref="SymmetricStorageEncryptionProvider"/> based on an instance of <see cref="StorageEncryptionProviderData"/>.
		/// </summary>
		/// <seealso cref="StorageEncryptionProviderCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="SymmetricStorageEncryptionProviderData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="SymmetricStorageEncryptionProvider"/>.</returns>
		public IStorageEncryptionProvider Assemble(IBuilderContext context,
		                                           StorageEncryptionProviderData objectConfiguration,
		                                           IConfigurationSource configurationSource,
		                                           ConfigurationReflectionCache reflectionCache)
		{
			SymmetricStorageEncryptionProviderData castedObjectConfiguration
				= (SymmetricStorageEncryptionProviderData) objectConfiguration;

			IBuilderContext cryptoProviderContext
				= context.CloneForNewBuild(
					NamedTypeBuildKey.Make<ISymmetricCryptoProvider>(castedObjectConfiguration.SymmetricInstance), null);

			ISymmetricCryptoProvider symmetricCrytoProvider
				= (ISymmetricCryptoProvider)cryptoProviderContext.Strategies.ExecuteBuildUp(cryptoProviderContext);

			IStorageEncryptionProvider createdObject
				= new SymmetricStorageEncryptionProvider(symmetricCrytoProvider);

			return createdObject;
		}
	}
}
