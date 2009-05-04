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

using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ConfigurationValidatedTypeFixture
    {
        [TestMethod]
        public void CreatedInstanceReturnsCorrectValuesOnImplementedInterfaces()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));

            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass), ((IValidatedType)validatedType).MemberInfo);
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass), ((IValidatedType)validatedType).TargetType);
            Assert.AreEqual(CompositionType.And, ((IValidatedType)validatedType).CompositionType);
            Assert.AreEqual(null, ((IValidatedType)validatedType).CompositionMessageTemplate);
            Assert.AreEqual(false, ((IValidatedType)validatedType).IgnoreNulls);
            Assert.AreEqual(null, ((IValidatedType)validatedType).IgnoreNullsMessageTemplate);
        }

        [TestMethod]
        public void InstanceReturnsCorrectValidatorDescriptors()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            rulesetData.Validators.Add(new MockValidatorData("validator1", false));
            rulesetData.Validators.Get("validator1").MessageTemplate = "type validator 1 message";
            rulesetData.Validators.Add(new MockValidatorData("validator2", false));
            rulesetData.Validators.Get("validator2").MessageTemplate = "type validator 2 message";

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));

            IEnumerator<IValidatorDescriptor> validatorDescriptorsEnumerator
                = ((IValidatedType)validatedType).GetValidatorDescriptors().GetEnumerator();
            Assert.IsNotNull(validatorDescriptorsEnumerator);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("type validator 1 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsTrue(validatorDescriptorsEnumerator.MoveNext());
            Assert.AreEqual("type validator 2 message",
                            ((MockValidatorData)validatorDescriptorsEnumerator.Current).MessageTemplate);
            Assert.IsFalse(validatorDescriptorsEnumerator.MoveNext());
        }

        [TestMethod]
        public void DoesNotSupportSelfValidation()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));

            IEnumerator<MethodInfo> selfValidationMethodsEnumerator
                = ((IValidatedType)validatedType).GetSelfValidationMethods().GetEnumerator();

            Assert.IsFalse(selfValidationMethodsEnumerator.MoveNext());
        }

        #region properties

        [TestMethod]
        public void ValidatedPropertiesEnumerableIsEmptyIfRulesetDataHasNoProperties()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedPropertiesEnumerableSkipsNonExistingPropertyWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedPropertyReference nonExistingPropertyReference
                = new ValidatedPropertyReference("NonExistingProperty");
            rulesetData.Properties.Add(nonExistingPropertyReference);
            nonExistingPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedPropertiesEnumerableSkipsPublicPropertyWithoutValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedPropertyReference publicPropertyWithoutValidatorsReference
                = new ValidatedPropertyReference("PublicPropertyWithoutValidators");
            rulesetData.Properties.Add(publicPropertyWithoutValidatorsReference);

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedPropertiesEnumerableSkipsWriteOnlyPublicPropertyWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedPropertyReference writeOnlyPublicPropertyReference
                = new ValidatedPropertyReference("WriteOnlyPublicProperty");
            rulesetData.Properties.Add(writeOnlyPublicPropertyReference);
            writeOnlyPublicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedPropertiesEnumerableSkipsIndexedPublicPropertyWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedPropertyReference writeOnlyPublicPropertyReference
                = new ValidatedPropertyReference("Item");
            rulesetData.Properties.Add(writeOnlyPublicPropertyReference);
            writeOnlyPublicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedPropertiesEnumerableSkipsNonPublicPropertyWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedPropertyReference nonPublicPropertyReference
                = new ValidatedPropertyReference("NonPublicProperty");
            rulesetData.Properties.Add(nonPublicPropertyReference);
            nonPublicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedPropertiesEnumerableIncludesPublicPropertyWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedPropertyReference publicPropertyReference
                = new ValidatedPropertyReference("PublicProperty");
            rulesetData.Properties.Add(publicPropertyReference);
            publicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsTrue(validatedPropertiesEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetProperty("PublicProperty"),
                           validatedPropertiesEnumerator.Current.MemberInfo);
            Assert.AreSame(typeof(string), validatedPropertiesEnumerator.Current.TargetType);

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedPropertiesEnumerableIncludesPublicPropertyWithValidatorsOnly()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ValidatedPropertyReference nonExistingPropertyReference
                = new ValidatedPropertyReference("NonExistingProperty");
            rulesetData.Properties.Add(nonExistingPropertyReference);
            nonExistingPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedPropertyReference publicPropertyWithoutValidatorsReference
                = new ValidatedPropertyReference("PublicPropertyWithoutValidators");
            rulesetData.Properties.Add(publicPropertyWithoutValidatorsReference);

            ValidatedPropertyReference writeOnlyPublicPropertyReference
                = new ValidatedPropertyReference("WriteOnlyPublicProperty");
            rulesetData.Properties.Add(writeOnlyPublicPropertyReference);
            writeOnlyPublicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedPropertyReference nonPublicPropertyReference
                = new ValidatedPropertyReference("NonPublicProperty");
            rulesetData.Properties.Add(nonPublicPropertyReference);
            nonPublicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedPropertyReference publicPropertyReference
                = new ValidatedPropertyReference("PublicProperty");
            rulesetData.Properties.Add(publicPropertyReference);
            publicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedPropertyReference secondPublicPropertyReference
                = new ValidatedPropertyReference("SecondPublicProperty");
            rulesetData.Properties.Add(secondPublicPropertyReference);
            secondPublicPropertyReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedPropertiesEnumerator
                = ((IValidatedType)validatedType).GetValidatedProperties().GetEnumerator();

            Assert.IsTrue(validatedPropertiesEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetProperty("PublicProperty"),
                           validatedPropertiesEnumerator.Current.MemberInfo);
            Assert.IsTrue(validatedPropertiesEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetProperty("SecondPublicProperty"),
                           validatedPropertiesEnumerator.Current.MemberInfo);
            Assert.AreSame(typeof(string), validatedPropertiesEnumerator.Current.TargetType);

            Assert.IsFalse(validatedPropertiesEnumerator.MoveNext());
        }

        #endregion

        #region fields

        [TestMethod]
        public void ValidatedFieldsEnumerableIsEmptyIfRulesetDataHasNoFields()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedFieldsEnumerator
                = ((IValidatedType)validatedType).GetValidatedFields().GetEnumerator();

            Assert.IsFalse(validatedFieldsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedFieldsEnumerableSkipsNonExistingFieldWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedFieldReference nonExistingFieldReference
                = new ValidatedFieldReference("NonExistingField");
            rulesetData.Fields.Add(nonExistingFieldReference);
            nonExistingFieldReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedFieldsEnumerator
                = ((IValidatedType)validatedType).GetValidatedFields().GetEnumerator();

            Assert.IsFalse(validatedFieldsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedFieldsEnumerableSkipsPublicFieldWithoutValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedFieldReference publicFieldWithoutValidatorsReference
                = new ValidatedFieldReference("PublicFieldWithoutValidators");
            rulesetData.Fields.Add(publicFieldWithoutValidatorsReference);

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedFieldsEnumerator
                = ((IValidatedType)validatedType).GetValidatedFields().GetEnumerator();

            Assert.IsFalse(validatedFieldsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedFieldsEnumerableSkipsNonPublicFieldWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedFieldReference nonPublicFieldReference
                = new ValidatedFieldReference("NonPublicField");
            rulesetData.Fields.Add(nonPublicFieldReference);
            nonPublicFieldReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedFieldsEnumerator
                = ((IValidatedType)validatedType).GetValidatedFields().GetEnumerator();

            Assert.IsFalse(validatedFieldsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedFieldsEnumerableIncludesPublicFieldWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedFieldReference publicFieldReference
                = new ValidatedFieldReference("PublicField");
            rulesetData.Fields.Add(publicFieldReference);
            publicFieldReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedFieldsEnumerator
                = ((IValidatedType)validatedType).GetValidatedFields().GetEnumerator();

            Assert.IsTrue(validatedFieldsEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetField("PublicField"),
                           validatedFieldsEnumerator.Current.MemberInfo);
            Assert.AreSame(typeof(string), validatedFieldsEnumerator.Current.TargetType);

            Assert.IsFalse(validatedFieldsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedFieldsEnumerableIncludesPublicFieldWithValidatorsOnly()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ValidatedFieldReference nonExistingFieldReference
                = new ValidatedFieldReference("NonExistingField");
            rulesetData.Fields.Add(nonExistingFieldReference);
            nonExistingFieldReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedFieldReference publicFieldWithoutValidatorsReference
                = new ValidatedFieldReference("PublicFieldWithoutValidators");
            rulesetData.Fields.Add(publicFieldWithoutValidatorsReference);

            ValidatedFieldReference nonPublicFieldReference
                = new ValidatedFieldReference("NonPublicField");
            rulesetData.Fields.Add(nonPublicFieldReference);
            nonPublicFieldReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedFieldReference publicFieldReference
                = new ValidatedFieldReference("PublicField");
            rulesetData.Fields.Add(publicFieldReference);
            publicFieldReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedFieldReference secondPublicFieldReference
                = new ValidatedFieldReference("SecondPublicField");
            rulesetData.Fields.Add(secondPublicFieldReference);
            secondPublicFieldReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedFieldsEnumerator
                = ((IValidatedType)validatedType).GetValidatedFields().GetEnumerator();

            Assert.IsTrue(validatedFieldsEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetField("PublicField"),
                           validatedFieldsEnumerator.Current.MemberInfo);
            Assert.IsTrue(validatedFieldsEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetField("SecondPublicField"),
                           validatedFieldsEnumerator.Current.MemberInfo);
            Assert.AreSame(typeof(string), validatedFieldsEnumerator.Current.TargetType);

            Assert.IsFalse(validatedFieldsEnumerator.MoveNext());
        }

        #endregion

        #region methods

        [TestMethod]
        public void ValidatedMethodsEnumerableIsEmptyIfRulesetDataHasNoMethods()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableSkipsNonExistingMethodWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedMethodReference nonExistingMethodReference
                = new ValidatedMethodReference("NonExistingMethod");
            rulesetData.Methods.Add(nonExistingMethodReference);
            nonExistingMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableSkipsPublicMethodWithoutValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedMethodReference publicMethodWithoutValidatorsReference
                = new ValidatedMethodReference("PublicMethodWithoutValidators");
            rulesetData.Methods.Add(publicMethodWithoutValidatorsReference);

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableSkipsVoidPublicMethodWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedMethodReference nonPublicMethodReference
                = new ValidatedMethodReference("VoidPublicMethod");
            rulesetData.Methods.Add(nonPublicMethodReference);
            nonPublicMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableSkipsPublicMethodWithParametersWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedMethodReference nonPublicMethodReference
                = new ValidatedMethodReference("PublicMethodWithParameters");
            rulesetData.Methods.Add(nonPublicMethodReference);
            nonPublicMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableSkipsNonPublicMethodWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedMethodReference nonPublicMethodReference
                = new ValidatedMethodReference("NonPublicMethod");
            rulesetData.Methods.Add(nonPublicMethodReference);
            nonPublicMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableIncludesPublicMethodWithValidators()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();
            ValidatedMethodReference publicMethodReference
                = new ValidatedMethodReference("PublicMethod");
            rulesetData.Methods.Add(publicMethodReference);
            publicMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsTrue(validatedMethodsEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetMethod("PublicMethod"),
                           validatedMethodsEnumerator.Current.MemberInfo);
            Assert.AreSame(typeof(string), validatedMethodsEnumerator.Current.TargetType);

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableIncludesPublicMethodWithValidatorsOnly()
        {
            ValidationRulesetData rulesetData = new ValidationRulesetData();

            ValidatedMethodReference nonExistingMethodReference
                = new ValidatedMethodReference("NonExistingMethod");
            rulesetData.Methods.Add(nonExistingMethodReference);
            nonExistingMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedMethodReference publicMethodWithoutValidatorsReference
                = new ValidatedMethodReference("PublicMethodWithoutValidators");
            rulesetData.Methods.Add(publicMethodWithoutValidatorsReference);

            ValidatedMethodReference nonPublicMethodReference
                = new ValidatedMethodReference("NonPublicMethod");
            rulesetData.Methods.Add(nonPublicMethodReference);
            nonPublicMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedMethodReference publicMethodReference
                = new ValidatedMethodReference("PublicMethod");
            rulesetData.Methods.Add(publicMethodReference);
            publicMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ValidatedMethodReference secondPublicMethodReference
                = new ValidatedMethodReference("SecondPublicMethod");
            rulesetData.Methods.Add(secondPublicMethodReference);
            secondPublicMethodReference.Validators.Add(new MockValidatorData("validator1", false));

            ConfigurationValidatedType validatedType
                = new ConfigurationValidatedType(rulesetData, typeof(ConfigurationValidatedTypeFixtureTestClass));
            IEnumerator<IValidatedElement> validatedMethodsEnumerator
                = ((IValidatedType)validatedType).GetValidatedMethods().GetEnumerator();

            Assert.IsTrue(validatedMethodsEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetMethod("PublicMethod"),
                           validatedMethodsEnumerator.Current.MemberInfo);
            Assert.IsTrue(validatedMethodsEnumerator.MoveNext());
            Assert.AreSame(typeof(ConfigurationValidatedTypeFixtureTestClass).GetMethod("SecondPublicMethod"),
                           validatedMethodsEnumerator.Current.MemberInfo);
            Assert.AreSame(typeof(string), validatedMethodsEnumerator.Current.TargetType);

            Assert.IsFalse(validatedMethodsEnumerator.MoveNext());
        }

        #endregion

        public class ConfigurationValidatedTypeFixtureTestClass
        {
            internal string NonPublicField = null;
            public string PublicField;
            public string PublicFieldWithoutValidators;
            public string SecondPublicField;

            public string this[int index]
            {
                get { return null; }
            }

            internal string NonPublicProperty
            {
                get { return null; }
            }

            public string PublicProperty
            {
                get { return null; }
            }

            public string PublicPropertyWithoutValidators
            {
                get { return null; }
            }

            public string SecondPublicProperty
            {
                get { return null; }
            }

            public string WriteOnlyPublicProperty
            {
                set { ; }
            }

            internal string NonPublicMethod()
            {
                return null;
            }

            public string PublicMethod()
            {
                return null;
            }

            public string PublicMethodWithoutValidators()
            {
                return null;
            }

            public string PublicMethodWithParameters(string parameter)
            {
                return null;
            }

            public string SecondPublicMethod()
            {
                return null;
            }

            public void VoidMethod() { }
        }
    }
}
