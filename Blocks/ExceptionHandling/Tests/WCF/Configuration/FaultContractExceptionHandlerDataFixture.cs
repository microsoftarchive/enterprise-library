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
}
