//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Unity
{
    [TestClass]
    public class SymmetricStorageEncryptionProviderPolicyCreatorFixture
    {
        private const string providerName = "foo";
        private const string symmetricInstance = "instance";
        private IUnityContainer container;
        private CacheManagerSettings settings;
        private CryptographySettings cryptoSettings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            container = new UnityContainer();
            cryptoSettings = new CryptographySettings();
            settings = new CacheManagerSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, settings);
            configurationSource.Add(CryptographyConfigurationView.SectionName, cryptoSettings);

			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
		}

        [TestCleanup]
        public void TearDown()
        {
            container.Dispose();
        }

        [TestMethod]
        public void CanResolveCryptoProvider()
        {
            SymmetricStorageEncryptionProviderData encrytionProvider = new SymmetricStorageEncryptionProviderData(providerName, symmetricInstance);
            settings.EncryptionProviders.Add(encrytionProvider);

            cryptoSettings.SymmetricCryptoProviders.Add(new DpapiSymmetricCryptoProviderData(symmetricInstance, DataProtectionScope.CurrentUser));

            container.AddExtension(new CachingBlockExtension());
            container.AddExtension(new CryptographyBlockExtension());

            ISymmetricCryptoProvider provider = container.Resolve<ISymmetricCryptoProvider>(symmetricInstance);

            Assert.IsNotNull(provider);
            Assert.AreSame(typeof(DpapiSymmetricCryptoProvider), provider.GetType());
        }
    }
}