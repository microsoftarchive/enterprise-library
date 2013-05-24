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

namespace AExpense.Tests.Functional
{
    using System;
    using System.Linq;
    using DataAccessLayer;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using System.Configuration;
    using util = AExpense.Tests.Util;
    using System.Threading;
    using System.Security.Principal;

    [TestClass]
    public class ExpenseRepositoryFixture
    {
        private static readonly string TestDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpenseTesting"].ConnectionString;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            util.DatabaseHelper.CreateDatabase(TestDatabaseConnectionString);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            util.DatabaseHelper.DeleteDatabase(TestDatabaseConnectionString);
        }

        [TestMethod]
        public void SaveExpense()
        {
            Guid expenseIdToSave = Guid.Empty;
            Assert.IsNull(util.DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expenseIdToSave));
            var stubUser = new User { UserName = "user name" };
            var stubManager = "the manager";
            var expenseToSave = new Model.Expense
                                    {
                                        Id = expenseIdToSave,
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

            var repository = new ExpenseRepository(TestDatabaseConnectionString);
            repository.SaveExpense(expenseToSave);

            var expenseEntity = util.DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expenseIdToSave);
            Assert.IsNotNull(expenseEntity);
            Assert.AreEqual(expenseToSave.Total, Convert.ToDouble(expenseEntity.Amount));
            Assert.AreEqual(expenseToSave.Approved, expenseEntity.Approved);
            Assert.AreEqual(expenseToSave.CostCenter, expenseEntity.CostCenter);
            Assert.AreEqual(expenseToSave.Date, expenseEntity.Date);
            Assert.AreEqual(expenseToSave.Description, expenseEntity.Description);
            Assert.AreEqual(expenseToSave.Id, expenseEntity.Id);
            Assert.AreEqual(expenseToSave.ReimbursementMethod, Enum.Parse(typeof(ReimbursementMethod), expenseEntity.ReimbursementMethod));
            Assert.AreEqual(expenseToSave.Title, expenseEntity.Title);
            Assert.AreEqual(expenseToSave.User.UserName, expenseEntity.UserName);
            Assert.AreEqual(expenseToSave.ApproverName, expenseEntity.Approver);
        }

        [TestMethod]
        public void GetAllExpenses()
        {
            var expected = new Model.Expense
                               {
                                   Id = Guid.NewGuid(),
                                   Date = new DateTime(1900, 01, 01),
                                   Title = "Title",
                                   Description = "Description",
                                   Total = 1.0,
                                   ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                                   CostCenter = "CostCenter",
                                   Approved = true,
                                   User = new User { UserName = "user name" },
                                   ApproverName = "approver name"
                               };
            util.DatabaseHelper.CreateExpense(TestDatabaseConnectionString, expected);

            var repository = new ExpenseRepository(TestDatabaseConnectionString);
            var expenses = repository.GetAllExpenses();

            Assert.IsNotNull(expenses);
            var actual = expenses.ToList().Where(e => e.Id == expected.Id).FirstOrDefault();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Total, actual.Total);
            Assert.AreEqual(expected.Approved, actual.Approved);
            Assert.AreEqual(expected.CostCenter, actual.CostCenter);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ReimbursementMethod, actual.ReimbursementMethod);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.User.UserName, actual.User.UserName);
        }

        [TestMethod]
        public void GetAllExpensesForApproval()
        {
            var expected = new Model.Expense
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(1900, 01, 01),
                Title = "Title",
                Description = "Description",
                Total = 1.0,
                ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                CostCenter = "CostCenter",
                Approved = true,
                User = new User { UserName = "user Name" },
                ApproverName = "approverName"
            };

            var anotherExpense = new Model.Expense
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(1900, 01, 01),
                Title = "Title",
                Description = "Description",
                Total = 1.0,
                ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                CostCenter = "CostCenter",
                Approved = true,
                User = new User { UserName = "another user vName" },
                ApproverName = "approverName"
            };

            var notForMeExpense = new Model.Expense
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(1900, 01, 01),
                Title = "Title",
                Description = "Description",
                Total = 1.0,
                ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                CostCenter = "CostCenter",
                Approved = true,
                User = new User { UserName = "another user vName" },
                ApproverName = "Another approverName"
            };
            util.DatabaseHelper.CreateExpense(TestDatabaseConnectionString, expected);
            util.DatabaseHelper.CreateExpense(TestDatabaseConnectionString, anotherExpense);
            util.DatabaseHelper.CreateExpense(TestDatabaseConnectionString, notForMeExpense);

            var repository = new ExpenseRepository(TestDatabaseConnectionString);
            var expenses = repository.GetExpensesForApproval("approverName");

            Assert.IsNotNull(expenses);
            Assert.AreEqual(2, expenses.Count());
        }

        [TestMethod]
        public void GetAllExpensesByUser()
        {
            var expected = new Model.Expense
                               {
                                   Id = Guid.NewGuid(),
                                   Date = new DateTime(1900, 01, 01),
                                   Title = "Title",
                                   Description = "Description",
                                   Total = 1.0,
                                   ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                                   CostCenter = "CostCenter",
                                   Approved = true,
                                   User = new User { UserName = "user name" },
                                   ApproverName = "approver name"
                               };
            util.DatabaseHelper.CreateExpense(TestDatabaseConnectionString, expected);

            var repository = new ExpenseRepository(TestDatabaseConnectionString);
            var expenses = repository.GetExpensesByUser("user name");

            Assert.IsNotNull(expenses);
            var actual = expenses.ToList().Where(e => e.Id == expected.Id).FirstOrDefault();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Total, actual.Total);
            Assert.AreEqual(expected.Approved, actual.Approved);
            Assert.AreEqual(expected.CostCenter, actual.CostCenter);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ReimbursementMethod, actual.ReimbursementMethod);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.User.UserName, actual.User.UserName);
        }

        [TestMethod]
        public void ShouldGetZeroExpensesFromNonExistingUser()
        {
            var expected = new Model.Expense
                               {
                                   Id = Guid.NewGuid(),
                                   Date = new DateTime(1900, 01, 01),
                                   Title = "Title",
                                   Description = "Description",
                                   Total = 1.0,
                                   ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                                   CostCenter = "CostCenter",
                                   Approved = true,
                                   User = new User { UserName = "user name" },
                                   ApproverName = "approver name"
                               };
            util.DatabaseHelper.CreateExpense(TestDatabaseConnectionString, expected);

            var repository = new ExpenseRepository(TestDatabaseConnectionString);
            var expenses = repository.GetExpensesByUser("no existing user name");

            Assert.AreEqual(0, expenses.Count());
        }

        [TestMethod]
        public void ApproveExpense()
        {
            var expected = new Model.Expense
                               {
                                   Id = Guid.NewGuid(),
                                   Date = new DateTime(1900, 01, 01),
                                   Title = "Title",
                                   Description = "Description",
                                   Total = 1.0,
                                   ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                                   CostCenter = "CostCenter",
                                   Approved = false,
                                   User = new User { UserName = "user name" },
                                   ApproverName = "approver name"
                               };

            util.DatabaseHelper.CreateExpense(TestDatabaseConnectionString, expected);

            var repository = new ExpenseRepository(TestDatabaseConnectionString);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("test"), new string[] { "Manager" });
            repository.UpdateApproved(expected);

            var actual = util.DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expected.Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Approved, actual.Approved);
        }
    }
}