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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.Mocks;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_configuration
{
    [TestClass]
    public class when_saving_application_settings : given_app_settings_configuration
    {
        DesignDictionaryConfigurationSource saveSource = new DesignDictionaryConfigurationSource();
            
        protected override void Act()
        {
            AppSettingsView.Save(saveSource);
        }

        [TestMethod]
        public void then_all_settings_are_saved_in_target_source()
        {
            AppSettingsSection savedSection = (AppSettingsSection)saveSource.GetSection("appSettings");
            Assert.AreEqual(base.AppSettings.Settings.Count, savedSection.Settings.Count);
            foreach (KeyValueConfigurationElement elementInSource in base.AppSettings.Settings)
            {
                Assert.IsTrue(savedSection.Settings.OfType<KeyValueConfigurationElement>().Where(x => x.Key == elementInSource.Key && x.Value == elementInSource.Value).Any());
            }
        }
    }
}
