//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationManageabilityProviderFixture
    {
        [TestMethod]
        public void SubProvidersAreSetForNewInstance()
        {
            MockConfigurationElementManageabilityProvider subProvider1 = new MockConfigurationElementManageabilityProvider();
            MockConfigurationElementManageabilityProvider subProvider2 = new MockConfigurationElementManageabilityProvider();

            IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(String), subProvider1);
            subProviders.Add(typeof(Boolean), subProvider2);

            MockConfigurationSectionManageabilityProvider provider = new MockConfigurationSectionManageabilityProvider(subProviders);
            Assert.IsNull(provider.GetSubProvider(typeof(Int32)));
            Assert.AreSame(subProvider1, provider.GetSubProvider(typeof(String)));
            Assert.AreSame(subProvider2, provider.GetSubProvider(typeof(Boolean)));
        }
    }
}
