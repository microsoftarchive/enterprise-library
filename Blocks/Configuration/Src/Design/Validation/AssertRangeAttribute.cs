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
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Specifies a property or event will be validated on a specific range.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes"), AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class AssertRangeAttribute : ValidationAttribute
    {
        private readonly IComparable lowerBound;
        private readonly IComparable upperBound;
        private readonly RangeBoundaryType lowerBoundType;
        private readonly RangeBoundaryType upperBoundType;

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Int32"/> lower and upper bounds.
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
        public AssertRangeAttribute(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Single"/> lower and upper bounds.
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
        public AssertRangeAttribute(float lowerBound, RangeBoundaryType lowerBoundType, float upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="String"/> lower and upper bounds.
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
        public AssertRangeAttribute(string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Double"/> lower and upper bounds.
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
        public AssertRangeAttribute(double lowerBound, RangeBoundaryType lowerBoundType, double upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Int16"/> lower and upper bounds.
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
        public AssertRangeAttribute(short lowerBound, RangeBoundaryType lowerBoundType, short upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Byte"/> lower and upper bounds.
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
        public AssertRangeAttribute(byte lowerBound, RangeBoundaryType lowerBoundType, byte upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Char"/> lower and upper bounds.
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
        public AssertRangeAttribute(char lowerBound, RangeBoundaryType lowerBoundType, char upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Decimal"/> lower and upper bounds.
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
        public AssertRangeAttribute(decimal lowerBound, RangeBoundaryType lowerBoundType, decimal upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="IComparable"/> lower and upper bounds.
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
        protected AssertRangeAttribute(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType)
        {
			if (null == lowerBound) throw new ArgumentNullException("lowerBound");
			if (null == upperBound) throw new ArgumentNullException("upperBound");

            if (lowerBound.CompareTo(upperBound) > 0)
            {
                throw new ArgumentOutOfRangeException("lowerBound", string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionLowerBoundOutOfRange, lowerBound.ToString(), upperBound.ToString()));
            }
            this.lowerBound = lowerBound;
            this.lowerBoundType = lowerBoundType;
            this.upperBound = upperBound;
            this.upperBoundType = upperBoundType;
        }

		/// <summary>
		/// Gets the lower bound of the range.
		/// </summary>
		/// <value>
		/// The lower bound of the range.
		/// </value>
		public IComparable LowerBound
		{
			get { return lowerBound; }
		}

		/// <summary>
		/// Gets the upper bound of the range.
		/// </summary>
		/// <value>
		/// The upper bound of the range.
		/// </value>
		public IComparable UpperBound
		{
			get { return upperBound; }
		}

		/// <summary>
		/// Gets the lower bound type condition.
		/// </summary>
		/// <value>
		/// One of the <see cref="RangeBoundaryType"/> values.
		/// </value>
		public RangeBoundaryType LowerBoundType
		{
			get { return lowerBoundType; }
		}

		/// <summary>
		/// Gets the upper bound type condition.
		/// </summary>
		/// <value>
		/// One of the <see cref="RangeBoundaryType"/> values.
		/// </value>
		public RangeBoundaryType UpperBoundType
		{
			get { return upperBoundType; }
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
        /// The collection to add any errors that occur during the validation.
        /// </param>		
		protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {			
            object propertyValue = propertyInfo.GetValue(instance, null);
            if (propertyValue == null)
            {
                return;
            }
            IComparable compareToObject = (IComparable)propertyValue;
            int lowerBoundGreaterThanValue = lowerBound.CompareTo(compareToObject);
            int upperBoundLessThanValue = upperBound.CompareTo(compareToObject);
			bool addError = false;

            if ((lowerBoundGreaterThanValue == 0) && (lowerBoundType == RangeBoundaryType.Exclusive))  addError = true; 

            if (lowerBoundGreaterThanValue > 0) addError = true;

            if ((upperBoundLessThanValue == 0) && (upperBoundType == RangeBoundaryType.Exclusive)) addError = true;

			if (upperBoundLessThanValue < 0) addError = true;

			if (addError) errors.Add(CreateValidationError(instance, propertyInfo));
        }

		private static ValidationError CreateValidationError(object instance, PropertyInfo propertyInfo)
		{
			return new ValidationError(instance as ConfigurationNode, propertyInfo.Name, GetErrorMessage(propertyInfo));
		}

		private static string GetErrorMessage(PropertyInfo propertyInfo)
		{
			return string.Format(CultureInfo.CurrentUICulture, Resources.ValueNotInRangeErrorMessage, propertyInfo.Name);
		}
    }
}
