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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	[TestClass]
	public class ConfiguredObjectStrategyFixture
	{
		private const string name = "name";

		private StrategyChain strategyChain;
		private PolicyList policyList;

		private DictionaryConfigurationSource source;

		[TestInitialize]
		public void SetUp()
		{
			strategyChain = new StrategyChain();
			strategyChain.Add(new ConfiguredObjectStrategy());
			strategyChain.Add(new MockBuilderStrategy());

			policyList = new PolicyList();
			source = new DictionaryConfigurationSource();
			policyList.Set<IConfigurationObjectPolicy>(new ConfigurationObjectPolicy(source), typeof (IConfigurationSource));
		}

		[TestMethod]
		public void ConfiguredObjectStrategyCallsCustomFactoryIfFactoryAttributeIsPresent()
		{
			BuilderContext context =
				new BuilderContext(strategyChain, null, null, policyList, NamedTypeBuildKey.Make<MockObjectWithFactory>(name), null);

			object createdObject = context.Strategies.ExecuteBuildUp(context);

			Assert.IsNotNull(createdObject);
			Assert.AreSame(MockFactory.MockObject, createdObject);
		}

		[TestMethod]
		[ExpectedException(typeof (BuildFailedException))]
		public void ConfiguredObjectStrategyThrowsIfFactoryAttributesIsNotPresent()
		{
			BuilderContext context =
				new BuilderContext(strategyChain, null, null, policyList, NamedTypeBuildKey.Make<object>(name), null);

			context.Strategies.ExecuteBuildUp(context);
		}
	}

	public class MockBuilderStrategy : BuilderStrategy
	{
		public object existing;

		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);

			existing = context.Existing;
		}
	}

	[CustomFactory(typeof (MockFactory))]
	public class MockObjectWithFactory
	{
	}

	public class MockFactory : ICustomFactory
	{
		public static object MockObject = new object();

		public object CreateObject(IBuilderContext context,
		                           string name,
		                           IConfigurationSource configurationSource,
		                           ConfigurationReflectionCache reflectionCache)
		{
			return MockObject;
		}
	}
}