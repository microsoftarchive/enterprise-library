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
using System.Security.Cryptography;
namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// Generates keys for specified cryptographic algorithms.
	/// </summary>
	interface IKeyGenerator
	{
		/// <overloads>
		/// Generates a key for a specific cryptographic algorithm.
		/// </overloads>
		/// <summary>
		/// Generates a key based on an algorithm name.
		/// </summary>
		/// <param name="algorithmName">Algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated key.</param>
		/// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		ProtectedKey GenerateKey(string algorithmName, DataProtectionScope dataProtectionScope);

		/// <summary>
		/// Generates a key based on an algorithm name.
		/// </summary>
		/// <param name="algorithmName">Algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
		/// <returns>Generated key for given algorithm. It is the responsibility of the caller to correctly dispose of the 
		/// returned byte array.</returns>
		byte[] GenerateKey(string algorithmName);

		/// <summary>
		/// Generates a key based on an algorithm type.
		/// </summary>
		/// <param name="algorithmType">Algorithm type. Should be subclass of appropriate cryptographic class.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated key.</param>
		/// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		ProtectedKey GenerateKey(Type algorithmType, DataProtectionScope dataProtectionScope);

		/// <summary>
		/// Generates a key based on an algorithm type.
		/// </summary>
		/// <param name="algorithmType">Algorithm type. Should be subclass of appropriate cryptographic class.</param>
		/// <returns>Generated key for given algorithm. It is the responsibility of the caller to correctly dispose of the 
		/// returned byte array.</returns>
		byte[] GenerateKey(Type algorithmType);
	}
}
