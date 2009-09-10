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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Fluent interface used to configure a custom <see cref="IAuthorizationProvider"/> instance.
    /// </summary>
    /// <seealso cref="IAuthorizationProvider"/>
    /// <seealso cref="CustomAuthorizationProviderData"/>
    public static class AuthorizeUsingCustomProviderExtension
    {

        /// <summary>
        /// Adds a custom <see cref="IAuthorizationProvider"/> instance of type <typeparamref name="TCustomAuthorizationProvider"/> to the security configuration.
        /// </summary>
        /// <typeparam name="TCustomAuthorizationProvider">The concrete type of the custom authorization provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customAuthorizationProviderName">The name of the authorization provider that should be added to configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomAuthorizationProviderData"/>. </returns>
        /// <seealso cref="IAuthorizationProvider"/>
        /// <seealso cref="CustomAuthorizationProviderData"/>
        public static IAuthorizeUsingCustomProvider AuthorizeUsingCustomProviderNamed<TCustomAuthorizationProvider>(this IConfigureSecuritySettings context, string customAuthorizationProviderName)
            where TCustomAuthorizationProvider : IAuthorizationProvider
        {
            return AuthorizeUsingCustomProviderNamed(context, customAuthorizationProviderName, typeof(TCustomAuthorizationProvider), new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom <see cref="IAuthorizationProvider"/> instance of type <paramref name="customAuthorizationProviderType"/> to the security configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customAuthorizationProviderName">The name of the authorization provider that should be added to configuration.</param>
        /// <param name="customAuthorizationProviderType">The concrete type of the custom authorization provider.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomAuthorizationProviderData"/>. </returns>
        /// <seealso cref="IAuthorizationProvider"/>
        /// <seealso cref="CustomAuthorizationProviderData"/>
        public static IAuthorizeUsingCustomProvider AuthorizeUsingCustomProviderNamed(this IConfigureSecuritySettings context, string customAuthorizationProviderName, Type customAuthorizationProviderType)
        {
            return AuthorizeUsingCustomProviderNamed(context, customAuthorizationProviderName, customAuthorizationProviderType, new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom <see cref="IAuthorizationProvider"/> instance of type <typeparamref name="TCustomAuthorizationProvider"/> to the security configuration.
        /// </summary>
        /// <typeparam name="TCustomAuthorizationProvider">The concrete type of the custom authorization provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customAuthorizationProviderName">The name of the authorization provider that should be added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomAuthorizationProvider"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomAuthorizationProviderData"/>. </returns>
        /// <seealso cref="IAuthorizationProvider"/>
        /// <seealso cref="CustomAuthorizationProviderData"/>
        public static IAuthorizeUsingCustomProvider AuthorizeUsingCustomProviderNamed<TCustomAuthorizationProvider>(this IConfigureSecuritySettings context, string customAuthorizationProviderName, NameValueCollection attributes)
            where TCustomAuthorizationProvider : IAuthorizationProvider
        {
            return AuthorizeUsingCustomProviderNamed(context, customAuthorizationProviderName, typeof(TCustomAuthorizationProvider), attributes);
        }

        /// <summary>
        /// Adds a custom <see cref="IAuthorizationProvider"/> instance of type <paramref name="customAuthorizationProviderType"/> to the security configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customAuthorizationProviderName">The name of the authorization provider that should be added to configuration.</param>
        /// <param name="customAuthorizationProviderType">The concrete type of the custom authorization provider.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customAuthorizationProviderType"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomAuthorizationProviderData"/>. </returns>
        /// <seealso cref="IAuthorizationProvider"/>
        /// <seealso cref="CustomAuthorizationProviderData"/>
        public static IAuthorizeUsingCustomProvider AuthorizeUsingCustomProviderNamed(this IConfigureSecuritySettings context, string customAuthorizationProviderName, Type customAuthorizationProviderType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(customAuthorizationProviderName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "customAuthorizationProviderName");
            if (customAuthorizationProviderType == null) throw new ArgumentNullException("customAuthorizationProviderType");
            if (attributes == null) throw new ArgumentNullException("attributes");

            if (!typeof(IAuthorizationProvider).IsAssignableFrom(customAuthorizationProviderType))
                throw new ArgumentException(string.Format(Resources.Culture,
                    Resources.ExceptionTypeMustImplementInterface, typeof(IAuthorizationProvider)), "customAuthorizationProviderType");

            return new AuthorizeUsingCustomProviderBuilder(context, customAuthorizationProviderName, customAuthorizationProviderType, attributes);
        }

        private class AuthorizeUsingCustomProviderBuilder : ConfigureSecuritySettingsExtension, IAuthorizeUsingCustomProvider
        {
            CustomAuthorizationProviderData customAuthZProvider;
            IConfigureSecuritySettings context;

            public AuthorizeUsingCustomProviderBuilder(IConfigureSecuritySettings context, string customAuthorizationProviderName, Type customAuthorizationProviderType, NameValueCollection attributes)
                : base(context)
            {
                this.context = context;
                customAuthZProvider = new CustomAuthorizationProviderData
                {
                    Name = customAuthorizationProviderName,
                    Type = customAuthorizationProviderType
                };

                customAuthZProvider.Attributes.Add(attributes);

                base.SecuritySettings.AuthorizationProviders.Add(customAuthZProvider);
            }

            IConfigureSecuritySettings IAuthorizeUsingCustomProvider.SetAsDefault()
            {
                base.SecuritySettings.DefaultAuthorizationProviderName = customAuthZProvider.Name;

                return this;
            }
        }
    }

}
