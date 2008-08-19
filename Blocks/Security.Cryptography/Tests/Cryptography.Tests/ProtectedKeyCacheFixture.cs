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
    public class ProtectedKeyCacheFixture
    {
        ProtectedKey originalKey;
        ProtectedKeyCache cache;

        [TestInitialize]
        public void CreateStuff()
        {
            cache = new ProtectedKeyCache();
            originalKey = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.LocalMachine);
        }

        [TestMethod]
        public void AskingForKeyForUnreadFileReturnsNull()
        {
            Assert.IsNull(cache["pathToUnfoundProtectedKeyFile"]);
        }

        [TestMethod]
        public void ReturnsProtectedKeyWhenPresentInCache()
        {
            cache["fileFoo"] = originalKey;

            ProtectedKey cachedKey = cache["fileFoo"];
            AssertHelpers.AssertArraysEqual(originalKey.DecryptedKey, cachedKey.DecryptedKey);
        }

        [TestMethod]
        public void ReturnsCorrectKeyForGivenPathname()
        {
            ProtectedKey secondKey = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.LocalMachine);

            cache["firstFile"] = originalKey;
            cache["secondFile"] = secondKey;

            AssertHelpers.AssertArraysEqual(originalKey.DecryptedKey, cache["firstFile"].DecryptedKey);
        }

        [TestMethod]
        public void AddingSameKeyFileTwiceReplacesOldProtectedKey()
        {
            ProtectedKey secondKey = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.LocalMachine);

            cache["firstFile"] = originalKey;
            cache["firstFile"] = secondKey;

            AssertHelpers.AssertArraysEqual(secondKey.DecryptedKey, cache["firstFile"].DecryptedKey);
        }

        [TestMethod]
        public void CacheCanBeProgrammaticallyCleared()
        {
            cache["FileName"] = originalKey;
            cache.Clear();

            Assert.IsNull(cache["FileName"]);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ProvidingNullFileNameThrowsArgumentException()
        {
            ProtectedKey neverActuallySetsThisVariable = cache[null];
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ProvidingEmptyFileNameThrowsArgumentException()
        {
            ProtectedKey neverActuallySetsThisVariable = cache[String.Empty];
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void TryingToStoreNullProtectedKeyInCacheThrowsArgumentNullException()
        {
            cache["FileName"] = null;
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TryingToStoreProtectedKeyWithNullFileNameThrowsArgumentException()
        {
            cache[null] = originalKey;
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TryingToStoreProtectedKeyWithEmptyFileNameThrowsArgumentException()
        {
            cache[String.Empty] = originalKey;
        }
    }
}