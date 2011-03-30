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

using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    partial class ValidationCallHandlerData
    {
        /// <summary>
        /// The ruleset name to use for all types. Empty string means default ruleset 
        /// </summary>
        /// <value>The "ruleSet" configuration property.</value>
        public string RuleSet { get; set; }

        /// <summary>
        /// SpecificationSource (Both | Attributes | Configuration) : Where to look for validation rules. Default is Both.
        /// </summary>
        /// <value>The "specificationSource" configuration attribute.</value>
        public SpecificationSource SpecificationSource { get; set; }
    }
}
