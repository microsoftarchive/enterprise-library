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
using System.Messaging;
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Logging.given_logging_configuration
{
    public abstract class given_msmq_trace_listener : LoggingConfigurationContext
    {
        protected ElementViewModel MsmqTraceListener;
        protected override void Arrange()
        {
            base.Arrange();

            var loggingSection = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, base.LoggingSection);
            MsmqTraceListener = loggingSection.GetDescendentsOfType<MsmqTraceListenerData>().First();
        }
    }

    [TestClass]
    public class when_accessing_properties_on_msmq_trace_listener : given_msmq_trace_listener
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            properties = MsmqTraceListener.Properties;
        }

        [TestMethod]
        public void then_time_to_receive_defaults_to_message_infinite()
        {
            Assert.AreEqual(Message.InfiniteTimeout, properties.Where(x => x.PropertyName == "TimeToBeReceived").First().Value);
        }

        [TestMethod]
        public void then_time_to_reach_queue_defaults_to_message_infinite()
        {
            Assert.AreEqual(Message.InfiniteTimeout, properties.Where(x => x.PropertyName == "TimeToReachQueue").First().Value);
        }

        [TestMethod]
        public void then_filter_property_has_exclusive_values()
        {
            Assert.IsFalse(properties.Where(x => x.PropertyName == "Filter").First().SuggestedValuesEditable);
        }
    }
}
