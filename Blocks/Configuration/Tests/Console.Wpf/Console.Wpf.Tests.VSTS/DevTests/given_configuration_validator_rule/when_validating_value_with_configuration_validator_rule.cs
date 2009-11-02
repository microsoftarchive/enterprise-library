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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Console.Wpf.Validation;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Windows.Controls;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_validator_rule
{
    public abstract class given_configuration_validator_and_validator_rule : ArrangeActAssert
    {
        protected ConfigurationValidatorRule CreateRule(ConfigurationValidatorAttribute attribute)
        {
            ConfigurationValidatorRule rule = new ConfigurationValidatorRule(attribute.ValidatorInstance);
            return rule;
        }
    }

    [TestClass]
    public class when_validating_value_with_configuration_validator_rule : given_configuration_validator_and_validator_rule
    {
        [TestMethod]
        public void then_validation_rule_validates_against_converted_proposed_value()
        {
            var rule = CreateRule(new StringValidatorAttribute() { MinLength = 5 });

            Assert.AreEqual(ValidationStep.ConvertedProposedValue, rule.ValidationStep);
        }

        [TestMethod]
        public void then_validating_against_string_validator_validates_minimum()
        {
            var rule = CreateRule(new StringValidatorAttribute() { MinLength = 5 });
            var result = rule.Validate("1234", CultureInfo.InvariantCulture);
            
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void then_validating_against_required_validator_returns_invalid_for_empty_string()
        {
            var rule = new RequiredValidationRule();
            var result = rule.Validate(string.Empty, CultureInfo.InvariantCulture);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void then_validating_against_required_validator_returns_valid_for_string()
        {
            var rule = new RequiredValidationRule();
            var result = rule.Validate("1234", CultureInfo.InvariantCulture);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void then_validating_against_required_validator_returns_invalid_for_null()
        {
            var rule = new RequiredValidationRule();
            var result = rule.Validate(null, CultureInfo.InvariantCulture);

            Assert.IsFalse(result.IsValid);
        }

    }
}
