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

using System.Configuration;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.ObjectBuilder2;

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

	[Assembler(typeof(MockAuthorizationProvider2Assembler))]
	[ContainerPolicyCreator(typeof(MockAuthorizationProvider2PolicyCreator))]
	public class MockAuthorizationProvider2Data : AuthorizationProviderData
	{
		public MockAuthorizationProvider2Data()
		{
		}

		public MockAuthorizationProvider2Data(string name)
			: base(name, typeof(MockAuthorizationProvider2))
		{
		}
	}

	public class MockAuthorizationProvider2Assembler : IAssembler<IAuthorizationProvider, AuthorizationProviderData>
	{
		public IAuthorizationProvider Assemble(IBuilderContext context,
											   AuthorizationProviderData objectConfiguration,
											   IConfigurationSource configurationSource,
											   ConfigurationReflectionCache reflectionCache)
		{
			return new MockAuthorizationProvider2();
		}
	}

	public class MockAuthorizationProvider2PolicyCreator : IContainerPolicyCreator
	{
		#region IContainerPolicyCreator Members

		public void CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
		}

		#endregion
	}
}
