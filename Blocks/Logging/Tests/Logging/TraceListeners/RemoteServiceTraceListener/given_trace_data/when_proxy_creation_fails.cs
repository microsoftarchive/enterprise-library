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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_trace_data
{
    [TestClass]
    public class when_proxy_creation_fails : Context
    {
        protected override void Act()
        {
            LoggingServiceFactoryMock.Setup(x => x.CreateChannel()).Throws<ProxyCreationException>();

            Listener.TraceData(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry);
            DoWork();
        }

        [TestMethod]
        public void then_error_is_notified_to_async_reporter()
        {
            AsyncTracingErrorReporterMock.Verify(x => x.ReportErrorDuringTracing(It.Is<string>(m => m.Contains(typeof(ProxyCreationException).FullName))), Times.Once());
        }
    }
}
