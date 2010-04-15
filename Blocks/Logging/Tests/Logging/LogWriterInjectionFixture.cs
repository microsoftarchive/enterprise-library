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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class GivenLoggingUpdateCoordinator
    {
        private MockLoggingUpdateCoordinator coordinator;

        [TestInitialize]
        public void Setup()
        {
            coordinator = new MockLoggingUpdateCoordinator();
        }

        [TestMethod]
        public void WhenChangeSourceIsInjectedOnLogWriter_ThenGetsRegisteredForChangeNotificationOnTheLoggingSettings()
        {
            var logWriter =
                new LogWriterImpl(
                    new LogWriterStructureHolder(
                        new ILogFilter[0],
                        new Dictionary<string, LogSource>(),
                        new LogSource(null),
                        new LogSource(null),
                        new LogSource(null),
                        null,
                        false,
                        false,
                        false),
                    new LoggingInstrumentationProvider(false, false, null),
                    coordinator);

            Assert.AreSame(logWriter, coordinator.AddedLoggingUpdateHandler);
        }
    }

    [TestClass]
    public class GivenANullLoggingUpdateCoordinator
    {
        private ILoggingUpdateCoordinator coordinator;

        [TestInitialize]
        public void Setup()
        {
            coordinator = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenUpdateCoordinateIsInjectedOnLogWriter_ThenArgumentNullExceptionIsThrown()
        {
            new LogWriterImpl(
                new LogWriterStructureHolder(
                    new ILogFilter[0],
                    new Dictionary<string, LogSource>(),
                    new LogSource(null),
                    new LogSource(null),
                    new LogSource(null),
                    null,
                    false,
                    false,
                    false),
                new LoggingInstrumentationProvider(false, false, null),
                coordinator);
        }
    }

    [TestClass]
    public class GivenALogWriterInjectedWithAConfigurationChangeEventSource
    {
        private MockLoggingUpdateCoordinator coordinator;
        private LogWriter logWriter;

        [TestInitialize]
        public void Setup()
        {
            coordinator = new MockLoggingUpdateCoordinator();
            logWriter =
                new LogWriterImpl(
                    new LogWriterStructureHolder(
                        new ILogFilter[0],
                        new Dictionary<string, LogSource>(),
                        new LogSource(null),
                        new LogSource(null),
                        new LogSource(null),
                        null,
                        false,
                        false,
                        false),
                    new LoggingInstrumentationProvider(false, false, null),
                    coordinator);
        }

        [TestMethod]
        public void WhenLogWriterIsDisposed_ThenRegisteredHandlerIsRemoved()
        {
            this.logWriter.Dispose();
            Assert.AreSame(coordinator.RemovedLoggingUpdateHandler, this.logWriter);
        }
    }

    [TestClass]
    public class GivenLogWriterInjectedWithLoggingStack
    {
        private MockLoggingUpdateCoordinator coordinator;
        private MockTraceListener traceListener;
        private LogWriter logWriter;
        private MockLoggingInstrumentationProvider instrumentationProvider;

        [TestInitialize]
        public void Setup()
        {
            this.coordinator = new MockLoggingUpdateCoordinator();
            this.traceListener = new MockTraceListener("original");
            this.instrumentationProvider = new MockLoggingInstrumentationProvider();
            this.logWriter =
                new LogWriterImpl(
                    new LogWriterStructureHolder(
                        new ILogFilter[0],
                        new Dictionary<string, LogSource>(),
                        new LogSource("all", new[] { traceListener }, SourceLevels.All),
                        new LogSource("not processed"),
                        new LogSource("error"),
                        "default",
                        false,
                        false,
                        false),
                    this.instrumentationProvider,
                    coordinator);
        }

        [TestMethod]
        public void WhenLogging_ThenLogWriterWritesToTheInjectedStack()
        {
            var logEntry = new LogEntry() { Message = "message" };
            this.logWriter.Write(logEntry);

            Assert.AreSame(logEntry, this.traceListener.tracedData);
        }

        [TestMethod]
        public void WhenLogWriterIsDisposed_ThenTraceListenerIsDisposed()
        {
            this.logWriter.Dispose();

            Assert.IsTrue(this.traceListener.wasDisposed);
        }

        [TestMethod]
        public void WhenSourceChangedEventIsFired_ThenNewStackIsResolvedToReplaceTheExistingOne()
        {
            var newTraceListener = new MockTraceListener("new");
            var newStack =
                new LogWriterStructureHolder(
                    new ILogFilter[0],
                    new Dictionary<string, LogSource>(),
                    new LogSource("all", new[] { newTraceListener }, SourceLevels.All),
                    new LogSource("not processed"),
                    new LogSource("error"),
                    "default",
                    false,
                    false,
                    false);
            var container = new UnityContainer();
            container.RegisterInstance(newStack);

            this.coordinator.RaiseLoggingUpdate(new UnityServiceLocator(container));

            var logEntry = new LogEntry() { Message = "message" };
            this.logWriter.Write(logEntry);

            Assert.AreSame(logEntry, newTraceListener.tracedData);
        }
    }

    public class ExceptionThrowingServiceLocator : IServiceLocator
    {
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public TService GetInstance<TService>(string key)
        {
            throw new ActivationException("not available");
        }

        public TService GetInstance<TService>()
        {
            throw new ActivationException("not available");
        }

        public object GetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }

        public object GetInstance(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    public class MockLoggingInstrumentationProvider : ILoggingInstrumentationProvider
    {
        public int ConfigurationFailureEventCalls = 0;

        public void FireLockAcquisitionError(string message)
        {
        }

        public void FireConfigurationFailureEvent(Exception configurationException)
        {
            ConfigurationFailureEventCalls++;
        }

        public void FireFailureLoggingErrorEvent(string message, Exception exception)
        {
        }

        public void FireLogEventRaised()
        {
        }

        public void FireTraceListenerEntryWrittenEvent()
        {
        }

        public void FireReconfigurationErrorEvent(Exception exception)
        {
        }
    }
}
