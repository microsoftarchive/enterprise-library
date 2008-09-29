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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class PolicyInjectionSettingsFixture
    {
        [TestMethod]
        [DeploymentItem("OldStyle.config")]
        public void SkipsInjectorsElement()
        {
            IConfigurationSource source = new FileConfigurationSource("OldStyle.config");

            PolicyInjectionSettings settings
                = (PolicyInjectionSettings)source.GetSection(PolicyInjectionSettings.SectionName);

            Assert.IsNotNull(settings);
            Assert.AreEqual(3, settings.Policies.Count);
        }
    }
}
