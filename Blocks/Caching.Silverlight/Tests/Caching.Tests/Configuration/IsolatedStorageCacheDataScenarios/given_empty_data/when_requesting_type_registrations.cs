//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.IsolatedStorageCacheDataScenarios.given_empty_data
{
    [TestClass]
    public class when_requesting_type_registrations : Context
    {
        private IEnumerable<TypeRegistration> registrations;

        protected override void Act()
        {
            base.Act();

            this.registrations = this.data.GetRegistrations(null);
        }

        [TestMethod]
        public void then_registration_has_name()
        {
            Assert.AreEqual("test name", this.registrations.First().Name);
            Assert.IsTrue(this.registrations.First().IsPublicName);
        }

        [TestMethod]
        public void then_registration_has_default_parameters()
        {
            Assert.AreEqual(6, this.registrations.First().ConstructorParameters.Count());
            Assert.AreEqual("test name", ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(0)).Value);
            Assert.AreEqual(1024, ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(1)).Value);
            Assert.AreEqual(80, ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(2)).Value);
            Assert.AreEqual(60, ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(3)).Value);
            Assert.AreEqual(TimeSpan.FromMinutes(2), ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(4)).Value);
            Assert.IsInstanceOfType(((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(5)).Value, typeof(IsolatedStorageCacheEntrySerializer));
        }
    }
}
