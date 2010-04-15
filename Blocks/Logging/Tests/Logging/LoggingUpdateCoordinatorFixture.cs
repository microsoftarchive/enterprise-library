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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Barrier = Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Barrier;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class GivenANullConfigurationChangeEventSource
    {
        private LoggingUpdateCoordinator coordinator;

        [TestInitialize]
        public void Setup()
        {
            coordinator = new LoggingUpdateCoordinator(null);
        }

        [TestMethod]
        public void WhenAReadOperationIsBeingPerformed_ThenWritesAreNotProcessedUntilTheReadOperationIsDone()
        {
            var lastUpdate = "none";
            var readSync = new AutoResetEvent(false);
            var writeSync = new AutoResetEvent(false);
            var barrier = new Barrier(2);

            ThreadPool.QueueUserWorkItem((o) =>
            {
                readSync.WaitOne(2000);
                this.coordinator.ExecuteReadOperation(() =>
                {
                    writeSync.Set();
                    Thread.Sleep(1000);
                    lastUpdate = "read";
                });
                barrier.Await();
            });

            readSync.Set();
            writeSync.WaitOne(2000);
            this.coordinator.ExecuteWriteOperation(() =>
            {
                lastUpdate = "write";
            });
            barrier.Await();

            Assert.AreEqual("write", lastUpdate);
        }

        [TestMethod]
        public void WhenDisposingCoordinator_ItSucceedsDespiteNoSource()
        {
            coordinator.Dispose();
        }
    }

    [TestClass]
    public class GivenALoggingUpdateCoordinator
    {
        private MockConfigurationChangeEventSource source;
        private UnityServiceLocator locator;
        private LoggingUpdateCoordinator coordinator;
        private Mock<ILoggingInstrumentationProvider> mockInstrumentationProvider;

        [TestInitialize]
        public void Setup()
        {
            this.source = new MockConfigurationChangeEventSource();
            this.locator = new UnityServiceLocator(new UnityContainer());
            mockInstrumentationProvider = new Mock<ILoggingInstrumentationProvider>();

            this.coordinator = new LoggingUpdateCoordinator(this.source, mockInstrumentationProvider.Object);
        }

        [TestMethod]
        public void WhenTheSourceRaisesAnEventForTheLoggingSectionChange_ThenTheCoordinatorRaisesThePrepareForUpdateEvent()
        {
            IServiceLocator notifiedLocator = null;

            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Returns(new object()).Callback((IServiceLocator l) => notifiedLocator = l);

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            Assert.AreSame(this.locator, notifiedLocator);
        }

        [TestMethod]
        public void WhenAReadOperationIsBeingPerformed_ThenWritesAreNotProcessedUntilTheReadOperationIsDone()
        {
            var lastUpdate = "none";
            var readSync = new AutoResetEvent(false);
            var writeSync = new AutoResetEvent(false);
            var barrier = new Barrier(2);

            ThreadPool.QueueUserWorkItem((o) =>
                {
                    readSync.WaitOne(2000);
                    this.coordinator.ExecuteReadOperation(() =>
                    {
                        writeSync.Set();
                        Thread.Sleep(1000);
                        lastUpdate = "read";
                    });
                    barrier.Await();
                });

            readSync.Set();
            writeSync.WaitOne(2000);
            this.coordinator.ExecuteWriteOperation(() =>
            {
                lastUpdate = "write";
            });
            barrier.Await();

            Assert.AreEqual("write", lastUpdate);
        }

        [TestMethod]
        public void WhenConfigurationChangeEventSourceRaisesItsEvent_ThenTheUpdateHandlerCommitsAreGuardedByTheWriterLock()
        {
            var lastUpdate = "none";
            var updateSync = new AutoResetEvent(false);
            var readSync = new AutoResetEvent(false);
            var barrier = new Barrier(2);

            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.CommitUpdate(It.IsAny<object>()))
                .Callback(() =>
                                {
                                    readSync.Set();
                                    Thread.Sleep(1000);
                                    lastUpdate = "config update";
                                });

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            ThreadPool.QueueUserWorkItem((o) =>
            {
                updateSync.WaitOne(2000);

                this.source.OnSectionChanged<LoggingSettings>(
                    new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

                barrier.Await();
            });

            updateSync.Set();
            readSync.WaitOne(2000);
            this.coordinator.ExecuteReadOperation(() =>
            {
                lastUpdate = "read";
            });

            barrier.Await();

            Assert.AreEqual("read", lastUpdate);
        }

        [TestMethod]
        public void WhenConfigurationChangeEventSourceRaisesItsEvent_ThenThePrepareUpdateIsCalledBeforeCommitUpdate()
        {
            bool PrepareForUpdateCalled = false;
            bool CommitUpdateCalled = false;

            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Returns(new object()).Callback((IServiceLocator l) => PrepareForUpdateCalled = true);

            updateHandler.Setup(h => h.CommitUpdate(It.IsAny<object>()))
                .Callback((object o) =>
                              {
                                  CommitUpdateCalled = true;
                                  Assert.IsTrue(PrepareForUpdateCalled);
                              });
            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            Assert.IsTrue(CommitUpdateCalled);
        }

        [TestMethod]
        public void WhenConfigurationChangeEventSourceRaisesItsEvent_TheCommitIsSuppliedWithTheContextFromPrepare()
        {
            object contextObject = new object();

            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Returns(contextObject);

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            updateHandler.Verify(h => h.CommitUpdate(contextObject));
        }

        [TestMethod]
        public void WhenConfigurationChangeEventSourceRaisesItsEvent_EachHandlerIsSuppliedItsContext()
        {
            object contextObject = new object();
            object anotherContextObject = new object();

            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Returns(contextObject);

            var anotherUpdateHandler = new Mock<ILoggingUpdateHandler>();
            anotherUpdateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Returns(anotherContextObject);

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);
            this.coordinator.RegisterLoggingUpdateHandler(anotherUpdateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            updateHandler.Verify(h => h.CommitUpdate(contextObject));
            anotherUpdateHandler.Verify(h => h.CommitUpdate(anotherContextObject));
        }

        [TestMethod]
        public void WhenConfigurationChangedAndAHandlerThrowsAnErrorDuringPrepare_ThenCommitUpdateMethodIsNotInvoked()
        {

            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Callback(() => { throw new Exception("TestException"); });

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            updateHandler.Verify(h => h.CommitUpdate(It.IsAny<object>()), Times.Never(), "CommitUpdate was called.");
        }

        [TestMethod]
        public void WhenConfigurationChangedAndAHandlerThrowsAnActivationErrorDuringPrepare_ThenCoordinatorLogsConfigErrorMessage()
        {
            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Throws(new ActivationException("TestActivationException"));

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            mockInstrumentationProvider.Verify(p => p.FireConfigurationFailureEvent(It.IsAny<ActivationException>()));
        }

        [TestMethod]
        public void WhenConfigurationChangedAndAHandlerThrowsANonActivationErrorDuringPrepare_ThenCoordinatorLogsGeneralLogErrorException()
        {
            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Throws(new Exception("TestException"));

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            mockInstrumentationProvider.Verify(p => p.FireReconfigurationErrorEvent(It.IsAny<Exception>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenConfigurationChangedAndHandlerThrowsDuringCommit_ThenExceptionIsBubbled()
        {
            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.CommitUpdate(It.IsAny<object>())).Throws(new ArgumentNullException("TestException"));

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));
        }

        [TestMethod]
        public void WhenConfigurationChangedAndHandlerHasUnregistered_ItIsNotInvoked()
        {
            bool handlerInvoked = false;

            var updateHandler = new Mock<ILoggingUpdateHandler>();
            updateHandler.Setup(h => h.PrepareForUpdate(It.IsAny<IServiceLocator>()))
                .Callback(() => handlerInvoked = true);

            this.coordinator.RegisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));

            Assert.IsTrue(handlerInvoked);
            handlerInvoked = false;
            this.coordinator.UnregisterLoggingUpdateHandler(updateHandler.Object);

            this.source.OnSectionChanged<LoggingSettings>(
                new SectionChangedEventArgs<LoggingSettings>(new LoggingSettings(), this.locator));
            Assert.IsFalse(handlerInvoked);
        }

        [TestMethod]
        public void WhenDisposingCoordinator_ThenItUnregistersFromEventSource()
        {
            this.coordinator.Dispose();
            Assert.IsFalse(this.source.handlers.ContainsKey(this.coordinator.GetType()));
        }
    }
}
