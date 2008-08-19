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
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	public class MockFactoryStrategy : BuilderStrategy
	{
		private readonly ICustomFactory factory;
		private readonly IConfigurationSource configurationSource;
		private readonly ConfigurationReflectionCache reflectionCache;

		public MockFactoryStrategy(ICustomFactory factory,
		                           IConfigurationSource configurationSource,
		                           ConfigurationReflectionCache reflectionCache)
		{
			this.factory = factory;
			this.configurationSource = configurationSource;
			this.reflectionCache = reflectionCache;
		}

		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);

			NamedTypeBuildKey key = (NamedTypeBuildKey) context.BuildKey;

			context.Existing = factory.CreateObject(context, key.Name, configurationSource, reflectionCache);
		}
	}
}