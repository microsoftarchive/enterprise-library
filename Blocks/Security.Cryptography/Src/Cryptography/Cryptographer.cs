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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// Facade which exposes common cryptography uses.
	/// </summary>
	public static class Cryptographer
	{
		/// <overrides>
		/// Computes the hash value of plain text using the given hash provider instance
		/// </overrides>
		/// <summary>
		/// Computes the hash value of plain text using the given hash provider instance
		/// </summary>
		/// <param name="hashInstance">A hash instance from configuration.</param>
		/// <param name="plaintext">The input for which to compute the hash.</param>
		/// <returns>The computed hash code.</returns>
        public static byte[] CreateHash(string hashInstance, byte[] plaintext)
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
        public static string CreateHash(string hashInstance, string plaintext)
        {
            IHashProvider hashProvider = GetHashProvider(hashInstance);
            return CreateHash(hashProvider, plaintext);
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
        public static bool CompareHash(string hashInstance, byte[] plaintext, byte[] hashedText)
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
        public static bool CompareHash(string hashInstance, string plaintext, string hashedText)
        {
            IHashProvider hashProvider = GetHashProvider(hashInstance);
            return CompareHash(hashProvider, plaintext, hashedText);
        }

		/// <summary>
		/// Encrypts a secret using a specified symmetric cryptography provider.
		/// </summary>
		/// <param name="symmetricInstance">A symmetric instance from configuration.</param>
		/// <param name="plaintext">The input for which you want to encrypt.</param>
		/// <returns>The resulting cipher text.</returns>
        public static byte[] EncryptSymmetric(string symmetricInstance, byte[] plaintext)
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
        public static string EncryptSymmetric(string symmetricInstance, string plaintext)
        {
            ISymmetricCryptoProvider provider = GetSymmetricCryptoProvider(symmetricInstance);
            return EncryptSymmetric(provider, plaintext);
        }

		/// <summary>
		/// Decrypts a cipher text using a specified symmetric cryptography provider.
		/// </summary>
		/// <param name="symmetricInstance">A symmetric instance from configuration.</param>
		/// <param name="ciphertext">The cipher text for which you want to decrypt.</param>
		/// <returns>The resulting plain text.</returns>
        public static byte[] DecryptSymmetric(string symmetricInstance, byte[] ciphertext)
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
        public static string DecryptSymmetric(string symmetricInstance, string ciphertextBase64)
        {
            ISymmetricCryptoProvider provider = GetSymmetricCryptoProvider(symmetricInstance);
            return DecryptSymmetric(provider, ciphertextBase64);
        }

		private static void TryLogHashConfigurationError(Exception configurationException, string hashInstance)
		{
			TryLogConfigurationError(configurationException, Resources.ErrorHashProviderConfigurationFailedMessage, hashInstance);
		}

		private static void TryLogSymmetricConfigurationError(Exception configurationException, string symmetricInstance)
		{
			TryLogConfigurationError(configurationException, Resources.ErrorSymmetricEncryptionConfigurationFailedMessage, symmetricInstance);
		}

		private static void TryLogConfigurationError(Exception configurationException, string hashInstance, string template)
		{
			try
			{
                var eventLogger = EnterpriseLibraryContainer.Current.GetInstance<IDefaultCryptographyInstrumentationProvider>();
				if (eventLogger != null)
				{
					eventLogger.LogConfigurationError(hashInstance, template, configurationException);
				}
			}
			catch { }
		}
        /// <summary>
        /// Creates an instance of a <see cref="ISymmetricCryptoProvider"/>.
        /// </summary>
        /// <param name="symmetricInstance">The symmetric crypto instance.</param>
        /// <returns>The <see cref="ISymmetricCryptoProvider"/>.</returns>
        private static ISymmetricCryptoProvider GetSymmetricCryptoProvider(string symmetricInstance)
        {
            if (string.IsNullOrEmpty(symmetricInstance)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "symmetricInstance");

            try
            {
                return EnterpriseLibraryContainer.Current.GetInstance<ISymmetricCryptoProvider>(symmetricInstance);
            }
            catch (Exception configurationException)
            {
                TryLogSymmetricConfigurationError(configurationException, symmetricInstance);
                throw;
            }
        }

        /// <summary>
        /// Creates an instance of a <see cref="IHashProvider"/>.
        /// </summary>
        /// <param name="hashInstance">The hash instance.</param>
        /// <returns>The <see cref="IHashProvider"/>.</returns>
        private static IHashProvider GetHashProvider(string hashInstance)
        {
            if (string.IsNullOrEmpty(hashInstance))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, hashInstance);

            try
            {
                return EnterpriseLibraryContainer.Current.GetInstance<IHashProvider>(hashInstance);
            }
            catch (Exception configurationException)
            {
                TryLogHashConfigurationError(configurationException, hashInstance);
                throw;
            }
        }

        internal static string CreateHash(IHashProvider provider, string plaintext)
        {
            byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
            byte[] resultBytes = provider.CreateHash(plainTextBytes);
            CryptographyUtility.GetRandomBytes(plainTextBytes);
            return Convert.ToBase64String(resultBytes);
        }

        internal static bool CompareHash(IHashProvider provider, string plaintext, string hashedText)
        {
            byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
            byte[] hashedTextBytes = Convert.FromBase64String(hashedText);

            bool result = provider.CompareHash(plainTextBytes, hashedTextBytes);
            CryptographyUtility.GetRandomBytes(plainTextBytes);

            return result;
        }

        internal static string DecryptSymmetric(ISymmetricCryptoProvider provider, string ciphertextBase64)
        {
            if (string.IsNullOrEmpty(ciphertextBase64)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "ciphertextBase64");

            byte[] cipherTextBytes = Convert.FromBase64String(ciphertextBase64);
            byte[] decryptedBytes = provider.Decrypt(cipherTextBytes);
            string decryptedString = UnicodeEncoding.Unicode.GetString(decryptedBytes);
            CryptographyUtility.GetRandomBytes(decryptedBytes);

            return decryptedString;
        }

        internal static string EncryptSymmetric(ISymmetricCryptoProvider provider, string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "plaintext");

            byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
            byte[] cipherTextBytes = provider.Encrypt(plainTextBytes);
            CryptographyUtility.GetRandomBytes(plainTextBytes);
            return Convert.ToBase64String(cipherTextBytes);
        }
	}
}
