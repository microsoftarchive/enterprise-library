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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Non-static entry point to the cryptography functionality.
    /// </summary>
    public class CryptographyManagerImpl : CryptographyManager
    {
        private const string HashProvider = "IHashProvider";
        private const string SymmetricCryptoProvider = "ISymmetricCryptoProvider";
        private readonly IDictionary<string, IHashProvider> hashProviders;
        private readonly IDictionary<string, ISymmetricCryptoProvider> symmetricCryptoProviders;
        private readonly IDefaultCryptographyInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// Initializes a new instance of the class <see cref="CryptographyManagerImpl"/> giving a collection of <see cref="IHashProvider"/> and a collection of
        /// <see cref="ISymmetricCryptoProvider"/>
        /// </summary>
        /// <param name="hashProviders"></param>
        /// <param name="symmetricCryptoProviders"></param>
        public CryptographyManagerImpl(IDictionary<string, IHashProvider> hashProviders, IDictionary<string, ISymmetricCryptoProvider> symmetricCryptoProviders)
            :this(hashProviders, symmetricCryptoProviders, new NullDefaultCryptographyInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the class <see cref="CryptographyManagerImpl"/> giving a collection of <see cref="IHashProvider"/> and a collection of
        /// <see cref="ISymmetricCryptoProvider"/> and an <see cref="IDefaultCryptographyInstrumentationProvider"/>.
        /// </summary>
        /// <param name="hashProviders">Dictionary of named <see cref="IHashProvider"/> objects</param>
        /// <param name="symmetricCryptoProviders">Dictionary of named <see cref="ISymmetricCryptoProvider"/> objects.</param>
        /// <param name="instrumentationProvider">Instrumentation provider used to report configuration errors.</param>
        public CryptographyManagerImpl(
            IDictionary<string, IHashProvider> hashProviders, 
            IDictionary<string, ISymmetricCryptoProvider> symmetricCryptoProviders,
            IDefaultCryptographyInstrumentationProvider instrumentationProvider)
        {
            if (hashProviders == null)
                throw new ArgumentNullException("hashProviders");

            if (symmetricCryptoProviders == null)
                throw new ArgumentNullException("symmetricCryptoProviders");

            if(instrumentationProvider == null)
                throw new ArgumentNullException("instrumentationProvider");

            this.hashProviders = hashProviders;
            this.symmetricCryptoProviders = symmetricCryptoProviders;
            this.instrumentationProvider = instrumentationProvider;
        }

        /// <summary>
        /// Initializes a new instance of the class <see cref="CryptographyManagerImpl"/> given a collection of <see cref="IHashProvider"/> and a
        /// collection of <see cref="ISymmetricCryptoProvider"/>.
        /// </summary>
        /// <param name="hashProviderNames">Sequence of names of the hash providers as defined in configuration.</param>
        /// <param name="hashProviders">The hash providers corresponding to the names in <paramref name="hashProviderNames"/> at the same index.</param>
        /// <param name="cryptoProviderNames">Sequence of names of the crypto providers as defined in configuration.</param>
        /// <param name="symmetricCryptoProviders">The symmetric crypto providers corresponding to the names give in <paramref name="cryptoProviderNames"/>
        /// <param name="instrumentationProvider">The instrumentation provider used to report errors.</param>
        /// at the same index.</param>
        public CryptographyManagerImpl(IEnumerable<string> hashProviderNames, IEnumerable<IHashProvider> hashProviders,
            IEnumerable<string> cryptoProviderNames, IEnumerable<ISymmetricCryptoProvider> symmetricCryptoProviders,
            IDefaultCryptographyInstrumentationProvider instrumentationProvider)
            : this(hashProviderNames.ToDictionary(hashProviders), cryptoProviderNames.ToDictionary(symmetricCryptoProviders), instrumentationProvider)
        {
            
        }

        /// <overrides>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </overrides>
        /// <summary>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </summary>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        public override byte[] CreateHash(string hashInstance, byte[] plaintext)
        {
            IHashProvider hashProvider = GetHashProvider(hashInstance);

            return hashProvider.CreateHash(plaintext);
        }

        /// <summary>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </summary>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        public override string CreateHash(string hashInstance, string plaintext)
        {
            IHashProvider hashProvider = GetHashProvider(hashInstance);

            return Cryptographer.CreateHash(hashProvider, plaintext);
        }

        /// <overrides>
        /// Compares plain text input with a computed hash using the given hash provider instance.
        /// </overrides>
        /// <summary>
        /// Compares plain text input with a computed hash using the given hash provider instance.
        /// </summary>
        /// <remarks>
        /// Use this method to compare hash values. Since hashes may contain a random "salt" value, two seperately generated
        /// hashes of the same plain text may result in different values. 
        /// </remarks>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which you want to compare the hash to.</param>
        /// <param name="hashedText">The hash value for which you want to compare the input to.</param>
        /// <returns><c>true</c> if plainText hashed is equal to the hashedText. Otherwise, <c>false</c>.</returns>
        public override bool CompareHash(string hashInstance, byte[] plaintext, byte[] hashedText)
        {
            IHashProvider hashProvider = GetHashProvider(hashInstance);

            return hashProvider.CompareHash(plaintext, hashedText);
        }

        /// <summary>
        /// Compares plain text input with a computed hash using the given hash provider instance.
        /// </summary>
        /// <remarks>
        /// Use this method to compare hash values. Since hashes contain a random "salt" value, two seperately generated
        /// hashes of the same plain text will result in different values. 
        /// </remarks>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input as a string for which you want to compare the hash to.</param>
        /// <param name="hashedText">The hash as a string for which you want to compare the input to.</param>
        /// <returns><c>true</c> if plainText hashed is equal to the hashedText. Otherwise, <c>false</c>.</returns>
        public override bool CompareHash(string hashInstance, string plaintext, string hashedText)
        {
            IHashProvider hashProvider = GetHashProvider(hashInstance);

            return Cryptographer.CompareHash(hashProvider, plaintext, hashedText);
        }

        /// <summary>
        /// Encrypts a secret using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="plaintext">The input for which you want to encrypt.</param>
        /// <returns>The resulting cipher text.</returns>
        public override byte[] EncryptSymmetric(string symmetricInstance, byte[] plaintext)
        {
            ISymmetricCryptoProvider symmetricProvider = GetSymmetricCryptoProvider(symmetricInstance);

            return symmetricProvider.Encrypt(plaintext);
        }

        /// <summary>
        /// Encrypts a secret using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="plaintext">The input as a base64 encoded string for which you want to encrypt.</param>
        /// <returns>The resulting cipher text as a base64 encoded string.</returns>
        public override string EncryptSymmetric(string symmetricInstance, string plaintext)
        {
            ISymmetricCryptoProvider symmetricProvider = GetSymmetricCryptoProvider(symmetricInstance);

            return Cryptographer.EncryptSymmetric(symmetricProvider, plaintext);
        }

        /// <summary>
        /// Decrypts a cipher text using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="ciphertext">The cipher text for which you want to decrypt.</param>
        /// <returns>The resulting plain text.</returns>
        public override byte[] DecryptSymmetric(string symmetricInstance, byte[] ciphertext)
        {
            ISymmetricCryptoProvider symmetricProvider = GetSymmetricCryptoProvider(symmetricInstance);

            return symmetricProvider.Decrypt(ciphertext);
        }

        /// <summary>
        /// Decrypts a cipher text using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="ciphertextBase64">The cipher text as a base64 encoded string for which you want to decrypt.</param>
        /// <returns>The resulting plain text as a string.</returns>
        public override string DecryptSymmetric(string symmetricInstance, string ciphertextBase64)
        {
            ISymmetricCryptoProvider symmetricProvider = GetSymmetricCryptoProvider(symmetricInstance);

            return Cryptographer.DecryptSymmetric(symmetricProvider, ciphertextBase64);
        }

        private IHashProvider GetHashProvider(string hashInstance)
        {
            if (string.IsNullOrEmpty(hashInstance)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "hashInstance");

            IHashProvider hashProvider;
            if (!this.hashProviders.TryGetValue(hashInstance, out hashProvider))
            {
                string message = string.Format(Resources.HashProviderInstanceNotFound, hashInstance);
                this.instrumentationProvider.FireCryptographyErrorOccurred(HashProvider, hashInstance, message);
                throw new ConfigurationErrorsException(message);
            }

            return hashProvider;
        }

        private ISymmetricCryptoProvider GetSymmetricCryptoProvider(string symmetricInstance)
        {
            if (string.IsNullOrEmpty(symmetricInstance)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "symmetricInstance");

            ISymmetricCryptoProvider symmetricProvider;
            if (!this.symmetricCryptoProviders.TryGetValue(symmetricInstance, out symmetricProvider))
            {
                string message = string.Format(Resources.SymmetricCrytoProviderInstanceNotFound, symmetricInstance);
                this.instrumentationProvider.FireCryptographyErrorOccurred(SymmetricCryptoProvider, symmetricInstance, message);
                throw new ConfigurationErrorsException(message);
            }

            return symmetricProvider;
        }
    }
}
