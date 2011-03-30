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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class TracerFixture
    {
        readonly Guid referenceGuid = new Guid("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        readonly Guid overwriteGuid = new Guid("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
        readonly Guid testActivityId1 = new Guid("1CF75C3C-127F-41c9-97B2-FBEDB64F974A");
        readonly Guid testActivityId2 = new Guid("5D352811-28DF-4baf-BD18-5997FE1260A3");
        const string operation = "operation";
        const string nestedOperation = "nested operation";
        const string badOperation = "bad operation";
        const string category = "category";

        private LogWriterImpl logWriter;

        [TestInitialize]
        public void Setup()
        {
            var sharedMockListener = new MockTraceListener();

            this.logWriter =
                new LogWriterImpl(
                    new ILogFilter[] 
                    { 
                        new CategoryFilter("category", new[] { "bad operation" }, CategoryFilterMode.AllowAllExceptDenied)
                    },
                    new LogSource[]
                    {
                        new LogSource("MockCategoryOne", new TraceListener[] { sharedMockListener }, SourceLevels.All),
                        new LogSource("operation", new TraceListener[] { sharedMockListener }, SourceLevels.All),
                        new LogSource("nested operation",new TraceListener[] { sharedMockListener },  SourceLevels.All),
                    },
                    new LogSource(""),
                    new LogSource(""),
                    new LogSource(""),
                    "MockCategoryOne",
                    true,
                    false);
        }

        [TestCleanup]
        public void Teardown()
        {
            this.logWriter.Dispose();
        }

        [TestMethod] //misterious build fail on commandline runner
        public void UsingTracerWritesEntryAndExitMessages()
        {
            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;

            using (new Tracer(operation, this.logWriter))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.startTitle, operation, currentActivityId, true);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.endTitle, operation, currentActivityId, false);
        }

        [TestMethod] //misterious build fail on commandline runner
        public void UsingTracerWritesEntryAndExitMessagesIgnoring_NoActivityIdCheck()
        {
            MockTraceListener.Reset();

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void TraceListenerWritesFullTypeNameAndMethodName()
        {
            string MyTypeAndMethodName = "Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TracerFixture.TraceListenerWritesFullTypeNameAndMethodName";

            MockTraceListener.Reset();

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.IsTrue(MockTraceListener.Entries[0].Message.Contains(MyTypeAndMethodName));
        }

        [TestMethod]
        public void UsingTracerDoesNotWriteWhenDisabled()
        {
            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;

            this.logWriter.SetTracingEnabled(false);

            using (new Tracer(operation, this.logWriter))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void TracerUpdatesStackAfterEntryAndExit()
        {
            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count);
                Assert.AreEqual(operation, Trace.CorrelationManager.LogicalOperationStack.Peek());
            }

            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);
        }

        [TestMethod]
        public void NestedTracerUpdatesStackAfterEntryAndExit()
        {
            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count);
                Assert.AreEqual(operation, Trace.CorrelationManager.LogicalOperationStack.Peek());

                using (new Tracer(nestedOperation, this.logWriter))
                {
                    Assert.AreEqual(2, Trace.CorrelationManager.LogicalOperationStack.Count);
                    Assert.AreEqual(nestedOperation, Trace.CorrelationManager.LogicalOperationStack.Peek());
                }

                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count);
                Assert.AreEqual(operation, Trace.CorrelationManager.LogicalOperationStack.Peek());
            }

            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);
        }

        [TestMethod]
        public void NewTracerWithoutActivityWillCreateNewActivityIdIfThereIsNoExistingActivityId()
        {
            Trace.CorrelationManager.ActivityId = Guid.Empty;

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);
            using (new Tracer(operation, this.logWriter))
            {
                Assert.IsFalse(Guid.Empty == Trace.CorrelationManager.ActivityId);
            }
            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        public void NewTracerWithoutActivityWillKeepExistingActivityId()
        {
            Trace.CorrelationManager.ActivityId = referenceGuid;

            Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
            }

            Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        public void NewTracerWithActivityWillOverwriteExistingActivityId()
        {
            Trace.CorrelationManager.ActivityId = referenceGuid;

            Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
            using (new Tracer(operation, overwriteGuid, this.logWriter))
            {
                Assert.AreEqual(overwriteGuid, Trace.CorrelationManager.ActivityId);
            }
            Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        public void NestedTracerInheritsActivityId()
        {
            Trace.CorrelationManager.ActivityId = Guid.Empty;

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreNotEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);
                Guid outerActivityId = Trace.CorrelationManager.ActivityId;

                using (new Tracer(nestedOperation, this.logWriter))
                {
                    Assert.AreEqual(outerActivityId, Trace.CorrelationManager.ActivityId);
                }

                Assert.AreEqual(outerActivityId, Trace.CorrelationManager.ActivityId);
            }

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        public void NestedTracerOverwritesActivityId()
        {
            Trace.CorrelationManager.ActivityId = Guid.Empty;

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreNotEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);
                Guid outerActivityId = Trace.CorrelationManager.ActivityId;

                using (new Tracer(nestedOperation, referenceGuid, this.logWriter))
                {
                    Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
                }

                Assert.AreEqual(outerActivityId, Trace.CorrelationManager.ActivityId);
            }

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        public void UseTracingWithOwnLogWriter()
        {
            MockTraceListener.Reset();

            LogSource source = new LogSource("tracesource", SourceLevels.All);
            source.Listeners.Add(new MockTraceListener());

            List<LogSource> traceSources = new List<LogSource>(new LogSource[] { source });
            LogWriter logWriter = new LogWriterImpl(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", true, false);

            using (Tracer tracer = new Tracer("testoperation", logWriter))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(2, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void UseTracingWithDisabledLoggerDoesntWrite()
        {
            MockTraceListener.Reset();

            LogSource source = new LogSource("tracesource", SourceLevels.All);
            source.Listeners.Add(new MockTraceListener());

            List<LogSource> traceSources = new List<LogSource>(new LogSource[] { source });
            LogWriter logWriter = new LogWriterImpl(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", false, false);

            using (Tracer tracer = new Tracer("testoperation", logWriter))
            {
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingTracerWithNullIServiceLocatorForInstrumentationThrows()
        {
            LogSource source = new LogSource("tracesource", SourceLevels.All);
            LogWriter logWriter = new LogWriterImpl(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", false, false);
            new Tracer("testoperation", logWriter, (IServiceLocator)null).Dispose();
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingTracerWithActivityIDAndNullIServiceLocatorForInstrumentationThrows()
        {
            LogSource source = new LogSource("tracesource", SourceLevels.All);
            LogWriter logWriter = new LogWriterImpl(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", false, false);
            new Tracer("testoperation", Guid.NewGuid(), logWriter, (IServiceLocator)null).Dispose();
        }

        [TestMethod]
        public void LoggedMessagesDuringTracerAddsCategoryIds()
        {
            MockTraceListener.Reset();

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                MockTraceListener.Reset();

                this.logWriter.Write("message", category);
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(2, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
                MockTraceListener.Reset();
            }
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
        }

        [TestMethod]
        public void LoggedMessagesDuringNestedTracerAddsAllCategoryIds()
        {
            MockTraceListener.Reset();

            using (new Tracer(operation, this.logWriter))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                MockTraceListener.Reset();

                using (new Tracer(nestedOperation, this.logWriter))
                {
                    Assert.AreEqual(1, MockTraceListener.Entries.Count);
                    Assert.AreEqual(2, MockTraceListener.LastEntry.Categories.Count);
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(nestedOperation));
                    MockTraceListener.Reset();

                    this.logWriter.Write("message", category);
                    Assert.AreEqual(1, MockTraceListener.Entries.Count);
                    Assert.AreEqual(3, MockTraceListener.LastEntry.Categories.Count);
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(nestedOperation));
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
                    MockTraceListener.Reset();
                }
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(2, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(nestedOperation));
                MockTraceListener.Reset();
            }
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
        }

        [TestMethod]
        public void TraceCategoriesAreConsideredInFilters()
        {
            MockTraceListener.Reset();

            using (new Tracer(badOperation, this.logWriter))
            {
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void TracingCanBeTurnedOnAndOff()
        {
            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;
            this.logWriter.SetTracingEnabled(false);

            using (new Tracer(operation, this.logWriter))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);


            MockTraceListener.Reset();
            currentActivityId = Guid.Empty;
            this.logWriter.SetTracingEnabled(true);

            using (new Tracer(operation, this.logWriter))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.startTitle, operation, currentActivityId, true);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.endTitle, operation, currentActivityId, false);
            MockTraceListener.Reset();
        }

        [TestMethod]
        public void ActivityIdsAreUniqueOnEachThread()
        {
            MockTraceListener.Reset();

            // This will put two events into the sink using nestedOperation and testActivityId2
            using (new Tracer(nestedOperation, testActivityId2, this.logWriter))
            {
                // This will put two events into the sink using operation and testActivityId1
                CrossThreadTestRunner t = new CrossThreadTestRunner(new ThreadStart(this.DoOtherThreadWork));
                t.Run();

                // Confirm that the Tracer on this thread has the expected activityID
                Assert.AreEqual(testActivityId2, Trace.CorrelationManager.ActivityId);
            }

            //Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);

            Assert.AreEqual(4, MockTraceListener.Entries.Count);
            AssertLogEntryIsValid(MockTraceListener.Entries[0], Tracer.startTitle, nestedOperation, testActivityId2, true);
            AssertLogEntryIsValid(MockTraceListener.Entries[1], Tracer.startTitle, operation, testActivityId1, true);
            AssertLogEntryIsValid(MockTraceListener.Entries[2], Tracer.endTitle, operation, testActivityId1, false);
            AssertLogEntryIsValid(MockTraceListener.Entries[3], Tracer.endTitle, nestedOperation, testActivityId2, false);
        }

        [TestMethod]
        public void ThreadsDoNotShareLogicalOperationsStack()
        {
            CrossThreadTestRunner t = new CrossThreadTestRunner(new ThreadStart(DoOtherThreadWork));
            t.Run();

            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);
            Assert.IsFalse(testActivityId1 == Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullWriterToTracerThrows()
        {
            using (new Tracer(operation, null))
            {
                int i = new Random().Next();
            }
        }

        [TestMethod]
        public void NonStringElementsInLogicalOperationStackAreIgnored()
        {
            this.logWriter.SetTracingEnabled(true);

            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;
            object nonStringLogicalOperation = new object();
            object stringLogicalOperation = "another operation";
            object nullLogicalOperation = null;

            try
            {
                Trace.CorrelationManager.LogicalOperationStack.Push(nonStringLogicalOperation);
                Trace.CorrelationManager.LogicalOperationStack.Push(stringLogicalOperation);
                Trace.CorrelationManager.LogicalOperationStack.Push(nullLogicalOperation);

                using (new Tracer(operation, this.logWriter))
                {
                    currentActivityId = Trace.CorrelationManager.ActivityId;
                    Assert.AreEqual(1, MockTraceListener.Entries.Count);
                }
            }
            finally
            {
                Trace.CorrelationManager.LogicalOperationStack.Pop();
                Trace.CorrelationManager.LogicalOperationStack.Pop();
                Trace.CorrelationManager.LogicalOperationStack.Pop();
            }

            Assert.AreEqual(2, MockTraceListener.Entries.Count);

            LogEntry logEntry = MockTraceListener.LastEntry;
            Assert.AreEqual(2, logEntry.Categories.Count);
            Assert.IsTrue(logEntry.Categories.Contains(operation));
            Assert.IsTrue(logEntry.Categories.Contains((string)stringLogicalOperation));
        }

        void DoOtherThreadWork()
        {
            using (Tracer tracer = new Tracer(operation, testActivityId1, this.logWriter))
            {
                Assert.IsNotNull(tracer);
                Assert.AreEqual(testActivityId1, Trace.CorrelationManager.ActivityId);
            }
        }

        void AssertLogEntryIsValid(LogEntry entry,
                                   string expectedTitle,
                                   string expectedCategory,
                                   Guid expectedActivityId,
                                   bool isStartMessage)
        {
            Assert.AreEqual(expectedTitle, entry.Title);

            Assert.AreEqual(expectedActivityId, entry.ActivityId);

            Assert.AreEqual(Tracer.eventId, entry.EventId);
            Assert.AreEqual(Tracer.priority, entry.Priority);
            Assert.AreEqual(Tracer.eventId, entry.EventId);
            Assert.IsTrue(entry.Categories.Contains(expectedCategory));

            if (isStartMessage)
            {
                Assert.AreEqual(TraceEventType.Start, entry.Severity);
                AssertMessageIsValidStartMessage(entry.Message);
            }
            else
            {
                Assert.AreEqual(TraceEventType.Stop, entry.Severity);
                AssertMessageIsValidEndMessage(entry.Message);
            }
        }

        void AssertMessageIsValidStartMessage(string message)
        {
            Assert.IsNotNull(message);

            string format = Resources.Tracer_StartMessageFormat;
            string pattern = ConvertFormatToRegex(format);

            Regex re = new Regex(pattern);

            Assert.IsTrue(re.IsMatch(message));

            MatchCollection matches = re.Matches(message);
            foreach (Match match in matches)
            {
                Assert.IsNotNull(match.Value);
                Assert.IsTrue(match.Value.ToString().Length > 0);
            }
        }

        void AssertMessageIsValidEndMessage(string message)
        {
            Assert.IsNotNull(message);

            string format = Resources.Tracer_EndMessageFormat;
            string pattern = ConvertFormatToRegex(format);

            Regex re = new Regex(pattern);

            Assert.IsTrue(re.IsMatch(message));

            MatchCollection matches = re.Matches(message);
            foreach (Match match in matches)
            {
                Assert.IsNotNull(match.Value);
                Assert.IsTrue(match.Value.ToString().Length > 0);
            }
        }

        string ConvertFormatToRegex(string format)
        {
            string pattern = format;
            pattern = pattern.Replace("(", @"\(");
            pattern = pattern.Replace(")", @"\)");
            pattern = Regex.Replace(pattern, @"\{[0-9]\}", "(.*?)");
            return pattern;
        }
    }


}
