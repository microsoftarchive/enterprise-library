//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block QuickStart
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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Specialized;

namespace ValidationQuickStart.CustomValidators
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class DivisibleByValidator : Validator<int>
    {
        private int divisor;

        public DivisibleByValidator(NameValueCollection attributes) : base(null, null)
        {
            divisor = Int32.Parse(attributes.Get("Divisor"));
        }

        public DivisibleByValidator(int divisor) : this(divisor, null, null)
        {
        }

        public DivisibleByValidator(int divisor, string messageTemplate)
            : this(divisor, messageTemplate, null)
        {
        }

        public DivisibleByValidator(int divisor, string messageTemplate, string tag) : base(messageTemplate, tag)
        {
            this.divisor = divisor;
        }


        protected override void DoValidate(int objectToValidate, object currentTarget, string key, Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults validationResults)
        {
            if (objectToValidate % divisor != 0)
            {
                string message = string.Format(this.MessageTemplate, objectToValidate, divisor);
                this.LogValidationResult(validationResults, message, currentTarget, key);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return "The value {0} is not divisible by {1}"; }
        }
    }
}
