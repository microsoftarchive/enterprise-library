//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations
{
	[ConfigurationElementType(typeof(MockStorageEncryptionProviderData))]
	public class MockStorageEncryptionProvider : IStorageEncryptionProvider
	{
		public static bool Encrypted;
		public static bool Decrypted;

		public MockStorageEncryptionProvider()
		{
		}

		public byte[] Encrypt(byte[] plaintext)
		{
			Encrypted = true;
			return plaintext;
		}

		public byte[] Decrypt(byte[] ciphertext)
		{
			Decrypted = true;
			return ciphertext;
		}
	}
}

