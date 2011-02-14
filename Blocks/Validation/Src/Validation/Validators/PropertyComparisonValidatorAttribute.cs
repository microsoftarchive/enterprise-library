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
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    /// <summary>
    /// Represents a <see cref="PropertyComparisonValidator"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019",
        Justification = "Fields are used internally")]
    [AttributeUsage(AttributeTargets.Property
        | AttributeTargets.Field
        | AttributeTargets.Method
        | AttributeTargets.Parameter,
        AllowMultiple = true,
        Inherited = false)]
    public sealed partial class PropertyComparisonValidatorAttribute : ValueValidatorAttribute
    {
        private string propertyToCompare;
        private ComparisonOperator comparisonOperator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyComparisonValidatorAttribute"/> class.
        /// </summary>
        /// <param name="propertyToCompare">The name of the property to use when comparing a value.</param>
        /// <param name="comparisonOperator">The <see cref="ComparisonOperator"/> representing the kind of comparison to perform.</param>
        public PropertyComparisonValidatorAttribute(string propertyToCompare, ComparisonOperator comparisonOperator)
        {
            if (propertyToCompare == null)
            {
                throw new ArgumentNullException("propertyToCompare");
            }

            this.propertyToCompare = propertyToCompare;
            this.comparisonOperator = comparisonOperator;
        }

        /// <summary>
        /// The name of the property to use when comparing a value.
        /// </summary>
        public string PropertyToCompare
        {
            get { return propertyToCompare; }
        }

        /// <summary>
        /// The <see cref="ComparisonOperator"/> representing the kind of comparison to perform.
        /// </summary>
        public ComparisonOperator ComparisonOperator
        {
            get { return comparisonOperator; }
        }

        /// <summary>
        /// Creates the <see cref="Validator"/> described by the attribute.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <param name="ownerType">The type of the object from which the value to validate is extracted.</param>
        /// <param name="memberValueAccessBuilder">The <see cref="MemberValueAccessBuilder"/> to use for validators that
        /// require access to properties.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <returns>The created <see cref="Validator"/>.</returns>
        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder, ValidatorFactory validatorFactory)
        {
            PropertyInfo propertyInfo = ValidationReflectionHelper.GetProperty(ownerType, this.PropertyToCompare, false);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException(
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

        /// <summary>
        /// Creates the <see cref="Validator"/> described by the attribute object providing validator specific
        /// information.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This method must not be called on this class. Call 
        /// <see cref="PropertyComparisonValidatorAttribute.DoCreateValidator(Type, Type, MemberValueAccessBuilder, ValidatorFactory)"/>.</remarks>
        protected override Validator DoCreateValidator(Type targetType)
        {
            throw new NotImplementedException(Resources.ExceptionShouldNotCall);
        }
    }
}
