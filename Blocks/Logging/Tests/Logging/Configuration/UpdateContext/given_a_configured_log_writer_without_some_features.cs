using System;
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

            var sharedMockListener = new MockTraceListener();

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[0],
                    new LogSource[]
                        {
                            new LogSource("MockCategoryOne", new TraceListener[] { sharedMockListener }, SourceLevels.Error, true),
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void then_cannot_update_logging_enabled_status()
        {
            var context = LogWriter.GetUpdateContext();
            context.IsLoggingEnabled = false;

            context.ApplyChanges();
        }
    }
}