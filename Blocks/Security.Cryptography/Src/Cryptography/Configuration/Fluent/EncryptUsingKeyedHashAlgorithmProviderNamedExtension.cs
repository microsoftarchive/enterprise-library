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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureCryptography"/> extensions to support configuring <see cref="KeyedHashAlgorithmProvider"/> instances.
    /// </summary>
    /// <seealso cref="KeyedHashAlgorithmProvider"/>
    /// <seealso cref="KeyedHashAlgorithmProviderData"/>
    public static class EncryptUsingKeyedHashAlgorithmProviderNamedExtension
    {

        /// <summary>
        /// Adds a <see cref="KeyedHashAlgorithmProvider"/> to the cryptography configuration settings.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="algorithmProviderName">The name of the <see cref="KeyedHashAlgorithmProvider"/> that should be configured.</param>
        /// <returns>Fluent interface to further configure the created <see cref="KeyedHashAlgorithmProviderData"/>.</returns>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        public static IEncryptUsingKeyedHashAlgorithmProviderNamed EncryptUsingKeyedHashAlgorithmProviderNamed(this IConfigureCryptography context, string algorithmProviderName)
        {
            if (string.IsNullOrEmpty(algorithmProviderName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "algorithmProviderName");

            return new EncryptUsingKeyedHashAlgorithmProviderNamedBuilder(context, algorithmProviderName);
        }

        private class EncryptUsingKeyedHashAlgorithmProviderNamedBuilder : ConfigureCryptographyExtension, IEncryptUsingKeyedHashAlgorithmProviderNamed, IEncryptUsingKeyedHashAlgorithmProviderNamedOptions
        {
            KeyedHashAlgorithmProviderData providerData;
            public EncryptUsingKeyedHashAlgorithmProviderNamedBuilder(IConfigureCryptography context, string algorithmProviderName)
                : base(context)
            {
                providerData = new KeyedHashAlgorithmProviderData
                {
                    Name = algorithmProviderName
                };
                CryptographySettings.HashProviders.Add(providerData);
            }

            IEncryptUsingKeyedHashAlgorithmProviderNamed IEncryptUsingKeyedHashAlgorithmProviderNamed.SetAsDefault()
            {
                CryptographySettings.DefaultHashProviderName = providerData.Name;
                
                return this;
            }

            IEncryptUsingKeyedHashAlgorithmProviderNamedOptions IEncryptUsingKeyedHashAlgorithmProviderNamed.WithOptions
            {
                get { return this; }
            }

            IEncryptUsingKeyedHashAlgorithmProviderNamedOptions IEncryptUsingKeyedHashAlgorithmProviderNamedOptions.UsingKeyedHashAlgorithm(Type algorithmType)
            {
                if (algorithmType == null) throw new ArgumentNullException("algorithmType");

                if (!typeof(KeyedHashAlgorithm).IsAssignableFrom(algorithmType))
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Resources.ExceptionTypeMustDeriveFromType, typeof(KeyedHashAlgorithm)), "algorithmType");

                providerData.AlgorithmType = algorithmType;

                return this;
            }

            IEncryptUsingKeyedHashAlgorithmProviderNamedOptions IEncryptUsingKeyedHashAlgorithmProviderNamedOptions.UsingKeyedHashAlgorithm<TKeyedHashAlgorithm>()
            {
                providerData.AlgorithmType = typeof(TKeyedHashAlgorithm);

                return this;
            }

            IEncryptUsingKeyedHashAlgorithmProviderNamedOptions IEncryptUsingKeyedHashAlgorithmProviderNamedOptions.DisableSalt()
            {
                providerData.SaltEnabled = false;
                
                return this;
            }

            IEncryptUsingKeyedHashAlgorithmProviderNamedOptions IEncryptUsingKeyedHashAlgorithmProviderNamedOptions.UseKeyFile(string keyFileName, DataProtectionScope keyScope)
            {
                providerData.ProtectedKeyFilename = keyFileName;
                providerData.ProtectedKeyProtectionScope = keyScope;

                return this;
            }
        }
    }
}
