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
    [DeploymentItem("test.exe.config")]
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
            TypeRegistration registration = ruleData.GetRegistrations("").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
}
