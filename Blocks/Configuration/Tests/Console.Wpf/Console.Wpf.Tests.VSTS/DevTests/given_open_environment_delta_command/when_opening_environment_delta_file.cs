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
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Moq;
using Microsoft.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.DevTests.given_open_environment_delta_command
{
    [TestClass]
    [DeploymentItem("environment.dconfig")]
    public class when_opening_environment_delta_file : ContainerContext
    {
        private string environmentDeltaFilePath;

        protected override void Act()
        {
            environmentDeltaFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "environment.dconfig");

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<OpenFileDialog>()))
                         .Returns(new FileDialogResult
                         {
                             DialogResult = true,
                             FileName = environmentDeltaFilePath
                         });
            var openEnvironmentDelta = Container.Resolve<OpenEnvironmentConfigurationDeltaCommand>();
            openEnvironmentDelta.Execute(null);
        }

        [TestMethod]
        public void then_environment_is_loaded_in_source_model()
        {
            var configurationSource = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsTrue(configurationSource.Environments.OfType<EnvironmentalOverridesViewModel>().Any(x => x.EnvironmentName == "Environment"));
        }

        [TestMethod]
        public void then_environment_file_is_set_to_source_file()
        {
            var configurationSource = Container.Resolve<ConfigurationSourceModel>();
            var environmentSection = configurationSource.Environments.OfType<EnvironmentalOverridesViewModel>().Single(x => x.EnvironmentName == "Environment");

            Assert.AreEqual(environmentDeltaFilePath, environmentSection.EnvironmentDeltaFile);
        }
    }
}
