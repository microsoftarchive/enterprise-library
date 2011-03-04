using System;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling
{
    /// <summary>
    /// A scheduler that will queue scavenging requests onto a background
    /// thread in the thread pool. Only one scavenging request will
    /// be queued at a time.
    /// </summary>
    public class ScavengingScheduler : IManuallyScheduledWork
    {
        private Action scavengingAction;
        private int scheduledScavenges;

        #region IManuallyScheduledWork Members

        public void SetAction(Action workToDo)
        {
            scavengingAction = workToDo;
        }

        public void ScheduleWork()
        {
            if (Interlocked.Increment(ref scheduledScavenges) == 1)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Interlocked.Exchange(ref scheduledScavenges, 0);
                    scavengingAction();
                });
            }
        }

        #endregion
    }
}
