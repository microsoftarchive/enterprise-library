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

using System;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
	/// <summary>
	/// <para>Configuration data for the <c>SymmetricAlgorithm</c> provider.</para>
	/// </summary>	
	[Assembler(typeof(SymmetricAlgorithmProviderAssembler))]
	public class SymmetricAlgorithmProviderData : SymmetricProviderData
	{
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

		private const string algorithmTypeProperty = "algorithmType";
		private const string protectedKeyFilename = "protectedKeyFilename";
		private const string protectedKeyProtectionScope = "protectedKeyProtectionScope";

		/// <summary>
		/// <para>Initializes a new instance of <see cref="SymmetricAlgorithmProviderData"/> class.</para>
		/// </summary>
		public SymmetricAlgorithmProviderData()
		{
		}

		/// <summary>
		/// <para>Initializes a new instance of <see cref="SymmetricAlgorithmProviderData"/> class.</para>
		/// </summary>
		/// <param name="name"><para>The name for the provider.</para></param>
		/// <param name="algorithmType"><para>The type name of the hash algorithm.</para></param>
		/// <param name="protectedKeyFilename">File name where key is stored</param>
		/// <param name="protectedKeyProtectionScope">DPAPI protection scope used to store key</param>
		public SymmetricAlgorithmProviderData(string name, Type algorithmType, string protectedKeyFilename, DataProtectionScope protectedKeyProtectionScope)
			: base(name, typeof(SymmetricAlgorithmProvider))
		{
			this.AlgorithmType = algorithmType;
			this.ProtectedKeyProtectionScope = protectedKeyProtectionScope;
			this.ProtectedKeyFilename = protectedKeyFilename;
		}

		/// <summary>
		/// <para>Gets or sets the type of <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</para>
		/// </summary>
		/// <value>
		/// <para>The type of <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</para>
		/// </value>
        public Type AlgorithmType
        {
            get { return (Type)typeConverter.ConvertFrom(AlgorithmTypeName); }
            set { AlgorithmTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the type of <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.
        /// </summary>
        /// <value>
        /// The fully qualified name of the type of <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.
        /// </value>
        [ConfigurationProperty(algorithmTypeProperty)]
        public string AlgorithmTypeName
        {
            get { return (string)this[algorithmTypeProperty]; }
            set { this[algorithmTypeProperty] = value; }
        }
		/// <summary>
		/// 
		/// </summary>	
		[ConfigurationProperty(protectedKeyFilename)]
		public string ProtectedKeyFilename
		{
			get { return (string)this[protectedKeyFilename]; }
			set { this[protectedKeyFilename] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[ConfigurationProperty(protectedKeyProtectionScope)]
		public DataProtectionScope ProtectedKeyProtectionScope
		{
			get { return (DataProtectionScope)this[protectedKeyProtectionScope]; }
			set { this[protectedKeyProtectionScope] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="SymmetricAlgorithmProvider"/> described by a <see cref="SymmetricAlgorithmProviderData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="SymmetricAlgorithmProviderData"/> type and it is used by the <see cref="SymmetricCryptoProviderCustomFactory"/> 
	/// to build the specific <see cref="ISymmetricCryptoProvider"/> object represented by the configuration object.
	/// </remarks>
	public class SymmetricAlgorithmProviderAssembler : IAssembler<ISymmetricCryptoProvider, SymmetricProviderData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="SymmetricAlgorithmProvider"/> based on an instance of <see cref="SymmetricAlgorithmProviderData"/>.
		/// </summary>
		/// <seealso cref="SymmetricCryptoProviderCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="SymmetricAlgorithmProviderData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="SymmetricAlgorithmProvider"/>.</returns>
		public ISymmetricCryptoProvider Assemble(IBuilderContext context, SymmetricProviderData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			SymmetricAlgorithmProviderData castedObjectConfiguration
				= (SymmetricAlgorithmProviderData)objectConfiguration;

			ISymmetricCryptoProvider createdObject
				= new SymmetricAlgorithmProvider(
						castedObjectConfiguration.AlgorithmType,
						castedObjectConfiguration.ProtectedKeyFilename,
						castedObjectConfiguration.ProtectedKeyProtectionScope);

			return createdObject;
		}
	}
}