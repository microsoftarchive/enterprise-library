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

using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class GivenFlatFileTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new FlatFileTraceListenerData("listener", "filename", "header", "footer", "formatter")
                    {
                        TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                        Filter = SourceLevels.Warning
                    };
        }

        [TestMethod]
        public void WhenCreatingInstanceUsingDefaultContructor_ThenListenerDataTypeIsSet()
        {
            var listener = new FlatFileTraceListenerData();
            Assert.AreEqual(typeof(FlatFileTraceListenerData), listener.ListenerDataType);
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesFileListener()
        {
            var settings = new LoggingSettings { Formatters = { new TextFormatterData { Name = "formatter", Template = "template" } } };

            var listener = (FlatFileTraceListener)listenerData.BuildTraceListener(settings);

            try
            {
                Assert.IsNotNull(listener);
                Assert.AreEqual("listener", listener.Name);
                Assert.AreEqual(TraceOptions.DateTime | TraceOptions.Callstack, listener.TraceOutputOptions);
                Assert.IsNotNull(listener.Filter);
                Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
                Assert.IsInstanceOfType(listener.Formatter, typeof(TextFormatter));
                Assert.AreEqual("filename", Path.GetFileName(((FileStream)((StreamWriter)listener.Writer).BaseStream).Name));
            }
            finally
            {
                listener.Dispose();
            }
        }
    }

    [TestClass]
    public class GivenFlatFileTraceListenerDataWithFilterDataAndNullFormatterName
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new FlatFileTraceListenerData("listener", "filename", "header", "footer", null)
                    {
                        TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                        Filter = SourceLevels.Warning
                    };
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesFileListenerWithoutFormatter()
        {
            var settings = new LoggingSettings { Formatters = { new TextFormatterData { Name = "formatter", Template = "template" } } };

            var listener = (FlatFileTraceListener)listenerData.BuildTraceListener(settings);
            try
            {
                Assert.IsNotNull(listener);
                Assert.AreEqual("listener", listener.Name);
                Assert.AreEqual(TraceOptions.DateTime | TraceOptions.Callstack, listener.TraceOutputOptions);
                Assert.IsNotNull(listener.Filter);
                Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
                Assert.IsNull(listener.Formatter);
                Assert.AreEqual("filename", Path.GetFileName(((FileStream)((StreamWriter)listener.Writer).BaseStream).Name));
            }
            finally
            {
                listener.Dispose();
            }
        }
    }
}
