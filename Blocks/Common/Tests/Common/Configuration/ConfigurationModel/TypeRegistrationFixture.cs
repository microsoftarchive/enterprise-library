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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel
{
    /// <summary>
    /// Summary description for TypeRegistrationFixture
    /// </summary>
    [TestClass]
    public class GivenAnInvalidLambdaExpression
    {
        LambdaExpression expression;

        public GivenAnInvalidLambdaExpression()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            expression = Expression.Lambda(Expression.Constant(32));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenRegistryTypeInstantiated_ThenThrowsArgumentException()
        {
            TypeRegistration registration = new TypeRegistration(expression);
        }
    }

    [TestClass]
    public class GivenAConstructionLambdaExpression
    {
        private Expression<Func<Foo>> expression;
        TypeRegistration registration;


        public GivenAConstructionLambdaExpression()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo();
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void WhenNoNameIsGiven_DefaultNonNullNameIsReturned()
        {
            Assert.AreEqual(TypeRegistration.DefaultName, registration.Name);
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_ThenWillProvideAccessToSameUnderlyingExpression()
        {
            Assert.AreSame(expression, registration.LambdaExpression);
        }

        [TestMethod]
        public void WhenRegistryTypeInstatiated_ThenWillReturnMatchingImplementationType()
        {
            Assert.AreEqual(typeof(Foo), registration.ImplementationType);
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_ThenConstructorListIsEmpty()
        {
            Assert.AreEqual(0, registration.ConstructorParameters.Count());
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_ThePropertiesListIsEmpty()
        {
            Assert.AreEqual(0, registration.InjectedProperties.Count());
        }
    }

    [TestClass]
    public class GivenAConstructionLambdaExpressionWithConstructorParameters
    {
        private Expression<Func<Foo>> expression;
        TypeRegistration registration;

        public GivenAConstructionLambdaExpressionWithConstructorParameters()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo("aString",
                Container.Resolved<IBar>(),
                Container.Resolved<IBar>("anotherBar"));
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_ThenWillProvideAccessToSameUnderlyingExpression()
        {
            Assert.AreSame(expression, registration.LambdaExpression);
        }

        [TestMethod]
        public void WhenRegistryTypeInstatiated_ThenWillReturnMatchingImplementationType()
        {
            Assert.AreEqual(typeof(Foo), registration.ImplementationType);
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_ConstantConstructorResultsInValueParameterDependency()
        {
            ConstantParameterValue param = (ConstantParameterValue)registration.ConstructorParameters.ElementAt(0);
            Assert.AreEqual("aString", param.Value);
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_ReferenceConstructorParamResultsInContainerResolvedParameter()
        {
            ContainerResolvedParameter param =
                (ContainerResolvedParameter)registration.ConstructorParameters.ElementAt(1);
            Assert.AreEqual(typeof(IBar), param.Type);
            Assert.AreEqual(null, param.Name);
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_ReferenceConstructorParamResultsInNamedContainerResolvedParameter()
        {
            ContainerResolvedParameter param =
                (ContainerResolvedParameter)registration.ConstructorParameters.ElementAt(2);
            Assert.AreEqual(typeof(IBar), param.Type);
            Assert.AreEqual("anotherBar", param.Name);
        }

    }

    [TestClass]
    public class GivenExpressionWithResolvedEnumerable
    {

        private Expression<Func<Foo>> expression;
        TypeRegistration registration;
        List<string> keyNames = new List<string>() { "itemOne", "itemTwo" };

        public GivenExpressionWithResolvedEnumerable()
        {

        }

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo(Container.ResolvedEnumerable<IBar>(keyNames));
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void WhenRegistryTypeInstantiated_EnumerableConstructorParam()
        {
            var param = (ContainerResolvedEnumerableParameter)registration.ConstructorParameters.ElementAt(0);

            CollectionAssert.AreEquivalent(keyNames, new List<string>(param.Names));
        }


        [TestMethod]
        public void WhenRegistryTypeInstatiated_ThenProvidesAccessToEnumerableType()
        {
            var param = (ContainerResolvedEnumerableParameter)registration.ConstructorParameters.ElementAt(0);
            Assert.AreEqual(typeof(IBar), param.ElementType);
        }
    }

    [TestClass]
    public class GivenAnExpressionWithNullKeyNames
    {

        private Expression<Func<Foo>> expression;
        TypeRegistration registration;
        private List<string> keyNames = null;

        public GivenAnExpressionWithNullKeyNames()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo(Container.ResolvedEnumerable<IBar>(keyNames));
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenRegistryTypeInstantiated_RetrievingPramNamesThrows()
        {
            var param = (ContainerResolvedEnumerableParameter)registration.ConstructorParameters.ElementAt(0);
            IEnumerable<string> names = param.Names;
        }

    }

    [TestClass]
    public class GivenALambdaExpressionForImplementationType
    {
        private Expression<Func<IFoo>> expression;

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo();

        }

        [TestMethod]
        public void WhenTypeRegistrationCreatedWithServiceAndImplmentationGenericParameters_ThenTheServiceTypePropertyIsSet()
        {
            TypeRegistration<IFoo> registration = new TypeRegistration<IFoo>(expression);
            Assert.AreEqual(typeof(IFoo), registration.ServiceType);
        }

        [TestMethod]
        public void WhenTypeRegistrationCreatedWithImplementationForGenericParameter_ThenServieTypeIsNull()
        {
            TypeRegistration<Foo> registration = new TypeRegistration<Foo>(() => new Foo());
            Assert.AreEqual(typeof(Foo), registration.ServiceType);
        }
    }

    [TestClass]
    public class GivenALambaExpressionForImplementationWithServiceType
    {
        private Expression<Func<Foo>> expression;

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenCreatingATypeRegistrationWithMismatchedService_ThenConstructorThrowsArgumentException()
        {
            new TypeRegistration(expression, typeof(IEnumerable));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingATypeRegistrationWithANullService_ThenThrowsArgumentNull()
        {
            var registration = new TypeRegistration(expression, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingATypeRegistrationWithANullExpression_ThenConstructorThrowsArgumentNullException()
        {
            new TypeRegistration(null);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationForLambdaExpressionWithInitializerOnly
    {
        private Expression<Func<TypeWithProperties>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            expression = () => new TypeWithProperties { Property = "value" };
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void ThenWillProvideAccessToSameUnderlyingExpression()
        {
            Assert.AreSame(expression, registration.LambdaExpression);
        }

        [TestMethod]
        public void ThenWillReturnMatchingImplementationType()
        {
            Assert.AreEqual(typeof(TypeWithProperties), registration.ImplementationType);
        }

        [TestMethod]
        public void ThenConstructorListIsEmpty()
        {
            Assert.AreEqual(0, registration.ConstructorParameters.Count());
        }

        [TestMethod]
        public void ThenPropertiesListHasOneElement()
        {
            Assert.AreEqual(1, registration.InjectedProperties.Count());
        }

        [TestMethod]
        public void ThenSingleInjectedPropertyForPropertyNameProperty()
        {
            Assert.AreEqual("Property", registration.InjectedProperties.ElementAt(0).PropertyName);
        }

        [TestMethod]
        public void ThenSingleInjectedPropertyForConstantValue()
        {
            ConstantParameterValue param =
                (ConstantParameterValue)registration.InjectedProperties.ElementAt(0).PropertyValue;
            Assert.AreEqual("value", param.Value);
        }
    }

    [TestClass]
    public class GivenATypeRegistrationWithMultipleInitialzerProperties
    {
        private Expression<Func<TypeWithProperties>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            expression = () => new TypeWithProperties
                {
                    Property = "value",
                    AnotherProperty = 42
                };
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void ThenAPropertyParameterIsAvailableForEachInitializedProperty()
        {
            Assert.AreEqual(2, registration.InjectedProperties.Count());
        }

        [TestMethod]
        public void ThenPropertyValueNamesAreProvided()
        {
            string[] names = { "AnotherProperty", "Property" };

            CollectionAssert.AreEquivalent(
                names,
                new List<string>(registration.InjectedProperties.Select(p => p.PropertyName))
                );
        }

        [TestMethod]
        public void ThenPropertyValuesMatchSuppliedValues()
        {
            ConstantParameterValue anotherPropertyValue =
                (ConstantParameterValue)registration.InjectedProperties.First(p => p.PropertyName == "AnotherProperty").PropertyValue;

            Assert.AreEqual(42, anotherPropertyValue.Value);

            ConstantParameterValue propertyValue =
                (ConstantParameterValue)registration.InjectedProperties.First(p => p.PropertyName == "Property").PropertyValue;
            Assert.AreEqual("value", propertyValue.Value);
        }
    }

    [TestClass]
    public class GivenExpressionWithContainerResolvedParameters
    {
        private Expression<Func<TypeWithProperties>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            expression = () => new TypeWithProperties
                {
                    ResolvedProperty = Container.Resolved<IFoo>(),
                    EnumerationResovledProperty = Container.ResolvedEnumerable<IFoo>(new string[] { "one", "two" })
                };
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void ThenContainerResovledParameterTypeIsSuppliedForResolvedProperty()
        {
            Assert.IsInstanceOfType(
                registration.InjectedProperties.First(p => p.PropertyName == "ResolvedProperty").PropertyValue,
                typeof(ContainerResolvedParameter));
        }

        [TestMethod]
        public void ThenContainerResolvedEnumerableParameterTypeIsSuppliedForEnumerableProperty()
        {
            Assert.IsInstanceOfType(
            registration.InjectedProperties.First(p => p.PropertyName == "EnumerationResovledProperty").PropertyValue,
            typeof(ContainerResolvedEnumerableParameter));
        }
    }

    [TestClass]
    public class GivenRegistrationWithConstructorAndPropertyInitializers
    {
        private Expression<Func<TypeWithProperties>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            expression = () => new TypeWithProperties(new Foo())
            {
                ResolvedProperty = Container.Resolved<IFoo>(),
                EnumerationResovledProperty = Container.ResolvedEnumerable<IFoo>(new string[] { "one", "two" })
            };
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void ThenRegistrationConstructorPropertyCountMatchesSuppliedConstructorParameters()
        {
            Assert.AreEqual(1, registration.ConstructorParameters.Count());
        }

        [TestMethod]
        public void ThenNumberOfPropertyParametersMatchesSuppliedNumberOfPropertyInitializers()
        {
            Assert.AreEqual(2, registration.InjectedProperties.Count());
        }
    }

    [TestClass]
    public class GivenAnUnsupportedNestedInitializationExpression
    {
        private Expression<Func<TypeWithProperties>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            expression = () => new TypeWithProperties
                                   {
                                       Property = "value",
                                       SomeOtherTypeWithProperties = { Property = "foo" }
                                   };

            registration = new TypeRegistration<TypeWithProperties>(expression);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ThenShouldThrowOnUnsupportedInitializationExpression()
        {
            registration.InjectedProperties.ElementAt(1);
        }
    }

    [TestClass]
    public class GivenAnUnsupportedCollectionInitializationExpression
    {
        private Expression<Func<TypeWithProperties>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            expression = () => new TypeWithProperties
            {
                Property = "value",
                CollectionProperty = { "foo", "bar" }
            };

            registration = new TypeRegistration<TypeWithProperties>(expression);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ThenShouldThrowOnUnsupportedInitializationExpression()
        {
            registration.InjectedProperties.ElementAt(1);
        }
    }

    [TestClass]
    public class GivenALambdaExpressionWithAnOptionalResolvedParameter
    {
        private Expression<Func<Foo>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo(
                "aString",
                Container.Resolved<IBar>(),
                Container.ResolvedIfNotNull<IBar>("anotherBar"));
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void ThenRegistrationHasOptionalContainerResolvedParameter()
        {
            ContainerResolvedParameter param =
                (ContainerResolvedParameter)registration.ConstructorParameters.ElementAt(2);
            Assert.AreEqual(typeof(IBar), param.Type);
            Assert.AreEqual("anotherBar", param.Name);
        }
    }

    [TestClass]
    public class GivenALambdaExpressionWithAnOptionalResolvedParameterWithoutName
    {
        private Expression<Func<Foo>> expression;
        private TypeRegistration registration;

        [TestInitialize]
        public void Setup()
        {
            expression = () => new Foo(
                "aString",
                Container.Resolved<IBar>(),
                Container.ResolvedIfNotNull<IBar>(null));
            registration = new TypeRegistration(expression);
        }

        [TestMethod]
        public void ThenRegistrationHasOptionalContainerResolvedParameter()
        {
            ConstantParameterValue param =
                (ConstantParameterValue)registration.ConstructorParameters.ElementAt(2);
            Assert.AreEqual(typeof(IBar), param.Type);
            Assert.AreEqual(null, param.Value);
        }

        [TestMethod]
        public void ThenRegistrationLifetimeIsSingleton()
        {
            Assert.AreEqual(TypeRegistrationLifetime.Singleton, registration.Lifetime);
        }
	
    }

    [TestClass]
    public class TypeWithProperties
    {
        public TypeWithProperties()
        {

        }

        public TypeWithProperties(IFoo fooParameter)
        {

        }
        public string Property { get; set; }
        public int AnotherProperty { get; set; }
        public IFoo ResolvedProperty { get; set; }
        public IEnumerable<IFoo> EnumerationResovledProperty { get; set; }
        public ICollection<string> CollectionProperty { get; set; }

        public TypeWithProperties SomeOtherTypeWithProperties { get; set; }
    }


    public interface IFoo
    {

    }

    class Foo : IFoo
    {
        public Foo()
        {

        }

        public Foo(string constantString, IBar bar, IBar bar2)
        {
        }

        public Foo(IEnumerable<IBar> barList)
        {

        }

    }

    internal interface IBar
    {
    }
}
