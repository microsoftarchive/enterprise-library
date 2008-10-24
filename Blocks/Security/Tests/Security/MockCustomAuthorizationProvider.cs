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

using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	[ConfigurationElementType(typeof(CustomAuthorizationProviderData))]
	public class MockCustomAuthorizationProvider
		: MockCustomProviderBase, IAuthorizationProvider
	{
		public MockCustomAuthorizationProvider(NameValueCollection attributes)
			: base(attributes)
		{
		}

		public bool Authorize(System.Security.Principal.IPrincipal principal, string context)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}
	}
}
