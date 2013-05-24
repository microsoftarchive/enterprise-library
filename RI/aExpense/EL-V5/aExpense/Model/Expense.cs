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
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AExpense.Validation;

namespace AExpense.Model
{
    [Serializable]
    public class Expense
    {        
        public Expense()
        {
            this.Id = Guid.NewGuid();
            this.Description = string.Empty;
            this.Approved = false;
            this.Details = new List<ExpenseItem>();
        }

        public Guid Id { get; set; }

        [NotNullValidator(MessageTemplateResourceName = "UserNotNullValidation", MessageTemplateResourceType=typeof(Properties.Resources))]
        public User User { get; set; }

        [StringLengthValidator(1, 100, MessageTemplateResourceName = "TitleNotNullValidation", MessageTemplateResourceType = typeof(Properties.Resources))]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public bool Approved { get; set; }

        [NotNullValidator(MessageTemplateResourceName = "CostCenterNotNullValidation", MessageTemplateResourceType = typeof(Properties.Resources))]
        public string CostCenter { get; set; }

        public ReimbursementMethod ReimbursementMethod { get; set; }

        public ICollection<ExpenseItem> Details { get; set; }

        public double Total { get; set; }

        [StringLengthValidator(1, 100, MessageTemplateResourceName = "ApproverNameNotNullValidation", MessageTemplateResourceType = typeof(Properties.Resources))]
        [ApproverValidator]
        public string ApproverName { get; set; }

        public string Description { get; set; }
    }
}