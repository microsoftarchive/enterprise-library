//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Logging.given_new_add_logging_command
{
    [TestClass]
    public class when_default_instance_is_created : Context
    {
        protected override void Act()
        {
            ConfigurationSection = AddBlockCommand.CreateConfigurationSection();
        }

        [TestMethod]
        public void then_section_name_is_properly_setted()
        {
            Assert.AreEqual(AddBlockCommand.SectionName, LoggingSettings.SectionName);
        }

        [TestMethod]
        public void then_default_configuration_section_is_properly_created()
        {
            var settings = ConfigurationSection as LoggingSettings;

            Assert.IsNotNull(settings);
            Assert.IsNotNull(settings.SpecialTraceSources.AllEventsTraceSource);
            Assert.IsNotNull(settings.SpecialTraceSources.ErrorsTraceSource);
            Assert.IsNotNull(settings.SpecialTraceSources.NotProcessedTraceSource);

            Assert.AreEqual(1, settings.TraceListeners.Count);
            var remoteServiceTraceListener = settings.TraceListeners.Get(0) as RemoteServiceTraceListenerData;
            Assert.IsNotNull(remoteServiceTraceListener);
            Assert.IsNotNull(remoteServiceTraceListener.Name);

            Assert.AreEqual(0, settings.Formatters.Count);

            Assert.AreEqual(1, settings.TraceSources.Count);
            var traceSource = settings.TraceSources.Get(0) as TraceSourceData;
            Assert.IsNotNull(traceSource);
            Assert.IsNotNull(traceSource.Name);

            Assert.AreEqual(1, settings.SpecialTraceSources.ErrorsTraceSource.TraceListeners.Count);
            Assert.AreEqual(remoteServiceTraceListener.Name, settings.SpecialTraceSources.ErrorsTraceSource.TraceListeners.Get(0).Name);

            Assert.AreEqual(1, settings.SpecialTraceSources.ErrorsTraceSource.TraceListeners.Count);
            Assert.AreEqual(remoteServiceTraceListener.Name, traceSource.TraceListeners.Get(0).Name);
        }
    }

}
