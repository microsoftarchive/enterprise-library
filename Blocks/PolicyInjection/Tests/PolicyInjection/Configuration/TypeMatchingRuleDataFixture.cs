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
    public class TypeMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            TypeMatchingRuleData typeMatchingRule = new TypeMatchingRuleData("RuleName",
                                                                             new MatchData[]
                                                                                 {
                                                                                     new MatchData("System.String"),
                                                                                     new MatchData("mydataobject", true),
                                                                                     new MatchData("Foo")
                                                                                 });

            TypeMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(typeMatchingRule) as TypeMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(typeMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(typeMatchingRule.Matches.Count, deserializedRule.Matches.Count);
            for (int i = 0; i < typeMatchingRule.Matches.Count; ++i)
            {
                AssertMatchDataEqual(typeMatchingRule.Matches[i],
                                     deserializedRule.Matches[i],
                                     "The match at index {0} is incorrect", i);
            }
        }

        
        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            TypeMatchingRuleData ruleData = new TypeMatchingRuleData("RuleName", "System.Int32");
            TypeRegistration registration = ruleData.GetRegistrations("").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
}
