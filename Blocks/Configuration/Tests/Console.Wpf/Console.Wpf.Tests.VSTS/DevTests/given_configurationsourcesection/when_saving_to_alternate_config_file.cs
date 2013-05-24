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
using Console.Wpf.Tests.VSTS.ConfigFiles;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configurationsourcesection
{
    public abstract class given_configurationsource : ContainerContext
    {
        protected const string MainConfigurationFile = "configurationsource_main.config";

        protected const string SatelliteSourceName = "FileSource";
        protected const string SatelliteConfigurationFile = "configurationsource_satellite.config";

        protected ConfigurationSourceModel ConfigurationSource { get; private set; }
        protected string SatelliteConfigurationSourcePath
        {
            get { return Path.Combine(Environment.CurrentDirectory, SatelliteConfigurationFile); }
        }

        protected string MainConfigurationSourcePath
        {
            get { return Path.Combine(Environment.CurrentDirectory, MainConfigurationFile); }
        }

        protected override void Arrange()
        {
            base.Arrange();

            File.Delete(MainConfigurationFile);
            File.Delete(SatelliteConfigurationSourcePath);

            var builder = new ConfigurationSourceBuilder();

            var configurationSettings = new ConfigurationSourceSection();
            configurationSettings.SelectedSource = "System Configuration";
            configurationSettings.Sources.Add(new FileConfigurationSourceElement(SatelliteSourceName, SatelliteConfigurationSourcePath));
            configurationSettings.Sources.Add(new SystemConfigurationSourceElement("System Configuration"));
            builder.AddSection(ConfigurationSourceSection.SectionName, configurationSettings);

            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("policy")
                .ForExceptionType<Exception>();

            var source = new DesignDictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            ConfigurationSource = Container.Resolve<ConfigurationSourceModel>();
            ConfigurationSource.Load(source);
        }

        protected void SaveConfigurationFile()
        {
            File.WriteAllLines(MainConfigurationSourcePath, new[] { @"<configuration />" });
            DesignConfigurationSource outputSource = new DesignConfigurationSource(MainConfigurationSourcePath);
            ConfigurationSource.Save(outputSource);
        }

        protected bool FileContainsSection(string file, string sectionName)
        {
            using (var configSource = new DesignConfigurationSource(file))
            {
                var section = configSource.GetLocalSection(sectionName);
                return section != null;
            }
        }
    }

    [TestClass]
    public class when_loading_from_file_configuration_source : ContainerContext
    {
        private DesignConfigurationSource designSource;
        private ConfigurationSourceModel configurationSourceModel;

        protected override void Arrange()
        {
            base.Arrange();

            var resources = new ResourceHelper<ConfigFileLocator>();
            resources.DumpResourceFileToDisk("configurationsource_main.config");
            resources.DumpResourceFileToDisk("configurationsource_satellite.config");

            var mainConfigFilePath = Path.Combine(Environment.CurrentDirectory, "configurationsource_main.config");
            designSource = new DesignConfigurationSource(mainConfigFilePath);

            configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();

        }

        protected override void Act()
        {
            configurationSourceModel.Load(designSource);
        }

        [TestMethod]
        public void then_config_source_loaded()
        {
            Assert.IsTrue(
                configurationSourceModel.Sections.Where(s => s.ConfigurationType == typeof(ConfigurationSourceSection))
                    .Any());
        }

        [TestMethod]
        public void then_sections_from_alternate_file_loaded()
        {
            Assert.IsTrue(
               configurationSourceModel.Sections.Where(s => s.ConfigurationType == typeof(ExceptionHandlingSettings))
                   .Any());
        }
    }

    [TestClass]
    public class when_saving_to_file_configuration_source : given_configurationsource
    {
        protected override void Arrange()
        {
            base.Arrange();

            var configurationSourceSection = ConfigurationSource.Sections.Where(s => s.ConfigurationType == typeof(ConfigurationSourceSection)).Single();
            configurationSourceSection.Property("SelectedSource").Value = SatelliteSourceName;
            File.Delete(SatelliteConfigurationSourcePath);
        }

        protected override void Act()
        {
            SaveConfigurationFile();
        }

        [TestMethod]
        public void then_alternate_file_created()
        {
            Assert.IsTrue(File.Exists(SatelliteConfigurationSourcePath));
        }

        [TestMethod]
        public void then_configuration_source_section_written_to_system()
        {
            Assert.IsTrue(FileContainsSection(MainConfigurationSourcePath, "enterpriseLibrary.ConfigurationSource"));
        }

        [TestMethod]
        public void then_configuration_source_section_not_written_to_alternate()
        {
            Assert.IsFalse(FileContainsSection(SatelliteConfigurationSourcePath, "enterpriseLibrary.ConfigurationSource"));
        }

        [TestMethod]
        public void then_other_sections_written_to_alternate()
        {
            foreach (var sectionName in ConfigurationSource.Sections
                                            .Where(x => x.SectionName != "enterpriseLibrary.ConfigurationSource")
                                            .Select(x => x.SectionName))
            {
                Assert.IsTrue(FileContainsSection(SatelliteConfigurationSourcePath, sectionName), "File did not contain " + sectionName);
            }
        }

        [TestMethod]
        public void then_other_sections_not_written_to_main()
        {
            foreach (var sectionName in ConfigurationSource.Sections
                                             .Where(x => x.SectionName != "enterpriseLibrary.ConfigurationSource")
                                             .Select(x => x.SectionName))
            {
                Assert.IsFalse(FileContainsSection(MainConfigurationSourcePath, sectionName), "File did not contain " + sectionName);
            }
        }
    }

    [TestClass]
    public class when_saving_with_system_configuration_source_specified : given_configurationsource
    {

        protected override void Act()
        {
            SaveConfigurationFile();
        }

        [TestMethod]
        public void then_satellite_file_not_created()
        {
            Assert.IsFalse(File.Exists(SatelliteConfigurationSourcePath));
        }

        [TestMethod]
        public void then_main_contains_config_source()
        {
            Assert.IsTrue(FileContainsSection(MainConfigurationSourcePath, ConfigurationSourceSection.SectionName));
        }

        [TestMethod]
        public void then_main_contains_logging_section()
        {
            Assert.IsTrue(FileContainsSection(MainConfigurationSourcePath, ExceptionHandlingSettings.SectionName));
        }
    }

    [TestClass]
    public class when_loading_saving_with_existing_file_configurationsource : given_configurationsource
    {
        protected override void Arrange()
        {
            base.Arrange();

            var resourceHelper = new ResourceHelper<ConfigFileLocator>();
            resourceHelper.DumpResourceFileToDisk(MainConfigurationFile);
            resourceHelper.DumpResourceFileToDisk(SatelliteConfigurationFile);

            var configurationSourceSection = ConfigurationSource.Sections.Where(s => s.ConfigurationType == typeof(ConfigurationSourceSection)).Single();
            configurationSourceSection.Property("SelectedSource").Value = SatelliteSourceName;

            var builder = new ConfigurationSourceBuilder();
            builder.ConfigureLogging()
                .LogToCategoryNamed("General")
                .SendTo.EventLog("EventLogListener")
                .ToLog("Application")
                .FormatWith(new FormatterBuilder().TextFormatterNamed("TextFormatter"));
            var section = builder.Get(LoggingSettings.SectionName);
            ConfigurationSource.AddSection(LoggingSettings.SectionName, section);
        }

        protected override void Act()
        {
            ConfigurationSource.Save(new DesignConfigurationSource(MainConfigurationFile));
        }

        [TestMethod]
        public void then_satellite_should_have_new_section()
        {
            Assert.IsTrue(FileContainsSection(SatelliteConfigurationSourcePath, LoggingSettings.SectionName));
        }

        [TestMethod]
        public void then_satellite_should_have_old_sections()
        {
            Assert.IsTrue(FileContainsSection(SatelliteConfigurationSourcePath, ExceptionHandlingSettings.SectionName));
        }

        [TestMethod]
        public void then_main_should_not_have_new_section()
        {
            Assert.IsFalse(FileContainsSection(MainConfigurationSourcePath, LoggingSettings.SectionName));
        }
    }
}
