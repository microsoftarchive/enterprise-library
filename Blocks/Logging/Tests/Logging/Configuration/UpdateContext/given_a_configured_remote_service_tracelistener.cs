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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class given_a_configured_remote_service_tracelistener : ArrangeActAssert
    {
        protected LogWriterImpl LogWriter;
        protected RemoteServiceTraceListener listener;
        protected Mock<ILogEntryMessageStore> store;

        protected override void Arrange()
        {
            base.Arrange();

            this.store = new Mock<ILogEntryMessageStore>();
            this.store.Setup(x => x.IsWritable).Returns(true);
            this.store.Setup(x => x.Quota).Returns(6000);

            listener = new RemoteServiceTraceListener(
                true, 
                Mock.Of<ILoggingServiceFactory>(), 
                Mock.Of<IRecurringWorkScheduler>(), 
                store.Object, 
                Mock.Of<IAsyncTracingErrorReporter>(), 
                Mock.Of<INetworkStatus>()) { Name = "remote" };

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[0],
                    new LogSource[]
                    {
                        new LogSource("category1", new TraceListener[] { listener }, SourceLevels.Error, true),
                    },
                    new LogSource(""),
                    new LogSource(""),
                    new LogSource(""),
                    "category1",
                    true,
                    false);
        }

        protected override void Teardown()
        {
            base.Teardown();
            LogWriter.Dispose();
            LogWriter = null;
        }

        [TestMethod]
        public void then_can_list_trace_listeners()
        {
            var context = LogWriter.GetUpdateContext();
            Assert.AreEqual(1, context.Listeners.Count);
            var listenerContext = ((IRemoteServiceTraceListenerUpdateContext)context.Listeners.Single(x => x.Name == "remote"));
            Assert.AreEqual(6000, listenerContext.IsolatedStorageBufferMaxSizeInKilobytes);
        }

        [TestMethod]
        public void then_can_update_to_send_immediately()
        {
            var context = LogWriter.GetUpdateContext();
            Assert.AreEqual(1, context.Listeners.Count);
            ((IRemoteServiceTraceListenerUpdateContext)context.Listeners.Single(x => x.Name == "remote")).SendImmediately = false;
            Assert.AreEqual(true, listener.SendImmediately);

            context.ApplyChanges();

            Assert.AreEqual(false, listener.SendImmediately);
        }

        [TestMethod]
        public void then_can_update_isolated_storage_quota_size()
        {
            var context = LogWriter.GetUpdateContext();

            ((IRemoteServiceTraceListenerUpdateContext)context.Listeners.Single(x => x.Name == "remote")).IsolatedStorageBufferMaxSizeInKilobytes = 15000;
            store.Verify(x => x.ResizeBackingStore(It.IsAny<int>()), Times.Never());

            context.ApplyChanges();

            store.Verify(x => x.ResizeBackingStore(15000), Times.Once());
        }
    }
}
