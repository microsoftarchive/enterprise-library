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
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.ServiceProcess;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    public class DistributorService : ServiceBase
    {
        private bool initializeComponentsCalled = false;
        internal const string DefaultApplicationName = "Enterprise Library Logging Distributor Service";
        private static string NameTag = Properties.Resources.DistributorServiceNameTag;

        private DistributorEventLogger eventLogger;
        private string applicationName;
        private ServiceStatus status;

        private MsmqListener queueListener;

        /// <summary/>
        /// <exclude/>
        public DistributorService()
        {
            base.CanStop = true;
            base.CanPauseAndContinue = true;
            base.CanStop = true;
            base.AutoLog = false;
        }

        /// <summary/>
        /// <exclude/>
        private static void Main()
        {
            ServiceBase[] servicesToRun = new ServiceBase[] { new DistributorService() };
            ServiceBase.Run(servicesToRun);
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Gets or sets the current status of the service.  Values are defined in the <see cref="ServiceStatus"/> enumeration.
        /// </devdoc>
        public virtual ServiceStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Gets or sets the name of the Windows service.
        /// </devdoc>
        public string ApplicationName
        {
            get { return this.applicationName; }
            set { this.applicationName = value; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Gets the logger used to log events for this service.
        /// </devdoc>
        public DistributorEventLogger EventLogger
        {
            get { return this.eventLogger; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Gets or sets the <see cref="MsmqListener"/> for the service.
        /// </devdoc>
        public MsmqListener QueueListener
        {
            get { return this.queueListener; }
            set { this.queueListener = value; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Initialization of the service.  Start the queue listener and write status to event log.
        /// </devdoc>
        public void InitializeComponent()
        {
            try
            {
                // Use the default settings for log name and application name.
                // This is done to ensure the Windows service starts correctly.
                this.ApplicationName = DefaultApplicationName;

                this.eventLogger = new DistributorEventLogger();
                this.eventLogger.AddMessage(Resources.InitializeComponentStartedMessage, Resources.InitializeComponentStarted);
                this.status = ServiceStatus.OK;

                IConfigurationSource configurationSource = GetConfigurationSource();

                MsmqDistributorSettings distributorSettings = MsmqDistributorSettings.GetSettings(configurationSource);
                if (distributorSettings == null)
                {
                    throw new ConfigurationErrorsException(string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionCouldNotFindConfigurationSection,
                            MsmqDistributorSettings.SectionName));
                }

                this.queueListener = CreateListener(this, distributorSettings.QueueTimerInterval, distributorSettings.MsmqPath);

                this.ApplicationName = this.ServiceName;
                this.ApplicationName = distributorSettings.ServiceName;
                this.eventLogger.AddMessage(NameTag, this.ApplicationName);

                this.eventLogger.EventSource = this.ApplicationName;
                this.eventLogger.AddMessage(Resources.InitializeComponentCompletedMessage, Resources.InitializeComponentCompleted);
            }
            catch (LoggingException loggingException)
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceStartError, this.ApplicationName),
                    loggingException,
                    TraceEventType.Error);

                throw;
            }
            catch (Exception ex)
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceStartError, this.ApplicationName),
                    ex,
                    TraceEventType.Error);

                throw new LoggingException(Resources.ErrorInitializingService, ex);
            }
        }

        /// <summary>
        /// Returns a new MSMQ listener.
        /// </summary>
        /// <param name="distributorService">The distributor service for the listener.</param>
        /// <param name="timerInterval">The interval to check for new messages.</param>
        /// <param name="msmqPath">The name of the queue to get messages from.</param>
        /// <returns>A new MSMQ listener.</returns>
        protected virtual MsmqListener CreateListener(DistributorService distributorService, int timerInterval, string msmqPath)
        {
            return new MsmqListener(distributorService, timerInterval, msmqPath);
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The Windows service start event.
        /// </devdoc>
        [SecurityCritical]
        protected override void OnStart(string[] args)
        {
            if (!initializeComponentsCalled)
            {
                InitializeComponent();
            }

            try
            {
                SanityCheck sanityCheck = new SanityCheck(this);
                sanityCheck.StartCheckTimer();

                BootstrapBlocks();

                if (this.Status == ServiceStatus.OK)
                {
                    StartMsmqListener();

                    this.eventLogger.LogServiceStarted();
                }
            }
            catch (Exception e)
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceStartError, this.ApplicationName),
                    e,
                    TraceEventType.Error);

                this.Status = ServiceStatus.Shutdown;
            }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The Windows service stop event.
        /// </devdoc>
        [SecurityCritical]
        protected override void OnStop()
        {
            try
            {
                StopMsmqListener();
            }
            catch (Exception e)
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceStopError, this.ApplicationName),
                    e,
                    TraceEventType.Error);

                this.Status = ServiceStatus.Shutdown;
            }

            Logger.Reset();

            GC.Collect();
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The Windows service pause event.
        /// </devdoc>
        [SecurityCritical]
        protected override void OnPause()
        {
            try
            {
                if (this.queueListener.StopListener())
                {
                    this.eventLogger.LogServicePaused();
                }
                else
                {
                    this.eventLogger.LogServiceFailure(
                        string.Format(CultureInfo.CurrentCulture, Resources.ServicePauseWarning, this.ApplicationName),
                        null,
                        TraceEventType.Warning);
                }
            }
            catch (Exception e)
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServicePauseError, this.ApplicationName),
                    e,
                    TraceEventType.Error);

                this.Status = ServiceStatus.Shutdown;
            }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The Windows service resume event.
        /// </devdoc>
        [SecurityCritical]
        protected override void OnContinue()
        {
            try
            {
                this.queueListener.StartListener();
                this.eventLogger.LogServiceResumed();
            }
            catch (Exception e)
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceResumeError, this.ApplicationName),
                    e,
                    TraceEventType.Error);

                this.Status = ServiceStatus.Shutdown;
            }
        }

        private void StartMsmqListener()
        {
            try
            {
                this.eventLogger.AddMessage(Resources.InitializeStartupSequenceStartedMessage, Resources.ValidationStarted);

                this.queueListener.StartListener();

                this.eventLogger.AddMessage(Resources.InitializeStartupSequenceFinishedMessage, Resources.ValidationComplete);
            }
            catch
            {
                this.eventLogger.AddMessage(Resources.InitializeStartupSequenceErrorMessage, Resources.ValidationError);

                this.Status = ServiceStatus.Shutdown;
                throw;
            }
        }

        private void StopMsmqListener()
        {
            if (this.queueListener.StopListener())
            {
                this.eventLogger.LogServiceStopped();
            }
            else
            {
                this.eventLogger.LogServiceFailure(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceStopWarning, this.ApplicationName),
                    null,
                    TraceEventType.Warning);
            }
        }

        private static IConfigurationSource GetConfigurationSource()
        {
            return ConfigurationSourceFactory.Create();
        }

        private static void BootstrapBlocks()
        {
            var configurationSource = GetConfigurationSource();
            // in case the data listener is used
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(configurationSource), false);
            Logger.SetLogWriter(new LogWriterFactory(configurationSource).Create(), false);
        }
    }
}
