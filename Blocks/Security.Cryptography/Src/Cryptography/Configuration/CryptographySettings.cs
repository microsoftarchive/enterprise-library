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
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for cryptography.</para>
    /// </summary>
    [ViewModel(CryptographyDesignTime.ViewModelTypeNames.CryptographySectionViewModel)]
    [ResourceDescription(typeof(DesignResources), "CryptographySettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CryptographySettingsDisplayName")]
    public class CryptographySettings : SerializableConfigurationSection, ITypeRegistrationsProvider
    {       
		private const string hashProvidersProperty = "hashProviders";
		private const string defaultHashProviderNameProperty = "defaultHashInstance";
		private const string symmetricCryptoProvidersProperty = "symmetricCryptoProviders";
		private const string defaultSymmetricCryptoProviderNameProperty = "defaultSymmetricCryptoInstance";

        /// <summary>
        /// Gets the configuration section name for the Cryptography Application Block.
        /// </summary>
        public const string SectionName = "securityCryptographyConfiguration";

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
        [Reference(typeof(NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData>), typeof(HashProviderData))]
        [ResourceDescription(typeof(DesignResources), "CryptographySettingsDefaultHashProviderNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CryptographySettingsDefaultHashProviderNameDisplayName")]
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
        [ConfigurationCollection(typeof(HashProviderData))]
        [ResourceDescription(typeof(DesignResources), "CryptographySettingsHashProvidersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CryptographySettingsHashProvidersDisplayName")]
		public NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData> HashProviders
		{
            get { return (NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData>)base[hashProvidersProperty]; }
		}	
		

		/// <summary>
		/// The instance name of the default <see cref="ISymmetricCryptoProvider"/> instance.
		/// </summary>
		[ConfigurationProperty(defaultSymmetricCryptoProviderNameProperty, IsRequired= false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData>), typeof(SymmetricProviderData))]
        [ResourceDescription(typeof(DesignResources), "CryptographySettingsDefaultSymmetricCryptoProviderNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CryptographySettingsDefaultSymmetricCryptoProviderNameDisplayName")]
		public string DefaultSymmetricCryptoProviderName
		{
			get { return (string)this[defaultSymmetricCryptoProviderNameProperty]; }
			set { this[defaultSymmetricCryptoProviderNameProperty] = value; }
		}

		/// <summary>
		/// TODOC
		/// </summary>
        [ConfigurationProperty(symmetricCryptoProvidersProperty, IsRequired = false)]
        [ConfigurationCollection(typeof(SymmetricProviderData))]
        [ResourceDescription(typeof(DesignResources), "CryptographySettingsSymmetricCryptoProvidersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CryptographySettingsSymmetricCryptoProvidersDisplayName")]
        public NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData> SymmetricCryptoProviders
		{
            get { return (NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData>)base[symmetricCryptoProvidersProperty]; }
		}

        /// <summary>
        /// Creates <see cref="TypeRegistration"/> entries registration by a container.
        /// </summary>
        /// <returns>The type registration entries.</returns>
        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var hashProviderRegistrations = HashProviders.SelectMany(hpd => hpd.GetRegistrations(configurationSource));
            hashProviderRegistrations = SetDefaultHashProviderRegistration(hashProviderRegistrations);

            var symmetricCryptoProviderRegistrations = SymmetricCryptoProviders.SelectMany(scpd => scpd.GetRegistrations(configurationSource));
            symmetricCryptoProviderRegistrations = SetDefaultSymmetricCryptoProvider(symmetricCryptoProviderRegistrations);

            var cryptoManagerRegistrations = CreateCryptographyManagerRegistrations(configurationSource);

            return hashProviderRegistrations
                .Concat(symmetricCryptoProviderRegistrations)
                .Concat(cryptoManagerRegistrations);
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrations(configurationSource);
        }

        private IEnumerable<TypeRegistration> SetDefaultHashProviderRegistration(IEnumerable<TypeRegistration> hashProviderRegisrations)
        {
            foreach (TypeRegistration registration in hashProviderRegisrations)
            {
                if (registration.ServiceType == typeof(IHashProvider) && string.Equals(registration.Name, DefaultHashProviderName))
                {
                    registration.IsDefault = true;
                    yield return registration;
                }
                else
                {
                    yield return registration;
                }
            }
        }

        private IEnumerable<TypeRegistration> SetDefaultSymmetricCryptoProvider(IEnumerable<TypeRegistration> symmetricCryptoProviders)
        {
            foreach (TypeRegistration registration in symmetricCryptoProviders)
            {
                if (registration.ServiceType == typeof(ISymmetricCryptoProvider) && string.Equals(registration.Name, DefaultSymmetricCryptoProviderName))
                {
                    registration.IsDefault = true;
                    yield return registration;
                }
                else
                {
                    yield return registration;
                }
            }
        }

        private IEnumerable<TypeRegistration> CreateCryptographyManagerRegistrations(IConfigurationSource configurationSource)
        {
            var hashProviderNames = (from data in HashProviders select data.Name).ToArray();
            var algorithmProviderNames = (from data in SymmetricCryptoProviders select data.Name).ToArray();

            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);
            yield return new TypeRegistration<IDefaultCryptographyInstrumentationProvider>(
                () => new DefaultCryptographyEventLogger(instrumentationSection.PerformanceCountersEnabled,
                                                         instrumentationSection.EventLoggingEnabled,
                                                         instrumentationSection.WmiEnabled,
                                                         instrumentationSection.ApplicationInstanceName))
                             {
                                 Lifetime = TypeRegistrationLifetime.Transient,
                                 IsDefault = true
                             };

            yield return new TypeRegistration<CryptographyManager>(() =>
                new CryptographyManagerImpl(
                    hashProviderNames,
                    Container.ResolvedEnumerable<IHashProvider>(hashProviderNames),
                    algorithmProviderNames,
                    Container.ResolvedEnumerable<ISymmetricCryptoProvider>(algorithmProviderNames),
                    Container.Resolved<IDefaultCryptographyInstrumentationProvider>()))
                {
                    IsDefault = true
                };
        }
    }
}
