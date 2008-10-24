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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
	[ConfigurationElementType(typeof(CustomSymmetricCryptoProviderData))]
	public class MockCustomSymmetricProvider : MockCustomProviderBase, ISymmetricCryptoProvider
	{
		public MockCustomSymmetricProvider(NameValueCollection attributes)
			: base(attributes)
		{
		}

		public byte[] Encrypt(byte[] plaintext)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public byte[] Decrypt(byte[] ciphertext)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
