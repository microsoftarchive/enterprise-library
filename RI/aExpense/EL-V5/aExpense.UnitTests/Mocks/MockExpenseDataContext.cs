// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AExpense.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace aExpense.UnitTests.Mocks
{

    public class MockExpenseDataContext : IExpenseDataContext
    {
        private IExpenseTable<Expense> _expenses;
        private IExpenseTable<ExpenseDetail> _expenseDetails;


        public MockExpenseDataContext()
        {
            _expenses = new MockExpenseTable<Expense>(new List<Expense>());
            _expenseDetails = new MockExpenseTable<ExpenseDetail>(new List<ExpenseDetail>());
        }

        public void SubmitChanges()
        {
        }

        public void Dispose()
        {
        }

        IExpenseTable<Expense> IExpenseDataContext.Expenses
        {
            get 
            {
                return _expenses;
            }
        }

        IExpenseTable<ExpenseDetail> IExpenseDataContext.ExpenseDetails
        {
            get 
            {
                return _expenseDetails;
            }
        }
    }
}