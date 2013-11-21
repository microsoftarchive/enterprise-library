// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using AExpense.DataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.ServiceLocation;

namespace AExpense.Validation
{
    public class ApproverValidator : ValueValidator<string>
    {
        public ApproverValidator()
            : base(null, null, false)
        { }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (!string.IsNullOrWhiteSpace(objectToValidate))
            {
                var user = ServiceLocator.Current.GetInstance<IUserRepository>().GetUser(objectToValidate, throwOnError: false);
                if (user == null || !user.Roles.Contains<string>("manager", StringComparer.OrdinalIgnoreCase))
                {
                    LogValidationResult(validationResults, GetMessage(objectToValidate, key), currentTarget, key);
                }
            }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return Properties.Resources.ApproverNotAuthorized; }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ApproverValidatorAttribute : ValueValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new ApproverValidator();
        }
    }
}