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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    partial class ObjectValidatorData
    {
        /// <summary>
        /// Gets or sets the name for the target ruleset for the represented validator.
        /// </summary>
        public string TargetRuleset { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to validate based on the static type or the actual type.
        /// </summary>
        public bool ValidateActualType { get; set; }
    }
}
