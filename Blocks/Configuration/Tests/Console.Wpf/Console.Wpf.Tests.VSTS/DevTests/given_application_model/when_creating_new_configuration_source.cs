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

using System.Linq;
using System.Windows;
using Console.Wpf.Tests.VSTS.DevTests.given_shell_service;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_application_model
{

    [TestClass]
    public class when_creating_new_configuration_source : given_dirty_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            base.UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                              .Returns(MessageBoxResult.Yes)
                              .Verifiable();

            ApplicationModel.ConfigurationFilePath = "test.config";
        }

        protected override void Act()
        {
            ApplicationModel.New();
        }

        [TestMethod]
        public void then_confirmation_message_was_shown()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_ui_is_not_dirty()
        {
            Assert.IsFalse(ApplicationModel.IsDirty);
        }

        [TestMethod]
        public void then_configuration_file_is_cleared()
        {
            Assert.IsTrue(string.IsNullOrEmpty(ApplicationModel.ConfigurationFilePath));
        }
    }

    [TestClass]
    public class when_creating_new_configuration_source_and_cancelling_save_dialog : given_clean_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();
            ApplicationModel.SetDirty();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Returns(new FileDialogResult { DialogResult = false })
                         .Verifiable();

            base.UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                              .Returns(MessageBoxResult.Yes)
                              .Verifiable();
        }

        protected override void Act()
        {
            ApplicationModel.New();
        }

        [TestMethod]
        public void then_confirmation_message_was_shown()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_ui_is_dirty()
        {
            Assert.IsTrue(ApplicationModel.IsDirty);
        }
    }

    [TestClass]
    public class when_creating_new_configugration_source_from_clean_model : given_clean_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            ApplicationModel.NewEnvironment();

            var environment = ApplicationModel.Environments.ElementAt(0);
            environment.EnvironmentDeltaFile = "myenvironment.dconfig";

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Returns(new FileDialogResult { DialogResult = false })
                         .Verifiable();

            base.UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                              .Returns(MessageBoxResult.No)
                              .Verifiable();
        
        }

        protected override void Act()
        {
            ApplicationModel.New();
        }

        [TestMethod]
        public void then_environments_are_cleared()
        {
            Assert.AreEqual(0, ApplicationModel.Environments.Count());
        }
    }
}
