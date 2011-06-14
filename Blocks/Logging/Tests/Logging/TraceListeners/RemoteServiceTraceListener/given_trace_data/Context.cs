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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_trace_data
{
    public abstract class Context : ArrangeActAssert
    {
        public class ProxyCreationException : Exception
        {
        }

        protected LogEntry TestLogEntry;
        protected Logging.TraceListeners.RemoteServiceTraceListener Listener;
        protected Mock<ILoggingService> LoggingServiceMock;
        protected Mock<ILoggingServiceFactory> LoggingServiceFactoryMock;
        protected Mock<IAsyncTracingErrorReporter> AsyncTracingErrorReporterMock;
        protected Mock<IRecurringWorkScheduler> TimerMock;
        protected IList<LogEntryMessage[]> SendLogEntriesMessages;
        protected Action DoWork;

        protected const string TestListenerName = "listenerName";
        protected const string TestSource = "source";
        protected const string TestMessage = "message";
        protected const int TestId = 100;
        protected readonly string[] TestCategories = new[] { "cat1", "cat2" };
        protected const TraceEventType TestTraceEventType = TraceEventType.Error;
        protected readonly TraceEventCache TestTraceEventCache = new TraceEventCache();
        protected readonly Dictionary<string, object> TestExtendedProperties = new Dictionary<string, object>
            {
                { "integer", "5" }, { "string", "test" }, { "long", 4L }, { "datetime", DateTimeOffset.UtcNow }, { "bool[]", new[] { true } }
            };

        protected override void Arrange()
        {
            base.Arrange();

            TestLogEntry = new LogEntry { Message = TestMessage, Categories = TestCategories, Severity = TestTraceEventType, ExtendedProperties = TestExtendedProperties};

            SendLogEntriesMessages = new List<LogEntryMessage[]>();
            LoggingServiceMock = new Mock<ILoggingService>();
            LoggingServiceMock
                .Setup(x => x.BeginAdd(It.IsAny<LogEntryMessage[]>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns<LogEntryMessage[], AsyncCallback, object>((m, c, s) =>
                {
                    SendLogEntriesMessages.Add(m);
                    var result = Mock.Of<IAsyncResult>(x => x.AsyncState == s && x.IsCompleted == true);
                    c.Invoke(result);
                    return result;
                });
            LoggingServiceMock.As<IDisposable>();

            LoggingServiceFactoryMock = new Mock<ILoggingServiceFactory>();
            LoggingServiceFactoryMock.Setup(x => x.CreateChannel()).Returns(this.LoggingServiceMock.Object);

            TimerMock = new Mock<IRecurringWorkScheduler>();
            TimerMock.Setup(x => x.SetAction(It.IsAny<Action>())).Callback<Action>(x => DoWork = x);
            AsyncTracingErrorReporterMock = new Mock<IAsyncTracingErrorReporter>();
            Listener = new Logging.TraceListeners.RemoteServiceTraceListener(
                true,
                LoggingServiceFactoryMock.Object,
                this.TimerMock.Object,
                new LogEntryMessageStore(TestListenerName, 15, 0),
                this.AsyncTracingErrorReporterMock.Object,
                Mock.Of<INetworkStatus>(x => x.GetIsNetworkAvailable() == true))
                {
                    Name = TestListenerName
                };
        }
    }
}
