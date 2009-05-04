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

using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValueAccessValidatorBuilderFixture
    {
        string value = new string('a', 10);

        [TestMethod]
        public void CanGetValidatorFromNewBuilderInstanceForAndWithoutNulls()
        {
            MockValueAccess valueAccess = new MockValueAccess(value);
            ValueAccessValidatorBuilder builder = new ValueAccessValidatorBuilder(valueAccess,
                                                                                  new MockValidatedElement(false, null, null, CompositionType.And, null, null));

            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void AddedValueValidatorIsIncludedInResultingValidatorForAndWithoutNulls()
        {
            MockValueAccess valueAccess = new MockValueAccess(value);
            MockValidator<object> failingValueValidator = new MockValidator<object>(true);
            ValueAccessValidatorBuilder builder = new ValueAccessValidatorBuilder(valueAccess,
                                                                                  new MockValidatedElement(false, null, null, CompositionType.And, null, null));

            builder.AddValueValidator(failingValueValidator);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);

            // does it validate nonnullvalues?
            ValidationResults validationResults = validator.Validate(this);
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, failingValueValidator.ValidatedTargets[0]);

            // does it ignore nulls? it should not
            valueAccess.Value = null;
            validationResults = validator.Validate(this);
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(2, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(null, failingValueValidator.ValidatedTargets[1]);
        }

        [TestMethod]
        public void MultipleAddedValueValidatorsAreIncludedInResultingValidatorForAndCompositionNotIgnoringNulls()
        {
            MockValueAccess valueAccess = new MockValueAccess(value);
            MockValidator<object> failingValueValidator = new MockValidator<object>(true);
            MockValidator<object> succeedingValueValidator = new MockValidator<object>(false);
            ValueAccessValidatorBuilder builder = new ValueAccessValidatorBuilder(valueAccess,
                                                                                  new MockValidatedElement(false, null, null, CompositionType.And, null, null));

            builder.AddValueValidator(failingValueValidator);
            builder.AddValueValidator(succeedingValueValidator);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);

            // does it validate nonnullvalues?
            ValidationResults validationResults = validator.Validate(this);
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, failingValueValidator.ValidatedTargets[0]);
            Assert.AreEqual(1, succeedingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, succeedingValueValidator.ValidatedTargets[0]);

            // does it ignore nulls? it should not
            valueAccess.Value = null;
            validationResults = validator.Validate(this);
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(2, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(null, failingValueValidator.ValidatedTargets[1]);
            Assert.AreEqual(2, succeedingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(null, succeedingValueValidator.ValidatedTargets[1]);
        }

        [TestMethod]
        public void MultipleAddedValueValidatorsAreIncludedInResultingValidatorForAndCompositionIgnoringNulls()
        {
            MockValueAccess valueAccess = new MockValueAccess(value);
            MockValidator<object> failingValueValidator = new MockValidator<object>(true);
            MockValidator<object> succeedingValueValidator = new MockValidator<object>(false);
            ValueAccessValidatorBuilder builder = new ValueAccessValidatorBuilder(valueAccess,
                                                                                  new MockValidatedElement(true, null, null, CompositionType.And, null, null));

            builder.AddValueValidator(failingValueValidator);
            builder.AddValueValidator(succeedingValueValidator);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);

            // does it validate nonnull values?
            ValidationResults validationResults = validator.Validate(this);
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, failingValueValidator.ValidatedTargets[0]);
            Assert.AreEqual(1, succeedingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, succeedingValueValidator.ValidatedTargets[0]);

            // does it ignore nulls? it should
            valueAccess.Value = null;
            validationResults = validator.Validate(this);
            Assert.IsTrue(validationResults.IsValid); // null value is ignored so validation succeeds
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count); // no changes
            Assert.AreEqual(1, succeedingValueValidator.ValidatedTargets.Count); // no changes
        }

        [TestMethod]
        public void MultipleAddedValueValidatorsAreIncludedInResultingValidatorForOrCompositionIgnoringNulls()
        {
            MockValueAccess valueAccess = new MockValueAccess(value);
            MockValidator<object> failingValueValidator = new MockValidator<object>(true);
            MockValidator<object> succeedingValueValidator = new MockValidator<object>(false);
            ValueAccessValidatorBuilder builder = new ValueAccessValidatorBuilder(valueAccess,
                                                                                  new MockValidatedElement(true, null, null, CompositionType.Or, null, null));

            builder.AddValueValidator(failingValueValidator);
            builder.AddValueValidator(succeedingValueValidator);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);

            // does it validate nonnullvalues?
            ValidationResults validationResults = validator.Validate(this);
            Assert.IsTrue(validationResults.IsValid); // it's an OR validator
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, failingValueValidator.ValidatedTargets[0]);
            Assert.AreEqual(1, succeedingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, succeedingValueValidator.ValidatedTargets[0]);

            // does it ignore nulls? it should
            valueAccess.Value = null;
            validationResults = validator.Validate(this);
            Assert.IsTrue(validationResults.IsValid); // null value is ignored so validation succeeds
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count); // no changes
            Assert.AreEqual(1, succeedingValueValidator.ValidatedTargets.Count); // no changes
        }

        [TestMethod]
        public void MultipleAddedValueValidatorsAreIncludedInResultingValidatorForOrCompositionNotIgnoringNulls()
        {
            MockValueAccess valueAccess = new MockValueAccess(value);
            MockValidator<object> failingValueValidator = new MockValidator<object>(true);
            MockValidator<object> succeedingValueValidator = new MockValidator<object>(false);
            ValueAccessValidatorBuilder builder = new ValueAccessValidatorBuilder(valueAccess,
                                                                                  new MockValidatedElement(false, null, null, CompositionType.Or, null, null));

            builder.AddValueValidator(failingValueValidator);
            builder.AddValueValidator(succeedingValueValidator);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);

            // does it validate nonnullvalues?
            ValidationResults validationResults = validator.Validate(this);
            Assert.IsTrue(validationResults.IsValid); // it's an OR validator
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, failingValueValidator.ValidatedTargets[0]);
            Assert.AreEqual(1, succeedingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, succeedingValueValidator.ValidatedTargets[0]);

            // does it ignore nulls? it should not
            valueAccess.Value = null;
            validationResults = validator.Validate(this);
            Assert.IsTrue(validationResults.IsValid); // it's an OR validator
            Assert.AreEqual(2, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(null, failingValueValidator.ValidatedTargets[1]);
            Assert.AreEqual(2, succeedingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(null, succeedingValueValidator.ValidatedTargets[1]);
        }
    }
}
