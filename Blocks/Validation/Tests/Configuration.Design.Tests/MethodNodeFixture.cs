//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class MethodNodeFixture
    {
        [TestMethod]
        public void MethodNodeDoesntSortChildNodes()
        {
            MethodNode methodNode = new MethodNode();

            Assert.AreEqual(false, methodNode.SortChildren);
        }

        [TestMethod]
        public void MethodNodeHasProperDefaultName()
        {
            MethodNode methodNode = new MethodNode();

            Assert.AreEqual("Method", methodNode.Name);
        }

        [TestMethod]
        public void MethodNodeHasMethodNameAsNodeName()
        {
            MethodNode methodNode = new MethodNode("MethodName");

            Assert.AreEqual("MethodName", methodNode.Name);
        }

        [TestMethod]
        public void MethodNodeHasConfiguredMethodNameAsNodeName()
        {
            ValidatedMethodReference methodReference = new ValidatedMethodReference("MethodName");
            MethodNode methodNode = new MethodNode(methodReference);

            Assert.AreEqual("MethodName", methodNode.Name);
        }
    }
}