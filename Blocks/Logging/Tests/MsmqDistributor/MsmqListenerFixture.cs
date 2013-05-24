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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestClass]
    public class MsmqListenerFixture
    {
        MsmqListener listener;
        DistributorServiceTestFacade distributorServiceTestFacade;
        MockMsmqLogDistributor mockQ;

        //private DistributorEventLogger eventLogger;

        [TestInitialize]
        public void Setup()
        {
            Logger.SetLogWriter(new LogWriter(new List<ILogFilter>(), new List<LogSource>(), new LogSource("errors"), "default"), throwIfSet: false);
            distributorServiceTestFacade = new DistributorServiceTestFacade();
            distributorServiceTestFacade.Initialize();
            listener = new MsmqListener(distributorServiceTestFacade, 1000, CommonUtil.MessageQueuePath);
            //this.eventLogger = distributorServiceTestFacade.EventLogger;
            mockQ = new MockMsmqLogDistributor(CommonUtil.MessageQueuePath);
        }

        [TestCleanup]
        public void Teardown() 
        {
            Logger.Reset();
        }

        [TestMethod]
        public void StartListener()
        {
            //this.eventLogger.AddMessage("HEADER", "Simulated Start");

            listener.QueueTimerInterval = 10;
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();

            Thread.Sleep(listener.QueueTimerInterval + 300);
            Assert.IsTrue(mockQ.ReceiveMsgCalled, "receive initiated");

            //this.eventLogger.WriteToLog(new Exception("simulated exception - forced event logger flush"), Severity.Error);

            //Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStarting), "start begin");
            //Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStartComplete(listener.QueueTimerInterval)), "start complete");

            listener.StopListener();
        }

        [TestMethod]
        public void StopListener()
        {
            //this.eventLogger.AddMessage("HEADER", "Simulated Stop");

            listener.QueueTimerInterval = 10;
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();

            Thread.Sleep(listener.QueueTimerInterval + 300);
            bool result = listener.StopListener();

            Assert.IsTrue(result, "stopListener result");

            try
            {
                throw new Exception("simulated exception - forced event logger flush");
            }
            catch (Exception /* e */)
            {
                //this.eventLogger.WriteToLog(e, Severity.Error);
            }

            //Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopStarted), "stop begin");
            //Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopCompleted("0")), "stop complete");
        }

        [TestMethod]
        public void StopListenerAndExceedStopRetries()
        {
            //this.eventLogger.AddMessage("HEADER", "Simulated Stop and Exceed Timeout");

            listener.QueueTimerInterval = 10;
            listener.QueueListenerRetries = 1;
            mockQ.SetIsCompleted(false);
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();

            Thread.Sleep(listener.QueueTimerInterval + 300);
            bool result = listener.StopListener();

            Assert.IsFalse(mockQ.StopReceiving, "stop receiving");
            Assert.IsFalse(result, "stopListener result");

            try
            {
                throw new Exception("simulated exception - forced event logger flush");
            }
            catch (Exception /* e */)
            {
                //this.eventLogger.WriteToLog(e, Severity.Error);
            }
            //Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopStarted), "stop begin");
            //Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerCannotStop("1")), "cannot stop warning");
        }

        [TestMethod]
        public void StopListenerError()
        {
            //this.eventLogger.AddMessage("HEADER", "Simulated Stop Exception");

            listener.QueueTimerInterval = 10000;
            mockQ.ExceptionOnGetIsCompleted = true;
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();

            try
            {
                listener.StopListener();
            }
            catch (Exception /* e */)
            {
                //this.eventLogger.WriteToLog(e, Severity.Error);

                //Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopError), "stop error");
                return;
            }

            Assert.Fail("exception not raised");
        }

        [TestMethod]
        public void RevertToDefaultTimerInterval()
        {
            listener.QueueTimerInterval = 0;

            Assert.AreEqual(20000, listener.QueueTimerInterval);
        }
    }
}
