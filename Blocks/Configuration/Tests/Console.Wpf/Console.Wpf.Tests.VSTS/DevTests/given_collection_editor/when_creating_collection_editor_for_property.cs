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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_collection_editor
{
    [TestClass]
    public class when_creating_collection_editor_for_property : given_logging_configuration.given_logging_configuration
    {

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void then_throws_invalid_opration_exception_if_no_collection_editor_templates_attribute_is_specified()
        {
            var loggingViewModel = SectionViewModel.CreateSection(base.Container, LoggingSettings.SectionName, LoggingSection);
            var property = loggingViewModel.CreateElementProperty(loggingViewModel, TypeDescriptor.GetProperties(loggingViewModel.ConfigurationElement).OfType<PropertyDescriptor>().Where(x => x.Name == "Formatters").First());
            
            CollectionElementEditor editor = new CollectionElementEditor();
            editor.DataContext = property;
        }

        [TestMethod]
        public void then_add_new_element_command_adds_new_element()
        {
            var loggingViewModel = SectionViewModel.CreateSection(base.Container, LoggingSettings.SectionName, LoggingSection);
            var categoryFilter = loggingViewModel.GetDescendentsOfType<CategoryFilterData>().First();
            var entriesProperty = categoryFilter.Property("CategoryFilters");

            var numberOfEntries = ((CategoryFilterData)categoryFilter.ConfigurationElement).CategoryFilters.Count;
            ((CollectionElementEditor)entriesProperty.Editor).NewCollectionElementCommand.Execute(null);
            Assert.AreEqual(numberOfEntries + 1, ((CategoryFilterData)categoryFilter.ConfigurationElement).CategoryFilters.Count);
            
        }
    }
}
