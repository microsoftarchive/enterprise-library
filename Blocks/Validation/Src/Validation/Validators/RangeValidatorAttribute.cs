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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using System.ComponentModel;
using System.Collections;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Represents a <see cref="RangeValidator"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Method
		| AttributeTargets.Parameter,
		AllowMultiple = true,
		Inherited = false)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019",
		Justification = "Fields are used internally")]
	public sealed class RangeValidatorAttribute : ValueValidatorAttribute
	{
		private IComparable lowerBound;
		private RangeBoundaryType lowerBoundType;
		private IComparable upperBound;
		private RangeBoundaryType upperBoundType;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorAttribute"/> class with fully specified
		/// int bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public RangeValidatorAttribute(int lowerBound, RangeBoundaryType lowerBoundType,
				int upperBound, RangeBoundaryType upperBoundType)
			: this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorAttribute"/> class with fully specified
		/// double bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public RangeValidatorAttribute(double lowerBound, RangeBoundaryType lowerBoundType,
				double upperBound, RangeBoundaryType upperBoundType)
			: this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorAttribute"/> class with fully specified
		/// DateTime bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public RangeValidatorAttribute(DateTime lowerBound, RangeBoundaryType lowerBoundType,
				DateTime upperBound, RangeBoundaryType upperBoundType)
			: this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorAttribute"/> class with fully specified
		/// long bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public RangeValidatorAttribute(long lowerBound, RangeBoundaryType lowerBoundType,
				long upperBound, RangeBoundaryType upperBoundType)
			: this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorAttribute"/> class with fully specified
		/// string bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public RangeValidatorAttribute(string lowerBound, RangeBoundaryType lowerBoundType,
				string upperBound, RangeBoundaryType upperBoundType)
			: this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorAttribute"/> class with fully specified
		/// float bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public RangeValidatorAttribute(float lowerBound, RangeBoundaryType lowerBoundType,
				float upperBound, RangeBoundaryType upperBoundType)
			: this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorAttribute"/> class with fully specified
		/// bound constraints using string representations of the boundaries.</para>
		/// </summary>
		/// <param name="boundType">The type of the boundaries.</param>
		/// <param name="lowerBound">The lower bound represented with a string, or <see langword="null"/>.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound, or <see langword="null"/>.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public RangeValidatorAttribute(Type boundType, string lowerBound, RangeBoundaryType lowerBoundType,
			string upperBound, RangeBoundaryType upperBoundType)
			: this(ConvertBound(boundType, lowerBound, "lowerBound"), lowerBoundType,
					ConvertBound(boundType, upperBound, "upperBound"), upperBoundType)
		{ }

		private RangeValidatorAttribute(IComparable lowerBound, RangeBoundaryType lowerBoundType,
			IComparable upperBound, RangeBoundaryType upperBoundType)
		{
			ValidatorArgumentsValidatorHelper.ValidateRangeValidator(lowerBound, lowerBoundType, upperBound, upperBoundType);

			this.lowerBound = lowerBound;
			this.lowerBoundType = lowerBoundType;
			this.upperBound = upperBound;
			this.upperBoundType = upperBoundType;
		}

		private static IComparable ConvertBound(Type boundType, string bound, string boundParameter)
		{
			if (boundType == null)
			{
				throw new ArgumentNullException("boundType");
			}
			if (!typeof(IComparable).IsAssignableFrom(boundType))
			{
				throw new ArgumentException(Resources.ExceptionBoundTypeNotIComparable, "boundType");
			}

			if (bound == null)
			{
				return null;
			}

			if (boundType == typeof(DateTime))
			{
				try
				{
					return DateTime.ParseExact(bound, "s", CultureInfo.InvariantCulture);
				}
				catch (FormatException e)
				{
					throw new ArgumentException(Resources.ExceptionInvalidDate, boundParameter, e);
				}
			}
			else
			{
				try
				{
					return (IComparable)TypeDescriptor.GetConverter(boundType).ConvertFrom(null, CultureInfo.InvariantCulture, bound);
				}
				catch (Exception e)
				{
					throw new ArgumentException(Resources.ExceptionCannotConvertBound, e);
				}
			}
		}

		/// <summary>
		/// Creates the <see cref="RangeValidator"/> described by the attribute object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <remarks>This operation must be overriden by subclasses.</remarks>
		/// <returns>The created <see cref="RangeValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new RangeValidator(this.lowerBound, this.lowerBoundType, this.upperBound, this.upperBoundType, this.Negated);
		}

		internal IComparable LowerBound { get { return this.lowerBound; } }
		internal RangeBoundaryType LowerBoundType { get { return this.lowerBoundType; } }
		internal IComparable UpperBound { get { return this.upperBound; } }
		internal RangeBoundaryType UpperBoundType { get { return this.upperBoundType; } }
	}
}
