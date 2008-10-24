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
    public class ValidationCallHandlerFixture
    {
        [TestMethod]
        public void ValidationCallHandlerNodeHasProperName()
        {
            ValidationCallHandlerNode node = new ValidationCallHandlerNode();
            Assert.AreEqual("Validation Handler", node.Name);
        }

        [TestMethod]
        public void ValidationCallHandlerNodeHasProperDefaults()
        {
            ValidationCallHandlerNode node = new ValidationCallHandlerNode();
            Assert.AreEqual("Validation Handler", node.Name);
            Assert.AreEqual(0, node.Order);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            ValidationCallHandlerData handlerData = new ValidationCallHandlerData();
            handlerData.Name = "validationHandler";
            handlerData.SpecificationSource = SpecificationSource.Both;
            handlerData.RuleSet = "ruleSet";
            handlerData.Order = 5;

            ValidationCallHandlerNode handlerNode = new ValidationCallHandlerNode(handlerData);
            Assert.AreEqual(handlerData.Name, handlerNode.Name);
            Assert.AreEqual(handlerData.SpecificationSource, handlerNode.SpecificationSource);
            Assert.AreEqual(handlerData.RuleSet, handlerNode.RuleSet);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            ValidationCallHandlerNode handlerNode = new ValidationCallHandlerNode();
            handlerNode.Name = "validationHandler";
            handlerNode.SpecificationSource = SpecificationSource.Both;
            handlerNode.RuleSet = "ruleSet";
            handlerNode.Order = 7;

            ValidationCallHandlerData handlerData = handlerNode.CreateCallHandlerData() as ValidationCallHandlerData;

            Assert.IsNotNull(handlerData);
            Assert.AreEqual(handlerNode.Name, handlerData.Name);
            Assert.AreEqual(handlerNode.SpecificationSource, handlerData.SpecificationSource);
            Assert.AreEqual(handlerNode.RuleSet, handlerData.RuleSet);
            Assert.AreEqual(handlerNode.Order, handlerData.Order);
        }
    }
}
