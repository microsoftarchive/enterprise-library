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
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        protected T ResolveUnnamed<T>()
        {
            return container.Resolve<T>(TypeRegistration.DefaultName<T>());
        }

        protected T ResolveDefault<T>()
        {
            return container.Resolve<T>();
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
            var foo = ResolveUnnamed<FooWithoutDefaultConstructor>();
            Assert.IsNotNull(foo);
        }

        [TestMethod]
        public void ThenTheInstanceIsRegisteredAsSingleton()
        {
            var fooFirst = ResolveUnnamed<FooWithoutDefaultConstructor>();
            var fooSecond = ResolveUnnamed<FooWithoutDefaultConstructor>();
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
            Assert.IsInstanceOfType(ResolveUnnamed<IFoo>(), typeof(Foo));
        }

        [TestMethod]
        public void TheInstanceIsRegisteredAsSingleton()
        {
            IFoo aFoo = ResolveUnnamed<IFoo>();
            IFoo anotherfoo = ResolveUnnamed<IFoo>();

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
            ResolveUnnamed<IFoo>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneArgumentConstructor>();
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
            var resolvedObject = (TypeWithOneArgumentConstructor)ResolveUnnamed<IFoo>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneArgumentConstructor>();
            Assert.IsNull(resolvedObject.ConstructorParameter);
        }
    }

    [TestClass]
    public class WhenATypeRegistrationSpecifiesANonConstantExpressionConstructorParameters : ConfiguredContainerContext
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
            var resolvedObject = ResolveUnnamed<TypeWithOneArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneReferenceArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneReferenceArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneReferenceArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneReferenceArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneEnumerableArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneEnumerableArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneEnumerableArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneReferenceArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneReferenceArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithOneReferenceArgumentConstructor>();
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
            var resolvedObject = ResolveUnnamed<TypeWithProperties>();
            Assert.AreEqual(5, resolvedObject.IntValueProperty);
        }

        [TestMethod]
        public void ThenContainerResolvedValuesAreInjected()
        {
            var resolvedObject = ResolveUnnamed<TypeWithProperties>();
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
            var defaultFoo = ResolveUnnamed<Tofu>();

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
                ResolveUnnamed<IFoo>();
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
    public partial class WhenTheConfiguratorInitializesAUnityContainer : ArrangeActAssert
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
        public void ThenTheContainerGetsTheTransientPolicyBuildUpExtension()
        {
            Assert.IsNotNull(this.container.Configure<TransientPolicyBuildUpExtension>());
        }
    }

    [TestClass]
    public class WhenTheConfiguratorInitializesAUnityContainerAndRegistersAll : ArrangeActAssert
    {
        private IUnityContainer container;
        private Mock<IConfigurationSource> configurationSource = new Mock<IConfigurationSource>();

        protected override void Arrange()
        {
            this.container = new UnityContainer();
        }

        protected override void Act()
        {
            var configurator = new UnityContainerConfigurator(this.container);
            configurator.RegisterAll(this.configurationSource.Object, new Mock<ITypeRegistrationsProvider>().Object);
        }

        [TestMethod]
        public void ThenTheContainerCanResolveTheConfigurationSource()
        {
            Assert.AreEqual(configurationSource.Object, container.Resolve<IConfigurationSource>());
        }

        [TestMethod]
        public void ThenDisposingTheContainerDoesNotDisposeTheConfigurationSource()
        {
            container.Dispose();

            configurationSource.Verify(x => x.Dispose(), Times.Never());
        }
    }

    internal partial class TestableUnityConfigurator : UnityContainerConfigurator
    {
        public TestableUnityConfigurator(IUnityContainer container)
            : base(container)
        { }
    }

    public class TypeWithProperties
    {
        public TypeWithProperties()
        {
        }

        public int IntValueProperty { get; set; }
        public IFoo FooProperty { get; set; }
    }

    public class TypeWithOneArgumentConstructor : IFoo
    {
        public TypeWithOneArgumentConstructor(string parameter)
        {
            this.ConstructorParameter = parameter;
        }
        public string ConstructorParameter;
    }

    public class TypeWithOneReferenceArgumentConstructor
    {
        public TypeWithOneReferenceArgumentConstructor(Foo parameter)
        {
            ConstructorParameter = parameter;
        }

        public Foo ConstructorParameter;
    }

    public class TypeWithOneEnumerableArgumentConstructor
    {
        public TypeWithOneEnumerableArgumentConstructor(IEnumerable<IFoo> foos)
        {
            ConstructorParameter = foos;
        }

        public IEnumerable<IFoo> ConstructorParameter;
    }

    public class Foo : IFoo
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

    public class Tofu : IFoo
    {

    }

    public interface IFoo
    {
    }

    public class FooWithoutDefaultConstructor
    {
        public FooWithoutDefaultConstructor(string constructorParam)
        {
        }
    }


}
