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
using Console.Wpf.Tests.VSTS.TestSupport;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;


namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_configuration
{
    [TestClass]
    public class when_saving_application_settings_after_adding_new_settings : given_app_settings_configuration
    {
        protected override void Act()
        {
            var collection = (ElementCollectionViewModel)base.AppSettingsView.GetDescendentsOfType<KeyValueConfigurationCollection>().FirstOrDefault();
            var newElement = collection.AddNewCollectionElement(typeof(KeyValueConfigurationElement));
            newElement.Property("Key").Value = "new-setting";
            newElement.Property("Value").Value = "value";

            var newElement2 = collection.AddNewCollectionElement(typeof(KeyValueConfigurationElement));
            newElement2.Property("Key").Value = "new-setting-2";
            newElement2.Property("Value").Value = "value";
      
            DesignDictionaryConfigurationSource saveSource = new DesignDictionaryConfigurationSource();
            AppSettingsView.Save(saveSource);
            savedSection = (AppSettingsSection)saveSource.GetSection("appSettings");
        }

        AppSettingsSection savedSection;

        [TestMethod]
        public void then_new_settings_appear_in_saved_section()
        {
            Assert.IsTrue(savedSection.Settings.OfType<KeyValueConfigurationElement>().Where(x => x.Key == "new-setting").Any());
        }
    }
}
