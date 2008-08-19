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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
    /// <summary>
    /// Not intended for direct use.  Provides symmetric encryption and decryption services 
    /// to Isolated and Database backing stores.  Allows this block to use 
    /// Security.Cryptography without having a direct reference to that assembly.
    /// </summary>
	[CustomFactory(typeof(StorageEncryptionProviderCustomFactory))]
    public interface IStorageEncryptionProvider 
    {
        /// <summary>
        /// Encrypt backing store data.
        /// </summary>
        /// <param name="plaintext">Clear bytes.</param>
        /// <returns>Encrypted bytes.</returns>
        byte[] Encrypt(byte[] plaintext);

        /// <summary>
        /// Decrypt backing store data.
        /// </summary>
        /// <param name="ciphertext">Encrypted bytes.</param>
        /// <returns>Decrypted bytes.</returns>
        byte[] Decrypt(byte[] ciphertext);
    }
}