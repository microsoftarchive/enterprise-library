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
using System.Configuration;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Represents a <see cref="PropertyComparisonValidator"/>.
    /// </summary>
    public partial class PropertyComparisonValidatorData : ValueValidatorData
    {
        /// <summary>
        /// Creates the <see cref="PropertyComparisonValidator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <param name="ownerType">The type of the object from which the value to validate is extracted.</param>
        /// <param name="memberValueAccessBuilder">The <see cref="MemberValueAccessBuilder"/> to use for validators that
        /// require access to properties.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <returns>The created <see cref="PropertyComparisonValidator"/>.</returns>	
        protected override Validator DoCreateValidator(
            Type targetType,
            Type ownerType,
            MemberValueAccessBuilder memberValueAccessBuilder,
            ValidatorFactory validatorFactory)
        {
            if (string.IsNullOrEmpty(this.PropertyToCompare))
            {
                throw new ConfigurationErrorsException(Resources.ExceptionPropertyToCompareNull);
            }
            PropertyInfo propertyInfo = ValidationReflectionHelper.GetProperty(ownerType, this.PropertyToCompare, false);
            if (propertyInfo == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionPropertyToCompareNotFound,
                        this.PropertyToCompare,
                        ownerType.FullName));
            }

            return new PropertyComparisonValidator(memberValueAccessBuilder.GetPropertyValueAccess(propertyInfo),
                this.ComparisonOperator,
                this.Negated);
        }
    }
}
