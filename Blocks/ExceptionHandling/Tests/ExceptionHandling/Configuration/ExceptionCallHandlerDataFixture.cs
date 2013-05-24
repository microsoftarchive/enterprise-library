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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Configuration
{

    [TestClass]
    public class GivenAnExceptionCallHandlerData
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new ExceptionCallHandlerData("exception")
                {
                    Order = 400,
                    ExceptionPolicyName = "policy"
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationHasTransientLifetime()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registrations.Lifetime);
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForICallHandlerWithNameAndImplementationType()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("exception-suffix")
                .ForImplementationType(typeof(ExceptionCallHandler));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCallHandlerRegistrationInjectsConstructorParameters()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithContainerResolvedParameter<ExceptionPolicyImpl>("policy")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationInjectsOrderProperty()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertProperties()
                .WithValueProperty("Order", 400)
                .VerifyProperties();
        }
    }
}
