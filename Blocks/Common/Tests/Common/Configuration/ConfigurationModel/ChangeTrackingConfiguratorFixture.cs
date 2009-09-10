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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel
{
    [TestClass]
    public class WhenContainerIsConfigured : GivenChangeTrackingConfigurator
    {
        [TestInitialize]
        public void Given()
        {
            Context();
        }

        [TestMethod]
        public void ThenConfiguratorRegistersForChangeNotificationsFromConfigSource()
        {
            Assert.AreEqual(1, configSource.ChangeHandlersRegistered);
        }

        [TestMethod]
        public void ThenTypeRegistrationProvidersCanRegisterForContainerReconfigureEvent()
        {
            Assert.AreEqual(2, configurator.EventHandlersRegistered);
        }
    }

    [TestClass]
    public class WhenConfigurationSourceChanges : GivenChangeTrackingConfigurator
    {
        [TestInitialize]
        public void Given()
        {
            Context();
            configSource.RaiseSourceChanged();
        }

        [TestMethod]
        public void ThenContainerReconfiguringEventIsRaisedFirst()
        {
            Assert.AreEqual(1, provider1ReconfiguredSequence);
            Assert.AreEqual(2, provider2ReconfiguredSequence);
        }

        [TestMethod]
        public void ThenUpdatedRegistrationsAreAppliedToContainer()
        {
            Assert.AreEqual(2, configurator.Registrations.Count());
            Assert.AreEqual(typeof(DummyClass3),
                configurator.Registrations[0].ServiceType);
            Assert.AreEqual(typeof(DummyClass1), configurator.Registrations[1].ServiceType);
        }

        [TestMethod]
        public void ThenConfigurationChangeEventSourceRaisesAfterContainerIsReconfigured()
        {
            Assert.IsNotNull(sourceChangedEventArgs);
        }

    }


    public class GivenChangeTrackingConfigurator
    {
        internal MockChangeTrackingConfigurator configurator = new MockChangeTrackingConfigurator();
        internal MockConfigSource configSource = new MockConfigSource();

        internal int currentEventSequence;
        internal int provider1ReconfiguredSequence;
        internal int provider2ReconfiguredSequence;

        internal ConfigurationSourceChangedEventArgs sourceChangedEventArgs;

        protected void Context()
        {
            currentEventSequence = 0;
            provider1ReconfiguredSequence = -1;
            provider2ReconfiguredSequence = -1;
            sourceChangedEventArgs = null;

            configurator = new MockChangeTrackingConfigurator();
            configSource = new MockConfigSource();

            var mockProvider1 = new Mock<ITypeRegistrationsProvider>();
            mockProvider1.Setup(p => p.GetRegistrations(It.IsAny<IConfigurationSource>()))
                .Returns(MakeRegistration<DummyClass1>());

            var mockProvider2 = new Mock<ITypeRegistrationsProvider>();
            mockProvider2.Setup(p => p.GetRegistrations(It.IsAny<IConfigurationSource>()))
                .Returns(MakeRegistration<DummyClass2>());

            var mockComposite = new Mock<ITypeRegistrationsProvider>();
            mockComposite.Setup(c => c.GetRegistrations(It.IsAny<IConfigurationSource>()))
                .Returns(
                    (IConfigurationSource cs) => mockProvider1.Object.GetRegistrations(cs)
                        .Concat(mockProvider2.Object.GetRegistrations(cs)));


            configurator.EventSource.SourceChanged += Configurator_OnConfigurationSourceChanged;
            configurator.ContainerReconfiguring += MockProvider1_OnContainerReconfiguring;
            configurator.ContainerReconfiguring += MockProvider2_OnContainerReconfiguring;

            configurator.RegisterAll(configSource, mockComposite.Object);
        }

        private void MockProvider1_OnContainerReconfiguring(object sender, ContainerReconfiguringEventArgs e)
        {
            provider1ReconfiguredSequence = ++currentEventSequence;
            e.AddTypeRegistrations(MakeRegistration<DummyClass3>());
        }

        private void MockProvider2_OnContainerReconfiguring(object sender, ContainerReconfiguringEventArgs e)
        {
            provider2ReconfiguredSequence = ++currentEventSequence;
            e.AddTypeRegistrations(MakeRegistration<DummyClass1>());
        }

        private void Configurator_OnConfigurationSourceChanged(object sender, ConfigurationSourceChangedEventArgs e)
        {
            sourceChangedEventArgs = e;
        }

        private static IEnumerable<TypeRegistration> MakeRegistration<T>()
            where T : new()
        {
            return new TypeRegistration[]
            {
                new TypeRegistration<T>(() => new T())
            };
        }
    }

    class MockChangeTrackingConfigurator : ChangeTrackingContainerConfigurator
    {
        public List<TypeRegistration> Registrations = new List<TypeRegistration>();

        public int EventHandlersRegistered { get; set; }

        public override event EventHandler<ContainerReconfiguringEventArgs> ContainerReconfiguring
        {
            add
            {
                ++EventHandlersRegistered;
                base.ContainerReconfiguring += value;
            }
            remove
            {
                --EventHandlersRegistered;
                base.ContainerReconfiguring -= value;
            }
        }

        public ConfigurationChangeEventSource EventSource { get { return base.ChangeEventSource; } }

        protected override void RegisterAllCore(IConfigurationSource configurationSource, ITypeRegistrationsProvider rootProvider)
        {
            rootProvider.GetRegistrations(configurationSource);
        }

        protected override void RegisterUpdates(IEnumerable<TypeRegistration> updatedRegistrations)
        {
            Registrations.AddRange(updatedRegistrations);
        }

        protected override IServiceLocator GetLocator()
        {
            return null; // Don't care about this for the purposes of this test.
        }
    }

    class MockConfigSource : IConfigurationSource
    {
        public int ChangeHandlersRegistered { get; set; }

        public ConfigurationSection GetSection(string sectionName)
        {
            return null;
        }

        public void Add(string sectionName, ConfigurationSection configurationSection)
        {
            throw new NotImplementedException();
        }

        public void Remove(string sectionName)
        {
            throw new NotImplementedException();
        }

        private EventHandler<ConfigurationSourceChangedEventArgs> sourceChanged;

        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged
        {
            add
            {
                ++ChangeHandlersRegistered;
                sourceChanged += value;
            }
            remove
            {
                --ChangeHandlersRegistered;
                sourceChanged -= value;
            }
        }

        public void RaiseSourceChanged()
        {
            sourceChanged(this, new ConfigurationSourceChangedEventArgs(this, new[] { "Dummy1", "Dummy2" }));
        }

        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    // Test classes for checking out configuration
    class DummyClass1 { }
    class DummyClass2 { }
    class DummyClass3 { }
}
