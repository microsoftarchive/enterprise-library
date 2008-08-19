//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	[Assembler(typeof(TypeInstantiationAssembler<ISecurityCacheProvider, SecurityCacheProviderData>))]
	public class MockSecurityCacheProviderData : SecurityCacheProviderData
	{
		public MockSecurityCacheProviderData()
		{
		}

		public MockSecurityCacheProviderData(string name)
			: base(name, typeof(MockSecurityCacheProvider))
		{
		}
	}
}
