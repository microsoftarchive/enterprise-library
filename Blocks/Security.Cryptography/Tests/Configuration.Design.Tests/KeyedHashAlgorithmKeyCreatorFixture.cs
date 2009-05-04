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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class KeyedHashAlgorithmKeyCreatorFixture
    {
        [TestMethod]
        public void CreateKeyCreatesKeyOfDefaultSize()
        {
            KeyedHashAlgorithmKeyCreator keyCreator = new KeyedHashAlgorithmKeyCreator(typeof(HMACMD5));
            Assert.AreEqual(keyCreator.KeyLength, keyCreator.GenerateKey().Length);
        }

        [TestMethod]
        public void CreateKeyCreatesValidKey()
        {
            KeyedHashAlgorithmKeyCreator keyCreator = new KeyedHashAlgorithmKeyCreator(typeof(HMACMD5));
            Assert.IsTrue(keyCreator.KeyIsValid(keyCreator.GenerateKey()));
        }

        [TestMethod]
        public void NullKeyDoesNotPassValidation()
        {
            KeyedHashAlgorithmKeyCreator keyCreator = new KeyedHashAlgorithmKeyCreator(typeof(HMACMD5));
            Assert.IsFalse(keyCreator.KeyIsValid(null));
        }

        [TestMethod]
        public void KeyWithZeroLEngthDoesNotPassValidation()
        {
            KeyedHashAlgorithmKeyCreator keyCreator = new KeyedHashAlgorithmKeyCreator(typeof(HMACMD5));
            Assert.IsFalse(keyCreator.KeyIsValid(new byte[0]));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void KeyedHashAlgorithmKeyCreatorExpectsHashAlgorithmType()
        {
            new KeyedHashAlgorithmKeyCreator(typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeyedHashAlgorithmKeyCreatorThrowsPassingNull()
        {
            new KeyedHashAlgorithmKeyCreator(null);
        }
    }
}
