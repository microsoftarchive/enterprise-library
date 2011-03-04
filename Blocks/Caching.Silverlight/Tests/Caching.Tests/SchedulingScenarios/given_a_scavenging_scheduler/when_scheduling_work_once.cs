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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.SchedulingScenarios.given_a_scavenging_scheduler
{
    [TestClass]
    public class when_scheduling_work_once : Context
    {
        private ManualResetEvent workHappenedEvent;

        protected override void Act()
        {
            base.Act();

            workHappenedEvent = new ManualResetEvent(false);
            Scheduler.SetAction(() => workHappenedEvent.Set());
            Scheduler.ScheduleWork();
        }

        [TestMethod]
        public void then_work_is_called()
        {
            Assert.IsTrue(workHappenedEvent.WaitOne(5000));   
        }
    }
}
