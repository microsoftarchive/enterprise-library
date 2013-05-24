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

using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.DevTests.given_shell_service;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileDialog = Microsoft.Win32.FileDialog;

namespace Console.Wpf.Tests.VSTS.DevTests.given_application_model
{
    [TestClass]
    public class when_saving_configuration_source : given_clean_application_model
    {
        string originalFileContents;
        bool saveOperationBeginFired;
        bool saveOperationEndFired;

        protected override void Arrange()
        {
            originalFileContents = File.ReadAllText(TestConfigurationFilePath);
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>())).Returns(new FileDialogResult { DialogResult = true, FileName = TestConfigurationFilePath });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>()));

            ApplicationModel.OpenConfigurationSource();

            SaveOperation saveOperation = Container.Resolve<SaveOperation>();

            saveOperationBeginFired = false;
            saveOperationEndFired = false;
            saveOperation.BeginSaveOperation += (sender, args) => saveOperationBeginFired = true;
            saveOperation.EndSaveOperation += (sender, args) => saveOperationEndFired = true;

        }

        [TestMethod]
        public void then_removed_sections_are_removed_when_saving()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            var loggingSettings = sourceModel.Sections.Where(x => x.ConfigurationType == typeof(LoggingSettings)).First();
            loggingSettings.Delete();

            ApplicationModel.Save();

            FileConfigurationSource source = new FileConfigurationSource(TestConfigurationFilePath, false);
            Assert.IsNull(source.GetSection(LoggingSettings.SectionName));

        }

        [TestMethod]
        public void then_save_operation_begin_and_end_events_where_fired()
        {
            ApplicationModel.Save();

            Assert.IsTrue(saveOperationBeginFired);
            Assert.IsTrue(saveOperationEndFired);
        }

        protected override void Teardown()
        {
            File.WriteAllText(TestConfigurationFilePath, originalFileContents);
        }
    }

    public abstract class ConfigurationWithErrorsCotnext : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new ElementForValidation();

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", section);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);


            Assert.IsTrue(sourceModel.Sections
                              .SelectMany(s => s.DescendentElements()
                                                   .SelectMany(e => e.Properties)
                                                   .SelectMany(p => p.ValidationResults)
                                                   .Union(s.Properties.SelectMany(p => p.ValidationResults))
                                                   ).Where(e => e.IsError).Any());
        }
    }


    [TestClass]
    public class when_saving_configuration_source_with_validation_errors : ConfigurationWithErrorsCotnext
    {
        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>()))
                .Returns(new FileDialogResult { DialogResult = true, FileName = "unused.config" });

            UIServiceMock.Setup(x => x.ShowError(It.IsAny<string>())).Verifiable();
        }

        protected override void Act()
        {
            var appModel = Container.Resolve<IApplicationModel>();
            appModel.Save();
        }

        [TestMethod]
        public void than_dialog_invoked()
        {
            UIServiceMock.Verify();
        }
    }

    public abstract class ConfigurationWithWarningsContext : ContainerContext
    {
        protected override void Arrange()
        {

            base.Arrange();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new ElementWithOnlyWarning();

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", section);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            var errors = sourceModel.Sections
                .SelectMany(s => s.DescendentElements()
                                     .SelectMany(e => e.Properties)
                                     .SelectMany(p => p.ValidationResults)
                                     .Union(s.Properties.SelectMany(p => p.ValidationResults)));

            Assert.IsTrue(errors.All(e => e.IsWarning));
        }
    }

    [TestClass]
    public class when_saving_configuratin_with_only_validation_warnings : ConfigurationWithWarningsContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>()))
                .Returns(new FileDialogResult { DialogResult = true, FileName = "unused.config" });

            UIServiceMock.Setup(x => x.ShowError(It.IsAny<string>())).AtMost(0);
        }

        protected override void Act()
        {
            var appModel = Container.Resolve<IApplicationModel>();
            appModel.Save();
        }

        [TestMethod]
        public void than_dialog_not_invoked()
        {
            UIServiceMock.Verify();
        }

    }

    [TestClass]
    public class when_saving_configuration_with_environment_errors : ConfigurationWithWarningsContext
    {
        private ApplicationViewModel appModel;

        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>()))
              .Returns(new FileDialogResult { DialogResult = true, FileName = "unused.config" });

            UIServiceMock.Setup(x => x.ShowError(It.IsAny<string>())).Verifiable();

            appModel = Container.Resolve<ApplicationViewModel>();
            appModel.NewEnvironment();
            var validEnvironment = appModel.Environments.ElementAt(0);
            validEnvironment.EnvironmentDeltaFile = "mydeltafile.dconfig";
            Assert.IsFalse(validEnvironment.Properties
                                .SelectMany(p => p.ValidationResults)
                              .Where(e => e.IsError).Any());

            appModel.NewEnvironment();
            var invalidEnvironment = appModel.Environments.ElementAt(0);
            invalidEnvironment.EnvironmentDeltaFile = string.Empty;

            Assert.IsTrue(invalidEnvironment.Properties
                              .SelectMany(p => p.ValidationResults)
                              .Where(e => e.IsError).Any());
        }

        protected override void Act()
        {
            appModel.Save();
        }

        [TestMethod]
        public void then_should_prompt_to_fix_validation_errors()
        {
            UIServiceMock.Verify();
        }

    }

    public class ElementWithOnlyWarning : ConfigurationSection
    {
        private const string propertyWithWarnings = "propertyWithWarnings";

        [ConfigurationProperty(propertyWithWarnings)]
        [Validation(typeof(WarningProductingValidator))]
        public string PropertyWithWarnings
        {
            get
            {
                return (string)this[propertyWithWarnings];
            }

            set
            {
                this[propertyWithWarnings] = value;
            }
        }
    }

    public class WarningProductingValidator : Validator
    {
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var property = instance as Property;
            if (property == null) return;

            results.Add(new PropertyValidationResult(property, "Test Warning Message", true));
        }
    }
}
