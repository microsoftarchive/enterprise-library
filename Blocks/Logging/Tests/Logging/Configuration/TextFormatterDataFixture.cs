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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenTextFormatterDataSection
    {
        private TypeRegistration registration;
        private TextFormatterData formatter;


        [TestInitialize]
        public void Given()
        {
            formatter = new TextFormatterData("formatterName", "someTemplate");

            registration = formatter.GetRegistrations().First();
        }

        [TestMethod]
        public void ThenShouldProvideProperRegistrationEntry()
        {
            registration.AssertForServiceType(typeof(ILogFormatter))
                .ForName(formatter.Name)
                .ForImplementationType(typeof(TextFormatter));
        }

        [TestMethod]
        public void ThenShouldProviderProperConstructorParameters()
        {
            registration.AssertConstructor()
                .WithValueConstructorParameter<string>(formatter.Template);
        }

        [TestMethod]
        public void ThenShouldHaveATransientLifetime()
        {
            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
}
