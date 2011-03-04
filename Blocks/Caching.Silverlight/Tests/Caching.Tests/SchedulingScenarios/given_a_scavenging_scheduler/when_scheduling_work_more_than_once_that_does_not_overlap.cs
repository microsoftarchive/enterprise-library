using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.SchedulingScenarios.given_a_scavenging_scheduler
{
    [TestClass]
    public class when_scheduling_work_more_than_once_that_does_not_overlap : Context
    {
        private AutoResetEvent workCompleted;
        private bool firstJobCompleted;
        private bool secondJobCompleted;

        protected override void Act()
        {
            base.Act();

            workCompleted = new AutoResetEvent(false);

            Scheduler.SetAction(() => workCompleted.Set());

            Scheduler.ScheduleWork();
            firstJobCompleted = workCompleted.WaitOne(5000);

            Scheduler.ScheduleWork();
            secondJobCompleted = workCompleted.WaitOne(5000);
        }

        [TestMethod]
        public void then_the_first_job_executed()
        {
            Assert.IsTrue(firstJobCompleted);
        }

        [TestMethod]
        public void then_the_second_job_executed()
        {
            Assert.IsTrue(secondJobCompleted);
        }
    }
}
