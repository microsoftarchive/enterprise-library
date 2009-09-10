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
    /// Fluent interface used to configure a <see cref="KeyedHashAlgorithmProvider"/> instance.
    /// </summary>
    /// <seealso cref="KeyedHashAlgorithmProvider"/>
    /// <seealso cref="KeyedHashAlgorithmProviderData"/>
    public interface IEncryptUsingKeyedHashAlgorithmProviderNamed : IConfigureCryptography, IFluentInterface
    {
        /// <summary>
        /// Specifies this <see cref="KeyedHashAlgorithmProvider"/> should be the cryptography blocks default <see cref="IHashProvider"/> instance.
        /// </summary>
        /// <seealso cref="KeyedHashAlgorithmProvider"/>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        IEncryptUsingKeyedHashAlgorithmProviderNamed SetAsDefault();

        /// <summary>
        /// Returns a fluent interface to further configure the current <see cref="KeyedHashAlgorithmProvider"/> instance. 
        /// </summary>
        /// <seealso cref="KeyedHashAlgorithmProvider"/>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        IEncryptUsingKeyedHashAlgorithmProviderNamedOptions WithOptions { get; }
    }

    /// <summary>
    /// Fluent interface used to further configure a <see cref="KeyedHashAlgorithmProvider"/> instance.
    /// </summary>
    /// <seealso cref="KeyedHashAlgorithmProvider"/>
    /// <seealso cref="KeyedHashAlgorithmProviderData"/>
    public interface IEncryptUsingKeyedHashAlgorithmProviderNamedOptions : IEncryptUsingKeyedHashAlgorithmProviderNamed, IFluentInterface
    {
        /// <summary>
        /// Specifies the hash algorithm that should be used by this <see cref="KeyedHashAlgorithmProvider"/> to create a hash.
        /// </summary>
        /// <param name="algorithmType">The <see cref="KeyedHashAlgorithm"/> that implements the hashing algorithm.</param>
        /// <seealso cref="KeyedHashAlgorithmProvider"/>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        IEncryptUsingKeyedHashAlgorithmProviderNamedOptions UsingKeyedHashAlgorithm(Type algorithmType);

        /// <summary>
        /// Specifies the hash algorithm that should be used by this <see cref="KeyedHashAlgorithmProvider"/> to create a hash.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The <see cref="KeyedHashAlgorithm"/> that implements the hashing algorithm.</typeparam>
        /// <seealso cref="KeyedHashAlgorithmProvider"/>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        IEncryptUsingKeyedHashAlgorithmProviderNamedOptions UsingKeyedHashAlgorithm<TKeyedHashAlgorithm>()
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;


        /// <summary>
        /// Specifies no salt should be used when creating a hash.
        /// </summary>
        /// <seealso cref="KeyedHashAlgorithmProvider"/>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        IEncryptUsingKeyedHashAlgorithmProviderNamedOptions DisableSalt();

        /// <summary>
        /// Specifies the location of a file that contains a dpapi protected key for creating the hash.
        /// </summary>
        /// <param name="keyFileName">The path to a dpapi protected keyfile that contains the key that should be used to create a hash.</param>
        /// <param name="keyScope">The <see cref="DataProtectionScope"/> that should be used to read the key from the key-file.</param>
        /// <seealso cref="KeyedHashAlgorithmProvider"/>
        /// <seealso cref="KeyedHashAlgorithmProviderData"/>
        IEncryptUsingKeyedHashAlgorithmProviderNamedOptions UseKeyFile(string keyFileName, DataProtectionScope keyScope);
    }
}
