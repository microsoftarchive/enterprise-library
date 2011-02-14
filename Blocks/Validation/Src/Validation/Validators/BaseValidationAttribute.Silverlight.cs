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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    partial class BaseValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the specified validation object on which the 
        /// <see cref="System.ComponentModel.DataAnnotations.ValidationAttribute "/> is declared.</param>
        /// <param name="context"></param>
        /// <returns><see langword="true"/> if the specified value is valid; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="NotSupportedException">when invoked on an attribute with a non-null ruleset.</exception>
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext context)
        {
            if (!string.IsNullOrEmpty(this.Ruleset))
            {
                return System.ComponentModel.DataAnnotations.ValidationResult.Success;
            }

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.ExceptionValidationAttributeNotSupported,
                    this.GetType().Name));
        }
    }
}
