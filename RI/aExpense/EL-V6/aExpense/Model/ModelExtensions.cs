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

using System;
using System.Linq;

namespace AExpense.Model
{
    public static class ModelExtensions
    {
        public static Model.Expense ToModel(this DataAccessLayer.Expense entity)
        {
            if (entity == null) 
                return null;

            var expense = new Expense()
            {
                Id = entity.Id,
                Details = entity.ExpenseDetails.Select(i => i.ToModel()).ToList(),
                Approved = entity.Approved,
                CostCenter = entity.CostCenter,
                Date = entity.Date,
                ReimbursementMethod = (ReimbursementMethod)Enum.Parse(typeof(ReimbursementMethod), entity.ReimbursementMethod),
                Title = entity.Title,
                User = new User { UserName = entity.UserName },
                ApproverName = entity.Approver,
                Total = Convert.ToDouble(entity.Amount),
                Description = entity.Description
            };

            return expense;
        }

        public static ExpenseItem ToModel(this DataAccessLayer.ExpenseDetail entity)
        {
            if (entity == null)
                return null;

            var expenseItem = new ExpenseItem
            {
                Id = entity.Id,
                Amount = Convert.ToDouble(entity.Amount),
                Description = entity.Description,
            };

            return expenseItem;
        }

        public static DataAccessLayer.Expense ToEntity(this Model.Expense model)
        {
            if (model == null)
                return null;

            var expense = new DataAccessLayer.Expense
            {
                Id = model.Id,
                Approved = model.Approved,
                CostCenter = model.CostCenter,
                Date = model.Date,
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), model.ReimbursementMethod),
                Title = model.Title,
                UserName = model.User.UserName,
                Approver = model.ApproverName,
                Amount = Convert.ToDecimal(model.Total),
                Description = model.Description
            };

            return expense;
        }

        public static DataAccessLayer.ExpenseDetail ToEntity(this Model.ExpenseItem model, Model.Expense expense)
        {
            if (model == null || expense == null)
                return null;

            var expenseItem = new DataAccessLayer.ExpenseDetail
            {
                Id = model.Id,
                Description = model.Description,
                Amount = Convert.ToDecimal(model.Amount),
                ExpenseId = expense.Id
            };

            return expenseItem;
        }
    }
}