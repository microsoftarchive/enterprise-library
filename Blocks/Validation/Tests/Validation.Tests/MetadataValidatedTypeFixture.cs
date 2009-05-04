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
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class MetadataValidatedTypeFixture
    {
        [TestMethod]
        public void CreatedInstanceReturnsCorrectValuesOnImplementedInterfaces()
        {
            MetadataValidatedType validatedType
                = new MetadataValidatedType(typeof(MetadataValidatedTypeFixtureTestClass), "");

            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass), ((IValidatedType)validatedType).MemberInfo);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass), ((IValidatedType)validatedType).TargetType);
            Assert.AreEqual(CompositionType.And, ((IValidatedType)validatedType).CompositionType);
            Assert.AreEqual(false, ((IValidatedType)validatedType).IgnoreNulls);
        }

        //[TestMethod]
        //public void DoesNotSupportSelfValidation()
        //{
        //    ValidationRulesetData rulesetData = new ValidationRulesetData();

        //    MetadataValidatedType validatedType
        //        = new MetadataValidatedType(rulesetData, typeof(MetadataValidatedTypeFixtureTestClass));

        //    IEnumerator<MethodInfo> selfValidationMethodsEnumerator
        //        = ((IValidatedType)validatedType).GetSelfValidationMethods().GetEnumerator();

        //    Assert.IsFalse(selfValidationMethodsEnumerator.MoveNext());
        //}

        [TestMethod]
        public void ValidatedPropertiesEnumerableIncludesNonIndexedReadablePublicPropertiesOnly()
        {
            MetadataValidatedType validatedType
                = new MetadataValidatedType(typeof(MetadataValidatedTypeFixtureTestClass), "");

            IEnumerable<IValidatedElement> validatedProperties
                = ((IValidatedType)validatedType).GetValidatedProperties();
            IDictionary<string, MemberInfo> validatedElementsMapping
                = CollectValidatedElements(validatedProperties);

            Assert.AreEqual(5, validatedElementsMapping.Count);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetProperty("InheritedPublicProperty"),
                           validatedElementsMapping["InheritedPublicProperty"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetProperty("PublicPropertyWithoutValidators"),
                           validatedElementsMapping["PublicPropertyWithoutValidators"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetProperty("PublicProperty"),
                           validatedElementsMapping["PublicProperty"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetProperty("SecondPublicProperty"),
                           validatedElementsMapping["SecondPublicProperty"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetProperty("PublicPropertyWithValidatorsForRuleset"),
                           validatedElementsMapping["PublicPropertyWithValidatorsForRuleset"]);
        }

        [TestMethod]
        public void ValidatedFieldsEnumerableIncludesPublicFieldsOnly()
        {
            MetadataValidatedType validatedType
                = new MetadataValidatedType(typeof(MetadataValidatedTypeFixtureTestClass), "");
            IEnumerable<IValidatedElement> validatedFields
                = ((IValidatedType)validatedType).GetValidatedFields();

            IDictionary<string, MemberInfo> validatedElementsMapping
                = CollectValidatedElements(validatedFields);

            Assert.AreEqual(5, validatedElementsMapping.Count);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetField("InheritedPublicField"),
                           validatedElementsMapping["InheritedPublicField"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetField("PublicFieldWithoutValidators"),
                           validatedElementsMapping["PublicFieldWithoutValidators"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetField("PublicField"),
                           validatedElementsMapping["PublicField"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetField("SecondPublicField"),
                           validatedElementsMapping["SecondPublicField"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetField("PublicFieldWithValidatorsForRuleset"),
                           validatedElementsMapping["PublicFieldWithValidatorsForRuleset"]);
        }

        [TestMethod]
        public void ValidatedMethodsEnumerableIncludesPublicMethodsOnly()
        {
            MetadataValidatedType validatedType
                = new MetadataValidatedType(typeof(MetadataValidatedTypeFixtureTestClass), "");
            IEnumerable<IValidatedElement> validatedMethods
                = ((IValidatedType)validatedType).GetValidatedMethods();

            IDictionary<string, MemberInfo> validatedElementsMapping
                = CollectValidatedElements(validatedMethods);

            Assert.AreEqual(13, validatedElementsMapping.Count);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetMethod("InheritedPublicMethod"),
                           validatedElementsMapping["InheritedPublicMethod"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetMethod("PublicMethodWithoutValidators"),
                           validatedElementsMapping["PublicMethodWithoutValidators"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetMethod("PublicMethod"),
                           validatedElementsMapping["PublicMethod"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetMethod("SecondPublicMethod"),
                           validatedElementsMapping["SecondPublicMethod"]);
            Assert.AreSame(typeof(MetadataValidatedTypeFixtureTestClass).GetMethod("PublicMethodWithValidatorsForRuleset"),
                           validatedElementsMapping["PublicMethodWithValidatorsForRuleset"]);
        }

        static IDictionary<string, MockValidatorAttribute> CollectValidatorAttributes(IEnumerable<IValidatorDescriptor> validatorDescriptors)
        {
            Dictionary<string, MockValidatorAttribute> dictionary = new Dictionary<string, MockValidatorAttribute>();

            foreach (MockValidatorAttribute attribute in validatorDescriptors)
            {
                dictionary.Add(attribute.MessageTemplate, attribute);
            }

            return dictionary;
        }

        static IDictionary<string, MemberInfo> CollectValidatedElements(IEnumerable<IValidatedElement> validatedElements)
        {
            Dictionary<string, MemberInfo> dictionary = new Dictionary<string, MemberInfo>();

            foreach (IValidatedElement element in validatedElements)
            {
                dictionary.Add(element.MemberInfo.Name, element.MemberInfo);
            }

            return dictionary;
        }

        [MockValidator(false, MessageTemplate = "base type")]
        public class MetadataValidatedTypeFixtureTestClassBase
        {
            [MockValidator(false)]
            public string InheritedPublicField;

            [MockValidator(false)]
            public string InheritedPublicProperty
            {
                get { return null; }
            }

            [MockValidator(false)]
            public string InheritedPublicMethod()
            {
                return null;
            }
        }

        [MockValidator(false, MessageTemplate = "type")]
        public class MetadataValidatedTypeFixtureTestClass : MetadataValidatedTypeFixtureTestClassBase
        {
            [MockValidator(false)]
            internal string NonPublicField = null;

            [MockValidator(false)]
            public string PublicField;

            public string PublicFieldWithoutValidators;

            [MockValidator(false, Ruleset = "ruleset")]
            public string PublicFieldWithValidatorsForRuleset;

            [MockValidator(false)]
            public string SecondPublicField;

            [MockValidator(false)]
            public string this[int index]
            {
                get { return null; }
            }

            [MockValidator(false)]
            internal string NonPublicProperty
            {
                get { return null; }
            }

            [MockValidator(false)]
            public string PublicProperty
            {
                get { return null; }
            }

            public string PublicPropertyWithoutValidators
            {
                get { return null; }
            }

            [MockValidator(false, Ruleset = "ruleset")]
            public string PublicPropertyWithValidatorsForRuleset
            {
                get { return null; }
            }

            [MockValidator(false)]
            public string SecondPublicProperty
            {
                get { return null; }
            }

            [MockValidator(false)]
            public string WriteOnlyPublicProperty
            {
                set { ; }
            }

            [MockValidator(false)]
            internal string NonPublicMethod()
            {
                return null;
            }

            [MockValidator(false)]
            public string PublicMethod()
            {
                return null;
            }

            public string PublicMethodWithoutValidators()
            {
                return null;
            }

            [MockValidator(false)]
            public string PublicMethodWithParameters(string parameter)
            {
                return null;
            }

            [MockValidator(false, Ruleset = "ruleset")]
            public string PublicMethodWithValidatorsForRuleset()
            {
                return null;
            }

            [MockValidator(false)]
            public string SecondPublicMethod()
            {
                return null;
            }

            [MockValidator(false)]
            public void VoidMethod() {}
        }
    }
}
