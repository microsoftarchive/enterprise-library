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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    public static class ValidationAttributeExtensions
    {
        public static bool IsValid(this ValidationAttribute attribute, object value)
        {
            var result = attribute.GetValidationResult(value, new ValidationContext(new object(), null, null));
            return result == null || string.IsNullOrEmpty(result.ErrorMessage);
        }
    }
}
