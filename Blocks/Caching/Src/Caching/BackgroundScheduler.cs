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
        private ExpirationTask expirationTask;
        private ScavengerTask scavengerTask;
        private ICachingInstrumentationProvider instrumentationProvider;

        private object scavengeExpireLock = new object();
        private int scavengePending = 0;

        /// <summary>
        /// Initialize a new instance of the <see cref="BackgroundScheduler"/> with a <see cref="ExpirationTask"/> and 
        /// a <see cref="ScavengerTask"/>.
        /// </summary>
        /// <param name="expirationTask">The expiration task to use.</param>
        /// <param name="scavengerTask">The scavenger task to use.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public BackgroundScheduler(ExpirationTask expirationTask, ScavengerTask scavengerTask, ICachingInstrumentationProvider instrumentationProvider)
        {
            this.expirationTask = expirationTask;
            this.scavengerTask = scavengerTask;
            this.instrumentationProvider = instrumentationProvider;
        }

        /// <summary>
        /// Queues a message that the expiration timeout has expired.
        /// </summary>
        /// <param name="notUsed">Ignored.</param>
        public void ExpirationTimeoutExpired(object notUsed)
        {
            ThreadPool.QueueUserWorkItem(unused => BackgroundWork(Expire));
        }

        /// <summary>
        /// Starts the scavenging process.
        /// </summary>
        public void StartScavenging()
        {
            int pendingScavengings = Interlocked.Increment(ref scavengePending);
            if (pendingScavengings == 1)
            {
                ThreadPool.QueueUserWorkItem(unused => BackgroundWork(Scavenge));
            }
        }

        internal void StartScavengingIfNeeded()
        {
            if (scavengerTask.IsScavengingNeeded())
            {
                StartScavenging();
            }              
        }

        internal void BackgroundWork(Action work)
        {
            try
            {
                lock (scavengeExpireLock)
                {
                    work();
                }
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCacheFailed(Resources.BackgroundSchedulerProducerConsumerQueueFailure, e);
            }
        }

        internal void Scavenge()
        {
            int pendingScavengings = Interlocked.Exchange(ref scavengePending, 0);
            int timesToScavenge = ((pendingScavengings - 1) % scavengerTask.NumberOfItemsToBeScavenged) + 1;
            while (timesToScavenge > 0)
            {
                scavengerTask.DoScavenging();
                --timesToScavenge;
            }
        }

        internal void Expire()
        {
            expirationTask.DoExpirations();
        }

    }
}
