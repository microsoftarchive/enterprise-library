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
    public class PropertyNodeFixture
    {
        [TestMethod]
        public void PropertyNodeDoesntSortChildNodes()
        {
            PropertyNode propertyNode = new PropertyNode();

            Assert.AreEqual(false, propertyNode.SortChildren);
        }

        [TestMethod]
        public void PropertyNodeHasProperDefaultName()
        {
            PropertyNode propertyNode = new PropertyNode();

            Assert.AreEqual("Property", propertyNode.Name);
        }

        [TestMethod]
        public void PropertyNodeHasPropertyNameAsNodeName()
        {
            PropertyNode propertyNode = new PropertyNode("PropertyName");

            Assert.AreEqual("PropertyName", propertyNode.Name);
        }

        [TestMethod]
        public void PropertyNodeHasConfiguredPropertyNameAsNodeName()
        {
            ValidatedPropertyReference propertyReference = new ValidatedPropertyReference("PropertyName");
            PropertyNode propertyNode = new PropertyNode(propertyReference);

            Assert.AreEqual("PropertyName", propertyNode.Name);
        }
    }
}