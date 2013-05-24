//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Messaging;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    public abstract class Given_ConfigurationSourceBuilder : ArrangeActAssert
    {
        protected ConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }

        public IConfigurationSource GetConfigurationSource()
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configSource);
            return configSource;
        }
    }

    [TestClass]
    public class When_ConfiguringLoggongOnConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder.ConfigureLogging();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsLoggingSettings()
        {
            var configurationSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configurationSource);

            Assert.IsNotNull(configurationSource.GetSection(LoggingSettings.SectionName));
        }

        [TestMethod]
        public void Then_RevertImpersonationIsTrue()
        {
            var configurationSource = GetConfigurationSource();
            var loggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.IsTrue(loggingSettings.RevertImpersonation);
        }

        [TestMethod]
        public void Then_EnableTracingIsTrue()
        {
            var configurationSource = GetConfigurationSource();
            var loggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.IsTrue(loggingSettings.TracingEnabled);
        }


        [TestMethod]
        public void Then_LogWarningsWhenNoCategoryExistsIsTrue()
        {
            var configurationSource = GetConfigurationSource();
            var loggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.IsTrue(loggingSettings.LogWarningWhenNoCategoriesMatch);
        }

    }
}
