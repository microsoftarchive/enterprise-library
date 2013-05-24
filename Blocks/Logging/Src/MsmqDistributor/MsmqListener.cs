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
using System.Threading;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Timer = System.Timers.Timer;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
	/// <summary>
	/// Represents the message queue polling timer.  Uses an <see cref="MsmqLogDistributor"/> 
	/// to check for new log messages each timer interval.
	/// </summary>
	public class MsmqListener
	{
		/// <summary>
		/// <exclude/>
		/// </summary>
		public int QueueListenerRetries = 30;

		private const int DefaultQueueTimerInterval = 20000;
		private const int QueueListenerRetrySleepTime = 1000;
		private static int queueTimerInterval;

		private System.Timers.Timer queueTimer = null;
		private DistributorEventLogger eventLogger = null;

		private DistributorService distributorService;
		private MsmqLogDistributor logDistributor;

		/// <summary>
		/// Initialize a new instance of the <see cref="MsmqListener"/>.
		/// </summary>
		/// <param name="distributorService">Distributor service inheriting from <see cref="System.ServiceProcess.ServiceBase"/>.</param>
		/// <param name="timerInterval">Interval to check for new messages.</param>
		/// <param name="msmqPath">The name of the queue to get messages from.</param>
		public MsmqListener(DistributorService distributorService, int timerInterval, string msmqPath)
		{
			this.distributorService = distributorService;
			this.QueueTimerInterval = timerInterval;
			this.eventLogger = distributorService.EventLogger;

			this.logDistributor = new MsmqLogDistributor(msmqPath, this.eventLogger);
		}

		/// <summary>
		/// Polling interval to check for new log messages.
		/// </summary>
		public int QueueTimerInterval
		{
			get
			{
				if (queueTimerInterval == 0)
				{
					return DefaultQueueTimerInterval;
				}
				else
				{
					return queueTimerInterval;
				}
			}

			set
			{
				if (value == 0)
				{
					queueTimerInterval = DefaultQueueTimerInterval;
				}
				else
				{
					queueTimerInterval = value;
				}
			}
		}

		/// <summary>
		/// Start the queue listener and begin polling the message queue.
		/// </summary>
		public virtual void StartListener()
		{
			try
			{
				this.eventLogger.AddMessage(Resources.ListenerStartingMessage, Resources.ListenerStarting);

				this.logDistributor.StopReceiving = false;

				if (this.queueTimer == null)
				{
					this.queueTimer = new Timer();
					this.queueTimer.Elapsed += new ElapsedEventHandler(OnQueueTimedEvent);
				}
				this.queueTimer.Interval = this.QueueTimerInterval;
				this.queueTimer.Enabled = true;

                this.eventLogger.AddMessage(Resources.ListenerStartCompleteMessage, string.Format(CultureInfo.CurrentCulture, Resources.ListenerStartComplete, this.QueueTimerInterval));
			}
			catch (Exception e)
			{
				this.eventLogger.AddMessage(Resources.ListenerStartErrorMessage, Resources.ListenerStartError);
				this.eventLogger.AddMessage(Resources.Exception, e.Message);
				throw;
			}
		}

		/// <summary>
		/// Attempt to stop the queue listener and discontinue polling the message queue.
		/// </summary>
		/// <returns>True if the listener stopped succesfully.</returns>
		public virtual bool StopListener()
		{
			try
			{
				if (this.queueTimer != null)
				{
					this.eventLogger.AddMessage(Resources.ListenerStopStartedMessage, Resources.ListenerStopStarted);

					this.queueTimer.Enabled = false;
					this.logDistributor.StopReceiving = true;

					if (WaitUntilListenerStopped())
					{
						return true;
					}

					this.queueTimer.Enabled = true;
					this.logDistributor.StopReceiving = false;
					return false;
				}
				return true;
			}
			catch (Exception e)
			{
				this.eventLogger.AddMessage(Resources.ListenerStopErrorMessage, Resources.ListenerStopError);
				this.eventLogger.AddMessage(Resources.Exception, e.Message);
				throw;
			}
		}

		private bool WaitUntilListenerStopped()
		{
			int timeOut = 0;
			while (timeOut < QueueListenerRetries)
			{
				// Try to stop for QueueListenerRetries retries (1 second per retry)
				if (this.logDistributor.IsCompleted)
				{
					this.eventLogger.AddMessage(Resources.ListenerStopCompletedMessage,
                                                string.Format(CultureInfo.CurrentCulture, Resources.ListenerStopCompleted, timeOut.ToString(CultureInfo.InvariantCulture)));

					return true;
				}
				Thread.Sleep(QueueListenerRetrySleepTime);
				++timeOut;
			}

			this.eventLogger.AddMessage(Resources.StopListenerWarningMessage,
                                        string.Format(CultureInfo.CurrentCulture, Resources.ListenerCannotStop, timeOut.ToString(CultureInfo.InvariantCulture)));

			return false;
		}

		/// <summary>
		/// <exclude />
		/// </summary>
		/// <devdoc>
		/// support unit tests - allows for a mock object
		/// </devdoc>
		public void SetMsmqLogDistributor(MsmqLogDistributor logDistributor)
		{
			this.logDistributor = logDistributor;
		}

		/// <devdoc>
		/// Event triggered by the queue timer event handler. 
		/// This method runs regularly to check the queue for pending queue messages.
		/// </devdoc>
		private void OnQueueTimedEvent(object sender, ElapsedEventArgs args)
		{
			try
			{
				if (this.logDistributor.IsCompleted &&
					(this.distributorService.Status == ServiceStatus.OK))
				{
					this.logDistributor.CheckForMessages();
				}
			}
			catch (Exception e)
			{
				this.eventLogger.LogServiceFailure(
					Resources.QueueTimedEventError,
					e,
					TraceEventType.Error);

				this.distributorService.Status = ServiceStatus.Shutdown;
			}
		}
	}
}
