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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Describes a <see cref="DateTimeRangeValidator"/>.
    /// </summary>
    public partial class DateTimeRangeValidatorData : RangeValidatorData<DateTime>
    {
        /// <summary>
        /// Creates the <see cref="DateTimeRangeValidator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <returns>The created <see cref="DateTimeRangeValidator"/>.</returns>	
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new DateTimeRangeValidator(this.LowerBound,
                this.LowerBoundType,
                this.UpperBound,
                this.UpperBoundType,
                Negated);
        }
    }
}
