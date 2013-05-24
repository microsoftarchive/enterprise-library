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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class given_log_writer_with_synchronous_and_asynchronous_listeners
    {
        private LogWriter logWriter;
        private WrappedTraceListener synchronous;
        private WrappedTraceListener asynchronous;
        private WrappedTraceListener errorListener;

        [TestInitialize]
        public void TestInitialize()
        {
            this.synchronous = new WrappedTraceListener();
            this.asynchronous = new WrappedTraceListener();
            this.errorListener = new WrappedTraceListener();

            this.logWriter = new LogWriter(new LoggingConfiguration());
            this.logWriter.Configure(c =>
                {
                    var asynchronous = new AsynchronousTraceListenerWrapper(this.asynchronous);

                    c.AddLogSource("synchronous").AddTraceListener(this.synchronous);
                    c.AddLogSource("asynchronous").AddTraceListener(asynchronous);
                    var autoflush = c.AddLogSource("asynchronous-autoflush");
                    autoflush.AddTraceListener(asynchronous);
                    autoflush.AutoFlush = true;
                    var mixed = c.AddLogSource("mixed");
                    mixed.AddTraceListener(this.synchronous);
                    mixed.AddTraceListener(asynchronous);

                    c.SpecialSources.LoggingErrorsAndWarnings.Listeners.Add(this.errorListener);
                });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.logWriter.Dispose();
        }

        [TestMethod]
        public void when_logging_to_synchronous_only_source_then_logs_synchronously()
        {
            var counter = new CountdownEvent(3);
            this.synchronous.OperationCompleted += (s, e) => counter.Signal();

            this.logWriter.Write("test1", "synchronous");
            this.logWriter.Write("test2", "synchronous");
            this.logWriter.Write("test3", "synchronous");

            Assert.IsTrue(counter.Wait(TimeSpan.FromSeconds(0.5)));

            CollectionAssert.AreEqual(new[] { "test1", "test2", "test3" }, this.synchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());
            Assert.AreEqual(0, this.asynchronous.requests.Count);
        }

        [TestMethod]
        public void when_logging_to_asynchronous_only_source_then_logs_asynchronously()
        {
            var counter = new CountdownEvent(3);
            this.asynchronous.OperationCompleted += (s, e) => counter.Signal();

            this.logWriter.Write("test1", "asynchronous");
            this.logWriter.Write("test2", "asynchronous");
            this.logWriter.Write("test3", "asynchronous");

            Assert.IsTrue(counter.Wait(TimeSpan.FromSeconds(0.5)));

            Assert.AreEqual(0, this.synchronous.requests.Count);
            CollectionAssert.AreEquivalent(new[] { "test1", "test2", "test3" }, this.asynchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());
        }

        [TestMethod]
        public void when_logging_transfer_to_asynchronous_only_source_then_logs_asynchronously()
        {
            var counter = new CountdownEvent(3);
            this.asynchronous.OperationCompleted += (s, e) => counter.Signal();

            this.logWriter.Write(new LogEntry { Message = "test1", Severity = TraceEventType.Transfer, Categories = new[] { "asynchronous" }, RelatedActivityId = Guid.NewGuid() });
            this.logWriter.Write(new LogEntry { Message = "test2", Severity = TraceEventType.Transfer, Categories = new[] { "asynchronous" }, RelatedActivityId = Guid.NewGuid() });
            this.logWriter.Write(new LogEntry { Message = "test3", Severity = TraceEventType.Transfer, Categories = new[] { "asynchronous" }, RelatedActivityId = Guid.NewGuid() });

            Assert.IsTrue(counter.Wait(TimeSpan.FromSeconds(0.5)));

            Assert.AreEqual(0, this.synchronous.requests.Count);
            CollectionAssert.AreEquivalent(new[] { "transfer-test1", "transfer-test2", "transfer-test3" }, this.asynchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());
        }

        [TestMethod]
        public void when_logging_to_mixed_source_then_logs_both_synchronously_and_asynchronously()
        {
            var counter = new CountdownEvent(6);
            this.synchronous.OperationCompleted += (s, e) => counter.Signal();
            this.asynchronous.OperationCompleted += (s, e) => counter.Signal();

            this.logWriter.Write("test1", "mixed");
            this.logWriter.Write("test2", "mixed");
            this.logWriter.Write("test3", "mixed");

            Assert.IsTrue(counter.Wait(TimeSpan.FromSeconds(0.5)));

            CollectionAssert.AreEqual(new[] { "test1", "test2", "test3" }, this.synchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());
            CollectionAssert.AreEqual(new[] { "test1", "test2", "test3" }, this.asynchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());
        }

        [TestMethod]
        public void when_logging_to_asynchronous_only_source_with_autoflush_then_logs_asynchronously()
        {
            var counter = new CountdownEvent(6);
            this.asynchronous.OperationCompleted += (s, e) => counter.Signal();

            this.logWriter.Write("test1", "asynchronous-autoflush");
            this.logWriter.Write("test2", "asynchronous-autoflush");
            this.logWriter.Write("test3", "asynchronous-autoflush");

            Assert.IsTrue(counter.Wait(TimeSpan.FromSeconds(0.5)));

            Assert.AreEqual(0, this.synchronous.requests.Count);
            CollectionAssert.AreEquivalent(new[] { "test1", "flush", "test2", "flush", "test3", "flush" }, this.asynchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());
        }

        [TestMethod]
        public void when_logging_to_synchronous_only_source_throws_then_logs_error()
        {
            var counter = new CountdownEvent(3);
            this.synchronous.OperationCompleted += (s, e) => counter.Signal();

            this.logWriter.Write("test1", "synchronous");
            this.logWriter.Write("throwtest2", "synchronous");
            this.logWriter.Write("test3", "synchronous");

            Assert.IsTrue(counter.Wait(TimeSpan.FromSeconds(0.5)));

            CollectionAssert.AreEqual(new[] { "test1", "test3" }, this.synchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());
            Assert.AreEqual(0, this.asynchronous.requests.Count);

            object entry;
            Assert.IsTrue(this.errorListener.requests.TryTake(out entry, TimeSpan.FromSeconds(0.5)));
            Assert.IsTrue(((LogEntry)entry).Message.Contains("throwtest2"));
        }

        [TestMethod]
        public void when_logging_to_asynchronous_only_source_throws_then_logs_error()
        {
            var counter = new CountdownEvent(3);
            this.asynchronous.OperationCompleted += (s, e) => counter.Signal();

            this.logWriter.Write("test1", "asynchronous");
            this.logWriter.Write("throwtest2", "asynchronous");
            this.logWriter.Write("test3", "asynchronous");

            Assert.IsTrue(counter.Wait(TimeSpan.FromSeconds(0.5)));

            Assert.AreEqual(0, this.synchronous.requests.Count);
            CollectionAssert.AreEqual(new[] { "test1", "test3" }, this.asynchronous.requests.Select(r => ((LogEntry)r).Message).ToArray());

            object entry;
            Assert.IsTrue(this.errorListener.requests.TryTake(out entry, TimeSpan.FromSeconds(0.5)));
            Assert.IsTrue(((LogEntry)entry).Message.Contains("throwtest2"));
        }

        public class WrappedTraceListener : TraceListener
        {
            public BlockingCollection<object> requests;

            public WrappedTraceListener()
            {
                this.requests = new BlockingCollection<object>();
                this.OperationCompleted += (s, e) => { };
            }

            public event EventHandler<object> OperationCompleted;

            public override bool IsThreadSafe
            {
                get
                {
                    return true;
                }
            }

            public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
            {
                try
                {
                    if (data is LogEntry && ((LogEntry)data).Message.StartsWith("throw", StringComparison.Ordinal))
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
                try
                {
                    if (message.StartsWith("throw", StringComparison.Ordinal))
                    {
                        throw new Exception();
                    }
                    else
                    {
                        this.requests.Add(new LogEntry { Message = "transfer-" + message });
                    }
                }
                finally
                {
                    this.OperationCompleted(this, message);
                }
            }

            public override void Flush()
            {
                try
                {

                    this.requests.Add(new LogEntry { Message = "flush" });
                }
                finally
                {
                    this.OperationCompleted(this, "flush");
                }
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
    }
}
