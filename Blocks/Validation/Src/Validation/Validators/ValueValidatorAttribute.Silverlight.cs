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
using DAValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    partial class ValueValidatorAttribute
    {
        protected override DAValidationResult IsValid(object value, ValidationContext context)
        {
            if (!string.IsNullOrEmpty(this.Ruleset))
            {
                return DAValidationResult.Success;
            }

            var validator = DoCreateValidator(null, null, null, null);
            var results = validator.Validate(value);

            return results.IsValid
                    ? DAValidationResult.Success
                    : new DAValidationResult(results.First().Message);
        }

        /// <summary>
        /// Applies formatting to an error message based on the data field where the error occurred. 
        /// </summary>
        /// <param name="name">The name of the data field where the error occurred.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return this.CreateValidator(null, null, null, null).GetMessage(null, name);
        }
    }
}
