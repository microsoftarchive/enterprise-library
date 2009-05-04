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
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class ParameterTypeMatchingRuleNodeFixture
    {
        [TestMethod]
        public void ShouldHaveCorrectDefaults()
        {
            ParameterTypeMatchingRuleNode node = new ParameterTypeMatchingRuleNode();
            Assert.AreEqual("Parameter Type Matching Rule", node.Name);
            Assert.AreEqual(0, node.Matches.Count);
        }

        [TestMethod]
        public void ShouldBeAbleToConstructFromConfigData()
        {
            ParameterTypeMatchingRuleData ruleData = new ParameterTypeMatchingRuleData("RuleName");
            ruleData.Matches.Add(new ParameterTypeMatchData("System.Double", ParameterKind.InputOrOutput));
            ruleData.Matches.Add(new ParameterTypeMatchData("ParameterTypeMatchingRuleNodeFixture"));

            ParameterTypeMatchingRuleNode node = new ParameterTypeMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, node.Name);
            Assert.AreEqual(ruleData.Matches.Count, node.Matches.Count);
            for (int i = 0; i < ruleData.Matches.Count; ++i)
            {
                AssertAreEquivalent(ruleData.Matches[i], node.Matches[i], "Mismatch at element {0}", i);
            }
        }

        [TestMethod]
        public void ShouldBeAbleToGetConfigDataFromNode()
        {
            ParameterTypeMatchingRuleNode node = new ParameterTypeMatchingRuleNode();
            node.Matches.Add(new ParameterTypeMatch("Foo", true, ParameterKind.InputOrOutput));
            node.Matches.Add(new ParameterTypeMatch("String", false, ParameterKind.ReturnValue));
            node.Matches.Add(new ParameterTypeMatch("System.Decimal", false, ParameterKind.Output));

            ParameterTypeMatchingRuleData ruleData = (ParameterTypeMatchingRuleData)node.GetConfigurationData();

            Assert.AreEqual(node.Name, ruleData.Name);
            Assert.AreEqual(node.Matches.Count, ruleData.Matches.Count);
            for (int i = 0; i < node.Matches.Count; ++i)
            {
                AssertAreEquivalent(ruleData.Matches[i], node.Matches[i], "Mismatch at element {0}", i);
            }
        }

        void AssertAreEquivalent(ParameterTypeMatchData matchData,
                                 ParameterTypeMatch nodeMatch,
                                 string message,
                                 params object[] args)
        {
            Assert.AreEqual(matchData.Match, nodeMatch.Value, message, args);
            Assert.AreEqual(matchData.IgnoreCase, nodeMatch.IgnoreCase, message, args);
            Assert.AreEqual(matchData.ParameterKind, nodeMatch.Kind, message, args);
        }
    }
}
