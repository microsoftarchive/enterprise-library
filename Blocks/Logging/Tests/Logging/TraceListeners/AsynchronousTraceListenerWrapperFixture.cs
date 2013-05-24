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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.AsynchronousTraceListenerWrapperFixture
{
    [TestClass]
    public class when_creating_with_null_trace_listener
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void then_throws()
        {
            new AsynchronousTraceListenerWrapper(null);
        }
    }

    [TestClass]
    public class when_creating_with_zero_buffer_size
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void then_throws()
        {
            new AsynchronousTraceListenerWrapper(new DefaultTraceListener(), bufferSize: 0);
        }
    }

    [TestClass]
    public class when_creating_with_negative_buffer_size
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void then_throws()
        {
            new AsynchronousTraceListenerWrapper(new DefaultTraceListener(), bufferSize: -10);
        }
    }

    [TestClass]
    public class when_disposing_wrapper_with_owned_listener : ArrangeActAssert
    {
        private DisposableTraceListener wrapped;
        private AsynchronousTraceListenerWrapper wrapper;

        protected override void Arrange()
        {
            this.wrapped = new DisposableTraceListener();
            this.wrapper = new AsynchronousTraceListenerWrapper(this.wrapped, ownsWrappedTraceListener: true);
        }

        protected override void Act()
        {
            this.wrapper.Dispose();
        }

        [TestMethod]
        public void then_listener_is_closed()
        {
            Assert.IsTrue(this.wrapped.closed);
        }
    }

    [TestClass]
    public class when_disposing_wrapper_with_non_owned_listener : ArrangeActAssert
    {
        private DisposableTraceListener wrapped;
        private AsynchronousTraceListenerWrapper wrapper;

        protected override void Arrange()
        {
            this.wrapped = new DisposableTraceListener();
            this.wrapper = new AsynchronousTraceListenerWrapper(this.wrapped, ownsWrappedTraceListener: false);
        }

        protected override void Act()
        {
            this.wrapper.Dispose();
        }

        [TestMethod]
        public void then_listener_is_not_closed()
        {
            Assert.IsFalse(this.wrapped.closed);
        }
    }

    public class given_wrapper_on_thread_safe_listener : ArrangeActAssert
    {
        protected WrappedTraceListener wrappedListener;
        protected AsynchronousTraceListenerWrapper wrapper;
        protected SemaphoreSlim continueEvent;

        protected override void Arrange()
        {
            this.continueEvent = new SemaphoreSlim(0, 2);
            this.wrappedListener = new WrappedTraceListener(true, this.continueEvent);
            this.wrapper = new AsynchronousTraceListenerWrapper(this.wrappedListener, bufferSize: 10, maxDegreeOfParallelism: 2);
        }

        protected override void Teardown()
        {
            this.wrapper.Close();
        }

        [TestClass]
        public class then_closing_multiple_times : given_wrapper_on_thread_safe_listener
        {
            protected override void Act()
            {
                this.wrapper.Close();
                this.wrapper.Close();
                this.wrapper.Close();
            }

            [TestMethod]
            public void does_not_throw()
            {
            }
        }

        [TestClass]
        public class when_writing_data : given_wrapper_on_thread_safe_listener
        {
            private object entry;
            private AutoResetEvent writeCompleted;
            private AutoResetEvent exceptionWritten;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new object();
                this.writeCompleted = new AutoResetEvent(false);
                this.exceptionWritten = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry, (e, d, s) => this.exceptionWritten.Set());
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.AreSame(this.entry, entry);
            }

            [TestMethod]
            public void then_no_exception_is_logged()
            {
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsFalse(this.exceptionWritten.WaitOne(TimeSpan.FromSeconds(0.1)));
            }
        }

        [TestClass]
        public class when_writing_data_with_null_reporting_delegate : given_wrapper_on_thread_safe_listener
        {
            private object entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new object();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry, null);
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.AreSame(this.entry, entry);
            }
        }

        [TestClass]
        public class when_writing_data_with_filter : given_wrapper_on_thread_safe_listener
        {
            private object entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.wrapper.Filter = new EventTypeFilter(SourceLevels.Critical);

                this.entry = new object();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry);
            }

            [TestMethod]
            public void then_data_is_not_written()
            {
                this.continueEvent.Release();
                Assert.IsFalse(this.writeCompleted.WaitOne(TimeSpan.FromSeconds(0.1)));
            }
        }

        [TestClass]
        public class when_writing_transfer : given_wrapper_on_thread_safe_listener
        {
            private string message;
            private AutoResetEvent writeCompleted;
            private AutoResetEvent exceptionWritten;
            private Guid relatedActivityId;

            protected override void Arrange()
            {
                base.Arrange();

                this.message = "some message";
                this.relatedActivityId = Guid.NewGuid();
                this.writeCompleted = new AutoResetEvent(false);
                this.exceptionWritten = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceTransfer(new TraceEventCache(), "source", 100, this.message, this.relatedActivityId, (e, d, s) => this.exceptionWritten.Set());
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.IsTrue(entry.ToString().Contains(this.message));
                Assert.IsTrue(entry.ToString().Contains(this.relatedActivityId.ToString()));
            }

            [TestMethod]
            public void then_no_exception_is_logged()
            {
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsFalse(this.exceptionWritten.WaitOne(TimeSpan.FromSeconds(0.1)));
            }
        }

        [TestClass]
        public class when_writing_transfer_with_null_reporting_delegate : given_wrapper_on_thread_safe_listener
        {
            private string message;
            private AutoResetEvent writeCompleted;
            private Guid relatedActivityId;

            protected override void Arrange()
            {
                base.Arrange();

                this.message = "some message";
                this.relatedActivityId = Guid.NewGuid();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceTransfer(new TraceEventCache(), "source", 100, this.message, this.relatedActivityId, null);
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.IsTrue(entry.ToString().Contains(this.message));
                Assert.IsTrue(entry.ToString().Contains(this.relatedActivityId.ToString()));
            }
        }

        [TestClass]
        public class when_writing_log_entry : given_wrapper_on_thread_safe_listener
        {
            private LogEntry entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new LogEntry();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry, (e, d, s) => { });
            }

            [TestMethod]
            public void then_data_is_written_asynchronously_with_captured_context_information()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();
                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.AreSame(this.entry, entry);
            }
        }

        [TestClass]
        public class when_writing_multiple_entries : given_wrapper_on_thread_safe_listener
        {
            private CountdownEvent writesCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesCompleted = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.continueEvent.Release();
                this.wrappedListener.OperationCompleted += (s, e) => this.writesCompleted.Signal();
            }

            protected override void Act()
            {
                this.writesCompleted.AddCount(6);

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one", (e, d, s) => { });
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "two", (e, d, s) => { });
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "three", (e, d, s) => { });
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "four", (e, d, s) => { });
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "five", (e, d, s) => { });
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "six", (e, d, s) => { });

                this.writesCompleted.Signal();
            }

            [TestMethod]
            public void then_data_is_written_asynchronously_in_order()
            {
                this.writesCompleted.Wait();

                CollectionAssert.AreEquivalent(new[] { "one", "two", "three", "four", "five", "six" }, this.wrappedListener.requests);
            }
        }

        [TestClass]
        public class when_flushing_then_enqueues_flush_request : given_wrapper_on_thread_safe_listener
        {
            private CountdownEvent writesCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesCompleted = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.continueEvent.Release();
                this.wrappedListener.OperationCompleted += (s, e) => this.writesCompleted.Signal();
            }

            protected override void Act()
            {
                this.writesCompleted.AddCount(4);

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one", (e, d, s) => { });
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "two", (e, d, s) => { });
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "three", (e, d, s) => { });
                this.wrapper.Flush((e, d, s) => { });

                this.writesCompleted.Signal();
            }

            [TestMethod]
            public void then_data_is_written_asynchronously_in_order()
            {
                this.writesCompleted.Wait();

                CollectionAssert.AreEquivalent(new[] { "one", "two", "three", "flush" }, this.wrappedListener.requests);
            }
        }

        [TestClass]
        public class when_multiple_threads_issue_simultaneous_requests : given_wrapper_on_thread_safe_listener
        {
            private CountdownEvent writesRequested;
            private CountdownEvent writesCompleted;
            private Exception exception;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesRequested = new CountdownEvent(1);
                this.writesCompleted = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.writesRequested.Signal();
                this.wrappedListener.OperationCompleted += (s, e) => this.writesCompleted.Signal();
            }

            protected override void Act()
            {
                this.writesRequested.AddCount(1);
                this.writesCompleted.AddCount(1);

                Task.Run(() => this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one", (e, d, s) => this.exception = e));

                this.writesRequested.Signal();
                this.writesCompleted.Signal();

                this.writesRequested.Wait();
            }

            [TestMethod]
            public void then_wrapped_listener_is_not_locked()
            {
                try
                {
                    Assert.IsTrue(Monitor.TryEnter(this.wrappedListener));
                }
                finally
                {
                    if (Monitor.IsEntered(this.wrappedListener)) Monitor.Exit(this.wrappedListener);
                }

                this.continueEvent.Release();
                this.writesCompleted.Wait();

                CollectionAssert.AreEquivalent(new[] { "one" }, this.wrappedListener.requests);

                Assert.IsNull(this.exception);
            }
        }

        [TestClass]
        public class when_tracing_throws : given_wrapper_on_thread_safe_listener
        {
            private AutoResetEvent writeCompleted;
            private AutoResetEvent exceptionWritten;

            protected override void Arrange()
            {
                base.Arrange();

                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();

                this.exceptionWritten = new AutoResetEvent(false);
            }

            protected override void Act()
            {
                this.wrapper.TraceData(
                    new TraceEventCache(),
                    "test source",
                    TraceEventType.Error,
                    100,
                    "throw",
                    (e, d, s) => this.exceptionWritten.Set());
            }

            [TestMethod]
            public void then_message_is_logged()
            {
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.exceptionWritten.WaitOne(TimeSpan.FromSeconds(0.1)));
            }
        }

        [TestClass]
        public class when_tracing_with_null_delegate_throws : given_wrapper_on_thread_safe_listener
        {
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(
                    new TraceEventCache(),
                    "test source",
                    TraceEventType.Error,
                    100,
                    "throw",
                    null);
            }

            [TestMethod]
            public void then_operation_is_completed()
            {
                this.continueEvent.Release();
                Assert.IsTrue(this.writeCompleted.WaitOne(TimeSpan.FromSeconds(0.2)));
            }
        }

        [TestClass]
        public class when_transfer_throws : given_wrapper_on_thread_safe_listener
        {
            private AutoResetEvent writeCompleted;
            private AutoResetEvent exceptionWritten;

            protected override void Arrange()
            {
                base.Arrange();

                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();

                this.exceptionWritten = new AutoResetEvent(false);
            }

            protected override void Act()
            {
                this.wrapper.TraceTransfer(
                    new TraceEventCache(),
                    "test source",
                    100,
                    "throw",
                    Guid.NewGuid(),
                    (e, d, s) => this.exceptionWritten.Set());
            }

            [TestMethod]
            public void then_message_is_logged()
            {
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.exceptionWritten.WaitOne(TimeSpan.FromSeconds(0.1)));
            }
        }

        [TestClass]
        public class when_transfer_with_null_delegate_throws : given_wrapper_on_thread_safe_listener
        {
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceTransfer(
                    new TraceEventCache(),
                    "test source",
                    100,
                    "throw",
                    Guid.NewGuid(),
                    null);
            }

            [TestMethod]
            public void then_operation_is_completed()
            {
                this.continueEvent.Release();
                Assert.IsTrue(this.writeCompleted.WaitOne(TimeSpan.FromSeconds(0.2)));
            }
        }

        [TestClass]
        public class when_writing_multiple_entries_with_some_failures : given_wrapper_on_thread_safe_listener
        {
            private CountdownEvent writesCompleted;
            private CountdownEvent exceptionsWritten;
            private BlockingCollection<string> exceptions;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesCompleted = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.continueEvent.Release();
                this.wrappedListener.OperationCompleted += (s, e) => this.writesCompleted.Signal();

                this.exceptions = new BlockingCollection<string>();
                this.exceptionsWritten = new CountdownEvent(1);
            }

            protected override void Act()
            {
                this.writesCompleted.AddCount(6);
                this.exceptionsWritten.AddCount(2);

                ReportTracingError reportError = (e, d, s) => { this.exceptions.Add(d.ToString()); this.exceptionsWritten.Signal(); };

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one", reportError);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "two", reportError);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "throw three", reportError);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "four", reportError);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "throw five", reportError);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "six", reportError);

                this.writesCompleted.Signal();
                this.exceptionsWritten.Signal();
            }

            [TestMethod]
            public void then_non_failing_data_is_written_asynchronously_in_order()
            {
                this.writesCompleted.Wait();
                this.exceptionsWritten.Wait();

                CollectionAssert.AreEquivalent(new[] { "one", "two", "four", "six" }, this.wrappedListener.requests);
            }

            [TestMethod]
            public void then_exceptions_are_logged()
            {
                this.writesCompleted.Wait();
                this.exceptionsWritten.Wait();

                CollectionAssert.AreEquivalent(new[] { "throw three", "throw five" }, this.exceptions);
            }
        }

        [TestClass]
        public class when_closing : given_wrapper_on_thread_safe_listener
        {
            private CountdownEvent writesRequested;
            private ManualResetEvent closeCompleted;
            private Exception exception;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesRequested = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.writesRequested.Signal();

                this.closeCompleted = new ManualResetEvent(false);
                this.wrappedListener.CloseCompleted += (s, e) => this.closeCompleted.Set();
            }

            protected override void Act()
            {
                this.writesRequested.AddCount(1);

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one", (e, d, s) => this.exception = e);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "two", (e, d, s) => this.exception = e);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "three", (e, d, s) => this.exception = e);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "four", (e, d, s) => this.exception = e);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "five", (e, d, s) => this.exception = e);
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "six", (e, d, s) => this.exception = e);

                this.writesRequested.Signal();

                this.writesRequested.Wait();

                Task.Run(() => { Thread.Sleep(100); this.continueEvent.Release(); });
                this.wrapper.Close();
            }

            [TestMethod]
            public void then_drops_outstanding_requests_and_closes_wrapped_listener()
            {
                this.closeCompleted.WaitOne();

                CollectionAssert.AreEquivalent(new[] { "one", "close" }, this.wrappedListener.requests);
            }
        }
    }

    public class given_wrapper_on_non_owned_thread_unsafe_listener : ArrangeActAssert
    {
        protected WrappedTraceListener wrappedListener;
        protected AsynchronousTraceListenerWrapper wrapper;
        protected SemaphoreSlim continueEvent;

        protected override void Arrange()
        {
            this.continueEvent = new SemaphoreSlim(0, 1);
            this.wrappedListener = new WrappedTraceListener(false, this.continueEvent);
            this.wrapper = new AsynchronousTraceListenerWrapper(this.wrappedListener, ownsWrappedTraceListener: false, bufferSize: 10);
        }

        protected override void Teardown()
        {
            this.wrapper.Close();
        }

        [TestClass]
        public class when_writing_data : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private object entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new object();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry);
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();
                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.AreSame(this.entry, entry);
            }
        }

        [TestClass]
        public class when_writing_log_entry : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private LogEntry entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new LogEntry();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry);
            }

            [TestMethod]
            public void then_data_is_written_asynchronously_with_captured_context_information()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();
                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.AreSame(this.entry, entry);
            }
        }

        [TestClass]
        public class when_writing_multiple_entries : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private CountdownEvent writesCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesCompleted = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.continueEvent.Release();
                this.wrappedListener.OperationCompleted += (s, e) => this.writesCompleted.Signal();
            }

            protected override void Act()
            {
                this.writesCompleted.AddCount(6);

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one");
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "two");
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "three");
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "four");
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "five");
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "six");

                this.writesCompleted.Signal();
            }

            [TestMethod]
            public void then_data_is_written_asynchronously_in_order()
            {
                this.writesCompleted.Wait();

                CollectionAssert.AreEqual(new[] { "one", "two", "three", "four", "five", "six" }, this.wrappedListener.requests);
            }
        }

        [TestClass]
        public class when_flushing_then_enqueues_flush_request : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private CountdownEvent writesCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesCompleted = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.continueEvent.Release();
                this.wrappedListener.OperationCompleted += (s, e) => this.writesCompleted.Signal();
            }

            protected override void Act()
            {
                this.writesCompleted.AddCount(4);

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one");
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "two");
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "three");
                this.wrapper.Flush();

                this.writesCompleted.Signal();
            }

            [TestMethod]
            public void then_data_is_written_asynchronously_in_order()
            {
                this.writesCompleted.Wait();

                CollectionAssert.AreEqual(new[] { "one", "two", "three", "flush" }, this.wrappedListener.requests);
            }
        }

        [TestClass]
        public class when_multiple_threads_issue_simultaneous_requests : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private CountdownEvent writesRequested;
            private CountdownEvent writesCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.writesRequested = new CountdownEvent(1);
                this.writesCompleted = new CountdownEvent(1);
                this.wrappedListener.OperationReceived += (s, e) => this.writesRequested.Signal();
                this.wrappedListener.OperationCompleted += (s, e) => this.writesCompleted.Signal();
            }

            protected override void Act()
            {
                this.writesRequested.AddCount(1);
                this.writesCompleted.AddCount(1);

                Task.Run(() => this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "one"));

                this.writesRequested.Signal();
                this.writesCompleted.Signal();

                this.writesRequested.Wait();
            }

            [TestMethod]
            public void then_wrapped_listener_is_locked()
            {
                try
                {
                    Assert.IsFalse(Monitor.TryEnter(this.wrappedListener));
                }
                finally
                {
                    this.continueEvent.Release();
                    this.writesCompleted.Wait();

                    if (Monitor.IsEntered(this.wrappedListener)) Monitor.Exit(this.wrappedListener);
                }

                CollectionAssert.AreEquivalent(new[] { "one" }, this.wrappedListener.requests);
            }
        }

        [TestClass]
        public class when_tracing_throws : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private AutoResetEvent writeCompleted;
            private AutoResetEvent exceptionWritten;

            protected override void Arrange()
            {
                base.Arrange();

                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();

                this.exceptionWritten = new AutoResetEvent(false);
            }

            protected override void Act()
            {
                this.wrapper.TraceData(
                    new TraceEventCache(),
                    "test source",
                    TraceEventType.Error,
                    100,
                    "throw",
                    (e, d, s) => this.exceptionWritten.Set());
            }

            [TestMethod]
            public void then_message_is_logged()
            {
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.exceptionWritten.WaitOne(TimeSpan.FromSeconds(0.1)));
            }
        }

        [TestClass]
        public class when_buffer_capacity_is_exhausted : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private AutoResetEvent operationReceived;
            private AutoResetEvent writeCompleted;
            private AutoResetEvent exceptionWritten;
            private InvalidOperationException exception;

            protected override void Arrange()
            {
                base.Arrange();

                this.operationReceived = new AutoResetEvent(false);
                this.writeCompleted = new AutoResetEvent(false);
                this.exceptionWritten = new AutoResetEvent(false);
                this.wrappedListener.OperationReceived += (s, e) => this.operationReceived.Set();
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Warning, 0, "initial", (e, d, s) => this.exceptionWritten.Set());
                Assert.IsTrue(this.operationReceived.WaitOne(TimeSpan.FromSeconds(0.2)));

                for (int i = 0; i < 10; i++)
                {
                    this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Warning, i, i, (e, d, s) => this.exceptionWritten.Set());
                }
            }

            protected override void Teardown()
            {
                for (int i = 0; i < 11; i++)
                {
                    this.continueEvent.Release();
                    if (!this.writeCompleted.WaitOne(TimeSpan.FromSeconds(0.2)))
                    {
                        break;
                    }
                }

                base.Teardown();
            }

            protected override void Act()
            {
                try
                {
                    this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "invalid", (e, d, s) => this.exceptionWritten.Set());
                    Assert.Fail("Should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_exception_is_thrown()
            {
                Assert.IsNotNull(this.exception);
            }

            [TestMethod]
            public void then_existing_data_is_written_asynchronously()
            {
                for (int i = 0; i < 5; i++)
                {
                    this.continueEvent.Release();
                    Assert.IsTrue(this.writeCompleted.WaitOne(TimeSpan.FromSeconds(0.1)));
                }

                CollectionAssert.AreEqual(new object[] { "initial", 0, 1, 2, 3 }, this.wrappedListener.requests);
            }

            [TestMethod]
            public void then_new_request_after_draining_buffer_succeeds_and_is_written()
            {
                for (int i = 0; i < 5; i++)
                {
                    this.continueEvent.Release();
                    Assert.IsTrue(this.writeCompleted.WaitOne(TimeSpan.FromSeconds(0.1)));
                }

                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "valid", (e, d, s) => this.exceptionWritten.Set());

                for (int i = 0; i < 7; i++)
                {
                    this.continueEvent.Release();
                    Assert.IsTrue(this.writeCompleted.WaitOne(TimeSpan.FromSeconds(0.1)));
                }

                CollectionAssert.AreEqual(new object[] { "initial", 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, "valid" }, this.wrappedListener.requests);
            }

            [TestMethod]
            public void then_no_exception_is_logged()
            {
                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsFalse(this.exceptionWritten.WaitOne(TimeSpan.FromSeconds(0.1)));
            }
        }

        #region standard tl methods

        [TestClass]
        public class when_writing_data_without_reporting : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private object entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new object();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry);
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.AreSame(this.entry, entry);
            }
        }

        [TestClass]
        public class when_writing_data_without_reporting_throws : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private object entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new object();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, "throw");
            }

            [TestMethod]
            public void then_data_is_not_written_asynchronously()
            {
                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));
            }
        }

        [TestClass]
        public class when_writing_data_array_without_reporting : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private object entry;
            private AutoResetEvent writeCompleted;

            protected override void Arrange()
            {
                base.Arrange();

                this.entry = new object();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 100, this.entry, 2, 3);
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.IsTrue(((ICollection<object>)entry).Contains(this.entry));
            }
        }

        [TestClass]
        public class when_writing_transfer_without_reporting : given_wrapper_on_non_owned_thread_unsafe_listener
        {
            private string message;
            private AutoResetEvent writeCompleted;
            private Guid relatedActivityId;

            protected override void Arrange()
            {
                base.Arrange();

                this.message = "some message";
                this.relatedActivityId = Guid.NewGuid();
                this.writeCompleted = new AutoResetEvent(false);
                this.wrappedListener.OperationCompleted += (s, e) => this.writeCompleted.Set();
            }

            protected override void Act()
            {
                this.wrapper.TraceTransfer(new TraceEventCache(), "source", 100, this.message, this.relatedActivityId);
            }

            [TestMethod]
            public void then_data_is_written_asynchronously()
            {
                object entry;

                Assert.IsFalse(this.wrappedListener.requests.TryTake(out entry));

                this.continueEvent.Release();
                this.writeCompleted.WaitOne();

                Assert.IsTrue(this.wrappedListener.requests.TryTake(out entry));
                Assert.IsTrue(entry.ToString().Contains(this.message));
                Assert.IsTrue(entry.ToString().Contains(this.relatedActivityId.ToString()));
            }
        }

        #endregion
    }

    public class given_trace_listener_with_pending_operations : ArrangeActAssert
    {
        protected WrappedTraceListener wrappedListener;
        protected AsynchronousTraceListenerWrapper wrapper;
        protected SemaphoreSlim continueEvent;

        protected override void Arrange()
        {
            this.continueEvent = new SemaphoreSlim(0, 1);
            this.wrappedListener = new WrappedTraceListener(false, this.continueEvent);
            this.wrapper = new AsynchronousTraceListenerWrapper(this.wrappedListener, bufferSize: 10, disposeTimeout: TimeSpan.FromSeconds(10));

            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 1, 1);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 2, 2);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 3, 3);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 4, 4);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 5, 5);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 6, 6);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 7, 7);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 8, 8);
        }

        [TestClass]
        public class when_disposing : given_trace_listener_with_pending_operations
        {
            protected override void Arrange()
            {
                base.Arrange();

                // allow the background task to move forward but not fast enough to deplete the buffer
                this.wrappedListener.OperationCompleted += (s, e) => { Thread.Sleep(TimeSpan.FromSeconds(2.5)); this.continueEvent.Release(); };
                this.continueEvent.Release();
            }

            protected override void Act()
            {
                this.wrapper.Dispose();
            }

            [TestMethod]
            [Timeout(14000)]
            public void then_completes_dispose_with_timeout_with_some_entries_dropped()
            {
                CollectionAssert.AreNotEqual(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, "close" }, this.wrappedListener.requests);
                Assert.AreEqual("close", this.wrappedListener.requests.Last());
            }
        }

        [TestClass]
        public class when_disposing_while_buffer_is_processed_completely : given_trace_listener_with_pending_operations
        {
            protected override void Arrange()
            {
                base.Arrange();

                // allow the background task to move forward but not fast enough to deplete the buffer
                this.wrappedListener.OperationCompleted += (s, e) => { Thread.Sleep(TimeSpan.FromSeconds(0.1)); this.continueEvent.Release(); };
                this.continueEvent.Release();
            }

            protected override void Act()
            {
                this.wrapper.Dispose();
            }

            [TestMethod]
            [Timeout(2000)]
            public void then_completes_dispose_before_timeout_with_all_entries_logged()
            {
                CollectionAssert.AreEqual(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, "close" }, this.wrappedListener.requests);
            }
        }

        [TestClass]
        public class when_disposing_with_filled_buffer : given_trace_listener_with_pending_operations
        {
            private AutoResetEvent bufferFull;

            protected override void Arrange()
            {
                base.Arrange();

                this.bufferFull = new AutoResetEvent(false);

                // fill up queue
                EventHandler<object> fillBuffer = null;
                fillBuffer = (s, e) =>
                    {
                        this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 9, 9);
                        this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 10, 10);
                        this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 11, 11);
                        this.wrappedListener.OperationReceived -= fillBuffer;
                        this.bufferFull.Set();
                    };
                this.wrappedListener.OperationReceived += fillBuffer;

                // allow the background task to move forward but not fast enough to deplete the buffer
                this.wrappedListener.OperationCompleted += (s, e) => { Thread.Sleep(TimeSpan.FromSeconds(0.5)); this.continueEvent.Release(); };
                this.continueEvent.Release();
                this.bufferFull.WaitOne();
            }

            protected override void Teardown()
            {
                base.Teardown();

                this.bufferFull.Dispose();
            }

            protected override void Act()
            {
                this.wrapper.Dispose();
            }

            [TestMethod]
            [Timeout(8000)]
            public void then_completes_dispose_before_timeout_with_all_entries_logged()
            {
                CollectionAssert.AreEqual(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, "close" }, this.wrappedListener.requests);
            }
        }
    }

    public class given_trace_listener_with_pending_operations_and_zero_dispose_timeout : ArrangeActAssert
    {
        protected WrappedTraceListener wrappedListener;
        protected AsynchronousTraceListenerWrapper wrapper;
        protected SemaphoreSlim continueEvent;

        protected override void Arrange()
        {
            this.continueEvent = new SemaphoreSlim(0, 1);
            this.wrappedListener = new WrappedTraceListener(false, this.continueEvent);
            this.wrapper = new AsynchronousTraceListenerWrapper(this.wrappedListener, bufferSize: 10, disposeTimeout: TimeSpan.Zero);

            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 1, 1);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 2, 2);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 3, 3);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 4, 4);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 5, 5);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 6, 6);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 7, 7);
            this.wrapper.TraceData(new TraceEventCache(), "test", TraceEventType.Error, 8, 8);
        }

        [TestClass]
        public class when_disposing : given_trace_listener_with_pending_operations_and_zero_dispose_timeout
        {
            protected override void Arrange()
            {
                base.Arrange();

                // allow the background task to move forward but not fast enough to deplete the buffer
                this.wrappedListener.OperationCompleted += (s, e) => { Thread.Sleep(TimeSpan.FromSeconds(0.5)); this.continueEvent.Release(); };
                this.continueEvent.Release();
            }

            protected override void Act()
            {
                this.wrapper.Dispose();
            }

            [TestMethod]
            [Timeout(1000)]
            public void then_completes_dispose_immediatly_with_entries_dropped()
            {
                CollectionAssert.AreNotEqual(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, "close" }, this.wrappedListener.requests);
                Assert.AreEqual("close", this.wrappedListener.requests.Last());
            }
        }
    }

    public class WrappedTraceListener : TraceListener
    {
        private bool isThreadSafe;
        private SemaphoreSlim continueOperation;

        public BlockingCollection<object> requests;

        public WrappedTraceListener(bool isThreadSafe, SemaphoreSlim continueOperation = null)
        {
            this.isThreadSafe = isThreadSafe;
            this.continueOperation = continueOperation;
            this.requests = new BlockingCollection<object>();
            this.OperationReceived += (s, e) => { };
            this.OperationCompleted += (s, e) => { };
            this.CloseReceived += (s, e) => { };
            this.CloseCompleted += (s, e) => { };
        }

        public event EventHandler<object> OperationReceived;
        public event EventHandler<object> OperationCompleted;

        public event EventHandler CloseReceived;
        public event EventHandler CloseCompleted;

        public override bool IsThreadSafe
        {
            get
            {
                return this.isThreadSafe;
            }
        }

        public override void Close()
        {
            this.CloseReceived(this, EventArgs.Empty);
            this.requests.Add("close");
            this.CloseCompleted(this, EventArgs.Empty);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            this.OperationReceived(this, data);
            if (this.continueOperation != null) this.continueOperation.Wait();
            try
            {
                if (data.ToString().StartsWith("throw", StringComparison.Ordinal))
                {
                    throw new Exception();
                }
                else
                {
                    this.requests.Add(data);
                }
            }
            finally
            {
                this.OperationCompleted(this, data);
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            this.OperationReceived(this, data);
            if (this.continueOperation != null) this.continueOperation.Wait();
            try
            {
                if (data.Any(d => d.ToString().StartsWith("throw", StringComparison.Ordinal)))
                {
                    throw new Exception();
                }
                else
                {
                    this.requests.Add(data);
                }
            }
            finally
            {
                this.OperationCompleted(this, data);
            }
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            this.OperationReceived(this, message);
            if (this.continueOperation != null) this.continueOperation.Wait();
            try
            {
                if (message.ToString().StartsWith("throw", StringComparison.Ordinal))
                {
                    throw new Exception();
                }
                else
                {
                    this.requests.Add(message + relatedActivityId.ToString());
                }
            }
            finally
            {
                this.OperationCompleted(this, message);
            }
        }

        public override void Flush()
        {
            this.OperationReceived(this, "flush");
            if (this.continueOperation != null) this.continueOperation.Wait();
            this.requests.Add("flush");
            this.OperationCompleted(this, "flush");
        }

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }

    public class DisposableTraceListener : TraceListener
    {
        public bool closed;

        public override void Close()
        {
            this.closed = true;
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }
    }
}
