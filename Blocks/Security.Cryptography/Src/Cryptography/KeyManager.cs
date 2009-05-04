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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Handles all utility tasks associated with <see cref="SymmetricAlgorithm"></see> keys.
    /// </summary>
    public static class KeyManager
    {
        static readonly ProtectedKeyCache cache = new ProtectedKeyCache();
        static readonly KeyedHashKeyGenerator keyedHashKeyGenerator = new KeyedHashKeyGenerator();
        static readonly SymmetricKeyGenerator symmetricKeyGenerator = new SymmetricKeyGenerator();

        /// <summary>
        /// Archives a cryptographic key to a <see cref="Stream"/>. This method is intended for use in 
        /// transferring a key between machines.
        /// </summary>
        /// <param name="outputStream"><see cref="Stream"/> to which key is to be archived.</param>
        /// <param name="keyToArchive">Key to be archived.</param>
        /// <param name="passphrase">User-provided passphrase used to encrypt the key in the arhive.</param>
        public static void ArchiveKey(Stream outputStream,
                                      ProtectedKey keyToArchive,
                                      string passphrase)
        {
            IKeyWriter writer = new KeyReaderWriter();
            writer.Archive(outputStream, keyToArchive, passphrase);
        }

        /// <summary>
        /// Clear the key manager cache.
        /// </summary>
        public static void ClearCache()
        {
            cache.Clear();
        }

        /// <summary>
        /// Generates a keyed hash key based on an algorithm name.
        /// </summary>
        /// <param name="keyedHashKeyAlgorithmName">Keyed hash algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
        /// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated keyed hash key.</param>
        /// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
        public static ProtectedKey GenerateKeyedHashKey(string keyedHashKeyAlgorithmName,
                                                        DataProtectionScope dataProtectionScope)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashKeyAlgorithmName, dataProtectionScope);
        }

        /// <summary>
        /// Generates a keyed hash key based on an algorithm name.
        /// </summary>
        /// <param name="keyedHashKeyAlgorithmName">Keyed hash algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
        /// <returns>Byte array containing plaintext keyed hash key. It is the responsibility of the caller to clear out the byte
        /// array to protect the key.</returns>
        public static byte[] GenerateKeyedHashKey(string keyedHashKeyAlgorithmName)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashKeyAlgorithmName);
        }

        /// <summary>
        /// Generates a keyed hash key based on an algorithm type.
        /// </summary>
        /// <param name="keyedHashAlgorithmType">Keyed hash algorithm type.</param>
        /// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated keyed hash key.</param>
        /// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
        public static ProtectedKey GenerateKeyedHashKey(Type keyedHashAlgorithmType,
                                                        DataProtectionScope dataProtectionScope)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashAlgorithmType, dataProtectionScope);
        }

        /// <summary>
        /// Generates a keyed hash key based on an algorithm type.
        /// </summary>
        /// <param name="keyedHashAlgorithmType">Keyed hash algorithm type.</param>
        /// <returns>Byte array containing plaintext keyed hash key. It is the responsibility of the caller to clear out the byte
        /// array to protect the key.</returns>
        public static byte[] GenerateKeyedHashKey(Type keyedHashAlgorithmType)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashAlgorithmType);
        }

        /// <overloads>
        /// Generates a symmetric key.
        /// </overloads>
        /// <summary>
        /// Generates a symmetric key based on an algorithm name.
        /// </summary>
        /// <param name="symmetricAlgorithmName">Symmetric algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
        /// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated symmetric key.</param>
        /// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
        public static ProtectedKey GenerateSymmetricKey(string symmetricAlgorithmName,
                                                        DataProtectionScope dataProtectionScope)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmName, dataProtectionScope);
        }

        /// <summary>
        /// Generates a symmetric key based on an algorithm name.
        /// </summary>
        /// <param name="symmetricAlgorithmName">Symmetric algorithm name as defined in <see cref="CryptoConfig"></see>.</param>
        /// <returns>Byte array containing plaintext symmetric key. It is the responsibility of the caller to clear out the byte
        /// array to protect the key.</returns>
        public static byte[] GenerateSymmetricKey(string symmetricAlgorithmName)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmName);
        }

        /// <summary>
        /// Generates a symmetric key based on an algorithm type.
        /// </summary>
        /// <param name="symmetricAlgorithmType">Symmetric algorithm type.</param>
        /// <param name="dataProtectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated symmetric key.</param>
        /// <returns>Generated key encapsulated in a <see cref="ProtectedKey"></see>.</returns>
        public static ProtectedKey GenerateSymmetricKey(Type symmetricAlgorithmType,
                                                        DataProtectionScope dataProtectionScope)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmType, dataProtectionScope);
        }

        /// <summary>
        /// Generates a symmetric key based on an algorithm type.
        /// </summary>
        /// <param name="symmetricAlgorithmType">Symmetric algorithm type.</param>
        /// <returns>Byte array containing plaintext symmetric key. It is the responsibility of the caller to clear out the byte
        /// array to protect the key.</returns>
        public static byte[] GenerateSymmetricKey(Type symmetricAlgorithmType)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmType);
        }

        /// <overloads>
        /// Reads an encrypted key from an input stream. This method is not intended to allow keys to be transferred
        /// from another machine.
        /// </overloads>
        /// <summary>
        /// Reads an encrypted key from an input stream. This method is not intended to allow keys to be transferred
        /// from another machine.
        /// </summary>
        /// <param name="inputStream"><see cref="Stream"/> from which DPAPI-protected key is to be read.</param>
        /// <param name="dpapiProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        /// <returns>Key read from stream, encapsulated in a <see cref="ProtectedKey"></see>.</returns>
        public static ProtectedKey Read(Stream inputStream,
                                        DataProtectionScope dpapiProtectionScope)
        {
            IKeyReader reader = new KeyReaderWriter();
            ProtectedKey key = reader.Read(inputStream, dpapiProtectionScope);

            return key;
        }

        /// <summary>
        /// Reads an encrypted key from an input file. This method is not intended to allow keys to be transferred
        /// from another machine.
        /// </summary>
        /// <param name="protectedKeyFileName">Input file from which DPAPI-protected key is to be read.</param>
        /// <param name="dpapiProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        /// <returns>Key read from stream, encapsulated in a <see cref="ProtectedKey"></see>.</returns>
        public static ProtectedKey Read(string protectedKeyFileName,
                                        DataProtectionScope dpapiProtectionScope)
        {
            string completeFileName = Path.GetFullPath(protectedKeyFileName);
            if (cache[completeFileName] != null) return cache[completeFileName];

            using (FileStream stream = new FileStream(protectedKeyFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ProtectedKey protectedKey = Read(stream, dpapiProtectionScope);
                cache[completeFileName] = protectedKey;

                return protectedKey;
            }
        }

        /// <summary>
        /// Restores a cryptogrpahic key from a <see cref="Stream"/>. This method is intended for use in
        /// transferring a key between machines.
        /// </summary>
        /// <param name="inputStream"><see cref="Stream"/> from which key is to be restored.</param>
        /// <param name="passphrase">User-provided passphrase used to encrypt the key in the arhive.</param>
        /// <param name="protectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        /// <returns>Key restored from stream, encapsulated in a <see cref="ProtectedKey"></see>.</returns>
        public static ProtectedKey RestoreKey(Stream inputStream,
                                              string passphrase,
                                              DataProtectionScope protectionScope)
        {
            IKeyReader reader = new KeyReaderWriter();
            return reader.Restore(inputStream, passphrase, protectionScope);
        }

        /// <overloads>
        /// Writes an encrypted key to an output stream. This method is not intended to allow the keys to be 
        /// moved from machine to machine.
        /// </overloads>
        /// <summary>
        /// Writes an encrypted key to an output stream. This method is not intended to allow the keys to be 
        /// moved from machine to machine.
        /// </summary>
        /// <param name="outputStream"><see cref="Stream"/> to which DPAPI-protected key is to be written.</param>
        /// <param name="encryptedKey">Encrypted key to be written to stream.</param>
        /// <param name="dpapiProtectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
        public static void Write(Stream outputStream,
                                 byte[] encryptedKey,
                                 DataProtectionScope dpapiProtectionScope)
        {
            ProtectedKey key = ProtectedKey.CreateFromEncryptedKey(encryptedKey, dpapiProtectionScope);
            Write(outputStream, key);
        }

        /// <summary>
        /// Writes an encrypted key to an output stream. This method is not intended to allow the keys to be 
        /// moved from machine to machine.
        /// </summary>
        /// <param name="outputStream"><see cref="Stream"/> to which DPAPI-protected key is to be written.</param>
        /// <param name="key">Encrypted key to be written to stream.</param>
        public static void Write(Stream outputStream,
                                 ProtectedKey key)
        {
            IKeyWriter writer = new KeyReaderWriter();
            writer.Write(outputStream, key);
        }
    }
}
