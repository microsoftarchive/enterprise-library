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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class CustomMatchingRuleNodeFixture
    {
        [TestMethod]
        public void CustomMatchingRuleNodeHasProperDefaultName()
        {
            CustomMatchingRuleNode ruleNode = new CustomMatchingRuleNode();
            Assert.AreEqual("Custom Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateCustomMatchingRuleNodeFromData()
        {
            CustomMatchingRuleData ruleData = new CustomMatchingRuleData();
            ruleData.Name = "CustomAttributeRuleName";
            ruleData.TypeName = "custom ruleType";
            ruleData.Attributes.Add("Attribute1", "Value1");
            ruleData.Attributes.Add("Attribute2", "Value2");

            CustomMatchingRuleNode ruleNode = new CustomMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.TypeName, ruleNode.Type);
            Assert.AreEqual(ruleData.Attributes.Count, ruleNode.Attributes.Count);
            Assert.AreEqual(ruleData.Attributes[0], ruleNode.Attributes[0].Value);
            Assert.AreEqual(ruleData.Attributes.AllKeys[0], ruleNode.Attributes[0].Key);
            Assert.AreEqual(ruleData.Attributes[1], ruleNode.Attributes[1].Value);
            Assert.AreEqual(ruleData.Attributes.AllKeys[1], ruleNode.Attributes[1].Key);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomMatchingRuleNode()
        {
            CustomMatchingRuleNode ruleNode = new CustomMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Type = "custom rule type";
            ruleNode.Attributes.Add(new EditableKeyValue("Key1", "value1"));
            ruleNode.Attributes.Add(new EditableKeyValue("Key2", "value2"));

            CustomMatchingRuleData ruleData = ruleNode.GetConfigurationData() as CustomMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.Type, ruleData.TypeName);
            Assert.AreEqual(ruleNode.Attributes[0].Key, ruleData.Attributes.AllKeys[0]);
            Assert.AreEqual(ruleNode.Attributes[0].Value, ruleData.Attributes[0]);
            Assert.AreEqual(ruleNode.Attributes[1].Key, ruleData.Attributes.AllKeys[1]);
            Assert.AreEqual(ruleNode.Attributes[1].Value, ruleData.Attributes[1]);
        }
    }
}