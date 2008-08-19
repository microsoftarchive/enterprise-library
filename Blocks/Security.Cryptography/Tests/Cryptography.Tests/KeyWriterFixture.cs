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
    public class KeyWriterFixture
    {
        RijndaelManaged symmetricAlgo;
        byte[] encryptedKey;
        MemoryStream stream;

        [TestInitialize]
        public void CreateSymmetricKey()
        {
            symmetricAlgo = (RijndaelManaged)RijndaelManaged.Create();
            symmetricAlgo.GenerateKey();
            encryptedKey = ProtectedData.Protect(symmetricAlgo.Key, null, DataProtectionScope.CurrentUser);

            stream = new MemoryStream();
        }

        [TestCleanup]
        public void CleanupStream()
        {
            stream.Dispose();
        }

        [TestMethod]
        public void KeysCanBeWrittenToStream()
        {
            ProtectedKey key = ProtectedKey.CreateFromEncryptedKey(encryptedKey, DataProtectionScope.CurrentUser);
            KeyReaderWriter writer = new KeyReaderWriter();

            writer.Write(stream, key);
            stream.Seek(0, SeekOrigin.Begin);

            KeyReaderWriter reader = new KeyReaderWriter();
            ProtectedKey readKey = reader.Read(stream, DataProtectionScope.CurrentUser);

            AssertHelpers.AssertArraysEqual(symmetricAlgo.Key, readKey.DecryptedKey);
        }
    }
}