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

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// <para>Represents a view for navigating the <see cref="SecuritySettings"/> configuration data.</para>
    /// </summary>
    public class SecurityConfigurationView
    {
        private readonly IConfigurationSource configurationSource;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SecurityConfigurationView"/> class.</para>
        /// </summary>
        /// <param name="configurationSource">
        /// <para>A <see cref="IConfigurationSource"/> object.</para>
        /// </param>
        public SecurityConfigurationView(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");

            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// Gets the <see cref="SecuritySettings"/> section in the configuration source.
        /// </summary>
        /// <returns>The security section.</returns>
        public SecuritySettings GetSecuritySettings()
        {
            SecuritySettings settings = (SecuritySettings)configurationSource.GetSection(SecuritySettings.SectionName);
            if (settings == null)
            {
                throw new ConfigurationErrorsException(Resources.MissingSection);
            }

            return settings;
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="SecurityCacheProviderData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="SecurityCacheProviderData"/>.</para>
        /// </returns>
        public string GetDefaultSecurityCacheProviderName()
        {
            SecuritySettings settings = GetSecuritySettings();
            if (string.IsNullOrEmpty(settings.DefaultSecurityCacheProviderName))
            {
                throw new ConfigurationErrorsException(Resources.NoDefaultSecurityCacheProvider);
            }

            return settings.DefaultSecurityCacheProviderName;
        }

        /// <summary>
        /// <para>Gets the named <see cref="SecurityCacheProviderData"/> from configuration.</para>
        /// </summary>
        /// <param name="securityCacheProviderName">
        /// <para>The name of the <see cref="SecurityCacheProviderData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="SecurityCacheProviderData"/> from configuration.</para>
        /// </returns>
        public SecurityCacheProviderData GetSecurityCacheProviderData(string securityCacheProviderName)
        {
            SecuritySettings settings = GetSecuritySettings();
            SecurityCacheProviderData data = settings.SecurityCacheProviders.Get(securityCacheProviderName);
            if (data == null)
            {
                throw new ConfigurationErrorsException(string.Format(Resources.ExceptionSecurityCacheProviderNotFound, securityCacheProviderName));
            }
            return data;
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="AuthorizationProviderData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="AuthorizationProviderData"/>.</para>
        /// </returns>
        public string GetDefaultAuthorizationProviderName()
        {
            SecuritySettings settings = GetSecuritySettings();
            if (string.IsNullOrEmpty(settings.DefaultAuthorizationProviderName))
            {
                throw new ConfigurationErrorsException(Resources.NoDefaultAuthorizationProvider);
            }

            return settings.DefaultAuthorizationProviderName;
        }

        /// <summary>
        /// <para>Gets the named <see cref="AuthorizationProviderData"/> from configuration.</para>
        /// </summary>
        /// <param name="authorizationProviderName">
        /// <para>The name of the <see cref="AuthorizationProviderData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="AuthorizationProviderData"/> from configuration.</para>
        /// </returns>
        public AuthorizationProviderData GetAuthorizationProviderData(string authorizationProviderName)
        {
            SecuritySettings settings = GetSecuritySettings();
            AuthorizationProviderData data = settings.AuthorizationProviders.Get(authorizationProviderName);
            if (data == null)
            {
                throw new ConfigurationErrorsException(string.Format(Resources.ExceptionAuthorizationProviderNotFound, authorizationProviderName));
            }
            return data;
        }
    }
}