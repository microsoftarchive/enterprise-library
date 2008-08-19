//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionHandlerNodeFixture
    {
        [TestMethod]
        public void ExceptionHandlerDataTest()
        {
            string name = "some name";

            ExceptionHandlerData data = new ExceptionHandlerData();
            data.Name = name;

            ExceptionHandlerNode node = new ExceptionHandlerNodeImpl(data);
            Assert.AreEqual(name, node.Name);
        }

        [TestMethod]
        public void ExceptionHandlerNodeDataTest()
        {
            string name = "some name";

            ExceptionHandlerData exceptionHandlerData = new ExceptionHandlerData();
            exceptionHandlerData.Name = name;

            ExceptionHandlerNode exceptionHandlerNode = new ExceptionHandlerNodeImpl(exceptionHandlerData);

            ExceptionHandlerData nodeData = exceptionHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
        }

        class ExceptionHandlerNodeImpl : ExceptionHandlerNode
        {
            ExceptionHandlerData data;

            public ExceptionHandlerNodeImpl(ExceptionHandlerData data)
            {
                this.data = data;
                Rename(data.Name);
            }

            public override ExceptionHandlerData ExceptionHandlerData
            {
                get { return data; }
            }
        }
    }
}