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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Console.Wpf.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;
using Console.Wpf.ViewModel.BlockSpecifics;

namespace Console.Wpf.Tests.VSTS.DevTests.given_environment_node_in_view_model
{
    [TestClass]
    public class when_adding_environment_node_to_view_model : ExceptionHandlingSettingsContext
    {
        SectionViewModel ehabModel;
        SectionViewModel environmentViewModel;

        protected override void  Act()
        {
            EnvironmentMergeSection mergeSection = new EnvironmentMergeSection()
                {
                    EnvironmentName = "environment"
                };

            ehabModel = SectionViewModel.CreateSection(ServiceProvider, Section);
            environmentViewModel = SectionViewModel.CreateSection(ServiceProvider, mergeSection);
        }

        [TestMethod]
        public void then_environment_model_is_extended_property_provider()
        {
            Assert.IsInstanceOfType(environmentViewModel, typeof(IElementExtendedPropertyProvider));
        }

        [TestMethod]
        public void then_wrap_handler_has_property_of_type_MergedConfigurationNode()
        {
            var aWrapHandler = ehabModel.DescendentElements().Where(x => x.ConfigurationType == typeof(WrapHandlerData)).First();
            Assert.IsTrue(aWrapHandler.Properties.Where(x => x.Type == typeof(OverriddenElementViewModel)).Any());
        }

        [TestMethod]
        public void then_wrap_handler_overrides_property_has_children_for_overridable_properties()
        {
            var aWrapHandler = ehabModel.DescendentElements().Where(x => x.ConfigurationType == typeof(WrapHandlerData)).First();
            var overridesProperty = aWrapHandler.Properties.Where(x => x.Type == typeof(OverriddenElementViewModel)).First();

            Assert.IsTrue(overridesProperty.HasChildProperties);
            Assert.IsTrue(overridesProperty.ChildProperties.Where(x => x.PropertyName == "ExceptionMessage").Any());
        }

        [TestMethod]
        public void then_wrap_handler_overrides_property_is_readonly()
        {
            var aWrapHandler = ehabModel.DescendentElements().Where(x => x.ConfigurationType == typeof(WrapHandlerData)).First();
            var overridesProperty = aWrapHandler.Properties.Where(x => x.Type == typeof(OverriddenElementViewModel)).First();

            Assert.IsTrue(overridesProperty.HasChildProperties);
            Assert.IsTrue(overridesProperty.ChildProperties.Where(x => x.PropertyName == "ExceptionMessage").First().ReadOnly);
        }
    }
}
