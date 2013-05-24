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