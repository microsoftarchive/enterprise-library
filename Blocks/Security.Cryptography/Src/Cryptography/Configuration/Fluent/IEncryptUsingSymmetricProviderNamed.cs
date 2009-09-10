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
    /// Fluent interface used to configure a <see cref="SymmetricAlgorithmProvider"/> instance.
    /// </summary>
    /// <seealso cref="SymmetricAlgorithmProvider"/>
    /// <seealso cref="SymmetricAlgorithmProviderData"/>
    public interface IEncryptUsingSymmetricProviderNamed : IConfigureCryptography, IFluentInterface
    {
        /// <summary>
        /// Specifies this <see cref="SymmetricAlgorithmProvider"/> should be the cryptography blocks default <see cref="ISymmetricCryptoProvider"/> instance.
        /// </summary>
        /// <seealso cref="SymmetricAlgorithmProvider"/>
        /// <seealso cref="SymmetricAlgorithmProviderData"/>
        IEncryptUsingSymmetricProviderNamed SetAsDefault();

        /// <summary>
        /// Returns a fluent interface to further configure the current <see cref="SymmetricAlgorithmProvider"/> instance. 
        /// </summary>
        /// <seealso cref="SymmetricAlgorithmProvider"/>
        /// <seealso cref="SymmetricAlgorithmProviderData"/>
        IEncryptUsingSymmetricProviderNamedOptions WithOptions { get; }
    }

    /// <summary>
    /// Fluent interface used to further configure a <see cref="SymmetricAlgorithmProvider"/> instance.
    /// </summary>
    /// <seealso cref="SymmetricAlgorithmProvider"/>
    /// <seealso cref="SymmetricAlgorithmProviderData"/>
    public interface IEncryptUsingSymmetricProviderNamedOptions : IEncryptUsingSymmetricProviderNamed, IFluentInterface
    {
        /// <summary>
        /// Specifies the symmetric cryptography algorithm that should be used by this <see cref="SymmetricAlgorithmProvider"/> to encrypt data.
        /// </summary>
        /// <param name="algorithmType">The <see cref="SymmetricAlgorithm"/> that implements the cryptography algorithm.</param>
        /// <seealso cref="SymmetricAlgorithmProvider"/>
        /// <seealso cref="SymmetricAlgorithmProviderData"/>
        IEncryptUsingSymmetricProviderNamedOptions UsingSymmetricAlgorithm(Type algorithmType);

        /// <summary>
        /// Specifies the symmetric cryptography algorithm that should be used by this <see cref="SymmetricAlgorithmProvider"/> to encrypt data.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The <see cref="SymmetricAlgorithm"/> that implements the cryptography algorithm.</typeparam>
        /// <seealso cref="SymmetricAlgorithmProvider"/>
        /// <seealso cref="SymmetricAlgorithmProviderData"/>
        IEncryptUsingSymmetricProviderNamedOptions UsingSymmetricAlgorithm<TSymmetricAlgorithm>()
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Specifies the location of a file that contains a dpapi protected key for encrypting.
        /// </summary>
        /// <param name="keyFileName">The path to a dpapi protected keyfile that contains the key that should be used to encrypt data.</param>
        /// <param name="keyScope">The <see cref="DataProtectionScope"/> that should be used to read the key from the key-file.</param>
        /// <seealso cref="SymmetricAlgorithmProvider"/>
        /// <seealso cref="SymmetricAlgorithmProviderData"/>
        IEncryptUsingSymmetricProviderNamedOptions UseKeyFile(string keyFileName, DataProtectionScope keyScope);
    }
}
