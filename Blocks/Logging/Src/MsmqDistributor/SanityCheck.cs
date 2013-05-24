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
using System.Diagnostics;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// Verifies that the distributor service is running as expected and force the service
    /// to shutdown if a shutdown is pending.
    /// </summary>
    internal class SanityCheck : MarshalByRefObject
    {
        private const int SanityInterval = 5000;
        private static string Header = ExceptionFormatter.Header;

        private Timer timer;
        private DistributorEventLogger eventLogger;
        private DistributorService distributorService;

        public SanityCheck(DistributorService distributorService)
        {
            this.eventLogger = distributorService.EventLogger;
            this.distributorService = distributorService;
        }

        public void StartCheckTimer()
        {
            // This will stop the service from the moment ServiceStatus = SHUTDOWN.
            this.timer = new Timer();
            this.timer.Elapsed += new ElapsedEventHandler(OnSanityTimedEvent);
            this.timer.Interval = SanityInterval;
            this.timer.Enabled = true;
        }

        /// <summary>
        /// Stop the current service from running. 
        /// </summary>
        public void StopService()
        {
            try
            {
                ServiceController myController =
                    new ServiceController(this.distributorService.ApplicationName);
                myController.Stop();
            }
            catch (Exception e)
            {
                string errorMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.ServiceControllerStopException,
                    this.distributorService.ApplicationName);
                this.eventLogger.LogServiceFailure(
                    errorMessage,
                    e,
                    TraceEventType.Error);

                throw new LoggingException(errorMessage, e);
            }
        }

        /// <devdoc>
        /// Event triggered by sanity event handler. 
        /// This method runs regularly to check sanity of the service.
        /// </devdoc>
        private void OnSanityTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (this.distributorService.Status == ServiceStatus.Shutdown)
            {
                try
                {
                    ShutdownQueueListener();
                }
                catch (Exception err)
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.ServiceUnableToShutdown, this.distributorService.ApplicationName);
                    this.eventLogger.LogServiceFailure(
                        errorMessage,
                        err,
                        TraceEventType.Error);
                    this.distributorService.Status = ServiceStatus.PendingShutdown;
                }
            }
        }

        private void ShutdownQueueListener()
        {
            bool result = this.distributorService.QueueListener.StopListener();
            if (result)
            {
                StopService();
            }
            else
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceControllerStopException, this.distributorService.ApplicationName),
                    null,
                    TraceEventType.Error);
            }
        }
    }
}
