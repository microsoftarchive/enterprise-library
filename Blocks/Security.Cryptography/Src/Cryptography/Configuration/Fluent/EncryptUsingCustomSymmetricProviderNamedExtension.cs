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
    /// <see cref="IConfigureCryptography"/> extensions to support configuring custom <see cref="ISymmetricCryptoProvider"/> instances.
    /// </summary>
    /// <seealso cref="CustomSymmetricCryptoProviderData"/>
    /// <seealso cref="ISymmetricCryptoProvider"/>
    public static class EncryptUsingCustomSymmetricProviderNamedExtension
    {
        /// <summary>
        /// Adds a custom symmetric crypto provider of type <typeparamref name="TCustomSymmetricCryptoProvider"/> to the cryptography configuration.
        /// </summary>
        /// <typeparam name="TCustomSymmetricCryptoProvider">The concrete type of the symmetric crypto provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customSymmetricCryptoProviderName">The name of the symmetric crypto provider that should be added to configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSymmetricCryptoProviderData"/>. </returns>
        /// <seealso cref="CustomSymmetricCryptoProviderData"/>
        public static IEncryptUsingCustomSymmetricProviderNamed EncryptUsingCustomSymmetricProviderNamed<TCustomSymmetricCryptoProvider>(this IConfigureCryptography context, string customSymmetricCryptoProviderName)
            where TCustomSymmetricCryptoProvider : ISymmetricCryptoProvider
        {
            return EncryptUsingCustomSymmetricProviderNamed(context, customSymmetricCryptoProviderName, typeof(TCustomSymmetricCryptoProvider), new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom symmetric crypto provider of type <paramref name="customSymmetricCryptoProviderType"/> to the cryptography configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customSymmetricCryptoProviderName">The name of the symmetric crypto provider that should be added to configuration.</param>
        /// <param name="customSymmetricCryptoProviderType">The concrete type of the symmetric crypto provider.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSymmetricCryptoProviderData"/>. </returns>
        /// <seealso cref="CustomSymmetricCryptoProviderData"/>
        public static IEncryptUsingCustomSymmetricProviderNamed EncryptUsingCustomSymmetricProviderNamed(this IConfigureCryptography context, string customSymmetricCryptoProviderName, Type customSymmetricCryptoProviderType)
        {
            return EncryptUsingCustomSymmetricProviderNamed(context, customSymmetricCryptoProviderName, customSymmetricCryptoProviderType, new NameValueCollection());
        }

        /// <summary>
        /// Adds a custom symmetric crypto provider of type <typeparamref name="TCustomSymmetricCryptoProvider"/> to the cryptography configuration.
        /// </summary>
        /// <typeparam name="TCustomSymmetricCryptoProvider">The concrete type of the symmetric crypto provider.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customSymmetricCryptoProviderName">The name of the symmetric crypto provider that should be added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomSymmetricCryptoProvider"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSymmetricCryptoProviderData"/>. </returns>
        /// <seealso cref="CustomSymmetricCryptoProviderData"/>
        public static IEncryptUsingCustomSymmetricProviderNamed EncryptUsingCustomSymmetricProviderNamed<TCustomSymmetricCryptoProvider>(this IConfigureCryptography context, string customSymmetricCryptoProviderName, NameValueCollection attributes)
            where TCustomSymmetricCryptoProvider : ISymmetricCryptoProvider
        {
            return EncryptUsingCustomSymmetricProviderNamed(context, customSymmetricCryptoProviderName, typeof(TCustomSymmetricCryptoProvider), attributes);
        }

        /// <summary>
        /// Adds a custom symmetric crypto provider of type <paramref name="customSymmetricCryptoProviderType"/> to the cryptography configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customSymmetricCryptoProviderName">The name of the symmetric crypto provider that should be added to configuration.</param>
        /// <param name="customSymmetricCryptoProviderType">The concrete type of the symmetric crypto provider.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customSymmetricCryptoProviderType"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomSymmetricCryptoProviderData"/>. </returns>
        /// <seealso cref="CustomSymmetricCryptoProviderData"/>
        public static IEncryptUsingCustomSymmetricProviderNamed EncryptUsingCustomSymmetricProviderNamed(this IConfigureCryptography context, string customSymmetricCryptoProviderName, Type customSymmetricCryptoProviderType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(customSymmetricCryptoProviderName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "customSymmetricCryptoProviderName");
            if (customSymmetricCryptoProviderType == null) throw new ArgumentNullException("customSymmetricCryptoProviderType");
            if (attributes == null) throw new ArgumentNullException("attributes");

            if (!typeof(ISymmetricCryptoProvider).IsAssignableFrom(customSymmetricCryptoProviderType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustImplementInterface, typeof(ISymmetricCryptoProvider)), "customSymmetricCryptoProviderType");

            return new EncryptUsingCustomSymmetricProviderNamedBuilder(context, customSymmetricCryptoProviderName, customSymmetricCryptoProviderType, attributes);
        }

        private class EncryptUsingCustomSymmetricProviderNamedBuilder : ConfigureCryptographyExtension, IEncryptUsingCustomSymmetricProviderNamed
        {
            CustomSymmetricCryptoProviderData customSymmetricProviderData;
            public EncryptUsingCustomSymmetricProviderNamedBuilder(IConfigureCryptography context, string customSymmetricCryptoProviderName, Type customSymmetricCryptoProviderType, NameValueCollection attributes)
                : base(context)
            {
                customSymmetricProviderData = new CustomSymmetricCryptoProviderData(customSymmetricCryptoProviderName, customSymmetricCryptoProviderType);
                customSymmetricProviderData.Attributes.Add(attributes);

                base.CryptographySettings.SymmetricCryptoProviders.Add(customSymmetricProviderData);

            }

            IConfigureCryptography IEncryptUsingCustomSymmetricProviderNamed.SetAsDefault()
            {
                base.CryptographySettings.DefaultSymmetricCryptoProviderName = customSymmetricProviderData.Name;

                return this;
            }
        }
    }

}
