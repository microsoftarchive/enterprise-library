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

using System.Globalization;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source_model
{
    [TestClass]
    public class when_adding_new_environment : ContainerContext
    {
        protected override void Act()
        {
            var applicationModel = Container.Resolve<ApplicationViewModel>();
            applicationModel.NewEnvironment();
        }

        [TestMethod]
        public void then_environment_is_not_contained_in_sections()
        {
            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsFalse(sourceModel.Sections.Where(x=>x.ConfigurationType == typeof(EnvironmentalOverridesSection)).Any());
        }

        [TestMethod]
        public void then_environment_is_contained_in_environments()
        {
            var applicationViewModel = Container.Resolve<ApplicationViewModel>();
            Assert.IsTrue(applicationViewModel.Environments.Where(x=>x.ConfigurationType == typeof(EnvironmentalOverridesSection)).Any());
        }

        [TestMethod]
        public void then_environment_name_is_calculated_correctly()
        {
            const string EnvironmentName = "Environment";
            var applicationViewModel = Container.Resolve<ApplicationViewModel>();

            Assert.AreEqual(EnvironmentName, applicationViewModel.Environments.Last().Name);

            for (var i = 2; i < 5; i++)
            {
                applicationViewModel.NewEnvironment();

                string correctName = string.Format(CultureInfo.CurrentCulture,
                                                  "{0} {1}",
                                                  EnvironmentName,
                                                  i.ToString()).Trim();

                var environmentName = applicationViewModel.Environments.Last().Name;
                Assert.AreEqual(correctName, environmentName);
            }
        }
    }
}
