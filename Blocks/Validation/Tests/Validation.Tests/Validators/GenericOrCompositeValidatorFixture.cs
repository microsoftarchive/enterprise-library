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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Validation.Tests.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
	[TestClass]
	public class GenericOrCompositeValidatorFixture
	{
		[TestMethod]
		public void ReturnsSuccessIfAllValidatorsReturnSuccess()
		{
			MockValidator<string> subValidator1 = new MockValidator<string>(false, "validator1");
			MockValidator<string> subValidator2 = new MockValidator<string>(false, "validator2");
			MockValidator<string> subValidator3 = new MockValidator<string>(false, "validator3");
			Validator<string> validator = new OrCompositeValidator<string>(subValidator1, subValidator2, subValidator3);
			string target = new string('a', 10);	// just any string, but a new one

			ValidationResults validationResults = validator.Validate(target);

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
			Assert.AreEqual(0, subValidator2.ValidatedTargets.Count);
			Assert.AreEqual(0, subValidator3.ValidatedTargets.Count);
		}

		[TestMethod]
		public void ReturnsSuccessAfterQueryingPreviousValidatorsIfOneValidatorReturnFailure()
		{
			MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
			MockValidator<string> subValidator2 = new MockValidator<string>(false, "validator2");
			MockValidator<string> subValidator3 = new MockValidator<string>(false, "validator3");
			Validator<string> validator = new OrCompositeValidator<string>(subValidator1, subValidator2, subValidator3);
			string target = new string('a', 10);	// just any string, but a new one

			ValidationResults validationResults = validator.Validate(target);

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
			Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
			Assert.AreEqual(0, subValidator3.ValidatedTargets.Count);
		}

		[TestMethod]
		public void ReturnsSuccessAfterQueryingPreviousValidatorsIfManyValidatorsReturnFailure()
		{
			MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
			MockValidator<string> subValidator2 = new MockValidator<string>(true, "validator2");
			MockValidator<string> subValidator3 = new MockValidator<string>(false, "validator3");
			Validator<string> validator = new OrCompositeValidator<string>(subValidator1, subValidator2, subValidator3);
			string target = new string('a', 10);	// just any string, but a new one

			ValidationResults validationResults = validator.Validate(target);

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
			Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
			Assert.AreSame(target, subValidator3.ValidatedTargets[0]);
		}

		[TestMethod]
		public void ReturnsCompositeFailureAfterQueryingAllValidatorsIfAllValidatorsReturnFailure()
		{
			MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
			MockValidator<string> subValidator2 = new MockValidator<string>(true, "validator2");
			MockValidator<string> subValidator3 = new MockValidator<string>(true, "validator3");
			Validator<string> validator = new OrCompositeValidator<string>(subValidator1, subValidator2, subValidator3);
			string target = new string('a', 10);	// just any string, but a new one

			ValidationResults validationResults = validator.Validate(target);

			Assert.IsFalse(validationResults.IsValid);
			IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
			Assert.AreEqual(3, resultsList.Count);
			Assert.AreEqual("validator1", resultsList[0].Message);
			Assert.AreEqual("validator2", resultsList[1].Message);
			Assert.AreEqual("validator3", resultsList[2].Message);
			Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
			Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
			Assert.AreSame(target, subValidator3.ValidatedTargets[0]);
		}

		[TestMethod]
		public void CanPerformValidationWithSuppliedValidationResults()
		{
			MockValidator<string> subValidator1 = new MockValidator<string>(true, "validator1");
			MockValidator<string> subValidator2 = new MockValidator<string>(true, "validator2");
			MockValidator<string> subValidator3 = new MockValidator<string>(true, "validator3");
			Validator<string> validator = new OrCompositeValidator<string>(subValidator1, subValidator2, subValidator3);
			string target = new string('a', 10);	// just any string, but a new one
			ValidationResults validationResults = new ValidationResults();

			validator.Validate(target, validationResults);

			Assert.IsFalse(validationResults.IsValid);
			IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(validationResults);
			Assert.AreEqual(3, resultsList.Count);
			Assert.AreEqual("validator1", resultsList[0].Message);
			Assert.AreEqual("validator2", resultsList[1].Message);
			Assert.AreEqual("validator3", resultsList[2].Message);
			Assert.AreSame(target, subValidator1.ValidatedTargets[0]);
			Assert.AreSame(target, subValidator2.ValidatedTargets[0]);
			Assert.AreSame(target, subValidator3.ValidatedTargets[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ValidationRequestWithNullSuppliedValidationResultsThrows()
		{
			Validator<string> validator = new OrCompositeValidator<string>();
			string target = new string('a', 10);	// just any string, but a new one

			validator.Validate(target, null);
		}
	}
}
