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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class given_a_configured_iso_storage_tracelistener : ArrangeActAssert
    {
        protected LogWriterImpl LogWriter;
        protected IsolatedStorageTraceListener listener;
        protected Mock<ILogEntryRepository> repository;

        protected override void Arrange()
        {
            base.Arrange();

            repository = new Mock<ILogEntryRepository>();
            repository.Setup(x => x.IsAvailable).Returns(true);
            repository.Setup(x => x.ActualMaxSizeInKilobytes).Returns(1000);
            listener = new IsolatedStorageTraceListener(repository.Object) { Name = "isolated" };

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[0],
                    new LogSource[]
                    {
                        new LogSource("category1", new TraceListener[] { listener }, SourceLevels.Error, true),
                        new LogSource("category2", new TraceListener[] { listener }, SourceLevels.Error, true),
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
            var listenerContext = ((IIsolatedStorageTraceListenerUpdateContext)context.Listeners.Single(x => x.Name == "isolated"));
            Assert.AreEqual(true, listenerContext.IsRepositoryAvailable);
            Assert.AreEqual(1000, listenerContext.MaxSizeInKilobytes);
        }

        [TestMethod]
        public void then_can_update_isolated_storage_quota_size()
        {
            var context = LogWriter.GetUpdateContext();
            ((IIsolatedStorageTraceListenerUpdateContext)context.Listeners.Single(x => x.Name == "isolated")).MaxSizeInKilobytes = 2000;
            repository.Verify(x => x.Resize(It.IsAny<int>()), Times.Never());

            context.ApplyChanges();

            repository.Verify(x => x.Resize(2000), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void when_repository_unavailable_then_can_update_isolated_storage_quota_size()
        {
            repository.Setup(x => x.ActualMaxSizeInKilobytes).Returns(0);
            repository.Setup(x => x.IsAvailable).Returns(false);

            var context = LogWriter.GetUpdateContext();
            var listenerContext = ((IIsolatedStorageTraceListenerUpdateContext)context.Listeners.Single(x => x.Name == "isolated"));
            Assert.AreEqual(false, listenerContext.IsRepositoryAvailable);
            listenerContext.MaxSizeInKilobytes = 2000;
        }
    }
}
