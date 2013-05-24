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
using System.IO;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_open_environment_delta_command
{
    [TestClass]
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

            var openEnvironmentDelta = new OpenEnvironmentConfigurationDeltaCommand(
                Container.Resolve<IUIServiceWpf>(), Container.Resolve<ApplicationViewModel>(), true);

            openEnvironmentDelta.Execute(null);
        }

        [TestMethod]
        public void then_environment_is_loaded_in_source_model()
        {
            var applicationViewModel = Container.Resolve<ApplicationViewModel>();
            Assert.IsTrue(applicationViewModel.Environments.OfType<EnvironmentSourceViewModel>().Any(x => x.EnvironmentName == "Environment"));
        }

        [TestMethod]
        public void then_environment_file_is_set_to_source_file()
        {
            var applicationViewModel = Container.Resolve<ApplicationViewModel>();
            var environmentSection = applicationViewModel.Environments.OfType<EnvironmentSourceViewModel>().Single(x => x.EnvironmentName == "Environment");

            Assert.AreEqual(environmentDeltaFilePath, environmentSection.EnvironmentDeltaFile);
        }
        [TestMethod]
        public void then_last_saved_environment_file_is_set_to_source_file()
        {
            var applicationViewModel = Container.Resolve<ApplicationViewModel>();
            var environmentSection = applicationViewModel.Environments.OfType<EnvironmentSourceViewModel>().Single(x => x.EnvironmentName == "Environment");

            Assert.AreEqual(environmentDeltaFilePath, environmentSection.LastEnvironmentDeltaSavedFilePath);
        }
    }
}
