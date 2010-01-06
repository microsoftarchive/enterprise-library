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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using System.Configuration;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_view_model
{
    [TestClass]
    public class when_getting_all_properties : ContainerContext
    {
        IEnumerable<Property> properties;
        MockAppSettingsViewModel settings;

        protected override void Arrange()
        {
            base.Arrange();

            AppSettingsSection AppSettings = new AppSettingsSection();

            AppSettings.Settings.Add(new KeyValueConfigurationElement("Setting1", "Value1"));
            AppSettings.Settings.Add(new KeyValueConfigurationElement("Setting2", "Value2"));

            settings = new MockAppSettingsViewModel(Container, "appSettings", AppSettings);
        }

        protected override void Act()
        {
            properties = settings.GetProperties();
        }

        [TestMethod]
        public void then_required_permission_property_should_be_filtered()
        {
            var count = properties.OfType<RequirePermissionProperty>().Count();

            Assert.AreEqual(0, count);
        }
    }

    public class MockAppSettingsViewModel : AppSettingsViewModel
    {
        public MockAppSettingsViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
        }

        public IEnumerable<Property> GetProperties()
        {
            return base.GetAllProperties();
        }
    }
}
