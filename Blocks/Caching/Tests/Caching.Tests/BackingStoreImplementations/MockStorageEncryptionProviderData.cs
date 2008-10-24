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

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{   
	[Assembler(typeof(TypeInstantiationAssembler<IStorageEncryptionProvider, StorageEncryptionProviderData>))]
    public class MockStorageEncryptionProviderData : StorageEncryptionProviderData
    {
        public MockStorageEncryptionProviderData()
        {
        }

		public MockStorageEncryptionProviderData(string name)
			: base(name, typeof(MockStorageEncryptionProvider))
		{
		}
    }
}

