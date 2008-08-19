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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [TestClass]
    public class ExceptionCallHandlerFixture
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ShouldThrowCorrectException()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddNoopPolicy(factory, new TypeMatchingRule("TargetType"));

            TargetType target = factory.Create<TargetType>();

            target.WillThrowException();
        }

        [TestMethod]
        public void ShouldBeCreatable()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));

            TargetType target = factory.Create<TargetType>();
            target.WillThrowException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldTranslateException()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddExceptionPolicy(factory, "Translate Exceptions", new TypeMatchingRule("TargetType"));
            TargetType target = factory.Create<TargetType>();
            target.WillThrowException();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ShouldRethrowFromNoOpPolicy()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddExceptionPolicy(factory, "No-Op Policy", new TypeMatchingRule("TargetType"));
            TargetType target = factory.Create<TargetType>();
            target.ThrowFromFunctionWithReturnValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowWhenSwallowingExceptionFromNonVoidMethod()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));
            TargetType target = factory.Create<TargetType>();
            target.ThrowFromFunctionWithReturnValue();
            Assert.Fail("An exception should have been thrown");
        }

        [TestMethod]
        public void ShouldBeAbleToSwallowExceptionFromPropertySet()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));
            TargetType target = factory.Create<TargetType>();
            target.MyProperty = 5;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowWhenSwallowingExceptionFromPropertyGet()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));
            TargetType target = factory.Create<TargetType>();

            int foo = target.MyProperty;
        }

        [TestMethod]
        public void CanCreateExceptionHandlerFromConfiguration()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData("policy");
            policyData.Handlers.Add(new ExceptionCallHandlerData("exceptionhandler", "Swallow Exceptions"));
            policyData.MatchingRules.Add(new CustomMatchingRuleData("matchesEverything", typeof(AlwaysMatchingRule)));
            settings.Policies.Add(policyData);

            ExceptionHandlingSettings ehabSettings = new ExceptionHandlingSettings();
            ExceptionPolicyData swallowExceptions = new ExceptionPolicyData("Swallow Exceptions");
            swallowExceptions.ExceptionTypes.Add(new ExceptionTypeData("Exception", typeof(Exception), PostHandlingAction.None));
            ehabSettings.ExceptionPolicies.Add(swallowExceptions);
            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);
            dictConfigurationSource.Add(ExceptionHandlingSettings.SectionName, ehabSettings);

            TargetType target = PolicyInjection.Create<TargetType>(dictConfigurationSource);
            target.WillThrowException();
        }

        [TestMethod]
        public void TestCallHandlerCustomFactory()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData("policy");
            ExceptionCallHandlerData data = new ExceptionCallHandlerData("exceptionhandler", "Swallow Exceptions");
            data.Order = 5;
            policyData.Handlers.Add(data);
            policyData.MatchingRules.Add(new CustomMatchingRuleData("matchesEverything", typeof(AlwaysMatchingRule)));
            settings.Policies.Add(policyData);

            ExceptionHandlingSettings ehabSettings = new ExceptionHandlingSettings();
            ExceptionPolicyData swallowExceptions = new ExceptionPolicyData("Swallow Exceptions");
            swallowExceptions.ExceptionTypes.Add(new ExceptionTypeData("Exception", typeof(Exception), PostHandlingAction.None));
            ehabSettings.ExceptionPolicies.Add(swallowExceptions);

            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            ICallHandler handler = CallHandlerCustomFactory.Instance.Create(null, data, dictConfigurationSource, null);
            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, data.Order);
        }

        void AddNoopPolicy(PolicyInjector factory,
                           params IMatchingRule[] rules)
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy("Noop");
            p.RuleSet.AddRange(rules);
            p.Handlers.Add(new NoopCallHandler());

            factory.Policies.Add(p);
        }

        void AddExceptionPolicy(PolicyInjector factory,
                                string exceptionPolicyName,
                                params IMatchingRule[] rules)
        {
            RuleDrivenPolicy exceptionPolicy = new RuleDrivenPolicy();
            exceptionPolicy.RuleSet.AddRange(rules);
            exceptionPolicy.Handlers.Add(new ExceptionCallHandler(exceptionPolicyName));

            factory.Policies.Add(exceptionPolicy);
        }
    }

    class TargetType : MarshalByRefObject
    {
        public int MyProperty
        {
            get { throw new NotImplementedException("Exception from property getter"); }
            set { throw new NotImplementedException("Exception from property setter"); }
        }

        public int ThrowFromFunctionWithReturnValue()
        {
            throw new NotImplementedException("This is not implemented either");
        }

        public void WillThrowException()
        {
            throw new NotImplementedException("This is not implemented");
        }
    }

    class NoopCallHandler : ICallHandler
    {
        int order;

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
}