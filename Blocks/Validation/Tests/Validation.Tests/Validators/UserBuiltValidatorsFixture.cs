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

using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    /// <summary>
    /// Summary description for UserBuiltValidatorsFixture
    /// </summary>
    [TestClass]
    public class UserBuiltValidatorsFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void CustomValidatorAttributesWorkWithoutRulesets_bug4683()
        {
            var user = new UserWithValidatedCreditCard()
            {
                Name = "Chris",
                CreditCardNumber = "Not a valid credit card"
            };


            var validator = ValidationFactory.CreateValidatorFromAttributes<UserWithValidatedCreditCard>();


            var validationResults = validator.Validate(user);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void CustomValidatorAttributesWorkWithoutRulesetsWhenUsingViaDataAnnotations_bug4683()
        {
            var user = new UserWithValidatedCreditCard()
            {
                Name = "Chris",
                CreditCardNumber = "Not a valid credit card"
            };

            var validator = GetValidationAttributeOnProperty<UserWithValidatedCreditCard>("CreditCardNumber");
            var result = validator.IsValid(user.CreditCardNumber);

            Assert.IsFalse(result);
        }

        private static ValidationAttribute GetValidationAttributeOnProperty<T>(string propertyName)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            Assert.IsNotNull(propInfo);

            var attributes =
                propInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>();
            return attributes.First();
        }
    }
}
