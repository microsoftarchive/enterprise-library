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
using System.Text.RegularExpressions;
using System.Windows;

namespace Console.Wpf.Tests.VSTS.DevTests.given_save_environment_delta_command
{
    [TestClass]
    public class when_saving_environment_delta_file_with_file_name : ContainerContext
    {
        string targetFile;
        string mainFile;
        string environmentConfigFile;
        ApplicationViewModel applicationModel;

        protected override void Arrange()
        {
            targetFile = "empty.dconfig";
            environmentConfigFile = "empty.config";

            ResourceHelper<ConfigFileLocator> helper = new ResourceHelper<ConfigFileLocator>();
            helper.DumpResourceFileToDisk(targetFile);
            helper.DumpResourceFileToDisk(environmentConfigFile);

            mainFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "any.config");
            base.Arrange();

            applicationModel = (ApplicationViewModel)Container.Resolve<IApplicationModel>();
            applicationModel.ConfigurationFilePath = mainFile;

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            applicationModel.NewEnvironment();

            applicationModel.Environments.First().EnvironmentDeltaFile = targetFile;
            applicationModel.Environments.First().EnvironmentConfigurationFile = environmentConfigFile;

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Callback(() => Assert.Fail());

            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsRegex("overwrite", RegexOptions.None), It.IsAny<string>(), System.Windows.MessageBoxButton.OKCancel))
                .Returns(MessageBoxResult.Yes)
                .Verifiable();
        }

        protected override void Act()
        {
            ApplicationViewModel appModel = (ApplicationViewModel)Container.Resolve<IApplicationModel>();
            appModel.Save();
        }


        [TestMethod]
        public void then_user_was_prompted_to_overwrite_file()
        {
            UIServiceMock.Verify();
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
            Assert.IsNull(source.GetSection(EnvironmentalOverridesSection.EnvironmentallyOverriddenProperties));
        }

        [TestMethod]
        public void then_saving_again_doesnt_prompt_to_overwrite_file()
        {
            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsRegex("overwrite", RegexOptions.None), It.IsAny<string>(), System.Windows.MessageBoxButton.OKCancel))
                .Callback(() => Assert.Fail());

            applicationModel.Save();

        }

        protected override void Teardown()
        {
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, targetFile));
            File.Delete(mainFile);
        }
    }
}
