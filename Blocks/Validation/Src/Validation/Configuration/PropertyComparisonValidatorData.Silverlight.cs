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
    partial class PropertyComparisonValidatorData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyComparisonValidatorData"/> class.
        /// </summary>
        public PropertyComparisonValidatorData()
        {
            this.PropertyToCompare = string.Empty;
        }

        /// <summary>
        /// Gets or sets the <see cref="ComparisonOperator"/> describing the comparison that the represented <see cref="PropertyComparisonValidator"/>.
        /// </summary>
        public ComparisonOperator ComparisonOperator { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that the represented <see cref="PropertyComparisonValidator"/> will use to retrieve the value to compare.
        /// </summary>
        public string PropertyToCompare { get; set; }
    }
}
