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
using System.Collections.Generic;

namespace AExpense.DataAccessLayer
{
    public interface IExpenseRepository
    {
        void SaveExpense(Model.Expense expense);
        IEnumerable<Model.Expense> GetAllExpenses();
        IEnumerable<Model.Expense> GetExpensesForApproval(string approverName);
        IEnumerable<Model.Expense> GetExpensesByUser(string userName);
        Model.Expense GetExpenseById(Guid expenseId);
        void UpdateApproved(Model.Expense expense);
    }
}