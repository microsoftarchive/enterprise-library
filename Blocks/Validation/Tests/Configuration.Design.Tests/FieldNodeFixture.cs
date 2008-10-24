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
    public class FieldNodeFixture
    {
        [TestMethod]
        public void FieldNodeDoesntSortChildNodes()
        {
            FieldNode fieldNode = new FieldNode();

            Assert.AreEqual(false, fieldNode.SortChildren);
        }

        [TestMethod]
        public void FieldNodeHasProperDefaultName()
        {
            FieldNode fieldNode = new FieldNode();
            Assert.AreEqual("Field", fieldNode.Name);
        }

        [TestMethod]
        public void FieldNodeHasFieldNameAsNodeName()
        {
            FieldNode fieldNode = new FieldNode("FieldName");
            Assert.AreEqual("FieldName", fieldNode.Name);
        }

        [TestMethod]
        public void FieldNodeHasConfiguredFieldNameAsNodeName()
        {
            ValidatedFieldReference fieldReference = new ValidatedFieldReference("FieldName");
            FieldNode fieldNode = new FieldNode(fieldReference);
            Assert.AreEqual("FieldName", fieldNode.Name);
        }
    }
}
