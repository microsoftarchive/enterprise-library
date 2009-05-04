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
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel
{
    public static class TypeRegistrationExtension
    {
        public static IAssertName AssertForServiceType(this TypeRegistration typeRegistration, Type serviceType)
        {
            Assert.AreSame(serviceType, typeRegistration.ServiceType);

            return new TypeRegistrationAssertion(typeRegistration);
        }

        public static IAssertConstructor AssertConstructor(this TypeRegistration typeRegistration)
        {
            return new TypeRegistrationAssertion(typeRegistration);
        }

        public static IAssertProperties AssertProperties(this TypeRegistration typeRegistration)
        {
            return new TypeRegistrationAssertion(typeRegistration);
        }

        private class TypeRegistrationAssertion : IAssertName, IAssertConstructor, IAssertProperties
        {
            private readonly TypeRegistration _registration;
            private int _currentParameterIndex;
            private HashSet<string> _verifiedProperties;

            public TypeRegistrationAssertion(TypeRegistration registration)
            {
                _registration = registration;
                _verifiedProperties = new HashSet<string>();
            }

            public IAssertName ForName(string name)
            {
                Assert.AreEqual(name, _registration.Name);

                return this;
            }

            public IAssertConstructor WithValueConstructorParameter<T>(T parameterValue)
            {
                CheckConstantParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex++),
                    parameterValue);

                return this;
            }

            public IAssertConstructor WithValueConstructorParameter<T>(out T parameterValue)
            {
                CheckConstantParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex++),
                    out parameterValue);

                return this;
            }

            public IAssertConstructor WithContainerResolvedEnumerableConstructorParameter<T>(string[] names)
            {
                CheckContainerResolvedEnumerableParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex++),
                    names);

                return this;
            }

            public IAssertConstructor WithContainerResolvedParameter<T>(string name)
            {
                CheckContainerResolvedParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex++),
                    name);

                return this;
            }

            public IAssertName ForImplementationType(Type implementationType)
            {
                Assert.AreSame(implementationType, _registration.ImplementationType);

                return this;
            }

            public void VerifyConstructorParameters()
            {
                Assert.AreEqual(_currentParameterIndex, _registration.ConstructorParameters.Count());
            }

            public IAssertProperties WithValueProperty<T>(string propertyName, T parameterValue)
            {
                CheckConstantParameterValue<T>(
                    GetInjectedProperty(propertyName).PropertyValue,
                    parameterValue);

                return this;
            }

            public IAssertProperties WithValueProperty<T>(string propertyName, out T parameterValue)
            {
                CheckConstantParameterValue<T>(
                    GetInjectedProperty(propertyName).PropertyValue,
                    out parameterValue);

                return this;
            }

            public IAssertProperties WithContainerResolvedEnumerableProperty<T>(string propertyName, string[] names)
            {
                CheckContainerResolvedEnumerableParameterValue<T>(
                    GetInjectedProperty(propertyName).PropertyValue,
                    names);

                return this;
            }

            public IAssertProperties WithContainerResolvedProperty<T>(string propertyName, string name)
            {
                CheckContainerResolvedParameterValue<T>(
                    GetInjectedProperty(propertyName).PropertyValue,
                    name);

                return this;
            }

            public void VerifyProperties()
            {
                Assert.AreEqual(
                    _registration.InjectedProperties.Count(),
                    _verifiedProperties.Count,
                    "Didn't verify all the properties in the type registration");
            }

            private InjectedProperty GetInjectedProperty(string propertyName)
            {
                if (_verifiedProperties.Contains(propertyName))
                {
                    Assert.Fail("Duplicate request for injected property {0}", propertyName);
                }

                _verifiedProperties.Add(propertyName);

                return _registration.InjectedProperties.First(p => p.PropertyName == propertyName);
            }

            private static void CheckConstantParameterValue<T>(ParameterValue parameterValue, T value)
            {
                var constantParameterValue = (ConstantParameterValue)parameterValue;

                Assert.AreSame(typeof(T), constantParameterValue.Type);
                Assert.AreEqual(value, constantParameterValue.Value);
            }

            private static void CheckConstantParameterValue<T>(ParameterValue parameterValue, out T value)
            {
                var constantParameterValue = (ConstantParameterValue)parameterValue;

                Assert.IsTrue(typeof(T).IsAssignableFrom(constantParameterValue.Type));
                value = (T)constantParameterValue.Value;
            }

            private void CheckContainerResolvedEnumerableParameterValue<T>(ParameterValue parameterValue, string[] names)
            {
                var containerResolvedEnumerableParameter = (ContainerResolvedEnumerableParameter)parameterValue;

                Assert.AreEqual(typeof(IEnumerable<T>), containerResolvedEnumerableParameter.Type);
                CollectionAssert.AreEqual(names, new List<string>(containerResolvedEnumerableParameter.Names));
            }

            private void CheckContainerResolvedParameterValue<T>(ParameterValue parameterValue, string name)
            {
                var containerResolvedParameter = (ContainerResolvedParameter)parameterValue;

                Assert.AreEqual(typeof(T), containerResolvedParameter.Type);
                Assert.AreEqual(name, containerResolvedParameter.Name);
            }
        }

        public interface IAssertName : IHideObjectMembers
        {
            IAssertName ForName(string name);
            IAssertName ForImplementationType(Type implementationType);
        }

        public interface IAssertConstructor : IHideObjectMembers
        {
            IAssertConstructor WithValueConstructorParameter<T>(T parameterValue);
            IAssertConstructor WithValueConstructorParameter<T>(out T parameterValue);
            IAssertConstructor WithContainerResolvedEnumerableConstructorParameter<T>(string[] names);
            IAssertConstructor WithContainerResolvedParameter<T>(string name);
            void VerifyConstructorParameters();
        }

        public interface IAssertProperties : IHideObjectMembers
        {
            IAssertProperties WithValueProperty<T>(string propertyName, T parameterValue);
            IAssertProperties WithValueProperty<T>(string propertyName, out T parameterValue);
            IAssertProperties WithContainerResolvedEnumerableProperty<T>(string propertyName, string[] names);
            IAssertProperties WithContainerResolvedProperty<T>(string propertyName, string name);
            void VerifyProperties();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public interface IHideObjectMembers
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            Type GetType();

            [EditorBrowsable(EditorBrowsableState.Never)]
            int GetHashCode();

            [EditorBrowsable(EditorBrowsableState.Never)]
            string ToString();

            [EditorBrowsable(EditorBrowsableState.Never)]
            bool Equals(object obj);
        }
    }
}
