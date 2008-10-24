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
    /// <remarks>
    /// The <see cref="BackgroundScheduler"/> will make its best effort to avoid scheduling a new scavenge request 
    /// when it is safe to assume that it's not necessary. Since <see cref="ScavengerTask.NumberOfItemsToBeScavenged"/> 
    /// elements are scavenged each time, there should be at least one scavenge request every 
    /// <see cref="ScavengerTask.NumberOfItemsToBeScavenged"/> elements the cache over the 
    /// <see cref="CacheCapacityScavengingPolicy.MaximumItemsAllowedBeforeScavenging"/> threshold.
    /// <para/>
    /// Each time a scheduled scavenge task is processed the counter used to avoid superfluous scavenges is reset to 
    /// zero, so the next scavenge request will result in a new scheduled scavenge task.
    /// </remarks>
    public class BackgroundScheduler : ICacheScavenger
    {
        private ProducerConsumerQueue inputQueue = new ProducerConsumerQueue();
        private Thread inputQueueThread;
        private ExpirationTask expirer;
        private ScavengerTask scavenger;
        private bool isActive;
        private bool running;
        private CachingInstrumentationProvider instrumentationProvider;
        private object ignoredScavengeRequestsCountLock = new object();
        private int ignoredScavengeRequestsCount;

        /// <summary>
        /// Initialize a new instance of the <see cref="BackgroundScheduler"/> with a <see cref="ExpirationTask"/> and 
        /// a <see cref="ScavengerTask"/>.
        /// </summary>
        /// <param name="expirer">The expiration task to use.</param>
        /// <param name="scavenger">The scavenger task to use.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public BackgroundScheduler(ExpirationTask expirer, ScavengerTask scavenger, CachingInstrumentationProvider instrumentationProvider)
        {
            this.expirer = expirer;
            this.scavenger = scavenger;
            this.instrumentationProvider = instrumentationProvider;
            this.ignoredScavengeRequestsCount = 0;

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
            // Despite its name, this method will schedule a scavenge task.
            // This request will be ignored if the request is superfluous, ie if a previous request
            // has not been processed yet and the current request would "fit" on it

            bool scheduleRequest = false;

            // This lock is required because by now the Cache would have released the lock for the in-memory 
            // representation and would only have the lock for the specific item (see Cache.Add()).
            // The overhead caused by acquiring this lock would be partially offset by avoiding the call to 
            // ProducerConsumerQueue.Enqueue() in some occasions; how many times this call will be avoided will depend 
            // on the load: the higher the load the more likely it will be avoided.
            lock (ignoredScavengeRequestsCountLock)
            {
                int currentCount = ignoredScavengeRequestsCount;

                scheduleRequest = currentCount == 0;

                ignoredScavengeRequestsCount = (currentCount + 1) % this.scavenger.NumberOfItemsToBeScavenged;
            }

            if (scheduleRequest)
            {
                inputQueue.Enqueue(new StartScavengingMsg(this));
            }
        }

        internal void DoStartScavenging()
        {
            lock (ignoredScavengeRequestsCountLock)
            {
                // This will make the next schedule request to be scheduled even if it may not be necessary; the
                // bookkeeping required to make a more accurate decision is more complex and the outcome cannot 
                // be 100% reliable.
                // The lock taken will impact the background scheduler thread only.
                ignoredScavengeRequestsCount = 0;
            }

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
