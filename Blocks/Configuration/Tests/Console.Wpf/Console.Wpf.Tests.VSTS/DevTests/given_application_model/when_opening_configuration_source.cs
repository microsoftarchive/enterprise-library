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
using System.Linq;
using System.Windows;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;


namespace Console.Wpf.Tests.VSTS.DevTests.given_shell_service
{
    public abstract class given_clean_application_model : ContainerContext
    {
        protected static string TestConfigurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.config");

        protected ApplicationViewModel ApplicationModel;

        protected PropertyChangedListener ApplicationModelChangedListener;

        protected override void Arrange()
        {
            base.Arrange();
            ApplicationModel = Container.Resolve<ApplicationViewModel>();

            ApplicationModelChangedListener = new PropertyChangedListener(ApplicationModel);
        }
    }

    [TestClass]
    public class when_user_cancels_open_file_dialog : given_clean_application_model
    {
        protected override void Act()
        {
            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = false });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_no_source_is_loaded()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.AreEqual(0, sourceModel.Sections.Count);
        }

        [TestMethod]
        public void then_current_file_is_not_changed()
        {
            Assert.AreNotEqual(TestConfigurationFilePath, ApplicationModel.ConfigurationFilePath);
        }
    }

    [TestClass]
    public class when_user_opens_configuration_file : given_clean_application_model
    {
        PropertyChangedListener shellServiceChangedListener;


        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = TestConfigurationFilePath });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<WaitDialog>())).Verifiable("Wait dialog not displayed");
            shellServiceChangedListener = new PropertyChangedListener(ApplicationModel);
        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_file_contents_is_loaded_in_source_model()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsTrue(sourceModel.Sections.Where(x => x.SectionName == "connectionStrings").Any());
        }

        [TestMethod]
        public void then_file_is_not_dirty()
        {
            Assert.IsFalse(ApplicationModel.IsDirty);
        }

        [TestMethod]
        public void then_setting_property_makes_ui_dirty()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            var connectionStringSection = sourceModel.Sections.Where(x => x.SectionName == "connectionStrings").First();
            var defaultConnectionProperty = connectionStringSection.Properties.First();
            defaultConnectionProperty.Value = "new value";

            Assert.IsTrue(ApplicationModel.IsDirty);
        }


        [TestMethod]
        public void then_current_file_is_changed()
        {
            Assert.AreEqual(TestConfigurationFilePath, ApplicationModel.ConfigurationFilePath);
        }

        [TestMethod]
        public void then_is_dirty_signals_change()
        {
            Assert.IsTrue(shellServiceChangedListener.ChangedProperties.Contains("IsDirty"));
        }

        [TestMethod]
        public void then_current_file_signals_change()
        {
            Assert.IsTrue(shellServiceChangedListener.ChangedProperties.Contains("ConfigurationFilePath"));
        }

        [TestMethod]
        public void then_wait_dialog_is_displayed_during_load()
        {
            UIServiceMock.Verify();
        }
    }

    [TestClass]
    public class when_adding_section_to_source_model : given_clean_application_model
    {
        protected override void Act()
        {
            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = TestConfigurationFilePath });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));
            ApplicationModel.OpenConfigurationSource();

            Assert.IsFalse(ApplicationModel.IsDirty);
            ApplicationModelChangedListener.ChangedProperties.Clear();

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.AddSection("my section", new ConnectionStringsSection());
        }

        [TestMethod]
        public void then_file_is_dirty()
        {
            Assert.IsTrue(ApplicationModel.IsDirty);
        }

        [TestMethod]
        public void then_is_dirty_signals_change()
        {
            Assert.IsTrue(ApplicationModelChangedListener.ChangedProperties.Contains("IsDirty"));
        }
    }

    public abstract class given_dirty_application_model : given_clean_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<OpenFileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = TestConfigurationFilePath });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));

            ApplicationModel.OpenConfigurationSource();

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.AddSection("mysection", new ConnectionStringsSection());

            ApplicationModelChangedListener.ChangedProperties.Clear();
        }
    }

    [TestClass]
    public class when_opening_configuration_file_while_editor_is_dirty_and_backing_out : given_dirty_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                         .Returns(MessageBoxResult.Cancel)
                         .Verifiable();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<OpenFileDialog>())).Callback(() => Assert.Fail());

        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_confirmation_dialog_is_shown()
        {
            UIServiceMock.Verify();
        }
    }

    [TestClass]
    public class when_opening_configuration_file_while_editor_is_dirty_and_proceeding : given_dirty_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                         .Returns(MessageBoxResult.Yes)
                         .Verifiable();
        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_current_file_signals_change()
        {
            ApplicationModelChangedListener.ChangedProperties.Contains("CurrentFileName");
        }

        [TestMethod]
        public void then_shell_isnt_dirty_anymore()
        {
            Assert.IsFalse(ApplicationModel.IsDirty);
        }

        [TestMethod]
        public void then_warning_dialog_was_shown()
        {
            UIServiceMock.Verify();
        }

    }

    [TestClass]
    public class when_opening_configuration_file_while_dirty_with_environments : given_dirty_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            ApplicationModel.NewEnvironment();
            ApplicationModel.Environments.ElementAt(0).EnvironmentDeltaFile = string.Format("unused{0}.dconfig", Guid.NewGuid().ToString("D"));

            UIServiceMock.Setup(x => x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                         .Returns(MessageBoxResult.Yes)
                         .Verifiable();

        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_environments_are_cleared()
        {
            Assert.AreEqual(0, ApplicationModel.Environments.Count());
        }
    }
}
