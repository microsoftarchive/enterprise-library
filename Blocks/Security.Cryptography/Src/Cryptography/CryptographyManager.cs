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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Non-static entry point to the cryptography functionality.
    /// </summary>
    public abstract class CryptographyManager
    {
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
        public abstract bool CompareHash(string hashInstance, string plaintext, string hashedText);

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
        public abstract bool CompareHash(string hashInstance, byte[] plaintext, byte[] hashedText);

        /// <overrides>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </overrides>
        /// <summary>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </summary>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        public abstract byte[] CreateHash(string hashInstance, byte[] plaintext);

        /// <summary>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </summary>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        public abstract string CreateHash(string hashInstance, string plaintext);

        /// <summary>
        /// Decrypts a cipher text using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="ciphertext">The cipher text for which you want to decrypt.</param>
        /// <returns>The resulting plain text.</returns>
        public abstract byte[] DecryptSymmetric(string symmetricInstance, byte[] ciphertext);

        /// <summary>
        /// Decrypts a cipher text using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="ciphertextBase64">The cipher text as a base64 encoded string for which you want to decrypt.</param>
        /// <returns>The resulting plain text as a string.</returns>
        public abstract string DecryptSymmetric(string symmetricInstance, string ciphertextBase64);


        /// <summary>
        /// Encrypts a secret using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="plaintext">The input for which you want to encrypt.</param>
        /// <returns>The resulting cipher text.</returns>
        public abstract byte[] EncryptSymmetric(string symmetricInstance, byte[] plaintext);

        /// <summary>
        /// Encrypts a secret using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="plaintext">The input as a base64 encoded string for which you want to encrypt.</param>
        /// <returns>The resulting cipher text as a base64 encoded string.</returns>
        public abstract string EncryptSymmetric(string symmetricInstance, string plaintext);
    }
}
