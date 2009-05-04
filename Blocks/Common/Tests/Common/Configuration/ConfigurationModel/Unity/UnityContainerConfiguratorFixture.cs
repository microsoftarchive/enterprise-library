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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel.Unity
{
    [TestClass]
    public class GivenATypeRegistrationWithANonmappedType
    {
        private MockContainer mockContainer;
        private UnityContainerConfigurator containerConfigurator;
        private TypeRegistration TypeRegistration;

        [TestInitialize]
        public void Setup()
        {
            mockContainer = new MockContainer();
            containerConfigurator = new UnityContainerConfigurator(mockContainer);
            TypeRegistration = new TypeRegistration<Foo>(() => new Foo());
        }

        [TestMethod]
        public void WhenContainerIsConfigured_TheSuppliedTypeIsRegistered()
        {
            containerConfigurator.Register(TypeRegistration);

            Assert.AreEqual(typeof(Foo), mockContainer.LastRegisteredFromType);
            Assert.AreEqual(typeof(Foo), mockContainer.LastRegisteredToType);
        }

        [TestMethod]
        public void WhenContainerIsConfigured_TheInstanceIsRegisteredAsSingleton()
        {
            containerConfigurator.Register(TypeRegistration);

            Assert.IsInstanceOfType(mockContainer.LastLifetimeManager, typeof(ContainerControlledLifetimeManager));
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithNonmappedNamed
    {
        MockContainer mockContainer;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration TypeRegistration;

        [TestInitialize]
        public void Setup()
        {
            mockContainer = new MockContainer();
            containerConfigurator = new UnityContainerConfigurator(mockContainer);
            TypeRegistration = new TypeRegistration<Foo>(() => new Foo()) { Name = "bar" };
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenSuppliedTypeIsRegisteredWithTheName()
        {
            containerConfigurator.Register(TypeRegistration);

            Assert.AreEqual("bar", mockContainer.LastRegisteredName);
            Assert.AreEqual(typeof(Foo), mockContainer.LastRegisteredFromType);
            Assert.AreEqual(typeof(Foo), mockContainer.LastRegisteredToType);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithMappedTypes
    {
        UnityContainer container;
        TypeRegistration TypeRegistration;
        private UnityContainerConfigurator containerConfigurator;

        public GivenATypeRegistrationWithMappedTypes()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            TypeRegistration = new TypeRegistration<IFoo>(() => new Foo());
            containerConfigurator = new UnityContainerConfigurator(container);
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenMappedInstanceIsAvailable()
        {
            containerConfigurator.Register(TypeRegistration);

            Assert.IsInstanceOfType(container.Resolve<IFoo>(), typeof(Foo));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_TheInstanceIsRegisteredAsSingleton()
        {
            containerConfigurator.Register(TypeRegistration);

            IFoo aFoo = container.Resolve<IFoo>();
            IFoo anotherfoo = container.Resolve<IFoo>();

            Assert.AreSame(aFoo, anotherfoo);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithANamedMappedRegistration
    {
        UnityContainer container;
        private UnityContainerConfigurator containerConfigurator;
        TypeRegistration TypeRegistration;

        public GivenATypeRegistrationWithANamedMappedRegistration()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            TypeRegistration = new TypeRegistration<IFoo>(() => new Foo()) { Name = "bar" };
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
        }

        [TestMethod]
        public void WhenContainerConfigured_ThenTheRegisteredTypeIsRegisteredWithTheName()
        {
            containerConfigurator.Register(TypeRegistration);

            IFoo item = container.Resolve<IFoo>("bar");
            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Foo));
        }


        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void WhenContainerIsConfigured_CannotResolveServiceTypeWithoutAName()
        {
            containerConfigurator.Register(TypeRegistration);

            container.Resolve<IFoo>();
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithValueConstructorParameters
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration TypeRegistration;

        public GivenATypeRegistrationWithValueConstructorParameters()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            TypeRegistration =
                new TypeRegistration<TypeWithOneArgumentConstructor>(() => new TypeWithOneArgumentConstructor("foo bar"));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenTheContainerInjectsTheParameters()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneArgumentConstructor>();
            Assert.AreEqual("foo bar", resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithMappedConstructorParameterValues
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration TypeRegistration;

        public GivenATypeRegistrationWithMappedConstructorParameterValues()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            TypeRegistration =
                new TypeRegistration<IFoo>(
                    () => new TypeWithOneArgumentConstructor("foo bar"));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenContainerSuppliesValueParameterToConstructedObject()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = (TypeWithOneArgumentConstructor)container.Resolve<IFoo>();
            Assert.AreEqual("foo bar", resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithNullValueConstructorParameters
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration TypeRegistration;

        public GivenATypeRegistrationWithNullValueConstructorParameters()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            TypeRegistration =
                new TypeRegistration<TypeWithOneArgumentConstructor>(() => new TypeWithOneArgumentConstructor(null));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_TheContainerProvidesANullValueForConstructedObject()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneArgumentConstructor>();
            Assert.IsNull(resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithNonConstantExpressionConstructorParameters
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration TypeRegistration;

        static string Property { get { return "foo bar"; } }

        public GivenATypeRegistrationWithNonConstantExpressionConstructorParameters()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            TypeRegistration =
                new TypeRegistration<TypeWithOneArgumentConstructor>(() => new TypeWithOneArgumentConstructor(Property));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_TheContainerProvidesANullValueForConstructedObject()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneArgumentConstructor>();
            Assert.AreEqual(Property, resolvedObject.ConstructorParameter);
        }
    }

    /// <summary>
    /// Summary description for UnityContainerConfiguratorFixture
    /// </summary>
    [TestClass]
    public class GivenATypeRegistrationWithAnUnamedContainerResolvedParameter
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        Foo theFoo;
        TypeRegistration TypeRegistration;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            theFoo = new Foo();
            container.RegisterInstance(theFoo);

            TypeRegistration = new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(() => new TypeWithOneReferenceArgumentConstructor(Container.Resolved<Foo>()));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenResultParameterIsResolved()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }


    }

    /// <summary>
    /// Summary description for UnityContainerConfiguratorFixture
    /// </summary>
    [TestClass]
    public class GivenATypeRegistrationWithAStaticCallConstructorParameter
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        Foo theFoo;
        TypeRegistration TypeRegistration;

        public GivenATypeRegistrationWithAStaticCallConstructorParameter()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            theFoo = new Foo();

            TypeRegistration =
                new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(GetFoo()));
        }

        internal Foo GetFoo()
        {
            return theFoo;
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenResultParameterIsResolved()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithANamedContainerResolvedParameter
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        Foo theFoo;
        Foo theOtherFoo;
        TypeRegistration TypeRegistration;

        public GivenATypeRegistrationWithANamedContainerResolvedParameter()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            theFoo = new Foo();
            container.RegisterInstance("foo bar", theFoo);

            // default instance
            theOtherFoo = new Foo();
            container.RegisterInstance(theOtherFoo);

            TypeRegistration = new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                () => new TypeWithOneReferenceArgumentConstructor((Container.Resolved<Foo>("foo bar"))));
        }

        [TestMethod]
        public void WhenTheContainerIsConfigured_TheNamedInstanceIsInjectedToCreatedObject()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithAContainerResolvedParameterUsingAPropertySuppliedName
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        Foo theFoo;
        Foo theOtherFoo;
        TypeRegistration TypeRegistration;

        public GivenATypeRegistrationWithAContainerResolvedParameterUsingAPropertySuppliedName()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            theFoo = new Foo();
            container.RegisterInstance(FooName, theFoo);

            // default instance
            theOtherFoo = new Foo();
            container.RegisterInstance(theOtherFoo);

            TypeRegistration = new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                () => new TypeWithOneReferenceArgumentConstructor((Container.Resolved<Foo>(FooName))));
        }

        static string FooName { get { return "foo bar"; } }

        [TestMethod]
        public void WhenTheContainerIsConfigured_TheNamedInstanceIsInjectedToCreatedObject()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(theFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenANonGenericRegistryInitializedWithAHandCraftedLambdaExpression
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration TypeRegistration;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);

            Type targetType = typeof(TypeWithOneArgumentConstructor);
            TypeRegistration =
                new TypeRegistration(
                    Expression.Lambda(
                        Expression.New(
                            targetType.GetConstructor(new Type[] { typeof(string) }),
                            new Expression[]
                                {
                                    Expression.Constant("foo bar")
                                })));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_TheContainerProvidesTheValueSuppliedToTheLambdaExpressionForConstructedObject()
        {
            containerConfigurator.Register(TypeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneArgumentConstructor>();
            Assert.AreEqual("foo bar", resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithAnEmptyContainerResolvedEnumerableParameter
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration typeRegistration;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);

            typeRegistration =
                new TypeRegistration<TypeWithOneEnumerableArgumentConstructor>(
                    () => new TypeWithOneEnumerableArgumentConstructor(Container.ResolvedEnumerable<IFoo>(new string[0])));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenTheContainerInjectsAnEmptyEnumerableToCreatedObject()
        {
            containerConfigurator.Register(typeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneEnumerableArgumentConstructor>();
            Assert.AreEqual(0, resolvedObject.ConstructorParameter.Count());
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithASingleNameContainerResolvedEnumerableParameter
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration typeRegistration;
        private IFoo theFoo;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);

            theFoo = new Foo();
            container.RegisterInstance("foo1", theFoo);

            typeRegistration =
                new TypeRegistration<TypeWithOneEnumerableArgumentConstructor>(
                    () => new TypeWithOneEnumerableArgumentConstructor(Container.ResolvedEnumerable<IFoo>(new string[] { "foo1" })));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenTheContainerInjectsAnSingleElementEnumerableToCreatedObject()
        {
            containerConfigurator.Register(typeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneEnumerableArgumentConstructor>();
            CollectionAssert.AreEquivalent(new[] { theFoo }, new List<IFoo>(resolvedObject.ConstructorParameter));
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithMultipleNamesContainerResolvedEnumerableParameter
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration typeRegistration;
        private IFoo theFoo1;
        private IFoo theFoo2;
        private IFoo theFoo3;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);

            theFoo1 = new Foo();
            container.RegisterInstance("foo1", theFoo1);
            theFoo2 = new Foo();
            container.RegisterInstance("foo2", theFoo2);
            theFoo3 = new Foo();
            container.RegisterInstance("foo3", theFoo3);

            typeRegistration =
                new TypeRegistration<TypeWithOneEnumerableArgumentConstructor>(
                    () => new TypeWithOneEnumerableArgumentConstructor(Container.ResolvedEnumerable<IFoo>(new[] { "foo2", "foo1" })));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenTheContainerInjectsTwoElementEnumerableToCreatedObject()
        {
            containerConfigurator.Register(typeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneEnumerableArgumentConstructor>();
            CollectionAssert.AreEqual(new[] { theFoo2, theFoo1, }, new List<IFoo>(resolvedObject.ConstructorParameter));
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithOptionalContainerResolvedParameterForNull
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration typeRegistration;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);

            typeRegistration =
                new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(Container.ResolvedIfNotNull<Foo>(null)));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenTheContainerInjectsNull()
        {
            containerConfigurator.Register(typeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneReferenceArgumentConstructor>();
            Assert.IsNull(resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithOptionalContainerResolvedParameterForEmptyString
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration typeRegistration;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);

            typeRegistration =
                new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(Container.ResolvedIfNotNull<Foo>(string.Empty)));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenTheContainerInjectsNull()
        {
            containerConfigurator.Register(typeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneReferenceArgumentConstructor>();
            Assert.IsNull(resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithOptionalContainerResolvedParameterForNonEmptyString
    {
        UnityContainer container;
        UnityContainerConfigurator containerConfigurator;
        TypeRegistration typeRegistration;
        private Foo namedFoo;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);

            container.RegisterInstance(new Foo());
            namedFoo = new Foo();
            container.RegisterInstance("name", namedFoo);

            typeRegistration =
                new TypeRegistration<TypeWithOneReferenceArgumentConstructor>(
                    () => new TypeWithOneReferenceArgumentConstructor(Container.ResolvedIfNotNull<Foo>("name")));
        }

        [TestMethod]
        public void WhenContainerIsConfigured_ThenTheContainerInjectsNull()
        {
            containerConfigurator.Register(typeRegistration);

            var resolvedObject = container.Resolve<TypeWithOneReferenceArgumentConstructor>();
            Assert.AreSame(namedFoo, resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class GivenConfiguredContainerIncludingATypeRegistrationWithInjectedProperties
    {
        private TypeRegistration<TypeWithProperties> registration;
        private IUnityContainer container;
        private IFoo foo = new Foo();

        [TestInitialize]    
        public void Given()
        {
            container = new UnityContainer();
            container.RegisterInstance<IFoo>(foo);

            UnityContainerConfigurator containerConfigurator = new UnityContainerConfigurator(container);

            registration = new TypeRegistration<TypeWithProperties>(
                () => new TypeWithProperties()
                          {
                              IntValueProperty = 5,
                              FooProperty = Container.Resolved<IFoo>()
                          }
                    );
            containerConfigurator.Register(registration);
        }

        [TestMethod]
        public void WhenTypeIsCreated_ThenConstantPropertyValuesAreInjected()
        {
            var resolvedObject = container.Resolve<TypeWithProperties>();
            Assert.AreEqual(5, resolvedObject.IntValueProperty);
        }

        [TestMethod]
        public void WhenTypeIsCreated_ThenContainerResolvedValuesAreInjected()
        {
            var resolvedObject = container.Resolve<TypeWithProperties>();
            Assert.AreEqual(foo, resolvedObject.FooProperty);
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
        public Foo()
        {
        }
    }

    internal interface IFoo
    {
    }

    internal class MockContainer : UnityContainerBase
    {
        public override IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            LastRegisteredName = name;
            LastRegisteredFromType = from;
            LastRegisteredToType = to;
            LastLifetimeManager = lifetimeManager;
            return this;
        }

        public LifetimeManager LastLifetimeManager
        {
            get;
            set;
        }

        public override IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new System.NotImplementedException();
        }

        public override object Resolve(Type t, string name)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<object> ResolveAll(Type t)
        {
            throw new System.NotImplementedException();
        }

        public override object BuildUp(Type t, object existing, string name)
        {
            throw new System.NotImplementedException();
        }

        public override void Teardown(object o)
        {
            throw new System.NotImplementedException();
        }

        public override IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new System.NotImplementedException();
        }

        public override object Configure(Type configurationInterface)
        {
            throw new System.NotImplementedException();
        }

        public override IUnityContainer RemoveAllExtensions()
        {
            throw new System.NotImplementedException();
        }

        public override IUnityContainer CreateChildContainer()
        {
            throw new System.NotImplementedException();
        }

        public override IUnityContainer Parent
        {
            get { throw new System.NotImplementedException(); }
        }

        public Type LastRegisteredFromType { get; set; }

        public Type LastRegisteredToType { get; set; }

        public string LastRegisteredName { get; set; }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
