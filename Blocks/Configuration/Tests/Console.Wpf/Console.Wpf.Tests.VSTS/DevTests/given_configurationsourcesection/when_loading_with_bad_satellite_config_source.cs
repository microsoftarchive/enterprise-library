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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Console.Wpf.Tests.VSTS.ConfigFiles;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configurationsourcesection
{

    [TestClass]
    public class when_loading_with_satellite_missing : ContainerContext
    { 
        private DesignConfigurationSource designSource;
        private ConfigurationSourceModel configurationSourceModel;

        protected override void Arrange()
        {
            base.Arrange();

            var resources = new ResourceHelper<ConfigFileLocator>();
            resources.DumpResourceFileToDisk("configurationsource_main_with_missing.config");

            designSource = new DesignConfigurationSource("configurationsource_main_with_missing.config");
            configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();

            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
            UIServiceMock.Setup(
                x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
                .Returns(MessageBoxResult.OK).Verifiable("Message not invoked");
        }

        protected override void Act()
        {
            configurationSourceModel.Load(designSource);
        }

        [TestMethod]
        public void then_message_is_shown_to_the_user()
        {
            UIServiceMock.Verify();
        }
    }

    [TestClass]
    public class when_loading_from_application_with_satellite_missing : ContainerContext
    {
        private IApplicationModel applicationModel;
        private const string sourceFileName = "configurationsource_main_with_missing.config";

        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
            UIServiceMock.Setup(
                x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>())).Returns(
                MessageBoxResult.OK);

            var resources = new ResourceHelper<ConfigFileLocator>();
            resources.DumpResourceFileToDisk(sourceFileName);

            applicationModel = Container.Resolve<IApplicationModel>();
        }

        protected override void Act()
        {
            applicationModel.Load(sourceFileName);
        }

        [TestMethod]
        public void then_configuration_file_path_is_null()
        {
            Assert.AreEqual(null, applicationModel.ConfigurationFilePath);    
        }

        [TestMethod]
        public void then_is_dirty_false()
        {
            Assert.IsFalse(applicationModel.IsDirty);
        }

        [TestMethod]
        public void then_configuration_source_sections_empty()
        {
            var model = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsFalse(model.Sections.Any());
        }
    }


    [TestClass]
    public class when_loading_with_invalid_satellite : ContainerContext
    {
        private DesignConfigurationSource designSource;
        private ConfigurationSourceModel configurationSourceModel;

        protected override void Arrange()
        {
            base.Arrange();

            var resources = new ResourceHelper<ConfigFileLocator>();
            resources.DumpResourceFileToDisk("configurationsource_with_invalid_satellite.config");
            resources.DumpResourceFileToDisk("configurationsource_invalid_satellite.config");

            designSource = new DesignConfigurationSource("configurationsource_with_invalid_satellite.config");
            configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();

            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
            UIServiceMock.Setup(
                x => x.ShowError(It.IsAny<Exception>(), It.IsAny<string>())).Verifiable();
        }

        protected override void Act()
        {
            configurationSourceModel.Load(designSource);
        }

        [TestMethod]
        public void then_message_is_shown_to_the_user()
        {
            UIServiceMock.Verify();
        }
    }

    [TestClass]
    public class when_loading_from_application_with_invalid_satellite : ContainerContext
    {
        private IApplicationModel applicationModel;
        private const string sourceFileName = "configurationsource_with_invalid_satellite.config";

        protected override void Arrange()
        {
            base.Arrange();
            
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
            UIServiceMock.Setup(
                           x => x.ShowError(It.IsAny<Exception>(), It.IsAny<string>()));

            var resources = new ResourceHelper<ConfigFileLocator>();
            resources.DumpResourceFileToDisk(sourceFileName);
            resources.DumpResourceFileToDisk("configurationsource_invalid_satellite.config");

            applicationModel = Container.Resolve<IApplicationModel>();
        }

        protected override void Act()
        {
            applicationModel.Load(sourceFileName);
        }

        [TestMethod]
        public void then_configuration_file_path_is_new_file()
        {
            Assert.AreEqual(Path.Combine(Environment.CurrentDirectory, sourceFileName), applicationModel.ConfigurationFilePath);
        }

        [TestMethod]
        public void then_is_dirty_false()
        {
            Assert.IsFalse(applicationModel.IsDirty);
        }
    }
}
