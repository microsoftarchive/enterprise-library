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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureCryptography"/> extensions to support configuring <see cref="DpapiSymmetricCryptoProvider"/> instances.
    /// </summary>
    /// <seealso cref="DpapiSymmetricCryptoProvider"/>
    /// <seealso cref="DpapiSymmetricCryptoProviderData"/>
    public static class EncryptUsingDPAPIProviderNamedExtension
    {
        /// <summary>
        /// Adds a <see cref="DpapiSymmetricCryptoProvider"/> to the cryptography configuration settings.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="dpapiProviderName">The name of the <see cref="DpapiSymmetricCryptoProvider"/> that should be configured.</param>
        /// <returns>Fluent interface to further configure the created <see cref="DpapiSymmetricCryptoProviderData"/>.</returns>
        /// <seealso cref="DpapiSymmetricCryptoProvider"/>
        /// <seealso cref="DpapiSymmetricCryptoProviderData"/>
        public static IEncryptUsingDPAPIProviderNamed EncryptUsingDPAPIProviderNamed(this IConfigureCryptography context, string dpapiProviderName)
        {
            if (string.IsNullOrEmpty(dpapiProviderName)) throw new ArgumentException("dpapiProviderName");

            return new EncryptUsingDPAPIProviderNamedBuilder(context, dpapiProviderName);
        }

        private class EncryptUsingDPAPIProviderNamedBuilder : ConfigureCryptographyExtension, IEncryptUsingDPAPIProviderNamed
        {
            DpapiSymmetricCryptoProviderData providerData;

            public EncryptUsingDPAPIProviderNamedBuilder(IConfigureCryptography context, string dpapiProviderName)
                : base(context)
            {
                providerData = new DpapiSymmetricCryptoProviderData
                {
                    Name = dpapiProviderName
                };

                base.CryptographySettings.SymmetricCryptoProviders.Add(providerData);
            }

            public IEncryptUsingDPAPIProviderNamed SetAsDefault()
            {
                base.CryptographySettings.DefaultSymmetricCryptoProviderName = providerData.Name;

                return this;
            }

            public IEncryptUsingDPAPIProviderNamed SetProtectionScope(DataProtectionScope scope)
            {
                providerData.Scope = scope;
                return this;
            }
        }
    }
}
