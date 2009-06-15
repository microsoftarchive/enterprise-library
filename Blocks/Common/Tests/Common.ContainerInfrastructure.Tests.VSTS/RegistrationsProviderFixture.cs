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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
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



   [TestClass]
    public class GivenATypeRegisteringConfigurationSectionAndContainerChangingEvent
    {
        private const string SectionName = "MockSection";

        [TestInitialize]
        public void Given()
        {
            var configSource = new DictionaryConfigurationSource();
            var mockEventSourceProvider = new Mock<IContainerReconfiguringEventSource>();

            var section = new MockSection();
            configSource.Add(SectionName, section);

            var builder = new SectionBuilder();
            builder.LocatorSection()
                .AddConfigSection(SectionName).WithProviderName("MockSectionProvider")
                .AddTo(configSource);
          
            var eventArgs = new Mock<ContainerReconfiguringEventArgs>(configSource, new[] { SectionName });
            var provider = TypeRegistrationsProvider.CreateDefaultProvider(configSource, mockEventSourceProvider.Object);

            MockSection.UpdatedRegistrationsWasCalled = false;
            mockEventSourceProvider.Raise(e => e.ContainerReconfiguring += null, eventArgs.Object);
        }

        [TestMethod]
        public void WhenReconfiguringEventRaised_ConfigurationSectionsAreNotifiedOfChange()
        {
            Assert.IsTrue(MockSection.UpdatedRegistrationsWasCalled);
        }
    }


    [TestClass]
    public class GivenTypeRegisteringProviderAndConfigurationChangingEvent
    {
        [TestInitialize]
        public void Given()
        {
            var configSource = new DictionaryConfigurationSource();
            var mockEventSourceProvider = new Mock<IContainerReconfiguringEventSource>();

            var builder = new SectionBuilder();
            builder.LocatorSection()
                .AddProvider<MockSection>().WithProviderName("SomeName")
                .AddTo(configSource);

            var eventArgs = new Mock<ContainerReconfiguringEventArgs>(configSource, new[] { "NotUsed" });
            var provider = TypeRegistrationsProvider.CreateDefaultProvider(configSource, mockEventSourceProvider.Object);

            MockSection.UpdatedRegistrationsWasCalled = false;
            mockEventSourceProvider.Raise(e => e.ContainerReconfiguring += null, eventArgs.Object);
        }

        [TestMethod]
        public void WhenReconfiguringEventRaised_ConfigurationSectionsAreNotifiedOfChange()
        {
            Assert.IsTrue(MockSection.UpdatedRegistrationsWasCalled);
        }
    }

 
    class MockSection : ConfigurationSection, ITypeRegistrationsProvider
    {
        public static bool UpdatedRegistrationsWasCalled { get; set; }

        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            UpdatedRegistrationsWasCalled = true;
            return Enumerable.Empty<TypeRegistration>();
        }
    }

    class MockLocatorBuilder
    {
        private readonly List<ITypeRegistrationsProvider> locators = new List<ITypeRegistrationsProvider>();
        private readonly MockFactory mockFactory = new MockFactory(MockBehavior.Strict);

        public MockLocatorBuilder(int numProviders)
        {
            for (int i = 0; i < numProviders; ++i)
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

