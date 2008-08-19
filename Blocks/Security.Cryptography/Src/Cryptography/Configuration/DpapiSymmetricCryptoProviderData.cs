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

using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for a DPAPI Symmetric Cryptography Provider.</para>
    /// </summary>		
	[Assembler(typeof(DpapiSymmetricCryptoProviderAssembler))]
	public class DpapiSymmetricCryptoProviderData : SymmetricProviderData
	{
		private const string scopeProperty = "scope";

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DpapiSymmetricCryptoProviderData"/> class.</para>
		/// </summary>
		public DpapiSymmetricCryptoProviderData()
		{
		}

		/// <summary>
		/// <para>Initialize a new instnace of the <see cref="DpapiSymmetricCryptoProviderData"/> class with entropy and a <see cref="DataProtectionScope"/>.</para>
		/// </summary>
		/// <param name="name">The name of the configued instance.</param>		
		/// <param name="scope">
		/// <para>One of the <see cref="DataProtectionScope"/> values.</para>
		/// </param> 
		public DpapiSymmetricCryptoProviderData(string name, DataProtectionScope scope)
			: base(name, typeof(DpapiSymmetricCryptoProvider))
		{
			this.Scope = scope;
		}

		/// <summary>
		/// <para>Gets or sets the <see cref="ProtectedData"/> storage scope.</para>
		/// </summary>
		/// <value>
		/// <para>One of the <see cref="DataProtectionScope"/> values.</para>
		/// </value>		
		[ConfigurationProperty(scopeProperty, IsRequired = false, DefaultValue = DataProtectionScope.CurrentUser)]
		//[TypeConverter(typeof(EnumConverter))]
		public DataProtectionScope Scope
		{
			get { return (DataProtectionScope)this[scopeProperty]; }
			set { this[scopeProperty] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="DpapiSymmetricCryptoProvider"/> described by a <see cref="DpapiSymmetricCryptoProviderData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="DpapiSymmetricCryptoProviderData"/> type and it is used by the <see cref="SymmetricCryptoProviderCustomFactory"/> 
	/// to build the specific <see cref="ISymmetricCryptoProvider"/> object represented by the configuration object.
	/// </remarks>
	public class DpapiSymmetricCryptoProviderAssembler : IAssembler<ISymmetricCryptoProvider, SymmetricProviderData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="DpapiSymmetricCryptoProvider"/> based on an instance of <see cref="DpapiSymmetricCryptoProviderData"/>.
		/// </summary>
		/// <seealso cref="SymmetricCryptoProviderCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="DpapiSymmetricCryptoProviderData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="DpapiSymmetricCryptoProvider"/>.</returns>
		public ISymmetricCryptoProvider Assemble(IBuilderContext context, SymmetricProviderData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			DpapiSymmetricCryptoProviderData castedObjectConfiguration
				= (DpapiSymmetricCryptoProviderData)objectConfiguration;

			ISymmetricCryptoProvider createdObject
				= new DpapiSymmetricCryptoProvider(
					castedObjectConfiguration.Scope);

			return createdObject;
		}
	}
}