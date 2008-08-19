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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class TypeConversionValidatorAttributeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionOfAttributeWithNullPatternThrows()
        {
            new TypeConversionValidatorAttribute(null);
        }

        [TestMethod]
        public void AttributeWithTargetTypeCreatesValidator()
        {
            ValidatorAttribute attribute = new TypeConversionValidatorAttribute(typeof(double));

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null);
            Assert.IsNotNull(validator);

            TypeConversionValidator typedValidator = validator as TypeConversionValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(false, typedValidator.Negated);
            Assert.AreEqual(typeof(double), typedValidator.TargetType);
        }

        [TestMethod]
        public void AttributeWithTargetTypeAndNegatedCreatesValidator()
        {
            ValueValidatorAttribute attribute = new TypeConversionValidatorAttribute(typeof(double));
            attribute.Negated = true;

            Validator validator = ((IValidatorDescriptor)attribute).CreateValidator(null, null, null);
            Assert.IsNotNull(validator);

            TypeConversionValidator typedValidator = validator as TypeConversionValidator;
            Assert.IsNotNull(typedValidator);

            Assert.AreEqual(true, typedValidator.Negated);
            Assert.AreEqual(typeof(double), typedValidator.TargetType);
        }
    }
}