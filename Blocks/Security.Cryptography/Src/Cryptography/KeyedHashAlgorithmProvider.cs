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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// A hash provider for any hash algorithm which derives from <see cref="System.Security.Cryptography.KeyedHashAlgorithm"/>.
    /// </summary>
    [ConfigurationElementType(typeof(KeyedHashAlgorithmProviderData))]
    public class KeyedHashAlgorithmProvider : HashAlgorithmProvider
    {
        ProtectedKey key;

        /// <summary>
        /// Initialize a new instance of the <see cref="KeyedHashAlgorithmProvider"/> class with a <see cref="KeyedHashAlgorithm"/>, if salt is enabled, and the key to use.
        /// </summary>
        /// <param name="algorithmType">
        /// The <see cref="KeyedHashAlgorithm"/> to use.
        /// </param>
        /// <param name="saltEnabled"><see langword="true"/> if salt should be used; otherwise, <see langword="false"/>.</param>
        /// <param name="protectedKeyFileName">File name of DPAPI-protected key used to encrypt and decrypt secrets through this provider.</param>
        /// <param name="protectedKeyProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        public KeyedHashAlgorithmProvider(Type algorithmType,
                                          bool saltEnabled,
                                          string protectedKeyFileName,
                                          DataProtectionScope protectedKeyProtectionScope)
            : this(algorithmType, saltEnabled, protectedKeyFileName, protectedKeyProtectionScope, new NullHashAlgorithmInstrumentationProvider()) { }

        /// <summary>
        /// Initialize a new instance of the <see cref="KeyedHashAlgorithmProvider"/> class with a <see cref="KeyedHashAlgorithm"/>, if salt is enabled, and the key to use.
        /// </summary>
        /// <param name="algorithmType">
        /// The <see cref="KeyedHashAlgorithm"/> to use.
        /// </param>
        /// <param name="saltEnabled"><see langword="true"/> if salt should be used; otherwise, <see langword="false"/>.</param>
        /// <param name="protectedKey">The <see cref="ProtectedKey"/> for the provider.</param>
        public KeyedHashAlgorithmProvider(Type algorithmType,
                                          bool saltEnabled,
                                          ProtectedKey protectedKey)
            : this(algorithmType, saltEnabled, protectedKey, new NullHashAlgorithmInstrumentationProvider())
        {
        }


        /// <summary>
        /// Initialize a new instance of the <see cref="KeyedHashAlgorithmProvider"/> class with a <see cref="KeyedHashAlgorithm"/>, if salt is enabled, and the key to use.
        /// </summary>
        /// <param name="algorithmType">
        /// The <see cref="KeyedHashAlgorithm"/> to use.
        /// </param>
        /// <param name="saltEnabled"><see langword="true"/> if salt should be used; otherwise, <see langword="false"/>.</param>
        /// <param name="protectedKeyFileName">File name of DPAPI-protected key used to encrypt and decrypt secrets through this provider.</param>
		/// <param name="protectedKeyProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        /// <param name="instrumentationProvider">The <see cref="IHashAlgorithmInstrumentationProvider"/> to use.</param>
        public KeyedHashAlgorithmProvider(Type algorithmType,
                                          bool saltEnabled,
                                          string protectedKeyFileName,
                                          DataProtectionScope protectedKeyProtectionScope,
                                          IHashAlgorithmInstrumentationProvider instrumentationProvider)
            : this(algorithmType, saltEnabled, KeyManager.Read(protectedKeyFileName, protectedKeyProtectionScope), instrumentationProvider) { }

        /// <summary>
        /// Initialize a new instance of the <see cref="KeyedHashAlgorithmProvider"/> class with a <see cref="KeyedHashAlgorithm"/>, if salt is enabled, and the key to use.
        /// </summary>
        /// <param name="algorithmType">
        /// The <see cref="KeyedHashAlgorithm"/> to use.
        /// </param>
        /// <param name="saltEnabled"><see langword="true"/> if salt should be used; otherwise, <see langword="false"/>.</param>
        /// <param name="protectedKey">The <see cref="ProtectedKey"/> for the provider.</param>
        /// <param name="instrumentationProvider">The <see cref="IHashAlgorithmInstrumentationProvider"/> to use.</param>
        public KeyedHashAlgorithmProvider(Type algorithmType,
                                          bool saltEnabled,
                                          ProtectedKey protectedKey,
                                          IHashAlgorithmInstrumentationProvider instrumentationProvider)
            : base(algorithmType, saltEnabled, instrumentationProvider)
        {
            if (protectedKey == null) 
                throw new ArgumentNullException("protectedKey");
            if (!typeof(KeyedHashAlgorithm).IsAssignableFrom(algorithmType)) 
                throw new ArgumentException(Resources.ExceptionMustBeAKeyedHashAlgorithm, "algorithmType");

            key = protectedKey;
        }

        /// <summary>
        /// Gets the cryptographer used for hashing.
        /// </summary>
        /// <returns>The cryptographer initialized with the configured key.</returns>
        protected override HashCryptographer HashCryptographer
        {
            get { return new HashCryptographer(AlgorithmType, key); }
        }
    }
}
