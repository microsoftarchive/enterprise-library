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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class NamespaceMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            NamespaceMatchingRuleData namespaceMatchingRule =
                new NamespaceMatchingRuleData("RuleName",
                                              new MatchData[]
                                                  {
                                                      new MatchData("System.*"),
                                                      new MatchData("microsoft.*", true),
                                                      new MatchData("Microsoft.Practices.EnterpriseLibrary.PolicyInjection")
                                                  });

            Assert.AreEqual(3, namespaceMatchingRule.Matches.Count);
            NamespaceMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(namespaceMatchingRule) as NamespaceMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(namespaceMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(namespaceMatchingRule.Matches.Count, deserializedRule.Matches.Count);
            for (int i = 0; i < namespaceMatchingRule.Matches.Count; ++i)
            {
                AssertMatchDataEqual(namespaceMatchingRule.Matches[i],
                                     deserializedRule.Matches[i],
                                     "Match data at index {0} is incorrect", i);
            }
        }

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            NamespaceMatchingRuleData ruleData = new NamespaceMatchingRuleData("ruleName", "Foo");
            TypeRegistration registration = ruleData.GetRegistrations("").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
}
