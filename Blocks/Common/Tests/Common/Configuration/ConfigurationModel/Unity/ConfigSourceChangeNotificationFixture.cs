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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel.Unity
{
    [TestClass]
    public class GivenAConfigSource
    {
        private MockConfigSource configurationSource;
        private ITypeRegistrationsProvider mockRegistrationsProvider;
        private UnityContainerConfigurator configurator;
        private IUnityContainer container;

        [TestInitialize]
        public void Given()
        {
            configurationSource = new MockConfigSource();

            var mock = new Mock<ITypeRegistrationsProvider>();
            mock.Setup(p => p.GetRegistrations(It.IsAny<IConfigurationSource>()))
                .Returns(new TypeRegistration[0]);

            mockRegistrationsProvider = mock.Object;

            container = new UnityContainer();
            configurator = new UnityContainerConfigurator(container);
            configurator.RegisterAll(configurationSource, mockRegistrationsProvider);
        }

        [TestCleanup]
        public void TearDown()
        {
            container.Dispose();
            container = null;
        }

        [TestMethod]
        public void WhenConfiguratorReadsTypeRegistrations_ThenItRegistersForChangeNotification()
        {
            Assert.AreEqual(1, configurationSource.ChangeHandlersRegistered);
        }

        [TestMethod]
        public void WhenConfiguratorIsDisposed_ThenItUnregistersForChangeNotifications()
        {
            configurator.Dispose();

            Assert.AreEqual(0, configurationSource.ChangeHandlersRegistered);
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenConfigurationChangeEventSourceIsRegistered()
        {
            container.Resolve<ConfigurationChangeEventSource>();
            // If we get here without throwing, we're good.
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenAlwaysGetTheSameConfigurationChangeEventSourceInstance()
        {
            var s1 = container.Resolve<ConfigurationChangeEventSource>();
            var s2 = container.Resolve<ConfigurationChangeEventSource>();

            Assert.AreSame(s1, s2);
        }
    }

    [TestClass]
    public class GivenAConfigurationChangeEventSource
    {
        private ConfigurationChangeEventSourceImpl eventSource;
        private DictionaryConfigurationSource configurationSource;
        private const string testsSectionName = "TestSection";
        private ConfigurationSourceChangedEventArgs sourceChangedEventArgs;
        private SectionChangedEventArgs<TestsConfigurationSection> sectionChangedEventArgs;
        private SectionChangedEventArgs<AppSettingsSection> appSettingsChangedEventArgs;
        private IServiceLocator mockLocator;

        [TestInitialize]
        public void Given()
        {
            eventSource = new ConfigurationChangeEventSourceImpl();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(testsSectionName, new TestsConfigurationSection());
            mockLocator = new Mock<IServiceLocator>().Object;

            eventSource.SourceChanged += (sender, e) => { sourceChangedEventArgs = e; };

            eventSource.GetSection<TestsConfigurationSection>().SectionChanged +=
                (sender, e) => { sectionChangedEventArgs = e; };

            eventSource.GetSection<AppSettingsSection>().SectionChanged +=
                (sender, e) => { appSettingsChangedEventArgs = e; };

            eventSource.ConfigurationSourceChanged(configurationSource, mockLocator, testsSectionName);
        }

        [TestMethod]
        public void WhenConfigSourceChangeIsSignaled_ThenConfigSourceAndChangedSectionsAreInEventArgs()
        {
            Assert.IsNotNull(sourceChangedEventArgs);
            Assert.AreSame(configurationSource, sourceChangedEventArgs.ConfigurationSource);
            CollectionAssert.AreEqual(new[] { testsSectionName }, sourceChangedEventArgs.ChangedSectionNames);
        }

        [TestMethod]
        public void WhenConfigSourceChangeIsSignalled_ThenServiceLocatorIsIncludedInEventArgs()
        {
            Assert.AreSame(mockLocator, sourceChangedEventArgs.Container);
        }

        [TestMethod]
        public void WhenConfigSourceChangeIsSignaled_ThenSectionChangedIsInEventArgs()
        {
            Assert.IsNotNull(sectionChangedEventArgs);
            Assert.AreSame(configurationSource.GetSection(testsSectionName), sectionChangedEventArgs.Section);
        }

        [TestMethod]
        public void WhenConfigSourceChangeIsSignaled_ThenServiceLocatorIsIncludedInSectionChangeEventArgs()
        {
            Assert.AreSame(mockLocator, sectionChangedEventArgs.Container);
        }

        [TestMethod]
        public void WhenConfigSourceChangeIsSignaled_ThenChangeEventsForSectionsNotInTheSourceAreNotRaised()
        {
            Assert.IsNull(appSettingsChangedEventArgs);
        }
    }

    class MockConfigSource : IConfigurationSource
    {
        public int ChangeHandlersRegistered { get; set; }

        public ConfigurationSection GetSection(string sectionName)
        {
            throw new NotImplementedException();
        }

        public void Add(string sectionName, ConfigurationSection configurationSection)
        {
            throw new NotImplementedException();
        }

        public void Remove(string sectionName)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged
        {
            add { ++ChangeHandlersRegistered; }
            remove { --ChangeHandlersRegistered; }
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
}
