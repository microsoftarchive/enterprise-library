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

using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class ProtectedKeyFixture
    {
        ProtectedKey key;
        RijndaelManaged symmetricAlgo;
        byte[] encryptedKey;

        [TestInitialize]
        public void CreateSymmetricKey()
        {
            symmetricAlgo = (RijndaelManaged)RijndaelManaged.Create();
            symmetricAlgo.GenerateKey();
            encryptedKey = ProtectedData.Protect(symmetricAlgo.Key, null, DataProtectionScope.LocalMachine);
        }

        [TestMethod]
        public void SymmetricKeyCanBeProtected()
        {
            key = ProtectedKey.CreateFromEncryptedKey(encryptedKey, DataProtectionScope.LocalMachine);
            AssertHelpers.AssertArraysEqual(encryptedKey, key.EncryptedKey);
        }

        [TestMethod]
        public void SymmetricKeyCanBeRecovered()
        {
            key = ProtectedKey.CreateFromEncryptedKey(encryptedKey, DataProtectionScope.LocalMachine);

            byte[] recoveredKey = key.DecryptedKey;

            AssertHelpers.AssertArraysEqual(symmetricAlgo.Key, recoveredKey);
        }
    }
}
