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
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.DevTests.given_shell_service;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using FileDialog = Microsoft.Win32.FileDialog;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

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

    [TestClass]
    public class when_saving_configuration_source_with_validation_errors : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>()))
                .Returns(new FileDialogResult { DialogResult = true, FileName = "unused.config" });

            UIServiceMock.Setup(x => x.ShowError(It.IsAny<string>())).Verifiable();

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
                                                   .SelectMany(p => p.ValidationErrors)
                                                   .Union(s.Properties.SelectMany(p => p.ValidationErrors))
                                                   ).Where(e => e.IsError).Any());
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

    [TestClass]
    public class when_saving_configuratin_with_only_validation_warnings : ContainerContext
    {
        private ConfigurationSourceModel sourceModel;

        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<FileDialog>()))
                .Returns(new FileDialogResult { DialogResult = true, FileName = "unused.config" });

            UIServiceMock.Setup(x => x.ShowError(It.IsAny<string>())).AtMost(0);

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
                                                 .SelectMany(p => p.ValidationErrors)
                                                 .Union(s.Properties.SelectMany(p => p.ValidationErrors)));

            Assert.IsTrue(errors.All(e => e.IsWarning));
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
        protected override void ValidateCore(object instance, string value, IList<ValidationError> errors)
        {
            var property = instance as Property;
            if (property == null) return;

            errors.Add(new ValidationError(property, "Test Warning Message", true));
        }
    }
}
