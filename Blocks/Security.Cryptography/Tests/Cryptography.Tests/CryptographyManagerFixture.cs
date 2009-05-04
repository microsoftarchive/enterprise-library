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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class CryptographyManagerFixture
    {
        const string hashInstance = "hmac1";
        const string symmInstance = "dpapiSymmetric1";
        const string symmetricAlgorithm1 = "symmetricAlgorithm1";
        const string symmetricKeyFile = "ProtectedKey.file";
        const string keyedHashKeyFile = "KeyedHashKey.file";
        const string plainTextString = "secret";

        private Dictionary<string, IHashProvider> hashProviders;
        private Dictionary<string, ISymmetricCryptoProvider> symmetricCrytoProviders;
        private IHashProvider defaultHashProvider;
        private ISymmetricCryptoProvider defaultSymmetricCryptoProvider;
        private ISymmetricCryptoProvider algorithSymmetricCryptoProvider;
        private CryptographyManager cryptographyManager;
        private IConfigurationSource configSource;
        private DefaultCryptographyErrorEventArgs cryptoArgs;

        private readonly byte[] plainTextBytes = new byte[] { 0, 1, 2, 3 };

        [TestInitialize]
        public void SetUp()
        {
            hashProviders = new Dictionary<string, IHashProvider>();
            symmetricCrytoProviders = new Dictionary<string, ISymmetricCryptoProvider>();

            CreateKeyFile(symmetricKeyFile);
            CreateKeyFile(keyedHashKeyFile);

            configSource = ConfigurationSourceFactory.Create();

            HashProviderFactory factory = new HashProviderFactory(configSource);
            defaultHashProvider = factory.Create(hashInstance);
            hashProviders.Add(hashInstance, defaultHashProvider);

            SymmetricCryptoProviderFactory symmfactory = new SymmetricCryptoProviderFactory(configSource);
            defaultSymmetricCryptoProvider = symmfactory.Create(symmInstance);
            algorithSymmetricCryptoProvider = symmfactory.Create(symmetricAlgorithm1);

            symmetricCrytoProviders.Add(symmInstance, defaultSymmetricCryptoProvider);
            symmetricCrytoProviders.Add(symmetricAlgorithm1, algorithSymmetricCryptoProvider);

            cryptographyManager = new CryptographyManagerImpl(hashProviders, symmetricCrytoProviders);

            this.cryptoArgs = null;
        }

        [TestCleanup]
        public void DeleteKeyFile()
        {
            File.Delete(symmetricKeyFile);
            File.Delete(keyedHashKeyFile);

            cryptographyManager = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenUsingInvalidHashParameters()
        {
            cryptographyManager.CreateHash("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenUsingInvalidSymmectricCryptoParameters()
        {
            cryptographyManager.DecryptSymmetric("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void GetExceptionWhenUsingInvalidHashInstance()
        { 
            cryptographyManager.CreateHash("invalidInstance", "anything");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void GetExceptionWhenUsingInvalidSymmetricCryptoInstance()
        {
            cryptographyManager.DecryptSymmetric("invalidInstance", "anything");
        }

        [TestMethod]
        public void CreateHashBytes()
        {
            byte[] hash = cryptographyManager.CreateHash(hashInstance, plainTextBytes);
            Assert.IsFalse(CryptographyUtility.CompareBytes(plainTextBytes, hash));
        }

        [TestMethod]
        public void CreateHashString()
        {
            string hashString = cryptographyManager.CreateHash(hashInstance, plainTextString);
            Assert.IsFalse(plainTextString == hashString);
        }

        [TestMethod]
        public void CreateAndCompareHashBytes()
        {
            byte[] hash = cryptographyManager.CreateHash(hashInstance, plainTextBytes);
            bool result = cryptographyManager.CompareHash(hashInstance, plainTextBytes, hash);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateAndCompareInvalidHashBytes()
        {
            byte[] hash = cryptographyManager.CreateHash(hashInstance, plainTextBytes);

            byte[] badPlainText = new byte[] { 2, 1, 0 };
            bool result = cryptographyManager.CompareHash(hashInstance, badPlainText, hash);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateAndCompareHashString()
        {
            string hashString = cryptographyManager.CreateHash(hashInstance, plainTextString);

            bool result = cryptographyManager.CompareHash(hashInstance, plainTextString, hashString);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EncryptAndDecryptBytes()
        {
            byte[] encrypted = cryptographyManager.EncryptSymmetric(symmInstance, plainTextBytes);
            Assert.IsFalse(CryptographyUtility.CompareBytes(plainTextBytes, encrypted));

            byte[] decrypted = cryptographyManager.DecryptSymmetric(symmInstance, encrypted);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainTextBytes, decrypted));
        }

        [TestMethod]
        public void EncryptAndDecryptString()
        {
            string encrypted = cryptographyManager.EncryptSymmetric(symmInstance, plainTextString);
            Assert.IsFalse(plainTextString == encrypted);

            string decrypted = cryptographyManager.DecryptSymmetric(symmInstance, encrypted);
            Assert.IsTrue(plainTextString == decrypted);
        }

        [TestMethod]
        public void EncryptAndDecryptStringWithASymmetricAlgorithm()
        {
            string encrypted = cryptographyManager.EncryptSymmetric(symmetricAlgorithm1, plainTextString);
            Assert.IsFalse(plainTextString == encrypted);

            string decrypted = cryptographyManager.DecryptSymmetric(symmetricAlgorithm1, encrypted);
            Assert.IsTrue(plainTextString == decrypted);
        }

        [TestMethod]
        public void EncryptAndDecryptOneByte()
        {
            byte[] onebyte = new byte[1];
            CryptographyUtility.GetRandomBytes(onebyte);

            byte[] encrypted = cryptographyManager.EncryptSymmetric(symmInstance, onebyte);
            Assert.IsFalse(CryptographyUtility.CompareBytes(onebyte, encrypted));

            byte[] decrypted = cryptographyManager.DecryptSymmetric(symmInstance, encrypted);

            Assert.IsTrue(CryptographyUtility.CompareBytes(onebyte, decrypted));
        }

        [TestMethod]
        public void EncryptAndDecryptOneMegabyte()
        {
            byte[] megabyte = new byte[1024 * 1024];
            CryptographyUtility.GetRandomBytes(megabyte);

            byte[] encrypted = cryptographyManager.EncryptSymmetric(symmInstance, megabyte);
            Assert.IsFalse(CryptographyUtility.CompareBytes(megabyte, encrypted));

            byte[] decrypted = cryptographyManager.DecryptSymmetric(symmInstance, encrypted);

            Assert.IsTrue(CryptographyUtility.CompareBytes(megabyte, decrypted));
        }

        [TestMethod]
        public void HashProviderWithNonExistingInstanceFiresInstrumentation()
        {
            const string ProviderName = "IHashProvider";
            const string InvalidHashInstance = "invalidHashInstance";

            ((DefaultCryptographyInstrumentationProvider)((IInstrumentationEventProvider)cryptographyManager).GetInstrumentationEventProvider())
                .cryptographyErrorOccurred += (sender, args) => { cryptoArgs = args; };
            try
            {
                cryptographyManager.CreateHash(InvalidHashInstance, new byte[0]);
                Assert.Fail("should have thrown exception");
            }
            catch (ConfigurationErrorsException)
            {
                Assert.IsNotNull(this.cryptoArgs);
                Assert.AreEqual(ProviderName, this.cryptoArgs.ProviderName);
                Assert.AreEqual(InvalidHashInstance, this.cryptoArgs.InstanceName);
            }
        }

        [TestMethod]
        public void SymmectricCryptoProviderWithNonExistingInstanceFiresInstrumentation()
        {
            const string ProviderName = "ISymmetricCryptoProvider";
            const string InvalidSymmectricInstance = "invalidSymmectricInstance";

            ((DefaultCryptographyInstrumentationProvider)((IInstrumentationEventProvider)cryptographyManager).GetInstrumentationEventProvider())
                .cryptographyErrorOccurred += (sender, args) => { cryptoArgs = args; };
            try
            {
                cryptographyManager.DecryptSymmetric(InvalidSymmectricInstance, "text");
                Assert.Fail("should have thrown exception");
            }
            catch (ConfigurationErrorsException)
            {
                Assert.IsNotNull(this.cryptoArgs);
                Assert.AreEqual(ProviderName, this.cryptoArgs.ProviderName);
                Assert.AreEqual(InvalidSymmectricInstance, this.cryptoArgs.InstanceName);
            }
        }

        private static void CreateKeyFile(string fileName)
        {
            ProtectedKey key = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.CurrentUser);
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                KeyManager.Write(stream, key);
            }
        }
    }
}
