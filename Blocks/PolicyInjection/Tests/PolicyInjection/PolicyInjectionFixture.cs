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
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Remoting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class PolicyInjectionFixture
    {
        [TestMethod]
        public void CanWrapObjectWithInterceptionAttributes()
        {
            GlobalCountCallHandler.Calls.Clear();

            WrappableWithAttributes wrappable
                = PolicyInjection.Wrap<WrappableWithAttributes>(new WrappableWithAttributes());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, GlobalCountCallHandler.Calls.Count);
            Assert.AreEqual(2, GlobalCountCallHandler.Calls["Method"]);
            Assert.AreEqual(1, GlobalCountCallHandler.Calls["Method3"]);
        }

        [TestMethod]
        public void CanWrapObjectWithInterceptionAttributesWithNonGenericMethods()
        {
            GlobalCountCallHandler.Calls.Clear();

            WrappableWithAttributes wrappable
                = (WrappableWithAttributes)PolicyInjection.Wrap(typeof(WrappableWithAttributes), new WrappableWithAttributes());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, GlobalCountCallHandler.Calls.Count);
            Assert.AreEqual(2, GlobalCountCallHandler.Calls["Method"]);
            Assert.AreEqual(1, GlobalCountCallHandler.Calls["Method3"]);
        }

        [TestMethod]
        public void CanCreateWrappedObjectWithInterceptionAttributes()
        {
            GlobalCountCallHandler.Calls.Clear();

            WrappableWithAttributes wrappable
                = PolicyInjection.Create<WrappableWithAttributes>();

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.IsTrue(wrappable.DefaultCtorCalled);
            Assert.AreEqual(2, GlobalCountCallHandler.Calls.Count);
            Assert.AreEqual(2, GlobalCountCallHandler.Calls["Method"]);
            Assert.AreEqual(1, GlobalCountCallHandler.Calls["Method3"]);
        }

        [TestMethod]
        public void CanCreateWrappedObjectWithInterceptionAttributesWithConstructorParameters()
        {
            GlobalCountCallHandler.Calls.Clear();

            WrappableWithAttributes wrappable
                = PolicyInjection.Create<WrappableWithAttributes>(10, "foo");

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.IsFalse(wrappable.DefaultCtorCalled);
            Assert.AreEqual(2, GlobalCountCallHandler.Calls.Count);
            Assert.AreEqual(2, GlobalCountCallHandler.Calls["Method"]);
            Assert.AreEqual(1, GlobalCountCallHandler.Calls["Method3"]);
        }

        [TestMethod]
        public void StaticFacadeUsesTheCurrentServiceLocator()
        {
            GlobalCountCallHandler.Calls.Clear();

            var interceptionConfigurationSource = new DictionaryConfigurationSource();
            interceptionConfigurationSource.Add(
                PolicyInjectionSettings.SectionName,
                new PolicyInjectionSettings
                {
                    Policies =
                    {
                        new PolicyData("policy")
                        {
                            MatchingRules =
                            {
                                new CustomMatchingRuleData("always", typeof(AlwaysMatchingRule))
                            },
                            Handlers = 
                            {
                                new CustomCallHandlerData("count", typeof(GlobalCountCallHandler))
                                {
                                    Attributes = { {"callhandler", "count"} }
                                }
                            }
                        }
                    }
                });
            var interceptionLocator = EnterpriseLibraryContainer.CreateDefaultContainer(interceptionConfigurationSource);

            var noInterceptionLocator = EnterpriseLibraryContainer.CreateDefaultContainer(new DictionaryConfigurationSource());

            try
            {
                EnterpriseLibraryContainer.Current = interceptionLocator;

                var interceptedWrappable = PolicyInjection.Create<Wrappable>();
                interceptedWrappable.Method();
                Assert.AreEqual(1, GlobalCountCallHandler.Calls.Count);
                GlobalCountCallHandler.Calls.Clear();


                EnterpriseLibraryContainer.Current = noInterceptionLocator;

                var nonInterceptedWrappable = PolicyInjection.Create<Wrappable>();
                nonInterceptedWrappable.Method();
                Assert.AreEqual(0, GlobalCountCallHandler.Calls.Count);
            }
            finally
            {
                EnterpriseLibraryContainer.Current = null;
                GlobalCountCallHandler.Calls.Clear();
                interceptionLocator.Dispose();
                noInterceptionLocator.Dispose();
            }
        }
    }

    [TestClass]
    public class LegacyPolicyInjectionTests
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalCountCallHandler.Calls.Clear();
        }

        [TestCleanup]
        public void TearDown()
        {
            var current = EnterpriseLibraryContainer.Current;
            EnterpriseLibraryContainer.Current = null;
            current.Dispose();
        }

        [TestMethod]
        public void CanCreateWrappedObject()
        {
            SetupContainer("CanCreateWrappedObject");

            Wrappable wrappable = PolicyInjection.Create<Wrappable>();
            Assert.IsNotNull(wrappable);
            Assert.IsTrue(RemotingServices.IsTransparentProxy(wrappable));
        }

        [TestMethod]
        public void CanInterceptWrappedObject()
        {
            SetupContainer("CanCreateWrappedObject");

            Wrappable wrappable = PolicyInjection.Create<Wrappable>();
            wrappable.Method2();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanCreateWrappedObject"]);
        }

        [TestMethod]
        public void CanInterceptWrappedObjectWithNonGenericMethods()
        {
            SetupContainer("CanCreateWrappedObject");

            Wrappable wrappable = (Wrappable)PolicyInjection.Create(typeof(Wrappable));
            wrappable.Method2();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanCreateWrappedObject"]);
        }

        [TestMethod]
        public void CanInterceptCallsToDerivedOfMBRO()
        {
            SetupContainer("CanInterceptCallsToDerivedOfMBRO");

            DerivedWrappable wrappable = PolicyInjection.Create<DerivedWrappable>();
            wrappable.Method2();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanInterceptCallsToDerivedOfMBRO"]);
        }

        [TestMethod]
        public void InterfaceImplementationsOnDerivedClassesAreWrappedMultipleTimes()
        {
            SetupContainer("InterfaceImplementationsOnDerivedClassesAreWrappedMultipleTimes");

            DerivedWrappable wrappable = PolicyInjection.Create<DerivedWrappable>();
            wrappable.Method();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["InterfaceImplementationsOnDerivedClassesAreWrappedMultipleTimes"]);
        }

        [TestMethod]
        public void CanInterceptCallsToMBROOverInterface()
        {
            SetupContainer("CanInterceptCallsToMBROOverInterface");

            Wrappable wrappable = PolicyInjection.Create<Wrappable>();
            ((Interface)wrappable).Method();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanInterceptCallsToMBROOverInterface"]);
        }

        [TestMethod]
        public void CanCreateWrappedObjectOverInterface()
        {
            SetupContainer("CanCreateWrappedObjectOverInterface");

            Interface wrappedOverInterface = PolicyInjection.Create<WrappableThroughInterface, Interface>();
            wrappedOverInterface.Method();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanCreateWrappedObjectOverInterface"]);
        }

        [TestMethod]
        public void CanCreateWrappedObjectOverInterfaceWithNonGenericMethods()
        {
            SetupContainer("CanCreateWrappedObjectOverInterface");

            Interface wrappedOverInterface = (Interface)PolicyInjection.Create(typeof(WrappableThroughInterface), typeof(Interface));
            wrappedOverInterface.Method();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanCreateWrappedObjectOverInterface"]);
        }

        [TestMethod]
        public void CanInterceptCallFromBaseOfWrappedInterface()
        {
            SetupContainer("CanInterceptCallFromBaseOfWrappedInterface");

            Interface wrappedOverInterface = PolicyInjection.Create<WrappableThroughInterface, Interface>();
            wrappedOverInterface.Method3();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanInterceptCallFromBaseOfWrappedInterface"]);
        }

        private void SetupContainer(string globalCallHandlerName)
        {
            EnterpriseLibraryContainer.Current =
                EnterpriseLibraryContainer.CreateDefaultContainer(CreateConfigurationSource(globalCallHandlerName));
        }

        IConfigurationSource CreateConfigurationSource(string globalCallHandlerName)
        {
            PolicyInjectionSettings injectionSettings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData();
            CustomCallHandlerData customCallHandlerData = new CustomCallHandlerData("globalCountHandler", typeof(GlobalCountCallHandler));
            customCallHandlerData.Attributes.Add("callhandler", globalCallHandlerName);
            policyData.Handlers.Add(customCallHandlerData);
            policyData.MatchingRules.Add(new CustomMatchingRuleData("alwaystrue", typeof(AlwaysMatchingRule)));
            injectionSettings.Policies.Add(policyData);
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            configSource.Add(PolicyInjectionSettings.SectionName, injectionSettings);

            return configSource;
        }
    }

    public class WrappableThroughInterface : Interface
    {
        public void Method() { }

        public void Method3() { }
    }

    public interface Interface : InterfaceBase
    {
        void Method();
    }

    public interface InterfaceBase
    {
        void Method3();
    }

    public class DerivedWrappable : Wrappable
    {
        public void Method4() { }
    }

    public class Wrappable : MarshalByRefObject, Interface
    {
        public void Method() { }

        public void Method2() { }

        public void Method3() { }
    }

    public class WrappableWithAttributes : MarshalByRefObject, Interface
    {
        public bool DefaultCtorCalled { get; private set; }

        public WrappableWithAttributes()
        {
            this.DefaultCtorCalled = true;
        }

        public WrappableWithAttributes(int parameter1, string parameter2)
        {
        }

        [GlobalCountCallHandler(Name = "Method")]
        public void Method() { }

        [GlobalCountCallHandler(Name = "Method2")]
        public void Method2() { }

        [GlobalCountCallHandler(Name = "Method3")]
        public void Method3() { }
    }
}
