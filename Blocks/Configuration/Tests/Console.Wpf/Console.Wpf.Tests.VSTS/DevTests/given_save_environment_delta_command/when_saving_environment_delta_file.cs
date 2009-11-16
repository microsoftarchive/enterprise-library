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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Moq;
using Microsoft.Win32;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_save_environment_delta_command
{
    [TestClass]
    public class when_saving_environment_delta_file_without_file_name : ContainerContext
    {
        EnvironmentalOverridesViewModel overridesModel;
        string targetFile;

        protected override void Arrange()
        {
            targetFile = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.dconfig", Guid.NewGuid()));

            base.Arrange();

            overridesModel = (EnvironmentalOverridesViewModel) SectionViewModel.CreateSection(Container, EnvironmentMergeSection.EnvironmentMergeData, new EnvironmentMergeSection
            {
                EnvironmentName = "environment"
            });

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

    [TestClass]
    public class when_saving_environment_delta_file_with_file_name : ContainerContext
    {
        EnvironmentalOverridesViewModel overridesModel;
        string targetFile;
        string mainFile;

        protected override void Arrange()
        {
            targetFile = string.Format("{0}.dconfig", Guid.NewGuid());
            mainFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "any.config");
            base.Arrange();

            StandAloneApplicationViewModel appModel = (StandAloneApplicationViewModel)Container.Resolve<IApplicationModel>();
            appModel.ConfigurationFilePath = mainFile;

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.NewEnvironment();

            sourceModel.Environments.First().EnvironmentDeltaFile = targetFile;

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Callback(() => Assert.Fail());
        }

        protected override void Act()
        {
            StandAloneApplicationViewModel appModel = (StandAloneApplicationViewModel)Container.Resolve<IApplicationModel>();
            appModel.Save();
        }

        [TestMethod]
        public void then_file_was_written()
        {
            Assert.IsTrue(File.Exists(targetFile));
        }

        [TestMethod]
        public void then_environmental_overrides_is_not_part_of_main_file()
        {
            FileConfigurationSource source = new FileConfigurationSource(mainFile);
            Assert.IsNull(source.GetSection(EnvironmentMergeSection.EnvironmentMergeData));
        }

        protected override void Teardown()
        {
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, targetFile));
            File.Delete(mainFile);
        }
    }
}
