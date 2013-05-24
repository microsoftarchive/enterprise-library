//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS
{
    /// <summary>
    /// Summary description for ParameterInspectorFixture
    /// </summary>
    [TestClass]
    public class ParameterInspectorFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void CanCreateGivenValidOperation()
        {
            OperationDescription operation =
                GetOperationDescription("AddCustomer", typeof(ITestService));
            ValidationParameterInspector inspector = new ValidationParameterInspector(operation, string.Empty);
            Assert.AreEqual(1, inspector.InputValidators.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ValidationFault>))]
        public void ShouldThrowValidationFaultInBeforeCallOnInvalidInput()
        {
            string opName = "AddCustomer";
            ValidationParameterInspector inspector = GetInspector(opName, typeof(ITestService));

            AddCustomerRequest request = new AddCustomerRequest("Jane", "Doe", "Invalid SSN");
            inspector.BeforeCall(opName, new object[] { request });
        }

        [TestMethod]
        public void ShouldNotThrowOnValidInput()
        {
            string opName = "AddCustomer";
            ValidationParameterInspector inspector = GetInspector(opName, typeof(ITestService));

            AddCustomerRequest request = new AddCustomerRequest("Jane", "Doe", "987-65-4321");
            inspector.BeforeCall(opName, new object[] { request });
        }

        [TestMethod]
        public void ShouldPassMultipleInputValidations()
        {
            string opName = "PlaceOrder";
            ValidationParameterInspector inspector = GetInspector(opName, typeof(ITestService));

            string customerId = "AGoodCustomer";
            TaxInfo taxInfo = new TaxInfo("123434", "WA");
            ItemInfo itemInfo = new ItemInfo("Item002", "A Fictitious book", 2);
            CustomerDiscountInfo discountInfo = new CustomerDiscountInfo(customerId, .15);

            inspector.BeforeCall(opName,
                                 new object[] { customerId, taxInfo, itemInfo, discountInfo });
        }

        [TestMethod]
        public void ShouldHaveProperFailureReportsInFault()
        {
            string opName = "PlaceOrder";
            ValidationParameterInspector inspector = GetInspector(opName, typeof(ITestService));

            string customerId = "AGoodCustomer";
            TaxInfo taxInfo = new TaxInfo("123434", "Not a state");
            ItemInfo itemInfo = new ItemInfo("1", "A Fictitious book", -2);
            CustomerDiscountInfo discountInfo = new CustomerDiscountInfo(customerId, .15);

            try
            {
                inspector.BeforeCall(opName,
                                     new object[] { customerId, taxInfo, itemInfo, discountInfo });
                Assert.Fail("Should not have gotten here, was expecting ValidationFault");
            }
            catch (FaultException<ValidationFault> e)
            {
                ValidationFault yourFault = e.Detail;
                Assert.AreEqual(3, yourFault.Details.Count);
            }
        }

        [TestMethod]
        public void ShouldRunValidatorsSpecifiedAsParameterAttributes()
        {
            string opName = "LookupById";
            string implName = "LookupById";
            Type[] opParams = new Type[] { typeof(int), typeof(string) };
            ValidationParameterInspector inspector =
                GetInspector(opName, opParams, implName, typeof(ITestService));

            try
            {
                inspector.BeforeCall(opName, new object[] { -1, null });
                Assert.Fail("Expected exception did not occur");
            }
            catch (FaultException<ValidationFault> e)
            {
                ValidationFault yourFault = e.Detail;
                Assert.IsFalse(yourFault.IsValid);
                Assert.AreEqual(3, yourFault.Details.Count);
            }
        }

        [TestMethod]
        public void ShouldPassParameterValidationWithGoodInputs()
        {
            string opName = "LookupById";
            string implName = "LookupById";
            Type[] opParams = new Type[] { typeof(int), typeof(string) };
            ValidationParameterInspector inspector =
                GetInspector(opName, opParams, implName, typeof(ITestService));

            inspector.BeforeCall(opName, new object[] { 42, "My Text" });
        }

        #region Helper Factory methods

        static ValidationParameterInspector GetInspector(string opName,
                                                         Type contractType)
        {
            return GetInspector(opName, null, opName, contractType);
        }

        static ValidationParameterInspector GetInspector(
            string opName,
            Type[] operationParamTypes,
            Type contractType)
        {
            return GetInspector(opName, operationParamTypes, opName, contractType);
        }

        static ValidationParameterInspector GetInspector(
            string opName,
            Type[] operationParameters,
            string implementationName,
            Type contractType)
        {
            return new ValidationParameterInspector(
                GetOperationDescription(opName, operationParameters, implementationName,
                                        contractType), string.Empty);
        }

        static OperationDescription GetOperationDescription(
            string operationName,
            Type contractType)
        {
            return GetOperationDescription(operationName, null, operationName, contractType);
        }

        static OperationDescription GetOperationDescription(
            string operationName,
            Type[] operationParameters,
            string implementationName,
            Type contractType)
        {
            ContractDescription contract = new ContractDescription(contractType.Name);
            OperationDescription operation = new OperationDescription(operationName, contract);
            if (operationParameters == null)
            {
                operation.SyncMethod = contractType.GetMethod(implementationName);
            }
            else
            {
                operation.SyncMethod =
                    contractType.GetMethod(implementationName, operationParameters);
            }

            if (operation.SyncMethod == null)
            {
                throw new ArgumentException(
                    string.Format("No matching method {0} found", implementationName));
            }

            return operation;
        }

        #endregion
    }
}
