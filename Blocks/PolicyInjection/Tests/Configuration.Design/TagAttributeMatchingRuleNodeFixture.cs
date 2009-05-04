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
    public class TagAttributeMatchingRuleNodeFixture
    {
        [TestMethod]
        public void TagAttributeMatchingRuleNodeHasProperDefaultName()
        {
            TagAttributeMatchingRuleNode ruleNode = new TagAttributeMatchingRuleNode();
            Assert.AreEqual("Tag Attribute Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateTagAttributeMatchingRuleNodeFromData()
        {
            TagAttributeMatchingRuleData ruleData = new TagAttributeMatchingRuleData();
            ruleData.Name = "matching rule name";
            ruleData.Match = "TagToMatch";
            ruleData.IgnoreCase = false;

            TagAttributeMatchingRuleNode ruleNode = new TagAttributeMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.Match, ruleNode.Match);
            Assert.AreEqual(ruleData.IgnoreCase, ruleNode.IgnoreCase);
        }

        [TestMethod]
        public void CanCreateRuleDataFromTagAttributeMatchingRuleNode()
        {
            TagAttributeMatchingRuleNode ruleNode = new TagAttributeMatchingRuleNode();
            ruleNode.Name = "matching rule name";
            ruleNode.Match = "TagToMatch";
            ruleNode.IgnoreCase = false;

            TagAttributeMatchingRuleData ruleData = ruleNode.GetConfigurationData() as TagAttributeMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.Match, ruleData.Match);
            Assert.AreEqual(ruleNode.IgnoreCase, ruleData.IgnoreCase);
        }
    }
}
