//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.FakeObjects;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    /// <summary>
    /// Test fixture verifying proper creation of injectors as specified in configuration.
    /// </summary>
    [TestClass]
    public class InjectorConfigurationFixture
    {
        [TestMethod]
        public void ShouldGetDefaultInjectorWhenNoneAreConfigured()
        {
            IConfigurationSource configSource = CreateConfigSource(CreateDefaultSettings());
            PolicyInjectorFactory factory = new PolicyInjectorFactory(configSource);
            PolicyInjector injector = factory.Create();
            Assert.IsNotNull(injector);
            Assert.AreEqual(typeof(RemotingPolicyInjector), injector.GetType());
        }

        [TestMethod]
        public void ShouldGetConfiguredDefaultWhenThereIsOne()
        {
            IConfigurationSource configSource = CreateConfigSource(CreateFakeInjectorSettings());
            PolicyInjectorFactory factory = new PolicyInjectorFactory(configSource);
            PolicyInjector injector = factory.Create();
            Assert.IsNotNull(injector);
            Assert.AreEqual(typeof(FakeInjector), injector.GetType());
        }

        [TestMethod]
        public void ShouldRetrieveInjectorByName()
        {
            IConfigurationSource configSource = CreateConfigSource(CreateMultipleInjectorSettings());
            PolicyInjectorFactory factory = new PolicyInjectorFactory(configSource);

            PolicyInjector fakeInjector = factory.Create("Fake");
            PolicyInjector remotingInjector = factory.Create("Remoting");

            Assert.IsNotNull(fakeInjector);
            Assert.AreEqual(typeof(FakeInjector), fakeInjector.GetType());
            Assert.IsNotNull(remotingInjector);
            Assert.AreEqual(typeof(RemotingPolicyInjector), remotingInjector.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(BuildFailedException))]
        public void ShouldThrowIfRequestingNonConfiguredInjector()
        {
            IConfigurationSource configSource = CreateConfigSource(CreateDefaultSettings());
            PolicyInjectorFactory factory = new PolicyInjectorFactory(configSource);
            factory.Create("No such injector");
        }

        [TestMethod]
        public void ShouldProperlyConfigureInjectorWithExtraConfigData()
        {
            IConfigurationSource configSource =
                CreateConfigSource(CreateConfiguredFakeInjectorSettings());
            PolicyInjectorFactory factory = new PolicyInjectorFactory(configSource);
            FakeInjector injector = (FakeInjector)factory.Create();

            Assert.AreEqual(42, injector.ExtraValue);
        }

        PolicyInjectionSettings CreateDefaultSettings()
        {
            return new PolicyInjectionSettings();
        }

        PolicyInjectionSettings CreateFakeInjectorSettings()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            FakeInjectorData injectorData = new FakeInjectorData("Fake Injector");
            settings.Injectors.Add(injectorData);
            settings.Injectors.DefaultInjector = "Fake Injector";
            return settings;
        }

        PolicyInjectionSettings CreateMultipleInjectorSettings()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            RemotingInjectorData remotingData = new RemotingInjectorData("Remoting");
            FakeInjectorData fakeData = new FakeInjectorData("Fake");
            settings.Injectors.Add(remotingData);
            settings.Injectors.Add(fakeData);
            settings.Injectors.DefaultInjector = remotingData.Name;
            return settings;
        }

        PolicyInjectionSettings CreateConfiguredFakeInjectorSettings()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            FakeInjectorData injectorData = new FakeInjectorData("Fake");
            injectorData.ExtraValue = 42;
            settings.Injectors.Add(injectorData);
            settings.Injectors.DefaultInjector = injectorData.Name;
            return settings;
        }

        IConfigurationSource CreateConfigSource(PolicyInjectionSettings settings)
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            configSource.Add(PolicyInjectionSettings.SectionName, settings);
            return configSource;
        }
    }
}