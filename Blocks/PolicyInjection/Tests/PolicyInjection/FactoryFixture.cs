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

using System.Runtime.Remoting;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    /// <summary>
    /// Summary description for FactoryFixture
    /// </summary>
    [TestClass]
    public class FactoryFixture
    {
        CallCountHandler countHandler;

        [TestMethod]
        public void ShouldCreateRawObjectWhenNoPolicyPresent()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            MockDal dal = factory.Create<MockDal>();

            Assert.IsNotNull(dal);
            Assert.IsTrue(dal is MockDal);
            Assert.IsFalse(RemotingServices.IsTransparentProxy(dal));
        }

        [TestMethod]
        public void ShouldRequireInteceptionForTypeMatchingPolicy()
        {
            RemotingPolicyInjector factory = GetFactoryWithPolicies();

            bool shouldInterceptMockDal = factory.TypeRequiresInterception(typeof(MockDal));
            bool shouldInterceptString = factory.TypeRequiresInterception(typeof(string));

            Assert.IsTrue(shouldInterceptMockDal);
            Assert.IsFalse(shouldInterceptString);
        }

        [TestMethod]
        public void ShouldAddInterceptionAccordingToPolicy()
        {
            RemotingPolicyInjector factory = GetFactoryWithPolicies();

            MockDal dal = factory.Create<MockDal>();

            Assert.IsTrue(dal is MockDal);
            Assert.IsTrue(RemotingServices.IsTransparentProxy(dal));
            object realProxy = RemotingServices.GetRealProxy(dal);
            Assert.IsNotNull(realProxy);
            Assert.IsTrue(realProxy is InterceptingRealProxy);
        }

        [TestMethod]
        public void ShouldCallHandlersWhenCallingMethods()
        {
            RemotingPolicyInjector factory = GetFactoryWithPolicies();
            MockDal dal = factory.Create<MockDal>();

            Assert.AreEqual(0, countHandler.CallCount);
            dal.DoSomething("43");
            dal.DoSomething("63");
            dal.DoSomething("Hike!");
            Assert.AreEqual(3, countHandler.CallCount);
        }

        [TestMethod]
        public void HandlerCanShortcutMethodExecution()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            factory.Policies.Add(GetShortcutPolicy());

            MockDal dal = factory.Create<MockDal>();

            Assert.AreEqual(42, dal.DoSomething("should return 42"));
            Assert.AreEqual(-1, dal.DoSomething("shortcut"));
        }

        [TestMethod]
        public void ShouldCallProperHandlersInThePresenceOfOverloadedMethods()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddOverloadsPolicy(factory);

            MockDalWithOverloads dal = factory.Create<MockDalWithOverloads>();

            Assert.AreEqual(42, dal.DoSomething("not intercepted"));
            Assert.IsNull(dal.DoSomething(42));
        }

        [TestMethod]
        public void ShouldCallHandlersOnPropertyGet()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddPropertiesPolicy(factory);

            MockDal dal = factory.Create<MockDal>();
            double startBalance = dal.Balance;
            dal.Balance = 162.3;
            double endBalance = dal.Balance;

            Assert.AreEqual(2, countHandler.CallCount);
        }

        [TestMethod]
        public void ShouldNotInterceptMethodsThatHaveNoPolicyAttribute()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddCallCountingDalPolicy(factory);

            MockDal dal = factory.Create<MockDal>();
            Assert.IsTrue(RemotingServices.IsTransparentProxy(dal));
            Assert.AreEqual(0, countHandler.CallCount);
            dal.SomethingCritical();
            Assert.AreEqual(0, countHandler.CallCount);
        }

        [TestMethod]
        public void ShouldNotInterceptAnyMethodsOnClassThatHasNoPolicyAttribute()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddCallCountingDalPolicy(factory);
            CriticalFakeDal dal = factory.Create<CriticalFakeDal>();

            Assert.IsFalse(RemotingServices.IsTransparentProxy(dal));
        }

        [TestMethod]
        public void ShouldInterceptInterfaceImplementationMethods()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddCallCountingDalPolicy(factory);

            IDal dal = factory.Create<MockDal, IDal>();
            Assert.AreEqual(0, countHandler.CallCount);
            dal.Deposit(200);
            Assert.AreEqual(1, countHandler.CallCount);
        }

        [TestMethod]
        public void ShouldNotBeAbleToCastToUnimplementedInterfaces()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddCallCountingDalPolicy(factory);

            IDal dal = factory.Create<MockDal, IDal>();

            ICallHandler ch = dal as ICallHandler;
            Assert.IsNull(ch);
        }

        [TestMethod]
        public void CanCastToImplementedInterfaces()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddCallCountingDalPolicy(factory);

            MockDal dal = factory.Create<MockDal>();

            IDal iDal = dal as IDal;
            Assert.IsNotNull(iDal);
        }

        [TestMethod]
        public void CanCastAcrossInterfaces()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddCallCountingDalPolicy(factory);

            IDal dal = factory.Create<MockDal, IDal>();

            IMonitor monitor = dal as IMonitor;
            Assert.IsNotNull(monitor);
        }

        [TestMethod]
        public void CannotCastToNonMBROBaseClass()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector(new PolicySet(GetCallCountingPolicy()));

            IDal dal = factory.Create<InterfacesOnlyDal, IDal>();

            Assert.IsNotNull(dal as IMonitor);
            Assert.IsNull(dal as InterfacesOnlyDal);
        }

        [TestMethod]
        public void CanCastToMBROBaseClass()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector(new PolicySet(GetCallCountingPolicy()));

            IDal dal = factory.Create<MockDal, IDal>();

            Assert.IsNotNull(dal as IMonitor);
            Assert.IsNotNull(dal as MockDal);
        }

        [TestMethod]
        public void ShouldCreateAllPipelinesForTargetWhenCreatingViaInterface()
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy("MockDal Policy");
            policy.RuleSet.Add(new TypeMatchingRule(typeof(MockDal)));
            countHandler = new CallCountHandler();
            policy.Handlers.Add(countHandler);

            RemotingPolicyInjector factory = new RemotingPolicyInjector(new PolicySet(policy));
            IDal dal = factory.Create<MockDal, IDal>();

            IMonitor monitor = (IMonitor)dal;

            monitor.Log("one");
            monitor.Log("two");
            monitor.Log("tree");

            MockDal target = (MockDal)dal;
            target.DoSomething("something");

            Assert.AreEqual(4, countHandler.CallCount);
        }

        [TestMethod]
        public void CanRewrapAnInterceptedObject()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector(new PolicySet(GetCallCountingPolicy()));
            IDal dal = factory.Create<MockDal, IDal>();

            object dalTarget = ((InterceptingRealProxy)RemotingServices.GetRealProxy(dal)).Target;

            IMonitor monitor = factory.Wrap<IMonitor>(dal);

            object monitorTarget = ((InterceptingRealProxy)RemotingServices.GetRealProxy(monitor)).Target;
            Assert.AreSame(dalTarget, monitorTarget);
        }

        [TestMethod]
        public void ShouldInterceptMethodsIfTypeImplementsMatchingInterface()
        {
            PolicySet policies = new PolicySet(GetPolicyThatMatchesInterface());
            Assert.IsTrue(policies.AppliesTo(typeof(MockDal)));

            RemotingPolicyInjector factory =
                new RemotingPolicyInjector(new PolicySet(GetPolicyThatMatchesInterface()));

            MockDal mockDal = factory.Create<MockDal>();
            IDal dal = mockDal;

            dal.Deposit(123.45);
            dal.Withdraw(54.32);
            mockDal.DoSomething("foo");

            Assert.AreEqual(2, countHandler.CallCount);
        }

        [TestMethod]
        public void ShouldCallHandlerOnImplementationMethodWhenCalledViaInterface()
        {
            // Set up policy to match underlying type and method name
            RuleDrivenPolicy policy = new RuleDrivenPolicy("MockDal.Deposit policy");
            policy.RuleSet.Add(new TypeMatchingRule(typeof(MockDal)));
            policy.RuleSet.Add(new MemberNameMatchingRule("Deposit"));
            countHandler = new CallCountHandler();
            policy.Handlers.Add(countHandler);

            RemotingPolicyInjector factory = new RemotingPolicyInjector(new PolicySet(policy));

            IDal dal = factory.Create<MockDal, IDal>();
            dal.Deposit(10);
            dal.Deposit(54);
            dal.Deposit(72.5);

            Assert.AreEqual(3, countHandler.CallCount);
        }

        #region Helper Factory methods

        void AddCallCountingDalPolicy(PolicyInjector factory)
        {
            RuleDrivenPolicy typeMatchPolicy = GetCallCountingPolicy();
            factory.Policies.Add(typeMatchPolicy);
        }

        RuleDrivenPolicy GetCallCountingPolicy()
        {
            RuleDrivenPolicy typeMatchPolicy = new RuleDrivenPolicy("DALPolicy");
            NamespaceMatchingRule nsMatchRule = new NamespaceMatchingRule(
                "Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest");
            typeMatchPolicy.RuleSet.Add(nsMatchRule);
            countHandler = new CallCountHandler();
            typeMatchPolicy.Handlers.Add(countHandler);
            return typeMatchPolicy;
        }

        RuleDrivenPolicy GetShortcutPolicy()
        {
            RuleDrivenPolicy typeMatchPolicy = new RuleDrivenPolicy("ShortcutPolicy");
            TypeMatchingAssignmentRule typeRule = new TypeMatchingAssignmentRule(typeof(MockDal));
            typeMatchPolicy.RuleSet.Add(typeRule);
            ShortcuttingHandler handler = new ShortcuttingHandler("shortcut");
            typeMatchPolicy.Handlers.Add(handler);
            return typeMatchPolicy;
        }

        void AddOverloadsPolicy(PolicyInjector factory)
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy("NullStringPolicy");
            TagAttributeMatchingRule tagRule = new TagAttributeMatchingRule("NullString");
            policy.RuleSet.Add(tagRule);

            policy.Handlers.Add(new MakeReturnNullHandler());

            factory.Policies.Add(policy);
        }

        void AddPropertiesPolicy(PolicyInjector factory)
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy("Intercept balance policy");
            MemberNameMatchingRule rule = new MemberNameMatchingRule("get_Balance");
            policy.RuleSet.Add(rule);

            countHandler = new CallCountHandler();
            policy.Handlers.Add(countHandler);
            factory.Policies.Add(policy);
        }

        RuleDrivenPolicy GetPolicyThatMatchesInterface()
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy("Matches IDal");
            TypeMatchingAssignmentRule rule = new TypeMatchingAssignmentRule(typeof(IDal));
            countHandler = new CallCountHandler();
            policy.RuleSet.Add(rule);
            policy.Handlers.Add(countHandler);
            return policy;
        }

        RemotingPolicyInjector GetFactoryWithPolicies()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddCallCountingDalPolicy(factory);
            return factory;
        }

        #endregion
    }
}