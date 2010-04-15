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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class CompositeValidatorBuilderFixture
    {
        string value = new string('a', 10);

        [TestMethod]
        public void CanGetValidatorFromNewBuilderInstanceForAndWithoutNulls()
        {
            CompositeValidatorBuilder builder
                = new CompositeValidatorBuilder(new MockValidatedElement(false, null, null,
                                                                         CompositionType.And, null, null));

            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void AddedValueValidatorIsIncludedInResultingValidatorForAndWithoutNulls()
        {
            MockValidator<object> failingValueValidator = new MockValidator<object>(true);
            CompositeValidatorBuilder builder
                = new CompositeValidatorBuilder(new MockValidatedElement(false, null, null,
                                                                         CompositionType.And, null, null));

            builder.AddValueValidator(failingValueValidator);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);

            // does it validate nonnullvalues?
            ValidationResults validationResults = validator.Validate(value);
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(value, failingValueValidator.ValidatedTargets[0]);

            // does it ignore nulls? it should not
            validationResults = validator.Validate(null);
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(2, failingValueValidator.ValidatedTargets.Count);
            Assert.AreSame(null, failingValueValidator.ValidatedTargets[1]);
        }

        [TestMethod]
        public void NoIgnoreNullsAndAndCompositionCreatesSingleAndValidator()
        {
            MockValidator<object> valueValidator1 = new MockValidator<object>(false);
            MockValidator<object> valueValidator2 = new MockValidator<object>(false);
            CompositeValidatorBuilder builder
                = new CompositeValidatorBuilder(new MockValidatedElement(false, null, null,
                                                                         CompositionType.And, null, null));

            builder.AddValueValidator(valueValidator1);
            builder.AddValueValidator(valueValidator2);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(AndCompositeValidator), validator.GetType());
            Assert.AreEqual(null, validator.Tag);
            IEnumerator<Validator> valueValidatorsEnumerator = ((AndCompositeValidator)validator).Validators.GetEnumerator();
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator1, valueValidatorsEnumerator.Current);
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator2, valueValidatorsEnumerator.Current);
            Assert.IsFalse(valueValidatorsEnumerator.MoveNext());
        }

        [TestMethod]
        public void NoIgnoreNullsAndOrCompositionCreatesSingleOrValidatorWithSuppliedMessageTemplate()
        {
            MockValidator<object> valueValidator1 = new MockValidator<object>(false);
            MockValidator<object> valueValidator2 = new MockValidator<object>(false);
            CompositeValidatorBuilder builder
                = new CompositeValidatorBuilder(new MockValidatedElement(false, null, null,
                                                                         CompositionType.Or, "composition template", null));

            builder.AddValueValidator(valueValidator1);
            builder.AddValueValidator(valueValidator2);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(OrCompositeValidator), validator.GetType());
            Assert.AreEqual("composition template", validator.MessageTemplate);
            Assert.AreEqual(null, validator.Tag);
            IEnumerator<Validator> valueValidatorsEnumerator = ((OrCompositeValidator)validator).Validators.GetEnumerator();
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator1, valueValidatorsEnumerator.Current);
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator2, valueValidatorsEnumerator.Current);
            Assert.IsFalse(valueValidatorsEnumerator.MoveNext());
        }

        [TestMethod]
        public void IgnoreNullsAndAndCompositionCreatesIgnoreNullsWrapperAndWrappedAndValidator()
        {
            MockValidator<object> valueValidator1 = new MockValidator<object>(false);
            MockValidator<object> valueValidator2 = new MockValidator<object>(false);
            CompositeValidatorBuilder builder
                = new CompositeValidatorBuilder(new MockValidatedElement(true, "ignore nulls", "ignore nulls tag",
                                                                         CompositionType.And, null, null));

            builder.AddValueValidator(valueValidator1);
            builder.AddValueValidator(valueValidator2);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NullIgnoringValidatorWrapper), validator.GetType());
            Assert.AreSame(typeof(AndCompositeValidator), ((NullIgnoringValidatorWrapper)validator).WrappedValidator.GetType());
            IEnumerator<Validator> valueValidatorsEnumerator = ((AndCompositeValidator)((NullIgnoringValidatorWrapper)validator).WrappedValidator).Validators.GetEnumerator();
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator1, valueValidatorsEnumerator.Current);
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator2, valueValidatorsEnumerator.Current);
            Assert.IsFalse(valueValidatorsEnumerator.MoveNext());
        }

        [TestMethod]
        public void IgnoreNullsAndOrCompositionCreatesIgnoreNullsWrapperAndWrappedOrValidatorWithSuppliedMessageTemplate()
        {
            MockValidator<object> valueValidator1 = new MockValidator<object>(false);
            MockValidator<object> valueValidator2 = new MockValidator<object>(false);
            CompositeValidatorBuilder builder
                = new CompositeValidatorBuilder(new MockValidatedElement(true, "ignore nulls", null,
                                                                         CompositionType.Or, "composition template", "composition tag"));

            builder.AddValueValidator(valueValidator1);
            builder.AddValueValidator(valueValidator2);
            Validator validator = builder.GetValidator();

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NullIgnoringValidatorWrapper), validator.GetType());
            Assert.AreSame(typeof(OrCompositeValidator), ((NullIgnoringValidatorWrapper)validator).WrappedValidator.GetType());
            Assert.AreEqual("composition template", ((NullIgnoringValidatorWrapper)validator).WrappedValidator.MessageTemplate);
            Assert.AreEqual("composition tag", ((NullIgnoringValidatorWrapper)validator).WrappedValidator.Tag);
            IEnumerator<Validator> valueValidatorsEnumerator = ((OrCompositeValidator)((NullIgnoringValidatorWrapper)validator).WrappedValidator).Validators.GetEnumerator();
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator1, valueValidatorsEnumerator.Current);
            Assert.IsTrue(valueValidatorsEnumerator.MoveNext());
            Assert.AreSame(valueValidator2, valueValidatorsEnumerator.Current);
            Assert.IsFalse(valueValidatorsEnumerator.MoveNext());
        }
    }
}
