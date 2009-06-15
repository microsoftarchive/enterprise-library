//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel.Unity
{
    public abstract class ConfiguredContainerContext : ArrangeActAssert
    {
        protected UnityContainer container;
        internal TestableUnityConfigurator containerConfigurator;

        protected override void Arrange()
        {
            container = new UnityContainer();
            containerConfigurator = new TestableUnityConfigurator(container);
        }

        protected override void Act()
        {
            var mockProvider = new Mock<ITypeRegistrationsProvider>();
            mockProvider.Setup(x => x.GetRegistrations(It.IsAny<IConfigurationSource>())).Returns(GetTypeRegistrations);

            containerConfigurator.RegisterAll(new DictionaryConfigurationSource(), mockProvider.Object);
        }

        private IEnumerable<TypeRegistration> GetTypeRegistrations()
        {
            return new[] { GetTypeRegistration() };
        }

        protected abstract TypeRegistration GetTypeRegistration();

        protected T ResolveDefault<T>()
        {
            return container.Resolve<T>(TypeRegistration.DefaultName);
        }

    }

    [TestClass]
    public class WhenTypeRegistrationsSpecifyNonMappedType : ConfiguredContainerContext
    {

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<FooWithoutDefaultConstructor>(() => new FooWithoutDefaultConstructor("someString"));
        }

        [TestMethod]
        public void ThenTheSuppliedTypeIsRegistered()
        {
            var foo = ResolveDefault<FooWithoutDefaultConstructor>();
            Assert.IsNotNull(foo);
        }

        [TestMethod]
        public void ThenTheInstanceIsRegisteredAsSingleton()
        {
            var fooFirst = ResolveDefault<FooWithoutDefaultConstructor>();
            var fooSecond = ResolveDefault<FooWithoutDefaultConstructor>();
            Assert.AreSame(fooFirst, fooSecond);
        }
    }

    [TestClass]
    public class WhenTypeRegistrationsSepecifyNamedNonMappedType : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<Foo>(() => new Foo()) { Name = "bar" };
        }

        [TestMethod]
        public void ThenSuppliedTypeIsRegisteredWithTheName()
        {
            var foo = container.Resolve<Foo>("bar");
            Assert.IsNotNull(foo);
        }
    }

    [TestClass]
    public class WhenTypeRegistrationsSpecifyMappedTypes : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() => new Foo());
        }

        [TestMethod]
        public void ThenMappedInstanceIsAvailable()
        {
            Assert.IsInstanceOfType(ResolveDefault<IFoo>(), typeof(Foo));
        }

        [TestMethod]
        public void TheInstanceIsRegisteredAsSingleton()
        {
            IFoo aFoo = ResolveDefault<IFoo>();
            IFoo anotherfoo = ResolveDefault<IFoo>();

            Assert.AreSame(aFoo, anotherfoo);
        }
    }

    [TestClass]
    public class WhenTypeRegistrationsSpecifyANamedMappedRegistration : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() => new Foo()) { Name = "bar" };
        }

        [TestMethod]
        public void ThenTheRegisteredTypeIsRegisteredWithTheName()
        {
            IFoo item = container.Resolve<IFoo>("bar");
            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Foo));
        }


        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void CannotResolveServiceTypeWithoutAName()
        {
            ResolveDefault<IFoo>();
        }
    }

    [TestClass]
    public class WhenTypeRegistrationSpecifiesSomethingWithConstructorParameters : ConfiguredContainerContext
    {

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() => new TypeWithOneArgumentConstructor("foo bar"));
        }

        [TestMethod]
        public void ThenTheContainerInjectsTheParameters()
        {
            var resolvedObject = ResolveDefault<TypeWithOneArgumentConstructor>();
            Assert.AreEqual("foo bar", resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenTypeRegistriedSpecifiesAMappedConstructorParameterValues : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() => new TypeWithOneArgumentConstructor("foo bar"));
        }

        [TestMethod]
        public void ThenContainerSuppliesValueParameterToConstructedObject()
        {
            var resolvedObject = (TypeWithOneArgumentConstructor)ResolveDefault<IFoo>();
            Assert.AreEqual("foo bar", resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenTypeRegistrationSpecifiesNullValueConstructorParameters : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() => new TypeWithOneArgumentConstructor(null));
        }

        [TestMethod]
        public void ThenTheContainerProvidesANullValueForConstructedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneArgumentConstructor>();
            Assert.IsNull(resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesANonConstantExpressionConstructorParameters: ConfiguredContainerContext
    {
        static string Property { get { return "foo bar"; } }
        protected override TypeRegistration GetTypeRegistration()
        {
            return
                new TypeRegistration<TypeWithOneArgumentConstructor>(() => new TypeWithOneArgumentConstructor(Property));
        }
        
        [TestMethod]
        public void ThenTheContainerProvidesTheExpressionForConstructedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneArgumentConstructor>();
            Assert.AreEqual(Property, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesAnUnamedContainerResolvedParameter : ConfiguredContainerContext
    {
        private readonly Foo theFoo = new Foo();

        protected override void Arrange()
        {
            base.Arrange();
            container.RegisterInstance<Foo>(theFoo);
        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(() => new TypeWithOneReferenceArgumentConstructor(Container.Resolved<Foo>()));
        }

        [TestMethod]
        public void ThenResultParameterIsResolved()
        {
            var resolvedObject = ResolveDefault<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesAStaticCallConstructorParameter : ConfiguredContainerContext
    {
        Foo theFoo;

        protected override void Arrange()
        {
            base.Arrange();
            theFoo = new Foo();
        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(GetFoo()));
        }

        internal Foo GetFoo()
        {
            return theFoo;
        }

        [TestMethod]
        public void ThenResultParameterIsEvaluated()
        {
            var resolvedObject = ResolveDefault<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesANamedContainerResolvedParameter : ConfiguredContainerContext
    {
        Foo theFoo;
        Foo theOtherFoo;

        protected override void Arrange()
        {
            base.Arrange();
            theFoo = new Foo();
            container.RegisterInstance("foo bar", theFoo);

            // default instance
            theOtherFoo = new Foo();
            container.RegisterInstance(theOtherFoo);
        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                () => new TypeWithOneReferenceArgumentConstructor((Container.Resolved<Foo>("foo bar"))));
        }

        [TestMethod]
        public void ThenTheNamedInstanceIsInjectedToCreatedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesAContainerResolvedParameterUsingAPropertySuppliedName : ConfiguredContainerContext
    {
        Foo theFoo;
        Foo theOtherFoo;
        static string FooName { get { return "name of foo"; } }

        protected override void Arrange()
        {
            base.Arrange();
            theFoo = new Foo();
            container.RegisterInstance(FooName, theFoo);

            // default instance
            theOtherFoo = new Foo();
            container.RegisterInstance(theOtherFoo);
        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                () => new TypeWithOneReferenceArgumentConstructor((Container.Resolved<Foo>(FooName))));
        }

        [TestMethod]
        public void ThenTheNamedInstanceIsInjectedToCreatedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenANonGenericTypeRegistrationIsSpecifiedWithAHandCraftedLambdaExpression : ConfiguredContainerContext
    {
        private const string constantExpression = "constant expression";
        protected override TypeRegistration GetTypeRegistration()
        {
            Type targetType = typeof(TypeWithOneArgumentConstructor);
            return 
                new TypeRegistration(
                    Expression.Lambda(
                        Expression.New(
                            targetType.GetConstructor(new Type[] { typeof(string) }),
                            new Expression[]
                                {
                                    Expression.Constant(constantExpression )
                                })));
        }

        [TestMethod]
        public void ThenTheContainerProvidesTheValueSuppliedToTheLambdaExpressionForConstructedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneArgumentConstructor>();
            Assert.AreEqual(constantExpression, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesAnEmptyContainerResolvedEnumerableParameter : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneEnumerableArgumentConstructor>(
                    () => new TypeWithOneEnumerableArgumentConstructor(Container.ResolvedEnumerable<IFoo>(new string[0])));
        }

        [TestMethod]
        public void ThenTheContainerInjectsAnEmptyEnumerableToCreatedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneEnumerableArgumentConstructor>();
            Assert.AreEqual(0, resolvedObject.ConstructorParameter.Count());
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpeciesASingleNameContainerResolvedEnumerableParameter : ConfiguredContainerContext
    {
        private IFoo theFoo;

        protected override void Arrange()
        {
            base.Arrange();
            theFoo = new Foo();
            container.RegisterInstance("foo1", theFoo);

        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneEnumerableArgumentConstructor>(
                    () => new TypeWithOneEnumerableArgumentConstructor(Container.ResolvedEnumerable<IFoo>(new string[] { "foo1" })));
        }

        [TestMethod]
        public void ThenTheContainerInjectsAnSingleElementEnumerableToCreatedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneEnumerableArgumentConstructor>();
            CollectionAssert.AreEquivalent(new[] { theFoo }, new List<IFoo>(resolvedObject.ConstructorParameter));
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesMultipleNamesContainerResolvedEnumerableParameter : ConfiguredContainerContext
    {

        private IFoo theFoo1;
        private IFoo theFoo2;
        private IFoo theFoo3;

        protected override void Arrange()
        {
            base.Arrange();
            theFoo1 = new Foo();
            container.RegisterInstance("foo1", theFoo1);
            theFoo2 = new Foo();
            container.RegisterInstance("foo2", theFoo2);
            theFoo3 = new Foo();
            container.RegisterInstance("foo3", theFoo3);
        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return 
                new TypeRegistration<TypeWithOneEnumerableArgumentConstructor>(
                    () => new TypeWithOneEnumerableArgumentConstructor(Container.ResolvedEnumerable<IFoo>(new[] { "foo2", "foo1" })));
        }

        [TestMethod]
        public void ThenTheContainerInjectsTwoElementEnumerableToCreatedObject()
        {
            var resolvedObject = ResolveDefault<TypeWithOneEnumerableArgumentConstructor>();
            CollectionAssert.AreEqual(new[] { theFoo2, theFoo1, }, new List<IFoo>(resolvedObject.ConstructorParameter));
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesOptionalContainerResolvedParameterForNull : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return
                new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(Container.ResolvedIfNotNull<Foo>(null)));
        }

        [TestMethod]
        public void ThenTheContainerInjectsNull()
        {
            var resolvedObject = ResolveDefault<TypeWithOneReferenceArgumentConstructor>();
            Assert.IsNull(resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesOptionalContainerResolvedParameterForEmptyString : ConfiguredContainerContext
    {

        protected override TypeRegistration GetTypeRegistration()
        {
            return 
                new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(Container.ResolvedIfNotNull<Foo>(string.Empty)));
        }

        [TestMethod]
        public void ThenTheContainerInjectsNull()
        {
            var resolvedObject = ResolveDefault<TypeWithOneReferenceArgumentConstructor>();
            Assert.IsNull(resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesOptionalContainerResolvedParameterForNonEmptyString : ConfiguredContainerContext
    {
        private Foo namedFoo;

        protected override void Arrange()
        {
            base.Arrange();
            container.RegisterInstance(new Foo());
            namedFoo = new Foo();
            container.RegisterInstance("name", namedFoo);
        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return 
                new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(Container.ResolvedIfNotNull<Foo>("name")));
        }

        [TestMethod]
        public void ThenTheContainerInjectsNull()
        {
            var resolvedObject = ResolveDefault<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(namedFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesInjectedProperties : ConfiguredContainerContext
    {
        private IFoo foo = new Foo();

        protected override void Arrange()
        {
            base.Arrange();
            container.RegisterInstance<IFoo>(foo);
        }

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithProperties>(
                () => new TypeWithProperties()
                          {
                              IntValueProperty = 5,
                              FooProperty = Container.Resolved<IFoo>()
                          }
                    );
        }

        [TestMethod]
        public void ThenConstantPropertyValuesAreInjected()
        {
            var resolvedObject = ResolveDefault<TypeWithProperties>();
            Assert.AreEqual(5, resolvedObject.IntValueProperty);
        }

        [TestMethod]
        public void ThenContainerResolvedValuesAreInjected()
        {
            var resolvedObject = ResolveDefault<TypeWithProperties>();
            Assert.AreEqual(foo, resolvedObject.FooProperty);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesANonDefaultWithNoTypeMapping : ConfiguredContainerContext
    {

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<Tofu>(() => new Tofu()) { Name = "foo" };
        }

        [TestMethod]
        public void ThenResolvingDefaultReturnsDifferentInstance()
        {
            var foo = container.Resolve<Tofu>("foo");
            var defaultFoo = ResolveDefault<Tofu>();

            Assert.AreNotSame(foo, defaultFoo);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesADefaultWithNoTypeMapping : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<Foo>(() => new Foo()) { Name = "foo", IsDefault = true };
        }

        [TestMethod]
        public void ThenResolvingDefaultImplementationReturnsSameInstance()
        {
            var foo = container.Resolve<Foo>("foo");
            var defaultFoo = container.Resolve<Foo>();

            Assert.AreSame(foo, defaultFoo);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesANonDefaultWithTypeMapping : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() => new Foo()) { Name = "foo" };
        }

        [TestMethod]
        public void ThenResolvingDefaultForServiceInterfaceTypeThrows()
        {
            try
            {
                ResolveDefault<IFoo>();
                Assert.Fail("Should have failed");
            }
            catch (ResolutionFailedException)
            {
                // expected exception
            }
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesADefaultWithTypeMapping : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() => new Foo()) { Name = "foo", IsDefault = true };
        }

        [TestMethod]
        public void ThenResolvingDefaultServiceInterfaceTypeReturnsSameInstance()
        {
            var foo = container.Resolve<Foo>("foo");
            var defaultIFoo = container.Resolve<IFoo>();

            Assert.AreSame(foo, defaultIFoo);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesADefaultWithTypeMappingAndTransientLifetime : ConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() => new Foo()) { Name = "foo", IsDefault = true, Lifetime = TypeRegistrationLifetime.Transient };
        }

        [TestMethod]
        public void ThenResolvingServiceInterfaceTypeReturnsDifferentInstances()
        {
            var foo1 = container.Resolve<IFoo>("foo");
            var foo2 = container.Resolve<IFoo>("foo");

            Assert.AreNotSame(foo1, foo2);
        }
    }

    [TestClass]
    public class WhenTheConfiguratorInitializesAUnityContainer : ArrangeActAssert
    {
        private IUnityContainer container;

        protected override void Arrange()
        {
            this.container = new UnityContainer();
        }

        protected override void Act()
        {
            var configurator = new UnityContainerConfigurator(this.container);
        }


        [TestMethod]
        public void ThenTheContainerGetsTheInterceptionExtension()
        {
            

            Assert.IsNotNull(this.container.Configure<Interception>());
        }

        [TestMethod]
        public void ThenTheContainerGetsTheTransientPolicyBuildUpExtension()
        {
            Assert.IsNotNull(this.container.Configure<TransientPolicyBuildUpExtension>());
        }

        [TestMethod]
        public void ThenTheContainerGetsTheReaderWriterLockExtension()
        {
            Assert.IsNotNull(this.container.Configure<ReaderWriterLockExtension>());
        }

    }


    public abstract class ReconfigurableConfiguredContainerContext : ConfiguredContainerContext
    {
        protected abstract TypeRegistration GetUpdatedRegistration();

        protected override void Act()
        {
            base.Act();

            ActBeforeReconfigured();

            containerConfigurator.Reconfigure(
                new TypeRegistration[]{
                    GetUpdatedRegistration()
                });

        }

        protected virtual void ActBeforeReconfigured()
        {
        }
    }

    [TestClass]
    public class WhenATransientTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() =>
                    new TypeWithOneArgumentConstructor("original value"))
                {
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(
                        () => new TypeWithOneArgumentConstructor("modified value"))
                    {
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }

        [TestMethod]
        public void ThenTheContainerResolvesTheUpdatedRegistrations()
        {
            var instance = ResolveDefault<TypeWithOneArgumentConstructor>();
            Assert.AreEqual("modified value", instance.ConstructorParameter);
        }

        [TestMethod]
        public void ThenTheUpdatedRegistrationStillHasTransientLifetime()
        {
            var instance = ResolveDefault<TypeWithOneArgumentConstructor>();
            var secondInstance = ResolveDefault<TypeWithOneArgumentConstructor>();

            Assert.AreNotSame(instance, secondInstance);
        }
    }

    [TestClass]
    public class WhenASingletonTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() =>
                new TypeWithOneArgumentConstructor("original value"))
                {
                    Lifetime = TypeRegistrationLifetime.Singleton
                };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(
                        () => new TypeWithOneArgumentConstructor("modified value")
                    )
                    {
                        Lifetime = TypeRegistrationLifetime.Singleton
                    };
        }

        [TestMethod]
        public void ThenTheContainerResolvesTheUpdatedRegistrations()
        {
            var instance = ResolveDefault<TypeWithOneArgumentConstructor>();
            Assert.AreEqual("modified value", instance.ConstructorParameter);
        }

        [TestMethod]
        public void ThenTheUpdatedRegistrationStillHasSingletonLifetime()
        {
            var instance = ResolveDefault<TypeWithOneArgumentConstructor>();
            var secondInstance = ResolveDefault<TypeWithOneArgumentConstructor>();

            Assert.AreSame(instance, secondInstance);
        }
    }

    [TestClass]
    public class WhenASingletonTypeRegistrationChangesAfterResolving : ReconfigurableConfiguredContainerContext
    {
        private TypeWithOneArgumentConstructor originalInstance;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() =>
                new TypeWithOneArgumentConstructor("original value"))
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(
                        () => new TypeWithOneArgumentConstructor("modified value")
                    )
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        protected override void ActBeforeReconfigured()
        {
            originalInstance = ResolveDefault<TypeWithOneArgumentConstructor>();
        }

        [TestMethod]
        public void ThenTheContainerResolvestheOriginalInstance()
        {
            var instance = ResolveDefault<TypeWithOneArgumentConstructor>();
            Assert.AreSame(originalInstance, instance);
        }
    }

    [TestClass]
    public class WhenAMappedSingletonTypeRegistrationChangesAfterResolving : ReconfigurableConfiguredContainerContext
    {
        private IFoo originalInstance;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                                              new TypeWithOneArgumentConstructor("original value"))
                       {
                           Lifetime = TypeRegistrationLifetime.Singleton,
                           IsDefault = true
                        };

        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(
                      () => new TypeWithOneArgumentConstructor("modified value"))
            {
                Lifetime = TypeRegistrationLifetime.Singleton,
                IsDefault = true
            };
        }

        protected override void ActBeforeReconfigured()
        {
            originalInstance = ResolveDefault<TypeWithOneArgumentConstructor>();
        }

        [TestMethod]
        public void ThenTheContainerResolvestheOriginalInstance()
        {
            var instance = ResolveDefault<IFoo>();
            Assert.AreSame(originalInstance, instance);
        }

    }

    [TestClass]
    public class WhenASingletonConcreteTypeOnMappedTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Tofu())
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        [TestMethod]
        public void ThenTheLifetimeShoudlStillBeSingleton()
        {
            var instance = ResolveDefault<IFoo>();
            var secondInstance = ResolveDefault<IFoo>();
            Assert.AreSame(instance, secondInstance);
        }
    }


    [TestClass]
    public class WhenADefaultTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            Assert.IsNotNull(container.Resolve<IFoo>());
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Name = "test instance",
                IsDefault = false,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void ThenResolveDefaultShouldThrow()
        {
            container.Resolve<IFoo>();
        }
    }

    [TestClass]
    public class WhenImplementationTypeForTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        private IFoo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    Name = "Foo"
                })
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = container.Resolve<IFoo>();
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Tofu())
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenResolveReturnsInstanceOfNewImplementationType()
        {
            IFoo instanceOfTfoe = container.Resolve<IFoo>();

            Assert.IsInstanceOfType(instanceOfTfoe, typeof(Tofu));
        }
    }


    [TestClass]
    public class WhenPropertySetsOnTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        private IFoo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    Name = "Foo"
                })
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = container.Resolve<IFoo>();
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenPreviouslyRegisteredPropertiesAreNotSet()
        {
            Foo reconfiguredInstance = container.Resolve<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.IsNull(reconfiguredInstance.Name);
            
        }
    }

    [TestClass]
    public class WhenConstructorOnTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        private Foo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo("string ctor"))
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = ResolveDefault<IFoo>() as Foo;
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo(42))
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenNewlyConfiguredConstrutorIsSet()
        {
            Assert.IsNotNull(instancePriorToReconfigure);
            Assert.AreEqual("string ctor", instancePriorToReconfigure.Name);

            Foo reconfiguredInstance = ResolveDefault<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.AreEqual(42.ToString(), reconfiguredInstance.Name);

        }
    }

    [TestClass]
    public class WhenConstructorOnTypeRegistrationChangesToParameterless : ReconfigurableConfiguredContainerContext
    {
        private Foo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo("string ctor"))
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = ResolveDefault<IFoo>() as Foo;
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenNewlyConfiguredConstrutorIsSet()
        {
            Assert.IsNotNull(instancePriorToReconfigure);
            Assert.AreEqual("string ctor", instancePriorToReconfigure.Name);

            Foo reconfiguredInstance = ResolveDefault<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.IsNull(reconfiguredInstance.Name);
        }
    }

    [TestClass]
    public class WhenNumberOfPropretiesOnTypeRegistrationChangesToFewer : ReconfigurableConfiguredContainerContext
    {
        private Foo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    Name = "Name",
                    LastName = "LastName",
                    Initials = "Initials"
                })
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = ResolveDefault<IFoo>() as Foo;
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    LastName = "different"
                })
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenNewlyConfiguredConstrutorIsSet()
        {
            Assert.IsNotNull(instancePriorToReconfigure);
            Assert.AreEqual("Name", instancePriorToReconfigure.Name);
            Assert.AreEqual("LastName", instancePriorToReconfigure.LastName);
            Assert.AreEqual("Initials", instancePriorToReconfigure.Initials);

            Foo reconfiguredInstance = ResolveDefault<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.IsNull(reconfiguredInstance.Name);
            Assert.AreEqual("different", reconfiguredInstance.LastName);
            Assert.IsNull(reconfiguredInstance.Initials);
        }
    }


    internal class TestableUnityConfigurator : UnityContainerConfigurator
    {
        public TestableUnityConfigurator(IUnityContainer container)
            :base(container)
        { }

        public void Reconfigure(IEnumerable<TypeRegistration> updatedRegistrations)
        {
            base.RegisterUpdates(updatedRegistrations);
        }
    }

    internal class TypeWithProperties
    {
        public TypeWithProperties()
        {
        }

        public int IntValueProperty { get; set; }
        public IFoo FooProperty { get; set; }
    }

    internal class TypeWithOneArgumentConstructor : IFoo
    {
        public TypeWithOneArgumentConstructor(string parameter)
        {
            this.ConstructorParameter = parameter;
        }
        public string ConstructorParameter;
    }

    internal class TypeWithOneReferenceArgumentConstructor
    {
        public TypeWithOneReferenceArgumentConstructor(Foo parameter)
        {
            ConstructorParameter = parameter;
        }

        public Foo ConstructorParameter;
    }

    internal class TypeWithOneEnumerableArgumentConstructor
    {
        public TypeWithOneEnumerableArgumentConstructor(IEnumerable<IFoo> foos)
        {
            ConstructorParameter = foos;
        }

        public IEnumerable<IFoo> ConstructorParameter;
    }

    internal class Foo : IFoo
    {
        public Foo() { }

        public Foo(int i)
        {
            Name = i.ToString();
        }

        public Foo(string c)
        {
            Name = c;
        }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
    }

    internal class Tofu : IFoo
    {

    }

    internal interface IFoo
    {
    }

    internal class FooWithoutDefaultConstructor
    {
        public FooWithoutDefaultConstructor(string constructorParam)
        {
        }   
    }

   
}
