//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenBinaryLogFormatterTypeRegistrationEntry
    {
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            var formatterData = new BinaryLogFormatterData("formatterName");
            registration = formatterData.GetRegistrations().First();
        }

        [TestMethod]
        public void ThenRegistryMatchesServiceTypeNameAndImplementationType()
        {
            registration
                .AssertForServiceType(typeof(ILogFormatter))
                .ForName("formatterName")
                .ForImplementationType(typeof(BinaryLogFormatter));
        }

        [TestMethod]
        public void ThenNoConstructorParametersSupplied()
        {
            registration.AssertConstructor().VerifyConstructorParameters();
        }
    }

}
