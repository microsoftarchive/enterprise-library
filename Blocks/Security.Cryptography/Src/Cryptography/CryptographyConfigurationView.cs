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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// <para>Represents a view to navigate the <see cref="CryptographySettings"/> configuration data.</para>
    /// </summary>
    public class CryptographyConfigurationView
    {
		/// <summary>
		/// Gets the configuration section name for the cryptography library.
		/// </summary>
		public const string SectionName = "securityCryptographyConfiguration";

		private IConfigurationSource configurationSource;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CryptographyConfigurationView"/> class with an <see cref="IConfigurationSource"/> object.</para>
        /// </summary>
		public CryptographyConfigurationView(IConfigurationSource configurationSource) 
        {
			if (configurationSource == null) throw new ArgumentNullException("configurationSource");			

			this.configurationSource = configurationSource;
        }
        
		/// <summary>
		/// <para>Gets the name of the default <see cref="HashProviderData"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>The name of the default <see cref="HashProviderData"/>.</para>
		/// </returns>
		public string DefaultHashProviderName
		{
			get
			{
				CryptographySettings settings = this.CryptographySettings;
				return settings.DefaultHashProviderName;
			}
		}
		
		/// <summary>
        /// <para>Gets the <see cref="CryptographySettings"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="CryptographySettings"/>.</para>
        /// </returns>
        public CryptographySettings CryptographySettings
        {
			get
			{
				CryptographySettings cryptographySection = configurationSource.GetSection(SectionName) as CryptographySettings;
				if (cryptographySection == null)
				{
					throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionSectionNotDefined, SectionName));
				}
				return cryptographySection;
			}
        }        

		/// <summary>
		/// <para>Gets the name of the default <see cref="SymmetricProviderData"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>The name of the default <see cref="HashProviderData"/>.</para>
		/// </returns>
		public string DefaultSymmetricCryptoProviderName
		{
			get
			{
				CryptographySettings settings = this.CryptographySettings;
				return settings.DefaultSymmetricCryptoProviderName;
			}
		}

		/// <summary>
		/// Get the named <see cref="HashProviderData"/> instance.
		/// </summary>
		/// <param name="hashProviderName">The name of the instance to retrieve.</param>
		/// <returns>The named <see cref="HashProviderData"/> instance.</returns>
		public HashProviderData GetHashProviderData(string hashProviderName)
		{
			if (string.IsNullOrEmpty(hashProviderName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "hashProviderName");

			CryptographySettings settings = this.CryptographySettings;
			if (settings.HashProviders.Contains(hashProviderName))
			{
				return settings.HashProviders.Get(hashProviderName);
			}
			throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoHashProviderDefined, hashProviderName));
		}

		/// <summary>
		/// Get the named <see cref="SymmetricProviderData"/> instance.
		/// </summary>
		/// <param name="symetricCryptoProviderName">The name of the instance to retrieve.</param>
		/// <returns>The named <see cref="SymmetricProviderData"/> instance.</returns>
		public SymmetricProviderData GetSymetricCryptoProviderData(string symetricCryptoProviderName)
		{
			if (string.IsNullOrEmpty(symetricCryptoProviderName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "symetricCryptoProviderName");

			CryptographySettings settings = this.CryptographySettings;
			if (settings.SymmetricCryptoProviders.Contains(symetricCryptoProviderName))
			{
				return settings.SymmetricCryptoProviders.Get(symetricCryptoProviderName);
			}
			throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoSymmetricCrytoProviderDefined, symetricCryptoProviderName));
		}

	}
}