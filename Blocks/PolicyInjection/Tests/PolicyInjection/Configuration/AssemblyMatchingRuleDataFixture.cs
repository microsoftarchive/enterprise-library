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

using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    [DeploymentItem("test.exe.config")]
    public class AssemblyNameMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {

#if !SILVERLIGHT
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            AssemblyMatchingRuleData asmMatchingRule = new AssemblyMatchingRuleData("RuleName", "mscorlib");

            AssemblyMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(asmMatchingRule) as AssemblyMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(asmMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(asmMatchingRule.Match, deserializedRule.Match);
        }
#else
        [TestMethod]
        public void HasDefaultCtor()
        {
            new AssemblyMatchingRuleData();
        }
#endif
        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            AssemblyMatchingRuleData asmMatchingRule = new AssemblyMatchingRuleData("RuleName", "mscorlib");
            TypeRegistration registration = asmMatchingRule.GetRegistrations("").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
}
