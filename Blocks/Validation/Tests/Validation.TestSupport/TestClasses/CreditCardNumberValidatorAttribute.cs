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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CreditCardNumberValidatorAttribute : ValueValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new CreditCardNumberValidator();
        }
    }
}
