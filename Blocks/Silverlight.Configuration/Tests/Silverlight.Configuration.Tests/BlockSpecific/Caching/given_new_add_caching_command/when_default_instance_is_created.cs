//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.Mocks;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching.given_new_add_caching_command
{
    [TestClass]
    public class when_default_instance_is_created : Context
    {
        protected override void Act()
        {
            ConfigurationSection = AddBlockCommand.CreateConfigurationSection();
        }

        [TestMethod]
        public void then_section_name_is_properly_setted()
        {
            Assert.AreEqual(AddBlockCommand.SectionName, CachingSettings.SectionName);
        }

        [TestMethod]
        public void then_default_configuration_section_is_properly_created()
        {
            var settings = ConfigurationSection as CachingSettings;

            Assert.IsNotNull(settings);
            Assert.AreEqual(1, settings.Caches.Count);

            var defaultInMemoryCache = settings.Caches.Get(0) as InMemoryCacheData;
            
            Assert.IsNotNull(defaultInMemoryCache);
            Assert.IsNotNull(defaultInMemoryCache.Name);
            Assert.AreEqual(TimeSpan.FromMinutes(2), defaultInMemoryCache.ExpirationPollingInterval);

            Assert.AreSame(defaultInMemoryCache.Name, settings.DefaultCache);
        }
    }

}
