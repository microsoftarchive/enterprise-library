// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AExpense.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.Unity;

namespace AExpense.Model
{
    [Serializable]
    public class Expense 
    {        
        [InjectionConstructor]
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