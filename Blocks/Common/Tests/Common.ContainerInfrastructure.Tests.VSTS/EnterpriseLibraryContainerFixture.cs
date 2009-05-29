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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    public abstract class ContainerConfigurationContext
    {
        protected IConfigurationSource Config { get; set; }
        protected CompositeTypeRegistrationsProviderLocator Locator { get; set; }
        protected abstract IContainerConfigurator Configurator { get; }

        [TestInitialize]
        public void SetupContext()
        {
            Config = CreateConfig();

            Locator = new CompositeTypeRegistrationsProviderLocator(TypeRegistrationsProvider.CreateDefaultProvider(Config));

            Given();

            EnterpriseLibraryContainer.ConfigureContainer(Locator, Configurator, Config);
        }

        protected virtual IConfigurationSource CreateConfig()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            var registrationSettings = new TypeRegistrationProvidersConfigurationSection();

            // Remove data strategy, it looks at machine.config and can screw up our tests
            registrationSettings.TypeRegistrationProviders.Remove(TypeRegistrationProvidersConfigurationSection.DataAccessTypeRegistrationProviderName);
            source.Add(null, TypeRegistrationProvidersConfigurationSection.SectionName, registrationSettings);

            return source;
        }

        protected virtual void Given()
        {
        }
    }

    public abstract class MockContainerConfigurationContext : ContainerConfigurationContext
    {
        protected Mock<IContainerConfigurator> MockConfigurator { get; set; }
        protected List<TypeRegistration> Registrations { get; private set; }

        protected override IContainerConfigurator Configurator
        {
            get { return MockConfigurator.Object; }
        }

        protected override void Given()
        {
            MockConfigurator = new Mock<IContainerConfigurator>(MockBehavior.Strict);
            MockConfigurator
                .Setup(c => c.RegisterAll(
                    It.IsAny<IConfigurationSource>(),
                    It.IsAny<ITypeRegistrationsProvider>()))
                .Callback<IConfigurationSource, ITypeRegistrationsProvider>(
                    (cs, p) => Registrations = p.GetRegistrations(cs).ToList())
                .AtMostOnce().Verifiable();
        }
    }

    public abstract class UnityContainerConfigurationContext : ContainerConfigurationContext
    {
        protected IUnityContainer Container;

        protected override IContainerConfigurator Configurator
        {
            get { return new UnityContainerConfigurator(Container); }
        }

        protected override void Given()
        {
            Container = new UnityContainer();
        }
    }

    [TestClass]
    public class GivenAnEmptyConfigSource : MockContainerConfigurationContext
    {
        [TestMethod]
        public void WhenConfiguringAContainer_ThenRegisterAllIsPassedAnEmptySequence()
        {
            MockConfigurator.Verify();
            Assert.AreEqual(0, Registrations.Count);
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithCryptoData : MockContainerConfigurationContext
    {
        protected override IConfigurationSource CreateConfig()
        {
            return new ConfigSourceBuilder().AddCryptoSettings().ConfigSource;
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegisterAllIsCalledExactlyOnce()
        {
            MockConfigurator.Verify();
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegisterAllIsPassedTypeRegistrations()
        {
            MockConfigurator.Verify();

            Assert.IsNotNull(Registrations);
            Assert.IsTrue(Registrations.Count > 0);
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithCryptoDataAndExceptionData : MockContainerConfigurationContext
    {
        protected override IConfigurationSource CreateConfig()
        {
            return new ConfigSourceBuilder()
                .AddCryptoSettings()
                .AddConnectionStringSettings()
                .AddExceptionHandlingSettings()
                .ConfigSource;
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegisterAllIsCalledExactlyOnce()
        {
            MockConfigurator.Verify();
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegistrationsArePresent()
        {
            Assert.IsTrue(Registrations.Count > 0);
        }
    }

    [TestClass]
    public class GivenAConfigSourceAndARealContainer : UnityContainerConfigurationContext
    {
        protected override void Given()
        {
            base.Given();
            // Add data back in for this one
            Locator = new CompositeTypeRegistrationsProviderLocator(Locator,
                new TypeLoadingLocator(
                    BlockSectionNames.DataRegistrationProviderLocatorType));
        }

        protected override IConfigurationSource CreateConfig()
        {
            return new ConfigSourceBuilder()
                .AddCryptoSettings()
                .AddConnectionStringSettings()
                .AddExceptionHandlingSettings()
                .ConfigSource;
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenCanResolveHashProvider()
        {
            var provider = Container.Resolve<IHashProvider>("md5");
            Assert.IsInstanceOfType(provider, typeof (HashAlgorithmProvider));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenCanResolveOtherHashProvider()
        {
            var provider = Container.Resolve<IHashProvider>("sha512");
            Assert.IsInstanceOfType(provider, typeof (HashAlgorithmProvider));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenNonConfiguredHashProvidersAreNotResolved()
        {
            try
            {
                Container.Resolve<IHashProvider>("sha128");
                Assert.Fail("Should have thrown ResolutionFailedException");
            }
            catch (ResolutionFailedException)
            {
                // ok if we get this.
            }
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenCanResolveDatabase()
        {
            var db = Container.Resolve<Database>("northwind");
            Assert.AreEqual(ConfigSourceBuilder.NorthwindConnectionString, db.ConnectionString);
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenCanResolveExceptionPolicy()
        {
            var policy = Container.Resolve<ExceptionPolicyImpl>("default");

            Assert.AreEqual(ConfigSourceBuilder.DefaultExceptionPolicyName, policy.PolicyName);
        }
    }

    [TestClass]
    public class GivenDefaultEnterpriseLibraryContainer
    {
        [TestMethod]
        public void WhenGettingCurrentProperty_ThenItIsNotNull()
        {
            Assert.IsNotNull(EnterpriseLibraryContainer.Current);
        }

        [TestMethod]
        public void WhenSettingCurrentContainer_ThenYouGetBackTheSameContainer()
        {
            var mockContainer = new Mock<IServiceLocator>();

            EnterpriseLibraryContainer.Current = mockContainer.Object;

            Assert.AreSame(mockContainer.Object, EnterpriseLibraryContainer.Current);
        }
    }

    [TestClass]
    public class GivenAConfigSourceWithPolicyInjectionData : MockContainerConfigurationContext
    {
        protected override IConfigurationSource CreateConfig()
        {
            return new ConfigSourceBuilder()
                .AddPolicyInjectionSettings()
                .ConfigSource;
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegisterAllIsCalledExactlyOnce()
        {
            MockConfigurator.Verify();
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegistrationsArePresent()
        {
            Assert.IsTrue(Registrations.Count > 0);
        }
    }
}
