//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration
{
    [TestClass]
    public class GivenRegistrationForEmptySecuritySettings
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            SecuritySettings settings = new SecuritySettings();
            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHas0RegistrationsForAuthorizationProviders()
        {
            Assert.AreEqual(0, 
                registrations.Where(r => r.ServiceType == typeof(IAuthorizationProvider)).Count());
        }

        [TestMethod]
        public void ThenHas0RegistrationsForSecurityCacheProviders()
        {
            Assert.AreEqual(0,
                registrations.Where(r => r.ServiceType == typeof(ISecurityCacheProvider)).Count());
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithRuleProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            AuthorizationRuleProviderData ruleProvider = new AuthorizationRuleProviderData();
            ruleProvider.Name = "Rule Provider";

            SecuritySettings settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(ruleProvider);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationForAuthorizationProvider()
        {
            TypeRegistration registration = registrations.Where(x => x.ServiceType == typeof(IAuthorizationProvider)).FirstOrDefault();
            Assert.IsNotNull(registration);
        }


        [TestMethod]
        public void ThenHasRegistrationForInstrumentationProvider()
        {
            TypeRegistration registration = registrations.Where(x => x.ServiceType == typeof(IAuthorizationProviderInstrumentationProvider)).FirstOrDefault();
            Assert.IsNotNull(registration);
            Assert.AreEqual("Rule Provider", registration.Name);
        }

        [TestMethod]
        public void ThenRegistrationHasImplementationTypeOfRuleProvider()
        {
            TypeRegistration registration = registrations.Where(x => x.ServiceType == typeof(IAuthorizationProvider)).FirstOrDefault();
            Assert.IsNotNull(registration);

            Assert.AreEqual(typeof(AuthorizationRuleProvider), registration.ImplementationType);

        }

        [TestMethod]
        public void ThenRegistrationhasApropriateName()
        {
            TypeRegistration ruleProviderRegistration = registrations.Where(r => r.ServiceType == typeof(IAuthorizationProvider)).First();
            Assert.AreEqual("Rule Provider", ruleProviderRegistration.Name);
        }

        [TestMethod]
        public void ThenRegistrationHasIsDefaultSetToFalse()
        {
            TypeRegistration ruleProviderRegistration = registrations.Where(r => r.ServiceType == typeof(IAuthorizationProvider)).First();
            Assert.IsFalse(ruleProviderRegistration.IsDefault);
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithMultipleRuleProviders
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            AuthorizationRuleProviderData ruleProvider = new AuthorizationRuleProviderData();
            ruleProvider.Name = "Rule Provider";

            AuthorizationRuleProviderData otherRuleProvider = new AuthorizationRuleProviderData();
            otherRuleProvider.Name = "Other Rule Provider";

            SecuritySettings settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(ruleProvider);
            settings.AuthorizationProviders.Add(otherRuleProvider);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenRegistrationsContainMultipleAuthorizationProviderRegistrations()
        {
            int numberOfIAuthorizationProviders = registrations.Where(tr => tr.ServiceType == typeof(IAuthorizationProvider)).Count();

            Assert.AreEqual(2, numberOfIAuthorizationProviders);
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithDefaultProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            AuthorizationRuleProviderData ruleProvider = new AuthorizationRuleProviderData();
            ruleProvider.Name = "Rule Provider";

            SecuritySettings settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(ruleProvider);

            settings.DefaultAuthorizationProviderName = "Rule Provider";

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenRegistrationHasIsDefaultSetToTrue()
        {
            TypeRegistration ruleProviderRegistration = registrations.Where(r => r.ServiceType == typeof(IAuthorizationProvider) && r.Name == "Rule Provider").First();
            Assert.IsTrue(ruleProviderRegistration.IsDefault);
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithCustomAuthZProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CustomAuthorizationProviderData ruleProvider = new CustomAuthorizationProviderData();
            ruleProvider.Name = "Custom Auth Provider";
            ruleProvider.Type = typeof(MockCustomAuthorizationProvider);

            SecuritySettings settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(ruleProvider);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenRegistrationsConainRegistrationForCustomProvider()
        {
            TypeRegistration authProviderRegistration = registrations.Where(tr => tr.ServiceType == typeof(IAuthorizationProvider)).FirstOrDefault();
            
            Assert.IsNotNull(authProviderRegistration);
            Assert.AreEqual(typeof(MockCustomAuthorizationProvider), authProviderRegistration.ImplementationType);
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithCustomCacheProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CustomSecurityCacheProviderData securityCacheProviderData = new CustomSecurityCacheProviderData();
            securityCacheProviderData.Name = "Custom Cache Provider";
            securityCacheProviderData.Type = typeof(MockCustomSecurityCacheProvider);

            SecuritySettings settings = new SecuritySettings();
            settings.SecurityCacheProviders.Add(securityCacheProviderData);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenRegistrationsConainRegistrationForCustomProvider()
        {
            TypeRegistration securityCacheProvider = registrations.Where(tr => tr.ServiceType == typeof(ISecurityCacheProvider)).FirstOrDefault();

            Assert.IsNotNull(securityCacheProvider);
            Assert.AreEqual(typeof(MockCustomSecurityCacheProvider), securityCacheProvider.ImplementationType);
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithIncompatibleTypeInCustomSecurityCacheProvider
    {
        private SecuritySettings settings;

        [TestInitialize]
        public void Setup()
        {
            CustomSecurityCacheProviderData securityCacheProviderData = new CustomSecurityCacheProviderData();
            securityCacheProviderData.Name = "Custom Cache Provider";
            securityCacheProviderData.Type = typeof(FaultyType);

            settings = new SecuritySettings();
            settings.SecurityCacheProviders.Add(securityCacheProviderData);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThenGettingRegistrationsThrows()
        {
            settings.GetRegistrations(null).ToList();
        }


        private class FaultyType
        {
            public FaultyType(NameValueCollection nvc)
            {
            }
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithIncompatibleTypeInCustomAuthZProvider
    {
        private SecuritySettings settings;

        [TestInitialize]
        public void Setup()
        {
            CustomAuthorizationProviderData customAuthZProvider = new CustomAuthorizationProviderData();
            customAuthZProvider.Name = "Custom authZ Provider";
            customAuthZProvider.Type = typeof(FaultyType);

            settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(customAuthZProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThenGettingRegistrationsThrows()
        {
            settings.GetRegistrations(null).ToList();
        }


        private class FaultyType
        {
            public FaultyType(NameValueCollection nvc)
            {
            }
        }
    }
}
