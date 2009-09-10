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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration
{

    public abstract class Given_AConfigurationSourceBuilder : ArrangeActAssert
    {
        public IConfigurationSourceBuilder Builder { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();
            Builder = new ConfigurationSourceBuilder();
        }

    }

    [TestClass]
    public class When_GivenAnEmptyConfigurationSource : Given_AConfigurationSourceBuilder
    {
        private IConfigurationSource mergedConfiguration;

        protected override void Arrange()
        {
            base.Arrange();

            Builder
                .ConfigureInstrumentation()
                    .EnableLogging()
                    .EnableWmi();

        }

        protected override void Act()
        {
            mergedConfiguration = new DictionaryConfigurationSource();
            Builder.UpdateConfigurationWithReplace(mergedConfiguration);
        }

        [TestMethod]
        public void Then_MergingConfigurationSourceMovesSections()
        {
            Assert.IsNotNull(mergedConfiguration.GetSection(InstrumentationConfigurationSection.SectionName));
        }
    }

    [TestClass]
    public class When_GivenAPopulatedConfigurationSection : Given_AConfigurationSourceBuilder
    {
        private IConfigurationSource mergedConfiguration;
        protected override void Arrange()
        {
            base.Arrange();

            mergedConfiguration = new DictionaryConfigurationSource();
            mergedConfiguration.Add(
                           InstrumentationConfigurationSection.SectionName, 
                           new InstrumentationConfigurationSection(false, true, false));

            Builder
                .ConfigureInstrumentation()
                    .EnablePerformanceCounters()
                    .EnableWmi();
        }

        protected override void Act()
        {
            mergedConfiguration = new DictionaryConfigurationSource();
            Builder.UpdateConfigurationWithReplace(mergedConfiguration);
        }

        [TestMethod]
        public void Then_MergingConfigurationSourceMovesSections()
        {
            var section = (InstrumentationConfigurationSection) mergedConfiguration
                                                                    .GetSection(InstrumentationConfigurationSection.SectionName);

            Assert.IsFalse(section.EventLoggingEnabled);
            Assert.IsTrue(section.PerformanceCountersEnabled);
            Assert.IsTrue(section.WmiEnabled);
        }
    }
}
