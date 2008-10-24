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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Tests
{
    [TestClass]
    public class CachingCallHandlerFixture
    {
        [TestMethod]
        public void CachingCallHandlerNodeHasProperName()
        {
            CachingCallHandlerNode node = new CachingCallHandlerNode();
            Assert.AreEqual("Caching Handler", node.Name);
        }

        [TestMethod]
        public void CachingCallHandlerHasPropertDefaultValues()
        {
            CachingCallHandlerNode node = new CachingCallHandlerNode();
            Assert.AreEqual("Caching Handler", node.Name);
            Assert.AreEqual(0, node.Order);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            CachingCallHandlerData handlerData = new CachingCallHandlerData();
            handlerData.Name = "cacheHandler";
            handlerData.ExpirationTime = TimeSpan.FromHours(1.23d);
            handlerData.Order = 1;

            CachingCallHandlerNode handlerNode = new CachingCallHandlerNode(handlerData);
            Assert.AreEqual(handlerData.Name, handlerNode.Name);
            Assert.AreEqual(handlerData.ExpirationTime, handlerNode.ExpirationTime);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            CachingCallHandlerNode handlerNode = new CachingCallHandlerNode();
            handlerNode.Name = "cacheHandler";
            handlerNode.ExpirationTime = TimeSpan.FromHours(1.23d);
            handlerNode.Order = 2;

            CachingCallHandlerData handlerData = handlerNode.CreateCallHandlerData() as CachingCallHandlerData;

            Assert.IsNotNull(handlerData);
            Assert.AreEqual(handlerNode.Name, handlerData.Name);
            Assert.AreEqual(handlerNode.ExpirationTime, handlerData.ExpirationTime);
            Assert.AreEqual(handlerNode.Order, handlerData.Order);
        }
    }
}
