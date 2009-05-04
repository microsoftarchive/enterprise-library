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

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class SecurityCacheProviderFactoryFixture
    {
        DictionaryConfigurationSource GetConfigurationSource()
        {
            DictionaryConfigurationSource sections = new DictionaryConfigurationSource();

            SecuritySettings securityConfig = new SecuritySettings();
            securityConfig.DefaultSecurityCacheProviderName = "provider1";
            securityConfig.SecurityCacheProviders.Add(new MockSecurityCacheProviderData("provider1"));
            sections.Add(SecuritySettings.SectionName, securityConfig);

            InstrumentationConfigurationSection instrumentationConfig = new InstrumentationConfigurationSection(true, true, true, "fooApplicationName");
            sections.Add(InstrumentationConfigurationSection.SectionName, instrumentationConfig);

            return sections;
        }

        [TestMethod]
        public void CanCreateDefaultSecurityCache()
        {
            SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(GetConfigurationSource());
            ISecurityCacheProvider provider = factory.CreateDefault();
            Assert.AreEqual(typeof(MockSecurityCacheProvider), provider.GetType());
        }

        [TestMethod]
        public void CanCreateSecurityCacheProvider()
        {
            SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(GetConfigurationSource());
            ISecurityCacheProvider provider = factory.Create("provider1");
            Assert.AreEqual(typeof(MockSecurityCacheProvider), provider.GetType());
        }

        [TestMethod]
        public void CreatedProviderHasCorrectInstrumentationListener()
        {
            SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(GetConfigurationSource());
            ISecurityCacheProvider provider = factory.Create("provider1");

            Assert.AreEqual(typeof(MockSecurityCacheProvider), provider.GetType());
            object instrumentationProvider = ((MockSecurityCacheProvider)provider).GetInstrumentationEventProvider();
            Assert.AreSame(typeof(SecurityCacheProviderInstrumentationProvider), instrumentationProvider.GetType());
            SecurityCacheProviderInstrumentationProvider castedInstrumentationProvider = (SecurityCacheProviderInstrumentationProvider)instrumentationProvider;

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                IToken token = new GuidToken();
                castedInstrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Identity, token);
                eventWatcher.WaitForEvents();

                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("SecurityCacheReadPerformedEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual(SecurityEntityType.Identity.ToString(), eventWatcher.EventsReceived[0].Properties["EntityType"].Value);
                Assert.AreEqual("provider1", eventWatcher.EventsReceived[0].Properties["InstanceName"].Value);
                Assert.AreEqual(token.Value, eventWatcher.EventsReceived[0].Properties["TokenUsed"].Value);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToCreateSecurityCacheProviderFromConfigurationWithNullName()
        {
            SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(GetConfigurationSource());
            ISecurityCacheProvider provider = factory.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void TryToCreateSecurityCacheProviderFromConfigurationThatDoesNotExist()
        {
            SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(GetConfigurationSource());
            ISecurityCacheProvider provider = factory.Create("provider3");
        }
    }
}
