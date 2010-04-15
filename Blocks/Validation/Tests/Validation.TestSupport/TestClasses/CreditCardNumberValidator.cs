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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
    public class CreditCardNumberValidator : RegexValidator
    {
        private const string CreditCardRegex = @"^\d{4}([-\s]?\d{4}){3}$";

        public CreditCardNumberValidator()
            : base(CreditCardRegex)
        {
            
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get
            {
                return "Invalid credit card number";
            }
        }
    }
}
