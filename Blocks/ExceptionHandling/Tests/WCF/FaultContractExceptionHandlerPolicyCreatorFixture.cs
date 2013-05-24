//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class FaultContractExceptionHandlerPolicyCreatorFixture
    {
        private ExceptionHandlingSettings settings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            settings = new ExceptionHandlingSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(ExceptionHandlingSettings.SectionName, settings);
        }

        // test logic copied from FaultContractExceptionHandlerFixture.CanInjectAttributesIntoFaultContract()
        [TestMethod]
        public void CanCreatePoliciesForHandler()
        {
            ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
            settings.ExceptionPolicies.Add(exceptionPolicyData);

            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
            exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData);

            FaultContractExceptionHandlerData exceptionHandlerData = new FaultContractExceptionHandlerData("handler1",
                typeof(MockFaultContract).AssemblyQualifiedName);
            exceptionHandlerData.ExceptionMessage = "fault message";
            exceptionHandlerData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("Message", "{Message}"));
            exceptionHandlerData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("Data", "{Data}"));
            exceptionHandlerData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("SomeNumber", "{OffendingNumber}"));
            exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);

            var manager = settings.BuildExceptionManager();

            NotFiniteNumberException originalException = new NotFiniteNumberException("MyException", 12341234123412);
            originalException.Data.Add("someKey", "someValue");
            try
            {
                manager.HandleException(originalException, "policy");
                Assert.Fail("a new exception should have been thrown");
            }
            catch (FaultContractWrapperException e)
            {
                MockFaultContract fault = (MockFaultContract)e.FaultContract;
                Assert.AreEqual(originalException.Message, fault.Message);
                Assert.AreEqual(originalException.Data.Count, fault.Data.Count);
                Assert.AreEqual(originalException.Data["someKey"], fault.Data["someKey"]);
                Assert.AreEqual(originalException.OffendingNumber, fault.SomeNumber);
            }
        }
    }
}
