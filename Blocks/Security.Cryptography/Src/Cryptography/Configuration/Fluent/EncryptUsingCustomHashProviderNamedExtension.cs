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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Security.Cryptography;
using System.Collections.Specialized;

using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureCryptography"/> extensions to support configuring custom <see cref="IHashProvider"/> instances.
    /// </summary>
    /// <seealso cref="CustomHashProviderData"/>
    /// <seealso cref="IHashProvider"/>
    public static class EncryptUsingCustomHashCryptoProviderNamedExtension
    {
        /// <summary>
        /// Adds a custom hash provider of type <typeparamref name="TCustomHashCryptoProvider"/> to the cryptography configuration.
        /// </summary>
        /// <typeparam name="TCustomHashCryptoProvider">The concrete type of the hash provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customHashCryptoProviderName">The name of the hash provider that should be added to configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomHashProviderData"/>. </returns>
        /// <seealso cref="CustomHashProviderData"/>
        public static IEncryptUsingCustomHashProviderNamed EncryptUsingCustomHashProviderNamed<TCustomHashCryptoProvider>(this IConfigureCryptography context, string customHashCryptoProviderName)
            where TCustomHashCryptoProvider : IHashProvider
        {
            return EncryptUsingCustomHashProviderNamed(context, customHashCryptoProviderName, typeof(TCustomHashCryptoProvider), new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom hash provider of type <paramref name="customHashCryptoProviderType"/> to the cryptography configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customHashCryptoProviderType">The concrete type of the hash provider.</param>
        /// <param name="customHashCryptoProviderName">The name of the hash provider that should be added to configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomHashProviderData"/>. </returns>
        /// <seealso cref="CustomHashProviderData"/>
        public static IEncryptUsingCustomHashProviderNamed EncryptUsingCustomHashProviderNamed(this IConfigureCryptography context, string customHashCryptoProviderName, Type customHashCryptoProviderType)
        {
            return EncryptUsingCustomHashProviderNamed(context, customHashCryptoProviderName, customHashCryptoProviderType, new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom hash provider of type <typeparamref name="TCustomHashCryptoProvider"/> to the cryptography configuration.
        /// </summary>
        /// <typeparam name="TCustomHashCryptoProvider">The concrete type of the hash provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customHashCryptoProviderName">The name of the hash provider that should be added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomHashCryptoProvider"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomHashProviderData"/>. </returns>
        /// <seealso cref="CustomHashProviderData"/>
        public static IEncryptUsingCustomHashProviderNamed EncryptUsingCustomHashProviderNamed<TCustomHashCryptoProvider>(this IConfigureCryptography context, string customHashCryptoProviderName, NameValueCollection attributes)
            where TCustomHashCryptoProvider : IHashProvider
        {
            return EncryptUsingCustomHashProviderNamed(context, customHashCryptoProviderName, typeof(TCustomHashCryptoProvider), attributes);
        }

        /// <summary>
        /// Adds a custom hash provider of type <paramref name="customHashCryptoProviderType"/> to the cryptography configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customHashCryptoProviderType">The concrete type of the hash provider.</param>
        /// <param name="customHashCryptoProviderName">The name of the hash provider that should be added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customHashCryptoProviderType"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomHashProviderData"/>. </returns>
        /// <seealso cref="CustomHashProviderData"/>
        public static IEncryptUsingCustomHashProviderNamed EncryptUsingCustomHashProviderNamed(this IConfigureCryptography context, string customHashCryptoProviderName, Type customHashCryptoProviderType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(customHashCryptoProviderName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "customHashCryptoProviderName");
            if (customHashCryptoProviderType == null) throw new ArgumentNullException("customHashCryptoProviderType");
            if (attributes == null) throw new ArgumentNullException("attributes");

            if (!typeof(IHashProvider).IsAssignableFrom(customHashCryptoProviderType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustImplementInterface, typeof(IHashProvider)), "customHashCryptoProviderType");

            return new EncryptUsingCustomHashProviderNamedBuilder(context, customHashCryptoProviderName, customHashCryptoProviderType, attributes);
        }

        private class EncryptUsingCustomHashProviderNamedBuilder : ConfigureCryptographyExtension, IEncryptUsingCustomHashProviderNamed
        {
            CustomHashProviderData customHashProviderData;
            public EncryptUsingCustomHashProviderNamedBuilder(IConfigureCryptography context, string customHashCryptoProviderName, Type customHashCryptoProviderType, NameValueCollection attributes)
                : base(context)
            {
                customHashProviderData = new CustomHashProviderData
                {
                    Name = customHashCryptoProviderName,
                    Type = customHashCryptoProviderType
                };
                customHashProviderData.Attributes.Add(attributes);

                base.CryptographySettings.HashProviders.Add(customHashProviderData);
            }

            IConfigureCryptography IEncryptUsingCustomHashProviderNamed.SetAsDefault()
            {
                base.CryptographySettings.DefaultHashProviderName = customHashProviderData.Name;
                return this;
            }

        }
    }
}
