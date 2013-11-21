// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AExpense.DataAccessLayer
{
    public partial class ExpensesDataContext : IExpenseDataContext, IDisposable
    {
        IExpenseTable<Expense> IExpenseDataContext.Expenses
        {
            get 
            {
                return new ExpenseTable<Expense>(this.GetTable<Expense>());
            }
        }

        IExpenseTable<ExpenseDetail> IExpenseDataContext.ExpenseDetails
        {
            get
            {
                return new ExpenseTable<ExpenseDetail>(this.GetTable<ExpenseDetail>());
            }
        }
    }
}