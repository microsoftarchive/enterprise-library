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
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS
{
    /// <summary>
    /// Tests to check if the ruleset specification in the ValidationBehavior atttribute
    /// work.
    /// </summary>
    [TestClass]
    public class RulesetFixture
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
        [ExpectedException(typeof(FaultException<ValidationFault>))]
        public void ShouldFailValidationWhenViolatingBank1Ruleset()
        {
            using (Banking1ServiceHost host = new Banking1ServiceHost())
            {
                CustomerInfo customer = GetInvalidBank1Customer();
                AccountSummary summary = host.Proxy.GetAccountSummary(customer);
            }
        }

        [TestMethod]
        public void ShouldPassValidationWhenPassingBank1Ruleset()
        {
            using (Banking1ServiceHost host = new Banking1ServiceHost())
            {
                CustomerInfo customer = GetValidBank1Customer();
                host.Proxy.GetAccountSummary(customer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ValidationFault>))]
        public void ShouldFailValidationWhenViolatingBank2Ruleset()
        {
            using (Banking2ServiceHost host = new Banking2ServiceHost())
            {
                CustomerInfo customer = GetValidBank1Customer();
                host.Proxy.GetAccountSummary(customer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ShouldAddValidationToEndpointViaConfiguration()
        {
            using (Banking3ServiceHost host = new Banking3ServiceHost())
            {
                CustomerInfo customer = GetValidBank1Customer();

                host.Proxy.GetAccountSummary(customer);
            }
        }

        [TestMethod]
        public void CanUseDifferentRulesetsForDifferentOperationsWhenUsingOperationBehavior()
        {
            using (Banking4ServiceHost host = new Banking4ServiceHost())
            {
                CustomerInfo customer1 = GetValidRule1Customer();
                CustomerInfo customer2 = GetValidRule2Customer();

                host.Proxy.GetAccountSummary(customer1);
                try
                {
                    host.Proxy.GetAccountSummary(customer2);
                    Assert.Fail("should have thrown");
                }
                catch (FaultException<ValidationFault>)
                {
                }
                try
                {
                    host.Proxy.GetAccountSummary2(customer1);
                    Assert.Fail("should have thrown");
                }
                catch (FaultException<ValidationFault>)
                {
                }
                host.Proxy.GetAccountSummary2(customer2);
            }
        }

        #region Various Customer Factories

        CustomerInfo GetInvalidBank1Customer()
        {
            CustomerInfo customer = new CustomerInfo();
            customer.FirstName = "FirstName";
            customer.LastName = null; // This violates the ruleset
            customer.Ssn = "Not valid, but that's ok, this ruleset doesn't care";
            return customer;
        }

        CustomerInfo GetValidBank1Customer()
        {
            CustomerInfo customer =
                new CustomerInfo("First", "LastName", "Illegal SSN, but Bank1 doesn't care");
            return customer;
        }


        CustomerInfo GetValidRule1Customer()
        {
            CustomerInfo customer = new CustomerInfo();
            customer.FirstName = "FirstName";
            customer.LastName = "LastName";
            customer.Ssn = "Not valid, but that's ok, this ruleset doesn't care";
            return customer;
        }

        CustomerInfo GetValidRule2Customer()
        {
            CustomerInfo customer = new CustomerInfo();
            customer.FirstName = "FirstName";
            customer.LastName = null; // ignored by this ruleset
            customer.Ssn = "333-22-4444";
            return customer;
        }

        #endregion
    }
}
