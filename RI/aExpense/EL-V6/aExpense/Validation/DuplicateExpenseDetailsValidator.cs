#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace AExpense.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class DuplicateExpenseDetailsValidator : ValueValidator<ICollection<Model.ExpenseItem>>
    {
        public DuplicateExpenseDetailsValidator(NameValueCollection attributes)
            : base(null, null, false)
        {
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return Properties.Resources.DuplicateExpenseDetailsValidation; }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            this.DoValidate(objectToValidate as ICollection<Model.ExpenseItem>, currentTarget, key, validationResults);
        }

        protected override void DoValidate(ICollection<Model.ExpenseItem> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
                if (objectToValidate.GroupBy(i => i.Description.ToLowerInvariant()).Any(g => g.Count() > 1))
                {
                    LogValidationResult(validationResults, GetMessage(objectToValidate, key), currentTarget, key);
                }
            }
            else
            {
                LogValidationResult(validationResults, GetMessage(objectToValidate, key), currentTarget, key);
            }
        }        
    }
}