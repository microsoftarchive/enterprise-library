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

namespace AExpense.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class ModelExtensionsFixture
    {
        [TestMethod]
        public void IdIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expectedId = Guid.NewGuid();
            var expense = new Model.Expense
            {
                Id = expectedId,
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual(expectedId, expenseEntity.Id);
        }

        [TestMethod]
        public void AmountIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                Total = 100.0,
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual(100.0, Convert.ToDouble(expenseEntity.Amount));
        }

        [TestMethod]
        public void ApprovedIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                Approved = true,
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual(true, expenseEntity.Approved);
        }

        [TestMethod]
        public void CostCenterIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                CostCenter = "Cost Center",
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual("Cost Center", expenseEntity.CostCenter);
        }

        [TestMethod]
        public void DateIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expectedDate = new DateTime(2000, 1, 1);
            var expense = new Model.Expense
            {
                Date = expectedDate,
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual(expectedDate, expenseEntity.Date);
        }

        [TestMethod]
        public void DescriptionIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                Description = "Description",
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual("Description", expenseEntity.Description);
        }

        [TestMethod]
        public void ReimbursementMethodIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                ReimbursementMethod = ReimbursementMethod.Cash,
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual(Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.Cash), expenseEntity.ReimbursementMethod);
        }

        [TestMethod]
        public void TitleIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                Title = "Title",
                User = new User(),
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual("Title", expenseEntity.Title);
        }

        [TestMethod]
        public void UserNameIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                User = new User { UserName = "User Name" },
                ApproverName = string.Empty
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual("User Name", expenseEntity.UserName);
        }

        [TestMethod]
        public void ApproverIsCopiedWhenConvertingAnExpenseToAnExpenseEntity()
        {
            var expense = new Model.Expense
            {
                User = new User(),
                ApproverName = "Approver"
            };

            var expenseEntity = expense.ToEntity();

            Assert.AreEqual("Approver", expenseEntity.Approver);
        }

        [TestMethod]
        public void IdIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expectedId = Guid.NewGuid();
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                Id = expectedId,
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual(expectedId, expense.Id);
        }

        [TestMethod]
        public void AmountIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                Amount = 1.0m,
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual(1.0m, Convert.ToDecimal(expense.Total));
        }

        [TestMethod]
        public void ApprovedIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                Approved = true,
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual(true, expense.Approved);
        }

        [TestMethod]
        public void CostCenterIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                CostCenter = "Cost Center",
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual("Cost Center", expense.CostCenter);
        }

        [TestMethod]
        public void DateIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expectedDate = new DateTime(2000, 1, 1);
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                Date = expectedDate,
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual(expectedDate, expense.Date);
        }

        [TestMethod]
        public void DescriptionIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                Description = "Description",
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual("Description", expense.Description);
        }

        [TestMethod]
        public void ReimbursementMethodIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.Cash)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual(ReimbursementMethod.Cash, expense.ReimbursementMethod);
        }

        [TestMethod]
        public void TitleIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                Title = "Title",
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual("Title", expense.Title);
        }

        [TestMethod]
        public void UserNameIsCopiedWhenConvertingAnExpenseEntityToAnExpense()
        {
            var expenseEntity = new AExpense.DataAccessLayer.Expense
            {
                UserName = "UserName",
                ReimbursementMethod = Enum.GetName(typeof(ReimbursementMethod), ReimbursementMethod.NotSet)
            };

            var expense = expenseEntity.ToModel();

            Assert.AreEqual("UserName", expense.User.UserName);
        }
    }
}