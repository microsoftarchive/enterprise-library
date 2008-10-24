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

using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class CustomAttributeMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            CustomAttributeMatchingRuleData customAttributeMatchingRule = new CustomAttributeMatchingRuleData("MatchesMyAttribure", "Namespace.MyAttribute, Assembly", true);

            CustomAttributeMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(customAttributeMatchingRule) as CustomAttributeMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(customAttributeMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(customAttributeMatchingRule.TypeName, deserializedRule.TypeName);
            Assert.AreEqual(customAttributeMatchingRule.SearchInheritanceChain, deserializedRule.SearchInheritanceChain);
        }
    }
}
