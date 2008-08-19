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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// <para>A contract for any provider for configurable symmetric cryptographic implementations.</para>
    /// </summary>
	[ConfigurationNameMapper(typeof(SymmetricCryptoProviderNameMapper))]
	[CustomFactory(typeof(SymmetricCryptoProviderCustomFactory))]
	public interface ISymmetricCryptoProvider 
    {
        /// <summary>
        /// <para>Encrypts a secret using a specified symmetric cryptography provider.</para>
        /// </summary>
        /// <param name="plaintext"><para>The input for which you want to encrypt.</para></param>
        /// <returns><para>The resulting cipher text.</para></returns>
        byte[] Encrypt(byte[] plaintext);

        /// <summary>
        /// <para>Decrypts a cipher text using a specified symmetric cryptography provider.</para>
        /// </summary>
        /// <param name="ciphertext"><para>The cipher text for which you want to decrypt.</para></param>
        /// <returns><para>The resulting plain text.</para></returns>
        byte[] Decrypt(byte[] ciphertext);
    }
}