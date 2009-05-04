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

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ObjectBuilder
{
	//public class MockBuilderContext : BuilderContext
	//{
	//    public IReadWriteLocator InnerLocator;
	//    public BuilderStrategyChain InnerChain = new BuilderStrategyChain();
	//    public PolicyList InnerPolicies = new PolicyList();
	//    public LifetimeContainer lifetimeContainer = new LifetimeContainer();

	//    public MockBuilderContext()
	//        : this(new Locator())
	//    {
	//    }

	//    public MockBuilderContext(IReadWriteLocator locator)
	//    {
	//        InnerLocator = locator;
	//        SetLocator(InnerLocator);
	//        StrategyChain = InnerChain;
	//        SetPolicies(InnerPolicies);

	//        if (Locator != null && !Locator.Contains(typeof(ILifetimeContainer)))
	//            Locator.Add(typeof(ILifetimeContainer), lifetimeContainer);
	//    }
	//}
}

