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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.ConfigFiles;
using System.Xml;
using System.IO;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_settings_and_overrides
{
    [TestClass]
    public class when_overriding_protection_provider : given_logging_settings_and_overrides
    {
        private EnvironmentOverriddenElementProperty overridesProperty;

        protected override void Act()
        {
            var applicationModel = Container.Resolve<ApplicationViewModel>();
            applicationModel.NewEnvironment();


            overridesProperty = LoggingSectionViewModel.Properties.OfType<EnvironmentOverriddenElementProperty>().First();
        }

        [TestMethod]
        public void then_protection_provider_property_can_be_overridden()
        {
            Assert.IsTrue(overridesProperty.ChildProperties.Any(x => x.PropertyName == "Protection Provider"));
        }

        [TestMethod]
        public void then_require_permission_property_has_xpath_that_resolves()
        {
            Property requirePermissionProperty = LoggingSectionViewModel.Properties.Where(x => x.PropertyName == "Require Permission").Single();

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
    }
}
