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

using System.Collections.Generic;
using System.Linq;
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Logging.given_logging_configuration
{
    public abstract class given_trace_source : LoggingConfigurationContext
    {
        protected ElementViewModel TraceSource;
        protected override void Arrange()
        {
            base.Arrange();

            var loggingSection = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, base.LoggingSection);
            TraceSource = loggingSection.GetDescendentsOfType<TraceSourceData>().First();
        }
    }

    [TestClass]
    public class when_accessing_properties_on_trace_source : given_trace_source
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            properties = TraceSource.Properties;
        }

        [TestMethod]
        public void then_default_level_property_has_exclusive_values()
        {
            Assert.IsFalse(properties.Where(x => x.PropertyName == "DefaultLevel").First().SuggestedValuesEditable);
        }
    }
}
