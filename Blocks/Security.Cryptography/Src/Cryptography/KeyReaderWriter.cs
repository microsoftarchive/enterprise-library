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
using System.IO;
using System.Security.Cryptography;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// Reads and writes cryptographic keys to and from streams.
	/// </summary>
	public class KeyReaderWriter : IKeyReader, IKeyWriter
	{
		internal const int versionNumber = 4321;
		internal const int versionNumberLength = 4;

		/// <summary>
		/// Reads a DPAPI-protected key from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="protectedKeyStream"><see cref="Stream"/> containing the DPAPI-protected key.</param>
		/// <param name="protectionScope"><see cref="DataProtectionScope"></see> used to decrypt the key read from the stream.</param>
		/// <returns>Key read from stream, encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		public ProtectedKey Read(Stream protectedKeyStream, DataProtectionScope protectionScope)
		{
			ValidateKeyVersion(protectedKeyStream);
			return ProtectedKey.CreateFromEncryptedKey(ReadEncryptedKey(protectedKeyStream), protectionScope);
		}

		/// <overloads>
		/// Restores a cryptographic key from a <see cref="Stream"/>. This method is intended for use in
		/// transferring a key between machines.
		/// </overloads>
		/// <summary>
		/// Restores a cryptogrpahic key from a <see cref="Stream"/>. This method is intended for use in
		/// transferring a key between machines.
		/// </summary>
		/// <param name="protectedKeyStream"><see cref="Stream"/> from which key is to be restored.</param>
		/// <param name="passphrase">User-provided passphrase used to encrypt the key in the arhive.</param>
		/// <param name="protectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
		/// <returns>Key restored from stream, encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		public ProtectedKey Restore(Stream protectedKeyStream, string passphrase, DataProtectionScope protectionScope)
		{
			return ProtectKey(Restore(protectedKeyStream,  passphrase), protectionScope);
		}

		/// <summary>
		/// Restores a cryptographic key from a <see cref="Stream"/>. This method is intended for use in
		/// transferring a key between machines.
		/// </summary>
		/// <param name="protectedKeyStream"><see cref="Stream"/> from which key is to be restored.</param>
		/// <param name="passphrase">User-provided passphrase used to encrypt the key in the arhive.</param>
		/// <returns>Unencrypted key restored from stream. It is the responsibility of the calling code to
		/// clear the returned byte array.</returns>
		public byte[] Restore(Stream protectedKeyStream, string passphrase)
		{
			ValidateKeyVersion(protectedKeyStream);

			byte[] salt = new byte[16];
			byte[] encryptedKey = new byte[protectedKeyStream.Length - versionNumberLength - salt.Length];

			protectedKeyStream.Read(salt, 0, salt.Length);
			protectedKeyStream.Read(encryptedKey, 0, encryptedKey.Length);

			return DecryptKeyForRestore(passphrase, encryptedKey, salt);
		}

		/// <summary>
		/// Writes an encrypted key to an output stream. This method is not intended to allow the keys to be 
		/// moved from machine to machine.
		/// </summary>
		/// <param name="outputStream"><see cref="Stream"/> to which DPAPI-protected key is to be written.</param>
		/// <param name="key">Encrypted key to be written to stream.</param>
		public void Write(Stream outputStream, ProtectedKey key)
		{
			WriteVersionNumber(outputStream, versionNumber);
			WriteEncryptedKey(outputStream, key);
		}

		/// <summary>
		/// Archives a cryptographic key to a <see cref="Stream"/>. This method is intended for use in 
		/// transferring a key between machines.
		/// </summary>
		/// <param name="outputStream"><see cref="Stream"/> to which key is to be archived.</param>
		/// <param name="keyToBeArchived">Key to be archived.</param>
		/// <param name="passphrase">User-provided passphrase used to encrypt the key in the arhive.</param>
		public void Archive(Stream outputStream, ProtectedKey keyToBeArchived, string passphrase)
		{
			byte[] versionNumberBytes = BitConverter.GetBytes(versionNumber);
			byte[] salt = GenerateSalt();
			byte[] encryptedKey = GetEncryptedKey(keyToBeArchived, passphrase, salt);

			outputStream.Write(versionNumberBytes, 0, versionNumberBytes.Length);
			outputStream.Write(salt, 0, salt.Length);
			outputStream.Write(encryptedKey, 0, encryptedKey.Length);
		}

		private byte[] GetEncryptedKey(ProtectedKey keyToBeArchived, string passphrase, byte[] salt)
		{
			byte[] decryptedKey = keyToBeArchived.DecryptedKey;
			try
			{
				return EncryptKeyForArchival(decryptedKey, passphrase, salt);
			}
			finally
			{
				CryptographyUtility.ZeroOutBytes(decryptedKey);
			}
		}

		private byte[] GenerateSalt()
		{
			return CryptographyUtility.GetRandomBytes(16);
		}

		private byte[] EncryptKeyForArchival(byte[] keyToExport, string passphrase, byte[] salt)
		{
			RijndaelManaged archivalEncryptionAlgorithm = new RijndaelManaged();
			byte[] archivalKey = GenerateArchivalKey(archivalEncryptionAlgorithm, passphrase, salt);
			byte[] iv = new byte[archivalEncryptionAlgorithm.BlockSize / 8];
			byte[] encryptedKey = CryptographyUtility.Transform(archivalEncryptionAlgorithm.CreateEncryptor(archivalKey, iv),keyToExport);
			return encryptedKey;
		}
		
		private void WriteEncryptedKey(Stream outputStream, ProtectedKey key)
		{
			outputStream.Write(key.EncryptedKey, 0, key.EncryptedKey.Length);
		}

		private byte[] ReadEncryptedKey(Stream protectedKeyStream)
		{
			byte[] encryptedKey = new byte[protectedKeyStream.Length - versionNumberLength];
			protectedKeyStream.Read(encryptedKey, 0, encryptedKey.Length);

			return encryptedKey;
		}

		private void WriteVersionNumber(Stream outputStream, int versionNumber)
		{
			byte[] keyFileVersion = BitConverter.GetBytes(versionNumber);
			outputStream.Write(keyFileVersion, 0, keyFileVersion.Length);
		}

		private uint ReadVersionNumber(Stream protectedKeyStream)
		{
			byte[] keyFileVersion = new byte[versionNumberLength];
			protectedKeyStream.Read(keyFileVersion, 0, keyFileVersion.Length);

			return BitConverter.ToUInt32(keyFileVersion, 0);
		}

		private void ValidateKeyVersion(Stream protectedKeyStream)
		{
			uint readVersionNumber = ReadVersionNumber(protectedKeyStream);
			if (readVersionNumber != versionNumber)
			{
				throw new InvalidOperationException(Resources.IncorrectKeyVersionError);
			}
		}

		private byte[] GenerateArchivalKey(SymmetricAlgorithm archivalEncryptionAlgorithm, string passphrase, byte[] salt)
		{
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(passphrase, salt);
			byte[] archivalKey = pdb.GetBytes(archivalEncryptionAlgorithm.KeySize / 8);

			return archivalKey;
		}
		private byte[] DecryptKeyForRestore(string passphrase, byte[] encryptedKey, byte[] salt)
		{
			RijndaelManaged archivalEncryptionAlgorithm = new RijndaelManaged();

			byte[] restoreKey = GenerateArchivalKey(archivalEncryptionAlgorithm, passphrase, salt);
			byte[] iv = new byte[archivalEncryptionAlgorithm.BlockSize / 8];
			byte[] key = CryptographyUtility.Transform(archivalEncryptionAlgorithm.CreateDecryptor(restoreKey, iv), encryptedKey);
			CryptographyUtility.ZeroOutBytes(restoreKey);

			return key;
		}
		private ProtectedKey ProtectKey(byte[] decryptedKey, DataProtectionScope protectionScope)
		{
			ProtectedKey protectedKey = ProtectedKey.CreateFromPlaintextKey(decryptedKey, protectionScope);
			CryptographyUtility.ZeroOutBytes(decryptedKey);

			return protectedKey;
		}
	}
}
