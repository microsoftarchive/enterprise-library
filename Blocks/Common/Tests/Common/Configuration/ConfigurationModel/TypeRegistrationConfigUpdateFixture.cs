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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel
{
    [TestClass]
    public class GivenAConfigSectionTypeRegistrationProvider : ConfigSectionTypeRegistrationContext
    {
        [TestInitialize]
        public void Given()
        {
            Context("MockSection");
            RaiseChangeEvent("MockSection");
        }

        [TestMethod]
        public void WhenTheConfiguratorChanges_ThenUpdatedRegistrationsAreRetrieved()
        {
            Assert.IsTrue(section.UpdatedRegistrationsWasCalled);
        }

        [TestMethod]
        public void WhenTheConfiguratorChanges_ThenUpdatedRegistrationsAreReturnedInEventArgs()
        {
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(typeof(DummyClass1), registrations[0].ServiceType);
        }
    }

    [TestClass]
    public class GivenAConfigSectionChangeOnADifferentSection : ConfigSectionTypeRegistrationContext
    {
        [TestInitialize]
        public void Given()
        {
            Context("NotMySection");
            RaiseChangeEvent("MockSection");
        }
        [TestMethod]
        public void WhenTheConfiguratorChanges_ThenUpdatedRegistrationsAreNotRetrieved()
        {
            Assert.IsFalse(section.UpdatedRegistrationsWasCalled);
        }

        [TestMethod]
        public void WhenTheConfiguratorChanges_ThenUpdatedRegistrationsAreNotReturnedInEventArgs()
        {
            Assert.AreEqual(0, registrations.Count);
        }
    }

    [TestClass]
    public class GivenTypeLoadingLocator
    {
        private List<TypeRegistration> registrations;

        [TestInitialize]
        public void Given()
        {
            registrations = new List<TypeRegistration>();

            var configurator = new Mock<IContainerReconfiguringEventSource>();

            new TypeLoadingLocator(typeof (MockRegistrationProvidingSection).AssemblyQualifiedName, configurator.Object);

            var eventArgs = new Mock<ContainerReconfiguringEventArgs>(new DictionaryConfigurationSource(), new [] { "NotUsed" });
            eventArgs.Setup(e => e.AddTypeRegistrations(It.IsAny<IEnumerable<TypeRegistration>>()))
                .Callback<IEnumerable<TypeRegistration>>(regs => registrations.AddRange(regs));

            configurator.Raise(c => c.ContainerReconfiguring += null, eventArgs.Object);
            
        }

        [TestMethod]
        public void WhenTheConfiguratorChanges_ThenUpdatedRegistrationsAreReturnedInEventArgs()
        {
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(typeof(DummyClass1), registrations[0].ServiceType);
        }
    }



    public class ConfigSectionTypeRegistrationContext
    {
        internal List<TypeRegistration> registrations;
        internal MockRegistrationProvidingSection section;
        private Mock<IContainerReconfiguringEventSource> configurator;
        private DictionaryConfigurationSource configSource;

        internal void Context(string sectionName)
        {
            registrations = new List<TypeRegistration>();

            this.configurator = new Mock<IContainerReconfiguringEventSource>();

            this.configSource = new DictionaryConfigurationSource();
            section = new MockRegistrationProvidingSection();
            configSource.Add(sectionName, section);

            new ConfigSectionLocator(sectionName, configurator.Object);

        }

        internal void RaiseChangeEvent(string sectionName)
        {
            var eventArgs = new Mock<ContainerReconfiguringEventArgs>(configSource, new string[] { sectionName });
            eventArgs.Setup(e => e.AddTypeRegistrations(It.IsAny<IEnumerable<TypeRegistration>>()))
                .Callback<IEnumerable<TypeRegistration>>(regs => registrations.AddRange(regs));

            configurator.Raise(c => c.ContainerReconfiguring += null, eventArgs.Object);
        }
    }

    public class MockRegistrationProvidingSection : ConfigurationSection, ITypeRegistrationsProvider
    {
        public bool UpdatedRegistrationsWasCalled { get; private set; }

        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            UpdatedRegistrationsWasCalled = true;

            yield return new TypeRegistration<DummyClass1>(() => new DummyClass1());
        }
    }
}
