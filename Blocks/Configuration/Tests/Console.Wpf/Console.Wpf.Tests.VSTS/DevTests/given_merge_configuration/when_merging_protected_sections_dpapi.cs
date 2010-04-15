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
using Console.Wpf.Tests.VSTS.ConfigFiles;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_merge_configuration
{
    [TestClass]
    public class when_merging_protected_sections_dpapi : merging_protected_sections_context
    {
        private string deltaConfigurationFile;
        private FileConfigurationSource mergedConfigurationFileSource;

        protected override void Arrange()
        {
            base.Arrange();

            using(var source = new FileConfigurationSource(mainConfigurationFile))
            {
                ProtectSection(source, LoggingSettings.SectionName, "DataProtectionConfigurationProvider");
                ProtectSection(source, ExceptionHandlingSettings.SectionName, "DataProtectionConfigurationProvider");
            }
        }

        protected override void Act()
        {
            var resources = new ResourceHelper<ConfigFileLocator>();
            deltaConfigurationFile = resources.DumpResourceFileToDisk("ehab_lab_and_daab.dconfig");

            ConfigurationMerger configurationMerger = new ConfigurationMerger(mainConfigurationFile, deltaConfigurationFile);
            configurationMerger.MergeConfiguration(mergedConfigurationFile);

            mergedConfigurationFileSource = new FileConfigurationSource(mergedConfigurationFile);
        }

        [TestMethod]
        public void then_lab_and_ehab_are_still_protected()
        {
            var loggingSection = mergedConfigurationFileSource.GetSection(LoggingSettings.SectionName);
            Assert.IsTrue(loggingSection.SectionInformation.IsProtected);
            Assert.AreEqual("DataProtectionConfigurationProvider", loggingSection.SectionInformation.ProtectionProvider.Name);

            var exceptionHandlingSection = mergedConfigurationFileSource.GetSection(ExceptionHandlingSettings.SectionName);
            Assert.IsTrue(exceptionHandlingSection.SectionInformation.IsProtected);
            Assert.AreEqual("DataProtectionConfigurationProvider", exceptionHandlingSection.SectionInformation.ProtectionProvider.Name);
        }

        [TestMethod]
        public void then_lab_has_overridden_values()
        {
            var loggingSection = (LoggingSettings)mergedConfigurationFileSource.GetSection(LoggingSettings.SectionName);
            var formatter = (TextFormatterData)loggingSection.Formatters.First();

            Assert.AreEqual("overridden template", formatter.Template);
        }

        [TestMethod]
        public void then_ehab_has_overridden_values()
        {
            var ehabSection = (ExceptionHandlingSettings)mergedConfigurationFileSource.GetSection(ExceptionHandlingSettings.SectionName);
            var wrapHandler = (WrapHandlerData)ehabSection.ExceptionPolicies.First().ExceptionTypes.First().ExceptionHandlers.First();

            Assert.AreEqual("Overridden Message", wrapHandler.ExceptionMessage);
        }

        [TestMethod]
        public void then_connectionStrings_has_overridden_values()
        {
            var connectionStringsSection = (ConnectionStringsSection)mergedConfigurationFileSource.GetSection(DataAccessDesignTime.ConnectionStringSettingsSectionName);
            var connection = connectionStringsSection.ConnectionStrings.Cast<ConnectionStringSettings>().First(x => x.Name == "another connection string");

            Assert.AreEqual("overridden connection string", connection.ConnectionString);
        }

        [TestMethod]
        public void then_connectionStrings_is_unprotected()
        {
            var connectionStringsSection = mergedConfigurationFileSource.GetSection(DataAccessDesignTime.ConnectionStringSettingsSectionName);
            Assert.IsFalse(connectionStringsSection.SectionInformation.IsProtected);
        }

        protected override void Teardown()
        {
            if (mergedConfigurationFileSource != null) mergedConfigurationFileSource.Dispose();
        }
    }
}
