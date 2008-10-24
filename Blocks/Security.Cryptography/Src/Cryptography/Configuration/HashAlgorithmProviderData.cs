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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
	/// <summary>
	/// Configuration settings for the <c>HashAlgorithm</c> hash provider.
	/// </summary>	
	[Assembler(typeof(HashAlgorithmProviderAssembler))]
	public class HashAlgorithmProviderData : HashProviderData
	{
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

		private const string algorithmTypeProperty = "algorithmType";
		private const string saltEnabledProperty = "saltEnabled";

		/// <summary>
		/// Initializes with default configuration.
		/// </summary>
		public HashAlgorithmProviderData()
		{
		}

		/// <summary>
		/// Initialized a new HashAlgorithmProviderData with the given name
		/// </summary>
		/// <param name="name">The name for this object</param>
		/// <param name="algorithmType">The hash algorithm to use</param>
		/// <param name="saltEnabled">Should a salt be used?</param>
		public HashAlgorithmProviderData(string name, Type algorithmType, bool saltEnabled)
			: this(name, typeof(HashAlgorithmProvider), algorithmType, saltEnabled)
		{
		}

		/// <summary>
		/// Initialized a new HashAlgorithmProviderData with the given name
		/// </summary>
		/// <param name="name">The name for this object</param>
		/// <param name="providerType">The</param>
		/// <param name="algorithmType">The hash algorithm to use</param>
		/// <param name="saltEnabled">Should a salt be used?</param>
		protected HashAlgorithmProviderData(string name, Type providerType, Type algorithmType, bool saltEnabled)
			: base(name, providerType)
		{
			this.AlgorithmType = algorithmType;
			this.SaltEnabled = saltEnabled;
		}

		/// <summary>
		/// Gets or sets the type of <see cref="System.Security.Cryptography.HashAlgorithm"/>.
		/// </summary>
        public Type AlgorithmType
        {
            get { return (Type)typeConverter.ConvertFrom(AlgorithmTypeName); }
            set { AlgorithmTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the type of <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the type of <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </value>
        [ConfigurationProperty(algorithmTypeProperty, IsRequired = true)]
        public string AlgorithmTypeName
        {
            get { return (string)this[algorithmTypeProperty]; }
            set { this[algorithmTypeProperty] = value; }
        }

		/// <summary>
		/// Gets or sets the salt enabled flag.
		/// </summary>
		[ConfigurationProperty(saltEnabledProperty, IsRequired = true)]
		public bool SaltEnabled
		{
			get { return (bool)this[saltEnabledProperty]; }
			set { this[saltEnabledProperty] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="HashAlgorithmProvider"/> described by a <see cref="HashAlgorithmProviderData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="HashAlgorithmProviderData"/> type and it is used by the <see cref="HashProviderCustomFactory"/> 
	/// to build the specific <see cref="IHashProvider"/> object represented by the configuration object.
	/// </remarks>
	public class HashAlgorithmProviderAssembler : IAssembler<IHashProvider, HashProviderData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="HashAlgorithmProvider"/> based on an instance of <see cref="HashAlgorithmProviderData"/>.
		/// </summary>
		/// <seealso cref="HashProviderCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="HashAlgorithmProviderData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="HashAlgorithmProvider"/>.</returns>
		public IHashProvider Assemble(IBuilderContext context, HashProviderData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			HashAlgorithmProviderData castedObjectConfiguration
				= (HashAlgorithmProviderData)objectConfiguration;

			IHashProvider createdObject
				= new HashAlgorithmProvider(
					castedObjectConfiguration.AlgorithmType,
					castedObjectConfiguration.SaltEnabled);

			return createdObject;
		}
	}
}
