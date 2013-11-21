// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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
