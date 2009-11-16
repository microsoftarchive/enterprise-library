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

using System.ComponentModel.DataAnnotations;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
    public class TestObjectWithMultipleSourceValidationAttributesOnProperties
    {
        [MockValidator(true, MessageTemplate = "vab-only")]
        [MockValidator(true, MessageTemplate = "vab-only-RuleA", Ruleset = "RuleA")]
        [MockValidator(true, MessageTemplate = "vab-only-RuleB", Ruleset = "RuleB")]
        public string PropertyWithVABOnlyAttributes { get; set; }

        [StringLength(1, ErrorMessage = "data annotations-only")]
        public string PropertyWithDataAnnotationsAttributes { get; set; }

        [MockValidator(true, MessageTemplate = "vab-mixed")]
        [StringLength(1, ErrorMessage = "data annotations-mixed")]
        public string PropertyWithMixedAttributes { get; set; }
    }
}
