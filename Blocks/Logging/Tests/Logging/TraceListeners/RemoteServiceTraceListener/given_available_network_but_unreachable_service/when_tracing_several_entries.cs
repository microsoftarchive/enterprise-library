using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_available_network_but_unreachable_service
{
    [TestClass]
    public class when_tracing_several_entries : Context
    {
        protected override void Act()
        {
            for (int i = 0; i < BufferSize; i++)
            {
                Listener.TraceData(TestTraceEventCache, TestSource, new TraceEventType(), i, new LogEntry { Message = i.ToString(), Severity = TraceEventType.Warning });
                DoWork();
            }

            LoggingServiceMock.Setup(x => x.EndAdd(It.IsAny<IAsyncResult>()));
        }

        [TestMethod]
        public void then_all_entries_are_buffered_and_intended_to_be_sent()
        {
            Assert.AreEqual(BufferSize, SendLogEntriesMessages.Count);
            for (int i = 0; i < BufferSize; i++)
            {
                Assert.AreEqual(i + 1, SendLogEntriesMessages[i].Length);
                for (int j = 0; j <= i; j++)
                {
                    Assert.AreEqual(j.ToString(), SendLogEntriesMessages[i][j].Message);
                }
            }
        }

        [TestMethod]
        public void then_additional_entries_start_discarding_older_messages()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, new TraceEventType(), 999, TestLogEntry1);
            DoWork();

            Assert.AreEqual(BufferSize + 1, SendLogEntriesMessages.Count);
            Assert.AreEqual(BufferSize, SendLogEntriesMessages[BufferSize].Length);
            for (int j = 1; j < BufferSize; j++)
            {
                Assert.AreEqual(j.ToString(), SendLogEntriesMessages[BufferSize][j-1].Message);
            }

            Assert.AreEqual(TestMessage1, SendLogEntriesMessages[BufferSize][BufferSize - 1].Message);
        }

        [TestMethod]
        public void then_successful_send_of_entries_empties_buffer()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, new TraceEventType(), 999, TestLogEntry1);
            DoWork();
            // buffer should be emptied by now

            Listener.TraceData(TestTraceEventCache, TestSource, new TraceEventType(), 999, TestLogEntry2);

            DoWork();
            DoWork();

            Assert.AreEqual(BufferSize + 2, SendLogEntriesMessages.Count);
            Assert.AreEqual(1, SendLogEntriesMessages[BufferSize + 1].Length);
            Assert.AreEqual(TestMessage2, SendLogEntriesMessages[BufferSize + 1][0].Message);
        }
    }
}
