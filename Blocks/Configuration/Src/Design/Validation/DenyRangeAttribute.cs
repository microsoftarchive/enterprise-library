//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Specifies a property or event will be validated on a specific range and make sure that the value is outside that range.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
	public sealed class DenyRangeAttribute : AssertRangeAttribute
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Int32"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        public DenyRangeAttribute(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Single"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        public DenyRangeAttribute(float lowerBound, RangeBoundaryType lowerBoundType, float upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="String"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>       
        public DenyRangeAttribute(string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Double"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        public DenyRangeAttribute(double lowerBound, RangeBoundaryType lowerBoundType, double upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Int16"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        public DenyRangeAttribute(short lowerBound, RangeBoundaryType lowerBoundType, short upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Byte"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        public DenyRangeAttribute(byte lowerBound, RangeBoundaryType lowerBoundType, byte upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Char"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        public DenyRangeAttribute(char lowerBound, RangeBoundaryType lowerBoundType, char upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Decimal"/> lower and upper bounds.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="lowerBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        /// <param name="upperBound">
        /// The lower bound of the range.
        /// </param>
        /// <param name="upperBoundType">
        /// One of the <see cref="RangeBoundaryType"/> values.
        /// </param>
        public DenyRangeAttribute(decimal lowerBound, RangeBoundaryType lowerBoundType, decimal upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Validate the range data for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
        /// </summary>
        /// <param name="instance">
        /// The instance to validate.
        /// </param>
        /// <param name="propertyInfo">
        /// The property containing the value to validate.
        /// </param>
        /// <param name="errors">
        /// The collection to add any errors that occur durring the validation.
        /// </param>
		protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
			object propertyValue = propertyInfo.GetValue(instance, null);
			if (propertyValue == null)
			{
				return;
			}
			IComparable compareToObject = (IComparable)propertyValue;
			int lowerBoundGreaterThanValue = LowerBound.CompareTo(compareToObject);
			int upperBoundLessThanValue = UpperBound.CompareTo(compareToObject);
			
			bool addError = false;
			if ((lowerBoundGreaterThanValue != 0) && (LowerBoundType != RangeBoundaryType.Exclusive)) addError = true;

			if (lowerBoundGreaterThanValue < 0) addError = true;

			if ((upperBoundLessThanValue != 0) && (UpperBoundType != RangeBoundaryType.Exclusive)) addError = true;
			
			if (upperBoundLessThanValue > 0) addError = true;

			if (addError)
			{
				string name = propertyInfo.Name;
				errors.Add(new ValidationError(instance as ConfigurationNode, name, string.Format(CultureInfo.CurrentUICulture, Resources.ValueOutsideRangeErrorMessage, name)));
			}			
        }
    }
}
