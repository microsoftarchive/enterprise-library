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
    public class AuthorizationCallHandlerFixture
    {
        [TestMethod]
        public void AuthorizationCallHandlerNodeHasProperName()
        {
            AuthorizationCallHandlerNode node = new AuthorizationCallHandlerNode();
            Assert.AreEqual("Authorization Handler", node.Name);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            AuthorizationCallHandlerData handlerData = new AuthorizationCallHandlerData();
            handlerData.Name = "authHandler";
            handlerData.OperationName = "operation";
            handlerData.Order = 5;

            AuthorizationCallHandlerNode handlerNode = new AuthorizationCallHandlerNode(handlerData);
            Assert.AreEqual(handlerData.Name, handlerNode.Name);
            Assert.AreEqual(handlerData.OperationName, handlerNode.OperationName);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            AuthorizationCallHandlerNode handlerNode = new AuthorizationCallHandlerNode();
            handlerNode.Name = "authHandler";
            handlerNode.OperationName = "Operation";
            handlerNode.Order = 5;

            AuthorizationCallHandlerData handlerData = handlerNode.CreateCallHandlerData() as AuthorizationCallHandlerData;

            Assert.IsNotNull(handlerData);
            Assert.AreEqual(handlerNode.Name, handlerData.Name);
            Assert.AreEqual(handlerNode.OperationName, handlerData.OperationName);
            Assert.AreEqual(handlerNode.Order, handlerData.Order);
        }
    }
}