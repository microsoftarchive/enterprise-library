// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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