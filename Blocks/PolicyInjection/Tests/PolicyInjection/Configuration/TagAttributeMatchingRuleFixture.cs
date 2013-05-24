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
    public class TagAttributeMatchingRuleFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            TagAttributeMatchingRuleData tagAttributeMatchingRule = new TagAttributeMatchingRuleData("RuleName", "Tag");
            tagAttributeMatchingRule.IgnoreCase = true;

            TagAttributeMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(tagAttributeMatchingRule) as TagAttributeMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(tagAttributeMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(tagAttributeMatchingRule.IgnoreCase, deserializedRule.IgnoreCase);
            Assert.AreEqual(tagAttributeMatchingRule.Match, deserializedRule.Match);
        }

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            TagAttributeMatchingRuleData ruleData = new TagAttributeMatchingRuleData("RuleName", "TAg");

            using (var container = new UnityContainer())
            {
                ruleData.ConfigureContainer(container, "-test");
                var registration = container.Registrations.Single(r => r.Name == "RuleName-test");
                Assert.AreSame(typeof(IMatchingRule), registration.RegisteredType);
                Assert.AreSame(typeof(TagAttributeMatchingRule), registration.MappedToType);
                Assert.AreSame(typeof(TransientLifetimeManager), registration.LifetimeManagerType);
            }
        }
    }
}
