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
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;


namespace Console.Wpf.Tests.VSTS.DevTests.given_shell_service
{
    [TestClass]
    public class when_opening_source_with_redirected_sections : given_clean_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();
            string redirectedSectionsConfigurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "redirected_sections.config");
            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = redirectedSectionsConfigurationPath });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_configuration_model_doesnt_contain_redirected_sections()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsFalse(sourceModel.Sections.Where(x => x.ConfigurationType == typeof(ValidationSettings)).Any());
        }
    }

    [TestClass]
    public class when_opening_source_with_parent_source : given_clean_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();
            string hierarchicalConfigurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hierarchical_config.config");
            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = hierarchicalConfigurationPath });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_configuration_model_doesnt_contain_inherited_sections()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsFalse(sourceModel.Sections.Where(x => x.ConfigurationType == typeof(ValidationSettings)).Any());
        }
    }
}
