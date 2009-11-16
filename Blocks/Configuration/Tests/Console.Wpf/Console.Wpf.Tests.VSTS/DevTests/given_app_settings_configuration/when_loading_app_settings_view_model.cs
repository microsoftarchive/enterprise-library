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

namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_configuration
{
    [TestClass]
    public class when_loading_app_settings_view_model : given_app_settings_configuration
    {
        [TestMethod]
        public void then_all_application_settings_have_corresponding_element_view_model()
        {
            foreach (KeyValueConfigurationElement keyValueElement in base.AppSettings.Settings)
            {
                var elementViewModel = base.AppSettingsView.GetDescendentsOfType<KeyValueConfigurationElement>().Where(x=>x.Name.Contains(keyValueElement.Key)).FirstOrDefault();
                Assert.IsNotNull(elementViewModel);
                Assert.AreEqual(keyValueElement.Key, (string)elementViewModel.Property("Key").Value);
                Assert.AreEqual(keyValueElement.Value, (string)elementViewModel.Property("Value").Value);
            }
        }
    }
}
