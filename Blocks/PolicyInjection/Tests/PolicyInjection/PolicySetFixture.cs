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
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    /// <summary>
    /// Tests for the PolicySet class.
    /// </summary>
    [TestClass]
    public class PolicySetFixture
    {
        [TestMethod]
        public void ShouldInitializeToEmpty()
        {
            PolicySet policies = new PolicySet();

            Assert.IsFalse(policies.AppliesTo(GetType()));
            MethodInfo thisMember = GetType().GetMethod("ShouldInitializeToEmpty");
            List<ICallHandler> handlers = new List<ICallHandler>(policies.GetHandlersFor(thisMember));
            Assert.AreEqual(0, handlers.Count);
        }

        [TestMethod]
        public void ShouldBeAbleToAddOnePolicy()
        {
            PolicySet policies = new PolicySet();

            RuleDrivenPolicy p = new RuleDrivenPolicy("NameMatching");
            p.RuleSet.Add(new MemberNameMatchingRule("ShouldBeAbleToAddOnePolicy"));
            p.Handlers.Add(new Handler1());
            p.Handlers.Add(new Handler2());

            policies.Add(p);

            MethodBase thisMember = GetType().GetMethod("ShouldBeAbleToAddOnePolicy");
            List<ICallHandler> handlers =
                new List<ICallHandler>(policies.GetHandlersFor(thisMember));

            Assert.IsTrue(policies.AppliesTo(GetType()));
            Assert.AreEqual(2, handlers.Count);
            Assert.IsTrue(typeof(Handler1) == handlers[0].GetType());
            Assert.IsTrue(typeof(Handler2) == handlers[1].GetType());
        }

        [TestMethod]
        public void ShouldMatchPolicyByTypeName()
        {
            PolicySet policies = GetMultiplePolicySet();
            Assert.IsTrue(policies.AppliesTo(typeof(MatchesByType)));

            MethodInfo nameDoesntMatchMember = typeof(MatchesByType).GetMethod("NameDoesntMatch");
            MethodInfo nameMatchMember = typeof(MatchesByType).GetMethod("NameMatch");

            List<ICallHandler> nameDoesntMatchHandlers =
                new List<ICallHandler>(policies.GetHandlersFor(nameDoesntMatchMember));
            List<ICallHandler> nameMatchHandlers =
                new List<ICallHandler>(policies.GetHandlersFor(nameMatchMember));

            Assert.AreEqual(1, nameDoesntMatchHandlers.Count);
            Assert.IsTrue(typeof(Handler1) == nameDoesntMatchHandlers[0].GetType());

            Assert.AreEqual(2, nameMatchHandlers.Count);
            Assert.IsTrue(typeof(Handler1) == nameMatchHandlers[0].GetType());
            Assert.IsTrue(typeof(Handler2) == nameMatchHandlers[1].GetType());
        }

        [TestMethod]
        public void ShouldMatchPolicyByMethodName()
        {
            PolicySet policies = GetMultiplePolicySet();

            Assert.IsTrue(policies.AppliesTo(typeof(MatchesByMemberName)));

            MethodInfo noMatchMember = typeof(MatchesByMemberName).GetMethod("NoMatch");
            MethodInfo nameMatchMember = typeof(MatchesByMemberName).GetMethod("NameMatch");
            List<ICallHandler> noMatchHandlers =
                new List<ICallHandler>(policies.GetHandlersFor(noMatchMember));
            List<ICallHandler> nameMatchHandlers =
                new List<ICallHandler>(policies.GetHandlersFor(nameMatchMember));

            Assert.AreEqual(0, noMatchHandlers.Count);
            Assert.AreEqual(1, nameMatchHandlers.Count);
            Assert.IsTrue(typeof(Handler2) == nameMatchHandlers[0].GetType());
        }

        [TestMethod]
        public void ShouldNotMatchPolicyWhenNoRulesMatch()
        {
            PolicySet policies = GetMultiplePolicySet();
            Assert.IsFalse(policies.AppliesTo(typeof(NoMatchAnywhere)));

            MethodBase noMatchMember = typeof(NoMatchAnywhere).GetMethod("NoMatchHere");
            List<ICallHandler> noMatchHandlers =
                new List<ICallHandler>(policies.GetHandlersFor(noMatchMember));
            Assert.AreEqual(0, noMatchHandlers.Count);
        }

        [TestMethod]
        public void ShouldClearCachesAfterChangesToPolicySet()
        {
            PolicySet policies = GetMultiplePolicySet();
            List<ICallHandler> handlersBefore =
                new List<ICallHandler>(
                    policies.GetHandlersFor(GetNameDoesntMatchMethod()));

            Assert.AreEqual(1, handlersBefore.Count);

            RuleDrivenPolicy newPolicy = new RuleDrivenPolicy("MatchesAnotherName");
            newPolicy.RuleSet.Add(new MemberNameMatchingRule("NameDoesntMatch"));
            newPolicy.Handlers.Add(new Handler2());

            policies.Add(newPolicy);

            List<ICallHandler> handlersAfter =
                new List<ICallHandler>(
                    policies.GetHandlersFor(GetNameDoesntMatchMethod()));

            Assert.AreEqual(2, handlersAfter.Count);

            newPolicy.Handlers.Add(new Handler3());
        }

        [TestMethod]
        public void ShouldClearCachesAfterChangesToPolicyInPolicySet()
        {
            PolicySet policies = GetMultiplePolicySet();

            List<ICallHandler> handlersBefore =
                new List<ICallHandler>(policies.GetHandlersFor(
                                           GetNameDoesntMatchMethod()));
            Assert.AreEqual(1, handlersBefore.Count);

            ((RuleDrivenPolicy)policies[2]).RuleSet.RemoveAt(0);
            ((RuleDrivenPolicy)policies[2]).RuleSet.Add(new MemberNameMatchingRule("NameDoesntMatch"));

            List<ICallHandler> handlersAfterChangingRule =
                new List<ICallHandler>(
                    policies.GetHandlersFor(GetNameDoesntMatchMethod()));

            Assert.AreEqual(2, handlersAfterChangingRule.Count);
        }

        [TestMethod]
        public void ShouldGetCorrectHandlersGivenAttributesOnInterfaceMethods()
        {
            PolicySet policies = new PolicySet();

            List<ICallHandler> oneHandlers = new List<ICallHandler>(policies.GetHandlersFor(
                                                                        typeof(Bar).GetMethod("One")));

            Assert.AreEqual(2, oneHandlers.Count);
            Assert.IsTrue(oneHandlers[0] is MarkerCallHandler);
            Assert.IsTrue(oneHandlers[1] is MarkerCallHandler);

            Assert.AreEqual("BarOneOverride", ((MarkerCallHandler)oneHandlers[0]).HandlerName);
            Assert.AreEqual("IOneOne", ((MarkerCallHandler)oneHandlers[1]).HandlerName);
        }

        [TestMethod]
        public void ShouldNotDuplicateHandlersWhenCreatingViaInterface()
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            RuleDrivenPolicy policy = new RuleDrivenPolicy("MatchesInterfacePolicy");
            policy.RuleSet.Add(new TypeMatchingRule("ITwo"));
            policy.Handlers.Add(new ValidationCallHandler(string.Empty, SpecificationSource.Both));
            policy.Handlers.Add(new CallCountHandler());

            PolicySet policies = new PolicySet(policy);
            List<ICallHandler> handlers = new List<ICallHandler>(policies.GetHandlersFor(typeof(Bar).GetMethod("Two")));
            Assert.AreEqual(2, handlers.Count);
        }

        [TestMethod]
        public void HandlersOrderedProperly()
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy("MatchesInterfacePolicy");
            policy.RuleSet.Add(new TypeMatchingRule("ITwo"));

            ValidationCallHandler validationCallHandler = new ValidationCallHandler(string.Empty, SpecificationSource.Both, 3);
            policy.Handlers.Add(validationCallHandler);

            CachingCallHandler cachingCallHandler = new CachingCallHandler(new TimeSpan(0, 0, 30), 0);
            policy.Handlers.Add(cachingCallHandler);

            LogCallHandler logCallHandler = new LogCallHandler();
            logCallHandler.Order = 2;
            policy.Handlers.Add(logCallHandler);

            AuthorizationCallHandler authorizationCallHandler = new AuthorizationCallHandler("providerName", "operacionName", null, 1);
            policy.Handlers.Add(authorizationCallHandler);

            PolicySet policies = new PolicySet(policy);
            List<ICallHandler> handlers = new List<ICallHandler>(policies.GetHandlersFor(typeof(Bar).GetMethod("Two")));

            Assert.AreEqual(authorizationCallHandler, handlers[0]);
            Assert.AreEqual(logCallHandler, handlers[1]);
            Assert.AreEqual(validationCallHandler, handlers[2]);
            Assert.AreEqual(cachingCallHandler, handlers[3]);
        }

        [TestMethod]
        public void HandlersOrderedProperlyUsingRelativeAndAbsoluteOrder()
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy("MatchesInterfacePolicy");
            policy.RuleSet.Add(new TypeMatchingRule("ITwo"));

            PerformanceCounterCallHandler perfomanceCounterCallHandler = new PerformanceCounterCallHandler("category", "instanceName");
            perfomanceCounterCallHandler.Order = 0;
            policy.Handlers.Add(perfomanceCounterCallHandler);

            ValidationCallHandler validationCallHandler = new ValidationCallHandler(string.Empty, SpecificationSource.Both, 3);
            policy.Handlers.Add(validationCallHandler);

            CachingCallHandler cachingCallHandler = new CachingCallHandler(new TimeSpan(0, 0, 30), 3);
            policy.Handlers.Add(cachingCallHandler);

            LogCallHandler logCallHandler = new LogCallHandler();
            logCallHandler.Order = 2;
            policy.Handlers.Add(logCallHandler);

            CallCountHandler callCountHandler = new CallCountHandler();
            callCountHandler.Order = 4;
            policy.Handlers.Add(callCountHandler);

            AuthorizationCallHandler authorizationCallHandler = new AuthorizationCallHandler("providerName", "operacionName", null, 1);
            policy.Handlers.Add(authorizationCallHandler);

            PolicySet policies = new PolicySet(policy);
            List<ICallHandler> handlers = new List<ICallHandler>(policies.GetHandlersFor(typeof(Bar).GetMethod("Two")));

            Assert.AreEqual(authorizationCallHandler, handlers[0]);
            Assert.AreEqual(logCallHandler, handlers[1]);
            Assert.AreEqual(validationCallHandler, handlers[2]);
            Assert.AreEqual(cachingCallHandler, handlers[3]);
            Assert.AreEqual(callCountHandler, handlers[4]);
            Assert.AreEqual(perfomanceCounterCallHandler, handlers[5]);
        }

        PolicySet GetMultiplePolicySet()
        {
            RuleDrivenPolicy typeMatchPolicy = new RuleDrivenPolicy("MatchesType");
            typeMatchPolicy.RuleSet.Add(new TypeMatchingRule(typeof(MatchesByType)));
            typeMatchPolicy.Handlers.Add(new Handler1());

            RuleDrivenPolicy nameMatchPolicy = new RuleDrivenPolicy("MatchesName");
            nameMatchPolicy.RuleSet.Add(new MemberNameMatchingRule("NameMatch"));
            nameMatchPolicy.Handlers.Add(new Handler2());

            return new PolicySet(typeMatchPolicy, nameMatchPolicy);
        }

        MethodInfo GetNameDoesntMatchMethod()
        {
            return typeof(MatchesByType).GetMethod("NameDoesntMatch");
        }
    }

    class MatchesByType
    {
        // Matches type policy
        public void NameDoesntMatch() {}

        // matches type & name policies
        public void NameMatch() {}
    }

    class MatchesByMemberName
    {
        public void NameMatch() {}

        public void NoMatch() {}
    }

    class NoMatchAnywhere
    {
        public void NoMatchHere() {}
    }

    public interface IOne
    {
        [MarkerCallHandler("IOneOne")]
        void One();
    }

    public interface ITwo
    {
        void Two();
    }

    public class Foo : IOne
    {
        public void FooOne() {}

        public virtual void FooTwo() {}

        public virtual void One() {}
    }

    public class Bar : Foo, ITwo
    {
        public void BarOne() {}

        public override void FooTwo() {}

        [MarkerCallHandler("BarOneOverride")]
        public override void One() {}

        public void Two() {}
    }

    public class MarkerCallHandler : ICallHandler
    {
        string handlerName;
        int order = 0;

        public MarkerCallHandler(string handlerName)
        {
            this.handlerName = handlerName;
        }

        public string HandlerName
        {
            get { return handlerName; }
            set { handlerName = value; }
        }

        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public IMethodReturn Invoke(IMethodInvocation input,
                                    GetNextHandlerDelegate getNext)
        {
            return getNext()(input, getNext);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class MarkerCallHandlerAttribute : HandlerAttribute
    {
        string handlerName;

        public MarkerCallHandlerAttribute(string handlerName)
        {
            this.handlerName = handlerName;
        }

        public override ICallHandler CreateHandler()
        {
            return new MarkerCallHandler(handlerName);
        }
    }
}