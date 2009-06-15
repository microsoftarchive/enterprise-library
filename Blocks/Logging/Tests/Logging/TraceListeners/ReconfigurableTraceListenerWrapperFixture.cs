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
using System.Diagnostics;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
    [TestClass]
    public class GivenATraceListenerAndANullLoggingUpdateEventSource
    {
        private MockTraceListener traceListener;
        private ILoggingUpdateCoordinator eventSource;

        [TestInitialize]
        public void Setup()
        {
            this.traceListener = new MockTraceListener("mock");
            this.eventSource = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingASelfUpdatingTraceListenerWrapper_ThenArgumentNullExceptionIsThrown()
        {
            new ReconfigurableTraceListenerWrapper(this.traceListener, this.eventSource);
        }
    }

    [TestClass]
    public class GivenALoggingUpdateEventSourceAndANullTraceListener
    {
        private MockTraceListener traceListener;
        private ILoggingUpdateCoordinator eventSource;

        [TestInitialize]
        public void Setup()
        {
            this.traceListener = null;
            this.eventSource = new MockLoggingUpdateCoordinator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingASelfUpdatingTraceListenerWrapper_ThenArgumentNullExceptionIsThrown()
        {
            new ReconfigurableTraceListenerWrapper(this.traceListener, this.eventSource);
        }
    }

    [TestClass]
    public class GivenALoggingUpdateEventSourceAndATraceListener
    {
        private MockTraceListener traceListener;
        private MockLoggingUpdateCoordinator eventSource;

        [TestInitialize]
        public void Setup()
        {
            this.traceListener = new MockTraceListener("mock");
            this.eventSource = new MockLoggingUpdateCoordinator();
        }

        [TestMethod]
        public void WhenCreatingASelfUpdatingTraceListenerWrapper_ThenTheALoggingUpdateHandlerIsRegistered()
        {
            var wrapper = new ReconfigurableTraceListenerWrapper(this.traceListener, this.eventSource);

            Assert.AreSame(wrapper, this.eventSource.AddedLoggingUpdateHandler);
        }
    }

    [TestClass]
    public class GivenASelfUpdatingTraceListenerWrapper
    {
        private MockTraceListener traceListener;
        private MockLoggingUpdateCoordinator coordinator;
        private ReconfigurableTraceListenerWrapper wrapperTraceListener;

        [TestInitialize]
        public void Setup()
        {
            this.traceListener = new MockTraceListener("mock");
            this.coordinator = new MockLoggingUpdateCoordinator();
            this.wrapperTraceListener = new ReconfigurableTraceListenerWrapper(this.traceListener, this.coordinator);
        }

        [TestMethod]
        public void WhenTracingThroughTheWrapper_ThenTheRequestIsForwardedToTheWrappedTraceListener()
        {
            var cache = new TraceEventCache();
            var data = new object();

            this.wrapperTraceListener.TraceData(cache, "source", TraceEventType.Critical, 100, data);

            Assert.AreSame(data, this.traceListener.tracedData);
            Assert.AreEqual("source", this.traceListener.tracedSource);
            Assert.AreEqual(TraceEventType.Critical, this.traceListener.tracedEventType);
        }

        [TestMethod]
        public void WhenTheLoggingUpdateEventIsTriggered_ThenTheWrapperStartsUsingANewTraceListenerResolvedFromTheLocatorInTheEventArguments()
        {
            var newTraceListener = new MockTraceListener("mock");
            var container = new UnityContainer().RegisterInstance<TraceListener>("mock", newTraceListener);

            this.coordinator.RaiseLoggingUpdate(new UnityServiceLocator(container));

            var cache = new TraceEventCache();
            var data = new object();

            this.wrapperTraceListener.TraceData(cache, "source", TraceEventType.Critical, 100, data);

            Assert.IsNull(this.traceListener.tracedData);
            Assert.IsNull(this.traceListener.tracedSource);
            Assert.IsNull(this.traceListener.tracedEventType);
            Assert.AreSame(data, newTraceListener.tracedData);
            Assert.AreEqual("source", newTraceListener.tracedSource);
            Assert.AreEqual(TraceEventType.Critical, newTraceListener.tracedEventType);
        }

        [TestMethod]
        public void WhenTheLoggingUpdateEventIsTriggered_ThenThePreviousTraceListenerIsDisposed()
        {
            var newTraceListener = new MockTraceListener("mock");
            var container = new UnityContainer().RegisterInstance<TraceListener>("mock", newTraceListener);

            this.coordinator.RaiseLoggingUpdate(new UnityServiceLocator(container));

            Assert.IsTrue(this.traceListener.wasDisposed);
        }

        [TestMethod]
        public void WhenTheWrapperIsDisposed_ThenItUnregistersItselfFromTheCoordinator()
        {
            this.wrapperTraceListener.Dispose();

            Assert.AreSame(this.wrapperTraceListener, this.coordinator.RemovedLoggingUpdateHandler);
        }

        [TestMethod]
        public void WhenTheWrapperIsDisposed_ThenItDisposesItsWrappedTraceListener()
        {
            this.wrapperTraceListener.Dispose();

            Assert.IsTrue(this.traceListener.wasDisposed);
        }
    }
}
