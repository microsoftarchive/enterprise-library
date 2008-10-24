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
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class HashProviderFactoryFixture
    {
        const string providerName = "hashAlgorithm1";
        const string hashAlgorithm = "hashAlgorithm2";
        const string keyedHashKeyFile = "KeyedHashKey.file";

        [TestInitialize]
        public void CreateKeyFile()
        {
            CreateKeyFile(keyedHashKeyFile);
        }

        [TestCleanup]
        public void DeleteKeyFile()
        {
            File.Delete(keyedHashKeyFile);
        }

        [TestMethod]
        public void CreateKeyedHashByNameTest()
        {
            HashProviderFactory factory = new HashProviderFactory(CreateSource(providerName));
            IHashProvider provider = factory.Create(providerName);

            Assert.AreEqual(typeof(KeyedHashAlgorithmProvider), provider.GetType());
        }

        [TestMethod]
        public void CreateHashByNameTest()
        {
            HashProviderFactory factory = new HashProviderFactory(CreateSource(hashAlgorithm));
            IHashProvider provider = factory.Create(hashAlgorithm);

            Assert.AreEqual(typeof(HashAlgorithmProvider), provider.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void LookupInvalidParmeterNameThrows()
        {
            HashProviderFactory factory = new HashProviderFactory(CreateSource(providerName));
            factory.Create("provider3");
        }

        [TestMethod]
        public void CreateDefaultProvider()
        {
            HashProviderFactory factory = new HashProviderFactory(CreateSource(providerName));
            IHashProvider provider = factory.CreateDefault();

            Assert.AreEqual(typeof(KeyedHashAlgorithmProvider), provider.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(BuildFailedException))]
        public void TryToCreateDefaultProviderWithNoneDefinedThrows()
        {
            HashProviderFactory factory = new HashProviderFactory(CreateSource(string.Empty));
            IHashProvider provider = factory.CreateDefault();
            Assert.IsNotNull(provider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToCreateProviderWithNullNameThrows()
        {
            HashProviderFactory factory = new HashProviderFactory(CreateSource(providerName));
            factory.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToCreateProviderWithEmptyNameThrows()
        {
            HashProviderFactory factory = new HashProviderFactory(CreateSource(providerName));
            factory.Create(string.Empty);
        }

        IConfigurationSource CreateSource(string defaultName)
        {
            DictionaryConfigurationSource sections = new DictionaryConfigurationSource();
            CryptographySettings settings = new CryptographySettings();
            settings.DefaultHashProviderName = defaultName;
            settings.HashProviders.Add(new KeyedHashAlgorithmProviderData(providerName, typeof(HMACSHA1), false, keyedHashKeyFile, DataProtectionScope.CurrentUser));
            settings.HashProviders.Add(new HashAlgorithmProviderData(hashAlgorithm, typeof(SHA1Managed), false));
            sections.Add(CryptographyConfigurationView.SectionName, settings);
            return sections;
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
