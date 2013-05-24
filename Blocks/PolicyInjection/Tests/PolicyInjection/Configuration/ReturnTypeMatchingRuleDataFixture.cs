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

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            ReturnTypeMatchingRuleData ruleData = new ReturnTypeMatchingRuleData("RuleName", "System.Int32");

            using (var container = new UnityContainer())
            {
                ruleData.ConfigureContainer(container, "-test");
                var registration = container.Registrations.Single(r => r.Name == "RuleName-test");
                Assert.AreSame(typeof(IMatchingRule), registration.RegisteredType);
                Assert.AreSame(typeof(ReturnTypeMatchingRule), registration.MappedToType);
                Assert.AreSame(typeof(TransientLifetimeManager), registration.LifetimeManagerType);
            }
        }
    }
}
