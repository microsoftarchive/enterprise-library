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
using System.Configuration;
using System.IO;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;

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
    public class when_saving_configuration_source_with_configuration_source_section_that_throws_to_external_source : given_dirty_application_model
    {
        string originalFileContents;
        protected override void Arrange()
        {
            originalFileContents = File.ReadAllText(TestConfigurationFilePath);

            base.Arrange();

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.RemoveSection(ConfigurationSourceSection.SectionName);
            sourceModel.AddSection("ThrowsOnSerialize", new ConfigurationSourceSectionThrowsOnSerialization());


            base.UIServiceMock.Setup(x => x.ShowError(It.IsAny<ConfigurationErrorsException>(), It.IsAny<string>()));
        }

        protected override void Act()
        {
            base.Act();

            ApplicationModel.Save();
        }

        [TestMethod]
        public void then_exception_message_was_displayed()
        {
            base.UIServiceMock.VerifyAll();
        }

        private class ConfigurationSourceSectionThrowsOnSerialization : ConfigurationSourceSection
        {
            public ConfigurationSourceSectionThrowsOnSerialization()
            {
                this.SelectedSource = "Source";
                this.Sources.Add(new FileConfigurationSourceElement { Name = "Source", FilePath = "ext.config" });
            }
            protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
            {
                throw new InvalidOperationException("TestException");
            }
        }

        protected override void Teardown()
        {
            File.WriteAllText(TestConfigurationFilePath, originalFileContents);
        }
    }

    [TestClass]
    public class when_saving_configuration_source_with_section_that_throws_to_external_source : given_dirty_application_model
    {
        string originalFileContents;
        protected override void Arrange()
        {
            originalFileContents = File.ReadAllText(TestConfigurationFilePath);

            base.Arrange();

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.AddSection("ThrowsOnSerialize", new SectionThrowsOnSerialization());

            sourceModel.RemoveSection(ConfigurationSourceSection.SectionName);
            sourceModel.AddSection(ConfigurationSourceSection.SectionName, new ConfigurationSourceSection
                                                                               {
                                                                                   SelectedSource = "ext",
                                                                                   Sources = { { new FileConfigurationSourceElement { Name = "ext", FilePath = "ext.config" } } }
                                                                               });


            base.UIServiceMock.Setup(x => x.ShowError(It.IsAny<ConfigurationErrorsException>(), It.IsAny<string>()));


        }

        protected override void Act()
        {
            base.Act();

            ApplicationModel.Save();
        }

        [TestMethod]
        public void then_exception_message_was_displayed()
        {
            base.UIServiceMock.VerifyAll();
        }

        protected override void Teardown()
        {
            File.WriteAllText(TestConfigurationFilePath, originalFileContents);
        }

        private class SectionThrowsOnSerialization : ConfigurationSection
        {
            protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
            {
                throw new InvalidOperationException("TestException");
            }
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
    public class when_saving_configuration_source_without_current_file : given_clean_application_model
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
