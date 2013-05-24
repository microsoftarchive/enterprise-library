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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.PolicyInjection
{
    [TestClass]
    public class ValidationCallHandlerAttributeFixture
    {
        private IUnityContainer container;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            var configurationSource = new DictionaryConfigurationSource();
        }

        [TestMethod]
        public void ShouldCreateDefaultHandlerFromAttribute()
        {
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute();
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreEqual(string.Empty, handler.RuleSet);
            Assert.AreSame(ValidationFactory.DefaultCompositeValidatorFactory, handler.ValidatorFactory);
        }

        [TestMethod]
        public void ShouldSetRulesetFromAttribute()
        {
            string ruleset = "Some Ruleset";
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute(ruleset);
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(ruleset, handler.RuleSet);
            Assert.IsTrue(handler.ValidatorFactory is CompositeValidatorFactory);
        }

        [TestMethod]
        public void ShouldUseConfigurationValidatorFactoryIfSpecified()
        {
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute();
            attribute.SpecificationSource = SpecificationSource.Configuration;
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(string.Empty, handler.RuleSet);
            Assert.AreSame(ValidationFactory.DefaultConfigurationValidatorFactory, handler.ValidatorFactory);
        }

        [TestMethod]
        public void ShouldUseAttributeValidatorFactoryIfSpecified()
        {
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute();
            attribute.SpecificationSource = SpecificationSource.Attributes;
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(string.Empty, handler.RuleSet);
            Assert.IsTrue(handler.ValidatorFactory is AttributeValidatorFactory);
        }

        [TestMethod]
        public void ShouldUseNoValidatorFactoryIfSpecified()
        {
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute();
            attribute.SpecificationSource = SpecificationSource.ParameterAttributesOnly;
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(string.Empty, handler.RuleSet);
            Assert.IsNull(handler.ValidatorFactory);
        }

        ValidationCallHandler GetHandlerFromAttribute(ValidationCallHandlerAttribute attribute)
        {
            return (ValidationCallHandler)attribute.CreateHandler(this.container);
        }
    }
}
