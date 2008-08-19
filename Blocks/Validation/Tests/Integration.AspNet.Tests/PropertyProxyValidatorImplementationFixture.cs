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
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;
using Microsoft.Practices.EnterpriseLibrary.Validation.Tests.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.Tests
{
	[TestClass]
	public class PropertyProxyValidatorImplementationFixture
	{
		private bool converterInvoked;
		private object converterSourceValue;
		private Type converterTargetType;
		private readonly object converterValue = new DateTime(2006, 01, 10);
		private const string ConversionErrorMessage = "conversion error messsage";

		[TestInitialize]
		public void SetUp()
		{
			MockValidator<object>.CreatedValidators.Clear();
			this.converterInvoked = false;
			this.converterSourceValue = null;
			this.converterTargetType = null;
		}

		[TestMethod]
		public void InstanceConfiguredForSuccessfulValidationReturnsValidMessageAndValidationState()
		{
			PropertyProxyValidatorImplementation implementation
				= new PropertyProxyValidatorImplementation("012345",
					typeof(ValidationSubject),
					"ValidationProperty",
					"",
					ValidationSpecificationSource.Attributes,
					null,
					ValidationSummaryDisplayMode.SingleParagraph);

			Assert.IsTrue(implementation.IsValid);
			Assert.AreEqual("", implementation.ErrorMessage);
		}

		[TestMethod]
		public void InstanceConfiguredForUnsuccessfulValidationReturnsValidMessageAndValidationState()
		{
			PropertyProxyValidatorImplementation implementation
				= new PropertyProxyValidatorImplementation("012345678901",
					typeof(ValidationSubject),
					"ValidationProperty",
					"",
					ValidationSpecificationSource.Attributes,
					null,
					ValidationSummaryDisplayMode.SingleParagraph);

			Assert.IsFalse(implementation.IsValid);
			Assert.AreNotEqual("", implementation.ErrorMessage);
		}

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InstanceConfiguredWithNullPropertyNameThrows()
        {
            PropertyProxyValidatorImplementation implementation
                = new PropertyProxyValidatorImplementation("012345678901",
                    typeof(ValidationSubject),
                    null,
                    "",
                    ValidationSpecificationSource.Attributes,
                    null,
                    ValidationSummaryDisplayMode.List);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InstanceConfiguredWithInvalidPropertyNameThrows()
        {
            PropertyProxyValidatorImplementation implementation
                = new PropertyProxyValidatorImplementation("012345678901",
                    typeof(ValidationSubject),
                    "Not a property name",
                    "",
                    ValidationSpecificationSource.Attributes,
                    null,
                    ValidationSummaryDisplayMode.List);
        }

		[TestMethod]
		public void InstanceCreatedWithNullValueConverterWillValidateTheOriginalValue()
		{
			string sourceValueToValidate = "2006-01-10T00:00:00";

			PropertyProxyValidatorImplementation implementation
				= new PropertyProxyValidatorImplementation(sourceValueToValidate,
					typeof(ValidationSubject),
					"StringValidationPropertyWithMockValidator",
					"",
					ValidationSpecificationSource.Attributes,
					null,
					ValidationSummaryDisplayMode.SingleParagraph);

			MockValidator<object> createdValidator = MockValidator<object>.CreatedValidators[0];
			Assert.IsFalse(this.converterInvoked);
			Assert.AreEqual(sourceValueToValidate, createdValidator.ValidatedTargets[0]);
		}

		[TestMethod]
		public void InstanceCreatedWithNonNullValueConverterWillInvokeConverterAndValidateTheConvertedValue()
		{
			string sourceValueToValidate = "2006-01-10T00:00:00";

			PropertyProxyValidatorImplementation implementation
				= new PropertyProxyValidatorImplementation(sourceValueToValidate,
					typeof(ValidationSubject),
					"DateTimeValidationPropertyWithMockValidator",
					"",
					ValidationSpecificationSource.Attributes,
					this.ConvertValue,
					ValidationSummaryDisplayMode.SingleParagraph);

			MockValidator<object> createdValidator = MockValidator<object>.CreatedValidators[0];
			Assert.IsTrue(this.converterInvoked);
			Assert.AreEqual(this.converterValue, createdValidator.ValidatedTargets[0]);
			Assert.AreSame(sourceValueToValidate, this.converterSourceValue);
			Assert.AreSame(typeof(DateTime), this.converterTargetType);
		}

		[TestMethod]
		public void InstanceCreatedWithNonNullValueConverterWillReturnConversionErrorIfConverterReturnsErrorWithMessage()
		{
			string sourceValueToValidate = "2006-01-10T00:00:00";

			PropertyProxyValidatorImplementation implementation
				= new PropertyProxyValidatorImplementation(sourceValueToValidate,
					typeof(ValidationSubject),
					"DateTimeValidationPropertyWithMockValidator",
					"",
					ValidationSpecificationSource.Attributes,
					this.ConvertValueWithErrorMessage,
					ValidationSummaryDisplayMode.SingleParagraph);

			Assert.IsTrue(this.converterInvoked);
			Assert.AreEqual(0, MockValidator<object>.CreatedValidators.Count);
			Assert.IsFalse(implementation.IsValid);
			Assert.IsTrue(implementation.ErrorMessage.Contains(ConversionErrorMessage));
		}

		[TestMethod]
		public void InstanceCreatedWithNonNullValueConverterWillReturnConversionErrorIfConverterThrows()
		{
			string sourceValueToValidate = "2006-01-10T00:00:00";

			PropertyProxyValidatorImplementation implementation
				= new PropertyProxyValidatorImplementation(sourceValueToValidate,
					typeof(ValidationSubject),
					"DateTimeValidationPropertyWithMockValidator",
					"",
					ValidationSpecificationSource.Attributes,
					this.ConvertValueThrowing,
					ValidationSummaryDisplayMode.SingleParagraph);

			Assert.IsTrue(this.converterInvoked);
			Assert.AreEqual(0, MockValidator<object>.CreatedValidators.Count);
			Assert.IsFalse(implementation.IsValid);
			Assert.IsTrue(implementation.ErrorMessage.Contains(Resources.ErrorConversionFailedMessage));
		}

		private bool ConvertValue(string sourceValue,
			Type targetType,
			out object convertedValue,
			out string conversionErrorMessage)
		{
			this.converterInvoked = true;
			this.converterSourceValue = sourceValue;
			this.converterTargetType = targetType;

			convertedValue = this.converterValue;
			conversionErrorMessage = null;

			return true;
		}

		private bool ConvertValueWithErrorMessage(string sourceValue,
			Type targetType,
			out object convertedValue,
			out string conversionErrorMessage)
		{
			this.converterInvoked = true;
			this.converterSourceValue = sourceValue;
			this.converterTargetType = targetType;

			convertedValue = null;
			conversionErrorMessage = ConversionErrorMessage;

			return false;
		}

		private bool ConvertValueThrowing(string sourceValue,
			Type targetType,
			out object convertedValue,
			out string conversionErrorMessage)
		{
			this.converterInvoked = true;
			this.converterSourceValue = sourceValue;
			this.converterTargetType = targetType;

			throw new Exception("ignored");
		}

		public class ValidationSubject
		{
			[StringLengthValidator(10)]
			public string ValidationProperty
			{
				get { return null; }
			}

			[MockValidator(false)]
			public string StringValidationPropertyWithMockValidator
			{
				get { return null; }
			}

			[MockValidator(false)]
			public DateTime DateTimeValidationPropertyWithMockValidator
			{
				get { return default(DateTime); }
			}
		}
	}
}
