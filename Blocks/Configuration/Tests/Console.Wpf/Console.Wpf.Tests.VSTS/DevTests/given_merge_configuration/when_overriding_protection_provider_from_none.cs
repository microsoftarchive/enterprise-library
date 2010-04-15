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
using Console.Wpf.Tests.VSTS.ConfigFiles;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_merge_configuration
{
    [TestClass]
    public class when_overriding_protection_provider_from_none : merging_protected_sections_context
    {
        private FileConfigurationSource mergedConfigurationFileSource;

        protected override void Act()
        {
            var resources = new ResourceHelper<ConfigFileLocator>();
            var deltaConfigurationFile = resources.DumpResourceFileToDisk("override_lab_protection_dpapi.dconfig");
            
            ConfigurationMerger configurationMerger = new ConfigurationMerger(mainConfigurationFile, deltaConfigurationFile);
            configurationMerger.MergeConfiguration(mergedConfigurationFile);

            mergedConfigurationFileSource = new FileConfigurationSource(mergedConfigurationFile);
        }

        [TestMethod]
        public void then_lab_has_overridden_protection_provider()
        {
            var loggingSection = mergedConfigurationFileSource.GetSection(LoggingSettings.SectionName);
            Assert.IsTrue(loggingSection.SectionInformation.IsProtected);
            Assert.AreEqual("DataProtectionConfigurationProvider", loggingSection.SectionInformation.ProtectionProvider.Name);
        }

        [TestMethod]
        public void then_lab_has_overridden_values()
        {
            var loggingSection = (LoggingSettings)mergedConfigurationFileSource.GetSection(LoggingSettings.SectionName);
            var formatter = (TextFormatterData)loggingSection.Formatters.First();

            Assert.AreEqual("overridden template", formatter.Template);
        }
    }
}
