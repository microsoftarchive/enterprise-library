//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionCallHandlerFixture
    {
        [TestMethod]
        public void ExceptionCallHandlerNodeHasProperName()
        {
            ExceptionCallHandlerNode node = new ExceptionCallHandlerNode();
            Assert.AreEqual("Exception Handling Handler", node.Name);
        }

        [TestMethod]
        public void ExceptionCallHandlerNodeHasProperDefaultValues()
        {
            ExceptionCallHandlerNode node = new ExceptionCallHandlerNode();
            Assert.AreEqual("Exception Handling Handler", node.Name);
            Assert.AreEqual(0, node.Order);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            ExceptionCallHandlerData handlerData = new ExceptionCallHandlerData();
            handlerData.Name = "exceptionHandler";
            handlerData.Order = 9;

            ExceptionCallHandlerNode handlerNode = new ExceptionCallHandlerNode(handlerData);
            Assert.AreEqual(handlerData.Name, handlerNode.Name);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            ExceptionCallHandlerNode handlerNode = new ExceptionCallHandlerNode();
            handlerNode.Name = "exceptionHandler";
            handlerNode.Order = 9;

            ExceptionCallHandlerData handlerData = handlerNode.CreateCallHandlerData() as ExceptionCallHandlerData;

            Assert.IsNotNull(handlerData);
            Assert.AreEqual(handlerData.Name, handlerNode.Name);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }
    }
}