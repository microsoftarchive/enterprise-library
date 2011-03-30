using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling
{
    /// <summary>
    /// This interface represents a task that will be run at recurring intervals.
    /// </summary>
    public interface IRecurringScheduledWork
    {
        /// <summary>
        /// Set the delegate that will be run when the schedule
        /// determines it should run.
        /// </summary>
        /// <param name="workToDo"></param>
        void SetAction(Action workToDo);

        /// <summary>
        /// Start the scheduler running.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the scheduler.
        /// </summary>
        void Stop();
    }
}
