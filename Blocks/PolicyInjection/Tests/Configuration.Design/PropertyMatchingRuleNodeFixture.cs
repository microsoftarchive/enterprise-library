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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class PropertyMatchingRuleNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void PropertyMatchingRuleNodeHasProperDefaultName()
        {
            PropertyMatchingRuleNode ruleNode = new PropertyMatchingRuleNode();
            Assert.AreEqual("Property Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreatePropertyMatchingRuleNodeFromData()
        {
            PropertyMatchingRuleData ruleData = new PropertyMatchingRuleData();
            ruleData.Name = "name o' rule";
            ruleData.Matches.Add(new PropertyMatchData("Property1", PropertyMatchingOption.GetOrSet, false));
            ruleData.Matches.Add(new PropertyMatchData("Property2", PropertyMatchingOption.Set, false));

            PropertyMatchingRuleNode ruleNode = new PropertyMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.Matches.Count, ruleNode.Matches.Count);
            Assert.AreEqual(ruleData.Matches[0].IgnoreCase, ruleNode.Matches[0].IgnoreCase);
            Assert.AreEqual(ruleData.Matches[0].MatchOption, ruleNode.Matches[0].MatchOption);
            Assert.AreEqual(ruleData.Matches[0].Match, ruleNode.Matches[0].Value);
            Assert.AreEqual(ruleData.Matches[1].IgnoreCase, ruleNode.Matches[1].IgnoreCase);
            Assert.AreEqual(ruleData.Matches[1].MatchOption, ruleNode.Matches[1].MatchOption);
            Assert.AreEqual(ruleData.Matches[1].Match, ruleNode.Matches[1].Value);
        }

        [TestMethod]
        public void CanCreateRuleDataFromPropertyMatchingRuleNode()
        {
            PropertyMatchingRuleNode ruleNode = new PropertyMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Matches.Add(new PropertyMatch("Property1", true, PropertyMatchingOption.GetOrSet));
            ruleNode.Matches.Add(new PropertyMatch("Property2", false, PropertyMatchingOption.Set));

            PropertyMatchingRuleData ruleData = ruleNode.GetConfigurationData() as PropertyMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.Matches[0].IgnoreCase, ruleData.Matches[0].IgnoreCase);
            Assert.AreEqual(ruleNode.Matches[0].Value, ruleData.Matches[0].Match);
            Assert.AreEqual(ruleNode.Matches[0].MatchOption, ruleData.Matches[0].MatchOption);
            Assert.AreEqual(ruleNode.Matches[1].IgnoreCase, ruleData.Matches[1].IgnoreCase);
            Assert.AreEqual(ruleNode.Matches[1].Value, ruleData.Matches[1].Match);
            Assert.AreEqual(ruleNode.Matches[1].MatchOption, ruleData.Matches[1].MatchOption);
        }

        [TestMethod]
        public void MatchWithEmptyValueCausesValidationError()
        {
            PropertyMatchingRuleNode ruleNode = new PropertyMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Matches.Add(new PropertyMatch("", false, PropertyMatchingOption.GetOrSet));

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, true, false);
            cmd.Execute(ruleNode);
            Assert.IsFalse(cmd.ValidationSucceeded);
        }
    }
}