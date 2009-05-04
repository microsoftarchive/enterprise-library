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

using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class KeyManagerFixture
    {
        byte[] generatedKey;
        byte[] encryptedKey;
        MemoryStream stream;

        [TestInitialize]
        public void CreateSymmetricKey()
        {
            RijndaelManaged symmetricAlgorithm = (RijndaelManaged)RijndaelManaged.Create();
            symmetricAlgorithm.GenerateKey();
            generatedKey = symmetricAlgorithm.Key;
            encryptedKey = ProtectedData.Protect(generatedKey, null, DataProtectionScope.CurrentUser);

            stream = new MemoryStream();
        }

        [TestCleanup]
        public void CleanUpStream()
        {
            KeyManager.ClearCache();

            stream.Dispose();
        }

        [TestMethod]
        public void SymmetricKeysCanBeRoundTrippedToAndFromStream()
        {
            KeyManager.Write(stream, encryptedKey, DataProtectionScope.CurrentUser);
            stream.Seek(0, SeekOrigin.Begin);

            ProtectedKey restoredKey = KeyManager.Read(stream, DataProtectionScope.CurrentUser);

            AssertHelpers.AssertArraysEqual(generatedKey, restoredKey.DecryptedKey);
        }

        [TestMethod]
        public void SymmetricKeysCanBeRoundTrippedToAndFromArchivalStream()
        {
            ProtectedKey keyToArchive = ProtectedKey.CreateFromPlaintextKey(generatedKey, DataProtectionScope.CurrentUser);

            KeyManager.ArchiveKey(stream, keyToArchive, "passphrase");
            stream.Seek(0, SeekOrigin.Begin);
            ProtectedKey restoredKey = KeyManager.RestoreKey(stream, "passphrase", DataProtectionScope.CurrentUser);

            AssertHelpers.AssertArraysEqual(keyToArchive.DecryptedKey, restoredKey.DecryptedKey);
        }

        [TestMethod]
        public void KeyReadFromFileFirstTimeAndFromCacheThereafter()
        {
            using (FileStream outputStream = new FileStream("tmpFile", FileMode.Create))
            {
                KeyManager.Write(outputStream, encryptedKey, DataProtectionScope.CurrentUser);
            }

            ProtectedKey firstRead = KeyManager.Read("tmpFile", DataProtectionScope.CurrentUser);

            File.Delete("tmpFile");
            ProtectedKey secondRead = KeyManager.Read("tmpFile", DataProtectionScope.CurrentUser);

            Assert.AreSame(firstRead, secondRead);
        }
    }
}
