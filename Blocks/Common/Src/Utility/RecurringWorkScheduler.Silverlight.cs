using System;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    /// <summary>
    /// Scheduler that wraps a timer.
    /// </summary>
    public class RecurringWorkScheduler : IRecurringWorkScheduler, IDisposable
    {
        private Timer timer;
        private readonly TimeSpan pollInterval;
        private Action expirationAction;
        private bool started;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringWorkScheduler"/> class.
        /// </summary>
        /// <param name="pollInterval">The poll interval.</param>
        public RecurringWorkScheduler(TimeSpan pollInterval)
        {
            if (pollInterval <= TimeSpan.Zero)
                throw new ArgumentException("pollInterval");

            this.timer = new Timer(OnTimerIntervalElapsed);
            this.pollInterval = pollInterval;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.timer != null)
            {
                this.Stop();
                this.timer.Dispose();
                this.timer = null;
            }
        }

        /// <summary>
        /// Set the delegate that will be run when the schedule
        /// determines it should run.
        /// </summary>
        /// <param name="workToDo"></param>
        public void SetAction(Action workToDo)
        {
            this.expirationAction = workToDo;
        }

        /// <summary>
        /// Start the scheduler running.
        /// </summary>
        public void Start()
        {
            if (this.timer != null)
            {
                this.started = true;
                this.timer.Change(pollInterval, TimeSpan.FromMilliseconds(-1));
            }
        }

        /// <summary>
        /// Stop the scheduler.
        /// </summary>
        public void Stop()
        {
            this.started = false;
            if (this.timer != null)
            {
                this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Forces the scheduler to perform the action as soon as possible, and not necessarily in a synchronous manner.
        /// </summary>
        public void ForceDoWork()
        {
            if (timer != null)
            {
                this.timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
            }
        }

        private void OnTimerIntervalElapsed(object state)
        {
            if (timer != null)
            {
                this.expirationAction.Invoke();
                if (this.started)
                {
                    this.Start();
                }
            }
        }
    }
}
