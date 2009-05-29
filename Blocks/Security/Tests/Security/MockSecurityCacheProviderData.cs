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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	public class MockSecurityCacheProviderData : SecurityCacheProviderData
	{
		public MockSecurityCacheProviderData()
		{
		}

		public MockSecurityCacheProviderData(string name)
			: base(name, typeof(MockSecurityCacheProvider))
		{
		}


        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            return base.GetRegistrations(configurationSource).Concat(new TypeRegistration[]{base.GetInstrumentationProviderRegistration(configurationSource)});
        }

        protected override System.Linq.Expressions.Expression<System.Func<ISecurityCacheProvider>> GetCreationExpression()
        {
            return () => new MockSecurityCacheProvider(Container.Resolved<ISecurityCacheProviderInstrumentationProvider>(this.Name));
        }
	}
}
