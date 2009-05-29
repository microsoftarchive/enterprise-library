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
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	[ConfigurationElementType(typeof(MockSecurityCacheProviderData))]
	public class MockSecurityCacheProvider : SecurityCacheProvider
	{
        public MockSecurityCacheProvider(ISecurityCacheProviderInstrumentationProvider instrumentationProvider)
            :base(instrumentationProvider)
        {
        }
        
        public ISecurityCacheProviderInstrumentationProvider GetInstrumentationProvder()
        {
            return base.InstrumentationProvider;
        }

		public override IToken SaveIdentity(IIdentity identity)
		{
			return null;
		}

		public override void SaveIdentity(IIdentity identity, IToken token)
		{
		}

		public override IToken SavePrincipal(IPrincipal principal)
		{
			return null;
		}

		public override void SavePrincipal(IPrincipal principal, IToken token)
		{
		}

		public override IToken SaveProfile(object profile)
		{
			return null;
		}

		public override void SaveProfile(object profile, IToken token)
		{
		}

		public override void ExpireIdentity(IToken token)
		{
		}

		public override void ExpirePrincipal(IToken token)
		{
		}

		public override void ExpireProfile(IToken token)
		{
		}

		public override IIdentity GetIdentity(IToken token)
		{
			return null;
		}

		public override IPrincipal GetPrincipal(IToken token)
		{
			return null;
		}

		public override Object GetProfile(IToken token)
		{
			return null;
		}
	}
}

