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
        public void WhenUsingLocatorWithEmptyProviderList_ThenNoProvidersAreReturned()
        {
            var locator = new CompositeTypeRegistrationsProviderLocator();

            var registrations = locator.GetRegistrations(configSource);

            Assert.AreEqual(0, registrations.Count());
        }

        [TestMethod]
        public void WhenUsingOneLocatorStrategy_ThenThatStrategyIsCalled()
        {
            var mocks = new MockLocatorBuilder(1);

            var locator = mocks.Locator;

            var registrations = locator.GetRegistrations(configSource);

            Assert.AreEqual(0, registrations.Count());

            mocks.Verify();
        }

        [TestMethod]
        public void WhenUsingMultipleLocatorStrategies_ThenTheyAreAllCalled()
        {
            var mocks = new MockLocatorBuilder(3);

            var locator = mocks.Locator;

            var registrations = locator.GetRegistrations(configSource).ToList();

            //CollectionAssert.AreEqual(mocks.Providers, registrations);

            mocks.Verify();
        }
    }

    class MockLocatorBuilder
    {
        private readonly List<ITypeRegistrationsProvider> locators = new List<ITypeRegistrationsProvider>();
        private readonly MockFactory mockFactory = new MockFactory(MockBehavior.Strict);

        public MockLocatorBuilder(int numProviders)
        {
            for(int i = 0; i < numProviders; ++i)
            {
                AddProvider();
            }
        }

        public TypeRegistrationsProvider Locator { get { return new CompositeTypeRegistrationsProviderLocator(locators); } }

        public void Verify()
        {
            mockFactory.Verify();
        }

        public void AddProvider()
        {
            var mockProvider = mockFactory.Create<TypeRegistrationsProvider>();
            mockProvider.Setup(p => p.GetRegistrations(It.IsAny<IConfigurationSource>())).Returns(new TypeRegistration[0]).Verifiable();

            locators.Add(mockProvider.Object);
        }
    }
}
