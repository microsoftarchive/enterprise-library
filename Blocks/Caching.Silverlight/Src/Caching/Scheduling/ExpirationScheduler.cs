using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling
{
    public class ExpirationScheduler : IRecurringScheduledWork
    {
        private Timer timer;
        private readonly TimeSpan pollInterval;
        private Action expirationAction;

        public ExpirationScheduler(TimeSpan pollInterval)
        {
            timer = new Timer(OnTimerIntervalElapsed);
            this.pollInterval = pollInterval;
        }

        public void Dispose()
        {

            if (timer != null)
            {
                Stop();
                timer.Dispose();
                timer = null;
            }
        }

        public void SetAction(Action workToDo)
        {
            expirationAction = workToDo;
        }

        public void Start()
        {
            timer.Change(pollInterval, TimeSpan.FromMilliseconds(-1));
        }

        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void OnTimerIntervalElapsed(object state)
        {
            expirationAction();
            Start();
        }
    }
}
