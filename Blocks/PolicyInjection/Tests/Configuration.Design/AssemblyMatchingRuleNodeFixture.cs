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
    public class AssemblyMatchingRuleNodeFixture
    {
        [TestMethod]
        public void AssemblyMatchingRuleNodeHasProperDefaultName()
        {
            AssemblyMatchingRuleNode ruleNode = new AssemblyMatchingRuleNode();
            Assert.AreEqual("Assembly Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateAssemblyMatchingRuleNodeFromData()
        {
            AssemblyMatchingRuleData ruleData = new AssemblyMatchingRuleData();
            ruleData.Name = "AsmRule";
            ruleData.Match = "Assembly1";

            AssemblyMatchingRuleNode ruleNode = new AssemblyMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.Match, ruleNode.AssemblyName);
        }

        [TestMethod]
        public void CanCreateRuleDataFromAssemblyMatchingRuleNode()
        {
            AssemblyMatchingRuleNode ruleNode = new AssemblyMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.AssemblyName = "AssemblyName";

            AssemblyMatchingRuleData ruleData = ruleNode.GetConfigurationData() as AssemblyMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.AssemblyName, ruleData.Match);
        }
    }
}
