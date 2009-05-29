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
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// <para>A symmetric provider for any symmetric algorithm which derives from <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</para>
	/// </summary>
	[ConfigurationElementType(typeof(SymmetricAlgorithmProviderData))]
	public class SymmetricAlgorithmProvider : ISymmetricCryptoProvider
	{
		Type algorithmType;

		ISymmetricAlgorithmInstrumentationProvider instrumentationProvider;
		ProtectedKey key;


        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.
        /// </summary>
        /// <param name="algorithmType">The symmetric algorithm type.</param>
        /// <param name="protectedKeyFileName">Input file from which DPAPI-protected key is to be read.</param>
        /// <param name="protectedKeyProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        public SymmetricAlgorithmProvider(Type algorithmType,
                                          string protectedKeyFileName,
                                          DataProtectionScope protectedKeyProtectionScope)
            : this(algorithmType, protectedKeyFileName, protectedKeyProtectionScope, new NullSymmetricAlgorithmInstrumentationProvider()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.
        /// </summary>
        /// <param name="algorithmType">The cryptographic algorithm type.</param>
        /// <param name="protectedKeyStream">Input <see cref="Stream"/> from which DPAPI-protected key is to be read.</param>
        /// <param name="protectedKeyProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        public SymmetricAlgorithmProvider(Type algorithmType,
                                          Stream protectedKeyStream,
                                          DataProtectionScope protectedKeyProtectionScope)
            : this(algorithmType, protectedKeyStream, protectedKeyProtectionScope, new NullSymmetricAlgorithmInstrumentationProvider()) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.
        /// </summary>
        /// <param name="algorithmType">The symmetric algorithm type.</param>
        /// <param name="key">The <see cref="ProtectedKey"/> for the provider.</param>
        public SymmetricAlgorithmProvider(Type algorithmType,
                                          ProtectedKey key)
            :this(algorithmType, key, new NullSymmetricAlgorithmInstrumentationProvider())
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.
		/// </summary>
		/// <param name="algorithmType">The symmetric algorithm type.</param>
		/// <param name="protectedKeyFileName">Input file from which DPAPI-protected key is to be read.</param>
		/// <param name="protectedKeyProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        /// <param name="instrumentationProvider">The <see cref="ISymmetricAlgorithmInstrumentationProvider"/> to use.</param>
		public SymmetricAlgorithmProvider(Type algorithmType,
										  string protectedKeyFileName,
										  DataProtectionScope protectedKeyProtectionScope, 
                                          ISymmetricAlgorithmInstrumentationProvider instrumentationProvider)
            : this(algorithmType, KeyManager.Read(protectedKeyFileName, protectedKeyProtectionScope), instrumentationProvider) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.
		/// </summary>
		/// <param name="algorithmType">The cryptographic algorithm type.</param>
		/// <param name="protectedKeyStream">Input <see cref="Stream"/> from which DPAPI-protected key is to be read.</param>
		/// <param name="protectedKeyProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        /// <param name="instrumentationProvider">The <see cref="ISymmetricAlgorithmInstrumentationProvider"/> to use.</param>
        public SymmetricAlgorithmProvider(Type algorithmType,
										  Stream protectedKeyStream,
                                          DataProtectionScope protectedKeyProtectionScope,
                                          ISymmetricAlgorithmInstrumentationProvider instrumentationProvider)
			: this(algorithmType, KeyManager.Read(protectedKeyStream, protectedKeyProtectionScope), instrumentationProvider) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.
		/// </summary>
		/// <param name="algorithmType">The symmetric algorithm type.</param>
		/// <param name="key">The <see cref="ProtectedKey"/> for the provider.</param>
        /// <param name="instrumentationProvider">The <see cref="ISymmetricAlgorithmInstrumentationProvider"/> to use.</param>
		public SymmetricAlgorithmProvider(Type algorithmType,
                                          ProtectedKey key,
                                          ISymmetricAlgorithmInstrumentationProvider instrumentationProvider)
		{
            if (instrumentationProvider == null) throw new ArgumentNullException("instrumentationProvider");
			if (algorithmType == null) throw new ArgumentNullException("algorithmType");
			if (!typeof(SymmetricAlgorithm).IsAssignableFrom(algorithmType)) throw new ArgumentException(Resources.ExceptionCreatingSymmetricAlgorithmInstance, "algorithmType");

			this.algorithmType = algorithmType;

			this.key = key;

            this.instrumentationProvider = instrumentationProvider;
		}


        /// <summary>
        /// Gets the <see cref="ISymmetricAlgorithmInstrumentationProvider"/> instance that defines the logical events 
        /// used to instrument this Symmetric Algorithm Provider.
        /// </summary>
        protected ISymmetricAlgorithmInstrumentationProvider InstrumentationProvider
        {
            get { return instrumentationProvider; }
        }


		/// <summary>
		/// Decrypts a secret using the configured <c>SymmetricAlgorithm</c>.
		/// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
		/// </summary>
		/// <param name="ciphertext"><para>The cipher text to be decrypted.</para></param>
		/// <returns><para>The resulting plain text. It is the responsibility of the caller to clear the returned byte array
		/// when finished.</para></returns>
		/// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
		public byte[] Decrypt(byte[] ciphertext)
		{
			if (ciphertext == null) throw new ArgumentNullException("ciphertext");
			if (ciphertext.Length == 0) throw new ArgumentException(Resources.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, "ciphertext");

			byte[] output = null;

			try
			{
				using (SymmetricCryptographer crypto = new SymmetricCryptographer(algorithmType, key))
				{
					output = crypto.Decrypt(ciphertext);
				}
			}
			catch (Exception e)
			{
                instrumentationProvider.FireCyptographicOperationFailed(Resources.DecryptionFailed, e);
				throw;
			}
            instrumentationProvider.FireSymmetricDecryptionPerformed();

			return output;
		}

		/// <summary>
		/// <para>Encrypts a secret using the configured <c>SymmetricAlgorithm</c>.</para>
		/// </summary>
		/// <param name="plaintext"><para>The input to be encrypted. It is the responsibility of the caller to clear this
		/// byte array when finished.</para></param>
		/// <returns><para>The resulting cipher text.</para></returns>
		/// <seealso cref="ISymmetricCryptoProvider.Encrypt"/>
		public byte[] Encrypt(byte[] plaintext)
		{
			if (plaintext == null) throw new ArgumentNullException("plainText");
			if (plaintext.Length == 0) throw new ArgumentException(Resources.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, "plaintext");

			byte[] output = null;

			try
			{
				using (SymmetricCryptographer crypto = new SymmetricCryptographer(algorithmType, key))
				{
					output = crypto.Encrypt(plaintext);
				}
			}
			catch (Exception e)
			{
                instrumentationProvider.FireCyptographicOperationFailed(Resources.EncryptionFailed, e);
				throw;
			}
            instrumentationProvider.FireSymmetricEncryptionPerformed();

			return output;
		}
	}
}
