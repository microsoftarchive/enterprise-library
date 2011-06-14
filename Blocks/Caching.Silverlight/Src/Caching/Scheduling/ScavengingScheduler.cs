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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling
{
    /// <summary>
    /// A scheduler that will queue scavenging requests onto a background
    /// thread in the thread pool. Only one scavenging request will
    /// be queued at a time.
    /// </summary>
    public sealed class ScavengingScheduler : IManuallyScheduledWork, IDisposable
    {
        private Action scavengingAction;
        private int scheduledScavenges;

        #region IManuallyScheduledWork Members

        /// <summary>
        /// Set the action that will be run when work is scheduled.
        /// </summary>
        /// <param name="workToDo">The <see cref="Action"/> that will
        /// be invoked when the action is scheduled.</param>
        public void SetAction(Action workToDo)
        {
            this.scavengingAction = workToDo;
        }

        /// <summary>
        /// Requests that the object perform its work
        /// at some point in the future.
        /// </summary>
        public void ScheduleWork()
        {
            if (Interlocked.Increment(ref scheduledScavenges) == 1)
            {
                // only the 1st scheduled scanging at a time actually enqueues a scavenge
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var action = this.scavengingAction;

                    try
                    {
                        if (action != null)
                        {
                            // if by the time is about to actually perform the scavenging action, there are more
                            // scheduled scavengings waiting in line, it collapses those into this running call
                            Interlocked.Exchange(ref scheduledScavenges, 1);
                            action();
                        }
                    }
                    finally
                    {
                        // if by the time it finished performing the scavenging action, there are more
                        // scheduled scavengings waiting in line, it resets the queue, and enqueues an
                        // additional call that will end up enqueueing a work item in the thread pool.
                        if (Interlocked.Exchange(ref scheduledScavenges, 0) > 1)
                        {
                            this.ScheduleWork();
                        }
                    }
                });
            }
        }

        #endregion

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            this.scavengingAction = null;
        }
    }
}
