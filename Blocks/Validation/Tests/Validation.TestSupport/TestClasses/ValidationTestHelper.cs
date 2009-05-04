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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public static class ValidationTestHelper
	{
		public static IList<ValidationResult> GetResultsList(IEnumerable<ValidationResult> validationResults)
		{
			List<ValidationResult> resultsList = new List<ValidationResult>();
			resultsList.AddRange(validationResults);

			return resultsList;
		}

		public static IDictionary<string, ValidationResult> GetResultsMapping(IEnumerable<ValidationResult> validationResults)
		{
			Dictionary<string, ValidationResult> resultsMapping = new Dictionary<string, ValidationResult>();
			foreach (ValidationResult validationResult in validationResults)
			{
				resultsMapping.Add(validationResult.Message, validationResult);
			}

			return resultsMapping;
		}

		public static List<T> CreateListFromEnumerable<T>(IEnumerable<T> enumerable)
		{
			List<T> result = new List<T>();

			foreach (T element in enumerable)
			{
				result.Add(element);
			}

			return result;
		}

		public static IDictionary<string, MockValidator<object>> CreateMockValidatorsMapping(IEnumerable<Validator> validators)
		{
			Dictionary<string, MockValidator<object>> result = new Dictionary<string, MockValidator<object>>();

			foreach (MockValidator<object> validator in validators)
			{
				result.Add(validator.MessageTemplate, validator);
			}

			return result;
		}
	}
}
