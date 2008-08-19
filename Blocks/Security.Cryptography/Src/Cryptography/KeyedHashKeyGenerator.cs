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
using System.Text;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// Provides several methods for generating a keyed hash algorithm key.
	/// </summary>
	public class KeyedHashKeyGenerator : IKeyGenerator
	{
		/// <overloads>
		/// Generates a keyed hash key.
		/// </overloads>
		/// <summary>
		/// Generates a keyed hash key based on an algorithm name.
		/// </summary>
		/// <param name="keyedHashAlgorithmName">Keyed hash algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated key.</param>
		/// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		public ProtectedKey GenerateKey(string keyedHashAlgorithmName, DataProtectionScope dataProtectionScope)
		{
			return GenerateKey(CreateAlgorithm(keyedHashAlgorithmName), dataProtectionScope);
		}

		/// <summary>
		/// Generates a keyed hash key based on an algorithm name.
		/// </summary>
		/// <param name="keyedHashAlgorithmName">Keyed hash algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
		/// <returns>Unencrypted key generated for specified keyed hash algorithm. The caller of this method
		/// is responsible for overwriting the contents of this byte array.</returns>
		public byte [] GenerateKey(string keyedHashAlgorithmName)
		{
			using (KeyedHashAlgorithm algorithm = CreateAlgorithm(keyedHashAlgorithmName))
			{
				return algorithm.Key;
			}
		}

		/// <summary>
		/// Generates a keyed hash key based on an algorithm type.
		/// </summary>
		/// <param name="keyedHashAlgorithmType">Keyed hash algorithm type.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated key.</param>
		/// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		public ProtectedKey GenerateKey(Type keyedHashAlgorithmType, DataProtectionScope dataProtectionScope)
		{
			return GenerateKey(CreateAlgorithm(keyedHashAlgorithmType), dataProtectionScope);
		}

		/// <summary>
		/// Generates a keyed hash key based on an algorithm type.
		/// </summary>
		/// <param name="keyedHashAlgorithmType">Keyed hash algorithm type.</param>
		/// <returns>Unencrypted key generated for specified keyed hash algorithm. The caller of this method
		/// is responsible for overwriting the contents of this byte array.</returns>
		public byte[] GenerateKey(Type keyedHashAlgorithmType)
		{
			using (KeyedHashAlgorithm algorithm = CreateAlgorithm(keyedHashAlgorithmType))
			{
				return algorithm.Key;
			}
		}

		private ProtectedKey GenerateKey(KeyedHashAlgorithm algorithm, DataProtectionScope dataProtectionScope)
		{
			using (algorithm)
			{
				return ProtectedKey.CreateFromPlaintextKey(algorithm.Key, dataProtectionScope);
			}
		}

		private KeyedHashAlgorithm CreateAlgorithm(string keyedHashAlgorithmName)
		{
			KeyedHashAlgorithm algorithm = (KeyedHashAlgorithm)CryptoConfig.CreateFromName(keyedHashAlgorithmName);
			if (algorithm == null)
			{
				string message = String.Format(Resources.UnknownKeyedHashAlgorithmCreatedError, keyedHashAlgorithmName);
				throw new ArgumentException(message);
			}
			return algorithm;
		}

		private KeyedHashAlgorithm CreateAlgorithm(Type keyedHashAlgorithmType)
		{
			KeyedHashAlgorithm algorithm = null;
			try
			{
				algorithm = (KeyedHashAlgorithm)Activator.CreateInstance(keyedHashAlgorithmType);
			}
			catch (Exception)
			{
				throw new ArgumentException(Resources.UnknownKeyedHashAlgorithmCreatedError, keyedHashAlgorithmType.Name);
			}
			return algorithm;
		}
	}
}
