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
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    ///<summary>
    /// A <see cref="Validator"/> factory producing validators from rules specified in a configuration file.
    ///</summary>
    /// <seealso cref="ValidatorFactory"/>
    public class ValidationAttributeValidatorFactory : ValidatorFactory
    {
        private static readonly ValidationAttributeValidatorBuilder builder = new ValidationAttributeValidatorBuilder();
        private static readonly Validator EmptyValidator = new AndCompositeValidator();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttributeValidatorFactory"/> class.
        /// </summary>
        ///<param name="instrumentationProvider">The <see cref="IValidationInstrumentationProvider"/> provider to use for instrumentation purposes.</param>
        public ValidationAttributeValidatorFactory(IValidationInstrumentationProvider instrumentationProvider)
            : base(instrumentationProvider)
        { }

        /// <summary>
        /// Creates the validator for the specified target and ruleset.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/>to validate.</param>
        /// <param name="ruleset">The ruleset to use when validating.</param>
        /// <returns>A <see cref="Validator"/>.</returns>
        protected internal override Validator InnerCreateValidator(Type targetType, string ruleset)
        {
            if (string.IsNullOrEmpty(ruleset))
            {
                return builder.CreateValidator(targetType);
            }
            else
            {
                return EmptyValidator;
            }
        }
    }
}
