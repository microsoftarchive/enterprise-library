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

using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{

    [TestClass]
    public class GivenCustomerFormatterDataRegistry
    {
        private TypeRegistration registration;
        private CustomFormatterData formatterData;

        [TestInitialize]
        public void Given()
        {
            formatterData = new CustomFormatterData("myName", typeof(MockCustomLogFormatter));
            formatterData.Attributes.Add("foo", "bar");
            registration = formatterData.GetRegistrations().First();
        }

        [TestMethod]
        public void ThenRegistryEntryMapsLogFormatterToProvidedTypeByName()
        {
            registration.AssertForServiceType(typeof(ILogFormatter))
                .ForName(formatterData.Name)
                .ForImplementationType(formatterData.Type);
        }

        [TestMethod]
        public void ThenConstructorParametersProvideAttributes()
        {
            NameValueCollection attributes;
            registration.AssertConstructor()
                .WithValueConstructorParameter<NameValueCollection>(out attributes)
                .VerifyConstructorParameters();


            CollectionAssert.AreEquivalent(
                attributes,
                ((ConstantParameterValue)registration.ConstructorParameters.ElementAt(0)).Value as NameValueCollection
                );
        }

        [TestMethod]
        public void ThenRegistrationIsTransient()
        {
            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }

}
