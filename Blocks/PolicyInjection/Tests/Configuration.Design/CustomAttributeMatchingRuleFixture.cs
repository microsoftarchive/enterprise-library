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

using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class CustomAttributeMatchingRuleFixture
    {
        [TestMethod]
        public void CustomAttributeMatchingRuleNodeHasProperDefaultName()
        {
            CustomAttributeMatchingRuleNode ruleNode = new CustomAttributeMatchingRuleNode();
            Assert.AreEqual("Custom Attribute Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            CustomAttributeMatchingRuleData ruleData = new CustomAttributeMatchingRuleData();
            ruleData.Name = "CustomAttributeRuleName";
            ruleData.AttributeTypeName = "attributeType";

            CustomAttributeMatchingRuleNode ruleNode = new CustomAttributeMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.AttributeTypeName, ruleNode.AttributeType);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            CustomAttributeMatchingRuleNode ruleNode = new CustomAttributeMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.AttributeType = "attributeType";

            CustomAttributeMatchingRuleData ruleData = ruleNode.GetConfigurationData() as CustomAttributeMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.AttributeType, ruleData.AttributeTypeName);
        }
    }
}
