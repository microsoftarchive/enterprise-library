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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using System.IO;
using Console.Wpf.Tests.VSTS.TestSupport;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;


namespace Console.Wpf.Tests.VSTS.DevTests.given_shell_service
{
    [TestClass]
    [DeploymentItem("redirected_sections.config")]
    public class when_opening_source_with_redirected_sections : given_clean_appllication_model
    {
        protected override void Arrange()
        {
            base.Arrange();
            string redirectedSectionsConfigurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "redirected_sections.config");
            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = redirectedSectionsConfigurationPath });
        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_configuration_model_doesnt_contain_redirected_sections()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsFalse(sourceModel.Sections.Where(x=>x.ConfigurationType == typeof(ValidationSettings)).Any());
        }
    }
    
    [TestClass]
    [DeploymentItem("hierarchical_config.config")]
    public class when_opening_source_with_parent_source : given_clean_appllication_model
    {
        protected override void Arrange()
        {
            base.Arrange();
            string hierarchicalConfigurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hierarchical_config.config");
            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = hierarchicalConfigurationPath });
        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_configuration_model_doesnt_contain_inherited_sections()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsFalse(sourceModel.Sections.Where(x=>x.ConfigurationType == typeof(ValidationSettings)).Any());
        }
    }
}
