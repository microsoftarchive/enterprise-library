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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// <para>Represents basic cryptography services for a <see cref="SymmetricAlgorithm"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Because the IV (Initialization Vector) has the same distribution as the resulting 
    /// ciphertext, the IV is randomly generated and prepended to the ciphertext.
    /// </para>
    /// </remarks>
    public class SymmetricCryptographer : IDisposable
    {
        private SymmetricAlgorithm algorithm;
        private ProtectedKey key;

		private byte[] Key
		{
			get
			{
				return key.DecryptedKey;
			}
		}

		/// <summary>
		/// <para>Initalize a new instance of the <see cref="SymmetricCryptographer"/> class with an algorithm type and a key.</para>
		/// </summary>
		/// <param name="algorithmType"><para>The qualified assembly name of a <see cref="SymmetricAlgorithm"/>.</para></param>
		/// <param name="key"><para>The key for the algorithm.</para></param>
		public SymmetricCryptographer(Type algorithmType, ProtectedKey key)
		{
			if (algorithmType == null) throw new ArgumentNullException("algorithmType");
			if (!typeof(SymmetricAlgorithm).IsAssignableFrom(algorithmType)) throw new ArgumentException(Resources.ExceptionCreatingSymmetricAlgorithmInstance, "algorithmType");
			if (key == null) throw new ArgumentNullException("key");

			this.key = key;
			this.algorithm = GetSymmetricAlgorithm(algorithmType);
		}

		/// <summary>
		/// Finalizer for <see cref="SymmetricCryptographer"/>
		/// </summary>
		~SymmetricCryptographer()
		{
			Dispose(false);
		}

		/// <summary>
		/// Releases all resources for this instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			System.GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Override to customize resources to be disposed.
		/// </summary>
		/// <param name="disposing">Unused.</param>
		protected virtual void Dispose(bool disposing)
		{
			if(algorithm != null)
			{
				algorithm.Clear();
				algorithm = null;
			}
		}

		/// <summary>
        /// <para>Encrypts bytes with the initialized algorithm and key.</para>
        /// </summary>
        /// <param name="plaintext"><para>The plaintext in which you wish to encrypt.</para></param>
        /// <returns><para>The resulting ciphertext.</para></returns>
		public byte[] Encrypt(byte[] plaintext)
		{
			byte[] output = null;
			byte[] cipherText = null;

			this.algorithm.Key = Key;

			using (ICryptoTransform transform = this.algorithm.CreateEncryptor())
			{
				cipherText = Transform(transform, plaintext);
			}

			output = new byte[IVLength + cipherText.Length];
			Buffer.BlockCopy(this.algorithm.IV, 0, output, 0, IVLength);
			Buffer.BlockCopy(cipherText, 0, output, IVLength, cipherText.Length);

			CryptographyUtility.ZeroOutBytes(this.algorithm.Key);

			return output;
		}

        /// <summary>
        /// <para>Decrypts bytes with the initialized algorithm and key.</para>
        /// </summary>
        /// <param name="encryptedText"><para>The text which you wish to decrypt.</para></param>
        /// <returns><para>The resulting plaintext.</para></returns>
        public byte[] Decrypt(byte[] encryptedText)
        {
            byte[] output = null;
            byte[] data = ExtractIV(encryptedText);

			this.algorithm.Key = Key;
			
			using (ICryptoTransform transform = this.algorithm.CreateDecryptor())
            {
                output = Transform(transform, data);
            }

			CryptographyUtility.ZeroOutBytes(this.algorithm.Key);

            return output;
        }

        private static byte[] Transform(ICryptoTransform transform, byte[] buffer)
        {
            return CryptographyUtility.Transform(transform, buffer);
        }

        private int IVLength
        {
            get
            {
                if (this.algorithm.IV == null)
                {
                    this.algorithm.GenerateIV();
                }
                return this.algorithm.IV.Length;
            }
        }

		private byte[] ExtractIV(byte[] encryptedText)
		{
			byte[] initVector = new byte[IVLength];

			if (encryptedText.Length < IVLength + 1)
			{
				throw new CryptographicException(Resources.ExceptionDecrypting);
			}

			byte[] data = new byte[encryptedText.Length - IVLength];

			Buffer.BlockCopy(encryptedText, 0, initVector, 0, IVLength);
			Buffer.BlockCopy(encryptedText, IVLength, data, 0, data.Length);

			this.algorithm.IV = initVector;

			return data;
		}

        private static SymmetricAlgorithm GetSymmetricAlgorithm(Type algorithmType)
        {
            SymmetricAlgorithm symmetricAlgorithm = Activator.CreateInstance(algorithmType) as SymmetricAlgorithm;                      
            return symmetricAlgorithm;
        }
    }
}