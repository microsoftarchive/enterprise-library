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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Logging
{
    [TestClass]
    public class when_logging_filter_priorities_invalid : NewConfigurationSourceModelContext
    {
        private ElementViewModel logFilter;

        protected override void Arrange()
        {
            base.Arrange();

            var source = new DesignDictionaryConfigurationSource();
            new TestConfigurationBuilder().AddLoggingSettings().Build(source);

            ConfigurationSourceModel.Load(source);

            LoggingSection =
                ConfigurationSourceModel.Sections.Where(x => x.ConfigurationType == typeof (LoggingSettings)).Single();
        }

        protected override void Act()
        {
            logFilter = LoggingSection.GetDescendentsOfType<PriorityFilterData>().FirstOrDefault();
            int maximumPriority = (int) logFilter.Property("MaximumPriority").Value;

            logFilter.Property("MinimumPriority").Value = maximumPriority + 1;
            logFilter.Validate();
        }

        protected SectionViewModel LoggingSection { get; private set; }

        [TestMethod]
        public void then_validation_error_ensures()
        {
            Assert.IsTrue(logFilter.ValidationResults.Any());
        }

        [TestMethod]
        public void then_validation_message_matches()
        {
            Assert.IsTrue(
                logFilter.ValidationResults.Any(
                    e => e.Message == "The maximum priority must be higher than the minimum priority."));

        }
        
    }
}
