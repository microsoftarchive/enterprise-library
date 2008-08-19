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
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// Represents a cache scavenger that runs on a background thread.
	/// </summary>
    public class BackgroundScheduler : ICacheScavenger
    {
        private ProducerConsumerQueue inputQueue = new ProducerConsumerQueue();
        private Thread inputQueueThread;
        private ExpirationTask expirer;
        private ScavengerTask scavenger;
        private bool isActive;
        private bool running;
		private CachingInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Initialize a new instance of the <see cref="BackgroundScheduler"/> with a <see cref="ExpirationTask"/> and a <see cref="ScavengerTask"/>.
		/// </summary>
		/// <param name="expirer">The expiration task to use.</param>
		/// <param name="scavenger">The scavenger task to use.</param>
		/// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public BackgroundScheduler(ExpirationTask expirer, ScavengerTask scavenger, CachingInstrumentationProvider instrumentationProvider)
        {
            this.expirer = expirer;
            this.scavenger = scavenger;
			this.instrumentationProvider = instrumentationProvider;

            ThreadStart queueReader = new ThreadStart(QueueReader);
            inputQueueThread = new Thread(queueReader);
            inputQueueThread.IsBackground = true;
        }

		/// <summary>
		/// Starts the scavenger.
		/// </summary>
        public void Start()
        {
            running = true;
            inputQueueThread.Start();
        }

		/// <summary>
		/// Stops the scavenger.
		/// </summary>
        public void Stop()
        {
            running = false;
            inputQueueThread.Interrupt();
        }

		/// <summary>
		/// Determines if the scavenger is active.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if the scavenger is active; otherwise, <see langword="false"/>.
		/// </value>
        public bool IsActive
        {
            get { return isActive; }
        }

		/// <summary>
		/// Queues a message that the expiration timeout has expired.
		/// </summary>
		/// <param name="notUsed">Ignored.</param>
        public void ExpirationTimeoutExpired(object notUsed)
        {
            inputQueue.Enqueue(new ExpirationTimeoutExpiredMsg(this));
        }

		/// <summary>
		/// Starts the scavenging process.
		/// </summary>
        public void StartScavenging()
        {
            inputQueue.Enqueue(new StartScavengingMsg(this));
        }

        internal void DoStartScavenging()
        {
            scavenger.DoScavenging();
        }

        internal void DoExpirationTimeoutExpired()
        {
            expirer.DoExpirations();
        }
		
		private void QueueReader()
        {
            isActive = true;
            while (running)
            {
                IQueueMessage msg = inputQueue.Dequeue() as IQueueMessage;
                try
                {
                    if (msg == null)
                    {
                        continue;
                    }

                    msg.Run();
                }
                catch (ThreadInterruptedException)
                {
                }
				catch (Exception e)
				{
					instrumentationProvider.FireCacheFailed(Resources.BackgroundSchedulerProducerConsumerQueueFailure, e);
				}
            }
            isActive = false;
        }
    }
}
