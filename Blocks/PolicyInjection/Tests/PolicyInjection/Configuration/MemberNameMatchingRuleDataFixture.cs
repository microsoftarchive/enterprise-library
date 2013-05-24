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
    public class MemberNameMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            MemberNameMatchingRuleData memberNameMatchingRule =
                new MemberNameMatchingRuleData("MatchThis", new MatchData[]
                                                                {
                                                                    new MatchData("ToString"),
                                                                    new MatchData("GetHashCode", true),
                                                                    new MatchData("Get*", false)
                                                                });

            MemberNameMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(memberNameMatchingRule) as MemberNameMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(memberNameMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(memberNameMatchingRule.Matches.Count, deserializedRule.Matches.Count);
            for (int i = 0; i < deserializedRule.Matches.Count; ++i)
            {
                AssertMatchDataEqual(memberNameMatchingRule.Matches[0],
                                     deserializedRule.Matches[0],
                                     "Match item {0} is incorrect", i);
            }
        }

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            MemberNameMatchingRuleData memberNameMatchingRule = new MemberNameMatchingRuleData("MatchThis");

            using (var container = new UnityContainer())
            {
                memberNameMatchingRule.ConfigureContainer(container, "-test");
                var registration = container.Registrations.Single(r => r.Name == "MatchThis-test");
                Assert.AreSame(typeof(IMatchingRule), registration.RegisteredType);
                Assert.AreSame(typeof(MemberNameMatchingRule), registration.MappedToType);
                Assert.AreSame(typeof(TransientLifetimeManager), registration.LifetimeManagerType);
            }
        }
    }
}
