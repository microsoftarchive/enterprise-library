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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for cryptography.</para>
    /// </summary>
    public class CryptographySettings : SerializableConfigurationSection
    {       
		private const string hashProvidersProperty = "hashProviders";
		private const string defaultHashProviderNameProperty = "defaultHashInstance";
		private const string symmetricCryptoProvidersProperty = "symmetricCryptoProviders";
		private const string defaultSymmetricCryptoProviderNameProperty = "defaultSymmetricCryptoInstance";
		
		/// <summary>
        /// <para>Initialize a new instance of the <see cref="CryptographySettings"/> class.</para>
        /// </summary>
        public CryptographySettings()
        {
        }

		/// <summary>
		/// The instance name of the default <see cref="IHashProvider"/> instance.
		/// </summary>
		[ConfigurationProperty(defaultHashProviderNameProperty, IsRequired= false)]
		public string DefaultHashProviderName
		{
			get { return (string)this[defaultHashProviderNameProperty]; }
			set { this[defaultHashProviderNameProperty] = value; }
		}

		/// <summary>
		/// <para>Gets the <see cref="HashProviders"/>.</para>
		/// </summary>
		/// <value>
		/// <para>The hash providers available in configuration. The default is an empty collection.</para>
		/// </value>
		/// <remarks>
		/// <para>This property maps to the <c>hashProviders</c> element in configuration.</para>
		/// </remarks>
		[ConfigurationProperty(hashProvidersProperty, IsRequired= false)]
		public NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData> HashProviders
		{
            get { return (NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData>)base[hashProvidersProperty]; }
		}	
		

		/// <summary>
		/// The instance name of the default <see cref="ISymmetricCryptoProvider"/> instance.
		/// </summary>
		[ConfigurationProperty(defaultSymmetricCryptoProviderNameProperty, IsRequired= false)]
		public string DefaultSymmetricCryptoProviderName
		{
			get { return (string)this[defaultSymmetricCryptoProviderNameProperty]; }
			set { this[defaultSymmetricCryptoProviderNameProperty] = value; }
		}

		/// <summary>
		/// <para>Gets the <see cref="HashProviders"/>.</para>
		/// </summary>
		/// <value>
		/// <para>The hash providers available in configuration. The default is an empty collection.</para>
		/// </value>
		/// <remarks>
		/// <para>This property maps to the <c>hashProviders</c> element in configuration.</para>
		/// </remarks>
		[ConfigurationProperty(symmetricCryptoProvidersProperty, IsRequired= false)]
        public NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData> SymmetricCryptoProviders
		{
            get { return (NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData>)base[symmetricCryptoProvidersProperty]; }
		}
    }
}