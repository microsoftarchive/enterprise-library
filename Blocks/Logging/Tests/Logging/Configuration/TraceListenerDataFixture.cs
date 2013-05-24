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
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class TraceListenerDataFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void HasDefaultValues()
        {
            TraceListenerData data = new TraceListenerData();

            Assert.AreEqual(TraceOptions.None, data.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.All, data.Filter);
            Assert.AreEqual(false, data.Asynchronous);
            Assert.AreEqual(30000, data.AsynchronousBufferSize);
            Assert.AreEqual(Timeout.InfiniteTimeSpan, data.AsynchronousDisposeTimeout);
            Assert.AreEqual(null, data.AsynchronousMaxDegreeOfParallelism);
        }

        [TestMethod]
        public void CreatesSynchronousTraceListenerByDefault()
        {
            var data = new MockTraceListenerData() { Filter = SourceLevels.Warning, TraceOutputOptions = TraceOptions.ProcessId };

            var listener = data.BuildTraceListener(new LoggingSettings());

            Assert.IsInstanceOfType(listener, typeof(MockTraceListener));
            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
        }

        [TestMethod]
        public void CreatesAynchronousTraceListenerWhenOverridden()
        {
            var data = new MockTraceListenerData() { Asynchronous = true, Filter = SourceLevels.Warning, TraceOutputOptions = TraceOptions.ProcessId };

            var listener = data.BuildTraceListener(new LoggingSettings());

            Assert.IsInstanceOfType(listener, typeof(AsynchronousTraceListenerWrapper));
            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
        }

        [TestMethod]
        public void CreatesAynchronousTraceListenerWithTimeoutWhenOverridden()
        {
            var data = new MockTraceListenerData() { Asynchronous = true, AsynchronousDisposeTimeout = TimeSpan.FromSeconds(10), Filter = SourceLevels.Warning, TraceOutputOptions = TraceOptions.ProcessId };

            var listener = data.BuildTraceListener(new LoggingSettings());

            Assert.IsInstanceOfType(listener, typeof(AsynchronousTraceListenerWrapper));
            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
        }
    }
}
