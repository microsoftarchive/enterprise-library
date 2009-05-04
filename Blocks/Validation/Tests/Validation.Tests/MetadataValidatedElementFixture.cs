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
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class MetadataValidatedElementFixture
    {
        [TestMethod]
        public void NewInstanceIsClean()
        {
            MetadataValidatedElement validatedElement = new MetadataValidatedElement("ruleset");

            Assert.IsNull(((IValidatedElement)validatedElement).TargetType);
            Assert.IsNull(((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IDictionary<string, MockValidatorAttribute> attributes
                = CollectValidatorAttributes(((IValidatedElement)validatedElement).GetValidatorDescriptors());
            Assert.AreEqual(0, attributes.Count);
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedPropertyReferenceReturnsCorrectValues()
        {
            MetadataValidatedElement validatedElement = new MetadataValidatedElement("");
            PropertyInfo propertyInfo = typeof(MetadataValidatedElementFixtureTestClass).GetProperty("Property");

            validatedElement.UpdateFlyweight(propertyInfo);

            Assert.AreSame(typeof(string), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(propertyInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IDictionary<string, MockValidatorAttribute> attributes
                = CollectValidatorAttributes(((IValidatedElement)validatedElement).GetValidatorDescriptors());
            Assert.AreEqual(2, attributes.Count);
            Assert.IsTrue(attributes.ContainsKey("property validator 1 message"));
            Assert.IsTrue(attributes.ContainsKey("property validator 2 message"));
        }

        [TestMethod]
        public void FlyweightWithRulesetUpdatedWithValidatedPropertyReferenceReturnsCorrectValues()
        {
            MetadataValidatedElement validatedElement = new MetadataValidatedElement("ruleset");
            PropertyInfo propertyInfo = typeof(MetadataValidatedElementFixtureTestClass).GetProperty("Property");

            validatedElement.UpdateFlyweight(propertyInfo);

            Assert.AreSame(typeof(string), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(propertyInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IDictionary<string, MockValidatorAttribute> attributes
                = CollectValidatorAttributes(((IValidatedElement)validatedElement).GetValidatorDescriptors());
            Assert.AreEqual(3, attributes.Count);
            Assert.IsTrue(attributes.ContainsKey("property validator 1 message-ruleset"));
            Assert.IsTrue(attributes.ContainsKey("property validator 2 message-ruleset"));
            Assert.IsTrue(attributes.ContainsKey("property validator 2 message-ruleset"));
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedPropertyReferenceWithValidationOverridesReturnsOverridenValues()
        {
            MetadataValidatedElement validatedElement = new MetadataValidatedElement("");
            PropertyInfo propertyInfo = typeof(MetadataValidatedElementFixtureTestClass).GetProperty("PropertyWithValidationOverrides");

            validatedElement.UpdateFlyweight(propertyInfo);

            Assert.AreSame(typeof(string), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(propertyInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.Or, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(true, ((IValidatedElement)validatedElement).IgnoreNulls);

            IDictionary<string, MockValidatorAttribute> attributes
                = CollectValidatorAttributes(((IValidatedElement)validatedElement).GetValidatorDescriptors());
            Assert.AreEqual(1, attributes.Count);
            Assert.IsTrue(attributes.ContainsKey("property validator 1 message"));
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedFieldReferenceReturnsCorrectValues()
        {
            MetadataValidatedElement validatedElement = new MetadataValidatedElement("");
            FieldInfo fieldInfo = typeof(MetadataValidatedElementFixtureTestClass).GetField("Field");

            validatedElement.UpdateFlyweight(fieldInfo);

            Assert.AreSame(typeof(int), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(fieldInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IDictionary<string, MockValidatorAttribute> attributes
                = CollectValidatorAttributes(((IValidatedElement)validatedElement).GetValidatorDescriptors());
            Assert.AreEqual(3, attributes.Count);
            Assert.IsTrue(attributes.ContainsKey("field validator 1 message"));
            Assert.IsTrue(attributes.ContainsKey("field validator 2 message"));
            Assert.IsTrue(attributes.ContainsKey("field validator 3 message"));
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedMethodReferenceReturnsCorrectValues()
        {
            MetadataValidatedElement validatedElement = new MetadataValidatedElement("");
            MethodInfo methodInfo = typeof(MetadataValidatedElementFixtureTestClass).GetMethod("Method");

            validatedElement.UpdateFlyweight(methodInfo);

            Assert.AreSame(typeof(DateTime), ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(methodInfo, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IDictionary<string, MockValidatorAttribute> attributes
                = CollectValidatorAttributes(((IValidatedElement)validatedElement).GetValidatorDescriptors());
            Assert.AreEqual(4, attributes.Count);
            Assert.IsTrue(attributes.ContainsKey("method validator 1 message"));
            Assert.IsTrue(attributes.ContainsKey("method validator 2 message"));
            Assert.IsTrue(attributes.ContainsKey("method validator 3 message"));
            Assert.IsTrue(attributes.ContainsKey("method validator 4 message"));
        }

        [TestMethod]
        public void FlyweightUpdatedWithValidatedTypeReturnsCorrectValues()
        {
            MetadataValidatedElement validatedElement = new MetadataValidatedElement("");
            Type type = typeof(MetadataValidatedElementFixtureTestClass);

            validatedElement.UpdateFlyweight(type);

            Assert.AreSame(type, ((IValidatedElement)validatedElement).TargetType);
            Assert.AreSame(type, ((IValidatedElement)validatedElement).MemberInfo);
            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElement).CompositionType);
            Assert.AreEqual(false, ((IValidatedElement)validatedElement).IgnoreNulls);

            IDictionary<string, MockValidatorAttribute> attributes
                = CollectValidatorAttributes(((IValidatedElement)validatedElement).GetValidatorDescriptors());
            Assert.AreEqual(1, attributes.Count);
            Assert.IsTrue(attributes.ContainsKey("type validator 1 message"));
        }

        [TestMethod]
        public void IgnoreNullsTakesRulesetInConsideration()
        {
            MetadataValidatedElement validatedElementForDefaultRuleset = new MetadataValidatedElement("");
            MetadataValidatedElement validatedElementForRuleset = new MetadataValidatedElement("ruleset");
            MetadataValidatedElement validatedElementForRuleset2 = new MetadataValidatedElement("ruleset2");
            MethodInfo methodInfo = typeof(MetadataValidatedElementFixtureTestClass).GetMethod("MethodWithRuleset");

            validatedElementForDefaultRuleset.UpdateFlyweight(methodInfo);
            validatedElementForRuleset.UpdateFlyweight(methodInfo);
            validatedElementForRuleset2.UpdateFlyweight(methodInfo);

            Assert.AreEqual(false, ((IValidatedElement)validatedElementForDefaultRuleset).IgnoreNulls);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForDefaultRuleset).IgnoreNullsMessageTemplate);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForDefaultRuleset).IgnoreNullsTag);
            Assert.AreEqual(true, ((IValidatedElement)validatedElementForRuleset).IgnoreNulls);
            Assert.AreEqual("message", ((IValidatedElement)validatedElementForRuleset).IgnoreNullsMessageTemplate);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForRuleset).IgnoreNullsTag);
            Assert.AreEqual(true, ((IValidatedElement)validatedElementForRuleset2).IgnoreNulls);
            Assert.AreEqual(Resources.TestMessageTemplate, ((IValidatedElement)validatedElementForRuleset2).IgnoreNullsMessageTemplate);
            Assert.AreEqual("ignore nulls tag", ((IValidatedElement)validatedElementForRuleset2).IgnoreNullsTag);
        }

        [TestMethod]
        public void CompositionTypeTakesRulesetInConsideration()
        {
            MetadataValidatedElement validatedElementForDefaultRuleset = new MetadataValidatedElement("");
            MetadataValidatedElement validatedElementForRulesetWithAnd = new MetadataValidatedElement("ruleset with and");
            MetadataValidatedElement validatedElementForRulesetWithOr = new MetadataValidatedElement("ruleset with or");
            MetadataValidatedElement validatedElementForRulesetWithout = new MetadataValidatedElement("ruleset without");
            MethodInfo methodInfo = typeof(MetadataValidatedElementFixtureTestClass).GetMethod("MethodWithRuleset");

            validatedElementForDefaultRuleset.UpdateFlyweight(methodInfo);
            validatedElementForRulesetWithAnd.UpdateFlyweight(methodInfo);
            validatedElementForRulesetWithOr.UpdateFlyweight(methodInfo);
            validatedElementForRulesetWithout.UpdateFlyweight(methodInfo);

            Assert.AreEqual(CompositionType.Or, ((IValidatedElement)validatedElementForDefaultRuleset).CompositionType);
            Assert.AreEqual("message or", ((IValidatedElement)validatedElementForDefaultRuleset).CompositionMessageTemplate);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForDefaultRuleset).CompositionTag);

            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElementForRulesetWithAnd).CompositionType);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForRulesetWithAnd).CompositionMessageTemplate);
            Assert.AreEqual("composition tag", ((IValidatedElement)validatedElementForRulesetWithAnd).CompositionTag);

            Assert.AreEqual(CompositionType.Or, ((IValidatedElement)validatedElementForRulesetWithOr).CompositionType);
            Assert.AreEqual(Resources.TestMessageTemplate, ((IValidatedElement)validatedElementForRulesetWithOr).CompositionMessageTemplate);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForRulesetWithOr).CompositionTag);

            Assert.AreEqual(CompositionType.And, ((IValidatedElement)validatedElementForRulesetWithout).CompositionType);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForRulesetWithout).CompositionMessageTemplate);
            Assert.AreEqual(null, ((IValidatedElement)validatedElementForRulesetWithout).CompositionTag);
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

        [MockValidator(false, MessageTemplate = "type validator 1 message")]
        public class MetadataValidatedElementFixtureTestClass
        {
            [MockValidator(false, MessageTemplate = "field validator 1 message")]
            [MockValidator(false, MessageTemplate = "field validator 2 message")]
            [MockValidator(false, MessageTemplate = "field validator 3 message")]
            public int Field;

            [MockValidator(false, MessageTemplate = "property validator 1 message")]
            [MockValidator(false, MessageTemplate = "property validator 2 message")]
            [MockValidator(false, MessageTemplate = "property validator 1 message-ruleset", Ruleset = "ruleset")]
            [MockValidator(false, MessageTemplate = "property validator 2 message-ruleset", Ruleset = "ruleset")]
            [MockValidator(false, MessageTemplate = "property validator 3 message-ruleset", Ruleset = "ruleset")]
            public string Property
            {
                get { return null; }
            }

            [MockValidator(false, MessageTemplate = "property validator 1 message")]
            [IgnoreNulls]
            [ValidatorComposition(CompositionType.Or)]
            public string PropertyWithValidationOverrides
            {
                get { return null; }
            }

            [MockValidator(false, MessageTemplate = "method validator 1 message")]
            [MockValidator(false, MessageTemplate = "method validator 2 message")]
            [MockValidator(false, MessageTemplate = "method validator 3 message")]
            [MockValidator(false, MessageTemplate = "method validator 4 message")]
            public DateTime Method()
            {
                return default(DateTime);
            }

            [ValidatorComposition(CompositionType.Or, MessageTemplate="message or")]
            [ValidatorComposition(CompositionType.And, Ruleset = "ruleset with and", Tag = "composition tag")]
            [ValidatorComposition(CompositionType.Or, MessageTemplateResourceType = typeof(Resources),
                MessageTemplateResourceName = "TestMessageTemplate", Ruleset = "ruleset with or")]
            [IgnoreNulls(Ruleset = "ruleset", MessageTemplate = "message")]
            [IgnoreNulls(Ruleset = "ruleset2", MessageTemplateResourceType=typeof(Resources),
                MessageTemplateResourceName="TestMessageTemplate", Tag = "ignore nulls tag")]
            public DateTime MethodWithRuleset()
            {
                return default(DateTime);
            }
        }
    }
}
