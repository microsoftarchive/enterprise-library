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
using System.Security.Principal;
using System.Threading;
using aExpense.UnitTests.Mocks;
using AExpense;
using AExpense.DataAccessLayer;
using AExpense.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace aExpense.UnitTests
{
    /// <summary>
    /// Summary description for ExpensesFixture
    /// </summary>
    [TestClass]
    public class ExpensesFixture
    {
        private MockExpenseDataContext dataContext;
        private IUnityContainer container;

        [TestInitialize]
        public void TestInit()
        {
            this.dataContext = new MockExpenseDataContext();
            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(this.container);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.dataContext.Dispose();
            this.container.Dispose();
        }

        [TestMethod]
        public void SaveExpense()
        {
            Guid expenseIdToSave = Guid.Empty;

            var stubUser = new User { UserName = "user name" };
            var stubManager = "the manager";
            var expenseToSave = new AExpense.Model.Expense
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

            var repository = new ExpenseRepository(dataContext);
            repository.SaveExpense(expenseToSave);

            var expenseEntity = repository.GetExpenseById(expenseIdToSave);

            Assert.IsNotNull(expenseEntity);
            Assert.AreEqual(expenseToSave.Approved, expenseEntity.Approved);
            Assert.AreEqual(expenseToSave.CostCenter, expenseEntity.CostCenter);
            Assert.AreEqual(expenseToSave.Date, expenseEntity.Date);
            Assert.AreEqual(expenseToSave.Description, expenseEntity.Description);
            Assert.AreEqual(expenseToSave.Id, expenseEntity.Id);
            Assert.AreEqual(expenseToSave.ReimbursementMethod, expenseEntity.ReimbursementMethod);
            Assert.AreEqual(expenseToSave.Title, expenseEntity.Title);
            Assert.AreEqual(expenseToSave.User.UserName, expenseEntity.User.UserName);
            Assert.AreEqual(expenseToSave.ApproverName, expenseEntity.ApproverName);

        }


        [TestMethod]
        public void GetAllExpenses()
        {
            var expected = new AExpense.Model.Expense
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

            var repository = new ExpenseRepository(dataContext);
            repository.SaveExpense(expected);
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
            var expected = new AExpense.Model.Expense
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

            var anotherExpense = new AExpense.Model.Expense
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

            var notForMeExpense = new AExpense.Model.Expense
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

            var repository = new ExpenseRepository(dataContext);

            repository.SaveExpense(expected);
            repository.SaveExpense(anotherExpense);
            repository.SaveExpense(notForMeExpense);

            var expenses = repository.GetExpensesForApproval("approverName");

            Assert.IsNotNull(expenses);
            Assert.AreEqual(2, expenses.Count());
        }

        [TestMethod]
        public void GetAllExpensesByUser()
        {
            var expected = new AExpense.Model.Expense
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

            var repository = new ExpenseRepository(dataContext);
            repository.SaveExpense(expected);

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
            var expected = new AExpense.Model.Expense
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

            var repository = new ExpenseRepository(dataContext);
            repository.SaveExpense(expected);

            var expenses = repository.GetExpensesByUser("no existing user name");

            Assert.AreEqual(0, expenses.Count());
        }

        [TestMethod]
        public void ApproveExpense()
        {
            var expected = new AExpense.Model.Expense
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
           
            var repository = new ExpenseRepository(dataContext);
            repository.SaveExpense(expected);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("test"), new string[] { "Manager" });
            repository.UpdateApproved(expected);

            var actual = repository.GetExpenseById(expected.Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Approved, actual.Approved);
        }

    }
}
