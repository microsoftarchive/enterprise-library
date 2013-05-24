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
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class GivenANewPolicyData
    {
        private PolicyData policyData;

        [TestInitialize]
        public void Setup()
        {
            policyData = new PolicyData("policy");
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenCanResolvePolicy()
        {
            using (var container = new UnityContainer())
            {
                this.policyData.ConfigureContainer(container);

                var policy = container.Resolve<RuleDrivenPolicy>("policy");

                var method = new MethodImplementationInfo(StaticReflection.GetMethodInfo<object>(o => o.ToString()), StaticReflection.GetMethodInfo<object>(o => o.ToString()));

                Assert.AreEqual("policy", policy.Name);
                Assert.IsFalse(policy.Matches(method));
                Assert.AreEqual(0, policy.GetHandlersFor(method, container).Count());
            }
        }
    }

    [TestClass]
    public class GivenAPolicyDataWithAMatchingRule
    {
        private PolicyData policyData;

        [TestInitialize]
        public void Setup()
        {
            policyData = new PolicyData("policy");
            policyData.MatchingRules.Add(new TypeMatchingRuleData("type", typeof(object).FullName));
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenCanResolvePolicy()
        {
            using (var container = new UnityContainer())
            {
                this.policyData.ConfigureContainer(container);

                var policy = container.Resolve<RuleDrivenPolicy>("policy");

                var method = new MethodImplementationInfo(StaticReflection.GetMethodInfo<object>(o => o.ToString()), StaticReflection.GetMethodInfo<object>(o => o.ToString()));

                Assert.AreEqual("policy", policy.Name);
                Assert.IsTrue(policy.Matches(method));
                Assert.AreEqual(0, policy.GetHandlersFor(method, container).Count());

                Assert.AreNotSame(policy, container.Resolve<RuleDrivenPolicy>("policy"));

                Assert.IsTrue(container.Registrations.Any(r => r.Name == "type-policy" && r.RegisteredType == typeof(IMatchingRule) && r.MappedToType == typeof(TypeMatchingRule)));
            }
        }
    }
}
