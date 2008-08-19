//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class BinaryFormatterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInBinaryFormatterNodeThrows()
        {
            new BinaryFormatterNode(null);
        }

        [TestMethod]
        public void BinaryFormatterNodeDefaults()
        {
            BinaryFormatterNode binaryFormatter = new BinaryFormatterNode();

            Assert.AreEqual("Binary Formatter", binaryFormatter.Name);
        }

        [TestMethod]
        public void BinaryFormatterDataTest()
        {
            string name = "some name";

            BinaryLogFormatterData data = new BinaryLogFormatterData();
            data.Name = name;

            BinaryFormatterNode node = new BinaryFormatterNode(data);

            Assert.AreEqual(name, node.Name);
        }

        [TestMethod]
        public void BinaryFormatterNodeTest()
        {
            string name = "some name";

            BinaryFormatterNode formatterNode = new BinaryFormatterNode();
            formatterNode.Name = name;

            FormatterData nodeData = formatterNode.FormatterData;

            Assert.AreEqual(name, nodeData.Name);
        }
    }
}