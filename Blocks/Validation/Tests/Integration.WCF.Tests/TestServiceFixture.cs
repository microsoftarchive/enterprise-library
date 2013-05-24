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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS
{
    /// <summary>
    /// Test fixture to make sure the test service is configured properly
    /// with validators.
    /// </summary>
    [TestClass]
    public class TestServiceFixture
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
        public void ShouldFailValidationWithInvalidSSN()
        {
            AddCustomerRequest request = new AddCustomerRequest("John", "Doe", "Not an SSN");
            Validator v = ValidationFactory.CreateValidator<AddCustomerRequest>();
            ValidationResults results = v.Validate(request);
            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void ShouldPassValidationWithValidSSN()
        {
            AddCustomerRequest request = new AddCustomerRequest("John", "Doe", "012-34-5678");
            Validator v = ValidationFactory.CreateValidator<AddCustomerRequest>();
            ValidationResults results = v.Validate(request);
            Assert.IsTrue(results.IsValid);
        }
    }
}
