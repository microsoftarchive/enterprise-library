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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Describes a <see cref="RangeValidator"/>.
	/// </summary>
    [ResourceDisplayName(typeof(DesignResources), "RangeValidatorDataDisplayName")]
    [ResourceDescription(typeof(DesignResources), "RangeValidatorDataDescription")]
	public class RangeValidatorData : RangeValidatorData<string>
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorData"/> class.</para>
		/// </summary>
		public RangeValidatorData() { Type = typeof(RangeValidator); }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorData"/> class with a name.</para>
		/// </summary>
		public RangeValidatorData(string name)
			: base(name, typeof(RangeValidator))
		{ }

		/// <summary>
		/// Creates the <see cref="RangeValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="TypeConversionValidator"/>.</returns>	
		protected override Validator DoCreateValidator(Type targetType)
		{
			TypeConverter typeConverter = null;
			IComparable lowerBound = null;
			IComparable upperBound = null;
			
			if (targetType != null)
			{
				typeConverter = TypeDescriptor.GetConverter(targetType);
				if (typeConverter != null)
				{
					lowerBound = (IComparable)typeConverter.ConvertFromString(null, CultureInfo.CurrentCulture, LowerBound);
					upperBound = (IComparable)typeConverter.ConvertFromString(null, CultureInfo.CurrentCulture, UpperBound); 
				}
			}

			return new RangeValidator(lowerBound, LowerBoundType, upperBound, UpperBoundType, MessageTemplate, Negated);
		}
	}
}
