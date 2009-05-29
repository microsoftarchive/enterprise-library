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

using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	[ConfigurationElementType(typeof(MockAuthorizationProvider2Data))]
	public class MockAuthorizationProvider2 : AuthorizationProvider
	{
		public MockAuthorizationProvider2()
		{
		}

		public override bool Authorize(IPrincipal principal, string context)
		{
			return false;
		}
	}

	public class MockAuthorizationProvider2Data : AuthorizationProviderData
	{
		public MockAuthorizationProvider2Data()
		{
		}

		public MockAuthorizationProvider2Data(string name)
			: base(name, typeof(MockAuthorizationProvider2))
		{
		}

        protected override System.Linq.Expressions.Expression<System.Func<IAuthorizationProvider>> GetCreationExpression()
        {
            return () => new MockAuthorizationProvider2();
        }
	}

}
