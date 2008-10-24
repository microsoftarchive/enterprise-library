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
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    internal class MockMsmqListener : MsmqListener
    {
        public bool ExceptionOnStart = false;
        public bool ExceptionOnStop = false;
        public bool StopReturnsFalse = false;

        public bool StartCalled = false;
        public bool StopCalled = false;

		public MockMsmqListener(DistributorService logDistributor, int timerInterval, string msmqPath)
            : base(logDistributor, timerInterval, msmqPath)
        {
        }

        public override void StartListener()
        {
            StartCalled = true;
            if (ExceptionOnStart)
            {
                throw new Exception("simulated exception");
            }
        }

        public override bool StopListener()
        {
            StopCalled = true;
            if (ExceptionOnStop)
            {
                throw new Exception("simulated exception");
            }
            if (StopReturnsFalse)
            {
                return false;
            }
            return true;
        }
    }
}
