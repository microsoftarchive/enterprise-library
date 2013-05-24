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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class PolicyInjectionSettingsFixture
    {
        [TestMethod]
        public void SkipsInjectorsElement()
        {
            using (var source = new FileConfigurationSource("OldStyle.config", false))
            {
                PolicyInjectionSettings settings
                    = (PolicyInjectionSettings)source.GetSection(PolicyInjectionSettings.SectionName);

                Assert.IsNotNull(settings);
                Assert.AreEqual(3, settings.Policies.Count);
            }
        }
    }

    [TestClass]
    public class GivenAnEmptySection
    {
        private PolicyInjectionSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new PolicyInjectionSettings();
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenConfiguresNoPolicies()
        {
            using (var container = new UnityContainer())
            {
                this.settings.ConfigureContainer(container);

                Assert.AreEqual(0, container.Registrations.Where(r => r.RegisteredType.Assembly != typeof(IUnityContainer).Assembly).Count());
            }
        }
    }

    [TestClass]
    public class GivenASectionWithAnEmptyPolicy
    {
        private PolicyInjectionSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("policy 1") { });
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenConfiguresEmptyPolicy()
        {
            using (var container = new UnityContainer())
            {
                this.settings.ConfigureContainer(container);

                Assert.AreEqual(1, container.Registrations.Where(r => r.RegisteredType.Assembly != typeof(IUnityContainer).Assembly).Count());
                Assert.AreEqual("policy 1", container.Registrations.Single(r => r.RegisteredType == typeof(InjectionPolicy)).Name);
            }
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithoutPolicySettings
    {
        private IConfigurationSource source;

        [TestInitialize]
        public void TestInitialize()
        {
            this.source = new DictionaryConfigurationSource();
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenConfiguresNoPolicies()
        {
            using (var container = new UnityContainer())
            {
                PolicyInjectionSettings.ConfigureContainer(container, this.source);

                Assert.AreEqual(0, container.Registrations.Where(r => r.RegisteredType.Assembly != typeof(IUnityContainer).Assembly).Count());
            }
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithPolicySettingsWithAnEmptyPolicy
    {
        private IConfigurationSource source;

        [TestInitialize]
        public void TestInitialize()
        {
            this.source = new DictionaryConfigurationSource();
            var settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("policy 1") { });

            this.source.Add(PolicyInjectionSettings.SectionName, settings);
        }

        [TestMethod]
        public void WhenConfiguringContainer_ThenConfiguresEmptyPolicy()
        {
            using (var container = new UnityContainer())
            {
                PolicyInjectionSettings.ConfigureContainer(container, this.source);

                Assert.AreEqual(1, container.Registrations.Where(r => r.RegisteredType.Assembly != typeof(IUnityContainer).Assembly).Count());
                Assert.AreEqual("policy 1", container.Registrations.Single(r => r.RegisteredType == typeof(InjectionPolicy)).Name);
            }
        }
    }
}
