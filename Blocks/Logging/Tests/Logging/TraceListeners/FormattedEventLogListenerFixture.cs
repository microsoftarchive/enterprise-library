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
using System.IO;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    /// <summary>
    /// Summary description for FormattingListenerFixture
    /// </summary>
    [TestClass]
    public class FormattedEventLogListenerFixture
    {
        [TestInitialize]
        [TestCleanup]
        public void SetUpCleanup()
        {
            try
            {
                if (EventLog.SourceExists(CommonUtil.EventLogSourceName))
                {
                    EventLog.DeleteEventSource(CommonUtil.EventLogSourceName);
                }
            }
            catch (SecurityException ex)
            {
                Assert.Inconclusive("In order to run the tests, please run Visual Studio as Administrator.\r\n{0}", ex.ToString());
            }
        }

        [TestMethod]
        public void ListenerWillUseFormatterIfExists()
        {
            StringWriter writer = new StringWriter();
            FormattedEventLogTraceListener listener =
                new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName, CommonUtil.EventLogNameCustom, new TextFormatter("DUMMY{newline}DUMMY"));

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", new[] { listener }, SourceLevels.All);

            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            source.TraceData(TraceEventType.Error, 1, logEntry);

            Assert.AreEqual("DUMMY" + Environment.NewLine + "DUMMY", CommonUtil.GetLastEventLogEntryCustom());
        }

        [TestMethod]
        public void ListenerWillFallbackToTraceEntryToStringIfFormatterDoesNotExists()
        {
            LogEntry testEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            StringWriter writer = new StringWriter();
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName, CommonUtil.EventLogNameCustom, null);

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", new[] { listener }, SourceLevels.All);
            source.TraceData(TraceEventType.Error, 1, testEntry);

            Assert.AreEqual(testEntry.ToString(), CommonUtil.GetLastEventLogEntryCustom());
        }

        [TestMethod]
        public void CanCreateListenerWithSourceAndFormatter()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", new TextFormatter("TEST"));

            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.InnerListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.InnerListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.InnerListener).EventLog.Source);
            Assert.AreEqual("", ((EventLogTraceListener)listener.InnerListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.InnerListener).EventLog.MachineName);
        }

        [TestMethod]
        public void CanCreateListenerWithSourceLogAndFormatter()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "log", new TextFormatter("TEST"));

            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.InnerListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.InnerListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.InnerListener).EventLog.Source);
            Assert.AreEqual("log", ((EventLogTraceListener)listener.InnerListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.InnerListener).EventLog.MachineName);
        }

        [TestMethod]
        public void CanCreateListenerWithSourceFormatterAndDefaultLogMachineName()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "", ".", new TextFormatter("TEST"));

            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.InnerListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.InnerListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.InnerListener).EventLog.Source);
            Assert.AreEqual("", ((EventLogTraceListener)listener.InnerListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.InnerListener).EventLog.MachineName);
        }

        [TestMethod]
        public void CanCreateListenerWithSourceFormatterLogAndMachineName()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "log", "machine", new TextFormatter("TEST"));

            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.InnerListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.InnerListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.InnerListener).EventLog.Source);
            Assert.AreEqual("log", ((EventLogTraceListener)listener.InnerListener).EventLog.Log);
            Assert.AreEqual("machine", ((EventLogTraceListener)listener.InnerListener).EventLog.MachineName);
        }

        [TestMethod]
        public void CanCreateListenerWithSourceFormatterLogAndEmptyMachineName()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "log", "", new TextFormatter("TEST"));

            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.InnerListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.InnerListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.InnerListener).EventLog.Source);
            Assert.AreEqual("log", ((EventLogTraceListener)listener.InnerListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.InnerListener).EventLog.MachineName);
        }

        [TestMethod]
        public void CanWriteToEventLog()
        {
            FormattedEventLogTraceListener listener =
                new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName,
                                                   CommonUtil.EventLogNameCustom,
                                                   FormattedEventLogTraceListener.DefaultMachineName,
                                                   new TextFormatter("{message}"));
            LogSource source = new LogSource("transient", new[] { listener }, SourceLevels.All);

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Severity = TraceEventType.Error;

            CommonUtil.ResetEventLogCounterCustom();
            source.TraceData(entry.Severity, entry.EventId, entry);
            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                Assert.AreEqual(1, CommonUtil.EventLogEntryCountCustom());
                Assert.AreEqual(CommonUtil.MsgBody, customLog.Entries[customLog.Entries.Count - 1].Message);
            }
        }

        [TestMethod]
        public void WillNotWriteToTheEventLogIfRequestIsFilteredOut()
        {
            FormattedEventLogTraceListener listener =
                new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName,
                                                   CommonUtil.EventLogNameCustom,
                                                   FormattedEventLogTraceListener.DefaultMachineName,
                                                   new TextFormatter("{message}"));
            listener.Filter = new EventTypeFilter(SourceLevels.Critical);
            LogSource source = new LogSource("transient", new[] { listener }, SourceLevels.All);

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Severity = TraceEventType.Error;

            CommonUtil.ResetEventLogCounterCustom();
            source.TraceData(entry.Severity, entry.EventId, entry);
            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                Assert.AreEqual(0, CommonUtil.EventLogEntryCountCustom());
            }
        }
    }
}
