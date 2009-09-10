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
    public class When_AddingCustomHashProviderToConfigureCryptographyPassingNullName : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingCustomHashProviderNamed_ThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed(null, typeof(CustomHashProvider));
        }
    }

    [TestClass]
    public class When_AddingCustomHashProviderToConfigureCryptographyPassingNullType : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_EncryptUsingCustomHashProviderNamed_ThrowsArgumentNullException()
        {
            ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed("name", null);
        }
    }

    [TestClass]
    public class When_AddingCustomHashProviderToConfigureCryptographyPassingWrongType : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingCustomHashProviderNamed_ThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed("name", typeof(object));
        }
    }


    [TestClass]
    public class When_AddingCustomHashProviderToConfigureCryptographyPassingNullAttributes : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_EncryptUsingCustomHashProviderNamed_ThrowsArgumentNullException()
        {
            ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed("name", typeof(CustomHashProvider), null);
        }
    }

    [TestClass]
    public class When_AddingCustomHashProviderToConfigureCryptography : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        IEncryptUsingCustomHashProviderNamed configureCustomHashProvider;

        protected override void Act()
        {
            configureCustomHashProvider= ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed("custom hash provider", typeof(CustomHashProvider));
        }

        [TestMethod]
        public void Then_CustomHashProviderIsContainedInHashProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual(typeof(CustomHashProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual("custom hash provider", providerData.Name);
        }


        [TestMethod]
        public void Then_CanMakeCustomHashProviderDefault()
        {
            configureCustomHashProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom hash provider", cryptographySettings.DefaultHashProviderName);
        }
    }

    [TestClass]
    public class When_AddingCustomHashProviderToConfigureCryptographyGeneric : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        IEncryptUsingCustomHashProviderNamed configureCustomHashProvider;

        protected override void Act()
        {
            configureCustomHashProvider = ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed <CustomHashProvider>("custom hash provider");
        }

        [TestMethod]
        public void Then_CustomHashProviderIsContainedInHashProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual(typeof(CustomHashProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual("custom hash provider", providerData.Name);
        }


        [TestMethod]
        public void Then_CanMakeCustomHashProviderDefault()
        {
            configureCustomHashProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom hash provider", cryptographySettings.DefaultHashProviderName);
        }
    }

    [TestClass]
    public class When_AddingCustomHashProviderToConfigureCryptographyPassingAttributes : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        NameValueCollection attributes;
        IEncryptUsingCustomHashProviderNamed configureCustomHashProvider;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            configureCustomHashProvider = ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed("custom hash provider", typeof(CustomHashProvider), attributes);
        }

        [TestMethod]
        public void Then_CustomHashProviderIsContainedInHashProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual(typeof(CustomHashProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual("custom hash provider", providerData.Name);
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedAttributes()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual(attributes.Count, providerData.Attributes.Count);
            foreach (string attributeKey in attributes)
            {
                Assert.AreEqual(attributes[attributeKey], providerData.Attributes[attributeKey]);
            }
        }

        [TestMethod]
        public void Then_CanMakeCustomHashProviderDefault()
        {
            configureCustomHashProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom hash provider", cryptographySettings.DefaultHashProviderName);
        }
    }

    [TestClass]
    public class When_AddingCustomHashProviderToConfigureCryptographyPassingAttributesGeneric : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        NameValueCollection attributes;
        IEncryptUsingCustomHashProviderNamed configureCustomHashProvider;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            configureCustomHashProvider = ConfigureCryptography
                .EncryptUsingCustomHashProviderNamed <CustomHashProvider>("custom hash provider", attributes);
        }

        [TestMethod]
        public void Then_CustomHashProviderIsContainedInHashProviders()
        {
            Assert.IsTrue(base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().Any());
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedType()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual(typeof(CustomHashProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual("custom hash provider", providerData.Name);
        }

        [TestMethod]
        public void Then_CustomHashProviderHasSpecifiedAttributes()
        {
            var providerData = base.GetCryptographySettings().HashProviders.OfType<CustomHashProviderData>().First();
            Assert.AreEqual(attributes.Count, providerData.Attributes.Count);
            foreach (string attributeKey in attributes)
            {
                Assert.AreEqual(attributes[attributeKey], providerData.Attributes[attributeKey]);
            }
        }

        [TestMethod]
        public void Then_CanMakeCustomHashProviderDefault()
        {
            configureCustomHashProvider.SetAsDefault();

            var cryptographySettings = GetCryptographySettings();
            Assert.AreEqual("custom hash provider", cryptographySettings.DefaultHashProviderName);
        }
    }
}
