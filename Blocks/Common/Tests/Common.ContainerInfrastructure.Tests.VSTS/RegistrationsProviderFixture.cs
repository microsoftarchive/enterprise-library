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

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    [TestClass]
    public class GivenAnEmptyConfigurationSource
    {
        private IConfigurationSource configSource;

        [TestInitialize]
        public void Given()
        {
            configSource = new DictionaryConfigurationSource();
        }

        [TestMethod]
        public void WhenUsingLocatorWithEmptyStrategyList_ThenNoProvidersAreIsReturned()
        {
            var locator = new TypeRegistrationsProviderLocator(new TypeRegistrationsProviderLocationStrategy[0]);

            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(0, providers.Count);
        }

        [TestMethod]
        public void WhenUsingOneLocatorStrategy_ThenThatStrategyIsCalled()
        {
            var mocks = new MockStrategyBuilder(1);

            var locator = new TypeRegistrationsProviderLocator(mocks.Strategies);

            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(1, providers.Count);
            Assert.AreSame(mocks.Providers[0], providers[0]);

            mocks.Verify();
        }

        [TestMethod]
        public void WhenUsingMultipleLocatorStrategies_ThenTheyAreAllCalled()
        {
            var mocks = new MockStrategyBuilder(3);

            var locator = new TypeRegistrationsProviderLocator(mocks.Strategies);

            var providers = locator.GetProviders(configSource).ToList();

            CollectionAssert.AreEqual(mocks.Providers, providers);

            mocks.Verify();
        }

        [TestMethod]
        public void WhenLocatorStrategyReturnsNull_ThenTheCorrectProvidersAreReturned()
        {
            var mocks = new MockStrategyBuilder(1);
            mocks.AddNullReturningLocator();
            mocks.AddProvider();

            var locator = new TypeRegistrationsProviderLocator(mocks.Strategies);
            var providers = locator.GetProviders(configSource).ToList();

            Assert.AreEqual(2, providers.Count);

            mocks.Verify();
        }
    }

    class MockStrategyBuilder
    {
        private readonly List<ITypeRegistrationsProvider> providers = new List<ITypeRegistrationsProvider>();
        private readonly List<TypeRegistrationsProviderLocationStrategy> strategies = new List<TypeRegistrationsProviderLocationStrategy>();
        private readonly MockFactory mockFactory = new MockFactory(MockBehavior.Strict);

        public MockStrategyBuilder(int numProviders)
        {
            for(int i = 0; i < numProviders; ++i)
            {
                AddProvider();
            }
        }

        public List<ITypeRegistrationsProvider> Providers { get { return providers; } }
        public List<TypeRegistrationsProviderLocationStrategy> Strategies { get { return strategies;  } }

        public void Verify()
        {
            mockFactory.Verify();
        }

        public void AddProvider()
        {
            var mockProvider = mockFactory.Create<ITypeRegistrationsProvider>();
            providers.Add(mockProvider.Object);

            var mockStrategy = mockFactory.Create<TypeRegistrationsProviderLocationStrategy>();
            mockStrategy.Setup(s => s.GetProvider(It.IsAny<IConfigurationSource>())).Returns(mockProvider.Object)
                .Verifiable();

            strategies.Add(mockStrategy.Object);
        }

        public void AddNullReturningLocator()
        {
            var mockStrategy = mockFactory.Create<TypeRegistrationsProviderLocationStrategy>();
            mockStrategy.Setup(s => s.GetProvider(It.IsAny<IConfigurationSource>())).Returns((ITypeRegistrationsProvider)null).Verifiable();
            strategies.Add(mockStrategy.Object);
        }
    }
}
