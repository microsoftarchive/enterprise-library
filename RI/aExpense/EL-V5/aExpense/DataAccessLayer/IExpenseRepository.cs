// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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