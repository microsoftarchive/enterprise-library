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

using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS
{
    /// <summary>
    /// Summary description for WCFIntegrationFixture
    /// </summary>
    [TestClass]
    public class WCFIntegrationFixture
    {
        TestServiceHost<TestServiceImplementation, ITestService> host;

        [TestInitialize]
        public void Setup()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
            host =
                new TestServiceHost<TestServiceImplementation, ITestService>(
                    Settings.Default.TestServiceAddress);
        }

        [TestCleanup]
        public void TearDown()
        {
            if (host != null)
                host.Dispose();
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void CanCallService()
        {
            ITestService proxy = host.Proxy;

            string input = "This is my test string";
            string result = proxy.ToUpperCase(input);
            Assert.AreEqual(input.ToUpperInvariant(), result);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ValidationFault>))]
        public void FailsValidationWithInvalidInput()
        {
            ITestService proxy = host.Proxy;

            AddCustomerRequest request = new AddCustomerRequest();
            request.FirstName = "John";
            request.LastName = "Doe";
            request.SSN = "This is not a valid SSN";

            proxy.AddCustomer(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ShouldFailWithoutFaultContractAttribute()
        {
            host.Proxy.PlaceOrder("customer1", new TaxInfo(), new ItemInfo(), new CustomerDiscountInfo());
        }

        [TestMethod]
        public void ShouldGiveCorrectFaultDetails()
        {
            AddCustomerRequest request = new AddCustomerRequest("First", "Last", "Invalid SSN");

            try
            {
                host.Proxy.AddCustomer(request);
                Assert.Fail("If you got here, the validation exception didn't happen");
            }
            catch (FaultException<ValidationFault> e)
            {
                ValidationFault yourFault = e.Detail;
                Assert.IsFalse(yourFault.IsValid);
                Assert.AreEqual(1, yourFault.Details.Count);
                Assert.AreEqual("SSN", yourFault.Details[0].Key);
                Assert.AreEqual("request", yourFault.Details[0].Tag);
            }
        }
    }
}
