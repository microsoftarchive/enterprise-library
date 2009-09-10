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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="HashAlgorithmProvider"/> instance.
    /// </summary>
    /// <seealso cref="HashAlgorithmProvider"/>
    /// <seealso cref="HashAlgorithmProviderData"/>
    public interface IEncryptUsingHashAlgorithmProviderNamed : IConfigureCryptography, IFluentInterface
    {
        /// <summary>
        /// Specifies this <see cref="HashAlgorithmProvider"/> should be the cryptography blocks default <see cref="IHashProvider"/> instance.
        /// </summary>
        /// <seealso cref="HashAlgorithmProvider"/>
        /// <seealso cref="HashAlgorithmProviderData"/>
        IEncryptUsingHashAlgorithmProviderNamed SetAsDefault();

        /// <summary>
        /// Returns a fluent interface to further configure the current <see cref="HashAlgorithmProvider"/> instance. 
        /// </summary>
        /// <seealso cref="HashAlgorithmProvider"/>
        /// <seealso cref="HashAlgorithmProviderData"/>
        IEncryptUsingHashAlgorithmProviderNamedOptions WithOptions { get; }
    }

    /// <summary>
    /// Fluent interface used to further configure a <see cref="HashAlgorithmProvider"/> instance.
    /// </summary>
    /// <seealso cref="HashAlgorithmProvider"/>
    /// <seealso cref="HashAlgorithmProviderData"/>
    public interface IEncryptUsingHashAlgorithmProviderNamedOptions : IEncryptUsingHashAlgorithmProviderNamed, IFluentInterface
    {
        /// <summary>
        /// Specifies the hash algorithm that should be used by this <see cref="HashAlgorithmProvider"/> to create a hash.
        /// </summary>
        /// <param name="algorithmType">The <see cref="HashAlgorithm"/> that implements the hashing algorithm.</param>
        /// <seealso cref="HashAlgorithmProvider"/>
        /// <seealso cref="HashAlgorithmProviderData"/>
        IEncryptUsingHashAlgorithmProviderNamedOptions UsingHashAlgorithm(Type algorithmType);

        /// <summary>
        /// Specifies the hash algorithm that should be used by this <see cref="HashAlgorithmProvider"/> to create a hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The <see cref="HashAlgorithm"/> that implements the hashing algorithm.</typeparam>
        /// <seealso cref="HashAlgorithmProvider"/>
        /// <seealso cref="HashAlgorithmProviderData"/>
        IEncryptUsingHashAlgorithmProviderNamedOptions UsingHashAlgorithm<THashAlgorithm>()
            where THashAlgorithm : HashAlgorithm;

        /// <summary>
        /// Specifies no salt should be used when creating a hash.
        /// </summary>
        /// <seealso cref="HashAlgorithmProvider"/>
        /// <seealso cref="HashAlgorithmProviderData"/>
        IEncryptUsingHashAlgorithmProviderNamedOptions DisableSalt();
    }
}
