//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    /// <summary>
    /// Scheduler that wraps a timer.
    /// </summary>
    public sealed class RecurringWorkScheduler : IRecurringWorkScheduler, IDisposable
    {
        private Timer timer;
        private readonly TimeSpan pollInterval;
        private Action recurringWork;
        private bool started;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringWorkScheduler"/> class.
        /// </summary>
        /// <param name="pollInterval">The poll interval.</param>
        public RecurringWorkScheduler(TimeSpan pollInterval)
        {
            if (pollInterval <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("pollInterval");

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
        /// <param name="recurringWork"></param>
        public void SetAction(Action recurringWork)
        {
            this.recurringWork = recurringWork;
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
                this.recurringWork.Invoke();
                if (this.started)
                {
                    this.Start();
                }
            }
        }
    }
}
