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
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class CryptographerBoundsFixture
    {
        const string hashInstance = "hmac1";
        const string symmInstance = "dpapiSymmetric1";
        const string keyedHashKeyFile = "KeyedHashKey.file";

        const string plainTextString = "secret";
        static readonly byte[] plainTextBytes = new byte[] { 0, 1, 2, 3 };

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
        [ExpectedException(typeof(ArgumentException))]
        public void CreateHashWithNullInstanceThrows()
        {
            Cryptographer.CreateHash(null, plainTextBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptSymmetricWithNullCipherTextThrows()
        {
            Cryptographer.DecryptSymmetric(symmInstance, (string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptSymmetricWithEmptyStringCipherTextThrows()
        {
            Cryptographer.DecryptSymmetric(symmInstance, (string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateHashWithZeroLengthInstanceThrows()
        {
            Cryptographer.CreateHash(string.Empty, plainTextBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateHashWithNullInstanceStringThrows()
        {
            Cryptographer.CreateHash(null, plainTextString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateHashWithZeroLengthInstanceStringThrows()
        {
            Cryptographer.CreateHash(string.Empty, plainTextString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareHashWithNullInstanceThrows()
        {
            byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes);
            Cryptographer.CompareHash(null, plainTextBytes, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareHashWithZeroLengthInstanceThrows()
        {
            byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes);
            Cryptographer.CompareHash(string.Empty, plainTextBytes, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareHashWithNullInstanceStringThrows()
        {
            string hash = Cryptographer.CreateHash(hashInstance, plainTextString);
            Cryptographer.CompareHash(null, plainTextString, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareHashWithZeroLengthInstanceStringThrows()
        {
            string hash = Cryptographer.CreateHash(hashInstance, plainTextString);
            Cryptographer.CompareHash(string.Empty, plainTextString, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptWithNullInstanceThrows()
        {
            Cryptographer.EncryptSymmetric(null, plainTextBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptWithZeroLengthInstanceThrows()
        {
            Cryptographer.EncryptSymmetric(string.Empty, plainTextBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptWithNullInstanceStringThrows()
        {
            Cryptographer.EncryptSymmetric(null, plainTextString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptWithZeroLengthInstanceStringThrows()
        {
            Cryptographer.EncryptSymmetric(string.Empty, plainTextString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptWithNullInstanceThrows()
        {
            Cryptographer.DecryptSymmetric(null, plainTextBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptWithZeroLengthInstanceThrows()
        {
            Cryptographer.DecryptSymmetric(string.Empty, plainTextBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptWithNullInstanceStringThrows()
        {
            Cryptographer.DecryptSymmetric(null, plainTextString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptWithZeroLengthInstanceStringThrows()
        {
            Cryptographer.DecryptSymmetric(string.Empty, plainTextString);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void DecryptWithInvalidStringThrows()
        {
            Cryptographer.DecryptSymmetric(symmInstance, "INVALID");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CompareHashWithInvalidStringThrows()
        {
            Cryptographer.CompareHash(hashInstance, plainTextString, "INVALID");
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
        public void CreateHashWithInvalidInstanceThrows()
        {
            Cryptographer.CreateHash("invalid instance", plainTextString);
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
        public void EncryptWithInvalidInstanceThrows()
        {
            Cryptographer.EncryptSymmetric("invalid instance", plainTextString);
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
