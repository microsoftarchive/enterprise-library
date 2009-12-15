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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Moq;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console;

namespace Console.Wpf.Tests.VSTS.DevTests.given_environment_node_in_view_model
{
    [TestClass]
    public class when_exporting_merged_configuration : given_environmental_overrides_and_ehab
    {
        [TestMethod]
        public void then_command_is_accesible_through_envrionment_section_view_model()
        {
            Assert.IsTrue(base.EnvironmentViewModel.Commands.OfType<SaveMergedEnvironmentConfigurationCommand>().Any());
        }
    }

    [TestClass]
    public class when_exporting_merged_configuration_if_configuration_is_dirty : given_environmental_overrides_and_ehab
    {
        protected override void Arrange()
        {
            base.Arrange();

            IApplicationModel applicationModel = Container.Resolve<IApplicationModel>();
            applicationModel.SetDirty();

            UIServiceMock
                .Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), System.Windows.MessageBoxButton.OKCancel))
                .Returns(MessageBoxResult.OK)
                .Verifiable();
            UIServiceMock
                .Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                .Returns(new FileDialogResult { FileName = "save_as.config", DialogResult = false })
                .Verifiable();
        }

        protected override void Act()
        {
            SaveMergedEnvironmentConfigurationCommand saveEnvrionmentConfig = base.EnvironmentViewModel.Commands.OfType<SaveMergedEnvironmentConfigurationCommand>().First();
            saveEnvrionmentConfig.Execute(null);
        }

        [TestMethod]
        public void then_main_configuration_saved_dialog_was_shown()
        {
            UIServiceMock.Verify();
        }
    }

    [TestClass]
    public class when_exporting_merged_configuration_on_clean_configuration_model: given_environmental_overrides_and_ehab
    {
        protected override void Arrange()
        {
            base.Arrange();

            ApplicationViewModel appModel = (ApplicationViewModel)Container.Resolve<IApplicationModel>();
            appModel.ConfigurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.config");
            appModel.IsDirty = false ;
            
            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Returns(new FileDialogResult { DialogResult = true, FileName = "mergedconfig.config" })
                         .Verifiable();
        }

        protected override void Act()
        {
            SaveMergedEnvironmentConfigurationCommand saveEnvrionmentConfig = base.EnvironmentViewModel.Commands.OfType<SaveMergedEnvironmentConfigurationCommand>().First();
            saveEnvrionmentConfig.Execute(null);
        }

        [TestMethod]
        public void then_user_is_prompted_for_save_dialog()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_output_file_is_assigned_to_environment_merge_node()
        {
            Assert.AreEqual("mergedconfig.config", (string)EnvironmentViewModel.Property("EnvironmentConfigurationFile").Value);
        }

        [TestMethod]
        public void then_merged_configuration_file_is_saved_to_target()
        {
            string outputPath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "mergedconfig.config");
            Assert.IsTrue(File.Exists(outputPath));
        }

        protected override void Teardown()
        {
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mergedconfig.config"));
        }
    }

}
