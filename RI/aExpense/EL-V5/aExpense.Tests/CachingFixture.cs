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

using AExpense.DataAccessLayer;
using AExpense.Model;
using AExpense.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using util = AExpense.Tests.Util;
using System.Security.Principal;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class CachingAppBlockFixture
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
        public void CanGetExpenseForApprovalFromCache()
        { 

            Guid expenseIdToSave = Guid.NewGuid();
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
                                        Approved = false,
                                        User = stubUser,
                                      
                                        ApproverName = stubManager
                                    };

            var repository = new ExpenseRepository(TestDatabaseConnectionString, TimeSpan.MaxValue);
            repository.SaveExpense(expenseToSave);

            var expToApprove = repository.GetExpensesForApproval(stubManager);

            DatabaseHelper.BypassRepositoryAndDeleteExpense(TestDatabaseConnectionString, expenseToSave.Id);

            expToApprove = repository.GetExpensesForApproval(stubManager);

            Assert.AreEqual(1, expToApprove.Count());

            Assert.AreEqual(expenseIdToSave, expToApprove.First().Id);
        }

        [TestMethod]
        public void GetExpenseForApprovalAreGoneAfterCacheExpires()
        {

            Guid expenseIdToSave = Guid.NewGuid();
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
                Approved = false,
                User = stubUser,

                ApproverName = stubManager
            };

            var repository = new ExpenseRepository(TestDatabaseConnectionString, new TimeSpan(0, 0, 2));
            repository.SaveExpense(expenseToSave);

            var expToApprove = repository.GetExpensesForApproval(stubManager);

            DatabaseHelper.BypassRepositoryAndDeleteExpense(TestDatabaseConnectionString, expenseToSave.Id);

            Thread.Sleep(3000);

            expToApprove = repository.GetExpensesForApproval(stubManager);

            Assert.AreEqual(0, expToApprove.Count());

        }


        [TestMethod]
        public void CanGetExpenseByUserFromCache()
        {

            Guid expenseIdToSave = Guid.NewGuid();
            Assert.IsNull(util.DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expenseIdToSave));
            var stubUser = new User { UserName = "testUser" };
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

            var repository = new ExpenseRepository(TestDatabaseConnectionString, TimeSpan.MaxValue);
            repository.SaveExpense(expenseToSave);

            var expense = repository.GetExpensesByUser(stubUser.UserName);

            DatabaseHelper.BypassRepositoryAndDeleteExpense(TestDatabaseConnectionString, expenseToSave.Id);

            expense = repository.GetExpensesByUser(stubUser.UserName);

            Assert.AreEqual(1, expense.Count());

            Assert.AreEqual(expenseIdToSave, expense.First().Id);
        }

        [TestMethod]
        public void GetExpenseByUserIsGoneAfterCacheExpires()
        {

            Guid expenseIdToSave = Guid.NewGuid();
            Assert.IsNull(util.DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expenseIdToSave));
            var stubUser = new User { UserName = "testUser" };
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

            var repository = new ExpenseRepository(TestDatabaseConnectionString, new TimeSpan(0, 0, 2));
            repository.SaveExpense(expenseToSave);

            var expense = repository.GetExpensesByUser(stubUser.UserName);

            DatabaseHelper.BypassRepositoryAndDeleteExpense(TestDatabaseConnectionString, expenseToSave.Id);

            Thread.Sleep(3000);

            expense = repository.GetExpensesByUser(stubUser.UserName);

            Assert.AreEqual(0, expense.Count());

        }

        [TestMethod]
        public void CanGetExpenseByIdFromCache()
        {

            Guid expenseIdToSave = Guid.NewGuid();
            Assert.IsNull(util.DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expenseIdToSave));
            var stubUser = new User { UserName = "testUser" };
            var stubManager = "the manager";
            var expenseToSave = new Model.Expense
            {
                Id = expenseIdToSave,
                Date = new DateTime(1900, 01, 01),
                Title = "Title",
                Description = "new expense",
                Total = 1.0,
                ReimbursementMethod = ReimbursementMethod.DirectDeposit,
                CostCenter = "CostCenter",
                Approved = true,
                User = stubUser,

                ApproverName = stubManager
            };

            var repository = new ExpenseRepository(TestDatabaseConnectionString, new TimeSpan(1, 0, 0));
            repository.SaveExpense(expenseToSave);

            var expense = repository.GetExpenseById(expenseToSave.Id);

            DatabaseHelper.BypassRepositoryAndDeleteExpense(TestDatabaseConnectionString, expenseToSave.Id);

            expense = repository.GetExpenseById(expenseToSave.Id);

            Assert.IsNotNull(expense);

            Assert.AreEqual("new expense", expense.Description);
        }

        [TestMethod]
        public void GetExpenseByIdIsGoneAfterCacheExpires()
        {

            Guid expenseIdToSave = Guid.NewGuid();
            Assert.IsNull(util.DatabaseHelper.GetExpenseById(TestDatabaseConnectionString, expenseIdToSave));
            var stubUser = new User { UserName = "testUser" };
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

            var repository = new ExpenseRepository(TestDatabaseConnectionString, new TimeSpan(0, 0, 2));
            repository.SaveExpense(expenseToSave);

            var expense = repository.GetExpenseById(expenseToSave.Id);

            DatabaseHelper.BypassRepositoryAndDeleteExpense(TestDatabaseConnectionString, expenseToSave.Id);

            Thread.Sleep(3000);

            expense = repository.GetExpenseById(expenseToSave.Id);

            Assert.IsNull(expense);

        }

        [TestMethod]
        public void UpdatingAnExpenseUpdatesTheCache()
        {
            //Creation of expense
            Guid expenseIdToSave = Guid.NewGuid();
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
                Approved = false,
                User = stubUser,

                ApproverName = stubManager
            };

            //Get the repository with the highest cache expiration
            var repository = new ExpenseRepository(TestDatabaseConnectionString, TimeSpan.MaxValue);

            //Save expense
            repository.SaveExpense(expenseToSave);

            //Get expense (it also caches the expense)
            var savedExpense = repository.GetExpenseById(expenseIdToSave);

            //basic Assertion
            Assert.IsNotNull(savedExpense);
            
            //Approve the expense
            savedExpense.Approved = true;

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("test"), new string[] { "Manager" });
            repository.UpdateApproved(savedExpense);

            //Delete expense in db so we make sure we will get expense from cache.
            DatabaseHelper.BypassRepositoryAndDeleteExpense(TestDatabaseConnectionString, savedExpense.Id);

            //Get expense (from cache) and verify new description
            var cachedExpense = repository.GetExpenseById(expenseIdToSave);

            //Assert
            Assert.IsNotNull(cachedExpense);
            Assert.IsTrue(cachedExpense.Approved);
        }
    }
}
