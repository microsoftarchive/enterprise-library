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
using System.Configuration;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_view_model
{
    public abstract class given_application_settings : ContainerContext
    {
        protected MockAppSettingsViewModel settings;

        protected override void Arrange()
        {
            base.Arrange();

            AppSettingsSection AppSettings = new AppSettingsSection();

            AppSettings.Settings.Add(new KeyValueConfigurationElement("Setting1", "Value1"));
            AppSettings.Settings.Add(new KeyValueConfigurationElement("Setting2", "Value2"));

            settings = new MockAppSettingsViewModel(Container, "appSettings", AppSettings);
            settings.ElementViewModelServiceDependencies(Container.Resolve<ElementLookup>(),
                                                         Container.Resolve<IApplicationModel>());
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
