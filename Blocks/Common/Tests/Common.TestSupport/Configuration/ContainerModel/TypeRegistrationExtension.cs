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
        public static IAssertRegistrationProperties AssertForServiceType(this TypeRegistration typeRegistration, Type serviceType)
        {
            Assert.AreEqual(serviceType, typeRegistration.ServiceType);

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

        private class TypeRegistrationAssertion : IAssertRegistrationProperties, IAssertConstructor, IAssertProperties
        {
            private readonly TypeRegistration _registration;
            private int _currentParameterIndex;
            private HashSet<string> _verifiedProperties;

            public TypeRegistrationAssertion(TypeRegistration registration)
            {
                Assert.IsFalse(string.IsNullOrEmpty(registration.Name));

                _registration = registration;
                _verifiedProperties = new HashSet<string>();
            }

            public IAssertRegistrationProperties ForName(string name)
            {
                Assert.AreEqual(name, _registration.Name);

                return this;
            }

            public IAssertRegistrationProperties IsDefault()
            {
                Assert.IsTrue(_registration.IsDefault);
                return this;
            }

            public IAssertRegistrationProperties IsNotDefault()
            {
                Assert.IsFalse(_registration.IsDefault);
                return this;
            }

            public IAssertRegistrationProperties IsSingleton()
            {
                Assert.AreEqual(TypeRegistrationLifetime.Singleton, _registration.Lifetime);
                return this;
            }

            public IAssertRegistrationProperties IsTransient()
            {
                Assert.AreEqual(TypeRegistrationLifetime.Transient, _registration.Lifetime);
                return this;
            }

            public IAssertConstructor WithValueConstructorParameter<T>(T parameterValue)
            {
                CheckConstantParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex),
                    parameterValue,
                    "parameter " + _currentParameterIndex++);

                return this;
            }

            public IAssertConstructor WithValueConstructorParameter<T>(out T parameterValue)
            {
                CheckConstantParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex),
                    out parameterValue,
                    "parameter " + _currentParameterIndex++);

                return this;
            }

            public IAssertConstructor WithContainerResolvedEnumerableConstructorParameter<T>(string[] names)
            {
                CheckContainerResolvedEnumerableParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex),
                    names,
                    "parameter " + _currentParameterIndex++);

                return this;
            }

            public IAssertConstructor WithContainerResolvedEnumerableConstructorParameter<T>(out string[] names)
            {
                CheckContainerResolvedEnumerableParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex),
                    out names,
                    "parameter " + _currentParameterIndex++);

                return this;
            }

            public IAssertConstructor WithContainerResolvedParameter<T>(string name)
            {
                CheckContainerResolvedParameterValue<T>(
                    _registration.ConstructorParameters.ElementAt(_currentParameterIndex),
                    name,
                    "parameter " + _currentParameterIndex++);

                return this;
            }

            public IAssertRegistrationProperties ForImplementationType(Type implementationType)
            {
                Assert.AreEqual(implementationType, _registration.ImplementationType);

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
                    parameterValue,
                    "property " + propertyName);

                return this;
            }

            public IAssertProperties WithValueProperty<T>(string propertyName, out T parameterValue)
            {
                CheckConstantParameterValue<T>(
                    GetInjectedProperty(propertyName).PropertyValue,
                    out parameterValue,
                    "property " + propertyName);

                return this;
            }

            public IAssertProperties WithContainerResolvedEnumerableProperty<T>(string propertyName, string[] names)
            {
                CheckContainerResolvedEnumerableParameterValue<T>(
                    GetInjectedProperty(propertyName).PropertyValue,
                    names,
                    "property " + propertyName);

                return this;
            }

            public IAssertProperties WithContainerResolvedProperty<T>(string propertyName, string name)
            {
                CheckContainerResolvedParameterValue<T>(
                    GetInjectedProperty(propertyName).PropertyValue,
                    name,
                    "property " + propertyName);

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

            private static void CheckConstantParameterValue<T>(ParameterValue parameterValue, T value, string description)
            {
                var constantParameterValue =
                    GetCheckedParameterValue<ConstantParameterValue>(parameterValue, description);

                Assert.AreEqual(typeof(T), constantParameterValue.Type, description);
                Assert.AreEqual(value, constantParameterValue.Value, description);
            }

            private static void CheckConstantParameterValue<T>(ParameterValue parameterValue, out T value, string description)
            {
                var constantParameterValue =
                    GetCheckedParameterValue<ConstantParameterValue>(parameterValue, description);

                Assert.IsTrue(typeof(T).IsAssignableFrom(constantParameterValue.Type), description);
                value = (T)constantParameterValue.Value;
            }

            private void CheckContainerResolvedEnumerableParameterValue<T>(ParameterValue parameterValue, string[] names, string description)
            {
                var containerResolvedEnumerableParameter =
                    GetCheckedParameterValue<ContainerResolvedEnumerableParameter>(parameterValue, description);

                Assert.AreEqual(typeof(IEnumerable<T>), containerResolvedEnumerableParameter.Type, description);
                CollectionAssert.AreEqual(names, new List<string>(containerResolvedEnumerableParameter.Names), description);
            }

            private void CheckContainerResolvedEnumerableParameterValue<T>(ParameterValue parameterValue, out string[] names, string description)
            {
                var containerResolvedEnumerableParameter =
                    GetCheckedParameterValue<ContainerResolvedEnumerableParameter>(parameterValue, description);

                Assert.AreEqual(typeof(IEnumerable<T>), containerResolvedEnumerableParameter.Type, description);
                names = containerResolvedEnumerableParameter.Names.ToArray();
            }

            private void CheckContainerResolvedParameterValue<T>(ParameterValue parameterValue, string name, string description)
            {
                var containerResolvedParameter =
                    GetCheckedParameterValue<ContainerResolvedParameter>(parameterValue, description);

                Assert.AreEqual(typeof(T), containerResolvedParameter.Type, description);
                Assert.AreEqual(name, containerResolvedParameter.Name, description);
            }

            private static T GetCheckedParameterValue<T>(ParameterValue parameterValue, string description)
                where T : ParameterValue
            {
                var containerResolvedParameter = parameterValue as T;
                if (containerResolvedParameter == null)
                {
                    Assert.Fail(
                        string.Format(
                            "Invalid type for {2}: expected {0} but got {1}",
                            typeof(T).Name,
                            parameterValue.GetType().Name,
                            description));
                }
                return containerResolvedParameter;
            }
        }

        public interface IAssertRegistrationProperties : IHideObjectMembers
        {
            IAssertRegistrationProperties ForName(string name);
            IAssertRegistrationProperties IsDefault();
            IAssertRegistrationProperties IsNotDefault();
            IAssertRegistrationProperties IsSingleton();
            IAssertRegistrationProperties IsTransient();
            IAssertRegistrationProperties ForImplementationType(Type implementationType);
        }

        public interface IAssertConstructor : IHideObjectMembers
        {
            IAssertConstructor WithValueConstructorParameter<T>(T parameterValue);
            IAssertConstructor WithValueConstructorParameter<T>(out T parameterValue);
            IAssertConstructor WithContainerResolvedEnumerableConstructorParameter<T>(string[] names);
            IAssertConstructor WithContainerResolvedEnumerableConstructorParameter<T>(out string[] names);
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
