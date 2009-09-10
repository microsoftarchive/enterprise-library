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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests.Configuration
{

    public abstract class Given_ExceptionTypeInConfigurationSourceBuilder : ArrangeActAssert
    {
        protected ConfigurationSourceBuilder configurationSourceBuilder;
        protected IExceptionConfigurationForExceptionType policy;
        protected IExceptionConfigurationAddExceptionHandlers exception;
        protected Type exceptionType = typeof(ArgumentException);

        protected string policyName = "Some Policy";

        protected override void Arrange()
        {

            configurationSourceBuilder = new ConfigurationSourceBuilder();
            policy = configurationSourceBuilder
                        .ConfigureExceptionHandling()
                            .GivenPolicyWithName(policyName);

            exception = policy.ForExceptionType(exceptionType);
        }

        protected ExceptionPolicyData GetExceptionPolicyData()
        {
            var source = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(source);

            return ((ExceptionHandlingSettings)source.GetSection(ExceptionHandlingSettings.SectionName))
                .ExceptionPolicies.Get(policyName);
        }

        protected ExceptionTypeData GetExceptionTypeData()
        {
            return GetExceptionPolicyData().ExceptionTypes.Where(x => x.Type == exceptionType).First();
        }
    }

    [TestClass]
    public class When_AddingExceptionShieldingHandlerToExceptionTypePassingNullType : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_ShieldExceptionForWcf_ThrowsArgumentNullException()
        {
            exception.ShieldExceptionForWcf(null, "Fault Contract Message");
        }
    }

    [TestClass]
    public class When_AddingExceptionShieldingHandlerToExceptionType : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            exception.ShieldExceptionForWcf(typeof(object), "Fault Contract Message");
        }

        [TestMethod]
        public void Then_ExceptionTypeContainsFaultHandler()
        {
            Assert.IsTrue(
                base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<FaultContractExceptionHandlerData>()
                .Any());
        }


        [TestMethod]
        public void Then_FaultHandlerHasAppropriateTypeAndMessage()
        {
            var shieldingExceptionHandler = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<FaultContractExceptionHandlerData>()
                .First();

            Assert.AreEqual(typeof(object).AssemblyQualifiedName, shieldingExceptionHandler.FaultContractType);
            Assert.AreEqual("Fault Contract Message", shieldingExceptionHandler.ExceptionMessage);
        }


        [TestMethod]
        public void Then_LoggingHandlerHasNoMappings()
        {
            var shieldingExceptionHandler = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<FaultContractExceptionHandlerData>()
                .First();

            Assert.AreEqual(0, shieldingExceptionHandler.PropertyMappings.Count);
        }
    }


    [TestClass]
    public class When_AddingExceptionShieldingHandlerWithPropertyMappingsToExceptionType : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            exception.ShieldExceptionForWcf(typeof(object), "Message")
                .MapProperty("prop2", "prop3")
                .MapProperty("prop1", "prop4");
        }

        [TestMethod]
        public void Then_LoggingHandlerHasMappings()
        {
            var shieldingExceptionHandler = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<FaultContractExceptionHandlerData>()
                .First();

            Assert.AreEqual(2, shieldingExceptionHandler.PropertyMappings.Count);
            Assert.IsTrue(shieldingExceptionHandler.PropertyMappings.Where(
                x => x.Name == "prop2" && x.Source == "prop3").Any());

            Assert.IsTrue(shieldingExceptionHandler.PropertyMappings.Where(
                x => x.Name == "prop1" && x.Source == "prop4").Any());
        }
    }

    [TestClass]
    public class When_AddingExceptionShieldingHandlerPasssingNullNameToPropertyMapping : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_MapProperty_ThrowsArgumentException()
        {
            exception.ShieldExceptionForWcf(typeof(object), "Message")
                .MapProperty(null, "prop3");
        }
    }
}
