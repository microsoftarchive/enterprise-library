//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Tests
{
    [TestFixture]
    public class SecurityConfigurationDesignManagerFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void BuildContextTest()
        {
            SecuritySettingsNode node = new SecuritySettingsNode(new SecuritySettings());
            GeneratedApplicationNode.Nodes.Add(node);
            SecurityConfigurationDesignManager b = new SecurityConfigurationDesignManager();
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            b.BuildContext(Host, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.Contains(SecuritySettings.SectionName));
        }

        [Test]
        public void MakeSureCanCreateAuthorizationRuleNodeFromAuthorizationRuleProviderData()
        {
            SecurityConfigurationDesignManager manager = new SecurityConfigurationDesignManager();
            manager.Register(Host);
            ConfigurationNode node = NodeCreationService.CreateNode(typeof(AuthorizationRuleProviderData));
            Assert.IsNotNull(node);
        }
    }
}

#endif
