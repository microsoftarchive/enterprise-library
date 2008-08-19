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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class AttributeDrivenPolicyFixture
    {
        MethodInfo nothingSpecialMethod;
        MethodInfo doSomethingMethod;
        MethodInfo getCriticalInfoMethod;
        MethodInfo mustBeFastMethod;
        MethodInfo getNameMethod;
        MethodInfo hasAttributeMethod;
        MethodInfo doesntHaveAttributeMethod;
        MethodInfo aNewMethod;

        [TestInitialize]
        public void Setup()
        {
            Type targetType = typeof(AttributeTestTarget);
            nothingSpecialMethod = targetType.GetMethod("NothingSpecial");
            doSomethingMethod = targetType.GetMethod("DoSomething");
            getCriticalInfoMethod = targetType.GetMethod("GetCriticalInformation");
            mustBeFastMethod = targetType.GetMethod("MustBeFast");
            getNameMethod = targetType.GetProperty("Name").GetGetMethod();
            hasAttributeMethod = typeof(SecondAttributeTestTarget).GetMethod("HasAttribute");
            doesntHaveAttributeMethod = typeof(SecondAttributeTestTarget).GetMethod("DoesntHaveAttribute");
            aNewMethod = typeof(DerivedAttributeTestTarget).GetMethod("ANewMethod");
        }

        [TestMethod]
        public void MatchingRuleMatchesForAllMethodsInAttributeTestTarget()
        {
            IMatchingRule rule = new AttributeDrivenPolicyMatchingRule();
            Assert.IsTrue(rule.Matches(nothingSpecialMethod));
            Assert.IsTrue(rule.Matches(doSomethingMethod));
            Assert.IsTrue(rule.Matches(getCriticalInfoMethod));
            Assert.IsTrue(rule.Matches(mustBeFastMethod));
        }

        [TestMethod]
        public void MatchingRuleOnlyMatchesOnMethodsWithAttributes()
        {
            IMatchingRule rule = new AttributeDrivenPolicyMatchingRule();

            Assert.IsTrue(rule.Matches(hasAttributeMethod));
            Assert.IsFalse(rule.Matches(doesntHaveAttributeMethod));
        }

        [TestMethod]
        public void ShouldMatchInheritedHandlerAttributes()
        {
            IMatchingRule rule = new AttributeDrivenPolicyMatchingRule();
            Assert.IsTrue(rule.Matches(aNewMethod));
        }

        [TestMethod]
        public void ShouldHaveAttributePolicyApplyToTypesWithAttributes()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();

            Assert.IsTrue(policy.AppliesTo(typeof(AttributeTestTarget)));
            Assert.IsTrue(policy.AppliesTo(typeof(SecondAttributeTestTarget)));
            Assert.IsTrue(policy.AppliesTo(typeof(DerivedAttributeTestTarget)));
            Assert.IsFalse(policy.AppliesTo(typeof(MockDal)));
        }

        [TestMethod]
        public void ShouldHaveAttributesCauseMatchesOnMethods()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();

            Assert.IsTrue(policy.Matches(nothingSpecialMethod));
            Assert.IsFalse(policy.Matches(mustBeFastMethod));
        }

        [TestMethod]
        public void ShouldGetCorrectHandlersForMethods()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(nothingSpecialMethod));

            Assert.AreEqual(1, handlers.Count);
            Assert.AreSame(typeof(LogCallHandler), handlers[0].GetType());
        }

        [TestMethod]
        public void ShouldGetHandlersFromClassAndMethodAttributes()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(doSomethingMethod));
            Assert.AreEqual(2, handlers.Count);
            Assert.AreSame(typeof(LogCallHandler), handlers[0].GetType());
            Assert.AreSame(typeof(ValidationCallHandler), handlers[1].GetType());
        }

        [TestMethod]
        public void ShouldGetNoHandlersIfApplyNoPoliciesIsPresent()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(mustBeFastMethod));
            Assert.AreEqual(0, handlers.Count);
        }

        [TestMethod]
        public void ShouldHaveLoggingHandlerForNothingSpecial()
        {
            PolicySet policies = GetPolicySet();
            List<ICallHandler> handlers = new List<ICallHandler>(policies.GetHandlersFor(nothingSpecialMethod));
            Assert.AreEqual(1, handlers.Count);
            Assert.AreSame(typeof(LogCallHandler), handlers[0].GetType());
        }

        [TestMethod]
        public void ShouldHaveLoggingAndValidationForDoSomething()
        {
            PolicySet policies = GetPolicySet();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policies.GetHandlersFor(doSomethingMethod));

            Assert.AreEqual(2, handlers.Count);
            Assert.AreSame(typeof(LogCallHandler), handlers[0].GetType());
            Assert.AreSame(typeof(ValidationCallHandler), handlers[1].GetType());
        }

        [TestMethod]
        public void ShouldApplyHandlersIfAttributesAreOnProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(getNameMethod));
            Assert.AreEqual(2, handlers.Count);
            Assert.AreSame(typeof(LogCallHandler), handlers[0].GetType());
            Assert.AreSame(typeof(ValidationCallHandler), handlers[1].GetType());
        }

        [TestMethod]
        public void ShouldInheritHandlersFromBaseClass()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers = new List<ICallHandler>(policy.GetHandlersFor(aNewMethod));
            Assert.AreEqual(1, handlers.Count);
            Assert.AreSame(typeof(LogCallHandler), handlers[0].GetType());
        }

        [TestMethod]
        public void ShouldInheritHandlersFromInterface()
        {
            MethodInfo getNewsMethod = typeof(NewsService).GetMethod("GetNews");
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers = new List<ICallHandler>(policy.GetHandlersFor(getNewsMethod));
            Assert.AreEqual(1, handlers.Count);
            Assert.AreSame(typeof(CachingCallHandler), handlers[0].GetType());
        }

        PolicySet GetPolicySet()
        {
            return new PolicySet();
        }
    }

    [LogCallHandler(Categories = new string[] { "one", "two" }, Priority = 34)]
    class AttributeTestTarget : MarshalByRefObject
    {
        [ValidationCallHandler]
        public string Name
        {
            get { return "foo"; }
            set { }
        }

        [ValidationCallHandler]
        public string DoSomething(string key,
                                  int value)
        {
            return "I did something";
        }

        //[Authorization]
        //[Validation]
        public int GetCriticalInformation(string key)
        {
            return 42;
        }

        [ApplyNoPolicies]
        public void MustBeFast() {}

        public int NothingSpecial()
        {
            return 43;
        }
    }

    class SecondAttributeTestTarget : MarshalByRefObject
    {
        public void DoesntHaveAttribute() {}

        [LogCallHandler]
        public void HasAttribute() {}
    }

    class DerivedAttributeTestTarget : AttributeTestTarget
    {
        public void ANewMethod() {}
    }

    public interface INewsService
    {
        [CachingCallHandler(0, 0, 30)]
        IList GetNews();
    }

    public class NewsService : INewsService
    {
        public IList GetNews()
        {
            return new ArrayList(new string[] { "News1", "News2", "News3" });
        }
    }
}