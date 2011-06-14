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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
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
        [ExpectedException(typeof(InvalidOperationException))]
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
            IWrappableWithAttributes wrappable =
                this.policyInjector.Wrap<IWrappableWithAttributes>(new WrappableWithAttributes());

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
            IWrappableWithAttributes wrappable =
                (IWrappableWithAttributes)this.policyInjector.Wrap(typeof(IWrappableWithAttributes), new WrappableWithAttributes());

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingWithANullType_ThenArgumentNullExceptionIsThrown()
        {
            this.policyInjector.Create(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingWithANullInterfaceType_ThenArgumentNullExceptionIsThrown()
        {
            this.policyInjector.Create(typeof(object), (Type)null);
        }


        [TestMethod]
        //todo: What should be exception expected?
        //[ExpectedException(typeof(ArgumentException))]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void WhenCreatingAnInstanceWithNonMatchingCreateAndReturnTypes_ThenArgumentExceptionIsThrown()
        {
            // this check is performed by the container
            this.policyInjector.Create(typeof(object), typeof(IFormatProvider));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenWrappingNull_ThenArgumentNullExceptionIsThrown()
        {
            // this check is performed by the container
            this.policyInjector.Wrap<IWrappable>(null);
        }

        [TestMethod]
        //todo: What should be exception expected?
        //[ExpectedException(typeof(ArgumentException))]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void WhenWrappingAnInstanceWithNonMatchingReturnType_ThenArgumentExceptionIsThrown()
        {
            // this check is performed by the container
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
            // TODO: Remove when the Interception extension will be added in the default container
            var configurationSource = new DictionaryConfigurationSource();
            this.policyInjector = new PolicyInjector(configurationSource);
            // EnterpriseLibraryContainer.ConfigureContainer(;
            //EnterpriseLibraryContainer.Current = 
            //    new UnityServiceLocator(
            //        new UnityContainer().AddNewExtension<Interception>());

            //this.policyInjector = new PolicyInjector(EnterpriseLibraryContainer.Current);
        }

        [TestMethod]
        public void WhenInjectorWrapsInstanceOfWrappableTypeWithCallHandlerAttributes_ThenMethodCallsAreIntercepted()
        {
            IWrappableWithAttributes wrappable
                = this.policyInjector.Wrap<IWrappableWithAttributes>(new WrappableWithAttributes());

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
            IWrappable wrappable = this.policyInjector.Wrap<IWrappable>(new Wrappable());

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

            var settings =  new PolicyInjectionSettings
                {
                    Policies =
                    {
                        new PolicyData()
                        {
                            Name = "policy",
                            MatchingRules =
                            {
                                new AlwaysMatchingRuleData("always")
                            },
                            Handlers =
                            {
                                new GlobalCountCallHandlerData("counter") { Callhandler = "counter" }
                            }
                        }
                    }
                };
            
            var configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(PolicyInjectionSettings.SectionName, settings);
            
            //var container = new UnityContainer().AddNewExtension<Interception>();
            //settings.ConfigureContainer(container, configurationSource);

            //EnterpriseLibraryContainer.Current = new UnityServiceLocator(container);

            //this.policyInjector = new PolicyInjector(EnterpriseLibraryContainer.Current);

            this.policyInjector = new PolicyInjector(configurationSource);
        }

        [TestMethod]
        public void WhenInjectorWrapsInstanceOfWrappableType_ThenMethodCallsAreInterceptedAccordingToThePolicyInjectionRulesInTheConfigurationSource()
        {
            IWrappable wrappable = this.policyInjector.Wrap<IWrappable>(new Wrappable());

            wrappable.Method();
            wrappable.Method();
            wrappable.Method3();

            Assert.AreEqual(1, GlobalCountCallHandler.Calls.Count);
            Assert.AreEqual(3, GlobalCountCallHandler.Calls["counter"]);
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

    public interface IWrappable
    {
        void Method();

        void Method2();

        void Method3();
    }

    public class Wrappable : IWrappable, Interface
    {
        public void Method() { }

        public void Method2() { }

        public void Method3() { }
    }

    public interface IWrappableWithAttributes
    {
        bool DefaultCtorCalled { get; }

        void Method();

        void Method2();

        void Method3();
    }

    public class WrappableWithAttributes : IWrappableWithAttributes, Interface
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
