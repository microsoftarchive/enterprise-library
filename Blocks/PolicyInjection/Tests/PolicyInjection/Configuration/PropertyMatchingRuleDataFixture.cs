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
    public class PropertyMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void ShouldSerializeAndDeserializeCorrectly()
        {
            PropertyMatchingRuleData original =
                new PropertyMatchingRuleData("MatchMyProperty",
                                             new PropertyMatchData[]
                                                 {
                                                     new PropertyMatchData("MyProperty", PropertyMatchingOption.Set, true),
                                                     new PropertyMatchData("*Name"),
                                                     new PropertyMatchData("Foo??", PropertyMatchingOption.Get)
                                                 });

            PropertyMatchingRuleData rehydrated =
                (PropertyMatchingRuleData)SerializeAndDeserializeMatchingRule(original);

            Assert.IsNotNull(rehydrated);
            Assert.AreEqual(original.Name, rehydrated.Name);
            Assert.AreEqual(original.Matches.Count, rehydrated.Matches.Count);
            for (int i = 0; i < original.Matches.Count; ++i)
            {
                AssertPropertyMatchEqual(original.Matches[i], rehydrated.Matches[i],
                                         "Match at index {0} is incorrect", i);
            }
        }

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            PropertyMatchingRuleData ruleData = new PropertyMatchingRuleData("Foo");

            using (var container = new UnityContainer())
            {
                ruleData.ConfigureContainer(container, "-test");
                var registration = container.Registrations.Single(r => r.Name == "Foo-test");
                Assert.AreSame(typeof(IMatchingRule), registration.RegisteredType);
                Assert.AreSame(typeof(PropertyMatchingRule), registration.MappedToType);
                Assert.AreSame(typeof(TransientLifetimeManager), registration.LifetimeManagerType);
            }
        }

        void AssertPropertyMatchEqual(PropertyMatchData expected,
                                      PropertyMatchData actual,
                                      string message,
                                      params object[] messageArgs)
        {
            Assert.AreEqual(expected.Match, actual.Match, message, messageArgs);
            Assert.AreEqual(expected.MatchOption, actual.MatchOption, message, messageArgs);
            Assert.AreEqual(expected.IgnoreCase, actual.IgnoreCase, message, messageArgs);
        }
    }
}
