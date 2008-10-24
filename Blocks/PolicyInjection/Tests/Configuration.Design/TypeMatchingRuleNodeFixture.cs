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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class TypeMatchingRuleNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void TypeMatchingRuleNodeHasProperDefaultName()
        {
            TypeMatchingRuleNode ruleNode = new TypeMatchingRuleNode();
            Assert.AreEqual("Type Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateTypeMatchingRuleNodeFromData()
        {
            TypeMatchingRuleData ruleData = new TypeMatchingRuleData();
            ruleData.Name = "name o' rule";
            ruleData.Matches.Add(new MatchData("TypeName", false));
            ruleData.Matches.Add(new MatchData("TypeName2", true));

            TypeMatchingRuleNode ruleNode = new TypeMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.Matches.Count, ruleNode.Matches.Count);
            Assert.AreEqual(ruleData.Matches[0].IgnoreCase, ruleNode.Matches[0].IgnoreCase);
            Assert.AreEqual(ruleData.Matches[0].Match, ruleNode.Matches[0].Value);
            Assert.AreEqual(ruleData.Matches[1].IgnoreCase, ruleNode.Matches[1].IgnoreCase);
            Assert.AreEqual(ruleData.Matches[1].Match, ruleNode.Matches[1].Value);
        }

        [TestMethod]
        public void CanCreateRuleDataFromTypeMatchingRuleNode()
        {
            TypeMatchingRuleNode ruleNode = new TypeMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Matches.Add(new Match("TypeName1", false));
            ruleNode.Matches.Add(new Match("TypeName2", true));

            TypeMatchingRuleData ruleData = ruleNode.GetConfigurationData() as TypeMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.Matches[0].IgnoreCase, ruleData.Matches[0].IgnoreCase);
            Assert.AreEqual(ruleNode.Matches[0].Value, ruleData.Matches[0].Match);
            Assert.AreEqual(ruleNode.Matches[1].IgnoreCase, ruleData.Matches[1].IgnoreCase);
            Assert.AreEqual(ruleNode.Matches[1].Value, ruleData.Matches[1].Match);
        }

        [TestMethod]
        public void MatchWithEmptyValueCausesValidationError()
        {
            TypeMatchingRuleNode ruleNode = new TypeMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Matches.Add(new Match("", false));

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, true, false);
            cmd.Execute(ruleNode);
            Assert.IsFalse(cmd.ValidationSucceeded);
        }
    }
}
