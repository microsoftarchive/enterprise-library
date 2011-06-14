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
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class given_a_configured_log_writer_without_some_features : ArrangeActAssert
    {
        protected LogWriterImpl LogWriter;
        protected override void Arrange()
        {
            base.Arrange();
            MockTraceListener.Reset();

            var sharedMockListener = new MockTraceListener("mockTraceListener");

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[0],
                    new LogSource[]
                        {
                            new LogSource("MockCategoryOne", new TraceListener[0], SourceLevels.Error, true),
                            new LogSource("operation", new TraceListener[] { sharedMockListener }, SourceLevels.All, false),
                        },
                    new LogSource(""),
                    new LogSource(""),
                    new LogSource(""),
                    "MockCategoryOne",
                    true,
                    false);
        }

        protected override void Teardown()
        {
            base.Teardown();
            LogWriter.Dispose();
            LogWriter = null;
        }

        [TestMethod]
        public void then_can_get_if_log_is_enabled()
        {
            var context = LogWriter.GetUpdateContext();

            Assert.IsTrue(LogWriter.IsLoggingEnabled());

            Assert.AreEqual(true, context.IsLoggingEnabled);
        }

        [TestMethod]
        public void then_can_update_logging_enabled_status()
        {
            var context = LogWriter.GetUpdateContext();
            context.IsLoggingEnabled = false;
            Assert.IsTrue(LogWriter.IsLoggingEnabled());

            context.ApplyChanges();

            Assert.IsFalse(LogWriter.IsLoggingEnabled());
        }

        [TestMethod]
        public void then_can_add_trace_listeners_to_source()
        {
            var context = LogWriter.GetUpdateContext();
            Assert.AreEqual(0, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);
            context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Add("mockTraceListener");

            context.ApplyChanges();

            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);
            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count(x => x.Name == "mockTraceListener"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void then_cannot_add_an_inexistant_tracelistener_name()
        {
            var context = LogWriter.GetUpdateContext();
            context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Add("inexistant");
        }
    }
}
