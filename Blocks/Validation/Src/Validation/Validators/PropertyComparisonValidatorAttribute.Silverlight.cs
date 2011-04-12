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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using DAValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    partial class PropertyComparisonValidatorAttribute
    {
        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="context">The context.</param>
        protected override DAValidationResult IsValid(object value, ValidationContext context)
        {
            if (!string.IsNullOrEmpty(this.Ruleset))
            {
                return DAValidationResult.Success;
            }

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.ExceptionValidationAttributeNotSupported,
                    this.GetType().Name));
        }
    }
}
