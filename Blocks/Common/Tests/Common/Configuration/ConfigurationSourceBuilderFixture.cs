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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_EmptyConfigurationSourceBuilder : ArrangeActAssert
    {
        protected IConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }

        protected IConfigurationSource GetConfigurationSource()
        {
            var configSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configSource);
            return configSource;
        }
    }


    [TestClass]
    public class When_AccessingConfigurationSourceBuilderMembers : ArrangeActAssert
    {
        [TestMethod]
        public void Then_ObjectMembersAreHiddenFromIntellisense()
        {
            Assert.IsTrue(typeof(IFluentInterface).IsAssignableFrom(typeof(IConfigurationSourceBuilder)));
        }
    }


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
    public class When_AddingSectionsToSourceBuilder : Given_EmptyConfigurationSourceBuilder
    {
        private InstrumentationConfigurationSection section;

        protected override void Act()
        {
            section = new InstrumentationConfigurationSection();
            base.ConfigurationSourceBuilder.AddSection(InstrumentationConfigurationSection.SectionName,
                                                       section);
        }

        [TestMethod]
        public void Then_CanRetrieveAddedSection()
        {
            Assert.AreSame(section, ConfigurationSourceBuilder.Get(InstrumentationConfigurationSection.SectionName));
        }

        [TestMethod]
        public void Then_ReturnsNullIfCannotFind()
        {
            Assert.IsNull(ConfigurationSourceBuilder.Get("unknown section name"));
        }

        [TestMethod]
        public void Then_ReturnsNullIfCannotFindViaGeneric()
        {
            Assert.IsNull(ConfigurationSourceBuilder.Get<InstrumentationConfigurationSection>("unknown section name"));
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
