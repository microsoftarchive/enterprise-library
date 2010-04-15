//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class GivenACustomCallHandlerData
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new CustomCallHandlerData("custom", typeof(GlobalCountCallHandler))
                {
                    Order = 100,
                    Attributes = { { "foo", "bar" }, { "bar", "baz" } }
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenRegistrationMapsICallHandlerToCustomHandlerForName()
        {
            var registration = callHandlerData.GetRegistrations("-suffix");

            registration.ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("custom-suffix")
                .ForImplementationType(typeof(GlobalCountCallHandler));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenRegistrationInjectsAttributesThroughConstructor()
        {
            NameValueCollection attributes;

            var registration = callHandlerData.GetRegistrations("-suffix");

            registration.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out attributes)
                .VerifyConstructorParameters();

            Assert.AreEqual(2, attributes.Count);
            Assert.AreEqual("bar", attributes["foo"]);
            Assert.AreEqual("baz", attributes["bar"]);
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenRegistrationInjectsOrderProperty()
        {
            var registration = callHandlerData.GetRegistrations("-suffix");

            registration.ElementAt(0)
                .AssertProperties()
                .WithValueProperty("Order", 100)
                .VerifyProperties();
        }
    }
}
