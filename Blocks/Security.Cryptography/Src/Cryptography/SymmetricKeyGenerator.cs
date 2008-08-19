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
	/// 
	/// </summary>
	public class SymmetricKeyGenerator : IKeyGenerator
	{
		/// <overloads>
		/// Generates a symmetric key.
		/// </overloads>
		/// <summary>
		/// Generates a symmetric key based on an algorithm name.
		/// </summary>
		/// <param name="symmetricAlgorithmName">Symmetric algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated symmetric key.</param>
		/// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		public ProtectedKey GenerateKey(string symmetricAlgorithmName, DataProtectionScope dataProtectionScope)
		{
			SymmetricAlgorithm algorithm = CreateAlgorithm(symmetricAlgorithmName);
			return GenerateKey(algorithm, dataProtectionScope);
		}

		/// <summary>
		/// Generates a symmetric key based on an algorithm type.
		/// </summary>
		/// <param name="symmetricAlgorithmType">Symmetric algorithm type.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated symmetric key.</param>
		/// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		public ProtectedKey GenerateKey(Type symmetricAlgorithmType, DataProtectionScope dataProtectionScope)
		{
			SymmetricAlgorithm algorithm = CreateAlgorithm(symmetricAlgorithmType);
			return GenerateKey(algorithm, dataProtectionScope);
		}

		/// <summary>
		/// Generates a symmetric key based on an algorithm name.
		/// </summary>
		/// <param name="symmetricAlgorithmName">Name of a symmetric algorithm as defined in <see cref="CryptoConfig"/>.</param>
		/// <returns>Unencrypted symmetric key. It is the responsibility of the caller of this method
		/// to clear the returned byte array.</returns>
		public byte[] GenerateKey(string symmetricAlgorithmName)
		{
			using (SymmetricAlgorithm algorithm = CreateAlgorithm(symmetricAlgorithmName))
			{
				return GenerateUnprotectedKey(algorithm);
			}
		}

		/// <summary>
		/// Generates a symmetric key based on an algorithm type.
		/// </summary>
		/// <param name="symmetricAlgorithmType">Symmetric algorithm type.</param>
		/// <returns>Unencrypted symmetric key. It is the responsibility of the caller of this method
		/// to clear the returned byte array.</returns>
		public byte[] GenerateKey(Type symmetricAlgorithmType)
		{
			using (SymmetricAlgorithm algorithm = CreateAlgorithm(symmetricAlgorithmType))
			{
				return GenerateUnprotectedKey(algorithm);
			}
		}

		private byte[] GenerateUnprotectedKey(SymmetricAlgorithm algorithm)
		{
			byte[] generatedKey = null;

			using (algorithm)
			{
				algorithm.GenerateKey();
				generatedKey = algorithm.Key;
			}
			return generatedKey;
		}

		private ProtectedKey GenerateKey(SymmetricAlgorithm algorithm, DataProtectionScope dataProtectionScope)
		{
			byte[] generatedKey = GenerateUnprotectedKey(algorithm);
			try
			{
				return ProtectedKey.CreateFromPlaintextKey(generatedKey, dataProtectionScope);
			}
			finally
			{
				if (generatedKey != null) CryptographyUtility.ZeroOutBytes(generatedKey);
			}
		}

		private SymmetricAlgorithm CreateAlgorithm(string symmetricAlgorithmName)
		{
			SymmetricAlgorithm algorithm = (SymmetricAlgorithm)CryptoConfig.CreateFromName(symmetricAlgorithmName);
			if (algorithm == null)
			{
				string message = String.Format(Resources.UnknownSymmetricAlgorithmCreatedError, symmetricAlgorithmName);
				throw new ArgumentException(message);
			}
			return algorithm;
		}

		private SymmetricAlgorithm CreateAlgorithm(Type symmetricAlgorithmType)
		{
			SymmetricAlgorithm algorithm = null;
			try
			{
				algorithm = (SymmetricAlgorithm)Activator.CreateInstance(symmetricAlgorithmType);
			}
			catch (Exception)
			{
				throw new ArgumentException(Resources.UnknownSymmetricAlgorithmCreatedError, symmetricAlgorithmType.Name);
			}
			return algorithm;
		}
	}
}
