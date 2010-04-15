//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration
{
    [TestClass]
    public class WhenGettingCryptoRegistations_GivenEmptyCryptoSettings : ArrangeActAssert
    {
        private CryptographySettings cryptographySettings;
        private IEnumerable<TypeRegistration> registrations;
        private TypeRegistration cryptoManagerRegistration;
        private TypeRegistration instrumentationRegistration;

        protected override void Arrange()
        {
            cryptographySettings = new CryptographySettings();
        }

        protected override void  Act()
        {
            registrations = cryptographySettings.GetRegistrations(null);
            cryptoManagerRegistration = registrations.Where(r => r.ServiceType == typeof(CryptographyManager)).First();
            instrumentationRegistration = registrations.Where(r => r.ServiceType == typeof(IDefaultCryptographyInstrumentationProvider)).First();
        }

        [TestMethod]
        public void ThenCreatesOneRegistrationForCryptographyManager()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null).Where(r => r.ServiceType == typeof(CryptographyManager));

            Assert.AreEqual(1, typeRegistrations.Count());
        }

        [TestMethod]
        public void ThenCryptographyManagerRegistrationIsDefault()
        {
            cryptoManagerRegistration.AssertForServiceType(typeof(CryptographyManager))
                .IsDefault()
                .ForImplementationType(typeof(CryptographyManagerImpl));
        }

        [TestMethod]
        public void ThenCryptographyManagerRegistrationIsPublic()
        {
            cryptoManagerRegistration.AssertForServiceType(typeof (CryptographyManager))
                .IsPublicName();
        }

        [TestMethod]
        public void ThenDefaultInstrumentationProviderRegistrationIsDefault()
        {
            instrumentationRegistration
                .AssertForServiceType(typeof(IDefaultCryptographyInstrumentationProvider))
                .ForImplementationType(typeof(DefaultCryptographyEventLogger))
                .IsDefault();
        }

        [TestMethod]
        public void ThenDefaultInstrumentationProviderIsNotPublicName()
        {
            instrumentationRegistration
                .AssertForServiceType(typeof (IDefaultCryptographyInstrumentationProvider))
                .IsNotPublicName();
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithHashAlgorithmProviderData
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            cryptographySettings.HashProviders.Add(
                new HashAlgorithmProviderData("name", typeof(RijndaelManaged), true));
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedNameAndOneForCryptographyManager()
        {
            var mangerRegistrations = cryptographySettings.GetRegistrations(null).Where(x=>x.ServiceType == typeof(CryptographyManager));
            Assert.AreEqual(1, mangerRegistrations.Count());

            var hashAlgorithmRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(IHashProvider));
            Assert.AreEqual(1, hashAlgorithmRegistrations.Count());
            Assert.AreEqual("name", hashAlgorithmRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenHashAlgorithmProviderHasTransientLifetime()
        {
            var hashAlgorithmRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(IHashProvider));
            Assert.AreEqual(1, hashAlgorithmRegistrations.Count());
            Assert.AreEqual(TypeRegistrationLifetime.Transient, hashAlgorithmRegistrations.ElementAt(0).Lifetime);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenHashAlgorithmInstrumentationProviderHasTransientLifetime()
        {
            var registration = cryptographySettings.GetRegistrations(null)
                .Where(x => x.ServiceType == typeof(IHashAlgorithmInstrumentationProvider))
                .First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.Where(x=>x.ServiceType == typeof(IHashProvider)).ElementAt(0);

            registration
                .AssertForServiceType(typeof(IHashProvider))
                .ForName("name")
                .ForImplementationType(typeof(HashAlgorithmProvider))
                .IsPublicName();

            registration.AssertConstructor()
                .WithValueConstructorParameter(typeof(RijndaelManaged))
                .WithValueConstructorParameter(true)
                .WithContainerResolvedParameter<IHashAlgorithmInstrumentationProvider>("name")
                .VerifyConstructorParameters();
                
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithKeyedHashAlgorithmProviderData
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            cryptographySettings.HashProviders.Add(
                new KeyedHashAlgorithmProviderData(
                    "keyed",
                    typeof(RijndaelManaged),
                    true,
                    "protected key",
                    DataProtectionScope.LocalMachine));
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedName()
        {
            var mangerRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(CryptographyManager));
            Assert.AreEqual(1, mangerRegistrations.Count());

            var hashAlgorithmRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(IHashProvider));
            Assert.AreEqual(1, hashAlgorithmRegistrations.Count());
            Assert.AreEqual("keyed", hashAlgorithmRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.Where(x=>x.ServiceType == typeof(IHashProvider)).ElementAt(0);

            registration
                .AssertForServiceType(typeof(IHashProvider))
                .ForName("keyed")
                .ForImplementationType(typeof(KeyedHashAlgorithmProvider))
                .IsPublicName();

            registration.AssertConstructor()
                .WithValueConstructorParameter(typeof(RijndaelManaged))
                .WithValueConstructorParameter(true)
                .WithValueConstructorParameter("protected key")
                .WithValueConstructorParameter(DataProtectionScope.LocalMachine)
                .WithContainerResolvedParameter<IHashAlgorithmInstrumentationProvider>("keyed")
                .VerifyConstructorParameters();
                
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationHasTransientLifetime()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);
            TypeRegistration registration = typeRegistrations.Where(x => x.ServiceType == typeof(IHashProvider)).ElementAt(0);

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithCustomHashProviderData
    {
        private CryptographySettings cryptographySettings;
        private CustomHashProviderData customHashProviderData;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            customHashProviderData = new CustomHashProviderData("custom", typeof(MockCustomHashProvider));
            customHashProviderData.Attributes["foo"] = "bar";
            cryptographySettings.HashProviders.Add(customHashProviderData);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedName()
        {
            var mangerRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(CryptographyManager));
            Assert.AreEqual(1, mangerRegistrations.Count());

            var hashAlgorithmRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(IHashProvider));
            Assert.AreEqual(1, hashAlgorithmRegistrations.Count());
            Assert.AreEqual("custom", hashAlgorithmRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCustomHashProviderShouldbeTransient()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(IHashProvider))
                .ForName("custom")
                .ForImplementationType(typeof(MockCustomHashProvider))
                .IsPublicName();

            registration.AssertConstructor()
                .WithValueConstructorParameter(customHashProviderData.Attributes)
                .VerifyConstructorParameters();
                
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithDefaultHashProvider
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings { DefaultHashProviderName = "name" };
            cryptographySettings.HashProviders.Add(
                new HashAlgorithmProviderData("name", typeof(RijndaelManaged), true));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenCreatesDefaultRegistration()
        {
            
            var mangerRegistrations = cryptographySettings.GetRegistrations(null).Where(x=>x.ServiceType == typeof(CryptographyManager));
            Assert.AreEqual(1, mangerRegistrations.Count());

            var registration = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(IHashProvider)).First();
            Assert.IsTrue(registration.IsDefault);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithoutDefaultHashProvider
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            cryptographySettings.HashProviders.Add(
                new HashAlgorithmProviderData("name", typeof(RijndaelManaged), true));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenCreatesNonDefaultRegistration()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.IsFalse(registration.IsDefault);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithSymmetricAlgorithmProviderData
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            cryptographySettings.SymmetricCryptoProviders.Add(
                new SymmetricAlgorithmProviderData(
                    "symmetric algorithm",
                    typeof(RijndaelManaged),
                    "protected key",
                    DataProtectionScope.LocalMachine));
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedName()
        {
            var mangerRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(CryptographyManager));
            Assert.AreEqual(1, mangerRegistrations.Count());

            var symmProviderRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(ISymmetricCryptoProvider));
            Assert.AreEqual(1, symmProviderRegistrations.Count());

            Assert.AreEqual("symmetric algorithm", symmProviderRegistrations.ElementAt(0).Name);
            var typeRegistrations = cryptographySettings.GetRegistrations(null);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null).Where(x=>x.ServiceType == typeof(ISymmetricCryptoProvider));

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(ISymmetricCryptoProvider))
                .ForName("symmetric algorithm")
                .ForImplementationType(typeof(SymmetricAlgorithmProvider))
                .IsPublicName();

            registration.AssertConstructor()
                .WithValueConstructorParameter(typeof(RijndaelManaged))
                .WithValueConstructorParameter("protected key")
                .WithValueConstructorParameter(DataProtectionScope.LocalMachine)
                .WithContainerResolvedParameter<ISymmetricAlgorithmInstrumentationProvider>("symmetric algorithm")
                .VerifyConstructorParameters();                
        }


        [TestMethod]
        public void WhenCreatingRegistrations_ThenSymmetricCryptoProviderHasTransientLifetime()
        {
            var registration = cryptographySettings.GetRegistrations(null)
                .Where(x => x.ServiceType == typeof(ISymmetricCryptoProvider))
                .First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenSymmetricCryptoInstrumentationProviderHasTransientLifetime()
        {
            var registration = cryptographySettings.GetRegistrations(null)
                .Where(x => x.ServiceType == typeof(ISymmetricAlgorithmInstrumentationProvider))
                .First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithDpapiSymmetricProviderData
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            cryptographySettings.SymmetricCryptoProviders.Add(
                new DpapiSymmetricCryptoProviderData("dpapi", DataProtectionScope.LocalMachine));
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedName()
        {
            var typeRegistrations =
                cryptographySettings.GetRegistrations(null).Where(
                    r => r.ServiceType == typeof (ISymmetricCryptoProvider));

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("dpapi", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(ISymmetricCryptoProvider))
                .ForName("dpapi")
                .ForImplementationType(typeof(DpapiSymmetricCryptoProvider))
                .IsPublicName();

            registration.AssertConstructor()
                .WithValueConstructorParameter(DataProtectionScope.LocalMachine)
                .VerifyConstructorParameters();
                
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenDpapiCryptoProviderHasTransientLifetime()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(ISymmetricCryptoProvider));

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.AreEqual(typeof(ISymmetricCryptoProvider), registration.ServiceType);
            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithCustomCryptoProviderData
    {
        private CryptographySettings cryptographySettings;
        private CustomSymmetricCryptoProviderData customSymmetricCryptoProviderData;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            customSymmetricCryptoProviderData =
                new CustomSymmetricCryptoProviderData("custom", typeof(MockCustomSymmetricProvider));
            customSymmetricCryptoProviderData.Attributes["foo"] = "bar";
            cryptographySettings.SymmetricCryptoProviders.Add(customSymmetricCryptoProviderData);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedName()
        {
            var typeRegistrations =
                cryptographySettings.GetRegistrations(null).Where(
                    r => r.ServiceType == typeof (ISymmetricCryptoProvider));

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("custom", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(ISymmetricCryptoProvider))
                .ForName("custom")
                .ForImplementationType(typeof(MockCustomSymmetricProvider))
                .IsPublicName();

            registration.AssertConstructor()
                .WithValueConstructorParameter(customSymmetricCryptoProviderData.Attributes)
                .VerifyConstructorParameters();           
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCustomCryptoProviderHasTransientLifetime()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null).Where(x => x.ServiceType == typeof(ISymmetricCryptoProvider));

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.AreEqual(typeof(ISymmetricCryptoProvider), registration.ServiceType);
            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithDefaultCryptoProvider
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings =
                new CryptographySettings
                    {
                        DefaultSymmetricCryptoProviderName = "name"

                    };
            cryptographySettings.SymmetricCryptoProviders.Add(
                new SymmetricAlgorithmProviderData(
                    "name", typeof(RijndaelManaged), "protected key", DataProtectionScope.CurrentUser));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenCreatesDefaultRegistration()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null).Where(x=>x.ServiceType == typeof(ISymmetricCryptoProvider));

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.IsTrue(registration.IsDefault);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithoutDefaultSymmetricProvider
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
            cryptographySettings.SymmetricCryptoProviders.Add(
                new SymmetricAlgorithmProviderData(
                    "name",
                    typeof(RijndaelManaged),
                    "protected key",
                    DataProtectionScope.LocalMachine));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenCreatesNonDefaultRegistration()
        {
            var typeRegistrations = cryptographySettings.GetRegistrations(null);

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.IsFalse(registration.IsDefault);
        }
    }
}
