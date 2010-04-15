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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Fluent interface used to configure a custom <see cref="ISecurityCacheProvider"/> instance.
    /// </summary>
    /// <seealso cref="ISecurityCacheProvider"/>
    /// <seealso cref="CustomSecurityCacheProviderData"/>
    public static class CacheSecurityInCustomStoreExtension
    {
        
        /// <summary>
        /// Adds a custom <see cref="ISecurityCacheProvider"/> instance of type <typeparamref name="TCustomSecurityCacheProvider"/> to the security configuration.
        /// </summary>
        /// <typeparam name="TCustomSecurityCacheProvider">The concrete type of the custom security cache provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customCacheProviderName">The name of the security cache provider that should be added to configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSecurityCacheProviderData"/>. </returns>
        /// <seealso cref="ISecurityCacheProvider"/>
        /// <seealso cref="CustomSecurityCacheProviderData"/>
        public static ICacheSecurityInCustomStore CacheSecurityInCustomStoreNamed<TCustomSecurityCacheProvider>(this IConfigureSecuritySettings context, string customCacheProviderName)
            where TCustomSecurityCacheProvider : ISecurityCacheProvider
        {
            return CacheSecurityInCustomStoreNamed(context, customCacheProviderName, typeof(TCustomSecurityCacheProvider), new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom <see cref="ISecurityCacheProvider"/> instance of type <paramref name="customCacheProviderType"/> to the security configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customCacheProviderName">The name of the security cache provider that should be added to configuration.</param>
        /// <param name="customCacheProviderType">The concrete type of the custom security cache provider.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSecurityCacheProviderData"/>. </returns>
        /// <seealso cref="ISecurityCacheProvider"/>
        /// <seealso cref="CustomSecurityCacheProviderData"/>
        public static ICacheSecurityInCustomStore CacheSecurityInCustomStoreNamed(this IConfigureSecuritySettings context, string customCacheProviderName, Type customCacheProviderType)
        {
            return CacheSecurityInCustomStoreNamed(context, customCacheProviderName, customCacheProviderType, new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom <see cref="ISecurityCacheProvider"/> instance of type <typeparamref name="TCustomSecurityCacheProvider"/> to the security configuration.
        /// </summary>
        /// <typeparam name="TCustomSecurityCacheProvider">The concrete type of the custom security cache provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customCacheProviderName">The name of the security cache provider that should be added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomSecurityCacheProvider"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSecurityCacheProviderData"/>. </returns>
        /// <seealso cref="ISecurityCacheProvider"/>
        /// <seealso cref="CustomSecurityCacheProviderData"/>
        public static ICacheSecurityInCustomStore CacheSecurityInCustomStoreNamed<TCustomSecurityCacheProvider>(this IConfigureSecuritySettings context, string customCacheProviderName, NameValueCollection attributes)
            where TCustomSecurityCacheProvider : ISecurityCacheProvider
        {
            return CacheSecurityInCustomStoreNamed(context, customCacheProviderName, typeof(TCustomSecurityCacheProvider), attributes);
        }

        /// <summary>
        /// Adds a custom <see cref="ISecurityCacheProvider"/> instance of type <paramref name="customCacheProviderType"/> to the security configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customCacheProviderName">The name of the security cache provider that should be added to configuration.</param>
        /// <param name="customCacheProviderType">The concrete type of the custom security cache provider.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customCacheProviderType"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSecurityCacheProviderData"/>. </returns>
        /// <seealso cref="ISecurityCacheProvider"/>
        /// <seealso cref="CustomSecurityCacheProviderData"/>
        public static ICacheSecurityInCustomStore CacheSecurityInCustomStoreNamed(this IConfigureSecuritySettings context, string customCacheProviderName, Type customCacheProviderType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(customCacheProviderName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "customCacheProviderName");
            if (customCacheProviderType == null) throw new ArgumentNullException("customCacheProviderType");
            if (attributes == null) throw new ArgumentNullException("attributes");

            if (!typeof(ISecurityCacheProvider).IsAssignableFrom(customCacheProviderType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustImplementInterface, typeof(ISecurityCacheProvider)), "customCacheProviderType");

            return new CacheSecurityInCustomStoreBuilder(context, customCacheProviderName, customCacheProviderType, attributes);
        }

        private class CacheSecurityInCustomStoreBuilder : ConfigureSecuritySettingsExtension, ICacheSecurityInCustomStore, IConfigureSecuritySettings
        {
            CustomSecurityCacheProviderData customCacheProvider;
            
            public CacheSecurityInCustomStoreBuilder(IConfigureSecuritySettings context, string customCacheProviderName, Type customCacheProviderType, NameValueCollection attributes)
                :base(context)
            {
                customCacheProvider = new CustomSecurityCacheProviderData
                {
                    Name = customCacheProviderName,
                    Type = customCacheProviderType
                };

                customCacheProvider.Attributes.Add(attributes);

                SecuritySettings.SecurityCacheProviders.Add(customCacheProvider);
            }

            public IConfigureSecuritySettings SetAsDefault()
            {
                SecuritySettings.DefaultSecurityCacheProviderName = customCacheProvider.Name;
                
                return this;
            }
        }
    }
}
