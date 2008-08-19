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
using System.Configuration.Provider;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the configuration data for the 
    /// security providers.
    /// </summary>
    public class SecuritySettings : SerializableConfigurationSection
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
		public NameTypeConfigurationElementCollection<SecurityCacheProviderData, CustomSecurityCacheProviderData> SecurityCacheProviders
		{
			get
			{
                return (NameTypeConfigurationElementCollection<SecurityCacheProviderData, CustomSecurityCacheProviderData>)base[securityCacheProvidersProperty];
			}
		}
    }
}