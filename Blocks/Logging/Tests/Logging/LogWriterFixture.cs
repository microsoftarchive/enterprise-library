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
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    /// <summary>
    /// Summary description for LogWriterFixture
    /// </summary>
    [TestClass]
    public class LogWriterFixture
    {
        [TestMethod]
        public void CanCreateLogWriterUsingConstructor()
        {
            LogWriter writer = new LogWriter(new List<ILogFilter>(), new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfLogWriterUsingConstructorWithNullFiltersThrows()
        {
            LogWriter writer = new LogWriter(null, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfLogWriterUsingConstructorWithNullTraceSourcesThrows()
        {
            LogWriter writer = new LogWriter(new List<ILogFilter>(), (IDictionary<string, LogSource>)null, new LogSource("errors"), "default");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfLogWriterUsingConstructorWithNullErrorsTraceSourceThrows()
        {
            LogWriter writer = new LogWriter(new List<ILogFilter>(), new Dictionary<string, LogSource>(), null, "default");
        }

        [TestMethod]
        public void CanGetLogFiltersByType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enable", true));

            LogWriter writer = new LogWriter(filters, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
            CategoryFilter categoryFilter = writer.GetFilter<CategoryFilter>();
            PriorityFilter priorityFilter = writer.GetFilter<PriorityFilter>();
            LogEnabledFilter enabledFilter = writer.GetFilter<LogEnabledFilter>();

            Assert.IsNotNull(categoryFilter);
            Assert.AreEqual(4, categoryFilter.CategoryFilters.Count);
            Assert.IsNotNull(priorityFilter);
            Assert.AreEqual(100, priorityFilter.MinimumPriority);
            Assert.IsNotNull(enabledFilter);
            Assert.IsTrue(enabledFilter.Enabled);
        }

        [TestMethod]
        public void CanGetLogFiltersByNameAndType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority1", 100));
            filters.Add(new LogEnabledFilter("enable", true));
            filters.Add(new PriorityFilter("priority2", 200));

            LogWriter writer = new LogWriter(filters, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
            PriorityFilter priorityFilter1 = writer.GetFilter<PriorityFilter>("priority1");
            PriorityFilter priorityFilter2 = writer.GetFilter<PriorityFilter>("priority2");

            Assert.IsNotNull(priorityFilter1);
            Assert.AreEqual(100, priorityFilter1.MinimumPriority);
            Assert.IsNotNull(priorityFilter2);
            Assert.AreEqual(200, priorityFilter2.MinimumPriority);
        }

        [TestMethod]
        public void CanGetLogFiltersByName()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority1", 100));
            filters.Add(new LogEnabledFilter("enable", true));
            filters.Add(new PriorityFilter("priority2", 200));

            LogWriter writer = new LogWriter(filters, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
            ILogFilter categoryFilter = writer.GetFilter("category");
            ILogFilter priorityFilter = writer.GetFilter("priority2");

            Assert.IsNotNull(categoryFilter);
            Assert.AreEqual(typeof(CategoryFilter), categoryFilter.GetType());
            Assert.IsNotNull(priorityFilter);
            Assert.AreEqual(typeof(PriorityFilter), priorityFilter.GetType());
        }

        [TestMethod]
        public void ShouldThrowNullExceptionIfLoggingConfigurationIsNull()
        {
            LoggingConfiguration config = null;

            AssertEx.Throws<ArgumentNullException>(() => new LogWriter(config));
        }

        [TestMethod]
        public void DisposingWriterDisposesAllTraceListenersOnce()
        {
            var traceListener = new MockDisposableTraceListener();

            var config = new LoggingConfiguration();
            config.AddLogSource("cat1", traceListener);
            config.AddLogSource("cat2", traceListener);
            config.SpecialSources.AllEvents.Listeners.Add(traceListener);

            var logWriter = new LogWriter(config);
            logWriter.Dispose();

            Assert.AreEqual(1, traceListener.DisposedCalls);
        }

        [TestMethod]
        public void DisposingWriterThrowsObjectDisposedExceptionWhenUsed()
        {
            var writer = new LogWriter(new LoggingConfiguration());
            writer.Dispose();

            AssertEx.Throws<ObjectDisposedException>(() => writer.Write("Should throw."));
        }
    }
}
