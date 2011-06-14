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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_unavailable_network
{
    [TestClass]
    public class when : Context
    {
        protected override void Act()
        {
        }

        [TestMethod]
        public void then_timer_is_stopped()
        {
            Assert.IsFalse(IsTimerStarted);
        }

        [TestMethod]
        public void then_network_change_restarts_timer()
        {
            NetworkStatusMock.Setup(x => x.GetIsNetworkAvailable()).Returns(true);
            NetworkStatusMock.Raise(x => x.NetworkStatusUpdated += null, EventArgs.Empty);

            Assert.IsTrue(IsTimerStarted);
        }
    }
}
