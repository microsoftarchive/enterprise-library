// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using AExpense.Model;

namespace AExpense.UnitTests.Util
{
    public static class StubsHelper
    {
        public static Model.Expense GenerateExpenseStub()
        {
            Guid expenseIdToValidate = Guid.NewGuid();

            var stubUser = new User { UserName = "user name" };
            var stubManager = "ADATUM\\mary";

            return new Model.Expense
            {
                Id = expenseIdToValidate,
                Date = new DateTime(1900, 01, 01),
                Title = "Title",
                Description = "Description",
                Total = 1.0,
                ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                CostCenter = "CostCenter",
                Approved = true,
                User = stubUser,
                ApproverName = stubManager
            };
        }

        public static Model.ExpenseItem GenerateExpenseItemStub()
        {
            Guid expenseItemIdToValidate = Guid.NewGuid();

            return new Model.ExpenseItem
            {
                Id = expenseItemIdToValidate,
                Amount = 1,
                Description = "Description"
            };
        }
    }
}
