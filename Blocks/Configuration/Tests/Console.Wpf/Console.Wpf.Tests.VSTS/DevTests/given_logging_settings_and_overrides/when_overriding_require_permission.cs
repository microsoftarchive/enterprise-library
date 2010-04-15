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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Console.Wpf.Tests.VSTS.ConfigFiles;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using System.Xml;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_settings_and_overrides
{
    [TestClass]
    public class when_overriding_require_permission : given_logging_settings_and_overrides
    {
        private EnvironmentOverriddenElementProperty overridesProperty;

        protected override void Act()
        {
            var applicationModel = Container.Resolve<ApplicationViewModel>();
            applicationModel.NewEnvironment();

            overridesProperty = LoggingSectionViewModel.Properties.OfType<EnvironmentOverriddenElementProperty>().First();
        }

        [TestMethod]
        public void then_require_permission_property_can_be_overridden()
        {
            Assert.IsTrue(overridesProperty.ChildProperties.Any(x=>x.PropertyName == "Require Permission"));
        }

        [TestMethod]
        public void then_require_permission_property_has_xpath_that_resolves()
        {
            Property requirePermissionProperty = LoggingSectionViewModel.Properties.Where(x=>x.PropertyName == "Require Permission").Single();

            var resources = new ResourceHelper<ConfigFileLocator>();
            resources.DumpResourceFileToDisk("logging_require_permission.config");


            string configFileDir = AppDomain.CurrentDomain.BaseDirectory;
            string configFilePath = Path.Combine(configFileDir, "logging_require_permission.config");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(File.ReadAllText(configFilePath));


            var element = xmlDocument.SelectSingleNode(((IEnvironmentalOverridesProperty)requirePermissionProperty).ContainingElementXPath);
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Attributes[((IEnvironmentalOverridesProperty)requirePermissionProperty).PropertyAttributeName]);
        }

        [TestMethod]
        public void then_stored_value_is_lower_case()
        {
            Property requirePermissionProperty = LoggingSectionViewModel.Properties.Where(x=>x.PropertyName == "Require Permission").Single();

            overridesProperty.Value = true;

            var overriddenRequirePermissions = overridesProperty.ChildProperties.First(x => x.PropertyName == "Require Permission");
            overriddenRequirePermissions.Value = true;

            EnvironmentalOverridesSection overridesSection = (EnvironmentalOverridesSection)overridesProperty.Environment.ConfigurationElement;
            var allProperties = overridesSection.MergeElements.OfType<EnvironmentalOverridesElement>().SelectMany(x => x.OverriddenProperties.OfType<EnvironmentOverriddenPropertyElement>());

            var propertyForRequirePermissions = allProperties.Where(x => x.ContainingElementXPath == ((IEnvironmentalOverridesProperty)requirePermissionProperty).ContainingElementXPath).Single();
            Assert.AreEqual("true", propertyForRequirePermissions.OverriddenValue);
        }
    }
}
