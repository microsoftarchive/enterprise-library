//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Linq;
using System.Configuration.Provider;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the configuration data for the 
    /// security providers.
    /// </summary>
    [ResourceDisplayName(typeof(Resources), "SecuritySettingsDisplayName")]
    [ViewModel("Console.Wpf.ViewModel.HierarchicalSectionViewModel, Console.Wpf")]
    public class SecuritySettings : SerializableConfigurationSection, ITypeRegistrationsProvider
    {
		/// <summary>
		/// The name of the configuration section for the security providers.
		/// </summary>
		public const string SectionName = "securityConfiguration";

        private const string authorizationProvidersProperty = "authorizationProviders";
		private const string securityCacheProvidersProperty = "securityCacheProviders";

		private const string defaultAuthorizationProviderNameProperty = "defaultAuthorizationInstance";
		private const string defaultSecurityCacheProviderNameProperty = "defaultSecurityCacheInstance";
		       

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SecuritySettings"/> class.
        /// </summary>
        public SecuritySettings()
        {
        }		

        /// <summary>
        /// The instance name of the default <see cref="IAuthorizationProvider"/> instance.
        /// </summary>
		[ConfigurationProperty(defaultAuthorizationProviderNameProperty, IsRequired= false)]   
        [Reference(typeof(SecuritySettings), typeof(AuthorizationProviderData))]
		public string DefaultAuthorizationProviderName
		{
			get
			{
				return (string)this[defaultAuthorizationProviderNameProperty];
			}
			set
			{
				this[defaultAuthorizationProviderNameProperty] = value;
			}
		}

        /// <summary>
        /// The instance name of the default <see cref="ISecurityCacheProvider"/> instance.
        /// </summary>
		[ConfigurationProperty(defaultSecurityCacheProviderNameProperty, IsRequired= false)]
        [Reference(typeof(SecuritySettings), typeof(SecurityCacheProviderData))]
		public string DefaultSecurityCacheProviderName
		{
			get
			{
				return (string)this[defaultSecurityCacheProviderNameProperty];
			}
			set
			{
				this[defaultSecurityCacheProviderNameProperty] = value;
			}
		}

        /// <summary>
		/// <para>Gets the collection of <see cref="AuthorizationProviderData"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The authorization providers available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>authorizationProviders</c> element in configuration.</para>
        /// </remarks>
		[ConfigurationProperty(authorizationProvidersProperty, IsRequired= false)]
        [ResourceDisplayName(typeof(Resources), "AuthorizationProvidersDisplayName")]
        [ConfigurationCollection(typeof(AuthorizationProviderData))]
        public NameTypeConfigurationElementCollection<AuthorizationProviderData, CustomAuthorizationProviderData> AuthorizationProviders
		{
			get
			{
                return (NameTypeConfigurationElementCollection<AuthorizationProviderData, CustomAuthorizationProviderData>)base[authorizationProvidersProperty];
			}
		}

        /// <summary>
		/// <para>Gets the collection of <see cref="SecurityCacheProviderData"/>.</para>
		/// </summary>
        /// <value>
        /// <para>The security cache providers available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>securityCacheProviders</c> element in configuration.</para>
        /// </remarks>
		[ConfigurationProperty(securityCacheProvidersProperty, IsRequired= false)]
        [ResourceDisplayName(typeof(Resources), "SecurityCacheProvidersDisplayName")]
        [ConfigurationCollection(typeof(SecurityCacheProviderData))]
		public NameTypeConfigurationElementCollection<SecurityCacheProviderData, CustomSecurityCacheProviderData> SecurityCacheProviders
		{
			get
			{
                return (NameTypeConfigurationElementCollection<SecurityCacheProviderData, CustomSecurityCacheProviderData>)base[securityCacheProvidersProperty];
			}
		}
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var defaultLoggerRegistrations = GetDefaultSecurityEventLoggerRegistrations(configurationSource);

            var authorizationProviderRegistrations = AuthorizationProviders.SelectMany(azp => azp.GetRegistrations(configurationSource));
            authorizationProviderRegistrations = SetDefaultAuthorizationProvider(authorizationProviderRegistrations);

            var securityCacheProviderRegistrations = SecurityCacheProviders.SelectMany(scp => scp.GetRegistrations(configurationSource));
            securityCacheProviderRegistrations = SetDefaultSecurityCacheProvider(securityCacheProviderRegistrations);

            return defaultLoggerRegistrations
                .Concat(authorizationProviderRegistrations)
                .Concat(securityCacheProviderRegistrations);
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

        private IEnumerable<TypeRegistration> GetDefaultSecurityEventLoggerRegistrations(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            yield return new TypeRegistration<DefaultSecurityEventLogger>(
                () => new DefaultSecurityEventLogger(instrumentationSection.EventLoggingEnabled, instrumentationSection.WmiEnabled))
                {
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }

        private IEnumerable<TypeRegistration> SetDefaultSecurityCacheProvider(IEnumerable<TypeRegistration> securityCacheProviderRegistrations)
        {
            foreach (TypeRegistration registration in securityCacheProviderRegistrations)
            {
                if (registration.ServiceType == typeof(ISecurityCacheProvider) && string.Equals(registration.Name, DefaultSecurityCacheProviderName))
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

        private IEnumerable<TypeRegistration> SetDefaultAuthorizationProvider(IEnumerable<TypeRegistration> authorizationProviderRegistrations)
        {
            foreach (TypeRegistration registration in authorizationProviderRegistrations)
            {
                if (registration.ServiceType == typeof(IAuthorizationProvider) && string.Equals(registration.Name, DefaultAuthorizationProviderName))
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
    }
}
