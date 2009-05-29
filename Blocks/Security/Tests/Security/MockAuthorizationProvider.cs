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
	[ConfigurationElementType(typeof(MockAuthorizationProviderData))]
	public class MockAuthorizationProvider : AuthorizationProvider
	{
		private bool initialized = false;

		public MockAuthorizationProvider()
		{
			initialized = true;
		}

		public bool Authorize(object authority, object context)
		{
			return false;
		}

		public override bool Authorize(IPrincipal principal, string context)
		{
			return false;
		}

		public bool Initialized
		{
			get { return initialized; }
		}
	}


	public class MockAuthorizationProviderData : AuthorizationProviderData
	{
		public MockAuthorizationProviderData()
		{
		}

		public MockAuthorizationProviderData(string name)
			: base(name, typeof(MockAuthorizationProvider))
		{
		}

        protected override System.Linq.Expressions.Expression<System.Func<IAuthorizationProvider>> GetCreationExpression()
        {
            return () => new MockAuthorizationProvider();
        }
	}
}
