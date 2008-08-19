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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// <para>Represents basic cryptography services for a <see cref="HashAlgorithm"/>.</para>
	/// </summary>
	public class HashCryptographer
	{
		private Type algorithmType;
		private ProtectedKey key;

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="HashCryptographer"/> with an algorithm type.</para>
		/// </summary>
		/// <param name="algorithmType">A fully qualifed type name derived from <see cref="HashAlgorithm"/>.</param>
		public HashCryptographer(Type algorithmType)
		{
			if (algorithmType == null) throw new ArgumentNullException("algorithmType");
			if (!typeof(HashAlgorithm).IsAssignableFrom(algorithmType)) throw new ArgumentException(Resources.ExceptionCreatingHashAlgorithmInstance, "algorithmType");

			this.algorithmType = algorithmType;
		}

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="HashCryptographer"/> with an algorithm type and key.</para>
		/// </summary>
		/// <param name="algorithmType">A fully qualifed type name derived from <see cref="HashAlgorithm"/>.</param>
		/// <param name="protectedKey"><para>The key for a <see cref="KeyedHashAlgorithm"/>.</para></param>
		/// <remarks>
		/// While this overload will work with a specified <see cref="HashAlgorithm"/>, the protectedKey 
		/// is only relevant when initializing with a specified <see cref="KeyedHashAlgorithm"/>.
		/// </remarks>
		public HashCryptographer(Type algorithmType, ProtectedKey protectedKey)
			: this(algorithmType)
		{
			this.key = protectedKey;
		}

		/// <summary>
		/// <para>Computes the hash value of the plaintext.</para>
		/// </summary>
		/// <param name="plaintext"><para>The plaintext in which you wish to hash.</para></param>
		/// <returns><para>The resulting hash.</para></returns>
		public byte[] ComputeHash(byte[] plaintext)
		{
			byte[] hash = null;

			using (HashAlgorithm algorithm = GetHashAlgorithm())
			{
				hash = algorithm.ComputeHash(plaintext);
			}

			return hash;
		}

		private HashAlgorithm GetHashAlgorithm()
		{
			HashAlgorithm algorithm = Activator.CreateInstance(algorithmType, true) as HashAlgorithm;
			KeyedHashAlgorithm keyedHashAlgorithm = algorithm as KeyedHashAlgorithm;
			if ((null != keyedHashAlgorithm) && (key != null))
			{
				keyedHashAlgorithm.Key = key.DecryptedKey;
			}
			return algorithm;
		}
	}
}