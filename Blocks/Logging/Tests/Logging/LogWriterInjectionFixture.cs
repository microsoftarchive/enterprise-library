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

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{

    [TestClass]
    public class GivenLogWriterInjectedWithLoggingStack
    {
        private MockTraceListener traceListener;
        private LogWriter logWriter;

        [TestInitialize]
        public void Setup()
        {
            this.traceListener = new MockTraceListener("original");
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
                        false));
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
    }
}
