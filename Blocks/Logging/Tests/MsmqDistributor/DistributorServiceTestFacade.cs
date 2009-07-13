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


namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    public class DistributorServiceTestFacade : DistributorService
    {
        public const string MockServiceName = "Enterprise Library Logging Distributor Service";

        public DistributorServiceTestFacade()
        {
            this.ServiceName = MockServiceName;
			//this.EventLogger.EventSource = ServiceName;
        }

        private ServiceStatus status;

        public override ServiceStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        new public void OnContinue()
        {
            base.OnContinue();
        }

        public void Initialize()
        {
            base.InitializeComponent();
        }

        public void OnStart()
        {
            base.OnStart(null);
        }

        new public void OnStop()
        {
            base.OnStop();
        }

        new public void OnPause()
        {
            base.OnPause();
        }

        protected override MsmqListener CreateListener(DistributorService distributorService, int timerInterval, string msmqPath)
        {
            if (this.QueueListener != null)
            {
                return this.QueueListener;
            }

            return new MockMsmqListener(distributorService, timerInterval, msmqPath);
        }
    }
}
