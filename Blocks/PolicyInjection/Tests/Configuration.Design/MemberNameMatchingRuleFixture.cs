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
    public class MemberNameMatchingRuleFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void MemberNameMatchingRuleNodeHasProperDefaultName()
        {
            MemberNameMatchingRuleNode ruleNode = new MemberNameMatchingRuleNode();
            Assert.AreEqual("Member Name Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateMemberNameMatchingRuleNodeFromData()
        {
            MemberNameMatchingRuleData ruleData = new MemberNameMatchingRuleData();
            ruleData.Name = "name o' rule";
            ruleData.Matches.Add(new MatchData("MemberName1", false));
            ruleData.Matches.Add(new MatchData("MemberName2", true));

            MemberNameMatchingRuleNode ruleNode = new MemberNameMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.Matches.Count, ruleNode.Matches.Count);
            Assert.AreEqual(ruleData.Matches[0].IgnoreCase, ruleNode.Matches[0].IgnoreCase);
            Assert.AreEqual(ruleData.Matches[0].Match, ruleNode.Matches[0].Value);
            Assert.AreEqual(ruleData.Matches[1].IgnoreCase, ruleNode.Matches[1].IgnoreCase);
            Assert.AreEqual(ruleData.Matches[1].Match, ruleNode.Matches[1].Value);
        }

        [TestMethod]
        public void CanCreateRuleDataFromMemberNameMatchingRuleNode()
        {
            MemberNameMatchingRuleNode ruleNode = new MemberNameMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Matches.Add(new Match("NametoMatch1", false));
            ruleNode.Matches.Add(new Match("NametoMatch2", true));

            MemberNameMatchingRuleData ruleData = ruleNode.GetConfigurationData() as MemberNameMatchingRuleData;

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
            MemberNameMatchingRuleNode ruleNode = new MemberNameMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Matches.Add(new Match("", false));

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, true, false);
            cmd.Execute(ruleNode);
            Assert.IsFalse(cmd.ValidationSucceeded);
        }
    }
}