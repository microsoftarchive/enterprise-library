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
using Console.Wpf.Tests.VSTS.DevTests.given_shell_service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Win32;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_application_model
{
    [TestClass]
    public class when_saving_configuration_source : given_clean_appllication_model
    {
        string originalFileContents;
        protected override void Arrange()
        {
            originalFileContents = File.ReadAllText(TestConfigurationFilePath);
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = TestConfigurationFilePath });
            ApplicationModel.OpenConfigurationSource();
            
        }

        [TestMethod]
        public void then_removed_sections_are_removed_when_saving()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            var cachingSettings = sourceModel.Sections.Where(x => x.ConfigurationType == typeof(CacheManagerSettings)).First();
            cachingSettings.Delete();

            ApplicationModel.Save();

            FileConfigurationSource source = new FileConfigurationSource(TestConfigurationFilePath);
            Assert.IsNull(source.GetSection(CacheManagerSettings.SectionName));

        }

        protected override void Teardown()
        {
            File.WriteAllText(TestConfigurationFilePath, originalFileContents);
        }
    }
}
