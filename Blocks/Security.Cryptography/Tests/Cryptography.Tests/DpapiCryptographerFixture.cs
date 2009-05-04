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
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class DpapiCryptographerFixture
    {
        static byte[] plainText;
        static byte[] entropy;

        [TestInitialize]
        public void Setup()
        {
            entropy = new byte[12];
            RandomNumberGenerator rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(entropy);

            plainText = new byte[12];
            rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(plainText);
        }

        [TestMethod]
        public void DefaultStorageMode()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer();
            Assert.AreEqual(DataProtectionScope.LocalMachine, dpapi.StoreScope);
        }

        [TestMethod]
        public void EncryptAndDecryptMachineMode()
        {
            DataProtectionScope mode = DataProtectionScope.LocalMachine;
            DpapiCryptographer dpapi = new DpapiCryptographer(mode);

            byte[] cipherText = dpapi.Encrypt(plainText, entropy);

            Assert.IsFalse(CryptographyUtility.CompareBytes(plainText, cipherText));

            byte[] decryptedText = dpapi.Decrypt(cipherText, entropy);

            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText));
        }

        [TestMethod]
        public void EncryptAndDecryptUserMode()
        {
            DataProtectionScope mode = DataProtectionScope.CurrentUser;
            DpapiCryptographer dpapi = new DpapiCryptographer(mode);

            byte[] cipherText = dpapi.Encrypt(plainText);

            Assert.IsFalse(CryptographyUtility.CompareBytes(plainText, cipherText));

            byte[] decryptedText = dpapi.Decrypt(cipherText);

            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText));
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void DecryptBadDataThrows()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer();
            byte[] cipherText = new byte[] { 0, 1 };
            dpapi.Decrypt(cipherText, entropy);
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void DecryptWithDifferentEntropyThanEncryptThrows()
        {
            byte[] plainBytes = new byte[] { 0, 1, 2, 3, 4 };
            byte[] entropy1 = new byte[16];
            byte[] entropy2 = new byte[16];
            CryptographyUtility.GetRandomBytes(entropy1);
            CryptographyUtility.GetRandomBytes(entropy2);

            DpapiCryptographer dpapi = new DpapiCryptographer(DataProtectionScope.LocalMachine);
            byte[] encrypted = dpapi.Encrypt(plainBytes, entropy1);
            dpapi.Decrypt(encrypted, entropy2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptWithNullCipherTextThrows()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer(DataProtectionScope.CurrentUser);
            dpapi.Decrypt(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptWithEmptyCipherTextThrows()
        {
            byte[] b = new byte[] { };
            DpapiCryptographer dpapi = new DpapiCryptographer(DataProtectionScope.CurrentUser);
            dpapi.Decrypt(b);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptWithNullPlainTextThrows()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer(DataProtectionScope.CurrentUser);
            dpapi.Encrypt(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptWithZeroLengthTextThrows()
        {
            byte[] b = new byte[] { };
            DpapiCryptographer dpapi = new DpapiCryptographer(DataProtectionScope.LocalMachine);
            dpapi.Encrypt(b, entropy);
        }
    }
}
