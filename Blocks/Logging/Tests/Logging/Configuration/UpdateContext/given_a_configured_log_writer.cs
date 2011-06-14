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
    public class given_a_configured_log_writer : ArrangeActAssert
    {
        protected LogWriterImpl LogWriter;
        LogSource allEventsCategory;
        LogSource notProcessedCategory;
        LogSource errorsCategory;

        protected override void Arrange()
        {
            base.Arrange();
            MockTraceListener.Reset();

            var sharedMockListener = new MockTraceListener("mockTraceListener");
            allEventsCategory = new LogSource("all events", new TraceListener[] { sharedMockListener }, SourceLevels.Critical, true);
            notProcessedCategory = new LogSource("not processed", new TraceListener[] { sharedMockListener }, SourceLevels.Information, true);
            errorsCategory = new LogSource("errors", new TraceListener[] { sharedMockListener }, SourceLevels.Error, false);

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[] { new LogEnabledFilter("something", true), },
                    new LogSource[]
                    {
                        new LogSource("MockCategoryOne", new TraceListener[] { sharedMockListener }, SourceLevels.Error, true),
                        new LogSource("operation", new TraceListener[] { sharedMockListener }, SourceLevels.All, false),
                    },
                    allEventsCategory,
                    notProcessedCategory,
                    errorsCategory,
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
        public void then_can_get_readonly_collection_of_categories()
        {
            var context = LogWriter.GetUpdateContext();

            Assert.AreEqual(2, LogWriter.TraceSources.Count);
            Assert.AreEqual(2, context.Categories.Count);
            Assert.IsTrue(context.Categories.Any(x => x.Name == "MockCategoryOne" && x.Level == SourceLevels.Error && x.AutoFlush == true));
            Assert.IsTrue(context.Categories.Any(x => x.Name == "operation" && x.Level == SourceLevels.All && x.AutoFlush == false));
            Assert.IsTrue(context.Categories.IsReadOnly);
        }

        [TestMethod]
        public void then_can_change_log_source_properties()
        {
            var context = LogWriter.GetUpdateContext();
            context.Categories.First(x => x.Name == "MockCategoryOne").Level = SourceLevels.Warning;
            context.Categories.First(x => x.Name == "MockCategoryOne").AutoFlush = false;

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
        public void then_can_get_trace_listeners_assigned_to_category()
        {
            var context = LogWriter.GetUpdateContext();

            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);
            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count(x => x.Name == "mockTraceListener"));

            Assert.AreEqual(1, context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Count);
            Assert.IsTrue(context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Contains("mockTraceListener"));
        }

        [TestMethod]
        public void then_can_remove_trace_listeners_from_source()
        {
            var context = LogWriter.GetUpdateContext();
            context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Remove("mockTraceListener");
            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);

            context.ApplyChanges();

            Assert.AreEqual(0, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void then_cannot_apply_changes_if_new_update_context_was_created()
        {
            var firstContext = LogWriter.GetUpdateContext();
            firstContext.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Remove("mockTraceListener");
            var secondContext = LogWriter.GetUpdateContext();

            firstContext.ApplyChanges();
        }

        [TestMethod]
        public void then_can_apply_changes_on_the_newest_update_context()
        {
            var firstContext = LogWriter.GetUpdateContext();
            var secondContext = LogWriter.GetUpdateContext();
            secondContext.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Remove("mockTraceListener");

            secondContext.ApplyChanges();

            Assert.AreEqual(0, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void then_cannot_reapply_changes()
        {
            var context = LogWriter.GetUpdateContext();
            context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Remove("mockTraceListener");
            context.ApplyChanges();

            context.ApplyChanges();
        }

        [TestMethod]
        public void then_can_apply_new_changes()
        {
            var context = LogWriter.GetUpdateContext();
            context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Remove("mockTraceListener");
            context.ApplyChanges();
            Assert.AreEqual(0, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count);

            context = LogWriter.GetUpdateContext();
            context.Categories.First(x => x.Name == "MockCategoryOne").Listeners.Add("mockTraceListener");
            context.ApplyChanges();

            Assert.AreEqual(1, LogWriter.TraceSources["MockCategoryOne"].Listeners.Count(x => x.Name == "mockTraceListener"));
        }


        [TestMethod]
        public void then_can_get_special_sources()
        {
            var context = LogWriter.GetUpdateContext();

            Assert.AreEqual(SourceLevels.Critical, context.AllEventsCategory.Level);
            Assert.AreEqual(true, context.AllEventsCategory.AutoFlush);
            Assert.AreEqual("mockTraceListener", context.AllEventsCategory.Listeners.Single());
            Assert.AreEqual(SourceLevels.Information, context.NotProcessedCategory.Level);
            Assert.AreEqual(true, context.NotProcessedCategory.AutoFlush);
            Assert.AreEqual("mockTraceListener", context.NotProcessedCategory.Listeners.Single());
            Assert.AreEqual(SourceLevels.Error, context.ErrorsCategory.Level);
            Assert.AreEqual(false, context.ErrorsCategory.AutoFlush);
            Assert.AreEqual("mockTraceListener", context.ErrorsCategory.Listeners.Single());
        }

        [TestMethod]
        public void then_can_update_special_sources()
        {
            var context = LogWriter.GetUpdateContext();
            context.AllEventsCategory.Level = SourceLevels.Error;
            context.NotProcessedCategory.Level = SourceLevels.Verbose;
            context.ErrorsCategory.Level = SourceLevels.Off;
            Assert.AreNotEqual(SourceLevels.Error, allEventsCategory.Level);
            Assert.AreNotEqual(SourceLevels.Verbose, notProcessedCategory.Level);
            Assert.AreNotEqual(SourceLevels.Off, errorsCategory.Level);

            context.ApplyChanges();

            Assert.AreEqual(SourceLevels.Error, allEventsCategory.Level);
            Assert.AreEqual(SourceLevels.Verbose, notProcessedCategory.Level);
            Assert.AreEqual(SourceLevels.Off, errorsCategory.Level);
        }
    }
}
