//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Integration
{
    [TestClass]
    public class ValidationIntegrationHelperFixture
    {
        object convertedValue = new object();
        object valueToConvert;
        object originalConvertedValue;
        const string conversionErrorMessage = "failure";

        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
            valueToConvert = null;
            originalConvertedValue = null;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HelperCreationWithNullProxyThrows()
        {
            new ValidationIntegrationHelper(null);
        }

        #region value access

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateInstanceForProxyWithNullValidatedTypeThrows()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "ObjectProperty",
                                                                             null);
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateInstanceForProxyWithNullValidatedPropertyNameThrows()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             null,
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateInstanceForProxyWithEmptyValidatedPropertyNameThrows()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateInstanceForProxyWithInvalidValidatedPropertyNameThrows()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "Invalid",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateInstanceForProxyWithNonPublicValidatedPropertyNameThrows()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "NonPublic",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateInstanceForProxyWithPublicNonReadableValidatedPropertyNameThrows()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "NonReadable",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);
        }

        [TestMethod]
        public void CanGetObjectValueFromIntegrationProxy()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "ObjectProperty",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);

            object retrievedValue;
            string valueRetrievalFailureMessage;

            bool status = integrationHelper.GetValue(out retrievedValue, out valueRetrievalFailureMessage);

            Assert.IsTrue(status);
            Assert.AreSame(value, retrievedValue);
            Assert.AreEqual(null, valueRetrievalFailureMessage);
        }

        [TestMethod]
        public void CanGetValueConvertedThroughEventFromIntegrationProxy()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "ObjectProperty",
                                                                             typeof(MockValidatedType));
            integrationProxy.ValueConvertEvent += OnValueConvert;
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);

            object retrievedValue;
            string valueRetrievalFailureMessage;

            bool status = integrationHelper.GetValue(out retrievedValue, out valueRetrievalFailureMessage);

            Assert.IsTrue(status);
            Assert.AreSame(convertedValue, retrievedValue);
            Assert.AreEqual(null, valueRetrievalFailureMessage);
            Assert.AreSame(value, originalConvertedValue);
            Assert.AreSame(value, valueToConvert);
        }

        [TestMethod]
        public void GetValueConvertedThroughEventSettingFailureMessageFromIntegrationProxyReturnsFailure()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "ObjectProperty",
                                                                             typeof(MockValidatedType));
            integrationProxy.ValueConvertEvent += OnValueConvertWithFailure;
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);

            object retrievedValue;
            string valueRetrievalFailureMessage;

            bool status = integrationHelper.GetValue(out retrievedValue, out valueRetrievalFailureMessage);

            Assert.IsFalse(status);
            Assert.AreEqual(null, retrievedValue);
            Assert.AreEqual(conversionErrorMessage, valueRetrievalFailureMessage);
            Assert.AreSame(value, originalConvertedValue);
            Assert.AreSame(value, valueToConvert);
        }

        [TestMethod]
        public void CanGetValueConvertedWithDefaultConversionIfNoEventHandlerSet()
        {
            object value = "00012345";
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "IntProperty",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);

            object retrievedValue;
            string valueRetrievalFailureMessage;

            bool status = integrationHelper.GetValue(out retrievedValue, out valueRetrievalFailureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual(12345, retrievedValue);
            Assert.AreEqual(null, valueRetrievalFailureMessage);
            Assert.AreEqual(null, originalConvertedValue);
            Assert.AreEqual(null, valueToConvert);
        }

        [TestMethod]
        public void CanGetValueConvertedWithDefaultConversionFromNullValueIfNoEventHandlerSet()
        {
            object value = null;
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "ObjectProperty",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);

            object retrievedValue;
            string valueRetrievalFailureMessage;

            bool status = integrationHelper.GetValue(out retrievedValue, out valueRetrievalFailureMessage);

            Assert.IsTrue(status);
            Assert.AreEqual(null, retrievedValue);
            Assert.AreEqual(null, valueRetrievalFailureMessage);
            Assert.AreEqual(null, originalConvertedValue);
            Assert.AreEqual(null, valueToConvert);
        }

        [TestMethod]
        public void GetValueConvertedWithDefaultConversionReturnsFailureIfConversionIsNotPossible()
        {
            object value = "00012345abc";
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "IntProperty",
                                                                             typeof(MockValidatedType));
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);

            object retrievedValue;
            string valueRetrievalFailureMessage;

            bool status = integrationHelper.GetValue(out retrievedValue, out valueRetrievalFailureMessage);

            Assert.IsFalse(status);
            Assert.AreEqual(null, retrievedValue);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.ErrorCannotPerfomDefaultConversion, valueRetrievalFailureMessage));
            Assert.AreEqual(null, originalConvertedValue);
            Assert.AreEqual(null, valueToConvert);
        }

        void OnValueConvert(object source,
                            ValueConvertEventArgs e)
        {
            valueToConvert = e.ValueToConvert;
            originalConvertedValue = e.ConvertedValue;
            e.ConvertedValue = convertedValue;
        }

        void OnValueConvertWithFailure(object source,
                                       ValueConvertEventArgs e)
        {
            valueToConvert = e.ValueToConvert;
            originalConvertedValue = e.ConvertedValue;
            e.ConversionErrorMessage = conversionErrorMessage;
        }

        #endregion

        #region validation building

        [TestMethod]
        public void CanBuildValidatorForProperty()
        {
            object value = new object();
            MockIntegrationProxy integrationProxy = new MockIntegrationProxy(value,
                                                                             "",
                                                                             ValidationSpecificationSource.Attributes,
                                                                             "ObjectProperty",
                                                                             typeof(MockValidatedType));
            integrationProxy.ValueConvertEvent += OnValueConvertWithFailure;
            ValidationIntegrationHelper integrationHelper = new ValidationIntegrationHelper(integrationProxy);

            Validator validator = integrationHelper.GetValidator();
            //Assert.IsNotNull(validator);

            //MockValidatedType instance = new MockValidatedType();
            //ValidationResults validationResults = validator.Validate(instance);
            //Assert.IsFalse(validationResults.IsValid);
            //IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
            //Assert.AreEqual(1, resultsList.Count);
            //Assert.AreEqual("object property message", resultsList[0].Message);
        }

        #endregion

        public class MockIntegrationProxy : IValidationIntegrationProxy
        {
            public object rawValue;
            public string rulesetField;
            public ValidationSpecificationSource specificationSourceField;
            public string validatedPropertyNameField;
            public Type validatedTypeField;

            public MockIntegrationProxy(object rawValue,
                                        string ruleset,
                                        ValidationSpecificationSource specificationSource,
                                        string validatedPropertyName,
                                        Type validatedType)
            {
                this.rawValue = rawValue;
                rulesetField = ruleset;
                specificationSourceField = specificationSource;
                validatedPropertyNameField = validatedPropertyName;
                validatedTypeField = validatedType;
            }

            public bool ProvidesCustomValueConversion
            {
                get { return ValueConvert != null; }
            }

            public string Ruleset
            {
                get { return rulesetField; }
            }

            public ValidationSpecificationSource SpecificationSource
            {
                get { return specificationSourceField; }
            }

            public string ValidatedPropertyName
            {
                get { return validatedPropertyNameField; }
            }

            public Type ValidatedType
            {
                get { return validatedTypeField; }
            }

            public EventHandler<ValueConvertEventArgs> ValueConvert
            {
                get { return ValueConvertEvent; }
            }

            public MemberValueAccessBuilder GetMemberValueAccessBuilder()
            {
                return new ReflectionMemberValueAccessBuilder();
            }

            public object GetRawValue()
            {
                return rawValue;
            }

            public void PerformCustomValueConversion(ValueConvertEventArgs e)
            {
                if (ValueConvert != null)
                {
                    ValueConvert(this, e);
                }
            }

            public event EventHandler<ValueConvertEventArgs> ValueConvertEvent;
        }

        public class MockValidatedType
        {
            int intProperty;
            object objectProperty;

            public int IntProperty
            {
                get { return intProperty; }
                set { intProperty = value; }
            }

            object NonPublic
            {
                get { return null; }
            }

            public object NonReadable
            {
                set { ; }
            }

            [MockValidator(true, MessageTemplate = "object property message")]
            public object ObjectProperty
            {
                get { return objectProperty; }
                set { objectProperty = value; }
            }
        }
    }
}
