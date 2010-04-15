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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Console.Wpf.Tests.VSTS.ConfigFiles;

namespace Console.Wpf.Tests.VSTS.DevTests.given_environment_node_in_view_model
{
    public abstract class given_environmental_overrides_and_ehab : ExceptionHandlingSettingsContext
    {
        protected SectionViewModel EhabModel;
        protected SectionViewModel EnvironmentViewModel;

        protected ElementViewModel WrapHandler;
        protected Property OverridesProperty;

        protected Property OverriddenExceptionMessage;
        protected Property MainExceptionMessage;

        protected EnvironmentalOverridesSection EnvironmentSection;

        protected override void Arrange()
        {
            base.Arrange();

            var resources = new ResourceHelper<ConfigFileLocator>();
            resources.DumpResourceFileToDisk("empty.config");

            var applicationViewModel = Container.Resolve<ApplicationViewModel>();
            ConfigurationSourceModel sourceModel = applicationViewModel.CurrentConfigurationSource;
            applicationViewModel.NewEnvironment();

            EhabModel = sourceModel.AddSection(ExceptionHandlingSettings.SectionName, Section);
            EnvironmentViewModel = applicationViewModel.Environments.First();
            EnvironmentSection = (EnvironmentalOverridesSection)EnvironmentViewModel.ConfigurationElement;

            ((EnvironmentSourceViewModel)EnvironmentViewModel).EnvironmentConfigurationFile = "empty.config";
            ((EnvironmentSourceViewModel)EnvironmentViewModel).EnvironmentDeltaFile = "empty.config";

            WrapHandler = EhabModel.DescendentElements().Where(x => x.ConfigurationType == typeof(WrapHandlerData)).First();

            MainExceptionMessage = WrapHandler.Property("ExceptionMessage");
            MainExceptionMessage.Value = "Main Value";

            OverridesProperty = WrapHandler.Properties.Where(x => x.PropertyName.StartsWith("Overrides")).First(); 
            OverriddenExceptionMessage = OverridesProperty.ChildProperties.Where(x => x.PropertyName == "ExceptionMessage").First();
        }
    }


    [TestClass]
    public class when_attaching_empty_overrides : given_environmental_overrides_and_ehab
    {
        [TestMethod]
        public void then_environment_section_delta_file_property_had_logical_parent_element()
        {
            var environmentDeltaFileProperty = (ILogicalPropertyContainerElement)EnvironmentViewModel.Property("EnvironmentDeltaFile");
            Assert.IsNotNull(environmentDeltaFileProperty);
            Assert.IsNotNull(environmentDeltaFileProperty.ContainingElement);
            Assert.IsNotNull(environmentDeltaFileProperty.ContainingElementDisplayName);
        }

        [TestMethod]
        public void then_environment_model_is_extended_property_provider()
        {
            Assert.IsInstanceOfType(EnvironmentViewModel, typeof(IElementExtendedPropertyProvider));
        }

        [TestMethod]
        public void then_wrap_handler_has_property_of_type_MergedConfigurationNode()
        {
            var aWrapHandler = EhabModel.DescendentElements().Where(x => x.ConfigurationType == typeof(WrapHandlerData)).First();
            Assert.IsTrue(aWrapHandler.Properties.Where(x => x.PropertyName.StartsWith("Overrides")).Any());
        }

        [TestMethod]
        public void then_wrap_handler_overrides_property_has_children_for_overridable_properties()
        {
            Assert.IsTrue(OverridesProperty.HasChildProperties);
            Assert.IsTrue(OverridesProperty.ChildProperties.Where(x => x.PropertyName == "ExceptionMessage").Any());
        }

        [TestMethod]
        public void then_wrap_handler_overrides_property_is_readonly()
        {
            Assert.IsTrue(OverriddenExceptionMessage.BindableProperty.ReadOnly);
        }

        [TestMethod]
        public void then_overrides_property_returns_dont_override()
        {
            string displayValueInGrid = OverridesProperty.Converter.ConvertToString(OverridesProperty.Value);
            Assert.AreEqual("Don't Override Properties", displayValueInGrid);
        }

        [TestMethod]
        public void then_overrides_property_itself_is_not_overridable()
        {
            Assert.IsFalse(OverridesProperty.ChildProperties.OfType<EnvironmentOverriddenElementProperty>().Any());
        }

        [TestMethod]
        public void then_name_property_is_not_overridable()
        {
            Assert.IsFalse(OverridesProperty.ChildProperties.Where(x => x.PropertyName == "Name").Any());
        }

        [TestMethod]
        public void then_designtimereadonly_property_is_not_overridable()
        {
            Assert.IsFalse(OverridesProperty.ChildProperties.Where(x => x.PropertyName == "TypeName").Any());
        }

        [TestMethod]
        public void then_overridden_property_has_same_value_as_main()
        {
            Assert.AreEqual(MainExceptionMessage.Value, OverriddenExceptionMessage.Value);
        }

        [TestMethod]
        public void then_overridden_value_changed_if_main_is_changed()
        {
            using (PropertyChangedListener listener = new PropertyChangedListener(OverriddenExceptionMessage))
            {
                MainExceptionMessage.Value = "New Value";

                Assert.IsTrue(listener.ChangedProperties.Contains("Value"));
                Assert.AreEqual("New Value", OverriddenExceptionMessage.Value);
            }
        }
    }
}
