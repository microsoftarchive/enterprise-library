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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration
{
    [TestClass]
    public class GivenConfigurationSectionWithNoProviderData
    {
        private CryptographySettings cryptographySettings;

        [TestInitialize]
        public void Given()
        {
            cryptographySettings = new CryptographySettings();
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesNoRegistrations()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            Assert.AreEqual(0, typeRegistrations.Count());
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
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedName()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("name", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(IHashProvider))
                .ForName("name")
                .ForImplementationType(typeof(HashAlgorithmProvider));

            registration.AssertConstructor()
                .WithValueConstructorParameter(typeof(RijndaelManaged))
                .WithValueConstructorParameter(true)
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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("keyed", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(IHashProvider))
                .ForName("keyed")
                .ForImplementationType(typeof(KeyedHashAlgorithmProvider));

            registration.AssertConstructor()
                .WithValueConstructorParameter(typeof(RijndaelManaged))
                .WithValueConstructorParameter(true)
                .WithValueConstructorParameter("protected key")
                .WithValueConstructorParameter(DataProtectionScope.LocalMachine)
                .VerifyConstructorParameters();
                
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithCustomHashProviderData
    {
        private CryptographySettings cryptographySettings;
        private CustomHashProviderData customHashProviderData;

        public GivenConfigurationSectionWithCustomHashProviderData()
        {
            
        }

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("custom", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(IHashProvider))
                .ForName("custom")
                .ForImplementationType(typeof(MockCustomHashProvider));

            registration.AssertConstructor()
                .WithValueConstructorParameter(customHashProviderData.Attributes)
                .VerifyConstructorParameters();
                
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithDefaultHashProvider
    {
        private CryptographySettings cryptographySettings;

        public GivenConfigurationSectionWithDefaultHashProvider()
        {
            
        }

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.IsTrue(registration.IsDefault);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithoutDefaultHashProvider
    {
        private CryptographySettings cryptographySettings;

        public GivenConfigurationSectionWithoutDefaultHashProvider()
        {            
        }

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.IsFalse(registration.IsDefault);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithSymmetricAlgorithmProviderData
    {
        private CryptographySettings cryptographySettings;

        public GivenConfigurationSectionWithSymmetricAlgorithmProviderData()
        {}

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("symmetric algorithm", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(ISymmetricCryptoProvider))
                .ForName("symmetric algorithm")
                .ForImplementationType(typeof(SymmetricAlgorithmProvider));

            registration.AssertConstructor()
                .WithValueConstructorParameter(typeof(RijndaelManaged))
                .WithValueConstructorParameter("protected key")
                .WithValueConstructorParameter(DataProtectionScope.LocalMachine)
                .VerifyConstructorParameters();
                
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithDpapiSymmetricProviderData
    {
        private CryptographySettings cryptographySettings;

        public GivenConfigurationSectionWithDpapiSymmetricProviderData()
        {
            
        }

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("dpapi", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(ISymmetricCryptoProvider))
                .ForName("dpapi")
                .ForImplementationType(typeof(DpapiSymmetricCryptoProvider));

            registration.AssertConstructor()
                .WithValueConstructorParameter(DataProtectionScope.LocalMachine)
                .VerifyConstructorParameters();
                
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithCustomCryptoProviderData
    {
        private CryptographySettings cryptographySettings;
        private CustomSymmetricCryptoProviderData customSymmetricCryptoProviderData;

        public GivenConfigurationSectionWithCustomCryptoProviderData()
        {
            
        }

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("custom", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(ISymmetricCryptoProvider))
                .ForName("custom")
                .ForImplementationType(typeof(MockCustomSymmetricProvider));

            registration.AssertConstructor()
                .WithValueConstructorParameter(customSymmetricCryptoProviderData.Attributes)
                .VerifyConstructorParameters();
                
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithDefaultCryptoProvider
    {
        private CryptographySettings cryptographySettings;

        public GivenConfigurationSectionWithDefaultCryptoProvider()
        {
            
        }

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.IsTrue(registration.IsDefault);
        }
    }

    [TestClass]
    public class GivenConfigurationSectionWithoutDefaultSymmetricProvider
    {
        private CryptographySettings cryptographySettings;

        public GivenConfigurationSectionWithoutDefaultSymmetricProvider()
        {
            
        }

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
            var typeRegistrations = cryptographySettings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);
            Assert.IsFalse(registration.IsDefault);
        }
    }
}
