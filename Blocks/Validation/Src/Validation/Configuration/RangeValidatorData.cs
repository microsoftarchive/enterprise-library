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
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Describes a <see cref="RangeValidator"/>.
    /// </summary>
    public partial class RangeValidatorData : RangeValidatorData<string>
    {
        /// <summary>
        /// Creates the <see cref="RangeValidator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <returns>The created <see cref="TypeConversionValidator"/>.</returns>	
        protected override Validator DoCreateValidator(Type targetType)
        {
            IComparable lowerBound = null;
            IComparable upperBound = null;
            
            if (targetType != null)
            {
                var conversionCulture = Culture ?? CultureInfo.CurrentCulture;
                lowerBound = Convert.ChangeType(this.LowerBound, targetType, conversionCulture) as IComparable;
                upperBound = Convert.ChangeType(this.UpperBound, targetType, conversionCulture) as IComparable;
            }

            return new RangeValidator(lowerBound, LowerBoundType, upperBound, UpperBoundType, MessageTemplate, Negated);
        }
    }
}
