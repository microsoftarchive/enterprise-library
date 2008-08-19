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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="RelativeDateTimeValidatorData"/>.
	/// </summary>
	public class RelativeDateTimeValidatorData : RangeValidatorData<int>
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidatorData"/> class.</para>
		/// </summary>
		public RelativeDateTimeValidatorData()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public RelativeDateTimeValidatorData(string name)
			: base(name, typeof(RelativeDateTimeValidator))
		{ }

		private const string LowerUnitPropertyName = "lowerUnit";
		/// <summary>
		/// Gets or sets the unit to use when calculating the relative lower bound for the represented <see cref="RelativeDateTimeValidator"/>.
		/// </summary>
		[ConfigurationProperty(LowerUnitPropertyName, DefaultValue=DateTimeUnit.None)]
		public DateTimeUnit LowerUnit
		{
			get { return (DateTimeUnit)this[LowerUnitPropertyName]; }
			set { this[LowerUnitPropertyName] = value; }
		}

		private const string UpperUnitPropertyName = "upperUnit";
		/// <summary>
		/// Gets or sets the unit to use when calculating the relative upper bound for the represented <see cref="RelativeDateTimeValidator"/>.
		/// </summary>
		[ConfigurationProperty(UpperUnitPropertyName, DefaultValue = DateTimeUnit.None)]
		public DateTimeUnit UpperUnit
		{
			get { return (DateTimeUnit)this[UpperUnitPropertyName]; }
			set { this[UpperUnitPropertyName] = value; }
		}

		/// <summary>
		/// Creates the <see cref="RelativeDateTimeValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="RelativeDateTimeValidator"/>.</returns>	
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new RelativeDateTimeValidator(LowerBound, LowerUnit, LowerBoundType, UpperBound, UpperUnit, UpperBoundType, Negated);
		}
	}
}
