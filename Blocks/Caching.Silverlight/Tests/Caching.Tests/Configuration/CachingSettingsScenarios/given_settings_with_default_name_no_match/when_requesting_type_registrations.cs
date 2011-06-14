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

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.CachingSettingsScenarios.given_settings_with_default_name_not_matching_the_cache_data
{
    [TestClass]
    public class when_requesting_type_registrations : Context
    {
        private IEnumerable<TypeRegistration> registrations;
        private IConfigurationSource expectedConfigurationSource;

        protected override void Act()
        {
            base.Act();

            this.expectedConfigurationSource = Mock.Of<IConfigurationSource>();
            this.registrations = this.settings.GetRegistrations(this.expectedConfigurationSource).ToArray();    // force the enumerable to be exercised
        }

        [TestMethod]
        public void then_gets_registration_from_cache_data()
        {
            Assert.AreEqual(1, this.registrations.Count());
            Assert.AreSame(this.expectedRegistration, this.registrations.First());
        }

        [TestMethod]
        public void then_registration_is_default()
        {
            Assert.IsFalse(this.registrations.First().IsDefault);
        }
    }
}
