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
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// Represents an encrypted cryptographic key and the DPAPI <see cref="DataProtectionScope"/> used to encrypt it.
	/// </summary>
	public class ProtectedKey
	{
		private byte[] protectedKey;
		private DataProtectionScope protectionScope;

		/// <summary>
		/// Constructor method use to create a new <see cref="ProtectedKey"></see> from a plaintext symmetric key. The caller of this method
		/// is responsible for clearing the byte array containing the plaintext key after calling this method.
		/// </summary>
		/// <param name="plaintextKey">Plaintext key to protect. The caller of this method is responsible for 
		/// clearing the byte array containing the plaintext key after calling this method.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see></param> used to protect this key.
		/// <returns>Initialized <see cref="ProtectedKey"></see> containing the plaintext key protected with DPAPI.</returns>
		public static ProtectedKey CreateFromPlaintextKey(byte[] plaintextKey, DataProtectionScope dataProtectionScope)
		{
			byte[] encryptedKey = ProtectedData.Protect(plaintextKey, null, dataProtectionScope);
			return new ProtectedKey(encryptedKey, dataProtectionScope);
		}

		/// <summary>
		/// Constructor method used to create a new <see cref="ProtectedKey"/> from an already encrypted symmetric key.
		/// </summary>
		/// <param name="encryptedKey">Encrypted key to protect.</param>
		/// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see></param> used to protect this key.
		/// <returns>Initialized <see cref="ProtectedKey"></see> containing the plaintext key protected with DPAPI.</returns>
		public static ProtectedKey CreateFromEncryptedKey(byte[] encryptedKey, DataProtectionScope dataProtectionScope)
		{
			return new ProtectedKey(encryptedKey, dataProtectionScope);
		}

		/// <summary>
		/// Gets the encrypted key contained by this object.
		/// </summary>
		public byte[] EncryptedKey
		{
			get
			{
				return (byte[])protectedKey.Clone();
			}
		}

		/// <summary>
		/// Gets the decrypted key protected by this object. It is the responsibility of the caller of this method 
		/// to clear the returned byte array.
		/// </summary>
		public byte[] DecryptedKey
		{
			get { return Unprotect(); }
		}

		/// <summary>
		/// Returns the decrypted symmetric key.
		/// </summary>
		/// <returns>Unencrypted symmetric key. It is the responsibility of the caller of this method to
		/// clear the returned byte array.</returns>
		public virtual byte[] Unprotect()
		{
			return ProtectedData.Unprotect(protectedKey, null, protectionScope);
		}

		private ProtectedKey(byte[] protectedKey, DataProtectionScope protectionScope)
		{
			this.protectionScope = protectionScope;
			this.protectedKey = protectedKey;
		}
	}
}
