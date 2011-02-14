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

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    partial class RelativeDateTimeValidatorData
    {
        /// <summary>
        /// Gets or sets the unit to use when calculating the relative lower bound for the represented <see cref="RelativeDateTimeValidator"/>.
        /// </summary>
        public DateTimeUnit LowerUnit { get; set; }

        /// <summary>
        /// Gets or sets the unit to use when calculating the relative upper bound for the represented <see cref="RelativeDateTimeValidator"/>.
        /// </summary>
        public DateTimeUnit UpperUnit { get; set; }
    }
}
