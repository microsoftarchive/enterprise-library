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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
    [TestClass]
    public class SymmetricCryptographerFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructingWithBadTypeThrows()
        {
            SymmetricCryptographer symm = new SymmetricCryptographer(typeof(object), (ProtectedKey)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingWithNullTypeThrows()
        {
            SymmetricCryptographer symm = new SymmetricCryptographer((Type)null, (ProtectedKey)null);
        }

        [TestMethod]
        public void EncryptAndDecryptWithType()
        {
            byte[] key = new byte[16];
            CryptographyUtility.GetRandomBytes(key);
            ProtectedKey protectedKey = ProtectedKey.CreateFromPlaintextKey(key, DataProtectionScope.LocalMachine);

            SymmetricCryptographer symm = new SymmetricCryptographer(typeof(RijndaelManaged), protectedKey);

            byte[] plainText = new byte[12];
            CryptographyUtility.GetRandomBytes(plainText);

            byte[] cipherText = symm.Encrypt(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(cipherText, plainText));

            byte[] decryptedText = symm.Decrypt(cipherText);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText));
        }

        [TestMethod]
        public void EncryptAndDecryptWithTypeUsingProtectedKey()
        {
            byte[] key = new byte[16];
            CryptographyUtility.GetRandomBytes(key);
            ProtectedKey protectedKey = ProtectedKey.CreateFromPlaintextKey(key, DataProtectionScope.LocalMachine);

            SymmetricCryptographer symm = new SymmetricCryptographer(typeof(RijndaelManaged), protectedKey);

            byte[] plainText = new byte[12];
            CryptographyUtility.GetRandomBytes(plainText);

            byte[] cipherText = symm.Encrypt(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(cipherText, plainText));

            byte[] decryptedText = symm.Decrypt(cipherText);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText));
        }
    }
}