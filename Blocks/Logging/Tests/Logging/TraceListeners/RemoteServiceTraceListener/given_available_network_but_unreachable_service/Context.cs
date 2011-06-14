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
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_available_network_but_unreachable_service
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntry TestLogEntry1;
        protected LogEntry TestLogEntry2;
        protected Logging.TraceListeners.RemoteServiceTraceListener Listener;
        protected Mock<ILoggingService> LoggingServiceMock;
        protected Mock<IAsyncTracingErrorReporter> AsyncTracingErrorReporterMock;
        protected IList<LogEntryMessage[]> SendLogEntriesMessages;
        protected Action DoWork;

        protected const string TestListenerName = "listenerName";
        protected const string TestSource = "source";
        protected const string TestMessage1 = "message1";
        protected const string TestMessage2 = "message2";
        protected const int BufferSize = 10;
        protected readonly TraceEventCache TestTraceEventCache = new TraceEventCache();

        protected override void Arrange()
        {
            base.Arrange();

            TestLogEntry1 = new LogEntry { Message = TestMessage1, Categories = new[] { "cat1", "cat2" }, Severity = TraceEventType.Error };
            TestLogEntry2 = new LogEntry { Message = TestMessage2, Categories = new[] { "cat2" }, Severity = TraceEventType.Warning };

            SendLogEntriesMessages = new List<LogEntryMessage[]>();
            LoggingServiceMock = new Mock<ILoggingService>();
            LoggingServiceMock
                .Setup(x => x.BeginAdd(It.IsAny<LogEntryMessage[]>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns<LogEntryMessage[], AsyncCallback, object>((m, c, s) =>
                {
                    SendLogEntriesMessages.Add(m);
                    var result = Mock.Of<IAsyncResult>(x => x.AsyncState == s && x.IsCompleted == false);
                    c.Invoke(result);
                    return result;
                });

            LoggingServiceMock
                .Setup(x => x.EndAdd(It.IsAny<IAsyncResult>()))
                .Throws<CommunicationException>();

            LoggingServiceMock.As<IDisposable>();

            var loggingServiceFactory = Mock.Of<ILoggingServiceFactory>(x => x.CreateChannel() == LoggingServiceMock.Object);

            var timerMock = new Mock<IRecurringWorkScheduler>();
            timerMock.Setup(x => x.SetAction(It.IsAny<Action>())).Callback<Action>(x => DoWork = x);
            AsyncTracingErrorReporterMock = new Mock<IAsyncTracingErrorReporter>();

            Listener = new Logging.TraceListeners.RemoteServiceTraceListener(
                true,
                loggingServiceFactory,
                timerMock.Object,
                new LogEntryMessageStore(TestListenerName, BufferSize, 0),
                this.AsyncTracingErrorReporterMock.Object,
                Mock.Of<INetworkStatus>(x => x.GetIsNetworkAvailable() == true))
                {
                    Name = TestListenerName
                };
        }
    }
}
