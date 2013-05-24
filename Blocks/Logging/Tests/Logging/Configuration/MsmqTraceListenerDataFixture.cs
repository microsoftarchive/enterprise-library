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
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class GivenMsmqTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new MsmqTraceListenerData(
                    "listener",
                    "queue path",
                    "formatter",
                    MessagePriority.High,
                    true,
                    TimeSpan.MinValue,
                    TimeSpan.MaxValue,
                    false,
                    true,
                    false,
                    MessageQueueTransactionType.Automatic)
                    {
                        TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                        Filter = SourceLevels.Warning
                    };
        }

        [TestMethod]
        public void WhenCreatingInstanceUsingDefaultContructor_ThenListenerDataTypeIsSet()
        {
            var listener = new MsmqTraceListenerData();
            Assert.AreEqual(typeof(MsmqTraceListenerData), listener.ListenerDataType);
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesMsmqListener()
        {
            var settings = new LoggingSettings { Formatters = { new BinaryLogFormatterData { Name = "formatter" } } };

            var listener = (MsmqTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(TraceOptions.DateTime | TraceOptions.Callstack, listener.TraceOutputOptions);
            Assert.IsNotNull(listener.Filter);
            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
            Assert.IsInstanceOfType(listener.Formatter, typeof(BinaryLogFormatter));
            Assert.AreEqual("queue path", listener.QueuePath);
        }
    }
}
