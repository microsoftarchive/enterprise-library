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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_AddingCustomSymmetricCryptoProviderToConfigureCryptographyPassingNullName : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingCustomSymmetricProviderNamed_ThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed(null, typeof(CustomSymmetricProvider));
        }
    }

    [TestClass]
    public class When_AddingCustomSymmetricCryptoProviderToConfigureCryptographyPassingNullType : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_EncryptUsingCustomSymmetricProviderNamed_ThrowsArgumentNullException()
        {
            ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed("name", null);
        }
    }

    [TestClass]
    public class When_AddingCustomSymmetricCryptoProviderToConfigureCryptographyPassingWrongType : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingCustomSymmetricProviderNamed_ThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed("name", typeof(object));
        }
    }


    [TestClass]
    public class When_AddingCustomSymmetricCryptoProviderToConfigureCryptographyPassingNullAttributes : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_EncryptUsingCustomSymmetricProviderNamed_ThrowsArgumentNullException()
        {
            ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed("name", typeof(CustomSymmetricProvider), null);
        }
    }


    [TestClass]
    public class When_AddingCustomSymmetricProviderToConfigureCryptography : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        IEncryptUsingCustomSymmetricProviderNamed configureCustomSymmetricProvider;

        protected override void Act()
        {
            configureCustomSymmetricProvider = ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed("custom symmetric provider", typeof(CustomSymmetricProvider));
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderIsContainedInSymmetricProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual(typeof(CustomSymmetricProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual("custom symmetric provider", providerData.Name);
        }


        [TestMethod]
        public void Then_CanMakeCustomSymmetricProviderDefault()
        {
            configureCustomSymmetricProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom symmetric provider", cryptographySettings.DefaultSymmetricCryptoProviderName);
        }
    }

    [TestClass]
    public class When_AddingCustomSymmetricProviderToConfigureCryptographyGeneric : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        IEncryptUsingCustomSymmetricProviderNamed configureCustomSymmetricProvider;

        protected override void Act()
        {
            configureCustomSymmetricProvider = ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed<CustomSymmetricProvider>("custom symmetric provider");
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderIsContainedInSymmetricProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual(typeof(CustomSymmetricProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual("custom symmetric provider", providerData.Name);
        }


        [TestMethod]
        public void Then_CanMakeCustomSymmetricProviderDefault()
        {
            configureCustomSymmetricProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom symmetric provider", cryptographySettings.DefaultSymmetricCryptoProviderName);
        }
    }

    [TestClass]
    public class When_AddingCustomSymmetricProviderToConfigureCryptographyPassingAttributes : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        NameValueCollection attributes;
        IEncryptUsingCustomSymmetricProviderNamed configureCustomSymmetricProvider;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            configureCustomSymmetricProvider = ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed("custom Symmetric provider", typeof(CustomSymmetricProvider), attributes);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderIsContainedInSymmetricProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual(typeof(CustomSymmetricProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual("custom Symmetric provider", providerData.Name);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedAttributes()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual(attributes.Count, providerData.Attributes.Count);
            foreach (string attributeKey in attributes)
            {
                Assert.AreEqual(attributes[attributeKey], providerData.Attributes[attributeKey]);
            }
        }

        [TestMethod]
        public void Then_CanMakeCustomSymmetricProviderDefault()
        {
            configureCustomSymmetricProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom Symmetric provider", cryptographySettings.DefaultSymmetricCryptoProviderName);
        }
    }

    [TestClass]
    public class When_AddingCustomSymmetricProviderToConfigureCryptographyPassingAttributesGeneric : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        NameValueCollection attributes;
        IEncryptUsingCustomSymmetricProviderNamed configureCustomSymmetricProvider;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            configureCustomSymmetricProvider = ConfigureCryptography
                .EncryptUsingCustomSymmetricProviderNamed<CustomSymmetricProvider>("custom symmetric provider", attributes);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderIsContainedInSymmetricProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual(typeof(CustomSymmetricProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual("custom symmetric provider", providerData.Name);
        }

        [TestMethod]
        public void Then_CustomSymmetricProviderHasSpecifiedAttributes()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<CustomSymmetricCryptoProviderData>().First();
            Assert.AreEqual(attributes.Count, providerData.Attributes.Count);
            foreach (string attributeKey in attributes)
            {
                Assert.AreEqual(attributes[attributeKey], providerData.Attributes[attributeKey]);
            }
        }

        [TestMethod]
        public void Then_CanMakeCustomSymmetricProviderDefault()
        {
            configureCustomSymmetricProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom symmetric provider", cryptographySettings.DefaultSymmetricCryptoProviderName);
        }
    }
}
