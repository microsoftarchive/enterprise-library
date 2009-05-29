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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Unity
{
    [TestClass]
    public class SymmetricStorageEncryptionProviderInstantiationFixture
    {
        private const string providerName = "foo";
        private const string symmetricInstance = "instance";
        private CacheManagerSettings settings;
        private CryptographySettings cryptoSettings;
        private DictionaryConfigurationSource configurationSource;


        [TestInitialize]
        public void SetUp()
        {
            cryptoSettings = new CryptographySettings();
            settings = new CacheManagerSettings();

            SymmetricStorageEncryptionProviderData encrytionProvider = new SymmetricStorageEncryptionProviderData(providerName, symmetricInstance);
            settings.EncryptionProviders.Add(encrytionProvider);

            cryptoSettings.SymmetricCryptoProviders.Add(new DpapiSymmetricCryptoProviderData(symmetricInstance, DataProtectionScope.CurrentUser));

            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(CacheManagerSettings.SectionName, settings);
            configurationSource.Add(CryptographySettings.SectionName, cryptoSettings);

		}

        [TestCleanup]
        public void TearDown()
        {
        }

        [TestMethod]
        public void CanResolveCryptoProvider()
        {
            IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);
            ISymmetricCryptoProvider provider = container.GetInstance<ISymmetricCryptoProvider>(symmetricInstance);

            Assert.IsNotNull(provider);
            Assert.AreSame(typeof(DpapiSymmetricCryptoProvider), provider.GetType());
        }
    }
}
