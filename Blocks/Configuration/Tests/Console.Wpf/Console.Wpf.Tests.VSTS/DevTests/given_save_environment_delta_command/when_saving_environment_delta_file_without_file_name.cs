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
using Console.Wpf.Tests.VSTS.ConfigFiles;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_save_environment_delta_command
{
    [TestClass]
    public class when_saving_environment_delta_file_without_file_name : ContainerContext
    {
        EnvironmentSourceViewModel overridesModel;
        string targetFile;

        protected override void Arrange()
        {
            targetFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.dconfig", Guid.NewGuid()));

            base.Arrange();

            overridesModel = (EnvironmentSourceViewModel)SectionViewModel.CreateSection(Container, EnvironmentalOverridesSection.EnvironmentallyOverriddenProperties, new EnvironmentalOverridesSection
            {
                EnvironmentName = "environment"
            });

            ApplicationViewModel applicationModel = Container.Resolve<ApplicationViewModel>();
            applicationModel.ConfigurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "main.config");


            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Returns(new FileDialogResult { FileName = targetFile, DialogResult = true })
                         .Verifiable();


        }

        protected override void Act()
        {
            var command = overridesModel.CreateCommand<SaveEnvironmentConfigurationDeltaCommand>(overridesModel);
            command.Execute(null);
        }

        [TestMethod]
        public void then_save_dialog_was_shown()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_file_was_written()
        {
            Assert.IsTrue(File.Exists(targetFile));
        }

        protected override void Teardown()
        {
            File.Delete(targetFile);
        }
    }

}
