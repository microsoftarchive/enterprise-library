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
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// A hash provider for any hash algorithm which derives from <see cref="System.Security.Cryptography.HashAlgorithm"/>.
	/// </summary>
	[ConfigurationElementType(typeof(HashAlgorithmProviderData))]
	public class HashAlgorithmProvider : IHashProvider, IInstrumentationEventProvider
	{
		/// <summary>
		/// Defines the salt length used by the provider.
		/// </summary>
		public const int SaltLength = 16;

		private Type algorithmType;
		private bool saltEnabled;
		HashAlgorithmInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Initialize a new instance of the <see cref="HashAlgorithmProvider"/> class with the <see cref="HashAlgorithm"/> type and if salt is enabled.
		/// </summary>
		/// <param name="algorithmType">The <see cref="HashAlgorithm"/> to use.</param>
		/// <param name="saltEnabled"><see langword="true"/> if salt should be applied; otherwise, <see langword="false"/>.</param>
		public HashAlgorithmProvider(Type algorithmType, bool saltEnabled)
		{
			if (algorithmType == null) throw new ArgumentNullException("algorithmType");
			if (!typeof(HashAlgorithm).IsAssignableFrom(algorithmType)) throw new ArgumentException(Resources.ExceptionMustBeAHashAlgorithm, "algorithmType");

			this.algorithmType = algorithmType;
			this.saltEnabled = saltEnabled;

			this.instrumentationProvider = new HashAlgorithmInstrumentationProvider();
		}

		/// <summary>
		/// Computes the hash value of plain text.
		/// </summary>
		/// <param name="plaintext">The input for which to compute the hash.</param>
		/// <returns>The computed hash code.</returns>
		public byte[] CreateHash(byte[] plaintext)
		{
			byte[] hash = null;

			try
			{
				hash = CreateHashWithSalt(plaintext, null);
			}
			catch (Exception e)
			{
				InstrumentationProvider.FireCyptographicOperationFailed(Resources.HashCreationFailed, e);
				throw;
			}

			InstrumentationProvider.FireHashOperationPerformed();
			return hash;
		}

		/// <summary>
		/// Compares plain text input with a computed hash.
		/// </summary>
		/// <param name="plaintext">The input for which you want to compare the hash to.</param>
		/// <param name="hashedtext">The hash value for which you want to compare the input to.</param>
		/// <returns><c>true</c> if plainText hashed is equal to the hashedText. Otherwise, <c>false</c>.</returns>
		public bool CompareHash(byte[] plaintext, byte[] hashedtext)
		{
			if (plaintext == null) throw new ArgumentNullException("plainText");
			if (hashedtext == null) throw new ArgumentNullException("hashedText");
			if (hashedtext.Length == 0) throw new ArgumentException(Resources.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, "hashedText");

			bool result = false;
			byte[] hashedPlainText = null;
			byte[] salt = null;
			try
			{
				try
				{
					salt = ExtractSalt(hashedtext);
					hashedPlainText = CreateHashWithSalt(plaintext, salt);
				}
				finally
				{
					CryptographyUtility.ZeroOutBytes(salt);
				}
				result = CryptographyUtility.CompareBytes(hashedPlainText, hashedtext);
			}
			catch (Exception e)
			{
				InstrumentationProvider.FireCyptographicOperationFailed(Resources.HashComparisonFailed, e);
				throw;
			}

			InstrumentationProvider.FireHashComparisonPerformed();
			if (!result)
			{
				InstrumentationProvider.FireHashMismatchDetected();
			}
			return result;
		}

		/// <summary>
		/// Gets the <see cref="HashAlgorithmInstrumentationProvider"/> instance that defines the logical events 
		/// used to instrument this hash provider.
		/// </summary>
		/// <returns>The <see cref="HashAlgorithmInstrumentationProvider"/> instance that defines the logical 
		/// events used to instrument this hash provider.</returns>
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}

		/// <summary>
		/// Creates a hash with a specified salt.
		/// </summary>
		/// <param name="plaintext">The plaintext to hash.</param>
		/// <param name="salt">The hash salt.</param>
		/// <returns>The computed hash.</returns>
		protected virtual byte[] CreateHashWithSalt(byte[] plaintext, byte[] salt)
		{
			AddSaltToPlainText(ref salt, ref plaintext);
			HashCryptographer cryptographer = this.HashCryptographer;
			byte[] hash = cryptographer.ComputeHash(plaintext);
			AddSaltToHash(salt, ref hash);
			return hash;
		}

		/// <summary>
		/// Extracts the salt from the hashedText.
		/// </summary>
		/// <param name="hashedtext">The hash in which to extract the salt.</param>
		/// <returns>The extracted salt.</returns>
		protected byte[] ExtractSalt(byte[] hashedtext)
		{
			if (!saltEnabled)
			{
				return null;
			}

			byte[] salt = null;
			if (hashedtext.Length > SaltLength)
			{
				salt = new byte[SaltLength];
				Buffer.BlockCopy(hashedtext, 0, salt, 0, SaltLength);
			}
			return salt;
		}

		/// <summary>
		/// Gets the <see cref="HashAlgorithm"/> type.
		/// </summary>
		/// <value>
		/// The <see cref="HashAlgorithm"/> type.
		/// </value>
		protected Type AlgorithmType
		{
			get { return algorithmType; }
		}

		/// <summary>
		/// Gets the cryptographer used for hashing.
		/// </summary>
		/// <returns>
		/// A <see cref="HashCryptographer"/> object.
		/// </returns>
		protected virtual HashCryptographer HashCryptographer
		{
			get { return new HashCryptographer(algorithmType); }
		}

		/// <summary>
		/// Gets the <see cref="HashAlgorithmInstrumentationProvider"/> instance that defines the logical events 
		/// used to instrument this hash provider.
		/// </summary>
		protected HashAlgorithmInstrumentationProvider InstrumentationProvider
		{
			get { return instrumentationProvider; }
		}

		private void AddSaltToHash(byte[] salt, ref byte[] hash)
		{
			if (!saltEnabled)
			{
				return;
			}
			hash = CryptographyUtility.CombineBytes(salt, hash);
		}

		private void AddSaltToPlainText(ref byte[] salt, ref byte[] plaintext)
		{
			if (!saltEnabled)
			{
				return;
			}

			if (salt == null)
			{
				salt = CryptographyUtility.GetRandomBytes(SaltLength);
			}

			plaintext = CryptographyUtility.CombineBytes(salt, plaintext);
		}
	}
}
