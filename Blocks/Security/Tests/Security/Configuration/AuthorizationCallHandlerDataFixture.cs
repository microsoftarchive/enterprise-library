//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration
{
    [TestClass]
    public class GivenAnAuthorizationCallHandlerData
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new AuthorizationCallHandlerData("authorization")
                    {
                        AuthorizationProvider = "provider",
                        OperationName = "operation",
                        Order = 200
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
        public void WhenCreatesRegistration_ThenRegistrationIsForCallHandlerForGivenName()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("authorization-suffix")
                .ForImplementationType(typeof(AuthorizationCallHandler));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCallHandlerRegistrationConfiguresInjectsExpirationTimeAndOrder()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithContainerResolvedParameter<IAuthorizationProvider>("provider")
                .WithValueConstructorParameter("operation")
                .WithValueConstructorParameter(200)
                .VerifyConstructorParameters();
        }
    }
}
