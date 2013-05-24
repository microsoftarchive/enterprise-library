#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Config
{
    [TestClass]
    public class LoggingConfigurationFixture
    {
        [TestMethod]
        public void AddsLogSourceToCollection()
        {
            var config = new LoggingConfiguration();
            config.AddLogSource("mock", new MockTraceListener());

            Assert.AreEqual<int>(1, config.LogSources["mock"].Listeners.Count);
        }

        [TestMethod]
        public void TracingIsEnabledByDefault()
        {
            var config = new LoggingConfiguration();

            Assert.IsTrue(config.IsTracingEnabled);
        }

        [TestMethod]
        public void FiltersListIsNotNull()
        {
            var config = new LoggingConfiguration();

            Assert.IsNotNull(config.Filters);
        }

        [TestMethod]
        public void SpecialLogSourcesAreNotNull()
        {
            var config = new LoggingConfiguration();

            Assert.IsNotNull(config.SpecialSources.AllEvents);
            Assert.IsNotNull(config.SpecialSources.LoggingErrorsAndWarnings);
            Assert.IsNotNull(config.SpecialSources.Unprocessed);
        }

        [TestMethod]
        public void LogSourcesCollectionIsNotNull()
        {
            var config = new LoggingConfiguration();

            Assert.IsNotNull(config.LogSources);
        }

        [TestMethod]
        public void LoggingIsEnabledByDefault()
        {
            var config = new LoggingConfiguration();

            Assert.IsTrue(config.IsLoggingEnabled);
        }

        [TestMethod]
        public void SetIsLogginEnabledToValseAddsFilter()
        {
            var config = new LoggingConfiguration();

            Assert.IsTrue(config.IsLoggingEnabled);

            config.IsLoggingEnabled = false;

            Assert.IsFalse(config.IsLoggingEnabled);

            Assert.AreEqual<int>(1, config.Filters.OfType<LogEnabledFilter>().Count());
        }

        [TestMethod]
        public void SharedTraceListener()
        {
            var sharedTraceListener = new DefaultTraceListener();

            var config = new LoggingConfiguration();
            config.AddLogSource("General").AddTraceListener(sharedTraceListener);
            config.AddLogSource("Special", SourceLevels.Critical, true, sharedTraceListener);

            Assert.AreEqual<int>(2, config.LogSources.Count);
            Assert.AreEqual<string>("General", config.LogSources.First().Name);
            Assert.AreEqual<string>("Special", config.LogSources.Last().Name);
        }

        [TestMethod]
        public void ShouldHaveDefaultSourceLevel()
        {
            var config = new LoggingConfiguration();
            config.AddLogSource("Default");

            Assert.AreEqual<SourceLevels>(SourceLevels.All, config.LogSources["Default"].Level);
        }

        [TestMethod]
        public void ShouldNotAutoflushByDefault()
        {
            var config = new LoggingConfiguration();
            config.AddLogSource("Default");

            Assert.AreEqual<bool>(false, config.LogSources["Default"].AutoFlush);
        }

        [TestMethod]
        public void LogFiltersArePassedToLogWriter()
        {
            var config = new LoggingConfiguration();
            config.Filters.Add(new MockLogFilter());

            var logger = new LogWriter(config);

            Assert.IsNotNull(logger.GetFilter<MockLogFilter>());
            Assert.AreEqual<string>("Mock", logger.GetFilter<MockLogFilter>().Name);
        }

        [TestMethod]
        public void ConfigurationIsCopiedNotReused()
        {
            var config = new LoggingConfiguration();

            var logger = new LogWriter(config);

            Assert.IsTrue(logger.IsTracingEnabled());

            config.IsTracingEnabled = false;

            Assert.IsTrue(logger.IsTracingEnabled());
        }

        [TestMethod]
        public void DontUseImpersonationByDefault()
        {
            var config = new LoggingConfiguration();

            Assert.IsFalse(config.UseImpersonation);
        }

        [TestMethod]
        public void WarnIfNoCategoriesAreMatchIsEnabled()
        {
            var config = new LoggingConfiguration();

            Assert.IsTrue(config.LogWarningsWhenNoCategoriesMatch);
        }

        [TestMethod]
        public void ReconfigureImpersonation()
        {
            var config = new LoggingConfiguration();
            config.UseImpersonation = false;

            var logger = new LogWriter(config);

            // Is it false?
            logger.Configure(cfg => Assert.IsFalse(cfg.UseImpersonation));

            // Use impersonation
            logger.Configure(cfg => cfg.UseImpersonation = true);

            // Did it change?
            logger.Configure(cfg => Assert.IsTrue(cfg.UseImpersonation));
        }

        [TestMethod]
        public void ReconfigureTraceEnabled()
        {
            var config = new LoggingConfiguration();
            config.IsTracingEnabled = false;

            var logger = new LogWriter(config);

            // Is it false?
            logger.Configure(cfg => Assert.IsFalse(cfg.UseImpersonation));

            // Toggle
            logger.Configure(cfg => cfg.IsTracingEnabled = true);

            // Did it change?
            logger.Configure(cfg => Assert.IsTrue(cfg.IsTracingEnabled));
        }

        [TestMethod]
        public void ReconfigureIsLoggingEnabled()
        {
            var config = new LoggingConfiguration();

            Assert.IsTrue(config.IsLoggingEnabled);

            var logger = new LogWriter(config);

            Assert.IsTrue(logger.IsLoggingEnabled());

            // Disable logging
            logger.Configure(cfg => cfg.IsLoggingEnabled = false);

            Assert.IsFalse(logger.IsLoggingEnabled());
        }

        [TestMethod]
        public void SpecialSourcesDefaults()
        {
            var config = new LoggingConfiguration();

            // All events
            Assert.AreEqual<string>("All Events", config.SpecialSources.AllEvents.Name);
            Assert.AreEqual<bool>(true, config.SpecialSources.AllEvents.AutoFlush);
            Assert.AreEqual<SourceLevels>(SourceLevels.All, config.SpecialSources.AllEvents.Level);
            Assert.AreEqual<int>(0, config.SpecialSources.AllEvents.Listeners.Count);

            // Unprocessed
            Assert.AreEqual<string>("Unprocessed Category", config.SpecialSources.Unprocessed.Name);
            Assert.AreEqual<bool>(true, config.SpecialSources.Unprocessed.AutoFlush);
            Assert.AreEqual<SourceLevels>(SourceLevels.All, config.SpecialSources.Unprocessed.Level);
            Assert.AreEqual<int>(0, config.SpecialSources.Unprocessed.Listeners.Count);

            // Logging Errors & Warnings
            Assert.AreEqual<string>("Logging Errors & Warnings", config.SpecialSources.LoggingErrorsAndWarnings.Name);
            Assert.AreEqual<bool>(true, config.SpecialSources.LoggingErrorsAndWarnings.AutoFlush);
            Assert.AreEqual<SourceLevels>(SourceLevels.All, config.SpecialSources.LoggingErrorsAndWarnings.Level);
            Assert.AreEqual<int>(0, config.SpecialSources.LoggingErrorsAndWarnings.Listeners.Count);
        }

        [TestMethod]
        public void AddTraceListenerToSpecialSource()
        {
            var config = new LoggingConfiguration();

            config.SpecialSources.LoggingErrorsAndWarnings.AddTraceListener(new DefaultTraceListener());

            Assert.AreEqual<int>(1, config.SpecialSources.LoggingErrorsAndWarnings.Listeners.Count);
        }

        [TestMethod]
        public void AddLogSourcesIsSetAsDefault()
        {
            var config = new LoggingConfiguration();

            Assert.IsNull(config.DefaultSource);

            config.AddLogSource("Default");

            Assert.AreEqual<string>("Default", config.DefaultSource);

            config.AddLogSource("Second");

            Assert.AreEqual<string>("Default", config.DefaultSource);
        }

        [TestMethod]
        public void ReconfigureDisposesUnusedListeners()
        {
            var traceListener = new MockDisposableTraceListener();

            var config = new LoggingConfiguration();
            config.AddLogSource("General", traceListener);

            var logWriter = new LogWriter(config);
            logWriter.Configure((cfg) =>
            {
                cfg.LogSources.Clear();

                cfg.AddLogSource("New", new DefaultTraceListener());
            });

            Assert.AreEqual(1, traceListener.DisposedCalls);
        }

        [TestMethod]
        public void ReconfigureDoesNotDisposeListenersStillUsedInSpecialSources()
        {
            var traceListener = new MockDisposableTraceListener();

            var config = new LoggingConfiguration();
            config.AddLogSource("General", traceListener);

            var logWriter = new LogWriter(config);
            logWriter.Configure((cfg) =>
            {
                cfg.LogSources.Clear();

                cfg.AddLogSource("New", new DefaultTraceListener());
                cfg.SpecialSources.AllEvents.Listeners.Add(traceListener);
            });

            Assert.AreEqual(0, traceListener.DisposedCalls);
        }

        [TestMethod]
        public void ThrowsArgumentNullIfAddLogSourceIsNull()
        {
            MockDisposableTraceListener listener = null;

            var config = new LoggingConfiguration();

            AssertEx.Throws<ArgumentNullException>(() => config.AddLogSource("test", listener));
        }

        [TestMethod]
        public void CanAddAsynchronousTraceListener()
        {
            var listener = new MockTraceListener { Name = "listener", Filter = new EventTypeFilter(SourceLevels.Critical) };

            var config = new LoggingConfiguration();

            config.AddLogSource("test").AddAsynchronousTraceListener(listener);

            var addedListener = (AsynchronousTraceListenerWrapper)config.AllTraceListeners.First();

            Assert.AreSame(listener, addedListener.TraceListener);
            Assert.AreEqual("listener", addedListener.Name);
            Assert.AreEqual(SourceLevels.Critical, ((EventTypeFilter)addedListener.Filter).EventType);
        }

        [TestMethod]
        public void CanAddAsynchronousTraceListenerToSpecialSource()
        {
            var listener = new MockTraceListener { Name = "listener", Filter = new EventTypeFilter(SourceLevels.Critical) };

            var config = new LoggingConfiguration();

            config.SpecialSources.LoggingErrorsAndWarnings.AddAsynchronousTraceListener(listener);

            var addedListener = (AsynchronousTraceListenerWrapper)config.SpecialSources.LoggingErrorsAndWarnings.Listeners.First();

            Assert.AreSame(listener, addedListener.TraceListener);
            Assert.AreEqual("listener", addedListener.Name);
            Assert.AreEqual(SourceLevels.Critical, ((EventTypeFilter)addedListener.Filter).EventType);
        }

        [TestMethod]
        public void CanGetAllTraceListeners()
        {
            var listener1 = new MockTraceListener();
            var listener2 = new MockTraceListener();
            var listener3 = new MockTraceListener();
            var listener4 = new MockTraceListener();

            var config = new LoggingConfiguration();

            config.AddLogSource("source1", listener1, listener2);
            config.AddLogSource("source2", listener2);
            config.AddLogSource("source3", listener1, listener3);
            config.AddLogSource("source4");

            config.SpecialSources.Unprocessed.AddTraceListener(listener2);
            config.SpecialSources.Unprocessed.AddTraceListener(listener4);

            var allListeners = config.AllTraceListeners.ToArray();

            Assert.AreEqual(4, allListeners.Length);
            CollectionAssert.AreEquivalent(new TraceListener[] { listener1, listener2, listener3, listener4 }, allListeners);
        }
    }
}
