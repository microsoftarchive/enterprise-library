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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Tests
{
    public class ValidatedObject
    {
        [RangeValidator(10, RangeBoundaryType.Inclusive, 20, RangeBoundaryType.Inclusive, MessageTemplate = "invalid int")]
        public int ValidatedIntProperty { get; set; }

        [RegexValidator(@"^a*$", MessageTemplate = "invalid string")]
        public string ValidatedStringProperty { get; set; }

        public int NonValidatedProperty { get; set; }

        [RegexValidator(@"^a*$", MessageTemplate = "invalid string: vab")]
        [StringLength(2, ErrorMessage = "invalid string: data annotations")]
        public string MultipleSourceValidatedStringProperty { get; set; }

        [RegexValidator(@"^a*$", MessageTemplate = "invalid string default")]
        [RegexValidator(@"^a*$", MessageTemplate = "invalid string ruleset", Ruleset = "A")]
        public string MultipleRulesetValidatedStringProperty { get; set; }
    }
}
