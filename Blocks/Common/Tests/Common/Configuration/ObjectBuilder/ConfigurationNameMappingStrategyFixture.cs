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
	public class ConfigurationNameMappingStrategyFixture
	{
		private IBuilder builder;
		private StagedStrategyChain<BuilderStage> strategies;

		[TestInitialize]
		public void SetUp()
		{
			builder = new Builder();
			strategies = new StagedStrategyChain<BuilderStage>();
			strategies.AddNew<ConfigurationNameMappingStrategy>(BuilderStage.PreCreation);

			MockNameMapper.invoked = false;
		}

		[TestMethod]
		public void ChainForNullIdOnTypeWithoutNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithoutNameMappingAttribute>(),
			                null);

			Assert.IsFalse(MockNameMapper.invoked);
		}

		[TestMethod]
		public void ChainForNullIdOnTypeWithNameMappingDoesInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithNameMappingAttribute>(),
			                null);

			Assert.IsTrue(MockNameMapper.invoked);
		}

		[TestMethod]
		public void ChainForNonNullIdOnTypeWithoutNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithoutNameMappingAttribute>("id"),
			                null);

			Assert.IsFalse(MockNameMapper.invoked);
		}

		[TestMethod]
		public void ChainForNonNullIdOnTypeWithNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithNameMappingAttribute>("id"),
			                null);

			Assert.IsFalse(MockNameMapper.invoked);
		}
	}

    public class TypeWithoutNameMappingAttribute {}

    [ConfigurationNameMapper(typeof(MockNameMapper))]
    public class TypeWithNameMappingAttribute {}

    public class MockNameMapper : IConfigurationNameMapper
    {
        internal static bool invoked;

        public string MapName(string name,
                              IConfigurationSource configSource)
        {
            invoked = true;
            return name;
        }
    }
}