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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Describes a <see cref="RangeValidator{T}"/>.
    /// </summary>
    partial class RangeValidatorData<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets or sets the lower bound for the represented validator.
        /// </summary>
        public virtual T LowerBound { get; set; }

        /// <summary>
        /// Gets or sets the lower bound type for the represented validator.
        /// </summary>
        public RangeBoundaryType LowerBoundType { get; set; }

        /// <summary>
        /// Gets or sets the upper bound for the represented validator.
        /// </summary>
        public virtual T UpperBound { get; set; }

        /// <summary>
        /// Gets or sets the upper bound type for the represented validator.
        /// </summary>
        public RangeBoundaryType UpperBoundType { get; set; }
    }
}
