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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.Tests.VSTS.TestSupport;
using System.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_configuration
{
    [TestClass]
    public class when_adding_new_key_value_element_to_app_settings_view_model : given_app_settings_configuration
    {
        private int numberOfKeyValuesInConfig;
        protected override void  Arrange()
        {
             base.Arrange();

            numberOfKeyValuesInConfig = base.AppSettings.Settings.Count;
        }

        protected override void Act()
        {
            var collectionElement = base.AppSettingsView.GetDescendentsOfType<KeyValueConfigurationCollection>().FirstOrDefault();
            var addCommand = collectionElement.Commands.Where(x => x.Placement == CommandPlacement.ContextAdd).First();
            addCommand.Execute(null);
        }

        [TestMethod]
        public void then_key_value_was_added_to_configuration()
        {
            Assert.AreEqual(numberOfKeyValuesInConfig + 1, base.AppSettings.Settings.Count);
        }

        [TestMethod]
        public void then_new_key_value_element_has_empty_value()
        {
            Assert.IsNotNull(base.AppSettings.Settings.OfType<KeyValueConfigurationElement>().Where(x=> string.IsNullOrEmpty(x.Value)).FirstOrDefault());
        }

        [TestMethod]
        public void then_new_key_value_element_has_key_set()
        {
            var addedKeyValueElement = base.AppSettings.Settings.OfType<KeyValueConfigurationElement>().Where(x => string.IsNullOrEmpty(x.Value)).FirstOrDefault();
            Assert.IsFalse(string.IsNullOrEmpty(addedKeyValueElement.Key));
        }

        
        [TestMethod]
        public void then_key_value_was_added_to_view()
        {
            var collectionElement = base.AppSettingsView.GetDescendentsOfType<KeyValueConfigurationCollection>().FirstOrDefault();
            Assert.AreEqual(numberOfKeyValuesInConfig + 1, collectionElement.GetDescendentsOfType<KeyValueConfigurationElement>().Count());
        }


        [TestMethod]
        public void then_new_key_value_element_has_empty_value_in_view()
        {
            var collectionElement = base.AppSettingsView.GetDescendentsOfType<KeyValueConfigurationCollection>().FirstOrDefault();
            Assert.IsNotNull(collectionElement.GetDescendentsOfType<KeyValueConfigurationElement>().Where(x=>string.IsNullOrEmpty((string)x.Property("Value").Value)).FirstOrDefault());
        }

    }
}
