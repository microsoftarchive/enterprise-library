//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class ExpirationPollTimerFixture
    {
        static int counter;

        [TestInitialize]
        public void InitializeCount()
        {
            counter = 0;
        }

        void CallbackMethod(object notUsed)
        {
            counter++;
        }

        [TestMethod]
        public void WillCallBackAtSetInterval()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(new TimerCallback(CallbackMethod), 100);
            Thread.Sleep(1100);
            timer.StopPolling();
            Assert.IsTrue((counter >= 9) && (counter <= 12));
        }

        [TestMethod]
        public void CanStopPolling()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(new TimerCallback(CallbackMethod), 100);
            Thread.Sleep(1100);
            timer.StopPolling();
            Thread.Sleep(250);
            Assert.IsTrue((counter >= 9) && (counter <= 12));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartingWithNullCallbackThrowsException()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(null, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartingWithZeroPollTimeThrowsException()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(new TimerCallback(CallbackMethod), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCallStopBeforeCallingStart()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StopPolling();
        }
    }
}
