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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class ReturnTypeMatchingRuleNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ReturnTypeMatchingRuleNodeHasProperDefaultName()
        {
            ReturnTypeMatchingRuleNode ruleNode = new ReturnTypeMatchingRuleNode();
            Assert.AreEqual("Return Type Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateReturnTypeMatchingRuleNodeFromData()
        {
            ReturnTypeMatchingRuleData ruleData = new ReturnTypeMatchingRuleData();
            ruleData.Name = "name o' rule";
            ruleData.Match = "System.Return.Type";
            ruleData.IgnoreCase = false;

            ReturnTypeMatchingRuleNode ruleNode = new ReturnTypeMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.Match, ruleNode.Match);
            Assert.AreEqual(ruleData.IgnoreCase, ruleNode.IgnoreCase);
        }

        [TestMethod]
        public void CanCreateRuleDataFromReturnTypeMatchingRuleNode()
        {
            ReturnTypeMatchingRuleNode ruleNode = new ReturnTypeMatchingRuleNode();
            ruleNode.Name = "name o' rule";
            ruleNode.Match = "System.Return.Type";
            ruleNode.IgnoreCase = false;

            ReturnTypeMatchingRuleData ruleData = ruleNode.GetConfigurationData() as ReturnTypeMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.Match, ruleData.Match);
            Assert.AreEqual(ruleNode.IgnoreCase, ruleData.IgnoreCase);
        }
    }
}
