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

using System;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	[ConfigurationElementType(typeof(CustomSecurityCacheProviderData))]
	public class MockCustomSecurityCacheProvider
		: MockCustomProviderBase, ISecurityCacheProvider
	{
		public MockCustomSecurityCacheProvider(NameValueCollection attributes)
			: base(attributes)
		{
		}

		public IToken SaveIdentity(System.Security.Principal.IIdentity identity)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void SaveIdentity(System.Security.Principal.IIdentity identity, IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public IToken SavePrincipal(System.Security.Principal.IPrincipal principal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void SavePrincipal(System.Security.Principal.IPrincipal principal, IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public IToken SaveProfile(object profile)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void SaveProfile(object profile, IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void ExpireIdentity(IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void ExpirePrincipal(IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void ExpireProfile(IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public System.Security.Principal.IIdentity GetIdentity(IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public System.Security.Principal.IPrincipal GetPrincipal(IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public object GetProfile(IToken token)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
