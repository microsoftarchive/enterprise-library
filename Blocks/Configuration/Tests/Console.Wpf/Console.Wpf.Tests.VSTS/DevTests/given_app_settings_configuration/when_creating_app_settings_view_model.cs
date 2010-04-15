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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Console.Wpf.Tests.VSTS.TestSupport;
using System.ComponentModel;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_configuration
{
    public abstract class given_app_settings_configuration : ContainerContext
    {
        protected AppSettingsSection AppSettings;
        protected SectionViewModel AppSettingsView;
        
        protected override void Arrange()
        {
            base.Arrange();

            AppSettings = new AppSettingsSection();
            AppSettings.Settings.Add(new KeyValueConfigurationElement("Setting1", "Value1"));
            AppSettings.Settings.Add(new KeyValueConfigurationElement("Setting2", "Value2"));

            AppSettingsView = SectionViewModel.CreateSection(Container, "appSettings", AppSettings);
        }
    }

    [TestClass]
    public class when_creating_app_settings_view_model : given_app_settings_configuration
    {
        [TestMethod]
        public void then_appsetting_collection_has_display_name_attribute()
        {
            var KeyValueCollection = AppSettingsView.GetDescendentsOfType<KeyValueConfigurationCollection>().First();
            Assert.IsTrue(KeyValueCollection.Attributes.OfType<DisplayNameAttribute>().Any());
        }

        [TestMethod]
        public void then_appsetting_key_property_has_display_name_attribute()
        {
            var KeyValueElement = AppSettingsView.GetDescendentsOfType<KeyValueConfigurationElement>().First();
            var KeyProperty = KeyValueElement.Property("Key");
            Assert.IsTrue(KeyProperty.Attributes.OfType<DisplayNameAttribute>().Any());
        }

        [TestMethod]
        public void then_key_property_is_writeable()
        {
            var KeyValueElement = AppSettingsView.GetDescendentsOfType<KeyValueConfigurationElement>().First();
            var KeyProperty = KeyValueElement.Property("Key");
            KeyProperty.Value = "new value";
            Assert.AreEqual("new value", KeyProperty.Value);
        }

        [TestMethod]
        public void then_name_contains_key_property_value()
        {
            var KeyValueElement = AppSettingsView.GetDescendentsOfType<KeyValueConfigurationElement>().First();
            var KeyProperty = KeyValueElement.Property("Key");
            Assert.IsTrue(KeyValueElement.Name.Contains((string)KeyProperty.Value));
        }

        [TestMethod]
        public void then_can_create_new_app_setting()
        {
            var KeyValueCollection = AppSettingsView.GetDescendentsOfType<KeyValueConfigurationCollection, ElementCollectionViewModel>().First();
            var element = KeyValueCollection.AddNewCollectionElement(typeof(KeyValueConfigurationElement));
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void then_app_settings_have_hierarchical_viewmodel()
        {
            Assert.IsNotNull(AppSettingsView as AppSettingsViewModel);
        }
    }
}
