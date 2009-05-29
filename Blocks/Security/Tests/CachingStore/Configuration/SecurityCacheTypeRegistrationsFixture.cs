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
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests.Configuration
{
    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithCacheStoreProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CachingStoreProviderData securityCacheProvider = new CachingStoreProviderData();
            securityCacheProvider.Name = "Caching Store Provider";

            SecuritySettings settings = new SecuritySettings();
            settings.SecurityCacheProviders.Add(securityCacheProvider);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationForCacheProvider()
        {
            var securityCacheProviderRegistrations = registrations.Where(x => x.ServiceType == typeof(ISecurityCacheProvider));
            Assert.AreEqual(1, securityCacheProviderRegistrations.Count());

            TypeRegistration typeRegistration = securityCacheProviderRegistrations.First();
            Assert.AreEqual(typeof(ISecurityCacheProvider), typeRegistration.ServiceType);
        }


        [TestMethod]
        public void ThenHasRegistrationForCorrespondingInstrumentationProvider()
        {
            var securityCacheProviderRegistrations = registrations.Where(x => x.ServiceType == typeof(ISecurityCacheProviderInstrumentationProvider));
            Assert.AreEqual(1, securityCacheProviderRegistrations.Count());

            TypeRegistration typeRegistration = securityCacheProviderRegistrations.First();
            Assert.AreEqual("Caching Store Provider", typeRegistration.Name);
        }

        [TestMethod]
        public void ThenImplementationTypeEqualsCacheStoreProvider()
        {
            TypeRegistration typeRegistration = registrations.Where(x => x.ServiceType == typeof(ISecurityCacheProvider)).First();
            Assert.AreEqual(typeof(CachingStoreProvider), typeRegistration.ImplementationType);
        }

        [TestMethod]
        public void ThenIsDefaultEqualsFalse()
        {
            TypeRegistration typeRegistration = registrations.Where(x => x.ServiceType == typeof(ISecurityCacheProvider)).First();
            Assert.IsFalse(typeRegistration.IsDefault);
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithDefaultCacheStoreProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CachingStoreProviderData securityCacheProvider = new CachingStoreProviderData();
            securityCacheProvider.Name = "Caching Store Provider";

            SecuritySettings settings = new SecuritySettings();
            settings.SecurityCacheProviders.Add(securityCacheProvider);
            settings.DefaultSecurityCacheProviderName = "Caching Store Provider";

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenIsDefaultEqualsTrue()
        {
            var securityCacheProviderRegistrations = registrations.Where(x=> x.ServiceType == typeof(ISecurityCacheProvider));
            Assert.AreEqual(1, securityCacheProviderRegistrations.Count());

            TypeRegistration typeRegistration = securityCacheProviderRegistrations.First();
            Assert.IsTrue(typeRegistration.IsDefault);
        }
    }
}
