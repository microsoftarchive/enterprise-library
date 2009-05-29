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
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class GivenConfigurationChangeSource
    {
        private MockConfigurationChangeEventSource changeSource;

        [TestInitialize]
        public void Setup()
        {
            changeSource = new MockConfigurationChangeEventSource();
        }

        [TestMethod]
        public void WhenChangeSourceIsInjectedOnLogWriter_ThenGetsRegisteredForChangeNotificationOnTheLoggingSettings()
        {
            var logWriter =
                new LogWriter(
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
                    new LoggingInstrumentationProvider(false, false, false, null),
                    changeSource);

            Assert.IsTrue(changeSource.handlers.ContainsKey(typeof(LoggingSettings)));
        }
    }

    [TestClass]
    public class GivenANullConfigurationChangeSource
    {
        private ConfigurationChangeEventSource changeSource;

        [TestInitialize]
        public void Setup()
        {
            changeSource = null;
        }

        [TestMethod]
        public void WhenChangeSourceIsInjectedOnLogWriter_ThenNoExceptionIsThrown()
        {
            var logWriter =
                new LogWriter(
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
                    new LoggingInstrumentationProvider(false, false, false, null),
                    changeSource);
        }
    }

    [TestClass]
    public class GivenALogWriterInjectedWithAConfigurationChangeEventSource
    {
        private MockConfigurationChangeEventSource changeSource;
        private LogWriter logWriter;

        [TestInitialize]
        public void Setup()
        {
            changeSource = new MockConfigurationChangeEventSource();
            logWriter =
                new LogWriter(
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
                    new LoggingInstrumentationProvider(false, false, false, null),
                    changeSource);
        }

        [TestMethod]
        public void WhenLogWriterIsDisposed_ThenSourceChangHandlerIsRemoved()
        {
            this.logWriter.Dispose();

            Assert.IsFalse(changeSource.handlers.ContainsKey(typeof(LoggingSettings)));
        }
    }

    [TestClass]
    public class GivenLogWriterInjectedWithLoggingStack
    {
        private MockConfigurationChangeEventSource changeSource;
        private MockTraceListener traceListener;
        private MockTraceListener errorTraceListener;
        private LogWriter logWriter;
        private MockLoggingInstrumentationProvider instrumentationProvider;

        [TestInitialize]
        public void Setup()
        {
            this.changeSource = new MockConfigurationChangeEventSource();
            this.traceListener = new MockTraceListener("original");
            this.instrumentationProvider = new MockLoggingInstrumentationProvider();
            this.logWriter =
                new LogWriter(
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
                    changeSource);
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

            changeSource.OnSectionChanged(
                new SectionChangedEventArgs<LoggingSettings>(null, new UnityServiceLocator(container)));

            var logEntry = new LogEntry() { Message = "message" };
            this.logWriter.Write(logEntry);

            Assert.AreSame(logEntry, newTraceListener.tracedData);
        }

        [TestMethod]
        public void WhenSourceChangedEventIsFired_ThenOldStackIsDisposed()
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

            this.changeSource.OnSectionChanged(
                new SectionChangedEventArgs<LoggingSettings>(null, new UnityServiceLocator(container)));

            Assert.IsTrue(this.traceListener.wasDisposed);
        }

        [TestMethod]
        public void WhenSourceChangedEventIsFiredAndTheNewStackFailsToBeResolved_ThenAnErrorIsLogged()
        {
            this.changeSource.OnSectionChanged(
                new SectionChangedEventArgs<LoggingSettings>(null, new ExceptionThrowingServiceLocator()));

            Assert.AreEqual(1, this.instrumentationProvider.ConfigurationFailureEventCalls);
        }
    }

    public class MockConfigurationChangeEventSource : ConfigurationChangeEventSource
    {
        public Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

        public override ConfigurationChangeEventSource.ISourceChangeEventSource<TSection> GetSection<TSection>()
        {
            return new SourceChangeEventSource<TSection>(this);
        }

        private class SourceChangeEventSource<T> : ConfigurationChangeEventSource.ISourceChangeEventSource<T>
            where T : ConfigurationSection
        {
            private MockConfigurationChangeEventSource owner;

            public SourceChangeEventSource(MockConfigurationChangeEventSource owner)
            {
                this.owner = owner;
            }

            public event EventHandler<SectionChangedEventArgs<T>> SectionChanged
            {
                add { this.owner.handlers[typeof(T)] = value; }
                remove { this.owner.handlers.Remove(typeof(T)); }
            }
        }

        public void OnSectionChanged<T>(SectionChangedEventArgs<T> sectionChangedEventArgs)
            where T : ConfigurationSection
        {
            Delegate handler = this.handlers[typeof(T)];
            handler.DynamicInvoke(this, sectionChangedEventArgs);
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
    }
}
