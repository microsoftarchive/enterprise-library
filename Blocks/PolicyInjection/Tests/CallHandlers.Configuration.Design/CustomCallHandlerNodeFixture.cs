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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Tests
{
    [TestClass]
    public class CustomCallHandlerFixture
    {
        [TestMethod]
        public void CustomCallHandlerNodeHasProperName()
        {
            CustomCallHandlerNode node = new CustomCallHandlerNode();
            Assert.AreEqual("Custom Handler", node.Name);
        }

        [TestMethod]
        public void CustomCallHandlerNodeHasProperDefaultValues()
        {
            CustomCallHandlerNode node = new CustomCallHandlerNode();
            Assert.AreEqual("Custom Handler", node.Name);
            Assert.AreEqual(0, node.Order);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            CustomCallHandlerData handlerData = new CustomCallHandlerData();
            handlerData.Name = "customHandler";
            handlerData.TypeName = "HandlerType";
            handlerData.Attributes.Add("Attribute1", "Value1");
            handlerData.Attributes.Add("Attribute2", "Value2");
            handlerData.Order = 6;

            CustomCallHandlerNode handlerNode = new CustomCallHandlerNode(handlerData);

            Assert.AreEqual(handlerData.TypeName, handlerNode.Type);
            Assert.AreEqual(handlerData.Attributes.Count, handlerNode.Attributes.Count);
            Assert.AreEqual(handlerData.Attributes[0], handlerNode.Attributes[0].Value);
            Assert.AreEqual(handlerData.Attributes.AllKeys[0], handlerNode.Attributes[0].Key);
            Assert.AreEqual(handlerData.Attributes[1], handlerNode.Attributes[1].Value);
            Assert.AreEqual(handlerData.Attributes.AllKeys[1], handlerNode.Attributes[1].Key);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            CustomCallHandlerNode handlerNode = new CustomCallHandlerNode();
            handlerNode.Name = "cacheHandler";
            handlerNode.Type = "HandlerType";
            handlerNode.Attributes.Add(new EditableKeyValue("Attribute1", "Value1"));
            handlerNode.Attributes.Add(new EditableKeyValue("Attribute2", "Value2"));
            handlerNode.Order = 7;

            CustomCallHandlerData handlerData = handlerNode.CreateCallHandlerData() as CustomCallHandlerData;

            Assert.IsNotNull(handlerData);
            Assert.AreEqual(handlerNode.Type, handlerData.TypeName);
            Assert.AreEqual(handlerNode.Attributes.Count, handlerData.Attributes.Count);
            Assert.AreEqual(handlerNode.Attributes[0].Value, handlerData.Attributes[0]);
            Assert.AreEqual(handlerNode.Attributes[0].Key, handlerData.Attributes.AllKeys[0]);
            Assert.AreEqual(handlerNode.Attributes[1].Value, handlerData.Attributes[1]);
            Assert.AreEqual(handlerNode.Attributes[1].Key, handlerData.Attributes.AllKeys[1]);
            Assert.AreEqual(handlerNode.Order, handlerData.Order);
        }
    }
}