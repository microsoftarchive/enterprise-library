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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests.Configuration
{
    [TestClass]
    public class GivenContractExceptionHandlerRegistrations : ArrangeActAssert
    {
        private IEnumerable<TypeRegistration> registrations;
        private FaultContractExceptionHandlerData configuration;

        protected override void Arrange()
        {
            configuration = new FaultContractExceptionHandlerData();
        }

        protected override void Act()
        {
            registrations = configuration.GetRegistrations("");
        }

        [TestMethod]
        public void ThenLoggingExceptionHandlerHasTransientLifetime()
        {
            TypeRegistration faultContractExceptionHandlerRegistration = registrations.Where(x => x.ServiceType == typeof(IExceptionHandler)).First();
            Assert.AreEqual(TypeRegistrationLifetime.Transient, faultContractExceptionHandlerRegistration.Lifetime);
        }
    }

    [TestClass]
    public class GivenFaultContractExceptionHandlerResourceName : ArrangeActAssert
    {
        private TypeRegistration typeRegistration;
        private FaultContractExceptionHandlerData configuration;

        protected override void Arrange()
        {
            configuration = new FaultContractExceptionHandlerData("wcf")
            {
                ExceptionMessageResourceName = "WcfMessageResource",
                ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName
            };
        }

        protected override void Act()
        {
            typeRegistration = configuration.GetRegistrations("prefix").First();
        }

        [TestMethod]
        public void WhenGettingRegistrationMessageIsBroughtFromResource()
        {
            IStringResolver resolver;
            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.wcf")
                .ForImplementationType(typeof(FaultContractExceptionHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(out resolver);

            Assert.AreEqual(Resources.WcfMessageResource, resolver.GetString());
        }
    }

    [TestClass]
    public class GivenFaultContractExceptionHandlerWithExceptionMessage : ArrangeActAssert
    {
        private TypeRegistration typeRegistration;
        private FaultContractExceptionHandlerData configuration;
        private const string ErrorMessage = "Exception message";
        protected override void Arrange()
        {
            configuration = new FaultContractExceptionHandlerData("wcf")
            {
                ExceptionMessage = ErrorMessage
            };
        }

        protected override void Act()
        {
            typeRegistration = configuration.GetRegistrations("prefix").First();
        }

        [TestMethod]
        public void WhenGettingRegistrationMessageIsShowCorrectly()
        {
            IStringResolver resolver;
            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.wcf")
                .ForImplementationType(typeof(FaultContractExceptionHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(out resolver);

            Assert.AreEqual(ErrorMessage, resolver.GetString());
        }
    }

    [TestClass]
    public class GivenFaultContractExceptionHandlerWithOverridenMessage : ArrangeActAssert
    {
        private TypeRegistration typeRegistration;
        private FaultContractExceptionHandlerData configuration;
        private const string ErrorMessage = "Exception message";
        protected override void Arrange()
        {
            configuration = new FaultContractExceptionHandlerData("wcf")
            {
                ExceptionMessage = ErrorMessage,
                ExceptionMessageResourceName = "WcfMessageResource",
                ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName
            };
        }

        protected override void Act()
        {
            typeRegistration = configuration.GetRegistrations("prefix").First();
        }

        [TestMethod]
        public void WhenGettingRegistrationMessageIsOverriden()
        {
            IStringResolver resolver;
            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.wcf")
                .ForImplementationType(typeof(FaultContractExceptionHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(out resolver);

            Assert.AreEqual(Resources.WcfMessageResource, resolver.GetString());
        }
    }
}
