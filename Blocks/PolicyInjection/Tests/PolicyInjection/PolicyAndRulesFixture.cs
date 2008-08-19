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
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    /// <summary>
    /// Summary description for PolicyAndRulesFixture
    /// </summary>
    [TestClass]
    public class PolicyAndRulesFixture
    {
        [TestMethod]
        public void ShouldNotMatchTypeWithNoRules()
        {
            MatchingRuleSet ruleSet = new MatchingRuleSet();
            Assert.IsFalse(ruleSet.Matches(typeof(MockDal)));
            Assert.IsFalse(ruleSet.Matches(typeof(string)));
        }

        [TestMethod]
        public void ShouldMatchTypeWithTypeMatchRule()
        {
            MatchingRuleSet ruleSet = new MatchingRuleSet();
            ruleSet.Add(new TypeMatchingAssignmentRule(typeof(MockDal)));
            Assert.IsTrue(ruleSet.Matches(typeof(MockDal)));
        }

        [TestMethod]
        public void ShouldNotMatchPolicyWithNoRules()
        {
            Policy policy = new Policy("DALPolicy");
            Assert.IsFalse(policy.AppliesTo(typeof(MockDal)));
        }

        [TestMethod]
        public void ShouldMatchPolicyWithRules()
        {
            Policy policy = new Policy("DALPolicy");
            policy.RuleSet.Add(new TypeMatchingAssignmentRule(typeof(MockDal)));
            ExceptionSwizzlerHandler handler = new ExceptionSwizzlerHandler();

            policy.Handlers.Add(handler);

            Assert.IsTrue(policy.AppliesTo(typeof(MockDal)));
            Assert.IsTrue(policy.Handlers.Contains(handler));
        }


        [TestMethod]
        public void ShouldReturnAllPoliciesInOrderThatApplyToTarget()
        {
            PolicySet policies = new PolicySet();
            Policy p1 = new Policy("DALPolicy");
            p1.RuleSet.Add(new TypeMatchingAssignmentRule(typeof(MockDal)));
            Policy p2 = new Policy("LoggingPolicy");
            p2.RuleSet.Add(new TypeMatchingAssignmentRule(typeof(string)));
            Policy p3 = new Policy("ExceptionPolicy");
            p3.RuleSet.Add(new TypeMatchingAssignmentRule(typeof(MockDal)));

            policies.Add(p1);
            policies.Add(p2);
            policies.Add(p3);

            PolicySet matchingPolicies = policies.GetPoliciesFor(typeof(MockDal));
            Assert.AreEqual(2, matchingPolicies.Count);
            Assert.AreSame(p1, matchingPolicies[0]);
            Assert.AreSame(p3, matchingPolicies[1]);
        }

        [TestMethod]
        public void ShouldReturnPoliciesForTypeAndMember()
        {
            Policy policy = new Policy("DALPolicy");
            policy.RuleSet.Add(new TypeMatchingAssignmentRule(typeof(MockDal)));
            policy.RuleSet.Add(new MemberNameMatchingRule("DoSomething"));

            ExceptionSwizzlerHandler handler = new ExceptionSwizzlerHandler();

            policy.Handlers.Add(handler);

            MemberInfo doSomethingMember = typeof(MockDal).GetMember("DoSomething")[0];
            MemberInfo toStringMember = typeof(MockDal).GetMember("ToString")[0];

            List<ICallHandler> doSomethingHandlers =
                new List<ICallHandler>(policy.GetHandlersFor(doSomethingMember));
            List<ICallHandler> toStringHandlers =
                new List<ICallHandler>(policy.GetHandlersFor(toStringMember));

            Assert.AreEqual(1, doSomethingHandlers.Count);
            Assert.AreEqual(0, toStringHandlers.Count);
            Assert.IsTrue(policy.Handlers.Contains(handler));
        }
    }
}
