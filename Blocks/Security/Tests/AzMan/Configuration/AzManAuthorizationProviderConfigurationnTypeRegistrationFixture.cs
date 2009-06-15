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
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Tests.Configuration
{
    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithAzManProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            AzManAuthorizationProviderData azManProviderdata = new AzManAuthorizationProviderData();
            azManProviderdata.Name = "AzMan Provider";

            SecuritySettings settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(azManProviderdata);

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationForAzManProvider()
        {
            TypeRegistration typeRegistration = registrations.Where(x => x.ServiceType == typeof(IAuthorizationProvider)).FirstOrDefault();
            
            Assert.IsNotNull(typeRegistration);
            Assert.AreEqual(typeof(IAuthorizationProvider), typeRegistration.ServiceType);
        }

        [TestMethod]
        public void ThenImplementationTypeEqualsAzManAuthorizationProvider()
        {
            TypeRegistration typeRegistration = registrations.Where(x => x.ServiceType == typeof(IAuthorizationProvider)).First();
            Assert.AreEqual(typeof(AzManAuthorizationProvider), typeRegistration.ImplementationType);
        }

        [TestMethod]
        public void ThenIsDefaultEqualsFalse()
        {
            TypeRegistration typeRegistration = registrations.Where(x=>x.ServiceType == typeof(IAuthorizationProvider)).First();
            Assert.IsFalse(typeRegistration.IsDefault);
        }

        [TestMethod]
        public void ThenLifetimeIsTransient()
        {
            TypeRegistration typeRegistration = registrations.Where(x => x.ServiceType == typeof(IAuthorizationProvider)).First();
            Assert.AreEqual(TypeRegistrationLifetime.Transient, typeRegistration.Lifetime);
        }


        [TestMethod]
        public void ThenRegistrationHasCorrespondingName()
        {
            TypeRegistration typeRegistration = registrations.Where(x=>x.ServiceType == typeof(IAuthorizationProvider)).First();
            Assert.AreEqual("AzMan Provider", typeRegistration.Name);
        }

        [TestMethod]
        public void ThenHasRegistrationForCorrespondingInstrumentationProvider()
        {
            TypeRegistration typeRegistration = registrations.Where(x => x.ServiceType == typeof(IAuthorizationProviderInstrumentationProvider)).FirstOrDefault();
            Assert.IsNotNull(typeRegistration);
            Assert.AreEqual("AzMan Provider", typeRegistration.Name);
        }
    }

    [TestClass]
    public class GivenRegistrationForSecuritySettingsWithDefaultAzManProvider
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            AzManAuthorizationProviderData azManProviderdata = new AzManAuthorizationProviderData();
            azManProviderdata.Name = "AzMan Provider";

            SecuritySettings settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(azManProviderdata);

            settings.DefaultAuthorizationProviderName = "AzMan Provider";
            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenIsDefaultEqualsTrue()
        {
            var authorizationProviders = registrations.Where(x=>x.ServiceType == typeof(IAuthorizationProvider));
            Assert.AreEqual(1, authorizationProviders.Count());

            TypeRegistration typeRegistration = authorizationProviders.First();
            Assert.IsTrue(typeRegistration.IsDefault);
        }
    }
}
