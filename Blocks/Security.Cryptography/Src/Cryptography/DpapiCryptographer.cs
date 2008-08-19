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
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;
using System.Security.Permissions;
using System.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// <para>Represents a wrapper over <see cref="ProtectedData"/>.</para>
	/// </summary>
	[DataProtectionPermission(SecurityAction.Demand, Flags = DataProtectionPermissionFlags.ProtectData | DataProtectionPermissionFlags.UnprotectData)]
	public sealed class DpapiCryptographer
	{
		private DataProtectionScope storeScope;

		/// <overloads>
		/// <para>Initializes a new instance of the <see cref="DpapiCryptographer"/> class with the <see cref="DataProtectionScope"/> set to <see cref="DataProtectionScope.LocalMachine"/>.</para>
		/// </overloads>
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DpapiCryptographer"/> class with the <see cref="DataProtectionScope"/> set to <see cref="DataProtectionScope.LocalMachine"/>.</para>
		/// </summary>
		public DpapiCryptographer()
			: this(DataProtectionScope.LocalMachine)
		{
		}

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DpapiCryptographer"/> class with a <see cref="DataProtectionScope"/>.</para>
		/// </summary>
		/// <param name="storeScope"><para>One of the <see cref="DataProtectionScope"/> values.</para></param>
		public DpapiCryptographer(DataProtectionScope storeScope)
		{
			this.storeScope = storeScope;
		}

		/// <summary>
		/// <para>Gets the storage scope for this instance.</para>
		/// </summary>
		/// <value>One of the <see cref="DataProtectionScope"/> values.</value>
		public DataProtectionScope StoreScope
		{
			get { return storeScope; }
		}

		/// <summary>
		/// <para>
		/// Encrypt given data; this overload can be used ONLY when storage mode is "User", since when storage mode 
		/// is "Machine" we MUST have optional entropy to "salt" the phrase.
		/// This will throw an Invalid Operation Exception if used in Machine mode.
		/// </para>
		/// </summary>
		/// <param name="plaintext"><para>The plain text that will be encrypted.</para></param>
		/// <returns><para>The resulting cipher text.</para></returns>
		/// <exception cref="InvalidOperationException">Thrown when attempt is made to DPAPI-encrypt data
		/// using LocalMachine scope and null entropy.</exception>
		public byte[] Encrypt(byte[] plaintext)
		{
			return Encrypt(plaintext, null);
		}

		/// <summary>
		/// <para>
		/// Encrypt given data; this overload should be used when storage mode is "Machine", since when storage mode 
		/// is "Machine" you must define entropy to "salt" the phrase.
		/// </para>
		/// </summary>
		/// <param name="plaintext"><para>The plain text that will be encrypted.</para></param>
		/// <param name="entropy"><para>The entropy to salt the phrase.</para></param>
		/// <returns><para>The resulting cipher text.</para></returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="plaintext"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="plaintext"/> is empty.</exception>
		/// <exception cref="InvalidOperationException">Thrown when attempt is made to DPAPI-encrypt data
		/// using LocalMachine scope and null entropy.</exception>
		public byte[] Encrypt(byte[] plaintext, byte[] entropy)
		{
			if (plaintext == null) throw new ArgumentNullException("plainText");
			if (plaintext.Length == 0) throw new ArgumentException(Resources.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, "plainText");

			return ProtectedData.Protect(plaintext, entropy, storeScope);
		}

		/// <summary>
		/// <para>
		/// Decrypts the given ciphertext.  Can be used only when in "User" mode, otherwise this will throw 
		/// an InvalidOperationException because entropy is required when using Machine mode.
		/// </para>
		/// </summary>
		/// <param name="cipherText">
		/// <para>The cipher text that will be decrypted.</para>
		/// </param>
		/// <returns>
		/// <para>The resulting plain text.</para>
		/// </returns>
		/// <exception cref="InvalidOperationException">Thrown when attempt is made to DPAPI-decrypt data
		/// using LocalMachine scope and null entropy.</exception>
		public byte[] Decrypt(byte[] cipherText)
		{
			return Decrypt(cipherText, null);
		}


		/// <summary>
		/// <para>Decrypt the given data.</para>
		/// </summary>
		/// <param name="cipherText"><para>The cipher text that will be decrypted.</para></param>
		/// <param name="entropy"><para>The entropy that was used to salt the phrase.</para></param>
		/// <returns><para>The resulting plain text.</para></returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cipherText"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="cipherText"/> is empty.</exception>
		/// <exception cref="InvalidOperationException">Thrown when attempt is made to DPAPI-decrypt data
		/// using LocalMachine scope and null entropy.</exception>
		public byte[] Decrypt(byte[] cipherText, byte[] entropy)
		{
			if (cipherText == null) throw new ArgumentNullException("cipherText");
			if (cipherText.Length == 0) throw new ArgumentException(Resources.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, "cipherText");

			return ProtectedData.Unprotect(cipherText, entropy, storeScope);
		}
	}
}