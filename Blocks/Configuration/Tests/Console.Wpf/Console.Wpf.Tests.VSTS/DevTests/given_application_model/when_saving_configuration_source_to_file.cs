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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Moq;
using Microsoft.Win32;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace Console.Wpf.Tests.VSTS.DevTests.given_shell_service
{
    [TestClass]
    public class when_saving_configuration_source : given_dirty_application_model
    {
        DateTime lastWriteTimeInArrange;
        protected override void Arrange()
        {
            base.Arrange();
            
            lastWriteTimeInArrange = File.GetLastWriteTime(TestConfigurationFilePath);
        }

        protected override void Act()
        {
            base.Act();

            ApplicationModel.Save();
        }

        [TestMethod]
        public void then_ui_is_not_dirty()
        {
            Assert.IsFalse(ApplicationModel.IsDirty);
        }

        [TestMethod]
        public void then_file_was_written()
        {
            var currentLastWriteTime = File.GetLastWriteTime(TestConfigurationFilePath);
            Assert.AreNotEqual(lastWriteTimeInArrange, currentLastWriteTime);
        }
    }

    [TestClass]
    public class when_saving_configuration_source_over_read_only_file_choosing_to_save_as : given_dirty_application_model
    {
        DateTime lastWriteTimeInArrange;
        protected override void Arrange()
        {
            base.Arrange();

            lastWriteTimeInArrange = File.GetLastWriteTime(TestConfigurationFilePath);
            File.SetAttributes(TestConfigurationFilePath, FileAttributes.ReadOnly);

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Returns(new FileDialogResult { DialogResult = false })
                         .Verifiable();

            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                         .Returns(MessageBoxResult.No)
                         .Verifiable();
        }

        protected override void Act()
        {
            ApplicationModel.Save();
        }

        [TestMethod]
        public void then_save_dialog_and_warning_are_shown()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_file_was_not_written()
        {
            var currentLastWriteTime = File.GetLastWriteTime(TestConfigurationFilePath);
            Assert.AreEqual(lastWriteTimeInArrange, currentLastWriteTime);
        }

        protected override void Teardown()
        {
            File.SetAttributes(TestConfigurationFilePath, FileAttributes.Normal);
        }
    }

    [TestClass]
    public class when_saving_configuration_source_over_read_only_file_choosing_to_overwrite : given_dirty_application_model
    {
        DateTime lastWriteTimeInArrange;
        protected override void Arrange()
        {
            base.Arrange();

            lastWriteTimeInArrange = File.GetLastWriteTime(TestConfigurationFilePath);
            File.SetAttributes(TestConfigurationFilePath, FileAttributes.ReadOnly);

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Callback(() => Assert.Fail());

            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                         .Returns(MessageBoxResult.Yes)
                         .Verifiable();
        }

        protected override void Act()
        {
            ApplicationModel.Save();
        }

        [TestMethod]
        public void then_warning_is_shown()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_file_was_written()
        {
            var currentLastWriteTime = File.GetLastWriteTime(TestConfigurationFilePath);
            Assert.AreNotEqual(lastWriteTimeInArrange, currentLastWriteTime);
        }

        [TestMethod]
        public void then_file_is_read_write()
        {
            var attributes = File.GetAttributes(TestConfigurationFilePath);
            Assert.AreNotEqual(attributes, attributes | FileAttributes.ReadOnly);
        }

        protected override void Teardown()
        {
            File.SetAttributes(TestConfigurationFilePath, FileAttributes.Normal);
        }
    }

    [TestClass]
    public class when_saving_configuration_source_over_read_only_file_backing_out : given_dirty_application_model
    {
        DateTime lastWriteTimeInArrange;
        protected override void Arrange()
        {
            base.Arrange();

            lastWriteTimeInArrange = File.GetLastWriteTime(TestConfigurationFilePath);
            File.SetAttributes(TestConfigurationFilePath, FileAttributes.ReadOnly);

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Callback(() => Assert.Fail());

            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                         .Returns(MessageBoxResult.Cancel)
                         .Verifiable();
        }

        protected override void Act()
        {
            ApplicationModel.Save();
        }

        [TestMethod]
        public void then_file_was_not_written()
        {
            var currentLastWriteTime = File.GetLastWriteTime(TestConfigurationFilePath);
            Assert.AreEqual(lastWriteTimeInArrange, currentLastWriteTime);
        }

        protected override void Teardown()
        {
            File.SetAttributes(TestConfigurationFilePath, FileAttributes.Normal);
        }
    }

    [TestClass]
    public class when_saving_configuration_source_without_current_file : given_clean_appllication_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<SaveFileDialog>()))
                         .Returns(new FileDialogResult { DialogResult = false })
                         .Verifiable();
        }

        protected override void Act()
        {
            ApplicationModel.Save();
        }

        [TestMethod]
        public void then_file_save_dialog_is_shown()
        {
            UIServiceMock.Verify();
        }
    }

}
