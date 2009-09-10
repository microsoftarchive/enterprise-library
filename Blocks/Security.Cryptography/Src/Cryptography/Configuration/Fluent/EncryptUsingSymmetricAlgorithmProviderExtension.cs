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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureCryptography"/> extensions to support configuring <see cref="SymmetricAlgorithmProvider"/> instances.
    /// </summary>
    /// <seealso cref="SymmetricAlgorithmProvider"/>
    /// <seealso cref="SymmetricAlgorithmProviderData"/>
    public static class EncryptUsingSymmetricProviderNamedExtension
    {
        /// <summary>
        /// Adds a <see cref="SymmetricAlgorithmProvider"/> to the cryptography configuration settings.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="algorithmProviderName">The name of the <see cref="SymmetricAlgorithmProvider"/> that should be configured.</param>
        /// <returns>Fluent interface to further configure the created <see cref="SymmetricAlgorithmProviderData"/>.</returns>
        /// <seealso cref="SymmetricAlgorithmProvider"/>
        /// <seealso cref="SymmetricAlgorithmProviderData"/>
        public static IEncryptUsingSymmetricProviderNamed EncryptUsingSymmetricAlgorithmProviderNamed(this IConfigureCryptography context, string algorithmProviderName)
        {
            if (string.IsNullOrEmpty(algorithmProviderName)) 
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "algorithmProviderName");

            return new EncryptUsingSymmetricProviderNamedBuilder(context, algorithmProviderName);
        }

        private class EncryptUsingSymmetricProviderNamedBuilder : ConfigureCryptographyExtension, IEncryptUsingSymmetricProviderNamed, IEncryptUsingSymmetricProviderNamedOptions
        {
            SymmetricAlgorithmProviderData providerData;

            public EncryptUsingSymmetricProviderNamedBuilder(IConfigureCryptography context, string algorithmProviderName)
                : base(context)
            {
                providerData = new SymmetricAlgorithmProviderData
                {
                    Name = algorithmProviderName,
                    Type = typeof(SymmetricAlgorithmProvider) 
                };
                base.CryptographySettings.SymmetricCryptoProviders.Add(providerData);
            }

            IEncryptUsingSymmetricProviderNamed IEncryptUsingSymmetricProviderNamed.SetAsDefault()
            {
                base.CryptographySettings.DefaultSymmetricCryptoProviderName = providerData.Name;

                return this;
            }

            IEncryptUsingSymmetricProviderNamedOptions IEncryptUsingSymmetricProviderNamed.WithOptions
            {
                get { return this; }
            }

            IEncryptUsingSymmetricProviderNamedOptions IEncryptUsingSymmetricProviderNamedOptions.UsingSymmetricAlgorithm(Type algorithmType)
            {

                if (algorithmType == null)
                    throw new ArgumentNullException(Resources.ExceptionStringNullOrEmpty, "algorithmType");

                if (!typeof(SymmetricAlgorithm).IsAssignableFrom(algorithmType))
                    throw new ArgumentException(string.Format(Resources.Culture,
                        Resources.ExceptionTypeMustDeriveFromType, typeof(SymmetricAlgorithm)), "algorithmType");


                providerData.AlgorithmType = algorithmType;

                return this;
            }

            IEncryptUsingSymmetricProviderNamedOptions IEncryptUsingSymmetricProviderNamedOptions.UsingSymmetricAlgorithm<TSymmetricAlgorithm>()
            {
                providerData.AlgorithmType = typeof(TSymmetricAlgorithm);

                return this;
            }

            IEncryptUsingSymmetricProviderNamedOptions IEncryptUsingSymmetricProviderNamedOptions.UseKeyFile(string keyFileName, DataProtectionScope keyScope)
            {
                providerData.ProtectedKeyFilename = keyFileName;
                providerData.ProtectedKeyProtectionScope = keyScope;

                return this;
            }
        }
    }
}
