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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Fluent
{
    public abstract class Given_LogEnabledFilterBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationFilterLogEnabled logEnabledFilterBuilder;
        protected string logEnabledFilterName = "log enabled filter";

        protected override void Arrange()
        {
            base.Arrange();

            logEnabledFilterBuilder = base.ConfigureLogging.WithOptions.FilterEnableOrDisable(logEnabledFilterName);
        }

        protected LogEnabledFilterData GetLogEnabledFilterData()
        {
            return GetLoggingConfiguration().LogFilters.OfType<LogEnabledFilterData>().FirstOrDefault();
        }
    }

    [TestClass]
    public class When_CreatinigLogEnabledFilterBuilder : Given_LogEnabledFilterBuilder
    {
        [TestMethod]
        public void Then_ConfigurationContainsLogEnabledFilter()
        {
            Assert.IsTrue(GetLoggingConfiguration().LogFilters.OfType<LogEnabledFilterData>().Any());
        }

        [TestMethod]
        public void Then_LoggingIsDisabledByDefault()
        {
            Assert.IsFalse(GetLogEnabledFilterData().Enabled);
        }

        [TestMethod]
        public void Then_LogEnabledFilterHasAppropriateName()
        {
            Assert.AreEqual(logEnabledFilterName, GetLogEnabledFilterData().Name);
        }

        [TestMethod]
        public void Then_LogEnabledFilterHasCorrectType()
        {
            Assert.AreEqual(typeof(LogEnabledFilter), GetLogEnabledFilterData().Type);
        }
    }

    [TestClass]
    public class When_CreatinigLogEnabledFilterBuilderPassingNullForName : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_FilterEnableOrDisable_ThrowsArgumentException()
        {
            ConfigureLogging.WithOptions.FilterEnableOrDisable(null);
        }
    }


    [TestClass]
    public class When_EnablingLoggingOnLogEnabledFilterBuilder : Given_LogEnabledFilterBuilder
    {
        protected override void Act()
        {
            logEnabledFilterBuilder.Enable();
        }

        [TestMethod]
        public void Then_LoggingIsEnabled()
        {
            Assert.IsTrue(GetLogEnabledFilterData().Enabled);
        }
    }
}
