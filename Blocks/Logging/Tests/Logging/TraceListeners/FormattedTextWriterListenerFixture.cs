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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    /// <summary>
    /// Summary description for FormattingListenerFixture
    /// </summary>
    [TestClass]
    public class FormattedTextWriterTraceListenerFixture
    {
        [TestMethod]
        public void ShouldFilterLog()
        {
            using (StringWriter writer = new StringWriter())
            {
                FormattedTextWriterTraceListener listener = new FormattedTextWriterTraceListener(writer, new TextFormatter("DUMMY{newline}DUMMY"));

                listener.Filter = new EventTypeFilter(SourceLevels.Off);

                LogSource source = new LogSource("notfromconfig", new[] { listener }, SourceLevels.All);

                source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));

                Assert.AreEqual(0, writer.ToString().Length);
            }
        }

        [TestMethod]
        public void ShouldLogApplyingLog()
        {
            using (StringWriter writer = new StringWriter())
            {
                FormattedTextWriterTraceListener listener = new FormattedTextWriterTraceListener(writer, new TextFormatter("DUMMY{newline}DUMMY"));

                listener.Filter = new EventTypeFilter(SourceLevels.Error);

                LogSource source = new LogSource("notfromconfig", new[] { listener }, SourceLevels.All);

                source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Critical, "title", null));

                Assert.AreNotEqual(0, writer.ToString().Length);
            }
        }

        [TestMethod]
        public void FormattedListenerWillUseFormatterIfExists()
        {
            using (StringWriter writer = new StringWriter())
            {
                FormattedTextWriterTraceListener listener = new FormattedTextWriterTraceListener(writer, new TextFormatter("DUMMY{newline}DUMMY"));

                // need to go through the source to get a TraceEventCache
                LogSource source = new LogSource("notfromconfig", new[] { listener }, SourceLevels.All);
                source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));

                Assert.AreEqual("DUMMY" + Environment.NewLine + "DUMMY", writer.ToString());
            }
        }

        [TestMethod]
        public void FormattedListenerWillFallbackToTraceEntryToStringIfFormatterDoesNotExists()
        {
            LogEntry testEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            using (StringWriter writer = new StringWriter())
            {
                FormattedTextWriterTraceListener listener = new FormattedTextWriterTraceListener(writer);

                // need to go through the source to get a TraceEventCache
                LogSource source = new LogSource("notfromconfig", new[] { listener }, SourceLevels.All);
                source.TraceData(TraceEventType.Error, 0, testEntry);

                string writtenData = writer.ToString();
                string testEntryToString = testEntry.ToString();

                Assert.IsTrue(-1 != writtenData.IndexOf(testEntryToString));
            }
        }

        [TestMethod]
        public void FormattedListenerWithNameAndFileNameWillUseFormatterIfExists()
        {
            string fileName = Path.GetTempFileName();

            try
            {
                using (FormattedTextWriterTraceListener listener
                    = new FormattedTextWriterTraceListener(fileName, "name", new TextFormatter("DUMMY{newline}DUMMY")))
                {
                    TraceEventCache eventCache = new TraceEventCache();
                    listener.TraceData(
                        eventCache,
                        "cat1",
                        TraceEventType.Error,
                        0,
                        new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
                }

                Assert.AreEqual("DUMMY" + Environment.NewLine + "DUMMY", File.ReadAllText(fileName));
            }
            finally
            {
                File.Delete(fileName);
            }
        }
    }
}
