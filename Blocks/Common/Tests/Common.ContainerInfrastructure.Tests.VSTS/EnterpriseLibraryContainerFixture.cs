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
using System.Linq;
using Common.ContainerInfrastructure.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    public abstract class Context_ContainerConfigurator : ArrangeActAssert
    {
        protected List<TypeRegistration> registrations;
        protected IConfigurationSource configSource;

        protected override void Arrange()
        {
            configSource = GetConfig();
            registrations = null;
        }

        protected override void Act()
        {
            EnterpriseLibraryContainer.ConfigureContainer(GetLocator(configSource), GetConfigurator(), configSource);
        }

        protected virtual IConfigurationSource GetConfig()
        {
            var configSource = new DictionaryConfigurationSource();
            // Rip out the default data provider - it pulls in from machine.config
            // and interferes with the test.
            new SectionBuilder().LocatorSection()
                .AddTo(configSource);

            return configSource;
        }

        protected abstract IContainerConfigurator GetConfigurator();

        private ITypeRegistrationsProvider GetLocator(IConfigurationSource configSource)
        {
            return TypeRegistrationsProvider.CreateDefaultProvider(configSource,
                new NullContainerReconfiguringEventSource());
        }
    }

    public abstract class Context_MockedContainerConfigurator : Context_ContainerConfigurator
    {
        protected Mock<IContainerConfigurator> mockConfigurator;

        protected override IContainerConfigurator GetConfigurator()
        {
            mockConfigurator = new Mock<IContainerConfigurator>();
            mockConfigurator.Setup(c => c.RegisterAll(
                It.IsAny<IConfigurationSource>(),
                It.IsAny<ITypeRegistrationsProvider>()))
            .Callback<IConfigurationSource, ITypeRegistrationsProvider>(
                (cs, p) => registrations = p.GetRegistrations(cs).ToList())
            .AtMostOnce().Verifiable();


            return mockConfigurator.Object;
        }
    }

    public abstract class Context_UnityContainerConfigurator : Context_ContainerConfigurator
    {
        protected IUnityContainer container;

        protected override void Arrange()
        {
            base.Arrange();
            container = new UnityContainer();
        }

        protected override IContainerConfigurator GetConfigurator()
        {
            return new UnityContainerConfigurator(container);
        }
    }

    [TestClass]
    public class When_ConfiguringAContainerFromAnEmptyConfigSource : Context_MockedContainerConfigurator
    {
        [TestMethod]
        public void Then_RegisterAllIsPassedAnEmptySequence()
        {
            mockConfigurator.Verify();
            Assert.AreEqual(0, registrations.Count);
        }
    }

    // TODO: Fix this to actually reset the Current property between tests.
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
    public class When_ConfiguratingContainerWithPolicyInjectionData : Context_MockedContainerConfigurator
    {
        protected override IConfigurationSource GetConfig()
        {
            return new ConfigSourceBuilder()
                .AddPolicyInjectionSettings()
                .ConfigSource;
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegisterAllIsCalledExactlyOnce()
        {
            mockConfigurator.Verify();
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenRegistrationsArePresent()
        {
            Assert.IsTrue(registrations.Count > 0);
        }
    }


    /// <summary>
    /// This is a bit of an integration test to validate that, through the EnterpriseLibraryContainer,
    /// configuration change notifications can make it entirely through type registration and
    /// for objects subscribing to the configuration change notification events.
    /// </summary>
    [TestClass]
    public class WhenChangingConfigurationDataOnConfiguredEnterpriseLibraryContainer : ArrangeActAssert
    {
        ConfigurationSourceUpdatable updatableConfigSource = new ConfigurationSourceUpdatable();
        private StaticFu staticFu;

        protected IConfigurationSource GetConfig()
        {
            SectionBuilder builder = new SectionBuilder();
            builder.LocatorSection()
                .AddProvider<MockRegistrationProvider>()
                .WithProviderName("MockRegistrationProvider")
                .AddTo(updatableConfigSource);

            return updatableConfigSource;
        }

        protected override void Act()
        {
            IServiceLocator locator = EnterpriseLibraryContainer.CreateDefaultContainer(GetConfig());
            EnterpriseLibraryContainer.Current = locator;
            staticFu = EnterpriseLibraryContainer.Current.GetInstance<StaticFu>();
            Assert.IsInstanceOfType(staticFu.MyInnerFoo, typeof(Foo));

            updatableConfigSource.DoSourceChanged(new[] { "MockRegistrationProvider" });
        }

        protected override void Teardown()
        {
            EnterpriseLibraryContainer.Current = null;
        }

        [TestMethod]
        public void ThenShouldBeAbleToResolveNewElements()
        {
            IFoo resolvedItem = EnterpriseLibraryContainer.Current.GetInstance<IFoo>();
            Assert.IsInstanceOfType(resolvedItem, typeof(Tofu));
        }

        [TestMethod]
        public void ThenShouldBeAbleToResolveDependenciesThroughConfigurationChangeEvent()
        {
            StaticFu resolvedStaticFu = EnterpriseLibraryContainer.Current.GetInstance<StaticFu>();
            Assert.AreSame(staticFu, resolvedStaticFu);
            Assert.IsInstanceOfType(resolvedStaticFu.MyInnerFoo, typeof(Tofu));
        }

        class MockRegistrationProvider : ITypeRegistrationsProvider
        {
            public MockRegistrationProvider()
            {
            }

            #region ITypeRegistrationsProvider Members

            public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
            {

                yield return new TypeRegistration<IFoo>(() => new Foo())
                {
                    Lifetime = TypeRegistrationLifetime.Transient,
                    IsDefault = true
                };
                yield return new TypeRegistration<StaticFu>(
                    () => new StaticFu(Container.Resolved<ConfigurationChangeEventSource>())
                              {
                                  MyInnerFoo = Container.Resolved<IFoo>()
                              }) { IsDefault = true };

            }

            public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
            {
                yield return new TypeRegistration<IFoo>(
                    () => new Tofu()) { Lifetime = TypeRegistrationLifetime.Transient, IsDefault = true };
            }

            #endregion
        }

        public interface IFoo { }
        public class Foo : IFoo { }
        public class Tofu : IFoo { }
        public class StaticFu
        {
            public StaticFu(ConfigurationChangeEventSource configurationChangeEventSource)
            {
                configurationChangeEventSource.SourceChanged += OnConfigurationChanged;
            }

            public IFoo MyInnerFoo { get; set; }

            private void OnConfigurationChanged(object sender, ConfigurationSourceChangedEventArgs e)
            {
                MyInnerFoo = e.Container.GetInstance<IFoo>();
            }
        }
    }



}
