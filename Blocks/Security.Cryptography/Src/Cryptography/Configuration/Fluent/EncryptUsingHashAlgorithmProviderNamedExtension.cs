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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureCryptography"/> extensions to support configuring <see cref="HashAlgorithmProvider"/> instances.
    /// </summary>
    /// <seealso cref="HashAlgorithmProvider"/>
    /// <seealso cref="HashAlgorithmProviderData"/>
    public static class EncryptUsingHashAlgorithmProviderNamedExtension
    {
        /// <summary>
        /// Adds a <see cref="HashAlgorithmProvider"/> to the cryptography configuration settings.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="algorithmProviderName">The name of the <see cref="HashAlgorithmProvider"/> that should be configured.</param>
        /// <returns>Fluent interface to further configure the created <see cref="HashAlgorithmProviderData"/>.</returns>
        /// <seealso cref="HashAlgorithmProvider"/>
        /// <seealso cref="HashAlgorithmProviderData"/>
        public static IEncryptUsingHashAlgorithmProviderNamed EncryptUsingHashAlgorithmProviderNamed(this IConfigureCryptography context, string algorithmProviderName)
        {
            if (string.IsNullOrEmpty(algorithmProviderName)) 
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "algorithmProviderName");

            return new EncryptUsingHashAlgorithmProviderNamedBuilder(context, algorithmProviderName);
        }

        private class EncryptUsingHashAlgorithmProviderNamedBuilder : ConfigureCryptographyExtension, IEncryptUsingHashAlgorithmProviderNamed, IEncryptUsingHashAlgorithmProviderNamedOptions
        {
            HashAlgorithmProviderData providerData;
            public EncryptUsingHashAlgorithmProviderNamedBuilder(IConfigureCryptography context, string algorithmProviderName)
                :base(context)
            {
                providerData = new HashAlgorithmProviderData
                {
                    Name = algorithmProviderName
                };

                CryptographySettings.HashProviders.Add(providerData);
            }

            public IEncryptUsingHashAlgorithmProviderNamed SetAsDefault()
            {
                CryptographySettings.DefaultHashProviderName = providerData.Name;
                
                return this;
            }

            public IEncryptUsingHashAlgorithmProviderNamedOptions WithOptions
            {
                get { return this; }
            }

            public IEncryptUsingHashAlgorithmProviderNamedOptions UsingHashAlgorithm(Type algorithmType)
            {
                if (algorithmType == null) throw new ArgumentNullException("algorithmType");

                if (!typeof(HashAlgorithm).IsAssignableFrom(algorithmType))
                    throw new ArgumentException(string.Format(Resources.Culture,
                        Resources.ExceptionTypeMustDeriveFromType, typeof(HashAlgorithm)), "algorithmType");

                providerData.AlgorithmType = algorithmType;

                return this;
            }

            public IEncryptUsingHashAlgorithmProviderNamedOptions UsingHashAlgorithm<THashAlgorithm>() where THashAlgorithm : HashAlgorithm
            {
                providerData.AlgorithmType = typeof(THashAlgorithm);

                return this;
            }

            public IEncryptUsingHashAlgorithmProviderNamedOptions DisableSalt()
            {
                providerData.SaltEnabled = false;

                return this;
            }
        }
    }
}
