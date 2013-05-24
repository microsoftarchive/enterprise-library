#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;

namespace AExpense.Model
{
    [Serializable]
    public class ExpenseItem
    {
        public Guid Id { get; set; }

        [StringLengthValidator(1, 100, MessageTemplateResourceName = "DescriptionNotNullValidation", MessageTemplateResourceType = typeof(Properties.Resources))]
        public string Description { get; set; }

        [RangeValidator(0d, RangeBoundaryType.Exclusive, 0d, RangeBoundaryType.Ignore, MessageTemplateResourceName = "AmountRangeValidation", MessageTemplateResourceType = typeof(Properties.Resources))]
        public double Amount { get; set; }
    }
}
