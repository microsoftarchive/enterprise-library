using System;
using System.Threading;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling
{
    /// <summary>
    /// A scheduler that will queue scavenging requests onto a background
    /// thread in the thread pool. Only one scavenging request will
    /// be queued at a time.
    /// </summary>
    public class ScavengingScheduler : IManuallyScheduledWork, IDisposable
    {
        private Action scavengingAction;
        private int scheduledScavenges;

        #region IManuallyScheduledWork Members

        public void SetAction(Action workToDo)
        {
            this.scavengingAction = workToDo;
        }

        public void ScheduleWork()
        {
            if (Interlocked.Increment(ref scheduledScavenges) == 1)
            {
                // only the 1st scheduled scanging at a time actually enqueues a scavenge
                bool enqueued = ThreadPool.QueueUserWorkItem(_ =>
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

                Debug.Assert(enqueued);
            }
        }

        #endregion

        public void Dispose()
        {
            if (this.scavengingAction != null)
            {
                this.scavengingAction = null;
            }
        }
    }
}
