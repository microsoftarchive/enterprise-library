//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography
{
	/// <summary>
	/// Implementation of Symmetric Storage Encryption used by the Caching block
	/// </summary>
	[ConfigurationElementType(typeof(SymmetricStorageEncryptionProviderData))]
	public class SymmetricStorageEncryptionProvider : IStorageEncryptionProvider
	{
		private ISymmetricCryptoProvider symmetricCrytoProvider;

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.</para>
		/// </summary>
		public SymmetricStorageEncryptionProvider(ISymmetricCryptoProvider symmetricCrytoProvider)
		{
			this.symmetricCrytoProvider = symmetricCrytoProvider;
		}

		/// <summary>
		/// Encrypts the data passed to this method according to the correct symmetric cryptographic
		/// algorithm as defined in configuration
		/// </summary>
		/// <param name="plaintext">Data to be encrypted</param>
		/// <returns>Encrypted data</returns>
		public byte[] Encrypt(byte[] plaintext)
		{
			return symmetricCrytoProvider.Encrypt(plaintext);
		}

		/// <summary>
		/// Decrypts the data passed to this method according to the correct symmetric cryptographic
		/// algoritm as defined in configuration
		/// </summary>
		/// <param name="ciphertext">Encrypted data to be decrypted</param>
		/// <returns>Decrypted data</returns>
		public byte[] Decrypt(byte[] ciphertext)
		{
			return symmetricCrytoProvider.Decrypt(ciphertext);
		}
	}
}