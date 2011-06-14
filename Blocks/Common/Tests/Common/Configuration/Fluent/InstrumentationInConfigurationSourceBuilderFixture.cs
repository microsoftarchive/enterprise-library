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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class When_AddingInstrumentationSettingsToConfigurationSourceBuilder : Given_EmptyConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsNoInstrumentationSection()
        {
            var configurationSource = GetConfigurationSource();
            var instrumentationSettings = (InstrumentationConfigurationSection)configurationSource.GetSection(InstrumentationConfigurationSection.SectionName);

            Assert.IsNull(instrumentationSettings);
        }
    }

    [TestClass]
    public class When_AddingInstrumentationSettingsWithApplicationInstanceNameToConfigurationSourceBuilder : Given_EmptyConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigurationSourceBuilder.ConfigureInstrumentation()
                .EnableLogging()
                .ForApplicationInstance("appInstanceName");
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsInstrumentationSection()
        {
            var configurationSource = GetConfigurationSource();
            var instrumentationSettings = (InstrumentationConfigurationSection)configurationSource.GetSection(InstrumentationConfigurationSection.SectionName);

            Assert.IsNotNull(instrumentationSettings);
            Assert.IsTrue(instrumentationSettings.EventLoggingEnabled);
            Assert.IsFalse(instrumentationSettings.PerformanceCountersEnabled);
            Assert.AreEqual("appInstanceName", instrumentationSettings.ApplicationInstanceName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Then_AddingInstrumentationSettingsAgainThrows()
        {
            ConfigurationSourceBuilder.ConfigureInstrumentation()
                .EnableLogging()
                .EnablePerformanceCounters();
        }
    }
}
