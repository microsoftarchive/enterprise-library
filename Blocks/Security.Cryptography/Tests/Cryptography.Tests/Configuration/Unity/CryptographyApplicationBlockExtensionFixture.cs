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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Unity
{
    [TestClass]
    public class CryptographyApplicationBlockExtensionFixture
    {
        private CryptographySettings settings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            settings = new CryptographySettings();

            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CryptographySettings.SectionName, settings);

            configurationSource.Add(InstrumentationConfigurationSection.SectionName,
                new InstrumentationConfigurationSection(false, false));
        }

        private IUnityContainer CreateContainer()
        {
            return new UnityContainer()
                .AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
        }

        [TestMethod]
        public void CanCreateCustomHashAlgorithmProvider()
        {
            CustomHashProviderData data
                = new CustomHashProviderData("provider", typeof(MockCustomHashProvider).AssemblyQualifiedName);
            data.Attributes.Add(MockCustomHashProvider.AttributeKey, "value");
            settings.HashProviders.Add(data);

            using (var container = CreateContainer())
            {
                IHashProvider provider = container.Resolve<IHashProvider>("provider");

                Assert.IsNotNull(provider);
                Assert.IsInstanceOfType(provider, typeof(MockCustomHashProvider));
                Assert.AreEqual("value", ((MockCustomHashProvider)provider).customValue);
            }
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

            using (var container = CreateContainer())
            {
                IHashProvider provider = container.Resolve<IHashProvider>();

                Assert.IsNotNull(provider);
                Assert.IsInstanceOfType(provider, typeof(MockCustomHashProvider));
                Assert.AreEqual("value1", ((MockCustomHashProvider)provider).customValue);
            }
        }

        [TestMethod]
        public void CanCreateCustomSymmetricEncryptionProvider()
        {
            CustomSymmetricCryptoProviderData data
                = new CustomSymmetricCryptoProviderData("provider", typeof(MockCustomSymmetricProvider).AssemblyQualifiedName);
            data.Attributes.Add(MockCustomSymmetricProvider.AttributeKey, "value");
            settings.SymmetricCryptoProviders.Add(data);

            using (var container = CreateContainer())
            {
                ISymmetricCryptoProvider provider = container.Resolve<ISymmetricCryptoProvider>("provider");

                Assert.IsNotNull(provider);
                Assert.IsInstanceOfType(provider, typeof(MockCustomSymmetricProvider));
                Assert.AreEqual("value", ((MockCustomSymmetricProvider)provider).customValue);
            }
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

            using (var container = CreateContainer())
            {
                ISymmetricCryptoProvider provider = container.Resolve<ISymmetricCryptoProvider>();

                Assert.IsNotNull(provider);
                Assert.IsInstanceOfType(provider, typeof(MockCustomSymmetricProvider));
                Assert.AreEqual("value1", ((MockCustomSymmetricProvider)provider).customValue);
            }
        }

        [TestMethod]
        [Ignore]    // TODO replace with other instrumentation mechanism for tests?
        public void CryptographyManagerGetsInstrumented()
        {
            using (var container = CreateContainer())
            {
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
}
