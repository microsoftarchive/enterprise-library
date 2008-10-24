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
using System.Runtime.Remoting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        public class WrappableWithAttributes : MarshalByRefObject
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


        [TestMethod]
        public void CanCreateWrappedObject()
        {
            IConfigurationSource configurationSource = CreateConfigurationSource("CanCreateWrappedObject");

            Wrappable wrappable = PolicyInjection.Create<Wrappable>(configurationSource);
            Assert.IsNotNull(wrappable);
            Assert.IsTrue(RemotingServices.IsTransparentProxy(wrappable));
        }

        [TestMethod]
        public void CanInterceptWrappedObject()
        {
            GlobalCountCallHandler.Calls.Clear();

            IConfigurationSource configurationSource = CreateConfigurationSource("CanCreateWrappedObject");

            Wrappable wrappable = PolicyInjection.Create<Wrappable>(configurationSource);
            wrappable.Method2();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanCreateWrappedObject"]);
        }

        [TestMethod]
        public void CanInterceptCallsToDerivedOfMBRO()
        {
            GlobalCountCallHandler.Calls.Clear();

            IConfigurationSource configurationSource = CreateConfigurationSource("CanInterceptCallsToDerivedOfMBRO");

            DerivedWrappable wrappable = PolicyInjection.Create<DerivedWrappable>(configurationSource);
            wrappable.Method2();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanInterceptCallsToDerivedOfMBRO"]);
        }

        [TestMethod]
        public void InterfaceImplementationsOnDerivedClassesAreWrappedMultipleTimes()
        {
            GlobalCountCallHandler.Calls.Clear();

            IConfigurationSource configurationSource = CreateConfigurationSource("InterfaceImplementationsOnDerivedClassesAreWrappedMultipleTimes");

            DerivedWrappable wrappable = PolicyInjection.Create<DerivedWrappable>(configurationSource);
            wrappable.Method();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["InterfaceImplementationsOnDerivedClassesAreWrappedMultipleTimes"]);
        }

        [TestMethod]
        public void CanInterceptCallsToMBROOverInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IConfigurationSource configurationSource = CreateConfigurationSource("CanInterceptCallsToMBROOverInterface");

            Wrappable wrappable = PolicyInjection.Create<Wrappable>(configurationSource);
            ((Interface)wrappable).Method();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanInterceptCallsToMBROOverInterface"]);
        }

        [TestMethod]
        public void CanCreateWrappedObjectOverInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IConfigurationSource configurationSource = CreateConfigurationSource("CanCreateWrappedObjectOverInterface");

            Interface wrappedOverInterface = PolicyInjection.Create<WrappableThroughInterface, Interface>(configurationSource);
            wrappedOverInterface.Method();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanCreateWrappedObjectOverInterface"]);
        }

        [TestMethod]
        public void CanInterceptCallFromBaseOfWrappedInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IConfigurationSource configurationSource = CreateConfigurationSource("CanInterceptCallFromBaseOfWrappedInterface");

            Interface wrappedOverInterface = PolicyInjection.Create<WrappableThroughInterface, Interface>(configurationSource);
            wrappedOverInterface.Method3();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls["CanInterceptCallFromBaseOfWrappedInterface"]);
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
}
