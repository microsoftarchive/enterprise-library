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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class GivenValidatorCreatedByValidationAttributeValidatorBuilderForTypeWithValidationAttributes
    {
        private Validator validator;

        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new ConfigurationValidatorFactory(new SystemConfigurationSource(false)));
            this.validator =
                new ValidationAttributeValidatorBuilder(
                    new MemberAccessValidatorBuilderFactory(),
                    ValidationFactory.DefaultCompositeValidatorFactory)
                    .CreateValidator(typeof(TypeWithValidationAttributes));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void WhenValidatingValidTarget_ThenGetsValidResult()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 6, MyField = "aaaaaa" };

            var results = this.validator.Validate(instance);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void WhenValidatingTargetWithInvalidField_ThenGetsNotValidResultWithResultsForEachFailedRule()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 6, MyField = "some invalid string" };

            var results = this.validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "regex"));
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "length"));
        }

        [TestMethod]
        public void WhenValidatingTargetWithInvalidProperty_ThenGetsNotValidResultWithResultsForEachFailedRule()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 0, MyField = "aaaaa" };

            var results = this.validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Where(vr => vr.Key == "MyProperty").Any(vr => vr.Message == "range"));
        }

        [TestMethod]
        public void WhenValidatingTargetWithInvalidFieldAndProperty_ThenGetsNotValidResultWithResultsForEachFailedRule()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 0, MyField = "some invalid string" };

            var results = this.validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(3, results.Count);
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "regex"));
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "length"));
            Assert.IsTrue(results.Where(vr => vr.Key == "MyProperty").Any(vr => vr.Message == "range"));
        }

        public class TypeWithValidationAttributes
        {
            [Range(4, 10, ErrorMessage = "range")]
            public int MyProperty { get; set; }

            [RegularExpression("a*", ErrorMessage = "regex")]
            [StringLength(10, ErrorMessage = "length")]
            public string MyField;
        }
    }

    [TestClass]
    public class GivenValidatorCreatedByValidationAttributeValidatorBuilderForTypeWithValidationAttributesOnMetadataType
    {
        private Validator validator;

        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new ConfigurationValidatorFactory(new SystemConfigurationSource(false)));
            this.validator =
                new ValidationAttributeValidatorBuilder(
                    new MemberAccessValidatorBuilderFactory(),
                    ValidationFactory.DefaultCompositeValidatorFactory)
                    .CreateValidator(typeof(TypeWithValidationAttributes));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void WhenValidatingValidTarget_ThenGetsValidResult()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 6, MyField = "aaaaaa" };

            var results = this.validator.Validate(instance);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void WhenValidatingTargetWithInvalidField_ThenGetsNotValidResultWithResultsForEachFailedRule()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 6, MyField = "some invalid string" };

            var results = this.validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "regex"));
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "length"));
        }

        [TestMethod]
        public void WhenValidatingTargetWithInvalidProperty_ThenGetsNotValidResultWithResultsForEachFailedRule()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 0, MyField = "aaaaa" };

            var results = this.validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Where(vr => vr.Key == "MyProperty").Any(vr => vr.Message == "range"));
        }

        [TestMethod]
        public void WhenValidatingTargetWithInvalidFieldAndProperty_ThenGetsNotValidResultWithResultsForEachFailedRule()
        {
            var instance = new TypeWithValidationAttributes { MyProperty = 0, MyField = "some invalid string" };

            var results = this.validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(3, results.Count);
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "regex"));
            Assert.IsTrue(results.Where(vr => vr.Key == "MyField").Any(vr => vr.Message == "length"));
            Assert.IsTrue(results.Where(vr => vr.Key == "MyProperty").Any(vr => vr.Message == "range"));
        }

        [MetadataType(typeof(TypeWithValidationAttributesMetadata))]
        public class TypeWithValidationAttributes
        {
            public class TypeWithValidationAttributesMetadata
            {
                [Range(4, 10, ErrorMessage = "range")]
                public int MyProperty { get; set; }

                [RegularExpression("a*", ErrorMessage = "regex")]
                [StringLength(10, ErrorMessage = "length")]
                public string MyField;
            }

            public int MyProperty { get; set; }

            public string MyField;
        }
    }
}
