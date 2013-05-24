#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Schema;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.Tracing;
using System.IO;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestObjects
{
    public class InMemoryEventListener : EventListener, IObserver<EventEntry>
    {
        private readonly EventSourceSchemaCache schemaCache = EventSourceSchemaCache.Instance;
        private MemoryStream memory;
        private StreamWriter writer;
        private long writenBytes;
        private ManualResetEventSlim waitOnAsync;
        private readonly object lockObject = new object();

        public InMemoryEventListener()
            : this(null)
        {
        }

        public InMemoryEventListener(IEventTextFormatter formatter = null)
        {
            this.Formatter = formatter ?? new EventTextFormatter(verbosityThreshold: EventLevel.LogAlways);
            this.memory = new MemoryStream();
            this.writer = new StreamWriter(this.memory) { AutoFlush = false  };
            this.waitOnAsync = new ManualResetEventSlim();
        }

        public IEventTextFormatter Formatter { get; set; }
        public int EventWrittenCount { get; private set; }
        public Func<bool> WaitSignalCondition { get; set; }

        public override string ToString()
        {
            this.memory.Position = 0;
            return new StreamReader(this.memory).ReadToEnd();
        }

        public WaitHandle WaitOnAsyncEvents { get { return this.waitOnAsync.WaitHandle; } }

        public override void Dispose()
        {
            base.Dispose();
            this.writer.Dispose();
            this.waitOnAsync.Dispose();
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            var entry = EventEntry.Create(eventData, this.schemaCache.GetSchema(eventData.EventId, eventData.EventSource));

            OnNext(entry);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(EventEntry value)
        {
            lock (this.lockObject)
            {
                try
                {
                    this.writer.Write(this.Formatter.WriteEvent(value));
                    this.EventWrittenCount++;
                    // check that no data was already flushed
                    Assert.AreEqual(writenBytes, this.memory.Length);
                    this.writer.Flush();
                    writenBytes = this.memory.Length;
                }
                finally
                {
                    // mark any async event as done       
                    if (WaitSignalCondition == null ||
                        WaitSignalCondition())
                    {
                        this.waitOnAsync.Set();
                    }
                }
            }
        }
    }
}
