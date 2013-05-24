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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class GivenANullServiceLocator
    {
        private IServiceLocator serviceLocator;

        [TestInitialize]
        public void Setup()
        {
            serviceLocator = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingAPolicyInjectorWithTheServiceLocator_ThenArgumentNullExceptionIsThrown()
        {
            new PolicyInjector(this.serviceLocator);
        }
    }

    [TestClass]
    public class GivenAServiceLocatorWhichCannotResolveAUnityContainer
    {
        private IServiceLocator serviceLocator;

        [TestInitialize]
        public void Setup()
        {
            serviceLocator = new FailingServiceLocator();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatingAPolicyInjectorWithTheServiceLocator_ThenArgumentExceptionIsThrown()
        {
            new PolicyInjector(this.serviceLocator);
        }

        public class FailingServiceLocator : IServiceLocator
        {
            public IEnumerable<TService> GetAllInstances<TService>()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> GetAllInstances(Type serviceType)
            {
                throw new NotImplementedException();
            }

            public TService GetInstance<TService>(string key)
            {
                throw new NotImplementedException();
            }

            public TService GetInstance<TService>()
            {
                throw new ActivationException();
            }

            public object GetInstance(Type serviceType, string key)
            {
                throw new NotImplementedException();
            }

            public object GetInstance(Type serviceType)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }
    }

    [TestClass]
    public class GivenAServiceLocatorWhichResolvesIUnityContainerToAnInstanceWithoutTheInterceptionExtension
    {
        private IServiceLocator serviceLocator;

        [TestInitialize]
        public void Setup()
        {
            var container = new UnityContainer();
            serviceLocator = new UnityServiceLocator(container);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenCreatingAPolicyInjectorWithTheServiceLocator_ThenArgumentExceptionIsThrown()
        {
            new PolicyInjector(this.serviceLocator);
        }
    }

    [TestClass]
    public class GivenAPolicyInjectorCreatedWithAServiceLocatorWhichCanResolveAUnityContainerWithTheInterceptionExtension
    {
        private PolicyInjector policyInjector;
        private CallCounter callCounter;

        [TestInitialize]
        public void Setup()
        {
            this.callCounter = new CallCounter();

            var container = new UnityContainer();
            container.AddNewExtension<Interception>();
            container.AddNewExtension<TransientPolicyBuildUpExtension>();
            container.RegisterInstance<CallCounter>(this.callCounter);
            var serviceLocator = new UnityServiceLocator(container);
            this.policyInjector = new PolicyInjector(serviceLocator);
        }

        [TestMethod]
        public void WhenInjectorWrapsInstanceOfWrappableTypeWithCallHandlerAttributes_ThenMethodCallsAreIntercepted()
        {
            WrappableWithAttributes wrappable =
                this.policyInjector.Wrap<WrappableWithAttributes>(new WrappableWithAttributes());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, this.callCounter.Calls.Count);
            Assert.AreEqual(2, this.callCounter.Calls["Method"]);
            Assert.AreEqual(1, this.callCounter.Calls["Method3"]);
        }

        [TestMethod]
        public void WhenInjectorWrapsInstanceOfWrappableTypeWithCallHandlerAttributesWithNonGenericMethods_ThenMethodCallsAreIntercepted()
        {
            WrappableWithAttributes wrappable =
                (WrappableWithAttributes)this.policyInjector.Wrap(typeof(WrappableWithAttributes), new WrappableWithAttributes());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, this.callCounter.Calls.Count);
            Assert.AreEqual(2, this.callCounter.Calls["Method"]);
            Assert.AreEqual(1, this.callCounter.Calls["Method3"]);
        }

        [TestMethod]
        public void WhenInjectorCreatesInstanceWithNonGenericOverloadWithArguments_ThenTheArgumentsArePassedToTheAppropriateConstructor()
        {
            WrappableWithAttributes wrappable =
                (WrappableWithAttributes)this.policyInjector.Create(typeof(WrappableWithAttributes), 42, "test");

            Assert.IsFalse(wrappable.DefaultCtorCalled);
        }

        [TestMethod]
        public void WhenInjectorCreatesInstanceOfWrappableTypeWithCallHandlerAttributes_ThenMethodCallsAreIntercepted()
        {
            WrappableWithAttributes wrappable = this.policyInjector.Create<WrappableWithAttributes>();

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, this.callCounter.Calls.Count);
            Assert.AreEqual(2, this.callCounter.Calls["Method"]);
            Assert.AreEqual(1, this.callCounter.Calls["Method3"]);
        }

        [TestMethod]
        public void WhenInjectorCreatesInstanceOfWrappableTypeWithInterfaceWithCallHandlerAttributes_ThenMethodCallsAreIntercepted()
        {
            Interface wrappable = this.policyInjector.Create<WrappableWithAttributes, Interface>();

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, this.callCounter.Calls.Count);
            Assert.AreEqual(2, this.callCounter.Calls["Method"]);
            Assert.AreEqual(1, this.callCounter.Calls["Method3"]);
        }

        [TestMethod]
        public void WhenInjectorCreatesInstanceOfWrappableTypeWithCallHandlerAttributes_ThenItUsesTheSuppliedConstructorParameters()
        {
            WrappableWithAttributes wrappable = this.policyInjector.Create<WrappableWithAttributes>();

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, this.callCounter.Calls.Count);
            Assert.AreEqual(2, this.callCounter.Calls["Method"]);
            Assert.AreEqual(1, this.callCounter.Calls["Method3"]);
        }
    }

    [TestClass]
    public class GivenAPolicyInjector
    {
        private PolicyInjector policyInjector;

        [TestInitialize]
        public void Setup()
        {
            this.policyInjector =
                new PolicyInjector(new UnityServiceLocator(
                    new UnityContainer()
                    .AddNewExtension<Interception>()
                    .AddNewExtension<TransientPolicyBuildUpExtension>()));
        }

        [TestMethod]
        public void WhenInjectorCreatesInstanceWithNoArguments_ThenTheDefaultConstructorIsInvoked()
        {
            WrappableWithAttributes wrappable = this.policyInjector.Create<WrappableWithAttributes>();

            Assert.IsTrue(wrappable.DefaultCtorCalled);
        }

        [TestMethod]
        public void WhenInjectorCreatesInstanceWithArguments_ThenTheArgumentsArePassedToTheAppropriateConstructor()
        {
            WrappableWithAttributes wrappable = this.policyInjector.Create<WrappableWithAttributes>(42, "test");

            Assert.IsFalse(wrappable.DefaultCtorCalled);
        }

        [TestMethod]
        public void WhenInjectorIsDisposedASecondTime_TheNoExceptionsAreThrown()
        {
            this.policyInjector.Dispose();
            this.policyInjector.Dispose();
        }

        [TestMethod]
        public void WhenInjectorIsDisposed_ThenWrapCallsThrowObjectDisposedException()
        {
            this.policyInjector.Dispose();

            try
            {
                this.policyInjector.Wrap<Wrappable>(new Wrappable());
                Assert.Fail("should have thrown");
            }
            catch (ObjectDisposedException)
            {
                // expected
            }
        }

        [TestMethod]
        public void WhenInjectorIsDisposed_ThenCreateCallsThrowObjectDisposedException()
        {
            this.policyInjector.Dispose();

            try
            {
                this.policyInjector.Create<Wrappable>();
                Assert.Fail("should have thrown");
            }
            catch (ObjectDisposedException)
            {
                // expected
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingWithANullType_ThenArgumentNullExceptionIsThrown()
        {
            this.policyInjector.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingWithANullInterfaceType_ThenArgumentNullExceptionIsThrown()
        {
            this.policyInjector.Create(typeof(object), (Type)null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenCreatingAnInstanceWithNonMatchingCreateAndReturnTypes_ThenArgumentExceptionIsThrown()
        {
            this.policyInjector.Create(typeof(object), typeof(IFormatProvider));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenWrappingNull_ThenArgumentNullExceptionIsThrown()
        {
            this.policyInjector.Wrap<Wrappable>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenWrappingAnInstanceWithNonMatchingReturnType_ThenArgumentExceptionIsThrown()
        {
            this.policyInjector.Wrap(typeof(IFormatProvider), new object());
        }
    }

    [TestClass]
    public class GivenANullConfigurationSource
    {
        private IConfigurationSource configurationSource;

        [TestInitialize]
        public void Setup()
        {
            configurationSource = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingAPolicyInjectorWithTheConfigurationSource_ThenArgumentExceptionIsThrown()
        {
            new PolicyInjector(this.configurationSource);
        }
    }

    [TestClass]
    public class GivenPolicyInjectorCreatedWithAnEmptyConfigurationSource
    {
        private PolicyInjector policyInjector;

        [TestInitialize]
        public void Setup()
        {
            GlobalCountCallHandler.Calls.Clear();

            var configurationSource = new DictionaryConfigurationSource();
            this.policyInjector = new PolicyInjector(configurationSource);
        }

        [TestMethod]
        public void WhenInjectorWrapsInstanceOfWrappableTypeWithCallHandlerAttributes_ThenMethodCallsAreIntercepted()
        {
            WrappableWithAttributes wrappable
                = this.policyInjector.Wrap<WrappableWithAttributes>(new WrappableWithAttributes());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(2, GlobalCountCallHandler.Calls.Count);
            Assert.AreEqual(2, GlobalCountCallHandler.Calls["Method"]);
            Assert.AreEqual(1, GlobalCountCallHandler.Calls["Method3"]);
        }
    }

    [TestClass]
    public class GivenAPolicyInjectorCreatedWithAServiceLocator
    {
        private MockUnityContainer container;
        private PolicyInjector policyInjector;

        [TestInitialize]
        public void Setup()
        {
            this.container = new MockUnityContainer();
            this.container.AddNewExtension<Interception>();
            this.container.AddNewExtension<TransientPolicyBuildUpExtension>();
            this.policyInjector = new PolicyInjector(new UnityServiceLocator(this.container));
        }

        [TestMethod]
        public void WhenThePolicyInjectorIsDisposed_ThenItDoesNotDisposeTheContainer()
        {
            this.policyInjector.Dispose();

            Assert.IsFalse(this.container.disposed);
        }

        private class MockUnityContainer : UnityContainer
        {
            public bool disposed;

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                this.disposed = true;
            }
        }
    }

    [TestClass]
    public class GivenAPolicyInjectorCreatedWithAContainerWithRuleDrivenPolicies
    {
        private CallCountHandler handler;
        private PolicyInjector policyInjector;

        [TestInitialize]
        public void Setup()
        {
            this.handler = new CallCountHandler();

            var container = new UnityContainer();
            container.AddNewExtension<Interception>();
            container.AddNewExtension<TransientPolicyBuildUpExtension>();
            container.Configure<Interception>()
                .AddPolicy("policy")
                    .AddMatchingRule(new AlwaysMatchingRule())
                    .AddCallHandler(this.handler);

            this.policyInjector = new PolicyInjector(new UnityServiceLocator(container));
        }

        [TestMethod]
        public void WhenInjectorWrapsInstanceOfWrappableType_ThenMethodCallsAreIntercepted()
        {
            Wrappable wrappable = this.policyInjector.Wrap<Wrappable>(new Wrappable());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(3, this.handler.CallCount);
        }
    }

    [TestClass]
    public class GivenAPolicyInjectorCreatedWithAConfigurationSourceWithPolicyInjectionSettings
    {
        private PolicyInjector policyInjector;

        [TestInitialize]
        public void Setup()
        {
            GlobalCountCallHandler.Calls.Clear();

            var configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(
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
                                new CustomCallHandlerData("counter", typeof(GlobalCountCallHandler))
                                {
                                    Attributes =
                                    {
                                        { "callhandler", "counter" }
                                    }
                                }
                            }
                        }
                    }
                });

            this.policyInjector = new PolicyInjector(configurationSource);
        }

        [TestMethod]
        public void WhenInjectorWrapsInstanceOfWrappableType_ThenMethodCallsAreInterceptedAccordingToThePolicyInjectionRulesInTheConfigurationSource()
        {
            Wrappable wrappable = this.policyInjector.Wrap<Wrappable>(new Wrappable());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls.Count);
            Assert.AreEqual(3, GlobalCountCallHandler.Calls["counter"]);
        }
    }
}
