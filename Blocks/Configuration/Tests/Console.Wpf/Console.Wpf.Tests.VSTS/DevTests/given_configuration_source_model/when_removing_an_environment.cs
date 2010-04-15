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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source_model
{
    [TestClass]
    public class when_removing_an_environment : ContainerContext
    {
        protected override void Act()
        {
            var applicationModel = Container.Resolve<ApplicationViewModel>();
            applicationModel.NewEnvironment();
        }

        [TestMethod]
        public void then_environment_name_is_recalculated_correctly()
        {
            var applicationViewModel = Container.Resolve<ApplicationViewModel>();

            Assert.AreEqual("Environment", applicationViewModel.Environments.Last().Name);

            applicationViewModel.NewEnvironment();

            Assert.AreEqual("Environment 2", applicationViewModel.Environments.Last().Name);

            applicationViewModel.NewEnvironment();

            Assert.AreEqual("Environment 3", applicationViewModel.Environments.Last().Name);

            applicationViewModel.RemoveEnvironment(applicationViewModel.Environments.Single(e => e.Name == "Environment 2"));

            applicationViewModel.NewEnvironment();

            Assert.AreEqual("Environment 2", applicationViewModel.Environments.Last().Name);
        }
    }
}
