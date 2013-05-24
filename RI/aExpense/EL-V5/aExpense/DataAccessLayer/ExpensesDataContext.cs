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