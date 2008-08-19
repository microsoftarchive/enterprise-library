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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Internal implementation for range checking validators.
	/// </summary>
	/// <typeparam name="T">The type of value being checked for ranges.</typeparam>
	internal class RangeChecker<T>
		where T : IComparable
	{
		private T lowerBound;
		private RangeBoundaryType lowerBoundType;
		private T upperBound;
		private RangeBoundaryType upperBoundType;

		public RangeChecker(T lowerBound, RangeBoundaryType lowerBoundType,
			T upperBound, RangeBoundaryType upperBoundType)
		{
			if (upperBound == null && upperBoundType != RangeBoundaryType.Ignore)
				throw new ArgumentException(Resources.ExceptionUpperBoundNull);
			if (lowerBound == null && lowerBoundType != RangeBoundaryType.Ignore)
				throw new ArgumentException(Resources.ExceptionLowerBoundNull);

			if (lowerBoundType != RangeBoundaryType.Ignore
				&& upperBoundType != RangeBoundaryType.Ignore
				&& upperBound.CompareTo(lowerBound) < 0)
				throw new ArgumentException(Resources.ExceptionUpperBoundLowerThanLowerBound);

			this.lowerBound = lowerBound;
			this.lowerBoundType = lowerBoundType;
			this.upperBound = upperBound;
			this.upperBoundType = upperBoundType;
		}

		public bool IsInRange(T target)
		{
			if (this.lowerBoundType > RangeBoundaryType.Ignore)
			{
				int lowerBoundComparison = this.lowerBound.CompareTo(target);
				if (lowerBoundComparison > 0)
					return false;
				if (this.lowerBoundType == RangeBoundaryType.Exclusive && lowerBoundComparison == 0)
					return false;
			}
			if (this.upperBoundType > RangeBoundaryType.Ignore)
			{
				int upperBoundComparison = this.upperBound.CompareTo(target);
				if (upperBoundComparison < 0)
					return false;
				if (this.upperBoundType == RangeBoundaryType.Exclusive && upperBoundComparison == 0)
					return false;
			}

			return true;
		}

		#region test only properties

		internal T LowerBound
		{
			get { return this.lowerBound; }
		}

		internal T UpperBound
		{
			get { return this.upperBound; }
		}

		internal RangeBoundaryType LowerBoundType
		{
			get { return this.lowerBoundType; }
		}

		internal RangeBoundaryType UpperBoundType
		{
			get { return this.upperBoundType; }
		}

		#endregion
	}
}
