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
    using System.Collections.Generic;
    using System.Linq;
    using aExpense.UnitTests.Properties;
    using AExpense.UnitTests.Util;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValidationFixture
    {
        private ValidatorFactory valFactory;
        private ValidatorFactory valFactoryFromConfig;
        private IUnityContainer container;

        [TestInitialize]
        public void Init()
        {
            IConfigurationSource config = ConfigurationSourceFactory.Create();
            this.valFactory = ValidationFactory.DefaultCompositeValidatorFactory;
            this.valFactoryFromConfig = ConfigurationValidatorFactory.FromConfigurationSource(config);
            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(this.container);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.container.Dispose();
        }

        [TestMethod]
        public void ExpenseIsOkValidationPasses()
        {
            var aExpenseValidator = this.valFactory.CreateValidator<Model.Expense>();

            var expenseToValidate = StubsHelper.GenerateExpenseStub();

            ValidationResults results = aExpenseValidator.Validate(expenseToValidate);

            Assert.IsTrue(results.IsValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TitleMustBeSuppliedValidation()
        {
            var aExpenseValidator = this.valFactory.CreateValidator<Model.Expense>();

            var expenseToValidate = StubsHelper.GenerateExpenseStub();

            expenseToValidate.Title = string.Empty;

            ValidationResults results = aExpenseValidator.Validate(expenseToValidate);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);

            var result = results.FirstOrDefault();

            Assert.IsNotNull(result);

            StringAssert.Equals(Resources.TitleMustBeSuppliedMessage, result.Message);
        }

        [TestMethod]
        public void UserShouldNotBeNullValidation()
        {
            var aExpenseValidator = this.valFactory.CreateValidator<Model.Expense>();

            var expenseToValidate = StubsHelper.GenerateExpenseStub();

            expenseToValidate.User = null;

            ValidationResults results = aExpenseValidator.Validate(expenseToValidate);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);

            var result = results.FirstOrDefault();

            Assert.IsNotNull(result);

            StringAssert.Equals(Resources.UserNotNullMessage, result.Message);
        }

        [TestMethod]
        public void CostCenterShouldNotBeNullValidation()
        {
            var aExpenseValidator = this.valFactory.CreateValidator<Model.Expense>();

            var expenseToValidate = StubsHelper.GenerateExpenseStub();

            expenseToValidate.CostCenter = null;

            ValidationResults results = aExpenseValidator.Validate(expenseToValidate);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);

            var result = results.FirstOrDefault();

            Assert.IsNotNull(result);

            StringAssert.Equals(Resources.CostCencerNotNullMessage, result.Message);
        }

        [TestMethod]
        public void ApproverNameMustBeSuppliedValidation()
        {
            var aExpenseValidator = this.valFactory.CreateValidator<Model.Expense>();

            var expenseToValidate = StubsHelper.GenerateExpenseStub();

            expenseToValidate.ApproverName = string.Empty;

            ValidationResults results = aExpenseValidator.Validate(expenseToValidate);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);

            var result = results.FirstOrDefault();

            Assert.IsNotNull(result);

            StringAssert.Equals(Resources.ApproverNameMustBeSuppliedMessage, result.Message);
        }

        [TestMethod]
        public void NotDuplicateExpenseDetailsValidation()
        {
            var aExpenseValidator = this.valFactoryFromConfig.CreateValidator<Model.Expense>("ExpenseRuleset");

            var expenseToValidate = StubsHelper.GenerateExpenseStub();

            expenseToValidate.Details = new List<Model.ExpenseItem>
            { 
                new Model.ExpenseItem() { Description = "Test" }, 
                new Model.ExpenseItem() { Description = "Test" }
            };

            ValidationResults results = aExpenseValidator.Validate(expenseToValidate);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);

            var result = results.FirstOrDefault();

            Assert.IsNotNull(result);

            StringAssert.Equals(Resources.DuplicateExpenseDetailsValidation, result.Message);
        }

        [TestMethod]
        public void ExpenseItemIsOkValidationPasses()
        {
            var aExpenseItemValidator = this.valFactory.CreateValidator<Model.ExpenseItem>();

            var expenseItemToValidate = StubsHelper.GenerateExpenseItemStub();

            ValidationResults results = aExpenseItemValidator.Validate(expenseItemToValidate);

            Assert.IsTrue(results.IsValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void DescriptionMustBeSuppliedInItemValidation()
        {
            var aExpenseItemValidator = this.valFactory.CreateValidator<Model.ExpenseItem>();

            var expenseItemToValidate = StubsHelper.GenerateExpenseItemStub();

            expenseItemToValidate.Description = string.Empty;

            ValidationResults results = aExpenseItemValidator.Validate(expenseItemToValidate);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);

            var result = results.FirstOrDefault();

            Assert.IsNotNull(result);

            StringAssert.Equals(Resources.DescriptionMustBeSuppliedMessage, result.Message);
        }

        [TestMethod]
        public void AmountMustBePositiveValidation()
        {
            var aExpenseItemValidator = this.valFactory.CreateValidator<Model.ExpenseItem>();

            var expenseItemToValidate = StubsHelper.GenerateExpenseItemStub();

            expenseItemToValidate.Amount = -1;

            ValidationResults results = aExpenseItemValidator.Validate(expenseItemToValidate);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Count);

            var result = results.FirstOrDefault();

            Assert.IsNotNull(result);

            StringAssert.Equals(Resources.AmountGreaterThanZeroMessage, result.Message);
        }
    }
}