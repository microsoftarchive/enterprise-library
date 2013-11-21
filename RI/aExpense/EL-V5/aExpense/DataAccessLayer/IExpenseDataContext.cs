// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

namespace AExpense.DataAccessLayer
{
    public interface IExpenseDataContext: IDisposable
    {
        IExpenseTable<Expense> Expenses { get; }
        IExpenseTable<ExpenseDetail> ExpenseDetails { get; }
        void SubmitChanges();
    }
}