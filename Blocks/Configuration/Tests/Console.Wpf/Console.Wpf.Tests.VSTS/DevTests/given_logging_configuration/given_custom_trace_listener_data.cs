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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_configuration
{

    public abstract class given_custom_trace_listener_data : given_logging_configuration
    {
        protected ElementViewModel CustomTraceListener;
        protected override void Arrange()
        {
            base.Arrange();

            var loggingSection = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, base.LoggingSection);
            CustomTraceListener = loggingSection.GetDescendentsOfType<CustomTraceListenerData>().First();
        }
    }


    [TestClass]
    public class when_accessing_properties_on_custom_trace_listener : given_custom_trace_listener_data
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            properties = CustomTraceListener.Properties;
        }

        [TestMethod]
        public void then_custom_trace_listener_has_formatter_property()
        {
            Assert.IsTrue(properties.Any(x => x.PropertyName == "Formatter"));
        }
    }
}
