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
    public class ReturnTypeMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            ReturnTypeMatchingRuleData returnTypeMatchingRule = new ReturnTypeMatchingRuleData("RuleName", "System.Int32");
            returnTypeMatchingRule.IgnoreCase = true;

            ReturnTypeMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(returnTypeMatchingRule) as ReturnTypeMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(returnTypeMatchingRule.Name, deserializedRule.Name);
            Assert.IsTrue(deserializedRule.IgnoreCase);
            Assert.AreEqual(returnTypeMatchingRule.Match, deserializedRule.Match);
        }
    }
}
