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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Attribute to specify date range validation on a property, method or field.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Method
		| AttributeTargets.Parameter,
		AllowMultiple = true,
		Inherited = false)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019",
		Justification = "Fields are used internally")]
	public sealed class DateTimeRangeValidatorAttribute : ValueValidatorAttribute
	{
		private DateTime lowerBound;
		private RangeBoundaryType lowerBoundType;
		private DateTime upperBound;
		private RangeBoundaryType upperBoundType;

		/// <summary>
		/// Initializes a new instance with an upper bound.
		/// </summary>
		/// <param name="upperBound">An ISO8601 formatted string representing the upper bound, like "2006-01-20T00:00:00".</param>
		public DateTimeRangeValidatorAttribute(string upperBound)
			: this(ConvertToISO8601Date(upperBound))
		{ }

		/// <summary>
		/// Initializes a new instance with an upper bound.
		/// </summary>
		/// <param name="upperBound">The upper bound.</param>
		public DateTimeRangeValidatorAttribute(DateTime upperBound)
			: this(default(DateTime), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
		{ }

		/// <summary>
		/// Initializes a new instance with lower and upper bounds.
		/// </summary>
		/// <param name="lowerBound">An ISO8601 formatted string representing the lower bound, like "2006-01-20T00:00:00".</param>
		/// <param name="upperBound">An ISO8601 formatted string representing the upper bound, like "2006-01-20T00:00:00".</param>
		public DateTimeRangeValidatorAttribute(string lowerBound, string upperBound)
			: this(ConvertToISO8601Date(lowerBound), ConvertToISO8601Date(upperBound))
		{ }

		/// <summary>
		/// Initializes a new instance with lower and upper bounds.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		public DateTimeRangeValidatorAttribute(DateTime lowerBound, DateTime upperBound)
			: this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
		{ }

		/// <summary>
		/// Initializes a new instance with lower and upper bounds, and bound types.
		/// </summary>
		/// <param name="lowerBound">An ISO8601 formatted string representing the lower bound, like "2006-01-20T00:00:00".</param>
		/// <param name="lowerBoundType">The bound type for the lower bound.</param>
		/// <param name="upperBound">An ISO8601 formatted string representing the upper bound, like "2006-01-20T00:00:00".</param>
		/// <param name="upperBoundType">The bound type for the upper bound.</param>
		public DateTimeRangeValidatorAttribute(string lowerBound,
			RangeBoundaryType lowerBoundType,
			string upperBound,
			RangeBoundaryType upperBoundType)
			: this(ConvertToISO8601Date(lowerBound), lowerBoundType, ConvertToISO8601Date(upperBound), upperBoundType)
		{ }

		/// <summary>
		/// Initializes a new instance with lower and upper bounds, and bound types.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The bound type for the lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The bound type for the upper bound.</param>
		public DateTimeRangeValidatorAttribute(DateTime lowerBound,
			RangeBoundaryType lowerBoundType,
			DateTime upperBound,
			RangeBoundaryType upperBoundType)
		{
			this.lowerBound = lowerBound;
			this.lowerBoundType = lowerBoundType;
			this.upperBound = upperBound;
			this.upperBoundType = upperBoundType;
		}

		/// <summary>
		/// Creates the <see cref="DateTimeRangeValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="Validator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new DateTimeRangeValidator(this.lowerBound,
				this.lowerBoundType,
				this.upperBound,
				this.upperBoundType,
				Negated);
		}

		private static DateTime ConvertToISO8601Date(string iso8601DateString)
		{
			if (string.IsNullOrEmpty(iso8601DateString))
			{
				return default(DateTime);
			}
			try
			{
				return DateTime.ParseExact(iso8601DateString, "s", CultureInfo.InvariantCulture);
			}
			catch (FormatException e)
			{
				throw new ArgumentException(Resources.ExceptionInvalidDate, "dateString", e);
			}
		}
	}
}
