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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	[ConfigurationElementType(typeof(MockValidatorData))]
	public class MockValidator<T> : Validator<T>
	{
		public const string DefaultMockValidatorMessageTemplate = "default mock validator message template";

		public static List<MockValidator<T>> CreatedValidators = new List<MockValidator<T>>();

		private bool returnFailure;
		private Exception exception;
		private List<T> validatedTargets;

		public MockValidator(bool returnFailure)
			: this(returnFailure, null)
		{ }

		public MockValidator(bool returnFailure, string messageTemplate)
			: base(messageTemplate, null)
		{
			this.returnFailure = returnFailure;
			this.validatedTargets = new List<T>();

			CreatedValidators.Add(this);
		}

		public MockValidator(Exception exception)
			: base(string.Empty, null)
		{
			this.exception = exception;
		}

		public static void ResetCaches()
		{
			CreatedValidators.Clear();
			ValidationFactory.ResetCaches();
			PropertyValidationFactory.ResetCaches();
		}

		protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (exception != null)
			{
				throw exception;
			}
			this.validatedTargets.Add(objectToValidate);

			if (returnFailure)
			{
				string message = this.MessageTemplate;
				LogValidationResult(validationResults, message, currentTarget, key);
			}
		}

		public List<T> ValidatedTargets
		{
			get { return validatedTargets; }
		}

		public new string MessageTemplate
		{
			get { return base.MessageTemplate; }
		}

		protected override string DefaultMessageTemplate
		{
			get { return DefaultMockValidatorMessageTemplate; }
		}

		public bool ReturnFailure
		{
			get { return this.returnFailure; }
			set { this.returnFailure = true; }
		}
	}
}
