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
    /// <summary>
    /// Configuration object to describe an instance of class <see cref="ValueValidator"/>.
    /// </summary>
    public abstract partial class ValueValidatorData : ValidatorData
    {
        /// <summary>
        /// Gets or sets a value indicating whether the behavior of the represented validator should be negated.
        /// </summary>
        public bool Negated { get; set; }
    }
}
