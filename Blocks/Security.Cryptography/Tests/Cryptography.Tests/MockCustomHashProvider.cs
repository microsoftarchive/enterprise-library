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
	[ConfigurationElementType(typeof(CustomHashProviderData))]
	public class MockCustomHashProvider : MockCustomProviderBase, IHashProvider
	{
		public MockCustomHashProvider(NameValueCollection attributes)
			: base(attributes)
		{
		}

		public byte[] CreateHash(byte[] plaintext)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool CompareHash(byte[] plaintext, byte[] hashedtext)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
