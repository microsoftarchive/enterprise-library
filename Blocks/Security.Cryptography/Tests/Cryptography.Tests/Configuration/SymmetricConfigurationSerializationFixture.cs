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

using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestClass]
    public class SymmetricConfigurationSerializationFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            byte[] key = new byte[] { 1, 2, 3, 4 };
            string name1 = "name1";
            string name2 = "name2";
            string name3 = "name3";

            CryptographySettings settings = new CryptographySettings();

            settings.DefaultSymmetricCryptoProviderName = name1;
            settings.SymmetricCryptoProviders.Add(new DpapiSymmetricCryptoProviderData(name1, DataProtectionScope.CurrentUser));
            settings.SymmetricCryptoProviders.Add(new SymmetricAlgorithmProviderData(name2, typeof(RijndaelManaged), "ProtectedKey.file", DataProtectionScope.CurrentUser));
            settings.SymmetricCryptoProviders.Add(new CustomSymmetricCryptoProviderData(name3, typeof(MockCustomSymmetricProvider)));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[CryptographySettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            CryptographySettings roSettings = (CryptographySettings)configurationSource.GetSection(CryptographySettings.SectionName);

            Assert.IsNotNull(roSettings);
            Assert.AreEqual(name1, roSettings.DefaultSymmetricCryptoProviderName);
            Assert.AreEqual(3, roSettings.SymmetricCryptoProviders.Count);

            Assert.IsNotNull(roSettings.SymmetricCryptoProviders.Get(name1));
            Assert.AreSame(typeof(DpapiSymmetricCryptoProviderData), roSettings.SymmetricCryptoProviders.Get(name1).GetType());
            Assert.AreSame(typeof(DpapiSymmetricCryptoProvider), roSettings.SymmetricCryptoProviders.Get(name1).Type);
            Assert.AreEqual(DataProtectionScope.CurrentUser, ((DpapiSymmetricCryptoProviderData)roSettings.SymmetricCryptoProviders.Get(name1)).Scope);

            Assert.IsNotNull(roSettings.SymmetricCryptoProviders.Get(name2));
            Assert.AreSame(typeof(SymmetricAlgorithmProviderData), roSettings.SymmetricCryptoProviders.Get(name2).GetType());
            Assert.AreSame(typeof(SymmetricAlgorithmProvider), roSettings.SymmetricCryptoProviders.Get(name2).Type);
            Assert.AreSame(typeof(RijndaelManaged), ((SymmetricAlgorithmProviderData)roSettings.SymmetricCryptoProviders.Get(name2)).AlgorithmType);
            Assert.AreEqual("ProtectedKey.file", ((SymmetricAlgorithmProviderData)roSettings.SymmetricCryptoProviders.Get(name2)).ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, ((SymmetricAlgorithmProviderData)roSettings.SymmetricCryptoProviders.Get(name2)).ProtectedKeyProtectionScope);
            Assert.IsNotNull(roSettings.SymmetricCryptoProviders.Get(name3));
            Assert.AreSame(typeof(CustomSymmetricCryptoProviderData), roSettings.SymmetricCryptoProviders.Get(name3).GetType());
            Assert.AreSame(typeof(MockCustomSymmetricProvider), roSettings.SymmetricCryptoProviders.Get(name3).Type);
        }
    }
}
