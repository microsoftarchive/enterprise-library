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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            using (var container = new UnityContainer())
            {
                ruleData.ConfigureContainer(container, "-test");
                var registration = container.Registrations.Single(r => r.Name == "ruleName-test");
                Assert.AreSame(typeof(IMatchingRule), registration.RegisteredType);
                Assert.AreSame(typeof(NamespaceMatchingRule), registration.MappedToType);
                Assert.AreSame(typeof(TransientLifetimeManager), registration.LifetimeManagerType);
            }
        }
    }
}
