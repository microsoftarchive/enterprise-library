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
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestClass]
    public class HashConfigurationSerializationFixture
    {
        const string keyedHashKeyFile = "KeyedHashKey.file";

        [TestInitialize]
        public void CreateKeyFile()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            CreateKeyFile(keyedHashKeyFile);
        }

        [TestCleanup]
        public void DeleteKeyFile()
        {
            File.Delete(keyedHashKeyFile);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string name1 = "name1";
            string name2 = "name2";
            string name3 = "name3";

            CryptographySettings settings = new CryptographySettings();

            settings.DefaultHashProviderName = name1;
            settings.HashProviders.Add(new HashAlgorithmProviderData(name1, typeof(SHA1Managed), true));
            settings.HashProviders.Add(new KeyedHashAlgorithmProviderData(name2, typeof(HMACSHA1), false, keyedHashKeyFile, DataProtectionScope.CurrentUser));
            settings.HashProviders.Add(new CustomHashProviderData(name3, typeof(MockCustomHashProvider)));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[CryptographyConfigurationView.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            CryptographySettings roSettings = (CryptographySettings)configurationSource.GetSection(CryptographyConfigurationView.SectionName);

            Assert.IsNotNull(roSettings);
            Assert.AreEqual(name1, roSettings.DefaultHashProviderName);
            Assert.AreEqual(3, roSettings.HashProviders.Count);

            Assert.IsNotNull(roSettings.HashProviders.Get(name1));
            Assert.AreSame(typeof(HashAlgorithmProviderData), roSettings.HashProviders.Get(name1).GetType());
            Assert.AreSame(typeof(HashAlgorithmProvider), roSettings.HashProviders.Get(name1).Type);
            Assert.AreSame(typeof(SHA1Managed), ((HashAlgorithmProviderData)roSettings.HashProviders.Get(name1)).AlgorithmType);
            Assert.AreEqual(true, ((HashAlgorithmProviderData)roSettings.HashProviders.Get(name1)).SaltEnabled);

            Assert.IsNotNull(roSettings.HashProviders.Get(name2));
            Assert.AreSame(typeof(KeyedHashAlgorithmProviderData), roSettings.HashProviders.Get(name2).GetType());
            Assert.AreSame(typeof(KeyedHashAlgorithmProvider), roSettings.HashProviders.Get(name2).Type);
            Assert.AreSame(typeof(HMACSHA1), ((KeyedHashAlgorithmProviderData)roSettings.HashProviders.Get(name2)).AlgorithmType);
            Assert.AreEqual(false, ((KeyedHashAlgorithmProviderData)roSettings.HashProviders.Get(name2)).SaltEnabled);
            Assert.AreEqual(keyedHashKeyFile, ((KeyedHashAlgorithmProviderData)roSettings.HashProviders.Get(name2)).ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, ((KeyedHashAlgorithmProviderData)roSettings.HashProviders.Get(name2)).ProtectedKeyProtectionScope);

            Assert.IsNotNull(roSettings.HashProviders.Get(name3));
            Assert.AreSame(typeof(CustomHashProviderData), roSettings.HashProviders.Get(name3).GetType());
            Assert.AreSame(typeof(MockCustomHashProvider), roSettings.HashProviders.Get(name3).Type);
        }

        void CreateKeyFile(string fileName)
        {
            ProtectedKey key = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.CurrentUser);
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                KeyManager.Write(stream, key);
            }
        }
    }
}
