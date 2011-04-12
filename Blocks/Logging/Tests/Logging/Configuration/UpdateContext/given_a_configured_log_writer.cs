using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class given_a_configured_log_writer : ArrangeActAssert
    {
        protected LogWriterImpl LogWriter;
        protected override void Arrange()
        {
            base.Arrange();
            MockTraceListener.Reset();

            var sharedMockListener = new MockTraceListener("mockTraceListener");

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[] { new LogEnabledFilter("something", true),  },
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
        public void then_can_get_readonly_collection_of_sources()
        {
            var context = LogWriter.GetUpdateContext();

            Assert.AreEqual(2, LogWriter.TraceSources.Count);
            Assert.AreEqual(2, context.TraceSources.Count);
            Assert.IsTrue(context.TraceSources.Any(x => x.Name == "MockCategoryOne" && x.Level == SourceLevels.Error));
            Assert.IsTrue(context.TraceSources.Any(x => x.Name == "operation" && x.Level == SourceLevels.All));
            Assert.IsTrue(context.TraceSources.IsReadOnly);
        }

        [TestMethod]
        public void then_can_change_log_source_properties()
        {
            var context = LogWriter.GetUpdateContext();
            context.TraceSources.First(x => x.Name == "MockCategoryOne").Level = SourceLevels.Warning;
            context.TraceSources.First(x => x.Name == "MockCategoryOne").AutoFlush = false;

            Assert.AreNotEqual(SourceLevels.Warning, LogWriter.TraceSources["MockCategoryOne"].Level);
            Assert.AreNotEqual(false, LogWriter.TraceSources["MockCategoryOne"].AutoFlush);

            context.ApplyChanges();

            Assert.AreEqual(SourceLevels.Warning, LogWriter.TraceSources["MockCategoryOne"].Level);
            Assert.AreEqual(false, LogWriter.TraceSources["MockCategoryOne"].AutoFlush);
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
        public void then_can_get_trace_listeners_assigned_to_source()
        {
            var context = LogWriter.GetUpdateContext();

            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);
            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count(x => x.Name == "mockTraceListener"));

            Assert.AreEqual(1, context.TraceSources.First(x => x.Name == "MockCategoryOne").Listeners.Count);
            Assert.IsTrue(context.TraceSources.First(x => x.Name == "MockCategoryOne").Listeners.Contains("mockTraceListener"));
        }

        [TestMethod]
        public void then_can_remove_trace_listeners_from_source()
        {
            var context = LogWriter.GetUpdateContext();
            context.TraceSources.First(x => x.Name == "MockCategoryOne").Listeners.Remove("mockTraceListener");
            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);

            context.ApplyChanges();

            Assert.AreEqual(0, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);
        }
    }
}
