//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	[TestClass]
	public class ContainerPolicyCreatorAttributeFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorWithNullTypeThrows()
		{
			new ContainerPolicyCreatorAttribute(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ContructorWithInvalidTypeThrows()
		{
			new ContainerPolicyCreatorAttribute(typeof(bool));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructorWithTypeWithoutNoArgsCtorThrows()
		{
			new ContainerPolicyCreatorAttribute(typeof(TestContainerPolicyCreatorWithoutNoArgsCtor));
		}

		[TestMethod]
		public void ConstructorAcceptsValidType()
		{
			ContainerPolicyCreatorAttribute attribute = new ContainerPolicyCreatorAttribute(typeof(TestContainerPolicyCreator));

			Assert.AreSame(typeof(TestContainerPolicyCreator), attribute.PolicyCreatorType);
		}

		private class TestContainerPolicyCreatorWithoutNoArgsCtor : IContainerPolicyCreator
		{
			public TestContainerPolicyCreatorWithoutNoArgsCtor(int ignored)
			{
			}

			#region IContainerPolicyCreator Members

			public void CreatePolicies(
				IPolicyList policyList,
				string instanceName,
				ConfigurationElement configurationObject,
				IConfigurationSource configurationSource)
			{
				throw new NotImplementedException();
			}

			#endregion
		}

		private class TestContainerPolicyCreator : IContainerPolicyCreator
		{
			#region IContainerPolicyCreator Members

			public void CreatePolicies(
				IPolicyList policyList,
				string instanceName,
				ConfigurationElement configurationObject,
				IConfigurationSource configurationSource)
			{
				throw new NotImplementedException();
			}

			#endregion
		}
	}
}
