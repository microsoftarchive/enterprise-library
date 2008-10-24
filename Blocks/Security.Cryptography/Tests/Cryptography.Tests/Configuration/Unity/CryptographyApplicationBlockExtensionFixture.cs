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

using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Unity
{
    [TestClass]
    public class CryptographyApplicationBlockExtensionFixture
    {
        private IUnityContainer container;
        private CryptographySettings settings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            container = new UnityContainer();

            settings = new CryptographySettings();

            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CryptographyConfigurationView.SectionName, settings);

            configurationSource.Add(InstrumentationConfigurationSection.SectionName,
                new InstrumentationConfigurationSection(false, false, true));

            container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
        }

        [TestCleanup]
        public void TearDown()
        {
            container.Dispose();
        }

        [TestMethod]
        public void CanCreateCustomHashAlgorithmProvider()
        {
            CustomHashProviderData data
                = new CustomHashProviderData("provider", typeof(MockCustomHashProvider).AssemblyQualifiedName);
            data.Attributes.Add(MockCustomHashProvider.AttributeKey, "value");
            settings.HashProviders.Add(data);

            container.AddExtension(new CryptographyBlockExtension());

            IHashProvider provider = container.Resolve<IHashProvider>("provider");

            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(MockCustomHashProvider));
            Assert.AreEqual("value", ((MockCustomHashProvider)provider).customValue);
        }

        [TestMethod]
        public void CanCreateDefaultCustomHashAlgorithmProvider()
        {
            CustomHashProviderData data1
                = new CustomHashProviderData("provider1", typeof(MockCustomHashProvider).AssemblyQualifiedName);
            data1.Attributes.Add(MockCustomHashProvider.AttributeKey, "value1");
            settings.HashProviders.Add(data1);

            CustomHashProviderData data2
                = new CustomHashProviderData("provider2", typeof(MockCustomHashProvider).AssemblyQualifiedName);
            data2.Attributes.Add(MockCustomHashProvider.AttributeKey, "value2");
            settings.HashProviders.Add(data2);

            settings.DefaultHashProviderName = "provider1";

            container.AddExtension(new CryptographyBlockExtension());

            IHashProvider provider = container.Resolve<IHashProvider>("provider1");

            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(MockCustomHashProvider));
            Assert.AreEqual("value1", ((MockCustomHashProvider)provider).customValue);
        }

        [TestMethod]
        public void CanCreateCustomSymmetricEncryptionProvider()
        {
            CustomSymmetricCryptoProviderData data
                = new CustomSymmetricCryptoProviderData("provider", typeof(MockCustomSymmetricProvider).AssemblyQualifiedName);
            data.Attributes.Add(MockCustomSymmetricProvider.AttributeKey, "value");
            settings.SymmetricCryptoProviders.Add(data);

            container.AddExtension(new CryptographyBlockExtension());

            ISymmetricCryptoProvider provider = container.Resolve<ISymmetricCryptoProvider>("provider");

            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(MockCustomSymmetricProvider));
            Assert.AreEqual("value", ((MockCustomSymmetricProvider)provider).customValue);
        }

        [TestMethod]
        public void CanCreateDefaultCustomSymmetricEncryptionProvider()
        {
            CustomSymmetricCryptoProviderData data1
                = new CustomSymmetricCryptoProviderData("provider1", typeof(MockCustomSymmetricProvider).AssemblyQualifiedName);
            data1.Attributes.Add(MockCustomSymmetricProvider.AttributeKey, "value1");
            settings.SymmetricCryptoProviders.Add(data1);

            CustomSymmetricCryptoProviderData data2
                = new CustomSymmetricCryptoProviderData("provider2", typeof(MockCustomSymmetricProvider).AssemblyQualifiedName);
            data2.Attributes.Add(MockCustomSymmetricProvider.AttributeKey, "value2");
            settings.SymmetricCryptoProviders.Add(data2);

            settings.DefaultSymmetricCryptoProviderName = "provider1";

            container.AddExtension(new CryptographyBlockExtension());

            ISymmetricCryptoProvider provider = container.Resolve<ISymmetricCryptoProvider>("provider1");

            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(MockCustomSymmetricProvider));
            Assert.AreEqual("value1", ((MockCustomSymmetricProvider)provider).customValue);
        }

        [TestMethod]
        public void CryptographyManagerGetsInstrumented()
        {
            container.AddExtension(new CryptographyBlockExtension());

            CryptographyManager manager = container.Resolve<CryptographyManager>();
            Assert.IsNotNull(manager);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                try
                {
                    manager.CreateHash("foo", "");
                }
                catch (ConfigurationErrorsException)
                {
                    eventListener.WaitForEvents();
                    Assert.AreEqual(1, eventListener.EventsReceived.Count);
                    Assert.AreEqual("foo", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
                }
            }
        }
    }
}
