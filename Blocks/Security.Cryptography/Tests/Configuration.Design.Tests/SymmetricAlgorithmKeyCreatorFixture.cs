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
    public class SymmetricAlgorithmKeyCreatorFixture
    {
        [TestMethod]
        public void CreateKeyCreatesValidKey()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged));
            Assert.IsTrue(keyCreator.KeyIsValid(keyCreator.GenerateKey()));
        }

        [TestMethod]
        public void NullKeyDoesNotPassValidation()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged));
            Assert.IsFalse(keyCreator.KeyIsValid(null));
        }

        [TestMethod]
        public void KeyWithZeroLEngthDoesNotPassValidation()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged));
            Assert.IsFalse(keyCreator.KeyIsValid(new byte[0]));
        }

        [TestMethod]
        public void KeyWithInvalidSizeDoesNotPassValidation()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged));
            Assert.IsFalse(keyCreator.KeyIsValid(new byte[3]));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SymmetricAlgorithmKeyCreatorExpectsHashAlgorithmType()
        {
            new SymmetricAlgorithmKeyCreator(typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SymmetricAlgorithmKeyCreatorThrowsPassingNull()
        {
            new SymmetricAlgorithmKeyCreator(typeof(string));
        }
    }
}